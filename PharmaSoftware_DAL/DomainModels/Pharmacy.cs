using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_DAL.DomainModels
{
    [Table("Pharmacy")]
    public class Pharmacy
    {
        public int PharmacyID { get; set; }

        [Required(ErrorMessage = "Username is een verplicht veld")]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required(ErrorMessage = "Stad is een verplicht veld")]
        public string City { get; set; }

        [Required(ErrorMessage = "Postcode is een verplicht veld")]
        [MaxLength(10)]
        public string ZIP { get; set; }

        [Required(ErrorMessage = "Straat is een verplicht veld")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Huisnummer is een verplicht veld")]
        [MaxLength(10)]
        public string HouseNr { get; set; }

        [Required]
        public int PhoneNr { get; set; }

        public string District { get; set; }

        //NavigationProperties
        public ICollection<PharmacyProduct> PharmacyProducts { get; set; }

        [InverseProperty("PharmacyBuy")]
        public virtual ICollection<OrderIntern> InternBought { get; set; }

        [InverseProperty("PharmacySell")]
        public virtual ICollection<OrderIntern> InternSold { get; set; }

    }
}
