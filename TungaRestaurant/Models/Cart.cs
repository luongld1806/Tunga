using System.ComponentModel.DataAnnotations.Schema;

namespace TungaRestaurant.Models
{
    public class Cart
    {
        public int Id { get; set; }
        [ForeignKey("Food")]
        public int FoodId { get; set; }
        [ForeignKey("UserInfo")]
        public string UserInfoId { get; set; }
        public int Quantity { get; set; }
        public Food Food { get; set; }
        public UserInfo UserInfo { get; set; }

    }
}
