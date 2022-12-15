using System;
using UnityEngine;

namespace Input
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private GameObject _XRRig;
        [SerializeField] private Transform _camera;
        [SerializeField] private Transform _forwardLook;

        //[SerializeField] private bool _moveUsingKeyboard;
        [SerializeField] private float _movementSpeed = 1.5f;
        [SerializeField] private float _rotateSpeed = 50.0f;

        private PlayerInputController _inputController;

        private void Awake()
        {
            _inputController = GetComponent<PlayerInputController>();
        }

        private void Update()
        {
            /*if (_moveUsingKeyboard)
                
            else
                HandleMovementWithController();*/

            HandleMovementWithKeyboard();
            HandleMovementWithController();
        }

        private void HandleMovementWithKeyboard()
        {
            _forwardLook.eulerAngles = new Vector3(0f, _camera.eulerAngles.y, 0f);

            //move forward
            if (UnityEngine.Input.GetKey(KeyCode.W))
            {
                _XRRig.transform.position += _forwardLook.forward * (_movementSpeed * Time.deltaTime);
            }

            //move left
            if (UnityEngine.Input.GetKey(KeyCode.A))
            {
                _XRRig.transform.position -= _XRRig.transform.right * (_movementSpeed * Time.deltaTime);
            }

            //move backwards
            if (UnityEngine.Input.GetKey(KeyCode.S))
            {
                _XRRig.transform.position -= _forwardLook.forward * (_movementSpeed * Time.deltaTime);
            }

            //move right
            if (UnityEngine.Input.GetKey(KeyCode.D))
            {
                _XRRig.transform.position += _XRRig.transform.right * (_movementSpeed * Time.deltaTime);
            }
        }

        private void HandleMovementWithController()
        {
            _forwardLook.eulerAngles = new Vector3(0f, _camera.eulerAngles.y, 0f);

            Vector2 inputLeft = _inputController.InputJoystick.LeftJoystickInput;
            Vector2 inputLeftHTC = _inputController.InputJoystick.LeftJoystickInputHTC;
            float trackpadTouchedLeft = _inputController.InputJoystick.LeftTrackpadTouched;

            Vector2 inputRight = _inputController.InputJoystick.RightJoystickInput;
            Vector2 inputRightHTC = _inputController.InputJoystick.RightJoystickInputHTC;
            float trackpadTouchedRight = _inputController.InputJoystick.RightTrackpadTouched;

            //move forward and backwards
            if (Math.Abs(inputLeft.y) >= 0.5f ^ (Math.Abs(inputLeftHTC.y) >= 0.5f && trackpadTouchedLeft > 0.5f))
            {
                _XRRig.transform.position += _forwardLook.forward * ((inputLeft.y + inputLeftHTC.y) * _movementSpeed * Time.deltaTime);
            }
            
            //move left and right
            if (Math.Abs(inputLeft.x) >= 0.5f ^ (Math.Abs(inputLeftHTC.x) >= 0.5f && trackpadTouchedLeft > 0.5f))
            {
                _XRRig.transform.position += _forwardLook.right * ((inputLeft.x + inputLeftHTC.x) * _movementSpeed * Time.deltaTime);
            }

            //turn right and left
            if (Math.Abs(inputRight.x) >= 0.5f ^ (Math.Abs(inputRightHTC.x) >= 0.5f && trackpadTouchedRight > 0.5f))
            {
                _XRRig.transform.Rotate(0, inputRightHTC.x * _rotateSpeed * Time.deltaTime, 0);
            }
        }
    }
}