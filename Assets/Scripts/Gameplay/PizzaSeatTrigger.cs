using UnityEngine;

namespace Gameplay
{
    public class PizzaSeatTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Pizza pizza = other.GetComponent<Pizza>();

            if (pizza == null)
                return;
            
            GetComponentInParent<Seat>().PizzaReceived(pizza);
        }
    }
}
