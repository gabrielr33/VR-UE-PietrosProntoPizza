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

            // photonView.RPC("AttachPizzaToPizzaShovel", RpcTarget.All, pizza.photonView.ViewID);

            if (pizza.GetComponent<PhotonView>().Owner.Equals(PhotonNetwork.LocalPlayer))
            {
                _pizzaOven.RemovePizzaFromPizzaSlotIfAssigned(pizza);
                pizza.StopBakingProcess();
                AttachedPizza = pizza;
                Transform pizzaTransform = AttachedPizza.transform;

                // pizza.GetComponent<Rigidbody>().isKinematic = true;
                pizzaTransform.SetParent(_pizzaPosition);
                pizzaTransform.localPosition = Vector3.zero;
                pizzaTransform.localRotation = Quaternion.identity;
                // AttachedPizza.GetComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>();
            }
        }

        // [PunRPC]
        // private void AttachPizzaToPizzaShovel(int pizzaViewId)
        // {
        //     Pizza[] pizzas = FindObjectsOfType<Pizza>();
        //     Pizza pizza = pizzas.First(x => x.photonView.ViewID == pizzaViewId);
        //     
        //     
        // }

        public void DetachPizza()
        {
            // pizza.GetComponent<Rigidbody>().isKinematic = false;
            if (AttachedPizza == null)
                return;

            // AttachedPizza.GetComponent<FixedJoint>().connectedBody = null;
            AttachedPizza.CanBePickedUp = false;
            AttachedPizza = null;
        }
    }
}