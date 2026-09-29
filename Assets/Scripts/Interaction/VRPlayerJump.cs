using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(CharacterController))]
public class VRPlayerJump : MonoBehaviour
{
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    [Header("Input Options")]
    [Tooltip("Method A: Bind an InputActionProperty directly (e.g. XRI RightHand Interaction/Primary Button)")]
    public InputActionProperty jumpAction;

    [Tooltip("Method B: Assign the XRBaseController (e.g., Right Hand Controller) to check its state")]
    public XRBaseController rightHandController;

    // We will check button A via Unity's internal feature system if Controller isn't using InputSystem
    public UnityEngine.XR.InputFeatureUsage<bool> jumpFeature = UnityEngine.XR.CommonUsages.primaryButton;

    private CharacterController cc;
    private float verticalVelocity;
    private bool isJumping = false;
    private bool previousButtonState = false;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        if (jumpAction.action != null)
        {
            jumpAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (jumpAction.action != null)
        {
            jumpAction.action.Disable();
        }
    }

    private void Update()
    {
        if (cc == null || !cc.enabled) return;

        bool isGrounded = cc.isGrounded || Physics.Raycast(transform.position, Vector3.down, 0.2f);

        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // Keep sticking to ground
            if (isJumping) isJumping = false;
        }

        // Method A: Check InputAction
        if (jumpAction.action != null && jumpAction.action.WasPressedThisFrame())
        {
            PerformJump();
        }

        // Method B: Check XR Controller Device Directly
        if (rightHandController != null && rightHandController.currentControllerState != null)
        {
            // Unity XR input abstraction
            var device = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.RightHand);
            if (device.isValid)
            {
                bool buttonPressed;
                if (device.TryGetFeatureValue(jumpFeature, out buttonPressed))
                {
                    if (buttonPressed && !previousButtonState)
                    {
                        PerformJump();
                    }
                    previousButtonState = buttonPressed;
                }
            }
        }

        // Method C: Also allow keyboard space for testing in editor
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            PerformJump();
        }

        if (isJumping || !isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        if (verticalVelocity != 0f)
        {
            cc.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
        }
    }

    public void PerformJump()
    {
        if (cc != null && cc.enabled)
        {
            // Ignore isGrounded for safety to ensure they can jump
            bool isGrounded = cc.isGrounded || Physics.Raycast(transform.position, Vector3.down, 0.5f);

            if (isGrounded || !isJumping)
            {
                verticalVelocity = Mathf.Sqrt(jumpForce * -3.0f * gravity);
                isJumping = true;
                Debug.Log("Player initiated jump!");
            }
            else
            {
                Debug.Log("Jump blocked: Not grounded and already jumping.");
            }
        }
        else
        {
            Debug.Log("Jump blocked: CharacterController missing or disabled.");
        }
    }
}
