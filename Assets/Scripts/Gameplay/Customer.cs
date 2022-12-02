using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace Gameplay
{
    public class Customer : MonoBehaviour
    {
        [field: SerializeField] public bool Female { get; set; }
        [field: SerializeField] public string Name { get; set; }

        [SerializeField] private TMP_Text _customerNameText;
        [SerializeField] private Animator _customerAnimator;
        [SerializeField] private PizzaType _orderedPizza;

        private Transform _cameraTransform;

        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
        }

        public void SelectCustomerName()
        {
            List<string> customerNames = Female ? IOFileManager.ReadCustomerNamesFemales() : IOFileManager.ReadCustomerNamesMales();
            
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

        private void Update()
        {
            _customerNameText.transform.LookAt(_cameraTransform.position);
            _customerNameText.transform.Rotate(0f, 180f, 0f);
        }
    }
}
