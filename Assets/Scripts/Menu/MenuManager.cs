using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    
    [SerializeField] private Menu[] _menus;
    
    void Awake()
    {
        Instance = this;
    }
    
    public void OpenMenu(Menu menuToOpen)
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
    
    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}
