using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Database_Package;

namespace Bussiness
{
    namespace pReservation
    {
        public class ServicesFactory
        {
            private List<string> servicesList = new List<string>();
            private DatabaseAccess da = new DatabaseAccess();

            // Method to initialize Services
            public List<string> getServicesList()
            {
                servicesList.Clear();
                servicesList = da.servicesList();
                return servicesList;
            }

            //Adding item in a Services List
            public void AddService(string item)
            {
                servicesList.Add(item);
                servicesList.Sort();
            }

            //Removing item in a Services List
            public void RemoveService(string item)
            {
                servicesList.Remove(item);
                servicesList.Sort();
            }

            //Get services list item by passing the index of array
            public string Get(int index)
            {
                //no validation
                return servicesList[index];
            }


            public static Services getService(string usedService)
            {

                switch (usedService)
                {
                    case "Laundry": return new LaundryService();

                    case "FoodOrder": return new FoodOrderService();

                    case "Taxi": return new TaxiService();

                    case "Cleaning": return new CleaningService();

                    default: return null;

                }
            }
        }
    }

}

