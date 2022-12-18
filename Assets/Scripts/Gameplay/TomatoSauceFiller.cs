using UnityEngine;

namespace Gameplay
{
    public class TomatoSauceFiller : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            TomatoSauceFillingManager fillingManager = other.GetComponent<TomatoSauceFillingManager>();

            if (fillingManager == null)
                return;

            fillingManager.FillSpoon();
        }
    }
}