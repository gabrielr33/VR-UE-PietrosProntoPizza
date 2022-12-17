using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "PizzaType", menuName = "ScriptableObjects/PizzaType", order = 1)]
    public class PizzaType : ScriptableObject
    {
        public string pizzaName;

        public List<PizzaIngredient> ingredients;
    }

    public enum PizzaIngredient
    {
        TomatoSauce,
        Mozzarella,
        Pepper,
        Salami,
        Onion,
        Tomato,
        Olives,
        Bacon,
        Egg,
        Mushrooms
    }
}
