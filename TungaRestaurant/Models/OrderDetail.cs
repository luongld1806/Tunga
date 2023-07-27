using System.ComponentModel.DataAnnotations.Schema;

namespace TungaRestaurant.Models
{
    public class OrderDetail
    {
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [ForeignKey("Food")]
        public int FoodId { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public Order Order { get; set; }
        public Food Food { get; set; }

    }
}
