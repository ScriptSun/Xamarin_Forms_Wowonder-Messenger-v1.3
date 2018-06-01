using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WowonderPhone.SQLite.Tables
{
   public class ChatActivityDB
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public string SeenMessageOrNo { get; set; }
        public string Url { get; set; }
        public string TextMessage { get; set; }
        public string LastMessageDateTime { get; set; }
        public string ChekeSeen { get; set; }
        public string Notifi { get; set; }
        public string lastseen { get; set; }
        public string TimeLong { get; set; }
        public string Isverifyed { get; set; }
       
        public override string ToString()
        {
            return string.Format("UserID : {0}, Username : {1}, Name: {2} , SeenMessageOrNo : {3}, LastMessageDateTime: {4}, TextMessage: {5}", UserID, Username, Name, SeenMessageOrNo, LastMessageDateTime, TextMessage);
        }
    }
}
