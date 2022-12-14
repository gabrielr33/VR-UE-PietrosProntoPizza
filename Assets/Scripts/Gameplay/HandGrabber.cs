using Input;
using Networking;
using UnityEngine;

namespace Gameplay
{
    public class HandGrabber : MonoBehaviour
    {
        public Transform GrabbedObject { get; set; }

        [SerializeField] private bool _isRightHand;
        
        private PlayerInputController _inputController;
        
        private void Awake()
        {
            _inputController = GameObject.FindWithTag("InputController").GetComponent<PlayerInputController>();
        }

        private void Update()
        {
            if (GrabbedObject != null && 
                ((_isRightHand && _inputController.InputTrigger.RightTriggerInput < 0.5f) ||
                 (!_isRightHand && _inputController.InputTrigger.LeftTriggerInput < 0.5f)))
            {
                GrabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                GrabbedObject.SetParent(null);
                GrabbedObject = null;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if ((_isRightHand && _inputController.InputTrigger.RightTriggerInput > 0.5f) ||
                (!_isRightHand && _inputController.InputTrigger.LeftTriggerInput > 0.5f))
            {
                if (GrabbedObject == null && other.GetComponent<NetworkGrabbable>() != null)
                {
                    other.GetComponent<NetworkGrabbable>().SetNewOwnerOfObject();
                    other.transform.SetParent(transform);
                    GrabbedObject = other.transform;
                    GrabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                    // GrabbedObject.GetComponent<Rigidbody>().useGravity = false;
                }
            }
        }
    }
}
