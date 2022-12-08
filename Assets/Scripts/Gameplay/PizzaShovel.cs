using UnityEngine;

namespace Gameplay
{
    public class PizzaShovel : MonoBehaviour
    {
        [SerializeField] private Transform _pizzaPosition;
        
        public Pizza AttachedPizza { get; private set; }
        
        private void OnTriggerEnter(Collider other)
        {
            Pizza pizza = other.GetComponent<Pizza>();
            
            // If the collided object is not a pizza or there is already a pizza on the shovel, return
            if (pizza == null || AttachedPizza != null || !pizza.CanBePickedUp)
                return;

            AttachPizzaToPizzaShovel(pizza);
        }

        private void AttachPizzaToPizzaShovel(Pizza pizza)
        {
            AttachedPizza = pizza;
            Transform pizzaTransform = AttachedPizza.transform;

            // pizza.GetComponent<Rigidbody>().isKinematic = true;
            pizzaTransform.localPosition = _pizzaPosition.position;
            pizzaTransform.localRotation = _pizzaPosition.rotation;
            AttachedPizza.GetComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>();
        }

        public void DetachPizza()
        {
            // pizza.GetComponent<Rigidbody>().isKinematic = false;
            if (AttachedPizza == null)
                return;
            
            AttachedPizza.GetComponent<FixedJoint>().connectedBody = null;
            AttachedPizza = null;
        }
    }
}
