namespace Gameplay
{
    public class Order
    {
        public int TableNumber { get; set; }
        public string CustomerName { get; set; }
        public PizzaType Pizza { get; set; }
        public int MaxWaitTimeInSec { get; set; }
    }
}
