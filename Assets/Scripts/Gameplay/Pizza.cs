using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class Pizza : MonoBehaviour
    {
        public List<PizzaIngredient> Ingredients { get; set; }
        public bool CanBePickedUp { get; set; }
        public BakingStageEnum BakingStage  { get; private set; }

        [SerializeField] private Transform _pizzaDough;
        [SerializeField] private List<Material> _pizzaDoughMaterials;

        private int _bakingCounter = 0;
        
        private void Start()
        {            
            Ingredients = new List<PizzaIngredient>();
            CanBePickedUp = true;
            SetPizzaBakingStage(_bakingCounter);
        }

        private void OnTriggerEnter(Collider other)
        {
            Ingredient ingredient = other.GetComponent<Ingredient>();

            if (ingredient == null || ingredient.IsContainer)
                return;

            EnableIngredient(ingredient);                  
            Ingredients.Add(ingredient.IngredientType);
        }

        private void EnableIngredient(Ingredient ingredient)
        {
            foreach(Ingredient child in transform.GetComponentsInChildren<Ingredient>(true))
            {
                if(child.IngredientType.Equals(ingredient.IngredientType))
                {
                    child.gameObject.SetActive(true);

                    if (ingredient.IngredientType.Equals(PizzaIngredient.TomatoSauce))
                        break;

                    Destroy(ingredient.gameObject);
                    break;
                }
            }
        }

        public void StartPizzaBakingProcess()
        {
            StartCoroutine(PizzaBakingProcess());
        }

        private IEnumerator PizzaBakingProcess()
        {
            Debug.Log("Pizza inserted in oven! Start baking!");
            CanBePickedUp = true;
            
            yield return new WaitForSeconds(5f);

            _bakingCounter++;
            SetPizzaBakingStage(_bakingCounter);

            yield return new WaitForSeconds(5f);

            _bakingCounter++;
            SetPizzaBakingStage(_bakingCounter);

            // pizza.GetComponent<Rigidbody>().isKinematic = false;
        }

        private void SetPizzaBakingStage(int bakingCounter)
        {
            if (bakingCounter > 2)
                return;
            
            BakingStage = (BakingStageEnum)bakingCounter;
            
            switch (BakingStage)
            {
                case BakingStageEnum.Raw:
                    _pizzaDough.GetComponent<MeshRenderer>().material = _pizzaDoughMaterials[0];
                    break;
                case BakingStageEnum.Done:
                    _pizzaDough.GetComponent<MeshRenderer>().material = _pizzaDoughMaterials[1];
                    break;
                case BakingStageEnum.Burned:
                    _pizzaDough.GetComponent<MeshRenderer>().material = _pizzaDoughMaterials[2];
                    break;
            }
        }

        public void StopBakingProcess()
        {
            StopAllCoroutines();
        }

        public enum BakingStageEnum
        {
            Raw = 0,
            Done,
            Burned
        }
    }
}
