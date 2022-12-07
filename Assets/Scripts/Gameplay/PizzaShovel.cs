using UnityEngine;

namespace Gameplay
{
    public class PizzaShovel : MonoBehaviour
    {
        [SerializeField] private Transform _pizzaPosition;
        
        private Pizza _attachedPizza;
        
        private void OnTriggerEnter(Collider other)
        {
            Pizza pizza = other.GetComponent<Pizza>();
            
            // If the collided object is not a pizza or there is already a pizza on the shovel, return
            if (pizza == null || _attachedPizza != null)
                return;

            AttachPizzaToPizzaShovel(pizza);
        }

        private void AttachPizzaToPizzaShovel(Pizza pizza)
        {
            _attachedPizza = pizza;
            Transform pizzaTransform = _attachedPizza.transform;
            
            pizzaTransform.SetParent(_pizzaPosition);
            pizzaTransform.localPosition = Vector3.zero;
            pizza.GetComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>();
        }

        public void DetachPizza(Pizza pizza)
        {
            pizza.GetComponent<FixedJoint>().connectedBody = null;
            _attachedPizza = null;
        }
    }
}
