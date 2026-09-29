using UnityEngine;

/// <summary>
/// Drive modes for the ship's transmission.
/// </summary>
public enum DriveMode
{
    DRIVE,
    REVERSE,
    NEUTRAL,
    PARK
}

/// <summary>
/// ShipController.cs — the pure physics layer on the root ship GameObject.
/// Holds the Rigidbody and translates control inputs (from the throttle/yoke or the
/// desktop tester) into 6DOF force and torque. Contains NO game logic beyond moving
/// the body. Engine.cs decides how much force the thrust request actually produces.
///
/// Spawn safety: the ship starts PARKED with zero velocity and applies directional
/// force ONLY on real input, so it can never fly off on its own at spawn.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ShipController : MonoBehaviour
{
    [Header("Dependencies")]
    [Tooltip("The engine that converts thrust requests into force. Auto-found in children if empty.")]
    public Engine engine;

    [Header("State (Read Only)")]
    [SerializeField] private DriveMode currentDriveMode = DriveMode.PARK;
    [SerializeField] private float thrustInput = 0f; // -1..1 (sign only matters via mode)
    [SerializeField] private float pitchInput = 0f;  // -1..1
    [SerializeField] private float yawInput = 0f;    // -1..1
    [SerializeField] private float rollInput = 0f;   // -1..1

    [Header("Rotation Tuning")]
    public float pitchTorque = 50f;
    public float yawTorque = 30f;
    public float rollTorque = 40f;

    [Header("Vertical (hangar lift) Tuning")]
    [Tooltip("Upward force applied from the yoke's left stick at low speed.")]
    public float verticalForce = 2000f;
    private float verticalInput = 0f; // -1..1

    [Header("Braking")]
    [Tooltip("Rigidbody drag applied while gradually braking (throttle pulled to 0 in DRIVE).")]
    public float brakeDrag = 1.5f;
    [Tooltip("Rigidbody drag applied while the E-brake is held (rapid stop).")]
    public float eBrakeDrag = 10f;

    [Header("Idle 'Alive' Drift (PARK + engine ON)")]
    [Tooltip("Tiny positional sway force while parked with the engine running. 0 = perfectly still.")]
    public float idleSwayAmount = 4f;
    [Tooltip("Tiny rotational drift torque while parked with the engine running. 0 = perfectly still.")]
    public float idleRotAmount = 1.5f;
    [Tooltip("How fast the idle wobble oscillates.")]
    public float idleFrequency = 0.5f;

    [Header("Speed Readout")]
    [Tooltip("World units per second -> knots conversion (tweak to taste). Used to gate yaw/vertical.")]
    public float knotsPerUnit = 1.94f;

    private Rigidbody rb;
    private float defaultDrag;
    private float defaultAngularDrag;
    private bool isEBraking = false;
    private float idleSeed;

    /// <summary>Approximate forward speed in "knots" for control gating (yaw &lt;50, vertical &lt;100).</summary>
    public float CurrentSpeedKnots => rb != null ? rb.linearVelocity.magnitude * knotsPerUnit : 0f;

    public DriveMode CurrentDriveMode => currentDriveMode;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        defaultDrag = rb.linearDamping;
        defaultAngularDrag = rb.angularDamping;

        // Artificial gravity governs this ship, not Unity gravity.
        rb.useGravity = false;

        if (engine == null)
            engine = GetComponentInChildren<Engine>();

        // Unique per-ship offset so multiple ships don't wobble in sync.
        idleSeed = (transform.position.x + transform.position.z) * 13.37f;
    }

    private void Start()
    {
        // Guaranteed inert spawn.
        currentDriveMode = DriveMode.PARK;
        thrustInput = pitchInput = yawInput = rollInput = verticalInput = 0f;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        // PARK: no navigation. Only the subtle "alive" idle drift, and only if engine is running.
        if (currentDriveMode == DriveMode.PARK)
        {
            ApplyIdleDrift();
            return;
        }

        ApplyThrust();
        ApplyRotation();
        ApplyVertical();
    }

    private void ApplyThrust()
    {
        if (engine == null || isEBraking) return;

        // Only DRIVE/REVERSE produce force; NEUTRAL glides.
        if (currentDriveMode == DriveMode.NEUTRAL) return;

        float requested = Mathf.Abs(thrustInput);
        if (requested <= 0.0001f) return; // no input -> no uncommanded propulsion

        float actualThrust = engine.ProcessThrust(requested, currentDriveMode);
        // Engine returns a signed value (negative in reverse); apply along forward.
        rb.AddRelativeForce(Vector3.forward * actualThrust, ForceMode.Force);
    }

    private void ApplyRotation()
    {
        Vector3 torque = new Vector3(
            pitchInput * pitchTorque,
            yawInput * yawTorque,
            -rollInput * rollTorque   // negative so positive roll input rolls right
        );
        rb.AddRelativeTorque(torque, ForceMode.Force);
    }

    private void ApplyVertical()
    {
        if (Mathf.Abs(verticalInput) <= 0.0001f) return;
        rb.AddForce(transform.up * (verticalInput * verticalForce), ForceMode.Force);
    }

    private void ApplyIdleDrift()
    {
        // Only "breathe" when the engine is actually running. Engine OFF = rock still.
        if (engine == null || !engine.IsRunning) return;
        if (idleSwayAmount <= 0f && idleRotAmount <= 0f) return;

        float t = Time.time * idleFrequency;

        Vector3 sway = new Vector3(
            Mathf.Sin(t + idleSeed),
            Mathf.Sin(t * 0.7f + idleSeed * 1.3f),
            Mathf.Cos(t * 0.9f + idleSeed * 0.6f)
        ) * idleSwayAmount;
        rb.AddForce(sway, ForceMode.Force);

        Vector3 wobble = new Vector3(
            Mathf.Sin(t * 0.6f + idleSeed * 0.4f),
            Mathf.Cos(t * 0.5f + idleSeed),
            Mathf.Sin(t * 0.8f + idleSeed * 0.9f)
        ) * idleRotAmount;
        rb.AddRelativeTorque(wobble, ForceMode.Force);
    }

    #region Public Control API (called by throttle, yoke, desktop tester)

    /// <summary>Sets normalized thrust (-1..1). Sign is informational; mode decides direction.</summary>
    public void SetThrust(float value) => thrustInput = Mathf.Clamp(value, -1f, 1f);

    public void SetPitch(float value) => pitchInput = Mathf.Clamp(value, -1f, 1f);
    public void SetYaw(float value)   => yawInput   = Mathf.Clamp(value, -1f, 1f);
    public void SetRoll(float value)  => rollInput  = Mathf.Clamp(value, -1f, 1f);
    public void SetVertical(float value) => verticalInput = Mathf.Clamp(value, -1f, 1f);

    public void SetDriveMode(DriveMode mode)
    {
        currentDriveMode = mode;

        if (mode == DriveMode.PARK)
        {
            // Lock down: kill input and motion. Idle drift (if engine on) re-adds a whisper of life.
            thrustInput = 0f;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.linearDamping = defaultDrag;
            rb.angularDamping = defaultAngularDrag;
        }
        else if (mode == DriveMode.NEUTRAL)
        {
            // Glide: no force, no braking, normal drag.
            thrustInput = 0f;
            rb.linearDamping = defaultDrag;
            rb.angularDamping = defaultAngularDrag;
        }
    }

    /// <summary>Gradual stop (throttle pulled fully back to 0 while in DRIVE).</summary>
    public void Brake()
    {
        thrustInput = 0f;
        if (!isEBraking)
            rb.linearDamping = brakeDrag;
    }

    public void SetNeutral() => SetDriveMode(DriveMode.NEUTRAL);
    public void SetPark()    => SetDriveMode(DriveMode.PARK);

    /// <summary>Emergency brake. Hold to sustain: rapid stop + engine damage.</summary>
    public void EBrake(bool active)
    {
        isEBraking = active;

        if (active)
        {
            rb.linearDamping = eBrakeDrag;
            rb.angularDamping = eBrakeDrag;
            if (engine != null)
                engine.TriggerEBrakeDamage();
        }
        else
        {
            rb.linearDamping = defaultDrag;
            rb.angularDamping = defaultAngularDrag;
        }
    }

    #endregion
}
