using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_DAL.Services.HashingServices
{
    public interface IHashingService
    {
        string EncryptString(string encr);
        string DecryptString(string decr);
    }
}
