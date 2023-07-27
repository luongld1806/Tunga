using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TungaRestaurant.Models
{

    public class Branch
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Required")]

        public BranchStatus Status { get; set; }
        public virtual List<UserInfo> Users { get; set; }
        public virtual List<Food> Foods { get; set; }
        public virtual List<Room> Rooms { get; set; }
    }
}
