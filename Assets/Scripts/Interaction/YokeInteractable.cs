using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

/*
=============================================================================
YOKE SETUP GUIDE (kinematic, no ConfigurableJoint)
=============================================================================
Attach this script to the grabbable yoke GameObject. It needs:
  - A Collider (grab target)
  - A Rigidbody (forced kinematic so it never fights the ship)
  - This XRGrabInteractable-derived component (select mode = Multiple for 2 hands)

How it works (no physics joint):
  The yoke rotates about its base pivot. While grabbed, we derive a target
  PITCH (push forward = nose down / pull back = nose up) and ROLL (car-style
  twist) from the grabbing hand(s), clamp them to limits, and:
    1. rotate the visual decor pivots (YolkForwardBackwardPivot for pitch,
       ScreenLeftRightRotatePivot for roll) so the model articulates cleanly,
    2. feed normalized pitch/roll to the ShipController.
  Thumbsticks add yaw (right stick, < 50 kn) and vertical lift (left stick,
  < 100 kn). On release the yoke dampens back toward center (no hard snap).

Wire the decor pivots + ShipController + input actions in the Inspector.
=============================================================================
*/

[RequireComponent(typeof(Rigidbody))]
public class YokeInteractable : XRGrabInteractable
{
    [Header("Yoke Visual Pivots")]
    [Tooltip("Pivot rotated for pitch (push/pull). e.g. YolkForwardBackwardPivot")]
    public Transform pitchPivot;
    [Tooltip("Pivot rotated for roll (twist). e.g. ScreenLeftRightRotatePivot")]
    public Transform rollPivot;

    [Header("Ship Link")]
    public ShipController controller;

    [Header("Input Actions")]
    [Tooltip("Right thumbstick — X used for yaw (rudder).")]
    public InputActionProperty yawAction;
    [Tooltip("Left thumbstick — Y used for vertical lift (hangar).")]
    public InputActionProperty verticalAction;
    [Tooltip("Left trigger — toggles yoke-screen UI navigation mode.")]
    public InputActionProperty toggleUIAction;

    [Header("Tuning")]
    public float maxPitchAngle = 45f;
    public float maxRollAngle = 90f;
    [Tooltip("How fast pitch/roll follow the hand while grabbed.")]
    public float followSpeed = 10f;
    [Tooltip("How fast the yoke dampens back to center when released.")]
    public float returnToCenterSpeed = 3f;

    [Header("Speed Gates (knots)")]
    public float yawMaxSpeed = 50f;
    public float verticalMaxSpeed = 100f;

    private Rigidbody rb;
    private bool uiModeActive = false;

    // Current applied angles (smoothed).
    private float pitchAngle = 0f;
    private float rollAngle = 0f;

    // Reference pose captured on grab, to measure hand deltas against.
    private Quaternion grabStartHandRot;
    private bool hasGrabRef = false;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        // We articulate decor pivots ourselves; don't let XRI move the base object.
        trackPosition = false;
        trackRotation = false;
        throwOnDetach = false;

        selectMode = InteractableSelectMode.Multiple; // one or two hands
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        yawAction.action?.Enable();
        verticalAction.action?.Enable();
        toggleUIAction.action?.Enable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        yawAction.action?.Disable();
        verticalAction.action?.Disable();
        toggleUIAction.action?.Disable();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (interactorsSelecting.Count > 0)
        {
            grabStartHandRot = interactorsSelecting[0].transform.rotation;
            hasGrabRef = true;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        if (interactorsSelecting.Count == 0)
            hasGrabRef = false;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            ProcessYoke();
    }

    private void ProcessYoke()
    {
        if (controller == null) return;

        if (isSelected && hasGrabRef)
            DriveFromHand();
        else
            DampenToCenter();

        ApplyVisualPivots();

        controller.SetPitch(Mathf.Clamp(pitchAngle / maxPitchAngle, -1f, 1f));
        controller.SetRoll(Mathf.Clamp(rollAngle / maxRollAngle, -1f, 1f));

        ProcessThumbsticks();
    }

    private void DriveFromHand()
    {
        // Delta rotation of the hand since grab, expressed in the yoke base's local space.
        Transform hand = interactorsSelecting[0].transform;
        Quaternion handDelta = hand.rotation * Quaternion.Inverse(grabStartHandRot);

        // Convert to local euler relative to this object's parent.
        Quaternion localDelta = Quaternion.Inverse(transform.rotation) * handDelta * transform.rotation;
        Vector3 e = localDelta.eulerAngles;

        float targetPitch = Mathf.Clamp(Normalize180(e.x), -maxPitchAngle, maxPitchAngle);
        float targetRoll = Mathf.Clamp(Normalize180(e.z), -maxRollAngle, maxRollAngle);

        pitchAngle = Mathf.Lerp(pitchAngle, targetPitch, followSpeed * Time.deltaTime);
        rollAngle = Mathf.Lerp(rollAngle, targetRoll, followSpeed * Time.deltaTime);
    }

    private void DampenToCenter()
    {
        pitchAngle = Mathf.Lerp(pitchAngle, 0f, returnToCenterSpeed * Time.deltaTime);
        rollAngle = Mathf.Lerp(rollAngle, 0f, returnToCenterSpeed * Time.deltaTime);
    }

    private void ApplyVisualPivots()
    {
        if (pitchPivot != null)
            pitchPivot.localRotation = Quaternion.Euler(pitchAngle, 0f, 0f);
        if (rollPivot != null)
            rollPivot.localRotation = Quaternion.Euler(0f, 0f, rollAngle);
    }

    private void ProcessThumbsticks()
    {
        if (!isSelected)
        {
            controller.SetYaw(0f);
            controller.SetVertical(0f);
            return;
        }

        // Toggle UI mode with the left trigger when held with both hands.
        if (toggleUIAction.action != null &&
            toggleUIAction.action.WasPressedThisFrame() &&
            interactorsSelecting.Count >= 2)
        {
            uiModeActive = !uiModeActive;
        }

        if (uiModeActive)
        {
            // Sticks drive the screen UI instead of the ship.
            controller.SetYaw(0f);
            controller.SetVertical(0f);
            return;
        }

        float speed = controller.CurrentSpeedKnots;

        // Yaw — right stick X, only below the yaw speed gate.
        float yaw = yawAction.action != null ? yawAction.action.ReadValue<Vector2>().x : 0f;
        controller.SetYaw(speed < yawMaxSpeed ? yaw : 0f);

        // Vertical — left stick Y, only below the vertical speed gate.
        float vert = verticalAction.action != null ? verticalAction.action.ReadValue<Vector2>().y : 0f;
        controller.SetVertical(speed < verticalMaxSpeed ? vert : 0f);
    }

    private static float Normalize180(float angle)
    {
        return angle > 180f ? angle - 360f : angle;
    }
}
