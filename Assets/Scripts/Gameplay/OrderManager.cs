using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

namespace Gameplay
{
    public class OrderManager : MonoBehaviourPun
    {
        [SerializeField] private List<Table> _tables;
        [SerializeField] private OrderSheetPizza _orderSheetPizzaPrefab;
        [SerializeField] private OrderSheetDrink _orderSheetDrinkPrefab;
        [SerializeField] private Transform _orderSheetPizzaTransform;
        [SerializeField] private Transform _orderSheetDrinkTransform;
        [SerializeField] private PrefabsManager _prefabsManager;

        private void Start()
        {
            // TODO temporary
            if (PhotonNetwork.IsMasterClient)
                SpawnNewCustomers();
        }

        public void SpawnNewCustomers()
        {
            Random rand = new Random();

            // Shuffle seats list
            _tables = _tables.OrderBy(a => rand.Next()).ToList();

            foreach (Table table in _tables)
            {
                if (!table.IsOccupied)
                {
                    List<Order> orders = table.SpawnNewCustomers();

                    foreach (Order order in orders)
                        photonView.RPC("SpawnNewOrderPostIt", RpcTarget.All, order.TableNumber, order.CustomerName,
                            _prefabsManager.PizzaTypes.IndexOf(order.Pizza),
                            _prefabsManager.DrinkTypes.IndexOf(order.Drink), order.MaxWaitTimeInSec);

                    break;
                }
            }
        }

        [PunRPC]
        private void SpawnNewOrderPostIt(int tableNumber, string customerName, int pizzaIndex, int drinkIndex,
            int maxWaitTime)
        {
            Order order = new Order()
            {
                TableNumber = tableNumber,
                CustomerName = customerName,
                Pizza = _prefabsManager.PizzaTypes[pizzaIndex],
                Drink = _prefabsManager.DrinkTypes[drinkIndex],
                MaxWaitTimeInSec = maxWaitTime
            };

            OrderSheetPizza sheetPizza = Instantiate(_orderSheetPizzaPrefab, _orderSheetPizzaTransform);
            sheetPizza.SetUp(order);

            OrderSheetDrink sheetDrink = Instantiate(_orderSheetDrinkPrefab, _orderSheetDrinkTransform);
            sheetDrink.SetUp(order);
        }
    }
}