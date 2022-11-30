using UnityEngine;

namespace Menu
{
    public class Menu : MonoBehaviour
    {
        public string menuName;
        public bool open;
    
        public void Open()
        {
            gameObject.SetActive(true);
            open = true;
        }

        public void Close()
        {
            gameObject.SetActive(false);
            open = false;
        }
    }
}
