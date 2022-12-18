using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace Gameplay
{
    public class PizzaShovel : MonoBehaviourPun
    {
        [SerializeField] private Transform _pizzaPosition;
        [SerializeField] private Oven _pizzaOven;

        public Pizza AttachedPizza { get; private set; }

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
                
                photonView.RPC("AttachPizzaToPizzaShovel", RpcTarget.All, pizza.GetComponent<PhotonView>().ViewID);
            }
        }

        [PunRPC]
        private void AttachPizzaToPizzaShovel(int pizzaViewId)
        {
            Pizza[] pizzas = FindObjectsOfType<Pizza>();
            Pizza pizza = pizzas.First(x => x.photonView.ViewID == pizzaViewId);
            
            AttachedPizza = pizza;
            Transform pizzaTransform = AttachedPizza.transform;

            // pizza.GetComponent<Rigidbody>().isKinematic = true;
            pizzaTransform.SetParent(_pizzaPosition);
            pizzaTransform.localPosition = Vector3.zero;
            pizzaTransform.localRotation = Quaternion.identity;
        }

        public void DetachPizza()
        {
            if (AttachedPizza == null)
                return;

            AttachedPizza.CanBePickedUp = false;
            AttachedPizza = null;
        }
    }
}