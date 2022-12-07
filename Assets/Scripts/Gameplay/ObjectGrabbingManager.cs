using Input;
using UnityEngine;

namespace Gameplay
{
    public class ObjectGrabbingManager : MonoBehaviour
    {
        private PlayerInputController _inputController;
        private FixedJoint _fixedJoint;
        
        private void Awake()
        {
            _inputController = GameObject.FindWithTag("InputController").GetComponent<PlayerInputController>();
            _fixedJoint = GetComponent<FixedJoint>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("RightController") && _inputController.InputTrigger.RightTriggerInput > 0.5f)
            {
                _fixedJoint.connectedBody = other.GetComponent<Rigidbody>();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("RightController") && _inputController.InputTrigger.RightTriggerInput > 0.5f && _fixedJoint.connectedBody == null)
            {
                _fixedJoint.connectedBody = other.GetComponent<Rigidbody>();
            }
            else if (other.CompareTag("RightController") && _inputController.InputTrigger.RightTriggerInput < 0.5f && _fixedJoint.connectedBody != null)
            {
                _fixedJoint.connectedBody = null;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("RightController") && _fixedJoint.connectedBody != null)
            {
                _fixedJoint.connectedBody = null;
            }
        }
    }
}
