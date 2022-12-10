using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Networking
{
    public class NetworkGrabbable : NetworkObject
    {
        private PhotonView _pv;

        private int _localPlayerID;

        private void Awake()
        {
            _pv = GetComponent<PhotonView>();
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                PhotonView pv = player.GetComponent<PhotonView>();
                if (pv != null && pv.IsMine)
                    _localPlayerID = pv.Owner.ActorNumber;
            }
        }

        public void SetNewOwnerOfObject()
        {
            _pv.RPC("SetOwner", RpcTarget.All, _localPlayerID);
        }

        [PunRPC]
        private void SetOwner(int newOwnerID)
        {
            if (_pv.IsMine)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject player in players)
                {
                    Player pl = player.GetComponent<PhotonView>().Owner;
                    if (pl != null && pl.ActorNumber.Equals(newOwnerID))
                        _pv.TransferOwnership(pl);
                }
            }
        }
    }
}