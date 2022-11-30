using System.IO;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _startPositionChef;
    [SerializeField] private Transform _startPositionWaiter;
    
    [SerializeField] private GameObject _XRRig;
    
    private bool _isMasterClient;
    
    private void Awake()
    {
        _isMasterClient = PhotonNetwork.IsMasterClient;

        SpawnPlayer();
    }
    
    public void SpawnPlayer()
    {
        // Distinguish between chef and waiter
        if (_isMasterClient)
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefabs\\Photon", "Player"), Vector3.zero, Quaternion.identity);
            _XRRig.transform.position = _startPositionChef.position;
        }
        else
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefabs\\Photon", "Player"),  Vector3.zero, Quaternion.identity);
            _XRRig.transform.position = _startPositionWaiter.position;
        }
    }
}
