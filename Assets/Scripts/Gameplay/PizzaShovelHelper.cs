using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace Gameplay
{
    public class PizzaShovelHelper : MonoBehaviourPun
    {
        [SerializeField] private PizzaShovel _pizzaShovel;
        private Transform _pizzaPosition;
        
        public void AttachPizzaToPizzaShovel(Pizza pizza, Transform pizzaPosition)
        {
            _pizzaPosition = pizzaPosition;
            photonView.RPC("AttachPizza", RpcTarget.All, pizza.GetComponent<PhotonView>().ViewID);
        }
        
        [PunRPC]
        private void AttachPizza(int pizzaViewId)
        {
            Pizza[] pizzas = FindObjectsOfType<Pizza>();
            Pizza pizza = pizzas.First(x => x.photonView.ViewID == pizzaViewId);
            
            _pizzaShovel.AttachedPizza = pizza;
            Transform pizzaTransform = _pizzaShovel.AttachedPizza.transform;

            // pizza.GetComponent<Rigidbody>().isKinematic = true;
            pizzaTransform.SetParent(_pizzaPosition);
            pizzaTransform.localPosition = Vector3.zero;
            pizzaTransform.localRotation = Quaternion.identity;
        }
    }
}
