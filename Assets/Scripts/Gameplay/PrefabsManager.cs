using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PrefabsManager : MonoBehaviour
    {
        [field: SerializeField] public List<GameObject> CustomerPrefabs { get; private set; }
        [field: SerializeField] public List<PizzaType> PizzaTypes { get; private set; }
    }
}
