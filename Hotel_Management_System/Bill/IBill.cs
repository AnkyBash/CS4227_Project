using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
    namespace pBill
    {
        interface IBill
        {
            //method To Get Current Bill
            List<KeyValuePair<double, int>> getCurrentBill(string userId);

            //Method To add amount to bill
            void addBill(string userId, double amountToAdd);

            //Method to remove amount from bill
            void removeBill(string userId, double amountToDeduct);

            //PayBill
            bool payBill();
        }
    }

}
