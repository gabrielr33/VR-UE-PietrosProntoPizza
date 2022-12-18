using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace Gameplay
{
    public class PizzaPlate : MonoBehaviourPun
    {
        public Pizza AttachedPizza { get; private set; }
        
        private void OnTriggerEnter(Collider other)
        {
            PizzaShovel pizzaShovel = other.GetComponent<PizzaShovel>();

            if (pizzaShovel == null || pizzaShovel.AttachedPizza == null || AttachedPizza != null)
                return;
            
            if (photonView.Owner.Equals(PhotonNetwork.LocalPlayer))
                photonView.RPC("AttachPizzaToPlate", RpcTarget.All, pizzaShovel.transform.parent.GetComponent<PhotonView>().ViewID, pizzaShovel.AttachedPizza.GetComponent<PhotonView>().ViewID);
        }

        [PunRPC]
        private void AttachPizzaToPlate(int pizzaShovelViewId, int pizzaViewId)
        {
            PizzaShovel[] pizzaShovels = FindObjectsOfType<PizzaShovel>();
            PizzaShovel pizzaShovel = pizzaShovels.First(x => x.transform.parent.GetComponent<PhotonView>().ViewID == pizzaShovelViewId);
            
            Pizza[] pizzas = FindObjectsOfType<Pizza>();
            Pizza pizza = pizzas.First(x => x.GetComponent<PhotonView>().ViewID == pizzaViewId);
            
            AttachedPizza = pizza;
            AttachedPizza.CanBePickedUp = false;
            
            Transform pizzaTransform = AttachedPizza.transform;
            AttachedPizza.GetComponent<PhotonView>().enabled = false;
            
            pizzaShovel.DetachPizza();
            
            pizzaTransform.SetParent(transform);
            pizzaTransform.localPosition = Vector3.zero;
            pizzaTransform.localRotation = Quaternion.identity;
        }
    }
}
