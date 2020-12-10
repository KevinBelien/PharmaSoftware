using PharmaSoftware_DAL.Partials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_DAL
{
    public partial class PharmacyProduct
    {
        [NotMapped]
        public bool IsSelected { get; set; }

        public override bool Equals(object obj)
        {
            return obj is PharmacyProduct product &&
                   PharmacyID == product.PharmacyID &&
                   ProductID == product.ProductID;
        }

        public override int GetHashCode()
        {
            int hashCode = 683625434;
            hashCode = hashCode * -1521134295 + PharmacyID.GetHashCode();
            hashCode = hashCode * -1521134295 + ProductID.GetHashCode();
            return hashCode;
        }

    }
}
