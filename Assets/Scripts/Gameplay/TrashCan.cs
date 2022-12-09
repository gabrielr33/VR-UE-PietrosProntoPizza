using UnityEngine;

namespace Gameplay
{
    public class TrashCan : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Pizza>() != null)
            {
                Destroy(other.gameObject);
            }
        }
    }
}