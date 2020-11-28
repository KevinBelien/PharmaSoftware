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
    [Table("Pharmacy")]
    public partial class Pharmacy: Baseclass
    {
        public int PharmacyID { get; set; }

        [Required(ErrorMessage = "Username is een verplicht veld")]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Stad is een verplicht veld")]
        public string City { get; set; }

        [Required(ErrorMessage = "Postcode is een verplicht veld")]
        [MaxLength(10,ErrorMessage = "Postcode mag maximaal 10 karakters bevatten")]
        public string ZIP { get; set; }

        [Required(ErrorMessage = "Straat is een verplicht veld")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Huisnummer is een verplicht veld")]
        [MaxLength(10,ErrorMessage = "Postcode mag maximaal 10 karakters bevatten")]
        public string HouseNr { get; set; }

        [Required(ErrorMessage = "Telefoonnummer moet ingevuld zijn!")]
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
