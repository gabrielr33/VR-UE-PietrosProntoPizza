using System.Collections;
using System.IO;
using Input;
using Photon.Pun;
using UnityEngine;

namespace Gameplay
{
    public class PizzaSpawner : MonoBehaviour
    {

        private PlayerInputController _inputController;
        private Transform _pizzaTemplate;
        private bool _startLerp;
        private int _pizzaScale = 125;

        private void Awake()
        {
            _inputController = GameObject.FindWithTag("InputController").GetComponent<PlayerInputController>();
        }

        private void Update()
        {
            if (_startLerp)
                _pizzaTemplate.localScale = Vector3.Lerp(_pizzaTemplate.localScale, new Vector3(_pizzaScale, _pizzaScale, _pizzaScale), Time.deltaTime * 0.5f);
        }

        private void OnTriggerStay(Collider other)
        {
            if (_startLerp == false && other.CompareTag("RightController") && _inputController.InputButtons.ButtonPrimary_Right > 0.5f)
            {
                _pizzaTemplate = PhotonNetwork.Instantiate(Path.Combine("Prefabs\\Photon", "PizzaTemplate") , transform.position, Quaternion.identity).transform;
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