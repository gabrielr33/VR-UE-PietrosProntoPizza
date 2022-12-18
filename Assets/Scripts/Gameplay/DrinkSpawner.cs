using System.IO;
using Input;
using Photon.Pun;
using UnityEngine;

namespace Gameplay
{
    public class DrinkSpawner : MonoBehaviour
    {
        private PrefabsManager _prefabsManager;
        private PlayerInputController _inputController;
        [SerializeField] private DrinkType _drinkType;

        private string _drinkName;

        private void Awake()
        {
            _inputController = GameObject.FindWithTag("InputController").GetComponent<PlayerInputController>();
            _prefabsManager = GameObject.FindWithTag("PrefabsManager").GetComponent<PrefabsManager>();
            _drinkName = _prefabsManager.GetDrinkNameFromDrinkType(_drinkType);
        }

        private void OnTriggerStay(Collider other)
        {
            if ((other.tag.Equals("RightController") && _inputController.InputTrigger.RightTriggerInput > 0.5f) ||
                (other.tag.Equals("LeftController") && _inputController.InputTrigger.LeftTriggerInput > 0.5f))
            {
                HandGrabber handGrabber = other.GetComponent<HandGrabber>();

                if (handGrabber != null && handGrabber.GrabbedObject == null)
                {
                    GameObject drink = PhotonNetwork.Instantiate(Path.Combine("Prefabs\\Drinks", _drinkName), handGrabber.transform.position, Quaternion.identity);
                    drink.transform.SetParent(handGrabber.transform);
                    drink.transform.localPosition = new Vector3(-0.0021f, -0.033f, 0.049f);
                    drink.GetComponent<Rigidbody>().isKinematic = true;

                    handGrabber.GrabbedObject = drink.transform;
                }
            }
        }
    }
}