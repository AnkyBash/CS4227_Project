using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Bussiness
{
    namespace pReservation
    {
        interface IBooking
        {
            //Method to return the list of all available room list
            int roomSearch();

            //method to reserve the room
            bool makeReservation(string _userID);  //pass the required parameter

            //method to modify the reserved room
            bool modifyReservation(string userID);  //pass the required parameter

            //Cancel reservation
            bool cancelReservation(string userID);

            //Method to get available roomID and Price 
            List<KeyValuePair<string, double>> getAvailableRoomPrice(); //room id and hotel location

            //Method Ta Auto Check-In user
            bool AutoCheckIn(string user);

            //Method To Auto Check-Out user
            bool AutoCheckOut(string key); 

        }
    }

}
