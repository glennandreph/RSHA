using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RSHA.Models
{
    public class Mechanics
    {
        public int Id { get; set; }

        [Display(Name = "Application-user bound to mechanic workshop")]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [Display(Name ="Postal Code")]
        public int PostalCode { get; set; }
        [Required]
        public string State { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

    }
}