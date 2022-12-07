using UnityEngine;

namespace Gameplay
{
    public class Ingredient : MonoBehaviour
    {
        [field: SerializeField] public PizzaIngredient IngredientType { get; set; }
    }
}
