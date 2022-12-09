using UnityEngine;

namespace Input
{
    public class PlayerInputController : MonoBehaviour
    {
        public InputTrigger InputTrigger;
        public InputJoystick InputJoystick;
        public InputVelocities InputVelocities;
        public InputButtons InputButtons;

        private VRController_InputActions _inputActions;

        private void OnEnable()
        {
            InputTrigger = new InputTrigger();
            InputJoystick = new InputJoystick();
            InputVelocities = new InputVelocities();
            InputButtons = new InputButtons();
            
            if (_inputActions == null)
            {
                _inputActions = new VRController_InputActions();

                // Hand Trigger
                _inputActions.VRControllers.Trigger_Right.performed +=
                    i => InputTrigger.RightTriggerInput = i.ReadValue<float>();
                _inputActions.VRControllers.Trigger_Left.performed +=
                    i => InputTrigger.LeftTriggerInput = i.ReadValue<float>();

                // Joystick/Trackpad
                _inputActions.VRControllers.Joystick_Left.performed +=
                    i => InputJoystick.LeftJoystickInput = i.ReadValue<Vector2>();

                _inputActions.VRControllerHTC.Joystick_Left.performed +=
                    i => InputJoystick.LeftJoystickInputHTC = i.ReadValue<Vector2>();

                _inputActions.VRControllerHTC.TrackpadTouched_Left.performed +=
                    i => InputJoystick.LeftTrackpadTouched = i.ReadValue<float>();

                // Hand Velocities
                _inputActions.VRControllers.AngularVelocities_Right.performed +=
                    i => InputVelocities.RightControllerVelocity = i.ReadValue<Vector3>();
                _inputActions.VRControllers.Velocities_Left.performed +=
                    i => InputVelocities.LeftControllerVelocity = i.ReadValue<Vector3>();
                _inputActions.VRControllers.AngularVelocities_Right.performed +=
                    i => InputVelocities.RightControllerAngularVelocity = i.ReadValue<Vector3>();
                _inputActions.VRControllers.AngularVelocities_Left.performed +=
                    i => InputVelocities.LeftControllerAngularVelocity = i.ReadValue<Vector3>();

                // Buttons
                _inputActions.VRControllers.ButtonPrimary_Right.performed +=
                    i => InputButtons.ButtonPrimary_Right = i.ReadValue<float>();
            }

            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }
    }

    public class InputTrigger
    {
        public float RightTriggerInput;
        public float LeftTriggerInput;
    }
    
    public class InputJoystick
    {
        public Vector2 LeftJoystickInput;
        public Vector2 LeftJoystickInputHTC;
        public float LeftTrackpadTouched;
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
        public float ButtonPrimary_Right;
    }
}
