using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_DAL.DomainModels
{
    [Table("ProductCategory")]
    public class ProductCategory
    {
        public int ProductCategoryID { get; set; }

        [Required]
        public string Name { get; set; }

        //NavigationProperties
        public ICollection<Product> Products { get; set; }
        public ICollection<ProductSubcategory> Subcategories { get; set; }

    }
}
