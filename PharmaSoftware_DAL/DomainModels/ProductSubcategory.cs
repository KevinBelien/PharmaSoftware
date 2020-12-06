using PharmaSoftware_DAL.Partials;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_DAL
{
    public class ProductSubcategory: Baseclass
    {
        public int ProductSubcategoryID { get; set; }

        [Required]
        public string Name { get; set; }

        //NavigationProperties
        public int ProductCategoryID { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
