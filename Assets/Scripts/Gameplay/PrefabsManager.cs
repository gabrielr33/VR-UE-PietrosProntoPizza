using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PrefabsManager : MonoBehaviour
    {
        [field: SerializeField] public List<Customer> CustomerPrefabs { get; private set; }
        [field: SerializeField] public List<PizzaType> PizzaTypes { get; private set; }
        [field: SerializeField] public List<Ingredient> PizzaIngredients { get; private set; }
        [field: SerializeField] public List<DrinkType> DrinkTypes { get; private set; }
        [field: SerializeField] public FloatingReviewText ReviewText { get; private set; }

        public GameObject GetIngredientPrefabFromPizzaIngredientType(PizzaIngredient type)
        {
            foreach (Ingredient ingredient in PizzaIngredients)
            {
                if (ingredient.IngredientType.Equals(type))
                    return ingredient.gameObject;
            }

            return null;
        }


        public string GetIngredientNameFromPizzaIngredientType(PizzaIngredient type)
        {
            foreach (Ingredient ingredient in PizzaIngredients)
            {
                if (ingredient.IngredientType.Equals(type))
                    return ingredient.gameObject.name;
            }

            return "";
        }
    }
}
