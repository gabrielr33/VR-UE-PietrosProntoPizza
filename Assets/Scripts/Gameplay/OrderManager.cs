using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Gameplay
{
    public class OrderManager : MonoBehaviour
    {
        [SerializeField] private List<Table> _tables;

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
                    break;
                }
            }
        }

        private void SpawnNewOrderPostIt(List<Order> orders)
        {
            foreach (Order order in orders)
            {
                Debug.Log($"{order.TableNumber} - {order.CustomerName}: {order.Pizza.pizzaName}");
            }
        }
    }
}
