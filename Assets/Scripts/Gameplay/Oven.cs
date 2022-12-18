using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace Gameplay
{
    public class Oven : MonoBehaviourPun
    {
        [SerializeField] private Transform _pizzaSlot1;
        [SerializeField] private Transform _pizzaSlot2;

        private Transform _pizzaTransform;
        private Dictionary<Transform, Pizza> _pizzaSlotsDictionary;

        private Coroutine _routine1;
        private Coroutine _routine2;

        private void Awake()
        {
            _pizzaSlotsDictionary = new Dictionary<Transform, Pizza>();
            _pizzaSlotsDictionary.Add(_pizzaSlot1, null);
            _pizzaSlotsDictionary.Add(_pizzaSlot2, null);
        }

        private void OnTriggerEnter(Collider other)
        {
            PizzaShovel pizzaShovel = other.GetComponent<PizzaShovel>();
            if (pizzaShovel == null)
                return;

            Pizza pizza = pizzaShovel.AttachedPizza;

            if (pizza == null)
                return;

            pizza.CanBePickedUp = false;

            // Detach pizza from pizza shovel
            pizzaShovel.DetachPizza();

            CheckForFreeSlot(pizza);
        }

        private void CheckForFreeSlot(Pizza pizza)
        {
            _pizzaTransform = pizza.transform;

            if (_pizzaSlotsDictionary[_pizzaSlot1] == null)
            {
                _pizzaSlotsDictionary[_pizzaSlot1] = pizza;
                pizza.transform.position = _pizzaSlot1.position;
                pizza.transform.rotation = Quaternion.identity;
                    
                pizza.StartPizzaBakingProcess();
            }
            else if (_pizzaSlotsDictionary[_pizzaSlot2] == null)
            {
                _pizzaSlotsDictionary[_pizzaSlot2] = pizza;
                pizza.transform.position = _pizzaSlot2.position;
                pizza.transform.rotation = Quaternion.identity;

                pizza.StartPizzaBakingProcess();
            }
            else
                Debug.Log("No free pizza slots right now!");
        }
        
        public void RemovePizzaFromPizzaSlotIfAssigned(Pizza pizza)
        {
            photonView.RPC("RemovePizzaFromPizzaSlot", RpcTarget.All, pizza.GetComponent<PhotonView>().ViewID);
        }

        [PunRPC]
        private void RemovePizzaFromPizzaSlot(int pizzaViewId)
        {
            Pizza[] pizzas = FindObjectsOfType<Pizza>();
            Pizza pizza = pizzas.First(x => x.photonView.ViewID == pizzaViewId);
            
            Transform slot = null;

            foreach (KeyValuePair<Transform, Pizza> kvp in _pizzaSlotsDictionary)
            {
                if (kvp.Value == pizza)
                {
                    slot = kvp.Key;
                    break;
                }
            }

            if (slot != null)
                _pizzaSlotsDictionary[slot] = null;
        }

        public void ClearOven()
        {
            _pizzaSlotsDictionary.Clear();
            _pizzaTransform = null;
        }
    }
}
