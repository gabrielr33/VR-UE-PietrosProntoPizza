using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Gameplay
{
    public class Seat : MonoBehaviour
    {
        [SerializeField] private PrefabsManager _prefabsManager;
        [SerializeField] private Transform _spawnRootPos;
        
        public Order SpawnCustomer(int tableNumber)
        {
            Random rand = new Random();

            List<GameObject> customerPrefabs = _prefabsManager.CustomerPrefabs;
            GameObject customer = customerPrefabs[rand.Next(customerPrefabs.Count)];

            Customer newCustomer = Instantiate(customer, _spawnRootPos.position, transform.rotation).GetComponent<Customer>();
            newCustomer.SelectCustomerName();
            newCustomer.SetAnimatorControllerState(CustomerAnimationState.Idle);
            
            return newCustomer.GenerateOrder(_prefabsManager, tableNumber);
        }
    }
}
