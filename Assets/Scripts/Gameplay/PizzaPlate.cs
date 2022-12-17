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

            // photonView.RPC("AttachPizzaToPlate", RpcTarget.All, pizzaShovel.AttachedPizza.photonView.ViewID, pizzaShovel.photonView.ViewID);
            
            AttachedPizza = pizzaShovel.AttachedPizza;
            AttachedPizza.CanBePickedUp = false;
            
            Transform pizzaTransform = AttachedPizza.transform;
            pizzaTransform.SetParent(transform);
            pizzaTransform.localPosition = Vector3.zero;
            pizzaTransform.localRotation = Quaternion.identity;
            AttachedPizza.GetComponent<PhotonView>().enabled = false;
            
            // PizzaShovel[] shovels = FindObjectsOfType<PizzaShovel>();
            pizzaShovel.DetachPizza();
        }
        
        // [PunRPC]
        // private void AttachPizzaToPlate(int pizzaViewId, int shovelViewId)
        // {
        //     Pizza[] pizzas = FindObjectsOfType<Pizza>();
        //     
        //     AttachedPizza = pizzas.First(x => x.photonView.ViewID == pizzaViewId);
        //     AttachedPizza.CanBePickedUp = false;
        //     
        //     Transform pizzaTransform = AttachedPizza.transform;
        //     pizzaTransform.SetParent(transform);
        //     pizzaTransform.localPosition = Vector3.zero;
        //     pizzaTransform.localRotation = Quaternion.identity;
        //     AttachedPizza.GetComponent<PhotonView>().enabled = false;
        //     
        //     PizzaShovel[] shovels = FindObjectsOfType<PizzaShovel>();
        //     shovels.First(x => x.photonView.ViewID == shovelViewId).DetachPizza();
        // }
    }
}
