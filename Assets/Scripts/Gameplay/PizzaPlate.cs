using UnityEngine;

namespace Gameplay
{
    public class PizzaPlate : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Pizza>() != null)
            {
                other.transform.SetParent(transform);
                other.transform.localPosition = Vector3.zero;
                other.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
