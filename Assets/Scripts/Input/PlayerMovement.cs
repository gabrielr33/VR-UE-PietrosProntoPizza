using System;
using UnityEngine;

namespace Input
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private GameObject _XRRig;
        [SerializeField] private Transform _camera;
        [SerializeField] private Transform _forwardLook;

        [SerializeField] private bool _moveUsingKeyboard;
        [SerializeField] private float _movementSpeed = 1.0f;

        private PlayerInputController _inputController;

        private void Awake()
        {
            _inputController = GetComponent<PlayerInputController>();
        }

        private void Update()
        {
            if (_moveUsingKeyboard)
                HandleMovementWithKeyboard();
            else
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

            Vector2 input = _inputController.InputJoystick.LeftJoystickInput;

            //move forward and backwards
            if (Math.Abs(input.y) >= 0.5f)
            {
                _XRRig.transform.position += _forwardLook.forward * (input.y * _movementSpeed * Time.deltaTime);
            }
            
            //move left and right
            if (Math.Abs(input.x) >= 0.5f)
            {
                _XRRig.transform.position += _XRRig.transform.right * (input.x * _movementSpeed * Time.deltaTime);
            }
        }
    }
}