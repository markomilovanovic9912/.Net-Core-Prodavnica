using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StoreData.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(32), MinLength(4)]
        public string Method { get; set; }

    }
}
