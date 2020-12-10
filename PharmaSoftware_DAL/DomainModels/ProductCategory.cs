using PharmaSoftware_DAL;
using PharmaSoftware_DAL.Partials;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_DAL
{
    [Table("ProductCategory")]
    public partial class ProductCategory: Baseclass
    {
        public int ProductCategoryID { get; set; }

        [Required]
        public string Name { get; set; }

        //NavigationProperties
        public ICollection<Product> Products { get; set; }
        public ICollection<ProductSubcategory> Subcategories { get; set; }

    }
}
