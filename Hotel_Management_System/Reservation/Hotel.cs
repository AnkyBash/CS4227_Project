using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database_Package;

namespace Bussiness
{
    namespace pReservation
    {
        /// <summary>
        /// The 'Singleton' class
        /// </summary>
        public class Hotel
        {
            private DatabaseAccess da = new DatabaseAccess();

            //private Contrustor
            private Hotel()
            { 
            }

            //hotel Object
            private volatile static Hotel htObject;

            // Lock synchronization object
            private static object lockingObject = new object();

            public static Hotel InstanceCreation()
            {
                // Support multithreaded applications through
                // 'Double checked locking' pattern which (once
                // the instance exists) avoids locking each
                // time the method is invoked
                if (htObject == null)
                {
                    lock (lockingObject)
                    {
                        if (htObject == null)
                        {
                            htObject = new Hotel();
                        }
                    }
                }
                return htObject;
            }

            //method to get hotel List
            public List<string> getHotelLocationList()
            {
                List<string> hotelList = da.hotelLocationList();
                return hotelList;
            }
        }
    }
}
