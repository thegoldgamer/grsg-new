using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

/*
=============================================================================
THROTTLE SETUP GUIDE (kinematic, no ConfigurableJoint)
=============================================================================
Attach this script to the throttle HANDLE GameObject (the grabbable part with
the E-Brake button). The handle needs:
  - A Collider (the grab target)
  - A Rigidbody (this script forces it kinematic so it never fights the ship)
  - This XRGrabInteractable-derived component

How it works:
  Instead of a physics joint, the handle is mathematically CONSTRAINED to the
  rail line (RailStart -> RailEnd). While grabbed, we project the grabbing
  hand onto that line and slide the handle there. Position along the rail maps
  to thrust (0 at start .. 1 at end). Gestures set the drive mode:
    - Twist the handle ~180 deg  -> REVERSE
    - Tilt/push the handle up     -> NEUTRAL (glide)
    - Push the handle down        -> PARK (locked)
  E-brake = B button (configured InputActionProperty) held while gripping.

Wire the references in the Inspector (RailStart, RailEnd, ThrottleHandle,
ThrottleScreen, EBrake button, ShipController).
=============================================================================
*/

[RequireComponent(typeof(Rigidbody))]
public class ThrottleInteractable : XRGrabInteractable
{
    [Header("Throttle Hierarchy")]
    [Tooltip("The handle transform that slides (usually this object). Defaults to this transform.")]
    public Transform throttleHandle;
    [Tooltip("The throttle's screen object (its Renderer's color shows thrust).")]
    public GameObject screenObject;
    [Tooltip("The physical E-brake button model (optional, cosmetic).")]
    public Transform eBrakeButton;

    [Header("Rail (slide axis)")]
    [Tooltip("World start of the rail = thrust 0 / fully back.")]
    public Transform railStartPoint;
    [Tooltip("World end of the rail = thrust 1 / full forward.")]
    public Transform railEndPoint;

    [Header("Ship Link")]
    public ShipController controller;

    [Header("Gesture Thresholds")]
    [Tooltip("Twist (deg) about the rail axis past which the throttle flips to REVERSE.")]
    public float reverseTwistAngle = 120f;
    [Tooltip("Upward tilt (deg) of the handle past which it enters NEUTRAL.")]
    public float neutralTiltAngle = 35f;
    [Tooltip("Downward tilt (deg) of the handle past which it enters PARK.")]
    public float parkTiltAngle = 35f;

    [Header("Haptics")]
    [Range(0f, 1f)] public float hapticIntensity = 0.5f;

    [Header("Screen Colors")]
    public Color minColor = Color.green;
    public Color maxColor = Color.red;
    public Color reverseColor = new Color(1f, 0.92f, 0.016f); // yellow

    private Rigidbody rb;
    private float railLength;
    private float normalizedPos = 0f;   // 0..1 along the rail
    private float previousNormalized = 0f;
    private DriveMode currentMode = DriveMode.PARK;
    private bool isEBraking = false;

    // The handle's local pose relative to the rail, captured at startup, so we can
    // detect twist/tilt gestures relative to the rest pose.
    private Quaternion restLocalRotation;

    [Header("Input Actions")]
    [Tooltip("Press/hold to engage the E-brake while gripping (Quest B button).")]
    public InputActionProperty eBrakeAction;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;            // never fights the ship Rigidbody
        rb.useGravity = false;

        // We move the handle ourselves; don't let XRI tracking yank the whole object.
        trackPosition = false;
        trackRotation = false;
        throwOnDetach = false;

        if (throttleHandle == null) throttleHandle = transform;

        if (railStartPoint != null && railEndPoint != null)
            railLength = Vector3.Distance(railStartPoint.position, railEndPoint.position);
        else
            Debug.LogWarning("ThrottleInteractable: Rail Start/End not assigned.");

        restLocalRotation = throttleHandle.localRotation;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (eBrakeAction.action != null) eBrakeAction.action.Enable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (eBrakeAction.action != null) eBrakeAction.action.Disable();
        // Safety: never leave the e-brake stuck on.
        if (isEBraking && controller != null) controller.EBrake(false);
        isEBraking = false;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
                FollowHandToRail();

            ProcessThrottleLogic();
            ProcessEBrake();
        }
    }

    /// <summary>
    /// While grabbed, slide the handle along the rail by projecting the grabbing
    /// hand's position onto the rail line. Keeps the handle locked to the rail.
    /// </summary>
    private void FollowHandToRail()
    {
        if (railStartPoint == null || railEndPoint == null || railLength <= 0f) return;
        if (interactorsSelecting.Count == 0) return;

        // Grabbing hand world position.
        Transform hand = interactorsSelecting[0].transform;

        Vector3 railVec = railEndPoint.position - railStartPoint.position;
        Vector3 railDir = railVec.normalized;
        Vector3 handOffset = hand.position - railStartPoint.position;

        float projection = Vector3.Dot(handOffset, railDir);
        normalizedPos = Mathf.Clamp01(projection / railLength);

        // Snap the handle to the projected point on the rail.
        Vector3 target = railStartPoint.position + railDir * (normalizedPos * railLength);
        throttleHandle.position = target;

        // Let the handle visually face along the rail while picking up the hand's twist/tilt.
        throttleHandle.rotation = hand.rotation;
    }

    private void ProcessThrottleLogic()
    {
        if (controller == null) return;

        // --- Detect gesture-based drive mode from the handle's rotation vs rest pose ---
        DriveMode newMode = DetermineMode();

        if (newMode != currentMode)
        {
            currentMode = newMode;
            controller.SetDriveMode(currentMode);
            TriggerHaptic(hapticIntensity, 0.08f);
        }

        // --- Feed thrust based on rail position (only meaningful when driving) ---
        if (currentMode == DriveMode.DRIVE || currentMode == DriveMode.REVERSE)
        {
            controller.SetThrust(normalizedPos);

            // Gradual brake if pulled all the way back to 0 while in DRIVE.
            if (currentMode == DriveMode.DRIVE && normalizedPos <= 0.02f && previousNormalized > 0.02f)
                controller.Brake();
        }

        UpdateScreen();

        // Aggressiveness/feel: haptic pulse proportional to how fast the player moves it.
        float delta = Mathf.Abs(normalizedPos - previousNormalized);
        if (delta > 0.01f && isSelected)
            TriggerHaptic(Mathf.Clamp01(hapticIntensity * delta * 10f), 0.05f);

        previousNormalized = normalizedPos;
    }

    /// <summary>
    /// Maps the handle's current rotation (relative to rest) to a drive mode.
    /// Twist about rail axis -> REVERSE; tilt up -> NEUTRAL; tilt down -> PARK;
    /// otherwise DRIVE.
    /// </summary>
    private DriveMode DetermineMode()
    {
        if (!isSelected)
            return currentMode; // hold last mode when released

        // Rotation of the handle relative to its rest pose, expressed locally.
        Quaternion delta = Quaternion.Inverse(restLocalRotation) * throttleHandle.localRotation;
        Vector3 euler = delta.eulerAngles;

        float twist = Normalize180(euler.z);  // roll about rail axis
        float tilt = Normalize180(euler.x);   // pitch up/down

        if (Mathf.Abs(twist) >= reverseTwistAngle)
            return DriveMode.REVERSE;
        if (tilt >= neutralTiltAngle)
            return DriveMode.NEUTRAL;
        if (tilt <= -parkTiltAngle)
            return DriveMode.PARK;

        return DriveMode.DRIVE;
    }

    private static float Normalize180(float angle)
    {
        return angle > 180f ? angle - 360f : angle;
    }

    private void UpdateScreen()
    {
        if (screenObject == null) return;
        Renderer r = screenObject.GetComponent<Renderer>();
        if (r == null) return;

        Color c;
        if (currentMode == DriveMode.REVERSE)
            c = reverseColor;
        else if (currentMode == DriveMode.PARK || currentMode == DriveMode.NEUTRAL)
            c = Color.Lerp(minColor, maxColor, 0f); // idle = min
        else
            c = Color.Lerp(minColor, maxColor, normalizedPos);

        r.material.color = c;
        r.material.SetColor("_EmissionColor", c * 1.5f);
        r.material.EnableKeyword("_EMISSION");
    }

    private void ProcessEBrake()
    {
        if (controller == null) return;

        bool gripping = interactorsSelecting.Count > 0;
        bool pressed = gripping && eBrakeAction.action != null && eBrakeAction.action.IsPressed();

        if (pressed && !isEBraking)
        {
            isEBraking = true;
            controller.EBrake(true);
        }
        else if (!pressed && isEBraking)
        {
            isEBraking = false;
            controller.EBrake(false);
        }
    }

    private void TriggerHaptic(float amplitude, float duration)
    {
        if (interactorsSelecting.Count == 0) return;
        var interactor = interactorsSelecting[0] as XRBaseControllerInteractor;
        if (interactor != null && interactor.xrController != null)
            interactor.xrController.SendHapticImpulse(amplitude, duration);
    }
}
