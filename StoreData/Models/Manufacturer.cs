using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StoreData.Models
{
    public class Manufacturer
    {
        public int Id { get; set; }
        public virtual Items Items { get; set; }

        [Required]
        [MaxLength(32), MinLength(4)]
        public string Name { get; set; }


    }
}
