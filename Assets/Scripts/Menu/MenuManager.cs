using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance;
        private Canvas _canvas;
        [SerializeField] private Camera _camera;
        private GameObject _eventSystem;

        [SerializeField] private bool _vrMode = false;
        [SerializeField] private global::Menu.Menu[] _menus;
    
        void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            _eventSystem = GameObject.Find("EventSystem");

            if(_canvas != null && _camera != null && _eventSystem != null){
                if (_vrMode)
                {
                    _canvas.renderMode = RenderMode.WorldSpace;
                    _canvas.worldCamera = _camera;

                    transform.localScale = new Vector3(0.0015f, 0.0015f, 0.0015f);
                    transform.position = _camera.transform.position + new Vector3(0.0f, 0.0f, 3.0f);
                }
                else
                {
                    _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    GetComponent<TrackedDeviceGraphicRaycaster>().enabled = false;
                }
            }
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
