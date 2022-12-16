using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
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

        private void Update()
        {
            if (_lerpPizza1)
                LerpPizzaToSlot(_pizzaSlot1);
            else if (_lerpPizza2)
                LerpPizzaToSlot(_pizzaSlot2);
        }

        private void LerpPizzaToSlot(Transform pizzaSlot)
        {
            _pizzaTransform.localPosition =
                Vector3.Lerp(_pizzaTransform.localPosition, Vector3.zero, Time.deltaTime * 1.5f);
            _pizzaTransform.localRotation =
                Quaternion.Lerp(_pizzaTransform.localRotation, Quaternion.identity, Time.deltaTime * 1.5f);
        }

        private void CheckForFreeSlot(Pizza pizza)
        {
            _pizzaTransform = pizza.transform;

            if (_pizzaSlotsDictionary[_pizzaSlot1] == null)
            {
                //pizza.GetComponent<Rigidbody>().isKinematic = true;
                _pizzaSlotsDictionary[_pizzaSlot1] = pizza;
                pizza.transform.SetParent(_pizzaSlot1);

                _lerpPizza1 = true;
                _routine1 = StartCoroutine(WaitForLerp1AndStartBaking(pizza));
            }
            else if (_pizzaSlotsDictionary[_pizzaSlot2] == null)
            {
                //pizza.GetComponent<Rigidbody>().isKinematic = true;
                _pizzaSlotsDictionary[_pizzaSlot2] = pizza;
                pizza.transform.SetParent(_pizzaSlot2);

                _lerpPizza2 = true;
                _routine2 = StartCoroutine(WaitForLerp2AndStartBaking(pizza));
            }
            else
                Debug.Log("No free pizza slots right now!");
        }

        private IEnumerator WaitForLerp1AndStartBaking(Pizza pizza)
        {
            yield return new WaitForSeconds(2.5f);
            _lerpPizza1 = false;
            pizza.StartPizzaBakingProcess();
        }

        private IEnumerator WaitForLerp2AndStartBaking(Pizza pizza)
        {
            yield return new WaitForSeconds(2.5f);
            _lerpPizza2 = false;
            pizza.StartPizzaBakingProcess();
        }

        public void RemovePizzaFromPizzaSlotIfAssigned(Pizza pizza)
        {
            Transform slot = null;

            foreach (KeyValuePair<Transform, Pizza> kvp in _pizzaSlotsDictionary)
            {
                if (kvp.Value == pizza)
                {
                    slot = kvp.Key;

                    if (slot.Equals(_pizzaSlot1))
                        StopCoroutine(_routine1);
                    else if (slot.Equals(_pizzaSlot2))
                        StopCoroutine(_routine2);

                    break;
                }
            }

            if (slot != null)
                _pizzaSlotsDictionary[slot] = null;
        }

        public void ClearOven()
        {
            StopAllCoroutines();
            _pizzaSlotsDictionary.Clear();
            _pizzaTransform = null;
            _lerpPizza1 = false;
            _lerpPizza2 = false;

            if (!PhotonNetwork.IsMasterClient)
                return;

            if (_pizzaSlot1.childCount > 0)
                PhotonNetwork.Destroy(_pizzaSlot1.GetChild(0).gameObject);

            if (_pizzaSlot2.childCount > 0)
                PhotonNetwork.Destroy(_pizzaSlot2.GetChild(0).gameObject);
        }
    }
}
