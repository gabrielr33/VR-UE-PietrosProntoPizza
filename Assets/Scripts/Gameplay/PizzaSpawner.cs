using System.Collections;
using Input;
using UnityEngine;

namespace Gameplay
{
    public class PizzaSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _pizzaTemplatePrefab;

        private PlayerInputController _inputController;
        private Transform _pizzaTemplate;
        private bool _startLerp;

        private void Awake()
        {
            _inputController = GameObject.FindWithTag("InputController").GetComponent<PlayerInputController>();
        }

        private void Update()
        {
            if (_startLerp)
                _pizzaTemplate.localScale = Vector3.Lerp(_pizzaTemplate.localScale, new Vector3(150, 150, 150), Time.deltaTime * 0.5f);
        }

        private void OnTriggerStay(Collider other)
        {
            if (_startLerp == false && other.CompareTag("RightController") && _inputController.InputButtons.ButtonPrimary_Right > 0.5f)
            {
                _pizzaTemplate = Instantiate(_pizzaTemplatePrefab, Vector3.zero, Quaternion.identity).transform;
                _pizzaTemplate.localPosition = transform.position;
                _pizzaTemplate.localScale = new Vector3(10, 10, 10);
                _pizzaTemplate.GetComponent<BoxCollider>().enabled = false;
                _startLerp = true;
                
                StartCoroutine(WaitForLerp());
            }
        }

        private IEnumerator WaitForLerp()
        {
            yield return new WaitForSeconds(5f);
            
            _startLerp = false;
            _pizzaTemplate.GetComponent<BoxCollider>().enabled = true;
        }
    }
}