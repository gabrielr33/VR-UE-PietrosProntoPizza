using Input;
using UnityEngine;

namespace Gameplay
{
    public class HandGrabber : MonoBehaviour
    {
        private PlayerInputController _inputController;
        private Transform _grabbedObject;
        
        private void Awake()
        {
            _inputController = GameObject.FindWithTag("InputController").GetComponent<PlayerInputController>();
        }

        private void Update()
        {
            if (_grabbedObject != null && _inputController.InputTrigger.RightTriggerInput < 0.5f)
            {
                _grabbedObject.SetParent(null);
                _grabbedObject = null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_grabbedObject == null && other.GetComponent<GrabbableObject>() != null && _inputController.InputTrigger.RightTriggerInput > 0.5f)
            {
                other.transform.SetParent(transform);
                _grabbedObject = other.transform;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (_grabbedObject == null && other.GetComponent<GrabbableObject>() != null && _inputController.InputTrigger.RightTriggerInput > 0.5f)
            {
                other.transform.SetParent(transform);
                _grabbedObject = other.transform;
            }
        }
    }
}
