using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PrefabsManager : MonoBehaviour
    {
        [field: SerializeField] public List<Customer> CustomerPrefabs { get; private set; }
        [field: SerializeField] public List<PizzaType> PizzaTypes { get; private set; }
        [field: SerializeField] public List<DrinkType> DrinkTypes { get; private set; }
        [field: SerializeField] public FloatingReviewText ReviewText { get; private set; }
    }
}
