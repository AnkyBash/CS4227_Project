using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database_Package;

namespace Bussiness
{
    namespace pBill
    {
        class Bill : IBill
        {
            private double _totalAmount;
            private string _billNumber;
            private DatabaseAccess da = new DatabaseAccess();

            //Get and Set Method for totalAmount
            public double totalAmount
            {
                get
                {
                    return this._totalAmount;
                }
                set
                {
                    this._totalAmount = value;
                }
            }

            //Get and Set Method for billNumber
            public string billNumber
            {
                get
                {
                    return this._billNumber;
                }
                set
                {
                    this._billNumber = value;
                }
            }

            //method To Get Current Bill
            public List<KeyValuePair<double, int>> getCurrentBill(string userId)
            {
                return da.getCurrentBill(userId);
            }

            //Method To add amount to bill
            public void addBill(string userId, double amountToAdd)
            {
                da.addBill(userId, amountToAdd);
            }

            //Method to remove amount from bill
            public void removeBill(string userId, double amountToDeduct)
            {
                da.addBill(userId, amountToDeduct);
            }

            //PayBill
            public bool payBill()
            {
                return true;
            }

        }
    }

}
