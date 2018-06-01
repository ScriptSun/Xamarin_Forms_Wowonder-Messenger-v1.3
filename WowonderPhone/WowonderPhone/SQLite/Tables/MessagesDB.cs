using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace WowonderPhone.SQLite.Tables
{
    public class MessagesDB
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string contactNumber { get; set; }
        public string contactName { get; set; }
        public int message_id { get; set; }
        public string from_id { get; set; }
        public string to_id { get; set; }
        public string text { get; set; }
        public string media { get; set; }
        public string ImageMedia { get; set; }
        public string mediafilename { get; set; }
        public string mediafilenameS { get; set; }
        public string time { get; set; }
        public string seen { get; set; }
        public string deleted_one { get; set; }
        public string deleted_two { get; set; }
        public string type { get; set; }
        public string position { get; set; }
        public string avatar { get; set; }
        public string MediaName { get; set; }
        public string DownloadFileUrl { get; set; }
       
       

        public override string ToString()
        {
            return string.Format("Status : {0}, ID : {1}, message_id: {2} , from_id : {3}, to_id: {4}, time: {5}, contactNumber: {6}, contactName: {7}", ID, message_id, from_id, to_id, time, contactNumber, contactName);
        }
    }
}
