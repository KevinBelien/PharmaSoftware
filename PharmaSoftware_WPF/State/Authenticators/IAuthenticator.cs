using PharmaSoftware_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_WPF.State.Authenticators
{
    public interface IAuthenticator
    {
        Pharmacy CurrentUser { get; set; }
        bool isLoggedIn { get; }

        void LogOut();

    }
}
