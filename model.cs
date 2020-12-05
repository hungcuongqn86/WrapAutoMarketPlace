using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrapAutoMarketPlace
{
    class model
    {
    }

    public class User
    {
        public string email { get; set; }
        public string password { get; set; }

        public User() { }
        public User(string email, string pass)
        {
            this.email = email;
            this.password = pass;
        }
    }

    public class ResLogin
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
