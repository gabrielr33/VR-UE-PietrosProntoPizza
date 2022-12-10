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
        [SerializeField] private PizzaType _orderedPizza;
        [SerializeField] private DrinkType _orderedDrink;
        [SerializeField] private bool _testCompare;

        private GameManager _gameManager;
        private PrefabsManager _prefabsManager;
        private Transform _cameraTransform;
        private Animator _customerAnimator;
        private static readonly int CustomerStateX = Animator.StringToHash("CustomerStateX");
        private static readonly int CustomerStateY = Animator.StringToHash("CustomerStateY");

        private void Awake()
        {
            _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
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

        public Order GenerateOrder(PrefabsManager prefabsManager, int tableNumber)
        {
            _prefabsManager = prefabsManager;
            Random rand = new Random();

            List<PizzaType> pizzaTypes = prefabsManager.PizzaTypes;
            _orderedPizza = pizzaTypes[rand.Next(pizzaTypes.Count)];
            
            List<DrinkType> drinkTypes = prefabsManager.DrinkTypes;
            _orderedDrink = drinkTypes[rand.Next(drinkTypes.Count)];
            
            Order order = new Order
            {
                TableNumber = tableNumber,
                Pizza = _orderedPizza,
                Drink = _orderedDrink,
                CustomerName = Name,
                MaxWaitTimeInSec = rand.Next(_gameManager.GameValues.CustomerMinWaitTime, _gameManager.GameValues.CustomerMaxWaitTime)   // TODO tweak this
            };

            StartCoroutine(StartWaitingProcedure(order.MaxWaitTimeInSec));

            return order;
        }

        public void SetAnimatorControllerState(CustomerAnimationState state)
        {
            if (_customerAnimator != null)
            {
                switch (state)
                {
                    case CustomerAnimationState.Talking:
                        photonView.RPC("SetAnimatorControllerValues", RpcTarget.All, 0f, 0f);
                        break;
                    case CustomerAnimationState.Angry:
                        photonView.RPC("SetAnimatorControllerValues", RpcTarget.All, -1f, 0f);
                        break;
                    case CustomerAnimationState.Clap:
                        photonView.RPC("SetAnimatorControllerValues", RpcTarget.All, 1f, 0f);
                        break;
                    case CustomerAnimationState.SitToStand:
                        photonView.RPC("SetAnimatorControllerValues", RpcTarget.All, 0f, 1f);
                        break;
                    case CustomerAnimationState.StandAngry:
                        photonView.RPC("SetAnimatorControllerValues", RpcTarget.All, 1f, 1f);
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
            List<PizzaIngredient> missingIngredients = _orderedPizza.ingredients.Except(recPizza.Ingredients).ToList();
            List<PizzaIngredient> unwantedIngredients = recPizza.Ingredients.Except(_orderedPizza.ingredients).ToList();

            decimal review = GameplayHelper.CalculateStarsReviewForOrder(missingIngredients, unwantedIngredients, recPizza.BakingStage);

            GenerateVisibleReviewInScene(review);

            // _customerNameText.text = review.ToString();
            // TODO
        }

        private void Update()
        {
            _customerNameText.transform.LookAt(_cameraTransform.position);
            _customerNameText.transform.Rotate(0f, 180f, 0f);

            if (_testCompare)
            {
                CompareReceivedPizzaWithOrder(new Pizza() { Ingredients = new List<PizzaIngredient>()
                    {
                        PizzaIngredient.Mozzarella,
                        PizzaIngredient.Egg,
                        PizzaIngredient.Bacon,
                        PizzaIngredient.TomatoSauce
                    }});
                _testCompare = false;
            }
        }

        private IEnumerator StartWaitingProcedure(int waitTime)
        {
            int counter = waitTime;
            while (counter > 0) {
                yield return new WaitForSeconds(1);
                counter--;
            }
            
            // TODO
            Debug.Log("Order expired!");
            SetAnimatorControllerState(CustomerAnimationState.SitToStand);
            
            GenerateVisibleReviewInScene(0);
        }

        private void GenerateVisibleReviewInScene(decimal review)
        {
            FloatingReviewText text = Instantiate(_prefabsManager.ReviewText, _floatingReviewStartPos.position, Quaternion.identity, _floatingReviewStartPos);
            text.SetUp(review);
            
            _gameManager.AddReviewToGameScore(review);
        }
    }

    public enum CustomerAnimationState
    {
        Talking,
        Angry,
        Clap,
        SitToStand,
        StandAngry
    }
}