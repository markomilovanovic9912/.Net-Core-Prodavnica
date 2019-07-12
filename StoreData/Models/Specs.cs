using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StoreData.Models
{
    public class Specs
    {
        public int Id { get; set; }


        /*[Required, ForeignKey(nameof(Model))]
        public int ModelId { get; set; }*/
        public virtual Model Model { get; set; }

        [Required]
        [MaxLength(9086), MinLength(4)]
        public string Specification { get; set; }


        [Required]
        [MaxLength(9086), MinLength(4)]
        public string Description { get; set; }
    }
}
