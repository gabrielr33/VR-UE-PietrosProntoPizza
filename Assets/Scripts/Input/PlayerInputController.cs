using UnityEngine;

namespace Input
{
    public class PlayerInputController : MonoBehaviour
    {
        public InputTriggerGrip InputTriggerGrip;
        public InputVelocities InputVelocities;
        public InputButtons InputButtons;
        public InputJoysticks InputJoysticks;

        [SerializeField] private Animator _rightHandAnimator;
        [SerializeField] private Animator _leftHandAnimator;

        private VRController_InputActions _inputActions;
        private static readonly int Trigger = Animator.StringToHash("Trigger");

        private void OnEnable()
        {
            InputTriggerGrip = new InputTriggerGrip();
            InputVelocities = new InputVelocities();
            InputButtons = new InputButtons();
            InputJoysticks = new InputJoysticks();
            
            if (_inputActions == null)
            {
                _inputActions = new VRController_InputActions();

                // Hand Trigger
                _inputActions.VRControllers.Trigger_Right.performed +=
                    i => InputTriggerGrip.RightControllerTriggerInput = i.ReadValue<float>();
                _inputActions.VRControllers.Trigger_Left.performed +=
                    i => InputTriggerGrip.LeftControllerTriggerInput = i.ReadValue<float>();

                // Hand Grip
                _inputActions.VRControllers.Grip_Right.performed +=
                    i => InputTriggerGrip.RightControllerGripInput = i.ReadValue<float>();
                _inputActions.VRControllers.Grip_Left.performed +=
                    i => InputTriggerGrip.LeftControllerGripInput = i.ReadValue<float>();

                // Hand Velocities
                _inputActions.VRControllers.ControllerVelocities_Right.performed +=
                    i => InputVelocities.RightControllerVelocity = i.ReadValue<Vector3>();
                _inputActions.VRControllers.ControllerVelocities_Left.performed +=
                    i => InputVelocities.LeftControllerVelocity = i.ReadValue<Vector3>();
                _inputActions.VRControllers.ControllerAngularVelocities_Right.performed +=
                    i => InputVelocities.RightControllerAngularVelocity = i.ReadValue<Vector3>();
                _inputActions.VRControllers.ControllerAngularVelocities_Left.performed +=
                    i => InputVelocities.LeftControllerAngularVelocity = i.ReadValue<Vector3>();

                // Buttons
                _inputActions.VRControllers.ButtonA.performed +=
                    i => InputButtons.PrimaryButtonA = i.ReadValue<float>();
                _inputActions.VRControllers.ButtonB.performed +=
                    i => InputButtons.SecondaryButtonB = i.ReadValue<float>();
                _inputActions.VRControllers.ButtonX.performed +=
                    i => InputButtons.PrimaryButtonX = i.ReadValue<float>();
                _inputActions.VRControllers.ButtonY.performed +=
                    i => InputButtons.SecondaryButtonY = i.ReadValue<float>();
                
                // Thumbstick
                _inputActions.VRControllers.JoystickRight.performed +=
                    i => InputJoysticks.RightControllerJoystick = i.ReadValue<Vector2>();
            }

            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void Update()
        {
            HandleHandAnimations();
        }

        private void HandleHandAnimations()
        {
            _rightHandAnimator.SetFloat(Trigger, InputTriggerGrip.RightControllerTriggerInput);
            _leftHandAnimator.SetFloat(Trigger, InputTriggerGrip.LeftControllerTriggerInput);
        }
    }

    public class InputVelocities
    {
        public Vector3 RightControllerVelocity;
        public Vector3 LeftControllerVelocity;
        public Vector3 RightControllerAngularVelocity;
        public Vector3 LeftControllerAngularVelocity;
    }

    public class InputButtons
    {
        public float PrimaryButtonA;
        public float SecondaryButtonB;
        public float PrimaryButtonX;
        public float SecondaryButtonY;
    }

    public class InputTriggerGrip
    {
        public float RightControllerTriggerInput;
        public float LeftControllerTriggerInput;
        public float RightControllerGripInput;
        public float LeftControllerGripInput;
    }
    
    public class InputJoysticks
    {
        public Vector2 RightControllerJoystick;
        public Vector2 LeftControllerJoystick;
    }
}
