using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TungaRestaurant.Models
{
    public class Table
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Name {get;set;}
        
        [ForeignKey("Room")]
        [DisplayName("Room")]
        [Required(ErrorMessage = "Required")]
        public int RoomId {get;set;}
        [DisplayName("Max Guest")]
        [Required(ErrorMessage = "Required")]
        public int NumberOfGuest {get;set;}
        [Required(ErrorMessage = "Required")]
        public TableStatus Status {get;set;}

        public Branch Branch {get;set;} 
        public Room Room {get;set;}
    }
}
