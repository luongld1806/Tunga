namespace TungaRestaurant.Models
{
    public class CategoryDetail
    {
        public int CategoryId { get; set; }
        public int FoodId { get; set; }
        public Category Category { get; set; }
        public Food Food { get; set; }
    }
}
