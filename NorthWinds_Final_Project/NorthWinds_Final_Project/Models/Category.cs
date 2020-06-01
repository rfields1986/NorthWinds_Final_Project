using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NorthWinds_Final_Project.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "No Name Entered, Category Must Have A Name")]
        [MaxLength(50, ErrorMessage = "Max Character Length Is 50")]
        public string CategoryName { get; set; }


        [Required(ErrorMessage = "No Description Entered, Category Must Have A Description")]
        [MaxLength(250, ErrorMessage = "Max Character Length Is 250")]
        public string Description { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}