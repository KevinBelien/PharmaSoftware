using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_DAL.DomainModels
{
    [Table("OrderIntern")]
    public class OrderIntern
    {
        public int OrderInternID { get; set; }

        [Required]
        public DateTime DateOfOrder { get; set; }

        public DateTime? DateOfDelivery { get; set; }

        [Required (ErrorMessage="Hoeveeldheid is een verplicht veld")]
        public int Quantity { get; set; }

        [Required]
        public bool IsSend { get; set; }

        //NavigationProperties

        [Index("IX_Product_Pharmacies", 1, IsUnique = true)]
        public int ProductID { get; set; }
        public Product Product { get; set; }

        [Required]
        [Index("IX_Product_Pharmacies", 2, IsUnique = true)]
        public int PharmacyBuyID { get; set; }
        [ForeignKey("PharmacyBuyID")]
        public virtual Pharmacy PharmacyBuy { get; set; }

        [Required]
        [Index("IX_Product_Pharmacies", 3, IsUnique = true)]
        public int PharmacySellID { get; set; }
        [ForeignKey("PharmacySellID")]
        public virtual Pharmacy PharmacySell { get; set; }
    }
}
