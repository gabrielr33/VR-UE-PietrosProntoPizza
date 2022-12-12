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
                _plateTransform.localPosition = Vector3.Lerp(_plateTransform.localPosition, Vector3.zero, Time.deltaTime * 1.5f);
        }

        public void ClearPizzaFromTrigger()
        {
            _lerpPizza = false;
            if (_plateTransform.childCount > 0)
                Destroy(_plateTransform.GetChild(0).gameObject);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            PizzaPlate plate = other.GetComponent<PizzaPlate>();

            if (plate == null || plate.AttachedPizza == null)
                return;
            
            _plateTransform = plate.transform;
            _plateTransform.SetParent(transform);

            _lerpPizza = true;

            plate.GetComponent<NetworkGrabbable>().enabled = false;
            GetComponentInParent<Seat>().PizzaReceived(plate.AttachedPizza);
        }
    }
}
