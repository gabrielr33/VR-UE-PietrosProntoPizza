using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkGrabbable : MonoBehaviour, IPunObservable
{
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
