using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Database_Package;

namespace Bussiness
{
    namespace pUser
    {
        class User : IUser
        {
            //user attributes
            //protected because can be used in child class 
            protected string _fname;
            protected string _lname;
            protected string _address;
            protected string _telephone;
            protected string _employeeType;
            protected string _userCat;
            protected string _userID;
            protected string _username;
            protected string _password;
            // private List<string> listUser = new List<string>();
            protected DatabaseAccess db = new DatabaseAccess();

            //user list
            public List<string> getUserList()
            {
                return db.getUserList();
            }

            //user id
            public string getUserID()
            {
                string uid = db.getUserID(this._username, this._password);
                return uid;
            }

            //Get and Set Method for room price
            public string userID
            {
                get
                {
                    return this._userID;
                }
                set
                {
                    this._userID = value;
                }
            }

            //fname, lname, address, telephone, employeeType, userCat
            //Get and Set Method for room price
            public string firstName
            {
                get
                {
                    return this._fname;
                }
                set
                {
                    this._fname = value;
                }
            }

            //lname, address, telephone, employeeType, userCat
            //Get and Set Method for room price
            public string lastName
            {
                get
                {
                    return this._lname;
                }
                set
                {
                    this._lname = value;
                }
            }

            //address, telephone, employeeType, userCat
            //Get and Set Method for room price
            public string address
            {
                get
                {
                    return this._address;
                }
                set
                {
                    this._address = value;
                }
            }

            //telephone, employeeType, userCat
            //Get and Set Method for room price
            public string telephone
            {
                get
                {
                    return this._telephone;
                }
                set
                {
                    this._telephone = value;
                }
            }

            //employeeType, userCat
            //Get and Set Method for room price
            public string employeeType
            {
                get
                {
                    return this._employeeType;
                }
                set
                {
                    this._employeeType = value;
                }
            }

            //userCat
            //Get and Set Method for room price
            public string userCat
            {
                get
                {
                    return this._userCat;
                }
                set
                {
                    this._userCat = value;
                }
            }

            //Get and Set Method for UserName
            public string username
            {
                get
                {
                    return this._username;
                }
                set
                {
                    this._username = value;
                }
            }

            //Get and Set Method for password
            public string password
            {
                get
                {
                    return this._password;
                }
                set
                {
                    this._password = value;
                }
            }

            //Override Method to check if user logged in  
            public bool isLogin()
            {
                if (_username != "" && _password != "")
                {
                    return true;
                }
                else
                    return false;
            }

            //Override Method to Sign in the user
            public bool signIn()
            {
                if (db.signIn(_username, _password))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            //Override Method to logout the user
            public bool Logout()
            {
                this._username = "";
                this._password = "";
                this._userID = "";
                if (_username == "" && _password == "" && _userID == "")
                {
                    return true;
                }
                else
                    return false;
            }

            //Override Method to Register the user
            public bool isRegistered()
            {
                if (db.isRegister(_username, _password, _fname, _lname, _address, _telephone, _employeeType, _userCat))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }

}
