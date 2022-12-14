using Photon.Pun;
using UnityEngine;

namespace Gameplay
{
    public class PizzaPlate : MonoBehaviour
    {
        public Pizza AttachedPizza { get; private set; }
        
        private void OnTriggerEnter(Collider other)
        {
            PizzaShovel pizzaShovel = other.GetComponent<PizzaShovel>();

            if (pizzaShovel == null || pizzaShovel.AttachedPizza == null || AttachedPizza != null)
                return;
            
            AttachedPizza = pizzaShovel.AttachedPizza;
            AttachedPizza.CanBePickedUp = false;
            
            Transform pizzaTransform = pizzaShovel.AttachedPizza.transform; 
            pizzaTransform.SetParent(transform);
            pizzaTransform.localPosition = Vector3.zero;
            pizzaTransform.localRotation = Quaternion.identity;
            AttachedPizza.GetComponent<PhotonView>().enabled = false;
            
            pizzaShovel.DetachPizza();
        }
    }
}
