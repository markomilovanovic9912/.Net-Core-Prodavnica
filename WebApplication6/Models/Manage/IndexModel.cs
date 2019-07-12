using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models.Manage
{
    public class IndexModel
    {   
        public int Id { get; set; }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [MaxLength(32)]
        public string FirstName { get; set; }

        [MaxLength(32)]
        public string LastName { get; set; }

        [MaxLength(32)]
        public string Adress { get; set; }

        [MaxLength(32)]
        public string City { get; set; }

        [MaxLength(32)]
        public string CountryOrState { get; set; }


        public string StatusMessage { get; set; }
    }
}
