using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using Bussiness.pUser;
using Bussiness.pReservation;
using Bussiness.pBill;

namespace UI
{
    public partial class Form1 : Form
    {
        //Making Instances 
        private Room room = new Room();  //Room Class
        private User user = new User();  //User Class
        private Booking booking = new Booking();   //Booking Class
        private SingleRoom single = new SingleRoom();  //Single Room Class
        private DoubleRoom dble = new DoubleRoom();  //Double Room Class
        private TwinRoom twin = new TwinRoom();     //Twin Room Class
        private DeluxRoom delux = new DeluxRoom();  //Delux Room Class
        private LuxuryRoom luxury = new LuxuryRoom();  //Luxury Room Class
        private Bill bill = new Bill();    //Bill Class

        //Singleton Design Pattern.
        Hotel hotel = Hotel.InstanceCreation();//create Single instance
        
        //User Configuration
        private string userName;
        private string password;
        private string userID;

        public Form1()
        {
            //Initializing Design
            InitializeComponent();    
        }

        //Event When form is loaded
        private void Form1_Load(object sender, EventArgs e)
        {
            //initialize form data
            //Singleton Pattern using Method
            //Filling Hotel Loaction
            ddHotelLocSearch.DataSource = hotel.getHotelLocationList();
            //Filling Room Type
            ddRoomTypeSearch.DataSource = room.getRoomTypeList();
            //Filling User Type 
            ddUserList.DataSource = user.getUserList();
            //Enable employe type if it is a customer
            ddEmployeeType.Enabled = false;
            //Button sign-in and out 
            btnSignOut.Enabled = false;
            btnSignIn.Enabled = true;
            
            //Tab Control Access
            RoomSearchTab.Enabled = false;
            BillTab.Enabled = false;
            ServiceTab.Enabled = false;
            AutoCheckInCheckOutTab.Enabled = false;
            SearchResultTab.Enabled = false;
            RoomReserveTab.Enabled = false;
            ModifyReservationTab.Enabled = false;
            cancelReservationTab.Enabled = false;
            PaymentTab.Enabled = false;
            AutoCheckInCheckOutTab.Enabled = false;

            //Button Control Access
            btnReservation.Enabled = false;
            btnServices.Enabled = false;
            btnBill.Enabled = false;

            //DropDown List Populate
            //Factory Pattern
            ServicesFactory servicesfact = new ServicesFactory();
            ddServiceList.DataSource = servicesfact.getServicesList(); 
         }

        //Credit Card Details Confirmation
        private void btnCreditCardConfirm_Click(object sender, EventArgs e)
        {
            //checking if user is logged in
            if (user.isLogin())
            {
                tabMain.SelectedTab = RoomReserveTab;
                RoomReserveTab.Enabled = true;
                booking.creditCard = txtBoxCreditCard.Text;
            }
            
        }

        //button event click when user confirm the booking
        private void btnResConfirm_Click(object sender, EventArgs e)
        {
            //disable the payment tab
            PaymentTab.Enabled = false;

            //chevking if user is logged in
            if (user.isLogin())
            {
                //attributes
                string htLoc, rmType; 
                DateTime startDate, endDate;
                int noOfRoom;
                
                //using keyvaluepair to get return two parameters at a same time
                List<KeyValuePair<string, double>> roomIDPrice = new List<KeyValuePair<string, double>>();
                
                //setting up parameters
                htLoc = lblAhtLocRes.Text;
                rmType = lblARmTypeRes.Text;
                startDate = Convert.ToDateTime(lblAstrDtRes.Text);
                endDate = Convert.ToDateTime(lblAendDtRes.Text);
                noOfRoom = Convert.ToInt32(ddRoomsAvailable.SelectedItem.ToString());

                //setting up values for making reservation
                booking.hotelLoc = htLoc;
                booking.roomType = rmType;
                booking.startDate = startDate;
                booking.endDate = endDate;
                booking.roomIDPrice = roomIDPrice;
                booking.noOfRoom = noOfRoom;

                //adding array values user selected view box in the keyvaluepair list
                foreach (KeyValuePair<string, double> l in lBoxRoomNPrice.SelectedItems)
                {
                    roomIDPrice.Add(l);
                }

                //check if reservation success and show appropriate message
                if (booking.makeReservation(userID))
                {
                    MessageBox.Show("Reservation Confirmed");
                    tabMain.SelectedTab = RoomSearchTab;
                    SearchResultTab.Enabled = false;
                    RoomReserveTab.Enabled = false;
                }
                else
                {
                    //message if reservation nt confirmed
                    MessageBox.Show("Reservation Can't Confirmed");
                }
            }
            else
            {
                //message if user nt logged in
                MessageBox.Show("You must logged in to reserve room.");
            }
            
        }

        //button click event for registering
        private void btnRegstn_Click(object sender, EventArgs e)
        {
            tabMain.SelectedTab = RegistrationTab;
        }

        //button click event for login
        private void btnSignIn_Click(object sender, EventArgs e)
        {
            tabMain.SelectedTab = LoginTab;
        }

        //button click event for reservation
        private void btnReservation_Click(object sender, EventArgs e)
        {
            tabMain.SelectedTab = RoomSearchTab;
        }

        //button click event for Bill
        private void btnBill_Click(object sender, EventArgs e)
        {
            //chnge tab to bill tab
            tabMain.SelectedTab = BillTab;
            //attributes
            double amount = 0.0;
            int status = 0;
            List<KeyValuePair<double, int>> billAmount;
            //getting current bill amount
            billAmount = bill.getCurrentBill(userID);
            
            //if user dont have any bill
            if (billAmount.Count == 0)
            {
                MessageBox.Show("There is nothing to show. No bills Yet. Happy Days.");
            }
            else
            {
                //amount and status of the bill
                amount = billAmount[0].Key;
                status = billAmount[0].Value;

                //label set it to current bill amount
                lblCurrentBill.Text = amount.ToString() + " €";
                if (status == 0)
                {
                    lblBillStatus.Text = "Not Paid";
                }
                else if (status == 1)
                {
                    lblBillStatus.Text = "Paid";
                }
                else
                { MessageBox.Show(status + ""); }
            }
        }

        //button click event for Services
        private void btnServices_Click(object sender, EventArgs e)
        {
            tabMain.SelectedTab = ServiceTab;
        }

        //button click event for Autocheck-In and Out
        private void btnAutoCheckOut_Click(object sender, EventArgs e)
        {
            tabMain.SelectedTab = AutoCheckInCheckOutTab;
        }

        private void linkLabel1_LinkClicked(object sender, EventArgs e)
        {
            tabMain.SelectedTab = LoginTab;
        }

        private void dateTimePickerEnd_ValueChanged(object sender, EventArgs e)
        {
            DateTime mCurrentTime = DateTime.Now;
            dateTimePickerEnd.MinDate = mCurrentTime;
        }

        private void dateTimePickerstrt_ValueChanged(object sender, EventArgs e)
        {
            DateTime mCurrentTime = DateTime.Now;
            dateTimePickerstrt.MinDate = mCurrentTime;
        }

        private void dateTimePickerEnd_CloseUp(object sender, EventArgs e)
        {
          /*  DateTime mCurrentTime = DateTime.Now;
            if (dateTimePickerEnd.Value < mCurrentTime)
            {
                MessageBox.Show("Date n Time can't be lesser than current Date Time");
            }*/
        }

        private void dateTimePickerstrt_CloseUp(object sender, EventArgs e)
        {
          /*  DateTime mCurrentTime = DateTime.Now;
            if (dateTimePickerstrt.Value < mCurrentTime)
            {
                MessageBox.Show("Date n Time can't be lesser than current Date Time");
            }  */
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {
            
        }

        //button click event for Logout
        private void btnSignOut_Click(object sender, EventArgs e)
        {
            //if user logged out retrict the access of software
            if (user.Logout())
            {
                //Setting User Name in Welcome Screen
                lblUserName.Text = user.username;

                //Button Control Access
                btnSignOut.Enabled = false;
                btnSignIn.Enabled = true;
                btnReservation.Enabled = false;
                btnServices.Enabled = false;
                btnBill.Enabled = false;

                //Setting User Config
                this.userName = user.username;
                this.password = user.password;
                this.userID = user.userID;

                //Control Access
                LoginTab.Enabled = true;
                RoomSearchTab.Enabled = false;
                BillTab.Enabled = false;
                ServiceTab.Enabled = false;
                AutoCheckInCheckOutTab.Enabled = false;
                SearchResultTab.Enabled = false;
                RoomReserveTab.Enabled = false;
                ModifyReservationTab.Enabled = false;
                cancelReservationTab.Enabled = false;
                AutoCheckInCheckOutTab.Enabled = false;
            }
             
        }

        //button click event for Login
        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Attributes
            Boolean flag = false;
            string uname, pwd;
            uname = txtbxUserName.Text;
            pwd = txtPassword.Text;
            user.username = uname;
            user.password = pwd;
             
            //checking if user got sign in
            flag = user.signIn();
            if (flag)
            {
                MessageBox.Show("Login Successful");
                lblUserName.Text = uname;
                //Buttons Login and Logout
                btnSignIn.Enabled = false;
                btnSignOut.Enabled = true;
                 
                this.userName = user.username;
                this.password = user.password;
                this.userID = user.getUserID();
                
                //Customer Access Pages
                if (userID.StartsWith("C"))
                {
                    //Tab Control Access
                    LoginTab.Enabled = false;
                    RoomSearchTab.Enabled = true;
                    BillTab.Enabled = true;
                    ServiceTab.Enabled = true;
                    AutoCheckInCheckOutTab.Enabled = true;
                    SearchResultTab.Enabled = false;
                    RoomReserveTab.Enabled = false;
                    ModifyReservationTab.Enabled = true;
                    cancelReservationTab.Enabled = true;
                    AutoCheckInCheckOutTab.Enabled = true;

                    //Button Control Access
                    btnReservation.Enabled = true;
                    btnServices.Enabled = true;
                    btnBill.Enabled = true;
                }
                
                //Employee Access Page
                else if (userID.StartsWith("E"))
                {
                    LoginTab.Enabled = false;
                    RoomSearchTab.Enabled = true;
                    BillTab.Enabled = true;
                    ServiceTab.Enabled = true;
                    AutoCheckInCheckOutTab.Enabled = true;
                    SearchResultTab.Enabled = false;
                    RoomReserveTab.Enabled = false;
                    ModifyReservationTab.Enabled = true;
                    cancelReservationTab.Enabled = true;
                    AutoCheckInCheckOutTab.Enabled = true;

                    //Button Control Access
                    btnReservation.Enabled = true;
                    btnServices.Enabled = true;
                    btnBill.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Wrong user ID. Please Call the Engineer.");
                    
                    //Tab Control Access
                    LoginTab.Enabled = true;
                    RoomSearchTab.Enabled = false;
                    BillTab.Enabled = false;
                    ServiceTab.Enabled = false;
                    AutoCheckInCheckOutTab.Enabled = false;
                    SearchResultTab.Enabled = false;
                    RoomReserveTab.Enabled = false;
                    ModifyReservationTab.Enabled = false;
                    cancelReservationTab.Enabled = false;
                    AutoCheckInCheckOutTab.Enabled = false;

                    //Button Control Access
                    btnReservation.Enabled = false;
                    btnServices.Enabled = false;
                    btnBill.Enabled = false;
                }
            }
            else
            {
                //If Login Failed
                MessageBox.Show("Login Failed");
            }
            
        }

        //Changing according to the customer or employee
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddUserList.SelectedItem.ToString().Equals("Employee"))
            {
                ddEmployeeType.Enabled = true;
            }
            else
            {
                ddEmployeeType.Enabled = false;
            }
        }

        //Registration
        private void btnRegister_Click(object sender, EventArgs e)
        {
            //Boolean flag = false;
            string uname, pwd, fname, lname, address, telephone, employeeType = "", userCat;

            uname = txtRegUName.Text;
            pwd = txtRegPwd.Text;
            fname = txtFName.Text;
            lname = txtLName.Text;
            address = txtAddress.Text;
            telephone = txtTelephoneNo.Text;
            
            userCat = ddUserList.SelectedItem.ToString();
            if (userCat == "Employee")
            {
                employeeType = ddEmployeeType.SelectedItem.ToString();
            }

            user.username = uname;
            user.password = pwd;
            user.firstName = fname;
            user.lastName = lname;
            user.address = address;
            user.telephone = telephone;
            user.employeeType = employeeType;
            user.userCat = userCat;

            if (user.isRegistered())
            {
                MessageBox.Show("Register Successful");
                tabMain.SelectedTab = LoginTab;
            }
            else
            {
                MessageBox.Show("Register Failed");
            }
        }

        //Button Click to Search Room 
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string hotelLoc, roomType, listP = "";
            DateTime startDate, endDate;

            hotelLoc = ddHotelLocSearch.SelectedItem.ToString();
            roomType = ddRoomTypeSearch.SelectedItem.ToString();
            startDate = dateTimePickerstrt.Value;
            endDate = dateTimePickerEnd.Value;

            if (startDate.Date < endDate.Date)
            {
                //setting values in booking class
                booking.hotelLoc = hotelLoc;
                booking.roomType = roomType;
                booking.startDate = startDate;
                booking.endDate = endDate;

                int noOfRoom = booking.roomSearch();//hotelLoc, roomType, startDate, endDate

                if (noOfRoom < 1)
                {
                    MessageBox.Show("Sorry No Room Available in these dates or location");
                }
                else
                {
                    List<KeyValuePair<string, double>> roomIDPrice = booking.getAvailableRoomPrice();//hotel_Loc, roomType, startDate, endDate

                    lblAhtLocSearch.Text = hotelLoc;
                    lblARoomType.Text = roomType;
                    lblAStartDate.Text = startDate.ToString("dd/MM/yyyy");
                    lblAEndDate.Text = endDate.ToString("dd/MM/yyyy");
                    lblARooms.Text = noOfRoom.ToString();

                    foreach (KeyValuePair<string, double> kvp in roomIDPrice)
                    {
                        //Console.WriteLine(string.Format("Key: {0} Value: {1}", kvp.Key, kvp.Value);
                        listP += "" + kvp.Value + " €\n";
                    }

                    lblARoomPrice.Text = listP;

                    tabMain.SelectedTab = SearchResultTab;
                    SearchResultTab.Enabled = true;

                    lblAhtLocRes.Text = hotelLoc;
                    lblARmTypeRes.Text = roomType;
                    lblAstrDtRes.Text = startDate.ToString("dd/MM/yyyy");
                    lblAendDtRes.Text = endDate.ToString("dd/MM/yyyy");

                    int[] num = new int[noOfRoom];
                    int n = num.Length;
                    for (int i = 0; i < num.Length; i++)
                    {
                        num[i] = noOfRoom - i;
                    }
                    ddRoomsAvailable.DataSource = num;
                    lBoxRoomNPrice.DataSource = roomIDPrice;
                }

            }
            else
            {
                MessageBox.Show("Start Date is bigger Than End Date. Check The Date Again.");
            }

            
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            //tab to show after serach Result Room
            tabMain.SelectedTab = PaymentTab;
            PaymentTab.Enabled = true;
        }

        //showing price according to the user selection
        private void ddServiceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblserviceText.Enabled = true;
            lblServiceCost.Enabled = true;

            //using of service Factory Method
            //Geting price of services according to the selected item
            Services service = ServicesFactory.getService(ddServiceList.SelectedItem.ToString());
            lblServiceCost.Text = service.CalculatePrice() + " €";
    
        }

        //Click button for service cost to bill
        private void button2_Click(object sender, EventArgs e)
        {
            if(booking.haveReservation(userID))
            {
              Services service = ServicesFactory.getService(ddServiceList.SelectedItem.ToString());
              bill.addBill(userID, service.CalculatePrice());
            }
            else
            {MessageBox.Show("You don't have any reservation yet.");}
        }

        //click button for modify search room
        private void button2_Click_1(object sender, EventArgs e)
        {
            DateTime startDate, endDate;
            string rmNumber;

            //Get value from GUI
            startDate = startDateModifyRes.Value;
            endDate = endDateModifyRes.Value;
            rmNumber = txtBxRoomNumber.Text;

            if (startDate < endDate)
            {
                if (booking.haveReservation(userID))
                {
                    //Set Values to booking class
                    booking.startDate = startDate;
                    booking.endDate = endDate;
                    booking.roomID = rmNumber;

                    //Calling modify reservation method by passing user ID
                    if (booking.modifyReservation(userID))
                    {
                        //MessageBox.Show("Reservation Modified.");
                        tabMain.SelectedTab = RoomSearchTab;
                    }
                    else
                    {
                        MessageBox.Show("Sorry No Rooms available for these dates.");
                    }
                }
                else
                { MessageBox.Show("You don't have any reservation yet."); }
            }
            else
                MessageBox.Show("Start Date and End Date are Wrong. Check Again.");
        }

        //button click when user cancel the reservation
        private void btnCancelReservation_Click(object sender, EventArgs e)
        {
            DateTime startDate, endDate;
            string rmNumber;

            //Get value from GUI
            startDate = stDateCancel.Value;
            endDate = edDateCancel.Value;
            rmNumber = txtbxRmNumCancel.Text;

            if (startDate < endDate)
            {
                if (booking.haveReservation(userID))
                {
                    //Set Values to booking class
                    booking.startDate = startDate;
                    booking.endDate = endDate;
                    booking.roomID = rmNumber;

                    //Calling modify reservation method by passing user ID
                    if (booking.cancelReservation(userID))
                    {
                        tabMain.SelectedTab = RoomSearchTab;
                    }
                    else
                    {
                        MessageBox.Show("Reservation Not Found");
                    }
                }
                else
                { MessageBox.Show("You don't have any reservation yet."); }
            }
            else
                MessageBox.Show("Start date or end date is Wrong . Check Again");
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            tabMain.SelectedTab = RoomSearchTab;
        }

        //Auto Check_In
        private void button3_Click(object sender, EventArgs e)
        {
            if (booking.AutoCheckIn(userID))
            {
                MessageBox.Show("You are checked In.");
                tabMain.SelectedTab = RoomSearchTab;
            }
            else
                MessageBox.Show("Sorry you don't have any reservation. Contact Staff");
        }

        //Auto Checkout Button
        private void button4_Click(object sender, EventArgs e)
        {
            if (booking.AutoCheckIn(userID))
            {
                MessageBox.Show("You are checked Out.");
                tabMain.SelectedTab = RoomSearchTab;
            }
            else
                MessageBox.Show("Error : Sorry for Inconvenience. Contact Staff Please");
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
 
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtRegPwd_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtRegUName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTelephoneNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtLName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtFName_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblcategory_Click(object sender, EventArgs e)
        {

        }

        private void lblregUName_Click(object sender, EventArgs e)
        {

        }

        private void lblLastName_Click(object sender, EventArgs e)
        {

        }

        private void lblRoomNo_Click(object sender, EventArgs e)
        {

        }

        private void lblempType_Click(object sender, EventArgs e)
        {

        }

        private void lblregPWD_Click(object sender, EventArgs e)
        {

        }

        private void lbltNumber_Click(object sender, EventArgs e)
        {

        }

        private void lbladdress_Click(object sender, EventArgs e)
        {

        }

        private void lblFName_Click(object sender, EventArgs e)
        {

        }

        private void ddRoomTypeSearch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ddHotelLocSearch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblStrtDate_Click(object sender, EventArgs e)
        {

        }

        private void lblEndDate_Click(object sender, EventArgs e)
        {

        }

        private void lblRmType_Click(object sender, EventArgs e)
        {

        }

        private void lblhotelLoc_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void lblPassword_Click(object sender, EventArgs e)
        {

        }

        private void txtbxUserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void tabSearchResult_Click(object sender, EventArgs e)
        {

        }

        private void lblARoomPrice_Click(object sender, EventArgs e)
        {

        }

        private void lblARooms_Click(object sender, EventArgs e)
        {

        }

        private void lblAEndDate_Click(object sender, EventArgs e)
        {

        }

        private void lblAStartDate_Click(object sender, EventArgs e)
        {

        }

        private void lblQhtLoc_Click(object sender, EventArgs e)
        {

        }

        private void lblQRoomPrice_Click(object sender, EventArgs e)
        {

        }

        private void lblQRooms_Click(object sender, EventArgs e)
        {

        }

        private void lblQRoomType_Click(object sender, EventArgs e)
        {

        }

        private void lblQStartDate_Click(object sender, EventArgs e)
        {

        }

        private void lblQEndDate_Click(object sender, EventArgs e)
        {

        }

        private void lblAhtLoc_Click(object sender, EventArgs e)
        {

        }

        private void lblARoomType_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        
        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabRoomReserve_Click(object sender, EventArgs e)
        {

        }

        private void ddPriceList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void lblQhtLocRes_Click(object sender, EventArgs e)
        {

        }

        private void lblQPrice_Click(object sender, EventArgs e)
        {

        }

        private void lblNoOfRoom_Click(object sender, EventArgs e)
        {

        }

        private void lblQRmType_Click(object sender, EventArgs e)
        {

        }

        private void lblQCheckIn_Click(object sender, EventArgs e)
        {

        }

        private void lblQCheckOut_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void ddRoomsAvailable_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel4_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

    }
}
