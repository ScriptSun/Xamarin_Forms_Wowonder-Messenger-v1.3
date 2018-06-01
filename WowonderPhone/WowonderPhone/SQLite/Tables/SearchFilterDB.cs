using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace WowonderPhone.SQLite.Tables
{
    public class SearchFilterDB
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string UserID { get; set; }
        public int Gender { get; set; }
        public int ProfilePicture { get; set; }
        public int Status { get; set; }
       

        public override string ToString()
        {
            return string.Format("UserID : {0}, Gender : {1}, ProfilePicture: {2} , Status : {3}", UserID, Gender, ProfilePicture, Status);
        }

    }
}
