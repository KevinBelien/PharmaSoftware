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
    [Table("Product")]
    public class Product: Baseclass
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Code is een verplicht veld")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Productnaam is een verplicht veld")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Barcode is een verplicht veld")]
        public int Barcode { get; set; }

        [Required(ErrorMessage = "Inhoud is een verplicht veld")]
        [MaxLength(20)]
        public string Content { get; set; }

        [Required(ErrorMessage = "Productprijs is een verplicht veld")]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Kostprijs is een verplicht veld")]
        [Column(TypeName = "money")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Merk is een verplicht veld")]
        public int Brand { get; set; }

        //Navigation properties
        public ICollection<PharmacyProduct> PharmacyProducts { get; set; }
        public ICollection<Product> OrdersIntern { get; set; }

        public int ProductCategoryID { get; set; }
        public ProductCategory ProductCategory { get; set; }

        public int ProductPreparationID { get; set; }
        public ProductPreparation ProductPreparation { get; set; }

        public int SupplierID { get; set; }
        public Supplier Supplier { get; set; }
    }
}
