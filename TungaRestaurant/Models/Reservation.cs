using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace TungaRestaurant.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        [DisplayName("Max Guest")]
        public int NumberOfGuest { get; set; }
        [DisplayName("Created Time")]
        public DateTime CreatedAt { get; set; }
        [DisplayName("Reserve At")]
        public DateTime ReservationAt { get; set; }
        [DisplayName("Reserve End")]
        public DateTime ReservationEnd { get; set; }
        public ReservationStatus Status { get; set; }
        [ForeignKey("User")]
        [DisplayName("Guest")]
        public string UserId { get; set; }
        public UserInfo User { get; set; }
        [ForeignKey("Table")]
        [DisplayName("Table")]
        public int TableId { get; set; }
        public Table Table { get; set; }

        public string Token { get; set; }
       
    }
}
