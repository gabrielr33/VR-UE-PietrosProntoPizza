using System.IO;
using Input;
using Photon.Pun;
using UnityEngine;

namespace Gameplay
{
    public class IngredientSpawner : MonoBehaviour
    {
        private PrefabsManager _prefabsManager;
        private PlayerInputController _inputController;

        private string _ingredientName;
        
        private void Awake()
        {
            _inputController = GameObject.FindWithTag("InputController").GetComponent<PlayerInputController>();
            _prefabsManager = GameObject.FindWithTag("PrefabsManager").GetComponent<PrefabsManager>();
            _ingredientName = _prefabsManager.GetIngredientNameFromPizzaIngredientType(GetComponent<Ingredient>().IngredientType);
        }

        private void OnTriggerStay(Collider other)
        {
            if ((other.tag.Equals("RightController") && _inputController.InputTrigger.RightTriggerInput > 0.5f) ||
                (other.tag.Equals("LeftController") && _inputController.InputTrigger.LeftTriggerInput > 0.5f))
            {
                HandGrabber handGrabber = other.GetComponent<HandGrabber>();
                
                if (handGrabber != null && handGrabber.GrabbedObject == null)
                {
                    GameObject ingredient = PhotonNetwork.Instantiate(Path.Combine("Prefabs\\Ingredients", _ingredientName), Vector3.zero, Quaternion.identity);
                    ingredient.transform.SetParent(handGrabber.transform);
                    ingredient.transform.localPosition = new Vector3(0f, 0f, 0.1f);
                    
                    handGrabber.GrabbedObject = ingredient.transform;
                    handGrabber.GrabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }
}