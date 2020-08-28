using System;

namespace UsKirtasiye.Models.Dto
{
    public class ProductDetailDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public bool IsContinued { get; set; }

        public string Photo { get; set; }

        public int UnitsInStock { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int Category_Id { get; set; }
    }
}