using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "PizzaType", menuName = "ScriptableObjects/PizzaType", order = 1)]
    public class PizzaType : ScriptableObject
    {
        public string pizzaName;

        public List<PizzaIngredients> ingredients;
    }

    public enum PizzaIngredients
    {
        TomatoSauce,
        Mozzarella,
        Paprika,
        Salami,
        Onion,
        Tomato,
        Olives,
        Bacon,
        Egg,
        Mushrooms
    }
}
