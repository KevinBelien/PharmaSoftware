using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_DAL
{
    public partial class PharmacyProduct
    {
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
