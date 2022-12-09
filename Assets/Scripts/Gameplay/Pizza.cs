using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class Pizza : MonoBehaviour
    {
        public List<PizzaIngredient> Ingredients { get; set; }

        private void Start()
        {            
            Ingredients = new List<PizzaIngredient>();            
        }

        private void OnTriggerEnter(Collider other)
        {
            Ingredient ingredient = other.GetComponent<Ingredient>();

            if (ingredient == null)
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
    }
}
