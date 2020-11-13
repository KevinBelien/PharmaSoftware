using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_DAL.DomainModels
{
    [Table("PharmacyProduct")]
    public class PharmacyProduct
    {
        public int PharmacyProductID { get; set; }

        [Required]
        public int QtyInStorage { get; set; }

        public int? QtyOrdered { get; set; }

        [Required]
        public DateTime DateOfOrder { get; set; }

        public DateTime? DateOfArrival { get; set; }

        //NavigationProperties
        [Index("IX_Product_Pharmacy", 1, IsUnique = true)]
        public int PharmacyID { get; set; }
        public Pharmacy Pharmacy { get; set; }

        [Index("IX_Product_Pharmacy", 2, IsUnique = true)]
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
