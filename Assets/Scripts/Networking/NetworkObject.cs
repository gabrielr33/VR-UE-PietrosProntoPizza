using Photon.Pun;
using UnityEngine;

namespace Networking
{
    public abstract class NetworkObject : MonoBehaviourPun, IPunObservable
    {
        protected Rigidbody _rb;
    
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            // Would need to be observed by a PhotonView on the prefab

            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                //   stream.SendNext(Object obj); // can observe any variable
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);

                if (_rb != null)
                {
                    stream.SendNext(_rb.velocity);
                    stream.SendNext(_rb.angularVelocity);
                    stream.SendNext(_rb.isKinematic);
                    stream.SendNext(_rb.useGravity);
                }
            }
            else
            {
                // Network player, receive data
                //   this.obj = (Object)stream.ReceiveNext(); // receive the same variable from the stream if not local player
                transform.position = (Vector3)stream.ReceiveNext();
                transform.rotation = (Quaternion)stream.ReceiveNext();

                if (_rb != null)
                {
                    _rb.velocity = (Vector3)stream.ReceiveNext();
                    _rb.angularVelocity = (Vector3)stream.ReceiveNext();
                    _rb.isKinematic = (bool)stream.ReceiveNext();
                    _rb.useGravity = (bool)stream.ReceiveNext();
                }
            }
        }
    }
}
