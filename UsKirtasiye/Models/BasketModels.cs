using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsKirtasiye.Models
{
    public class BasketModels
    {
        public BasketModels()
        {
            this.Products = new Dictionary<int, int>();
        }

        public Dictionary<int, int> Products { get; set; }//ürün id ve ürün sayısı
    }
}