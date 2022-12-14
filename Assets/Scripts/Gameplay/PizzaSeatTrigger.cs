using Networking;
using UnityEngine;

namespace Gameplay
{
    public class PizzaSeatTrigger : MonoBehaviour
    {
        private Transform _plateTransform;
        private bool _lerpPizza;
        
        private void Update()
        {
            if (_lerpPizza)
            {
                _plateTransform.localPosition =
                    Vector3.Lerp(_plateTransform.localPosition, Vector3.zero, Time.deltaTime * 1.5f);
                // _plateTransform.localRotation =
                //     Quaternion.Lerp(_plateTransform.localRotation, Quaternion.identity, Time.deltaTime * 1.5f);
            }
        }

        public void ClearPizzaFromTrigger()
        {
            _lerpPizza = false;
            if (_plateTransform != null && _plateTransform.childCount > 0)
                Destroy(_plateTransform.GetChild(0).gameObject);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            PizzaPlate plate = other.GetComponent<PizzaPlate>();

            if (plate == null || plate.AttachedPizza == null)
                return;
            
            _plateTransform = plate.transform;
            plate.transform.localRotation = Quaternion.identity;
            plate.transform.SetParent(transform);

            plate.GetComponentInChildren<Pizza>().CanBePickedUp = false;
            plate.GetComponent<NetworkGrabbable>().enabled = false;
            plate.GetComponent<BoxCollider>().enabled = false;
            plate.GetComponent<Rigidbody>().isKinematic = true;
            GetComponentInParent<Seat>().PizzaReceived(plate.AttachedPizza);
            
            _lerpPizza = true;
        }
    }
}
