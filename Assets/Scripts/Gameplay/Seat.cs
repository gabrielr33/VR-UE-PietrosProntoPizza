using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Gameplay
{
    public class Seat : MonoBehaviour
    {
        [SerializeField] private PrefabsManager _prefabsManager;
        [SerializeField] private Transform _spawnRootPos;
        [SerializeField] private GameObject _pizzaTrigger;
        
        public Order SpawnCustomer(int tableNumber)
        {
            Random rand = new Random();

            List<Customer> customerPrefabs = _prefabsManager.CustomerPrefabs;
            Customer customer = customerPrefabs[rand.Next(customerPrefabs.Count)];

            Customer newCustomer = Instantiate(customer, _spawnRootPos.position, transform.rotation);
            newCustomer.SelectCustomerName();
            newCustomer.SetAnimatorControllerState(CustomerAnimationState.Talking);
            
            _pizzaTrigger.SetActive(true);
            
            return newCustomer.GenerateOrder(_prefabsManager, tableNumber);
        }
    }
}
