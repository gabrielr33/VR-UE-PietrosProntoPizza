using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Networking
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        public static RoomManager Instance;

        [SerializeField] private TMP_InputField _playerNameInputField;

        private const string RoomName = "PietrosProntoPizzaRoom";
        
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

        public override void OnEnable()
        {
            base.OnEnable();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (!scene.name.Equals("GameScene"))
                return;
            
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.Destroy(gameObject);
        }

        public void OnPlayPressed()
        {
            PhotonNetwork.JoinLobby();
        }

        public void OnExitPressed()
        {
            Application.Quit();
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
    }
}
