using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gameplay;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameManager : MonoBehaviourPun
{
    public GameValues GameValues { get; private set; }
    public GameResultValues GameResultValues { get; private set; }

    [SerializeField] private Transform _startPositionChef;
    [SerializeField] private Transform _startPositionWaiter;

    [SerializeField] private GameObject _XRRig;

    [SerializeField] private OrderManager _orderManager;

    [SerializeField] private Oven _oven;
    [SerializeField] private BillboardButtonManager _billboardButtonManager;
    [SerializeField] private List<PlateSpawner> _plateSpawner;

    private bool _isMasterClient;

    private List<decimal> _reviews;
    private decimal _gameScore; // ranges from 0 to 5

    private int _playerCount = 0;
    private bool _gameStarted;

    private void Awake()
    {
        _isMasterClient = PhotonNetwork.IsMasterClient;
        _reviews = new List<decimal>();

        GameValues = IOFileManager.ReadGameValuesFromFile();
        GameResultValues = new GameResultValues();

        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        // Distinguish between chef and waiter
        if (_isMasterClient && _XRRig.transform.position != _startPositionWaiter.position)
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefabs\\Photon", "PlayerWaiter"), Vector3.zero, Quaternion.identity);
            _XRRig.transform.position = _startPositionWaiter.position;
            _XRRig.transform.rotation = _startPositionWaiter.rotation;
        }
        else if (!_isMasterClient && _XRRig.transform.position != _startPositionChef.position)
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefabs\\Photon", "PlayerChef"), Vector3.zero, Quaternion.identity);
            _XRRig.transform.position = _startPositionChef.position;
            _XRRig.transform.rotation = _startPositionChef.rotation;
        }

        photonView.RPC("NewPlayerSpawned", RpcTarget.MasterClient);
    }

    public void DisconnectFromGame()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
            PhotonNetwork.LeaveRoom();
        }
    }

    [PunRPC]
    private void NewPlayerSpawned()
    {
        if (!_isMasterClient)
            return;

        _playerCount++;
        _billboardButtonManager.UpdatePlayerListText(PhotonNetwork.PlayerList);
    }

    public void AddReviewToGameScore(decimal review, bool correctDrinkServed, bool correctPizzaServed)
    {
        if (!_isMasterClient || !_gameStarted)
            return;

        _reviews.Add(review);
        _gameScore = _reviews.Average();

        GameResultValues.ReviewScore = _gameScore;

        if (review > 0)
            GameResultValues.ServedCustomers++;

        if (correctPizzaServed)
            GameResultValues.CorrectlyServedPizzas++;

        if (correctDrinkServed)
            GameResultValues.CorrectlyServedDrinks++;
    }

    public void StartGame(GameMode gameMode)
    {
        if (!_isMasterClient || _playerCount < 2)
            return;

        _gameStarted = true;

        StopAllCoroutines();
        _reviews.Clear();
        _gameScore = 0;
        GameResultValues.ReviewScore = 0;
        GameResultValues.ServedCustomers = 0;
        GameResultValues.TotalCustomers = 0;
        GameResultValues.CorrectlyServedPizzas = 0;
        GameResultValues.CorrectlyServedDrinks = 0;

        CheckForFreeTablesAndSpawnCustomers();

        photonView.RPC("StartGame", RpcTarget.All, (int)gameMode);
    }

    public void GameEnded()
    {
        if (!_isMasterClient)
            return;

        StopAllCoroutines();
        _gameStarted = false;
        _orderManager.RemoveAllOrdersAndCustomers();
        _oven.ClearOven();
        foreach (PlateSpawner spawner in _plateSpawner)
            spawner.RemoveAllPlatesInScene();
    }

    private void CheckForFreeTablesAndSpawnCustomers()
    {
        StopAllCoroutines();

        Debug.Log("Try spawning customers.");
        _orderManager.SpawnNewCustomers();

        StartCoroutine(WaitForNextCustomerSpawn());
    }

    private IEnumerator WaitForNextCustomerSpawn()
    {
        yield return new WaitForSeconds(GameValues.CustomerSpawnSpeed / ((int)_billboardButtonManager.GameMode + 1));

        CheckForFreeTablesAndSpawnCustomers();
    }

    [PunRPC]
    private void StartGame(int gameMode)
    {
        _billboardButtonManager.StartGameCountdown();
    }
}

public class GameResultValues
{
    public decimal ReviewScore { get; set; }
    public int ServedCustomers { get; set; }
    public int TotalCustomers { get; set; }
    public int CorrectlyServedPizzas { get; set; }
    public int CorrectlyServedDrinks { get; set; }
}