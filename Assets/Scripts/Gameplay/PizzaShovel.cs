using Photon.Pun;
using UnityEngine;

namespace Gameplay
{
    public class PizzaShovel : MonoBehaviourPun
    {
        [SerializeField] private Transform _pizzaPosition;
        [SerializeField] private Oven _pizzaOven;

        public Pizza AttachedPizza { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            Pizza pizza = other.GetComponent<Pizza>();

            // If the collided object is not a pizza or there is already a pizza on the shovel, return
            if (pizza == null || AttachedPizza != null || !pizza.CanBePickedUp)
                return;

            if (pizza.GetComponent<PhotonView>().Owner.Equals(PhotonNetwork.LocalPlayer))
            {
                _pizzaOven.RemovePizzaFromPizzaSlotIfAssigned(pizza);
                pizza.StopBakingProcess();
                
                transform.GetComponentInParent<PizzaShovelHelper>().AttachPizzaToPizzaShovel(pizza, _pizzaPosition);
            }
        }

        public void DetachPizza()
        {
            if (AttachedPizza == null)
                return;

            AttachedPizza.transform.SetParent(null);
            AttachedPizza.CanBePickedUp = false;
            AttachedPizza = null;
        }
    }
}