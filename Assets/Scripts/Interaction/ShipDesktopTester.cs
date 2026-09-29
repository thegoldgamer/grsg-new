using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ShipDesktopTester.cs — placed on the root ship GameObject for editor/testing only.
/// Lets you fly the ship from the keyboard without a headset by calling the SAME
/// ShipController API the throttle/yoke use. Disable or remove for real builds.
///
/// Controls:
///   W / S        throttle up / down (ramps thrust 0..1)
///   A / D        roll left / right
///   Up / Down    pitch
///   Left / Right yaw
///   Q            NEUTRAL (glide)
///   E            PARK (locked)
///   R            toggle REVERSE / DRIVE
///   Space        E-brake (hold)
///   Z / X        vertical down / up (hangar lift)
/// </summary>
public class ShipDesktopTester : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("The ShipController to drive. Auto-found on this object if empty.")]
    public ShipController controller;
    [Tooltip("Master switch — turn off to disable keyboard testing.")]
    public bool enableKeyboardTesting = true;

    [Header("Tuning")]
    [Tooltip("How fast W/S ramps the throttle (units per second).")]
    public float throttleRampSpeed = 0.75f;

    private float throttle = 0f;          // 0..1 magnitude
    private bool reversing = false;
    private bool ebrakeHeld = false;

    private void Awake()
    {
        if (controller == null)
            controller = GetComponent<ShipController>();
    }

    private void Update()
    {
        if (!enableKeyboardTesting || controller == null) return;

        Keyboard kb = Keyboard.current;
        if (kb == null) return; // no keyboard present (real VR build)

        // --- Drive mode toggles ---
        if (kb.qKey.wasPressedThisFrame)
        {
            controller.SetNeutral();
            throttle = 0f;
        }
        if (kb.eKey.wasPressedThisFrame)
        {
            controller.SetPark();
            throttle = 0f;
        }
        if (kb.rKey.wasPressedThisFrame)
        {
            reversing = !reversing;
            controller.SetDriveMode(reversing ? DriveMode.REVERSE : DriveMode.DRIVE);
        }

        // --- Throttle ramp (W/S) ---
        // Pressing W/S implies we want to be in a driving mode (not parked/neutral).
        float ramp = 0f;
        if (kb.wKey.isPressed) ramp += 1f;
        if (kb.sKey.isPressed) ramp -= 1f;

        if (ramp != 0f &&
            controller.CurrentDriveMode != DriveMode.DRIVE &&
            controller.CurrentDriveMode != DriveMode.REVERSE)
        {
            controller.SetDriveMode(reversing ? DriveMode.REVERSE : DriveMode.DRIVE);
        }

        throttle = Mathf.Clamp01(throttle + ramp * throttleRampSpeed * Time.deltaTime);
        controller.SetThrust(throttle);

        // Gradual brake when throttle hits 0 in DRIVE.
        if (throttle <= 0.001f && controller.CurrentDriveMode == DriveMode.DRIVE)
            controller.Brake();

        // --- Rotation ---
        float roll = (kb.dKey.isPressed ? 1f : 0f) - (kb.aKey.isPressed ? 1f : 0f);
        float pitch = (kb.upArrowKey.isPressed ? 1f : 0f) - (kb.downArrowKey.isPressed ? 1f : 0f);
        float yaw = (kb.rightArrowKey.isPressed ? 1f : 0f) - (kb.leftArrowKey.isPressed ? 1f : 0f);
        controller.SetRoll(roll);
        controller.SetPitch(pitch);
        controller.SetYaw(yaw);

        // --- Vertical lift (Z/X) ---
        float vert = (kb.xKey.isPressed ? 1f : 0f) - (kb.zKey.isPressed ? 1f : 0f);
        controller.SetVertical(vert);

        // --- E-brake (Space, hold) ---
        bool space = kb.spaceKey.isPressed;
        if (space && !ebrakeHeld) { ebrakeHeld = true; controller.EBrake(true); }
        else if (!space && ebrakeHeld) { ebrakeHeld = false; controller.EBrake(false); }
    }
}
