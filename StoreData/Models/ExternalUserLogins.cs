using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StoreData.Models
{
    public class ExternalUserLogins
    {
        [Key, Required]
        public int Id { get; set; }

        [Required, ForeignKey(nameof(Users))]
        public int UserId { get; set; }

        public virtual Users Users { get; set; }

        [Required]
        public string LoginProvider { get; set; }

        [Required]
        public string ProviderKey { get; set; }

         
        public string ProviderDisplayName { get; set; }

    }
}
