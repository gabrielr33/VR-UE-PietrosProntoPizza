using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace Gameplay
{
    public class Customer : MonoBehaviourPun
    {
        [field: SerializeField] public bool Female { get; private set; }
        [field: SerializeField] public string Name { get; private set; }

        [SerializeField] private TMP_Text _customerNameText;
        [SerializeField] private Transform _floatingReviewStartPos;
        
        private PrefabsManager _prefabsManager;
        private GameManager _gameManager;
        private BillboardButtonManager _billboardButtonManager;
        private OrderManager _orderManager;
        private Transform _cameraTransform;
        private Animator _customerAnimator;
        private static readonly int CustomerStateX = Animator.StringToHash("CustomerStateX");
        private static readonly int CustomerStateY = Animator.StringToHash("CustomerStateY");

        private Order _order;
        private Seat _seat;
        private Drink _receivedDrink;
        private bool _correctlyReceivedPizza;
        private bool _correctlyReceivedDrink;
        
        private void Awake()
        {
            _prefabsManager = GameObject.FindWithTag("PrefabsManager").GetComponent<PrefabsManager>();
            _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
            _billboardButtonManager = GameObject.FindWithTag("BillboardButtonManager").GetComponent<BillboardButtonManager>();
            _customerAnimator = GetComponent<Animator>();
            _cameraTransform = Camera.main.transform;
        }

        public void SelectCustomerName()
        {
            List<string> customerNames =
                Female ? IOFileManager.ReadCustomerNamesFemales() : IOFileManager.ReadCustomerNamesMales();

            if (customerNames.Count == 0)
                return;

            Random rand = new Random();
            photonView.RPC("SetCustomerName", RpcTarget.All, customerNames[rand.Next(customerNames.Count)]);
        }

        [PunRPC]
        private void SetCustomerName(string name)
        {
            Name = name;
            _customerNameText.text = name;
        }

        public Order GenerateOrder(OrderManager orderManager, int tableNumber, Seat seat)
        {
            _orderManager = orderManager;
            
            _seat = seat;

            _gameManager.GameResultValues.TotalCustomers++;

            List<PizzaType> pizzaTypes = _prefabsManager.PizzaTypes;
            List<DrinkType> drinkTypes = _prefabsManager.DrinkTypes;
            
            Random rand = new Random();
            _order = new Order
            {
                TableNumber = tableNumber,
                Pizza = pizzaTypes[rand.Next(pizzaTypes.Count)],
                Drink = drinkTypes[rand.Next(drinkTypes.Count)],
                CustomerName = Name,
                MaxWaitTimeInSec = rand.Next(_gameManager.GameValues.CustomerMinWaitTime / ((int)_billboardButtonManager.GameMode + 1), _gameManager.GameValues.CustomerMaxWaitTime / ((int)_billboardButtonManager.GameMode + 1))   // TODO tweak this
            };

            StartCoroutine(StartWaitingProcedure(_order.MaxWaitTimeInSec));

            return _order;
        }

        public void SetAnimatorControllerState(CustomerAnimationState state)
        {
            photonView.RPC("SetCustomerAnimation", RpcTarget.All, (int)state);
        }

        [PunRPC]
        private void SetCustomerAnimation(int stateIndex)
        {
            if (_customerAnimator != null)
            {
                switch ((CustomerAnimationState)stateIndex)
                {
                    case CustomerAnimationState.Idle:
                        photonView.RPC("SetAnimatorControllerValues", RpcTarget.All, 0f, 0f);
                        break;
                    case CustomerAnimationState.Eating:
                        photonView.RPC("SetAnimatorControllerValues", RpcTarget.All, 1f, 0f);
                        break;
                    case CustomerAnimationState.SitToStand:
                        photonView.RPC("SetAnimatorControllerValues", RpcTarget.All, 0f, 1f);
                        break;
                }
            }
        }

        [PunRPC]
        private void SetAnimatorControllerValues(float x, float y)
        {
            _customerAnimator.SetFloat(CustomerStateX, x);
            _customerAnimator.SetFloat(CustomerStateY, y);
        }

        public void CompareReceivedPizzaWithOrder(Pizza recPizza)
        {
            SetAnimatorControllerState(CustomerAnimationState.Eating);
            
            List<PizzaIngredient> missingIngredients = _order.Pizza.ingredients.Except(recPizza.Ingredients).ToList();
            List<PizzaIngredient> unwantedIngredients = recPizza.Ingredients.Except(_order.Pizza.ingredients).ToList();

            _correctlyReceivedPizza = missingIngredients.Count == 0 && unwantedIngredients.Count == 0;
            _correctlyReceivedDrink = _order.Drink.drinkName.Equals(_receivedDrink.DrinkType.drinkName);

            decimal review = GameplayHelper.CalculateStarsReviewForOrder(missingIngredients, unwantedIngredients, recPizza.BakingStage, _order.Drink, _receivedDrink);

            CustomerReceivedPizza(review);
        }

        public void DrinkReceived(Drink drink)
        {
            _receivedDrink = drink;
            _orderManager.RemoveDrinkOrderSheet(_order);
        }

        private void Update()
        {
            _customerNameText.transform.LookAt(_cameraTransform.position);
            _customerNameText.transform.Rotate(0f, 180f, 0f);
        }

        private IEnumerator StartWaitingProcedure(int waitTime)
        {
            int counter = waitTime;
            while (counter > 0) {
                yield return new WaitForSeconds(1);
                counter--;
            }
            
            Debug.Log("Order expired!");

            GenerateVisibleReviewInScene(0);
            RemoveOrderAndAddReview(0, false, false);
            StartCoroutine(DisappearAfterSeconds(0f));
        }

        private void CustomerReceivedPizza(decimal review)
        {
            GenerateVisibleReviewInScene(review);
            RemoveOrderAndAddReview(review, _correctlyReceivedDrink, _correctlyReceivedPizza);
            StartCoroutine(DisappearAfterSeconds(5f));
        }
        
        private void GenerateVisibleReviewInScene(decimal review)
        {
            photonView.RPC("GenerateReviewText", RpcTarget.All, (int)review);
        }

        [PunRPC]
        private void GenerateReviewText(int review)
        {
            FloatingReviewText text = Instantiate(_prefabsManager.ReviewText, _floatingReviewStartPos.position, Quaternion.identity, _floatingReviewStartPos);
            text.SetUp(review);
        }

        private void RemoveOrderAndAddReview(decimal review, bool correctDrinkServed, bool correctPizzaServed)
        {
            StopAllCoroutines();
            _orderManager.RemoveDrinkOrderSheet(_order);
            _orderManager.RemovePizzaOrderSheet(_order);
            _gameManager.AddReviewToGameScore(review, correctDrinkServed, correctPizzaServed);
        }

        private IEnumerator DisappearAfterSeconds(float sec)
        {
            yield return new WaitForSeconds(sec);

            _seat.FinishedEating();
            SetAnimatorControllerState(CustomerAnimationState.SitToStand);
            
            yield return new WaitForSeconds(3);
            
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public enum CustomerAnimationState
    {
        Idle,
        Eating,
        SitToStand
    }
}