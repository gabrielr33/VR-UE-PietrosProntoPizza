using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Gameplay
{
    public class OrderManager : MonoBehaviour
    {
        [SerializeField] private List<Table> _tables;
        [SerializeField] private OrderSheetPizza _orderSheetPizzaPrefab;
        [SerializeField] private OrderSheetDrink _orderSheetDrinkPrefab;
        [SerializeField] private Transform _orderSheetPizzaTransform;
        [SerializeField] private Transform _orderSheetDrinkTransform;

        private void Start()
        {
            // TODO temporary
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
                    SpawnNewOrderPostIt(orders);
                    //break;
                }
            }
        }

        private void SpawnNewOrderPostIt(List<Order> orders)
        {
            foreach (Order order in orders)
            {
                OrderSheetPizza sheetPizza = Instantiate(_orderSheetPizzaPrefab, _orderSheetPizzaTransform);
                sheetPizza.SetUp(order);
                
                OrderSheetDrink sheetDrink = Instantiate(_orderSheetDrinkPrefab, _orderSheetDrinkTransform);
                sheetDrink.SetUp(order);
            }
        }
    }
}
