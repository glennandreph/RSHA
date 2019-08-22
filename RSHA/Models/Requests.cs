using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RSHA.Models
{
    public class Requests
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone number")]
        public int PhoneNumber { get; set; }

        public string Location { get; set; }

        [Display(Name = "Car license plate")]
        public string CarLicensePlate { get; set; }

        [Display(Name = "Car model")]
        public string CarModel { get; set; }

        public int ProblemId { get; set; }
        [ForeignKey("ProblemId")]
        public virtual ProblemTypes ProblemTypes { get; set; }

        public string Message { get; set; }

        public DateTime RequestCreated { get; set; }
        [Required]
        public DateTime RequestScheduledDate { get; set; }
        [Required]
        [NotMapped]
        public DateTime RequestScheduledTime { get; set; }

        public bool Completed { get; set; } = false;

        public bool AcceptedByMechanic { get; set; }

        [Display(Name = "Mechanic assigned")]
        public int MechanicAssigned { get; set; }
        [ForeignKey("MechanicAssigned")]
        public virtual Mechanics Mechanics { get; set; }

        [Display(Name = "Customer Id")]
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
