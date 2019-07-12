using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StoreData.Models
{
    public class UserClaims
    {
        [Key, Required]
        public int Id { get; set; }


        [Required]
        [MaxLength(32), MinLength(2)]
        public string ClaimType { get; set; }


        [Required]
        [MaxLength(32), MinLength(2)]
        public string ClaimValue { get; set; }

        [Required, ForeignKey(nameof(Users))]
        public int UserId { get; set; }

        public virtual Users Users { get; set; }





    }
}
