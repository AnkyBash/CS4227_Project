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
        class Booking : IBooking
        {
            private string _reservationID;
            private string _creditCard;
            private DateTime _startDate;
            private DateTime _endDate;
            private string _roomType;
            private int _numOfBed;
            private string _hotel_Loc;
            private int _noOfRoom;
            private string _roomID;

            private List<KeyValuePair<string, double>> _roomIDPrice;
            private DatabaseAccess da = new DatabaseAccess();

            //Get and Set Method for No Of Room Needed to Book
            public List<KeyValuePair<string, double>> roomIDPrice
            {
                get
                {
                    return this._roomIDPrice;
                }
                set
                {
                    this._roomIDPrice = value;
                }
            }

            //Get and Set Method for No Of Room Needed to Book
            public int noOfRoom
            {
                get
                {
                    return this._noOfRoom;
                }
                set
                {
                    this._noOfRoom = value;
                }
            }

            //Get and Set Method for Hotel Location
            public string hotelLoc
            {
                get
                {
                    return this._hotel_Loc;
                }
                set
                {
                    this._hotel_Loc = value;
                }
            }

            //Get and Set Method for roomID
            public string roomID
            {
                get
                {
                    return this._roomID;
                }
                set
                {
                    this._roomID = value;
                }
            }

            //Get and Set Method for Reservation ID
            public string reservationID
            {
                get
                {
                    return this._reservationID;
                }
                set
                {
                    this._reservationID = value;
                }
            }


            //Get and Set Method for Costumer ID
            public string creditCard
            {
                get
                {
                    return this._creditCard;
                }
                set
                {
                    this._creditCard = value;
                }
            }


            //Get and Set Method for start date
            public DateTime startDate
            {
                get
                {
                    return this._startDate;
                }
                set
                {
                    this._startDate = value;
                }
            }


            //Get and Set Method for End Date
            public DateTime endDate
            {
                get
                {
                    return this._endDate;
                }
                set
                {
                    this._endDate = value;
                }
            }


            //Get and Set Method for Room Type
            public string roomType
            {
                get
                {
                    return this._roomType;
                }
                set
                {
                    this._roomType = value;
                }
            }


            //Get and Set Method for No. Of Bed
            public int numOfBed
            {
                get
                {
                    return this._numOfBed;
                }
                set
                {
                    this._numOfBed = value;
                }
            }

            //Method To check if user have reservation
            public bool haveReservation(string userID)
            {
                if (da.haveReservation(userID))
                {
                    return true;
                }
                else
                    return false;
            }

            //Method to return the list of all available room list
            public int roomSearch()
            {
                //number of rooms available according to the user selection 
                int roomsCount = da.roomSearch(_hotel_Loc, _roomType, _startDate, _endDate);
                return roomsCount;
            }

            //method to reserve the room
            public bool makeReservation(string _userID)  //pass the required parameter
            {
                if (da.makeReservation(_userID, _roomIDPrice, _startDate, _endDate, _noOfRoom, _creditCard))
                {
                    return true;
                }
                else
                    return false;
            }

            //Cancel Resrvation
            public bool cancelReservation(string userID)
            {
                if (da.RemoveReservation(userID, _roomID, _startDate, _endDate))
                {
                    return true;
                }
                else
                    return false;
            }

            //method to modify the reserved room
            public bool modifyReservation(string userID)  //pass the required parameter
            {
                //Need to put the code to modify the reservation
                if (da.modifyReservation(userID, _roomID, _startDate, _endDate))
                {
                    return true;
                }
                else
                    return false;
            }

            //Method to get available roomID and Price 
            public List<KeyValuePair<string, double>> getAvailableRoomPrice() //room id and hotel location
            {
                return da.getAvailableRoomPrice(_hotel_Loc, _roomType, _startDate, _endDate); ;
            }

            //Method Ta Auto Check-In user
            public bool AutoCheckIn(string user)
            {
                return da.AutoCheckIn(user);
            }

            //Method To Auto Check-Out user
            public bool AutoCheckOut(string key)
            {
                return da.AutoCheckOut(key);
            }

        }
    }

}

