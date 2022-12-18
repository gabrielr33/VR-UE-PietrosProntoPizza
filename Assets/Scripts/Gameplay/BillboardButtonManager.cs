using System.Text;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class BillboardButtonManager : MonoBehaviourPun
    {
        public GameMode GameMode { get; private set; }

        [SerializeField] private TMP_Text _connectedPlayersText;
        [SerializeField] private TMP_Text _countdownTimerText;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _stopGameButton;
        [SerializeField] private Transform _toggleGroup;

        [SerializeField] private GameObject _resultsBillboard;
        [SerializeField] private TMP_Text _resultsBillboardText;

        private GameManager _gameManager;

        // Countdown
        [SerializeField] private bool _countdownStarted;

        private float _countDownTimer;
        // private float _currentGameTimeSelected;

        void Awake()
        {
            _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
            _stopGameButton.interactable = false;
        }

        private void Start()
        {
            _countDownTimer = _gameManager.GameValues.GameTime;
            _countdownTimerText.text = CalculateTimeFromGameTime(_countDownTimer);
        }

        private void Update()
        {
            if (_countdownStarted)
            {
                if (_countDownTimer > 0.005f)
                {
                    _countDownTimer -= Time.deltaTime;
                    _countdownTimerText.text = CalculateTimeFromGameTime(_countDownTimer);
                }
                else
                {
                    ResetGameValues();
                    _resultsBillboard.SetActive(true);

                    _gameManager.GameEnded();

                    GameResultValues values = _gameManager.GameResultValues;
                    string text =
                        $"\n\n{values.ReviewScore:0}/5.0\n\n{values.ServedCustomers}/{values.TotalCustomers}\n\n{values.CorrectlyServedPizzas}/{values.TotalCustomers}\n{values.CorrectlyServedDrinks}/{values.TotalCustomers}";
                    photonView.RPC("ShowResultsText", RpcTarget.Others, text);
                }
            }
        }

        [PunRPC]
        private void ShowResultsText(string text)
        {
            _resultsBillboardText.text = text;
        }

        public void UpdatePlayerListText(Player[] players)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            StringBuilder sb = new StringBuilder();
            foreach (Player player in players)
            {
                sb.Append($"- {player.NickName}");
                sb.Append("\n");
            }

            photonView.RPC("SetPlayerListText", RpcTarget.All, sb.ToString());
        }

        [PunRPC]
        private void SetPlayerListText(string text)
        {
            _connectedPlayersText.text = text;
        }

        private void ResetGameValues()
        {
            _countdownStarted = false;
            _stopGameButton.interactable = false;
            _startGameButton.interactable = true;
            _countdownTimerText.color = Color.black;
            _countDownTimer = _gameManager.GameValues.GameTime;
            _countdownTimerText.text = CalculateTimeFromGameTime(_countDownTimer);

            for (int i = 0; i < _toggleGroup.childCount; i++)
            {
                Toggle toggle = _toggleGroup.GetChild(i).GetComponent<Toggle>();
                toggle.interactable = true;
            }
        }

        public void OnToggleSelected(int index)
        {
            _countDownTimer = _gameManager.GameValues.GameTime;
            // _currentGameTimeSelected = _countDownTimer;

            _countdownTimerText.text = CalculateTimeFromGameTime(_countDownTimer);
        }

        public void OnStartGamePressed()
        {
            for (int i = 0; i < _toggleGroup.childCount; i++)
            {
                Toggle toggle = _toggleGroup.GetChild(i).GetComponent<Toggle>();
                if (toggle.isOn)
                    GameMode = (GameMode)i;

                toggle.interactable = false;
            }

            _startGameButton.interactable = false;
            _stopGameButton.interactable = true;

            _gameManager.StartGame(GameMode);
            _countdownStarted = true;
            _resultsBillboard.SetActive(false);
        }

        public void OnStopGamePressed()
        {
            ResetGameValues();
        }

        public void OnDisconnectPressed()
        {
            _gameManager.DisconnectFromGame();
        }

        public void StartGameCountdown()
        {
            _countdownStarted = true;
        }

        /// <summary>
        /// Calculates the time that is running down and displayed on the billboard.
        /// </summary>
        private string CalculateTimeFromGameTime(float gameTime)
        {
            int intTime = (int)gameTime;
            int min = intTime / 60;
            int sec = intTime % 60;

            return $"Time left:\n{min:00}:{sec:00}";
        }
    }

    public enum GameMode
    {
        Easy = 0,
        Hard,
        Pietro
    }
}