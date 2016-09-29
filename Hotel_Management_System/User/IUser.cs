using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
    namespace pUser
    {
        interface IUser
        {
            //base method to check if user is logged in
            bool isLogin();

            //base method to sign in the user
            bool signIn();

            //base method to logout the user
            bool Logout();

            //base method to Registered the user
            bool isRegistered();

        }
    }

}
