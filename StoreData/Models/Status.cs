using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StoreData.Models
{
    public class Status
    {
        [Key, Required]
        public int Id { get; set; }


        [Required]
        [MaxLength(10), MinLength(4)]
        public string StatusText { get; set; }

    }
}
