using PharmaSoftware_DAL.Partials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_DAL
{
    public partial class Pharmacy
    {
        public override bool Equals(object obj)
        {
            return obj is Pharmacy pharmacy &&
                   Username == pharmacy.Username;
        }

        public override int GetHashCode()
        {
            return -182246463 + EqualityComparer<string>.Default.GetHashCode(Username);
        }
    }
}
