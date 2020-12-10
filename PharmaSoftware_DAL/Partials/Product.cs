using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_DAL
{
    public partial class Product
    {

        public override bool Equals(object obj)
        {
            return obj is Product product &&
                   Code == product.Code;
        }

        public override int GetHashCode()
        {
            return -434485196 + EqualityComparer<string>.Default.GetHashCode(Code);
        }


    }
}
