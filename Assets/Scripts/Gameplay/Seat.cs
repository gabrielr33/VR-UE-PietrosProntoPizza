using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

namespace Gameplay
{
    public class Seat : MonoBehaviour
    {
        public bool IsOccupied { get; private set; }

        [SerializeField] private PrefabsManager _prefabsManager;
        [SerializeField] private Transform _spawnRootPos;
        [SerializeField] private PizzaSeatTrigger _pizzaTrigger;
        [SerializeField] private DrinkSeatTrigger _drinkTrigger;

        private Customer _customer;

        public Order SpawnCustomer(int tableNumber, OrderManager orderManager)
        {
            Random rand = new Random();

            List<Customer> customerPrefabs = _prefabsManager.CustomerPrefabs;
            string prefabName = customerPrefabs[rand.Next(customerPrefabs.Count)].name;

            _customer = PhotonNetwork.Instantiate(Path.Combine("Prefabs\\Customers", prefabName), _spawnRootPos.position, transform.rotation).GetComponent<Customer>();
            _customer.SelectCustomerName();
            _customer.SetAnimatorControllerState(CustomerAnimationState.Talking);

            _pizzaTrigger.gameObject.SetActive(true);
            _drinkTrigger.gameObject.SetActive(true);

            return _customer.GenerateOrder(_prefabsManager, orderManager, tableNumber, this);
        }

        public void PizzaReceived(Pizza pizza)
        {
            _pizzaTrigger.GetComponent<BoxCollider>().enabled = false;
            _pizzaTrigger.GetComponent<MeshRenderer>().enabled = false;
            _drinkTrigger.GetComponent<BoxCollider>().enabled = false;
            _drinkTrigger.GetComponent<MeshRenderer>().enabled = false;
            _customer.CompareReceivedPizzaWithOrder(pizza);
        }

        public void DrinkReceived(Drink drink)
        {
            _customer.DrinkReceived(drink);
            _drinkTrigger.GetComponent<BoxCollider>().enabled = false;
            _drinkTrigger.GetComponent<MeshRenderer>().enabled = false;
        }

        public void FinishedEating()
        {
            _pizzaTrigger.gameObject.SetActive(false);
            _drinkTrigger.gameObject.SetActive(false);
            _pizzaTrigger.ClearPizzaFromTrigger();
            _drinkTrigger.ClearDrinkFromTrigger();
            _customer = null;
            IsOccupied = false;
        }
    }
}
