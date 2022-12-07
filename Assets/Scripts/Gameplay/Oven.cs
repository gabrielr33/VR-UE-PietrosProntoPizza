using System;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public class Oven : MonoBehaviour
    {
        [SerializeField] private Transform _pizzaSlot1;
        [SerializeField] private Transform _pizzaSlot2;

        private Transform _pizzaTransform;
        [SerializeField] private bool _lerpPizza;
        
        private void OnTriggerEnter(Collider other)
        {
            Pizza pizza = other.GetComponent<Pizza>();
            if (pizza == null)
                return;

            // Detach from pizza shovel
            pizza.GetComponentInParent<PizzaShovel>().DetachPizza();
            pizza.transform.SetParent(transform);
            
            CheckForFreeSlot(pizza);
        }

        private void Update()
        {
            if (_lerpPizza)
                _pizzaTransform.localPosition = Vector3.Lerp(_pizzaTransform.localPosition, Vector3.zero, Time.deltaTime * 1.5f);
        }

        private void CheckForFreeSlot(Pizza pizza)
        {
            _pizzaTransform = pizza.transform;
            
            if (_pizzaSlot1.childCount == 0)
            {
                _pizzaTransform.SetParent(_pizzaSlot1);
                _lerpPizza = true;
                StartCoroutine(WaitForLerp());
                StartPizzaBakingProcess(pizza);
                Debug.Log("Pizza inserted in oven!");
            }
            else if (_pizzaSlot2.childCount == 0)
            {
                _pizzaTransform.SetParent(_pizzaSlot2);
                _lerpPizza = true;
                StartCoroutine(WaitForLerp());
                StartPizzaBakingProcess(pizza);
                Debug.Log("Pizza inserted in oven!");
            }
            else
                Debug.Log("No free pizza slots right now!");
        }

        private IEnumerator WaitForLerp()
        {
            yield return new WaitForSeconds(2.5f);
            _lerpPizza = false;
        }

        private void StartPizzaBakingProcess(Pizza pizza)
        {
            
        }
    }
}
