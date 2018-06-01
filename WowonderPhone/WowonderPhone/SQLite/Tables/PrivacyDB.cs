using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace WowonderPhone.SQLite.Tables
{
   public class PrivacyDB
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string UserID { get; set; }
        public int WhoCanFollowMe { get; set; }
        public int WhoCanMessageMe { get; set; }
        public int WhoCanSeeMyBirday { get; set; }

        public override string ToString()
        {
            return string.Format("UserID : {0}, WhoCanFollowMe : {1}, WhoCanMessageMe: {2} , WhoCanSeeMyBirday : {3}", UserID, WhoCanFollowMe, WhoCanMessageMe, WhoCanSeeMyBirday);
        }
    }
}
