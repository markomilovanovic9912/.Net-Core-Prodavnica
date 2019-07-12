using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StoreData.Models
{
    public class Reviews
    {
        public int Id { get; set; }
        public virtual Items Item { get; set; }

        [Required(ErrorMessage ="This field is required!")]
        [MaxLength (1024), MinLength(4)]
        public string Text { get; set; }

        [Range(0, 5, ErrorMessage = "Value must be between 0.0 and 5.0")]
        public double Rating { get; set; }

        public DateTime DayOfReview { get; set; }
         
        public virtual Users Users { get; set; }


    }
}
