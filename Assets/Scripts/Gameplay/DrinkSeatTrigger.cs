using Networking;
using Photon.Pun;
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
            {
                _drinkTransform.localPosition = Vector3.Lerp(_drinkTransform.localPosition, Vector3.zero, Time.deltaTime * 1.5f);
                // _drinkTransform.localRotation = Quaternion.Lerp(_drinkTransform.localRotation, Quaternion.identity, Time.deltaTime * 1.5f);
            }
        }
        
        public void ClearDrinkFromTrigger()
        {
            _lerpDrink = false;
            if (_drinkTransform != null)
            {
                PhotonNetwork.Destroy(_drinkTransform.gameObject);
                _drinkTransform = null;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Drink drink = other.GetComponent<Drink>();

            if (drink == null)
                return;
            
            _drinkTransform = drink.transform;
            drink.transform.localRotation = Quaternion.identity;
            drink.transform.SetParent(transform);

            drink.GetComponent<NetworkGrabbable>().enabled = false;
            drink.GetComponent<BoxCollider>().enabled = false;
            drink.GetComponent<Rigidbody>().isKinematic = true;
            GetComponentInParent<Seat>().DrinkReceived(drink);
            
            _lerpDrink = true;
        }
    }
}
