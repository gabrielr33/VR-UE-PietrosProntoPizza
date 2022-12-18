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

        private Table _table;
        private Customer _customer;

        public Order SpawnCustomer(Table table, OrderManager orderManager)
        {
            _table = table;
            
            Random rand = new Random();

            List<Customer> customerPrefabs = _prefabsManager.CustomerPrefabs;
            string prefabName = customerPrefabs[rand.Next(customerPrefabs.Count)].name;

            _customer = PhotonNetwork.Instantiate(Path.Combine("Prefabs\\Customers", prefabName), _spawnRootPos.position, transform.rotation).GetComponent<Customer>();
            _customer.SelectCustomerName();
            _customer.SetAnimatorControllerState(CustomerAnimationState.Idle);

            _pizzaTrigger.gameObject.SetActive(true);
            _drinkTrigger.gameObject.SetActive(true);
            _pizzaTrigger.GetComponent<BoxCollider>().enabled = true;
            _pizzaTrigger.GetComponent<MeshRenderer>().enabled = true;
            _drinkTrigger.GetComponent<BoxCollider>().enabled = true;
            _drinkTrigger.GetComponent<MeshRenderer>().enabled = true;

            return _customer.GenerateOrder(_prefabsManager, orderManager, table.TableNumber, this);
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
            _pizzaTrigger.ClearPizzaFromTrigger();
            _drinkTrigger.ClearDrinkFromTrigger();
            _pizzaTrigger.gameObject.SetActive(false);
            _drinkTrigger.gameObject.SetActive(false);
            IsOccupied = false;
            _table.CustomerLeft();
        }

        public void RemoveCustomer()
        {
            if (_customer != null)
                PhotonNetwork.Destroy(_customer.gameObject);
        }
    }
}
