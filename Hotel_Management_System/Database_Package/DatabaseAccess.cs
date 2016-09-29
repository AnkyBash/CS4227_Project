using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Database_Package
{
    public class DatabaseAccess : DatabaseInterface  //using interface
    {
        private string sqlString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\Hotel_db.mdf;Integrated Security=True";

        //method to check if current user have reservation
        public bool haveReservation(string userID)
        {
            try
            {
                string stmt = "Select COUNT(*) FROM Reservation WHERE userID ='"+userID+"';";
                int count = 0;

                using (SqlConnection thisConnection = new SqlConnection(sqlString))
                {
                    using (SqlCommand cmdCount = new SqlCommand(stmt, thisConnection))
                    {
                        thisConnection.Open();
                        count = (int)cmdCount.ExecuteScalar();
                    }
                }
                if (count > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Modify Reservation
        public bool modifyReservation(string userID, string roomID, DateTime startDate, DateTime endDate) 
        {
            string updateQuery = "UPDATE [Reservation] SET Check_In = @startDate, Check_Out = @endDate WHERE RoomID='"+Convert.ToInt16(roomID)+"' AND userID='"+userID+"';";

            try
            {
                using (SqlConnection con = new SqlConnection(sqlString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@startDate", startDate.Date);
                        cmd.Parameters.AddWithValue("@endDate", endDate.Date);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Changed the Reservation Date.");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            } 
        }

        //Delete Reservation
        public bool RemoveReservation(string userID, string roomID, DateTime startDate, DateTime endDate)
        {
            string updateQuery = "DELETE FROM [Reservation] WHERE (userID =@uID AND RoomID=@rmID) AND (Check_In = @startDate AND Check_Out = @endDate);";

            try
            {
                using (SqlConnection con = new SqlConnection(sqlString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@uID", userID);
                        cmd.Parameters.AddWithValue("@rmID", Convert.ToInt16(roomID));
                        cmd.Parameters.AddWithValue("@startDate", startDate.Date);
                        cmd.Parameters.AddWithValue("@endDate", endDate.Date);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Reservation Cancel");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //method To Get Current Bill
        public List<KeyValuePair<double, int>> getCurrentBill(string userId)
        {
            List<KeyValuePair<double, int>> billPayment = new List<KeyValuePair<double, int>>();
            try
            {
                if (userId == "")
                {
                    MessageBox.Show("User Id Not Found. Are you Logged in ?");
                    return billPayment;
                }
                else
                {
                    string queryStr = "SELECT totalAmount, Payment FROM [Bill] where custID='" + userId + "';";
                   
                        using (SqlConnection cnn = new SqlConnection(sqlString))
                        {
                            SqlDataAdapter da = new SqlDataAdapter(queryStr, cnn);
                            DataSet ds = new DataSet();
                            da.Fill(ds, "BillTable");

                            foreach (DataRow row in ds.Tables["BillTable"].Rows)
                            {
                                billPayment.Add(new KeyValuePair<double, int>(Convert.ToDouble(row["totalAmount"]), Convert.ToInt16(row["Payment"])));
                            }
                        }
                    
                }
                return billPayment;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return billPayment;
            }
        }

        //Method To add amount to bill
        public void addBill(string userId, double amountToAdd)
        {
            double currentBillAmount;
            List<KeyValuePair<double, int>> billAmount;
            billAmount = getCurrentBill(userId);
            currentBillAmount = Convert.ToDouble(billAmount[0].Key);
            currentBillAmount = currentBillAmount + amountToAdd;
            string updateQuery = "UPDATE [Bill] SET totalAmount = "+currentBillAmount+" WHERE custID ='"+userId+"';";

            try
            {
                using (SqlConnection con = new SqlConnection(sqlString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(updateQuery, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Amount added to the Bill");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Method to remove amount from bill
        public void removeBill(string userId, double amountToDeduct)
        {
            double currentBillAmount;
            List<KeyValuePair<double, int>> billAmount;
            billAmount = getCurrentBill(userId);
            currentBillAmount = Convert.ToDouble(billAmount[0].Key);
            currentBillAmount = currentBillAmount - amountToDeduct;
            string updateQuery = "UPDATE [Bill] SET totalAmount = " + currentBillAmount + " WHERE custID ='" + userId + "';";

            try
            {
                using (SqlConnection con = new SqlConnection(sqlString))
                {
                    SqlCommand cmd = new SqlCommand(updateQuery, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Amount added to the Bill");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //confirm Reservation
        public bool makeReservation(string userID, List<KeyValuePair<string, double>> roomIDPrice, DateTime startDate, DateTime endDate, int noOfRoom, string creditCard)
        {
            try
            {
                 int resIDCount = 0;
                 double totalAmount = 0.0;
                 string roomID;//, resID;
                 string stmt = "select coalesce(max(ReservationID), 0) from Reservation;";

                 using (SqlConnection thisConnection = new SqlConnection(sqlString))
                 {
                     using (SqlCommand cmdCount = new SqlCommand(stmt, thisConnection))
                     {
                         thisConnection.Open();
                         resIDCount = (int)cmdCount.ExecuteScalar();
                     }
                 }
                 resIDCount = resIDCount+1;

                 if(noOfRoom>0)
                   {
                       for (int i = 0; i < noOfRoom; i++)
                       {
                       //    resID = resIDCount;
                           roomID = roomIDPrice[i].Key;
                           totalAmount = totalAmount + Convert.ToDouble(roomIDPrice[i].Value);

                           string queryString = "INSERT INTO [Reservation](ReservationID,RoomID,userID,Check_In,Check_Out) values(@resID,@roomID,@userID,@startDate,@endDate);";
                           using (SqlConnection conn = new SqlConnection(sqlString))
                           {
                               conn.Open();
                               using (SqlCommand cmd = new SqlCommand(queryString, conn))
                               {
                                   //@resID,@roomID,@userID
                                   cmd.Parameters.AddWithValue("@resID", resIDCount);
                                   cmd.Parameters.AddWithValue("@roomID", roomID);
                                   cmd.Parameters.AddWithValue("@userID", userID);
                                   cmd.Parameters.AddWithValue("@startDate", startDate.Date);
                                   cmd.Parameters.AddWithValue("@endDate", endDate.Date);

                                   cmd.ExecuteNonQuery();
                                   
                                   //rows number of record got updated
                               }
                           } 
                           resIDCount = resIDCount+1;

                           string updateQuery2 = "UPDATE [Room] SET Status = 'C' WHERE roomID = '"+roomID+"';";
                           using (SqlConnection con = new SqlConnection(sqlString))
                           {
                               con.Open();
                               using (SqlCommand cmd = new SqlCommand(updateQuery2, con))
                               {
                                   cmd.ExecuteNonQuery();
                               }
                           }
                       }

                       string billString = "select coalesce(max(billID), 0) from Bill;";
                       int billIDCount = 0;  

                       using (SqlConnection thisConnection = new SqlConnection(sqlString))
                       {
                           using (SqlCommand cmdCount = new SqlCommand(billString, thisConnection))
                           {
                               thisConnection.Open();
                               billIDCount = (int)cmdCount.ExecuteScalar();
                           }
                       }
                       billIDCount = billIDCount + 1;

                       string queryString2 = "INSERT INTO [Bill](billID, custID, totalAmount, Payment, CreditCard) values(@billID,@custID,@tAmount,@payment,@creditCard);";
                       using (SqlConnection conn = new SqlConnection(sqlString))
                       {
                           conn.Open();
                           using (SqlCommand cmd = new SqlCommand(queryString2, conn))
                           {
                               //@resID,@roomID,@userID
                               cmd.Parameters.AddWithValue("@billID", billIDCount);
                               cmd.Parameters.AddWithValue("@custID", userID);
                               cmd.Parameters.AddWithValue("@tAmount", totalAmount);
                               cmd.Parameters.AddWithValue("@payment", 0);
                               cmd.Parameters.AddWithValue("@creditCard", creditCard);

                               cmd.ExecuteNonQuery();

                               //rows number of record got updated
                           }
                       } 
                       return true;
                   }
                   else
                   {
                     MessageBox.Show("Rooms are not Available.");
                     return false;
                   }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //override method for getuserList
        public List<string> getUserList()
        {
            List<string> userList = new List<string>();

            userList.Add("Employee");
            userList.Add("Guest");
            userList.Sort();

            return userList;
        }

        //override method for getting user id
        public string getUserID(string uname, string pwd)
        {
            string uID="";
            string queryStr = "SELECT userID FROM [User] where (username='" + uname + "'AND password='" + pwd + "');";
           // SqlDataReader reader = null;
            try
            {
                using (SqlConnection cnn = new SqlConnection(sqlString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(queryStr, cnn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "UserId");

                    foreach (DataRow row in ds.Tables["UserId"].Rows)
                    {
                        uID = Convert.ToString(row["UserId"]);
                    }
                }
                return uID;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
            finally
            {
               
            }
            
        }

        //@override
        public List<string> servicesList()
        {
            List<string> servicesLists = new List<string>();
            servicesLists.Add("Laundry");
            servicesLists.Add("FoodOrder");
            servicesLists.Add("Taxi");
            servicesLists.Add("Cleaning");
            servicesLists.Sort();

            return servicesLists;
        }

        //override private List<KeyValuePair<string, double>> roomPriceList;
        public List<KeyValuePair<string, double>> getAvailableRoomPrice(string htLoc, string rmType,DateTime startDate,DateTime endDate)
        {
            List<KeyValuePair<string, double>> roomPriceList = new List<KeyValuePair<string, double>>();
            try
            {
                string queryStr = "SELECT roomID, roomPrice FROM Room where (roomType='" + rmType + "'AND hotelLoc='" + htLoc + "') AND (Status='A');"; 
                using (SqlConnection cnn = new SqlConnection(sqlString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(queryStr, cnn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "Room");

                    foreach (DataRow row in ds.Tables["Room"].Rows)
                    {
                        roomPriceList.Add(new KeyValuePair<string, double>(Convert.ToString(row["roomID"]), Convert.ToDouble(row["roomPrice"])));
                        //roomPriceList.Add(Convert.ToDouble(row["roomPrice"]));
                    }
                }
                return roomPriceList;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return roomPriceList;
            }
        }
   
        //override
        public int roomSearch(string hotelLoc, string roomType, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (hotelLoc == "" || roomType == "" || startDate == null || endDate == null)
                {
                    MessageBox.Show("Data Missing From The Form(Check hotel Loaction, room Type, Check-in Date, CHeck-out Date)");
                }

                string stmt = "Select COUNT(*) FROM Room WHERE (roomType='" + roomType + "'AND hotelLoc='" + hotelLoc + "')  AND (Status='A');";
                int count = 0;

                using (SqlConnection thisConnection = new SqlConnection(sqlString))
                {
                    using (SqlCommand cmdCount = new SqlCommand(stmt, thisConnection))
                    {
                        thisConnection.Open();
                        count = (int)cmdCount.ExecuteScalar();
                    }
                }
                
                return count;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
         
        }

        //override
        public List<string> roomTypeList()
        {
            List<string> roomList = new List<string>();

            roomList.Add("Single");
            roomList.Add("Double");
            roomList.Add("Twin");
            roomList.Add("Delux");
            roomList.Add("Luxury");
            roomList.Sort();

            return roomList;
        }

       //override
       public bool isRegister(string uname, string pwd, string fname, string lname, string address, string telephone, string employeeType, string userCat)
       {
           try
           {
               if (uname == "" || pwd == "" || fname == "" || lname == "" || address == "" || telephone == "" || userCat == "")
               {
                   MessageBox.Show(" Details are Missing. Something is not filled, Check it again.");
                   return false;
               }

               string stmt = "SELECT COUNT(*) FROM [User]";
               int count = 0;

               using (SqlConnection thisConnection = new SqlConnection(sqlString))
               {
                   using (SqlCommand cmdCount = new SqlCommand(stmt, thisConnection))
                   {
                       thisConnection.Open();
                       count = (int)cmdCount.ExecuteScalar();
                   }
               }
               count++;

               string userId;
               if (userCat.Equals("Employee"))
               {
                   userId = "E" + count;
               }
               else
               {
                   userId = "C" + count;
               }

               SqlConnection con = new SqlConnection(sqlString);

               string insertStr = "insert into [User](userID,username,password,firstname,lastname,address,telephone,employeetype) values('" + userId + "','" + uname + "','" + pwd + "','" + fname + "','" + lname + "','" + address + "','" + telephone + "','" + employeeType + "')";
              // string insertStr = "insert into [User](userID,username,password,firstname,lastname,address,telephone,employeetype) values('e2','ank','ank','ank','waliya','2,kil','8275273','manager')";

               SqlCommand cmd = new SqlCommand(insertStr, con);

               cmd.CommandType = CommandType.Text;

               con.Open();
               cmd.ExecuteNonQuery();
               con.Close();

               return true;
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.Message);
               return false;
           }
       }
 
        //override
        public bool signIn(string uname, string pwd)
        {
            try
            {
                if (uname == "" || pwd == "")
                {
                    MessageBox.Show(" Enter UserName and Password .");
                    return false;
                }

                SqlConnection con = new SqlConnection(sqlString);
                con.Open();

                SqlCommand cmd = new SqlCommand("select username,password from [User] where username='" + uname + "'and password='" + pwd + "'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    //MessageBox.Show("Login sucess ");  
                    return true;
                }
                else
                {
                    MessageBox.Show("Invalid Login please check username and password");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
           
        }
    
        //override 
        public List<string> hotelLocationList()
        {
            List<string> hotelList = new List<string>();

            hotelList.Add("Limerick");
            hotelList.Add("Cork");
            hotelList.Add("Dublin");
            hotelList.Add("Kerry");
            hotelList.Sort();

            return hotelList;
        }

        //Method Ta Auto Check-In user
        public bool AutoCheckIn(string user)
        {
            return true;
        }

        //Method To Auto Check-Out user
        public bool AutoCheckOut(string key)
        {
            return true;
        }

    }
}
