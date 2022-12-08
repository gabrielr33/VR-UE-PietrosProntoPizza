using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gameplay;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameValues GameValues { get; private set; }
    
    [SerializeField] private Transform _startPositionChef;
    [SerializeField] private Transform _startPositionWaiter;

    [SerializeField] private GameObject _XRRig;

    [Header("Gameplay")]
    [SerializeField] private TMP_Text _countdownTimerText;

    private bool _isMasterClient;

    private List<decimal> _reviews;
    private decimal _gameScore; // ranges from 0 to 5

    private void Awake()
    {
        _isMasterClient = PhotonNetwork.IsMasterClient;
        _reviews = new List<decimal>();

        GameValues = IOFileManager.ReadGameValuesFromFile();
        
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        // Distinguish between chef and waiter
        if (_isMasterClient)
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefabs\\Photon", "Player"), Vector3.zero, Quaternion.identity);
            _XRRig.transform.position = _startPositionWaiter.position;
            _XRRig.transform.rotation = _startPositionWaiter.rotation;
        }
        else
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefabs\\Photon", "Player"), Vector3.zero, Quaternion.identity);
            _XRRig.transform.position = _startPositionChef.position;
            _XRRig.transform.rotation = _startPositionChef.rotation;
        }
    }

    public void AddReviewToGameScore(decimal review)
    {
        _reviews.Add(review);
        _gameScore = _reviews.Average();

        _countdownTimerText.text = $"Time left:\n00:00\nScore: {_gameScore:F}";
    }
}