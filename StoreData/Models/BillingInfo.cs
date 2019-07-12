using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StoreData.Models
{
    public  class BillingInfo
    {
        public int Id { get; set; }

        [Required, MaxLength(60)]
        public string Email { get; set; }

        [Required, MaxLength(20)]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required, MaxLength(60)]
        [Display(Name = "Adress")]
        public string Adress { get; set; }

        [Required, MaxLength(20)]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required, MaxLength(20)]
        [Display(Name = "Country/State")]
        public string CountryOrState { get; set; }

    }

}
