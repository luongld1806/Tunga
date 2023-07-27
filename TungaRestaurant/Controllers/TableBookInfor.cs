using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TungaRestaurant.Models;

namespace TungaRestaurant.Controllers
{
    public class TableBookInfor
    {
        [Required(ErrorMessage ="Requied")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Requied")]
        public string date { get; set; }
        [Required(ErrorMessage = "Requied")]
        public string time { get; set; }
        [Required(ErrorMessage = "Requied")]
        public string time_to { get; set; }
        [Required(ErrorMessage = "Requied")]
        public string phone { get; set; }
     
        [Required(ErrorMessage = "Requied")]
        public int numberOfGuest { get; set; }
        [Required(ErrorMessage = "Requied")]
        public int TableId { get; set; }

        public string message { get; set; }

    }
}
