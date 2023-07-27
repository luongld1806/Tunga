using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TungaRestaurant.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name {get; set;}
        public string Image {get; set;}
        public int Status { get; set; }
        public virtual ICollection<Food> Foods { get; set; }
    }
}
