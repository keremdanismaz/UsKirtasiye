using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsKirtasiye.Models
{
    public class ProductModel
    {
        public List<DB.Products> Products { get; set; }
        public DB.Categories Categories { get; set; }
    }
}