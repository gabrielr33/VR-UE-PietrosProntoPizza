using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class Pizza : MonoBehaviour
    {
        private List<PizzaIngredient> _ingredients;

        private void Start()
        {
            // TODO for testing purposes only
            _ingredients = new List<PizzaIngredient>();
            _ingredients.Add(PizzaIngredient.Mozzarella);
            _ingredients.Add(PizzaIngredient.TomatoSauce);
            _ingredients.Add(PizzaIngredient.Onion);
        }

        private void OnTriggerEnter(Collider other)
        {
            Ingredient ingredient = other.GetComponent<Ingredient>();

            if (ingredient == null)
                return;
            
            _ingredients.Add(ingredient.IngredientType);
        }
    }
}
