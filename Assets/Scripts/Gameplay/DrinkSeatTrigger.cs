using Networking;
using Photon.Pun;
using UnityEngine;

namespace Gameplay
{
    public class DrinkSeatTrigger : MonoBehaviour
    {
        private Transform _drinkTransform;
        private bool _lerpDrink;
        private Rigidbody _rb;
        
        private void Update()
        {
            if (_lerpDrink)
            {
                _drinkTransform.localPosition = Vector3.Lerp(_drinkTransform.localPosition, transform.position, Time.deltaTime * 1.5f);
                _drinkTransform.localRotation = Quaternion.Lerp(_drinkTransform.localRotation, transform.rotation, Time.deltaTime * 1.5f);
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
            _drinkTransform.SetParent(null);

            _rb = _drinkTransform.GetComponent<Rigidbody>();
            _drinkTransform.GetComponent<NetworkGrabbable>().enabled = false;
            _drinkTransform.GetComponent<Rigidbody>().useGravity = false;
            _drinkTransform.GetComponent<Rigidbody>().isKinematic = true;
            _drinkTransform.GetComponent<BoxCollider>().enabled = false;
            GetComponentInParent<Seat>().DrinkReceived(drink);
            
            _lerpDrink = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (_rb != null)
                _rb.isKinematic = true;
        }
    }
}
