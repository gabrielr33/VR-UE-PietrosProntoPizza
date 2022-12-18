using Networking;
using Photon.Pun;
using UnityEngine;

namespace Gameplay
{
    public class PizzaSeatTrigger : MonoBehaviour
    {
        private Transform _plateTransform;
        private bool _lerpPizza;
        private Rigidbody _rb;
        
        private void Update()
        {
            if (_lerpPizza)
            {
                _plateTransform.localPosition = Vector3.Lerp(_plateTransform.localPosition, transform.position, Time.deltaTime * 1.5f);
                _plateTransform.localRotation = Quaternion.Lerp(_plateTransform.localRotation, transform.rotation, Time.deltaTime * 1.5f);
            }
        }

        public void ClearPizzaFromTrigger()
        {
            _lerpPizza = false;
            if (_plateTransform != null)
            {
                Pizza pizza = _plateTransform.GetComponentInChildren<Pizza>();
                if (pizza != null)
                {
                    pizza.GetComponent<PhotonView>().enabled = true;
                    PhotonNetwork.Destroy(pizza.gameObject);
                }
                
                PhotonNetwork.Destroy(_plateTransform.gameObject);
                _plateTransform = null;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            PizzaPlate plate = other.GetComponent<PizzaPlate>();

            if (plate == null || plate.AttachedPizza == null)
                return;
            
            _plateTransform = plate.transform;
            _plateTransform.SetParent(null);

            _rb = _plateTransform.GetComponent<Rigidbody>();
            _plateTransform.GetComponent<NetworkGrabbable>().enabled = false;
            _plateTransform.GetComponent<Rigidbody>().useGravity = false;
            _plateTransform.GetComponent<Rigidbody>().isKinematic = true;
            _plateTransform.GetComponent<BoxCollider>().enabled = false;
            plate.GetComponentInChildren<Pizza>().CanBePickedUp = false;
            GetComponentInParent<Seat>().PizzaReceived(plate.AttachedPizza);
            
            _lerpPizza = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (_rb != null)
                _rb.isKinematic = true;
        }
    }
}
