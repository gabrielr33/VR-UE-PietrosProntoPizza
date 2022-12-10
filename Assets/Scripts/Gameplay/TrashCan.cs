using Photon.Pun;
using UnityEngine;

namespace Gameplay
{
    public class TrashCan : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Pizza>() != null)
            {
                if (PhotonNetwork.LocalPlayer.Equals(other.GetComponent<PhotonView>().Owner))
                    PhotonNetwork.Destroy(other.gameObject);
            }
        }
    }
}