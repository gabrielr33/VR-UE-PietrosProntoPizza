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

        public void SpawnNewCustomers()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            Random rand = new Random();

            // Shuffle seats list
            _tables = _tables.OrderBy(a => rand.Next()).ToList();

            foreach (Table table in _tables)
            {
                if (!table.IsOccupied)
                {
                    List<Order> orders = table.SpawnNewCustomers(this);

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

        public void RemovePizzaOrderSheet(Order order)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            photonView.RPC("DestroyPizzaOrderSheet", RpcTarget.All, order.TableNumber, order.CustomerName,
                order.Pizza.pizzaName, order.Drink.drinkName, order.MaxWaitTimeInSec);
        }

        [PunRPC]
        private void DestroyPizzaOrderSheet(int tableNumber, string customerName, string pizzaName, string drinkName,
            int maxWaitTime)
        {
            foreach (OrderSheetPizza orderSheet in _orderSheetPizzaTransform.GetComponentsInChildren<OrderSheetPizza>())
            {
                if (orderSheet.Order.TableNumber == tableNumber &&
                    orderSheet.Order.CustomerName == customerName &&
                    orderSheet.Order.Pizza.pizzaName == pizzaName &&
                    orderSheet.Order.Drink.drinkName == drinkName &&
                    orderSheet.Order.MaxWaitTimeInSec == maxWaitTime)
                {
                    Destroy(orderSheet.gameObject);
                }
            }
        }

        public void RemoveDrinkOrderSheet(Order order)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            photonView.RPC("DestroyDrinkOrderSheet", RpcTarget.All, order.TableNumber, order.CustomerName,
                order.Pizza.pizzaName, order.Drink.drinkName, order.MaxWaitTimeInSec);
        }

        [PunRPC]
        private void DestroyDrinkOrderSheet(int tableNumber, string customerName, string pizzaName, string drinkName,
            int maxWaitTime)
        {
            foreach (OrderSheetDrink orderSheet in _orderSheetDrinkTransform.GetComponentsInChildren<OrderSheetDrink>())
            {
                if (orderSheet.Order.TableNumber == tableNumber &&
                    orderSheet.Order.CustomerName == customerName &&
                    orderSheet.Order.Pizza.pizzaName == pizzaName &&
                    orderSheet.Order.Drink.drinkName == drinkName &&
                    orderSheet.Order.MaxWaitTimeInSec == maxWaitTime)
                {
                    Destroy(orderSheet.gameObject);
                }
            }
        }

        public void RemoveAllOrdersAndCustomers()
        {
            photonView.RPC("RemoveOrdersAndCustomers", RpcTarget.All);
            
            foreach (Table table in _tables)
                table.RemoveAllCustomers();
        }

        [PunRPC]
        private void RemoveOrdersAndCustomers()
        {
            foreach (OrderSheetPizza orderSheet in _orderSheetPizzaTransform.GetComponentsInChildren<OrderSheetPizza>())
                Destroy(orderSheet.gameObject);

            foreach (OrderSheetDrink orderSheet in _orderSheetDrinkTransform.GetComponentsInChildren<OrderSheetDrink>())
                Destroy(orderSheet.gameObject);
        }
    }
}