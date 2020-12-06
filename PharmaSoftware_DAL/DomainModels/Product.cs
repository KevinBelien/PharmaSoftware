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
    public partial class Product: Baseclass
    {
        [Key]
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
        [MaxLength(150)]
        public string Brand { get; set; }

        //Navigation properties
        public ICollection<PharmacyProduct> PharmacyProducts { get; set; }
        public ICollection<OrderIntern> OrdersIntern { get; set; }

        [Required(ErrorMessage = "Selecteer een categorie")]
        public int ProductCategoryID { get; set; }
        public ProductCategory ProductCategory { get; set; }

        [Required(ErrorMessage = "Selecteer een subcategorie")]
        public int ProductSubcategoryID { get; set; }
        public ProductSubcategory ProductSubcategory { get; set; }

        public int? ProductPreparationID { get; set; }
        public ProductPreparation ProductPreparation { get; set; }

        [Required(ErrorMessage = "Selecteer een leverancier")]
        public int SupplierID { get; set; }
        public Supplier Supplier { get; set; }
    }
}
