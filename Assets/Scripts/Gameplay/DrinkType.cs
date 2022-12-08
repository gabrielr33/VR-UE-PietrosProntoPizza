using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "DrinkType", menuName = "ScriptableObjects/DrinkType", order = 1)]
    public class DrinkType : ScriptableObject
    {
        public string drinkName;
    }
}
