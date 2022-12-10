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
                PhotonNetwork.Destroy(other.gameObject);
            }
        }
    }
}