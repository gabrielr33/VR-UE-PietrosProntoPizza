using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class Oven : MonoBehaviour
    {
        [SerializeField] private Transform _pizzaSlot1;
        [SerializeField] private Transform _pizzaSlot2;

        private Transform _pizzaTransform;
        private bool _lerpPizza1;
        private bool _lerpPizza2;
        private Dictionary<Transform, Pizza> _pizzaSlotsDictionary;

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

        private void Update()
        {
            if (_lerpPizza1)
                LerpPizzaToSlot(_pizzaSlot1);
            else if (_lerpPizza2)
                LerpPizzaToSlot(_pizzaSlot2);
        }

        private void LerpPizzaToSlot(Transform pizzaSlot)
        {
            _pizzaTransform.localPosition = Vector3.Lerp(_pizzaTransform.localPosition, pizzaSlot.position, Time.deltaTime * 1.5f);
            _pizzaTransform.localRotation = Quaternion.Lerp(_pizzaTransform.localRotation, pizzaSlot.rotation, Time.deltaTime * 1.5f);
        }

        private void CheckForFreeSlot(Pizza pizza)
        {
            _pizzaTransform = pizza.transform;
            
            if (_pizzaSlotsDictionary[_pizzaSlot1] == null)
            {
                pizza.GetComponent<Rigidbody>().isKinematic = true;
                _pizzaSlotsDictionary[_pizzaSlot1] = pizza;
                
                _lerpPizza1 = true;
                StartCoroutine(WaitForLerpAndStartBaking(true, pizza));
            }
            else if (_pizzaSlotsDictionary[_pizzaSlot2] == null)
            {
                pizza.GetComponent<Rigidbody>().isKinematic = true;
                _pizzaSlotsDictionary[_pizzaSlot1] = pizza;
                
                _lerpPizza2 = true;
                StartCoroutine(WaitForLerpAndStartBaking(false, pizza));
            }
            else
                Debug.Log("No free pizza slots right now!");
        }

        private IEnumerator WaitForLerpAndStartBaking(bool lerpPizza1, Pizza pizza)
        {
            yield return new WaitForSeconds(2.5f);

            if (lerpPizza1)
                _lerpPizza1 = false;
            else
                _lerpPizza2 = false;

            StartPizzaBakingProcess(pizza);
        }

        private void StartPizzaBakingProcess(Pizza pizza)
        {
            Debug.Log("Pizza inserted in oven! Start baking!");
            pizza.CanBePickedUp = true;
            // pizza.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
