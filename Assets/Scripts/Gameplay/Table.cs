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

        public List<Order> SpawnNewCustomers()
        {
            List<Order> orders = new List<Order>();
            Random rand = new Random();
            
            // Shuffle seats list
            _seats = _seats.OrderBy(a => rand.Next()).ToList();
            
            // Spawn n new customers
            int numberOfNewCustomers = rand.Next(2, _seats.Count + 1);
            for (int i = 0; i < numberOfNewCustomers; i++)
                orders.Add(_seats[i].SpawnCustomer(TableNumber));
            
            _audioSource.Play();
            IsOccupied = true;
            return orders;
        }
    }
}
