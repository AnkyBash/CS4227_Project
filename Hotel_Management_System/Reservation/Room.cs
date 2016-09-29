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
        class Room   //inheritance base class
        {
            //Attribute of Room Type
            protected string _roomID;
            protected double _roomPrice;
            protected int _numberOfBed;
            protected string _roomStatus;
            private List<string> roomList;

            //members use in base and derived class
            protected string _hotelLocation;
            protected string _roomType;

            //private List<KeyValuePair<string, double>> roomIDPrice;
            protected DatabaseAccess da = new DatabaseAccess();

            //Get and Set Method for room ID
            public string roomType
            {
                /* get
                 {
                     return this._roomType;
                 }  */
                set
                {
                    this._roomType = value;
                }
            }

            //Get and Set Method for room ID
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

            //Get and Set Method for room price
            public double roomPrice
            {
                get
                {
                    return this._roomPrice;
                }
                set
                {
                    this._roomPrice = value;
                }
            }

            //Get and Set Method for Number Of Bed
            public int numberOfBed
            {
                get
                {
                    return this._numberOfBed;
                }
                set
                {
                    this._numberOfBed = value;
                }
            }

            //Get and Set Method for Room Status
            public string roomStatus
            {
                get
                {
                    return this._roomStatus;
                }
                set
                {
                    this._roomStatus = value;
                }
            }

            //Get and Set Method for hotel Location
            public string hotelLocation
            {
                get
                {
                    return this._hotelLocation;
                }
                set
                {
                    this._hotelLocation = value;
                }
            }

            //initialising list for room type
            public List<string> getRoomTypeList()
            {
                roomList = new List<string>();
                roomList = da.roomTypeList();
                return roomList;
            }

            //base method to add rooms 
            public void addRooms()
            {
                //rooms Add
            }


        }
    }

}
