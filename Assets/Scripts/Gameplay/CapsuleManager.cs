using Unity.XR.CoreUtils;
using UnityEngine;

namespace Gameplay
{
    public class CapsuleManager : MonoBehaviour
    {
        private XROrigin _rig;
        private CharacterController _characterController;
        private readonly float _additionalHeight = 0.2f;

        private void Start()
        {
            _rig = GetComponent<XROrigin>();
            _characterController = GetComponent<CharacterController>();
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        }

        private void FixedUpdate()
        {
            _characterController.height = _rig.CameraInOriginSpaceHeight + _additionalHeight;
            Vector3 capsuleCenter = transform.InverseTransformPoint(_rig.Camera.gameObject.transform.position);
            _characterController.center = new Vector3(capsuleCenter.x, _characterController.height / 2.0f + _characterController.skinWidth, capsuleCenter.z);
        }
    }
}
