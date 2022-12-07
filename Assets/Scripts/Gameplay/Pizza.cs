using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class Pizza : MonoBehaviour
    {
        public List<PizzaIngredient> Ingredients { get; set; }

        private void Start()
        {
            // TODO for testing purposes only
            Ingredients = new List<PizzaIngredient>();
            Ingredients.Add(PizzaIngredient.Mozzarella);
            Ingredients.Add(PizzaIngredient.TomatoSauce);
            Ingredients.Add(PizzaIngredient.Onion);
        }

        private void OnTriggerEnter(Collider other)
        {
            Ingredient ingredient = other.GetComponent<Ingredient>();

            if (ingredient == null)
                return;
            
            Ingredients.Add(ingredient.IngredientType);
        }
    }
}
