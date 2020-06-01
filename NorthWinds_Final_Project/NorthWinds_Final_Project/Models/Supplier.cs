using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;


namespace NorthWinds_Final_Project.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(50, ErrorMessage = "Max Character Length Is 50")]
        [Required(ErrorMessage = "A Company Name Must Be Entered")]
        public string CompanyName { get; set; }
        [MaxLength(50, ErrorMessage = "Max Character Length Is 50")]
        [Required(ErrorMessage = "A Contact Name Must Be Entered")]
        public string ContactName { get; set; }
        [MaxLength(50,ErrorMessage = "Max Character Length Is 50")]
        public string ContactTitle { get; set; }
        [MaxLength(50, ErrorMessage = "Max Character Length Is 50")]
        public string Address { get; set; }
        [MaxLength(50, ErrorMessage = "Max Character Length Is 50")]
        public string City { get; set; }
        [MaxLength(50, ErrorMessage = "Max Character Length Is 50")]
        public string Region { get; set; }
        [MaxLength(50, ErrorMessage = "Max Character Length Is 50")]
        public string PostalCode { get; set; }
        [MaxLength(50, ErrorMessage = "Max Character Length Is 50")]
        public string Country { get; set; }
        [MaxLength(50, ErrorMessage = "Max Character Length Is 50")]
        public string Phone { get; set; }
        [MaxLength(50, ErrorMessage = "Max Character Length Is 50")]
        public string Fax { get; set; }


        public virtual List<Product> Products { get; set; }
    }
}
