using UnityEngine;

namespace Gameplay
{
    public class PizzaPlate : MonoBehaviour
    {
        public Pizza AttachedPizza { get; private set; }
        
        private void OnTriggerEnter(Collider other)
        {
            Pizza pizza = other.GetComponent<Pizza>();

            if (pizza == null || AttachedPizza != null)
                return;
            
            AttachedPizza = pizza;
            
            Transform pizzaTransform = other.transform; 
            pizzaTransform.SetParent(transform);
            pizzaTransform.localPosition = Vector3.zero;
            pizzaTransform.localRotation = Quaternion.identity;
        }
    }
}
