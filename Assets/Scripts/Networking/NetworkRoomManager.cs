using Menu;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

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
            Instance = this;
        }
        
        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
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
                Debug.Log("In game as master.");
            else
                Debug.Log("In game as client.");
            
            // _gameManager.SpawnPlayerAndRaceTrack();
            
            PhotonNetwork.LoadLevel(1);
        }

        public override void OnLeftRoom()
        {
            MenuManager.Instance.OpenMenu("mainMenu");
        }
        
        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            Debug.Log("Disconnected.");
        }
    }
}
