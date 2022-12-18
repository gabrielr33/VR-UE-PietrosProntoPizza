using UnityEngine;

namespace Gameplay
{
    public class TomatoSauceFillingManager : MonoBehaviour
    {
        [field: SerializeField] public bool IsSpoonFilled { get; private set; }

        private MeshRenderer _filling;
        
        private void Start()
        {
            _filling = transform.GetChild(0).GetComponent<MeshRenderer>();
            IsSpoonFilled = true;
            FillSpoon();
        }

        public void EmptySpoon()
        {
            _filling.enabled = false;
            IsSpoonFilled = false;
        }

        public void FillSpoon()
        {
            _filling.enabled = true;
            IsSpoonFilled = true;
        }
    }
}