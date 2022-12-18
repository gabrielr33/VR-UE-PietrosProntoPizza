using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance;

        [SerializeField] private bool _vrMode;
        [SerializeField] private TMP_InputField _playerNameInputField;
        
        private const string RoomName = "PietrosProntoPizzaRoom";

        private Camera _camera;
        private Canvas _canvas;
        private GameObject _eventSystem;
        
        private Menu[] _menus;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        
        public void OnPlayPressed()
        {
            PhotonNetwork.JoinLobby();
        }

        public void OnExitPressed()
        {
            Application.Quit();
        }
        
        public void OnControlsPressed()
        {
            Instance.OpenMenu("controlsMenu");
        }
        
        public void OnAboutPressed()
        {
            Instance.OpenMenu("aboutMenu");
        }
        
        public void OnBackPressed()
        {
            Instance.OpenMenu("mainMenu");
        }
        
        public void OnConnectPressed()
        {
            RoomOptions roomOptions = new RoomOptions {MaxPlayers = 4};
            TypedLobby typedLobby = new TypedLobby(RoomName, LobbyType.Default);
            
            PhotonNetwork.NickName = !string.IsNullOrEmpty(_playerNameInputField.text)
                ? _playerNameInputField.text
                : "Player#" + Random.Range(0, 100).ToString("000");
            
            PhotonNetwork.JoinOrCreateRoom(RoomName, roomOptions, typedLobby);
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

                    _canvas.transform.localScale = new Vector3(0.0015f, 0.0015f, 0.0015f);
                    _canvas.transform.position = _camera.transform.position + new Vector3(0.0f, 0.0f, 3.0f);
                }
                else
                {
                    _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    _canvas.GetComponent<TrackedDeviceGraphicRaycaster>().enabled = false;
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
            _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            _eventSystem = GameObject.Find("EventSystem");
            _menus = GameObject.FindGameObjectsWithTag("Menu").Select(x => x.GetComponent<Menu>()).ToArray();
        }
    }
}