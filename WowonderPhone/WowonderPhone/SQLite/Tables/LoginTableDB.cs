using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WowonderPhone.SQLite.Tables
{
   public class LoginTableDB
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Session { get; set; }
        public string UserID { get; set; }
        public string Status { get; set; } 
        public string MyConnectionStatus { get; set; } = "0";
        public string Onesignal_APP_ID { get; set; }
        
        public bool NotificationVibrate { get; set; } = true;
        public bool NotificationSound { get; set; } = true;
        public bool NotificationPopup { get; set; } = true;
        public string NotificationLedColor { get; set; }
        public string NotificationLedColorName { get; set; }

        public override string ToString()
        {
            return string.Format("Status : {0}, ID : {1}, Username: {2} , Password : {3}, Session: {4}, UserID: {5}", Status, ID, Username, Password, Session, UserID);
        }
    }
}
