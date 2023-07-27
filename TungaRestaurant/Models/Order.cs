using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TungaRestaurant.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public  DateTime CreatedAt { get; set; }
        public string ShipAddress { get; set; }
        public OrderStatus Status { get; set; }
        [ForeignKey("User")]
        public string UserInfoId { get; set; }

        public UserInfo User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();
    }
}
