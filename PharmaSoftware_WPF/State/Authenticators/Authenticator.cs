using PharmaSoftware_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaSoftware_WPF.State.Authenticators
{
    public class Authenticator : IAuthenticator
    {

        public Pharmacy CurrentUser { get; set; }

        public bool isLoggedIn => CurrentUser != null;
        public void LogOut()
        {
            CurrentUser = null;
        }

    }
}
