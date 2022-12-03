using System.Collections.Generic;
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

        private Transform _cameraTransform;
        
        private Animator _customerAnimator;
        private static readonly int CustomerState = Animator.StringToHash("CustomerState");

        private void Awake()
        {
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
                CustomerName = Name
            };

            return order;
        }

        public void SetAnimatorControllerState(CustomerAnimationState state)
        {
            if (_customerAnimator != null)
            {
                switch (state)
                {
                    case CustomerAnimationState.Idle:
                        _customerAnimator.SetFloat(CustomerState, 0.5f);
                        break;
                    case CustomerAnimationState.Mad:
                        _customerAnimator.SetFloat(CustomerState, 1f);
                        break;
                    case CustomerAnimationState.Happy:
                        _customerAnimator.SetFloat(CustomerState, 0f);
                        break;
                }
            }
        }

        private void Update()
        {
            _customerNameText.transform.LookAt(_cameraTransform.position);
            _customerNameText.transform.Rotate(0f, 180f, 0f);
        }
    }

    public enum CustomerAnimationState
    {
        Idle,
        Mad,
        Happy
    }
}