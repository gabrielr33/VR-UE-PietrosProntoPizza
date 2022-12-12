using Networking;
using UnityEngine;

namespace Gameplay
{
    public class DrinkSeatTrigger : MonoBehaviour
    {
        private Transform _drinkTransform;
        private bool _lerpDrink;
        
        private void Update()
        {
            if (_lerpDrink)
                _drinkTransform.localPosition = Vector3.Lerp(_drinkTransform.localPosition, Vector3.zero, Time.deltaTime * 1.5f);
        }
        
        public void ClearDrinkFromTrigger()
        {
            _lerpDrink = false;
            if (_drinkTransform != null && _drinkTransform.childCount > 0)
                Destroy(_drinkTransform.GetChild(0).gameObject);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Drink drink = other.GetComponent<Drink>();

            if (drink == null)
                return;
            
            _drinkTransform = drink.transform;
            _drinkTransform.SetParent(transform);

            _lerpDrink = true;

            drink.GetComponent<NetworkGrabbable>().enabled = false;
            GetComponentInParent<Seat>().DrinkReceived(drink);
        }
    }
}
