using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TungaRestaurant.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [ForeignKey("Branch")]
        [DisplayName("Branch")]
        [Required(ErrorMessage = "Required")]
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
        public virtual List<Table> Tables { get; set; }
    }
}
