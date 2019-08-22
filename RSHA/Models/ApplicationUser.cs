using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RSHA.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Car license plate")]
        public string CarLicensePlate { get; set; }

        [Display(Name = "Car model")]
        public string CarModel { get; set; }

        [NotMapped]
        public bool IsMechanic { get; set; }

        [NotMapped]
        public bool IsAdmin { get; set; }
    }
}
