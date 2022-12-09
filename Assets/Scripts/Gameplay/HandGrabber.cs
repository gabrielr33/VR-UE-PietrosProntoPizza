using Input;
using UnityEngine;

namespace Gameplay
{
    public class HandGrabber : MonoBehaviour
    {
        private PrefabsManager _prefabsManager;
        private PlayerInputController _inputController;
        [SerializeField] private Transform _grabbedObject;
        
        private void Awake()
        {
            _inputController = GameObject.FindWithTag("InputController").GetComponent<PlayerInputController>();
            _prefabsManager = GameObject.FindWithTag("PrefabsManager").GetComponent<PrefabsManager>();
        }

        private void Update()
        {
            if (_grabbedObject != null && _inputController.InputTrigger.RightTriggerInput < 0.5f)
            {
                _grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                _grabbedObject.SetParent(null);
                _grabbedObject = null;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if ((gameObject.tag.Equals("RightController") && _inputController.InputTrigger.RightTriggerInput > 0.5f) ||
                (gameObject.tag.Equals("LeftController") && _inputController.InputTrigger.LeftTriggerInput > 0.5f))
            {
                if (_grabbedObject == null && other.GetComponent<GrabbableObject>() != null)
                {
                    other.transform.SetParent(transform);
                    _grabbedObject = other.transform;
                    _grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                }
                else if (_grabbedObject == null && other.GetComponent<Ingredient>() != null)
                {
                    GameObject ingredientPrefab = _prefabsManager.GetIngredientPrefabFromPizzaIngredientType(other.GetComponent<Ingredient>().IngredientType);
                    if (ingredientPrefab == null)
                        return;
                    GameObject ingredient = Instantiate(ingredientPrefab, Vector3.zero, Quaternion.identity);
                    ingredient.transform.SetParent(transform);
                    ingredient.transform.localPosition = new Vector3(0f, 0f, 0.1f);
                    _grabbedObject = ingredient.transform;
                    _grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }
}
