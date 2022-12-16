using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Gameplay
{
    public class Table : MonoBehaviour
    {
        [field: SerializeField] public int TableNumber { get; private set; }
        public bool IsOccupied { get; private set; }

        [SerializeField]  private AudioSource _audioSource;
        [SerializeField] private List<Seat> _seats;

        private int _numberOfNewCustomers;
        
        public List<Order> SpawnNewCustomers(OrderManager orderManager)
        {
            List<Order> orders = new List<Order>();
            Random rand = new Random();
            
            // Shuffle seats list
            _seats = _seats.OrderBy(a => rand.Next()).ToList();
            
            // Spawn n new customers
           _numberOfNewCustomers = rand.Next(2, _seats.Count + 1);
            for (int i = 0; i < _numberOfNewCustomers; i++)
            {
                if (!_seats[i].IsOccupied)
                    orders.Add(_seats[i].SpawnCustomer(this, orderManager));
            }

            _audioSource.Play();
            IsOccupied = true;
            return orders;
        }

        public void CustomerLeft()
        {
            if (_numberOfNewCustomers <= 0)
                return;
            
            if (--_numberOfNewCustomers <= 0)
            {
                IsOccupied = false;
            }
        }

        public void RemoveAllCustomers()
        {
            foreach (Seat seat in _seats)
                seat.RemoveCustomer();
        }
    }
}
