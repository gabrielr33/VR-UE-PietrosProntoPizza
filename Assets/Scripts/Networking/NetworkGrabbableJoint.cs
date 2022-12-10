using Input;
using Photon.Pun;
using UnityEngine;

namespace Gameplay
{
    public class NetworkGrabbableJoint : MonoBehaviour, IPunObservable
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

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            // Would need to be observed by a PhotonView on the prefab

            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                //   stream.SendNext(Object obj); // can observe any variable
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else
            {
                // Network player, receive data
                //   this.obj = (Object)stream.ReceiveNext(); // receive the same variable from the stream if not local player
                transform.position = (Vector3)stream.ReceiveNext();
                transform.rotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }


}
