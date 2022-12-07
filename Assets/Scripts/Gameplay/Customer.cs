using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace Gameplay
{
    public class Customer : MonoBehaviour
    {
        [field: SerializeField] public bool Female { get; private set; }
        [field: SerializeField] public string Name { get; private set; }

        [SerializeField] private TMP_Text _customerNameText;
        [SerializeField] private PizzaType _orderedPizza;
        [SerializeField] private bool _testCompare;

        private GameManager _gameManager;
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
            Name = customerNames[rand.Next(customerNames.Count)];
            _customerNameText.text = Name;
        }

        public Order GenerateOrder(PrefabsManager prefabsManager, int tableNumber)
        {
            Random rand = new Random();

            List<PizzaType> pizzaTypes = prefabsManager.PizzaTypes;
            _orderedPizza = pizzaTypes[rand.Next(pizzaTypes.Count)];

            Order order = new Order
            {
                TableNumber = tableNumber,
                Pizza = _orderedPizza,
                CustomerName = Name,
                //MaxWaitTimeInSec = rand.Next(80, 120)   // TODO tweak this
                MaxWaitTimeInSec = rand.Next(20, 40)
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
                        _customerAnimator.SetFloat(CustomerStateX, 0f);
                        _customerAnimator.SetFloat(CustomerStateY, 0f);
                        break;
                    case CustomerAnimationState.Angry:
                        _customerAnimator.SetFloat(CustomerStateX, -1f);
                        _customerAnimator.SetFloat(CustomerStateY, 0f);
                        break;
                    case CustomerAnimationState.Clap:
                        _customerAnimator.SetFloat(CustomerStateX, 1f);
                        _customerAnimator.SetFloat(CustomerStateY, 0f);
                        break;
                    case CustomerAnimationState.SitToStand:
                        _customerAnimator.SetFloat(CustomerStateX, 0f);
                        _customerAnimator.SetFloat(CustomerStateY, 1f);
                        break;
                    case CustomerAnimationState.StandAngry:
                        _customerAnimator.SetFloat(CustomerStateX, 1f);
                        _customerAnimator.SetFloat(CustomerStateY, 1f);
                        break;
                }
            }
        }

        public void CompareReceivedPizzaWithOrder(PizzaType recPizza)
        {
            List<PizzaIngredients> missingIngredients = _orderedPizza.ingredients.Except(recPizza.ingredients).ToList();
            List<PizzaIngredients> unwantedIngredients = recPizza.ingredients.Except(_orderedPizza.ingredients).ToList();

            decimal review = GameplayHelper.CalculateStarsReviewForOrder(missingIngredients, unwantedIngredients);
            _gameManager.AddReviewToGameScore(review);
            
            // _customerNameText.text = review.ToString();
            // TODO
        }

        private void Update()
        {
            _customerNameText.transform.LookAt(_cameraTransform.position);
            _customerNameText.transform.Rotate(0f, 180f, 0f);

            if (_testCompare)
            {
                CompareReceivedPizzaWithOrder(new PizzaType() { pizzaName = "Name", ingredients = new List<PizzaIngredients>()
                    {
                        PizzaIngredients.Mozzarella,
                        PizzaIngredients.Egg,
                        PizzaIngredients.Bacon,
                        PizzaIngredients.TomatoSauce
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