using UnityEngine;

namespace Gameplay
{
    public class PizzaPositionManager : MonoBehaviour
    {
        private MeshRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            other.transform.position = transform.position;

            _renderer.enabled = false;
        }

        private void OnTriggerExit(Collider other)
        {
            _renderer.enabled = true;
        }
    }
}
