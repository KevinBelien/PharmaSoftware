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
    [Table("ProductPreparation")]
    public class ProductPreparation : Baseclass
    {
        public int ProductPreparationID { get; set; }

        [Required]
        public string Name { get; set; }

        //NavigationProperties
        public ICollection<Product> Products { get; set; }
    }
}
