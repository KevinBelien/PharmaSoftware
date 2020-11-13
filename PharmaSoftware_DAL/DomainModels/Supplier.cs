using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_DAL.DomainModels
{
    [Table("Supplier")]
    public class Supplier
    {
        public int SupplierID { get; set; }

        [Required(ErrorMessage = "Leveranciernaam is een verplicht veld")]
        public string Name { get; set; }

        public string Mail { get; set; }

        public int? PhoneNr { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        [Required(ErrorMessage = "Postcode is een verplicht veld")]
        [MaxLength(10)]
        public string ZIP { get; set; }

        [Required(ErrorMessage = "Straat is een verplicht veld")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Huisnummer is een verplicht veld")]
        [MaxLength(10)]
        public string HouseNr { get; set; }

        public string Website { get; set; }

        //Navigationproperties
        public ICollection<Product> Products { get; set; }
    }
}
