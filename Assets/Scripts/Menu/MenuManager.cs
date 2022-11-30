using UnityEngine;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance;
    
        [SerializeField] private global::Menu.Menu[] _menus;
    
        void Awake()
        {
            Instance = this;
        }
    
        public void OpenMenu(global::Menu.Menu menuToOpen)
        {
            foreach (var menu in _menus)
            {
                if (menu.open || menu.gameObject.activeSelf)
                    CloseMenu(menu);
            }
            menuToOpen.Open();
        }
    
        public void OpenMenu(string menuName)
        {
            foreach (var menu in _menus)
            {
                if (menu.menuName.Equals(menuName))
                    menu.Open();

                else if (menu.open || menu.gameObject.activeSelf)
                    CloseMenu(menu);
            }
        }
    
        public void CloseMenu(global::Menu.Menu menu)
        {
            menu.Close();
        }
    }
}
