using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TungaRestaurant.Models
{
    // Add profile data for application users by adding properties to the User class
    public class UserInfo : IdentityUser
    {
        [Required]
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        [Required]
        public Sex Sex { get; set; }
        [Required]
        public string Address {get;set;}
        public Boolean IsVegan { get; set; } = false;
        public UserStatus Status { get; set; } = UserStatus.NORMAL;
        [ForeignKey("Branch")]
        public int? BranchId { get; set; }
        public Branch Branch { get; set; }
        [ForeignKey("PreferBranch")]
        public int? PreferBranchId { get; set; }
        public Branch PreferBranch { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public static implicit operator List<object>(UserInfo v)
        {
            throw new NotImplementedException();
        }
    }
}
