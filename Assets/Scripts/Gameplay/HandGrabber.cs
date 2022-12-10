using Input;
using Networking;
using UnityEngine;

namespace Gameplay
{
    public class HandGrabber : MonoBehaviour
    {
        public Transform GrabbedObject { get; set; }
        
        private PrefabsManager _prefabsManager;
        private PlayerInputController _inputController;
        
        private void Awake()
        {
            _inputController = GameObject.FindWithTag("InputController").GetComponent<PlayerInputController>();
            _prefabsManager = GameObject.FindWithTag("PrefabsManager").GetComponent<PrefabsManager>();
        }

        private void Update()
        {
            if (GrabbedObject != null && _inputController.InputTrigger.RightTriggerInput < 0.5f)
            {
                GrabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                GrabbedObject.SetParent(null);
                GrabbedObject = null;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if ((gameObject.tag.Equals("RightController") && _inputController.InputTrigger.RightTriggerInput > 0.5f) ||
                (gameObject.tag.Equals("LeftController") && _inputController.InputTrigger.LeftTriggerInput > 0.5f))
            {
                if (GrabbedObject == null && other.GetComponent<NetworkGrabbable>() != null)
                {
                    other.GetComponent<NetworkGrabbable>().SetNewOwnerOfObject();
                    other.transform.SetParent(transform);
                    GrabbedObject = other.transform;
                    GrabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }
}
