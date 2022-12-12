using UnityEngine;

namespace Gameplay
{
    public class Drink : MonoBehaviour
    {
        [field: SerializeField] public DrinkType DrinkType { get; private set; }
    }
}
