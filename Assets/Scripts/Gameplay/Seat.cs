using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Gameplay
{
    public class Seat : MonoBehaviour
    {
        [SerializeField] private PrefabsManager _prefabsManager;
        [SerializeField] private Transform _spawnRootPos;
        [SerializeField] private PizzaSeatTrigger _pizzaTrigger;

        private Customer _customer;
        
        public Order SpawnCustomer(int tableNumber)
        {
            Random rand = new Random();

            List<Customer> customerPrefabs = _prefabsManager.CustomerPrefabs;

            _customer = Instantiate(customerPrefabs[rand.Next(customerPrefabs.Count)], _spawnRootPos.position, transform.rotation);
            _customer.SelectCustomerName();
            _customer.SetAnimatorControllerState(CustomerAnimationState.Talking);
            
            _pizzaTrigger.gameObject.SetActive(true);
            
            return _customer.GenerateOrder(_prefabsManager, tableNumber);
        }

        public void PizzaReceived(Pizza pizza)
        {
            _customer.CompareReceivedPizzaWithOrder(pizza);
        }
    }
}
