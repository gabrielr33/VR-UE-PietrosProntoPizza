using Menu;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Networking
{
    public class NetworkRoomManager : MonoBehaviourPunCallbacks
    {
        public static NetworkRoomManager Instance;
        
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
        
        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
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
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (!scene.name.Equals("GameScene"))
                return;
            
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.Destroy(gameObject);
        }
        
        public override void OnConnected()
        {
            base.OnConnected();
            Debug.Log("Connected to Photon!");
        }
        
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            MenuManager.Instance.OpenMenu("mainMenu");
            Debug.Log("Connected to master");
        }
        
        public override void OnJoinedLobby()
        {
            MenuManager.Instance.OpenMenu("lobbyMenu");
            Debug.Log("Joined Lobby");
        }
        
        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} in game as master.");
                PhotonNetwork.LoadLevel(1);
            }
            else
                Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} in game as client.");
            
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.LoadLevel(0);
            MenuManager.Instance.LoadData();
            MenuManager.Instance.OpenMenu("mainMenu");
        }
        
        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            Debug.Log("Disconnected.");
        }
    }
}
