using System;
using System.Collections.Generic;
using System.Text;

namespace StoreData.Models
{
    public class ImageUrls
    {
        public int Id { get; set; }
        public virtual Items Item{ get; set; }
        public string Url { get; set; }

    }
}
