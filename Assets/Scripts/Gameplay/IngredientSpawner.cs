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
                    GameObject ingredient = PhotonNetwork.Instantiate(Path.Combine("Prefabs\\Ingredients", _ingredientName), handGrabber.transform.position, Quaternion.identity);
                    ingredient.transform.SetParent(handGrabber.transform);
                    ingredient.transform.localPosition = new Vector3(-0.0021f, -0.033f, 0.049f);
                    // ingredient.GetComponent<Rigidbody>().useGravity = false;
                    // ingredient.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    // ingredient.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    ingredient.GetComponent<Rigidbody>().isKinematic = true;
                    
                    handGrabber.GrabbedObject = ingredient.transform;
                }
            }
        }
    }
}