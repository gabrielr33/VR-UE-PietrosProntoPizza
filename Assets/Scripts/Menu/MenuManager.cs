using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance;

        [SerializeField] private bool _vrMode;

        private Camera _camera;
        private Canvas _canvas;
        private GameObject _eventSystem;
        
        private Menu[] _menus;

        void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        private void Start()
        {
            LoadData();
            
            if (_canvas != null && _camera != null && _eventSystem != null)
            {
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

        public void LoadData()
        {
            _camera = Camera.main;
            _canvas = GetComponent<Canvas>();
            _eventSystem = GameObject.Find("EventSystem");
            _menus = GameObject.FindGameObjectsWithTag("Menu").Select(x => x.GetComponent<Menu>()).ToArray();
        }
    }
}