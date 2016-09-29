using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Database_Package
{
    interface DatabaseInterface
    { 
       // To login User
       bool signIn(string username, string password);

       //Register Method
       bool isRegister(string uname, string pwd, string fname, string lname, string address, string telephone, string employeeType, string userCat);
       
       //List Of Hotel Location 
       List<string> hotelLocationList();

       //List Of different Room type
       List<string> roomTypeList();

       //Method to search available room and return no of room 
       int roomSearch(string hotelLoc, string roomType, DateTime startDate, DateTime endDate);

       //List Of Room Price
       List<KeyValuePair<string, double>> getAvailableRoomPrice(string htLoc, string rmType, DateTime startDate, DateTime endDate);
       
       //List Of User List
       List<string> getUserList();

       //confirm Reservation
       bool makeReservation(string userID, List<KeyValuePair<string, double>> roomIDPrice, DateTime startDate, DateTime endDate, int noOfRoom, string creditCard);

       //Modify Reservation
       bool modifyReservation(string userID, string roomID, DateTime startDate, DateTime endDate);

       //Cancel Reservation
       bool RemoveReservation(string userID, string roomID, DateTime startDate, DateTime endDate);

       //List Of services
       List<string> servicesList();
       
       //Getting a user id from table
       string getUserID(string uname, string pwd);

       //method To Get Current Bill
       List<KeyValuePair<double, int>> getCurrentBill(string userId);

       //Method To add amount to bill
       void addBill(string userId, double amountToAdd);

       //Method to remove amount from bill
       void removeBill(string userId, double amountToDeduct);

       //Method To check if user have reservation
       bool haveReservation(string userID);

       //Method Ta Auto Check-In user
       bool AutoCheckIn(string user);

       //Method To Auto Check-Out user
       bool AutoCheckOut(string key); 

    }
}
