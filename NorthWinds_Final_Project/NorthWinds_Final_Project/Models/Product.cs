using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;
using System.Linq;



namespace NorthWinds_Final_Project.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(100)]
        [Required(ErrorMessage = "Product Name Must Be Entered. Edit Operation Canceled")]
        public string ProductName { get; set; }
        [MaxLength(1000)]
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? WholeSalePrice { get; set; }
        public Int16? UnitsInStock { get; set; }
        public Int16? UnitsOnOrder { get; set; }
        public Int16? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Supplier Supplier { get; set; }


       
    }

}
