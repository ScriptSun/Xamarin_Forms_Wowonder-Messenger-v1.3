using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WowonderPhone.SQLite.Tables
{
    public class LoginUserProfileTableDB
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Avatar { get; set; }
        public string Cover { get; set; }
        public string Relationship_id { get; set; }
        public string Address { get; set; }
        public string Working { get; set; }
        public string Working_link { get; set; }
        public string About { get; set; }
        public string School { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Google { get; set; }
        public string Twitter { get; set; }
        public string Linkedin { get; set; }
        public string vk { get; set; }
        public string instagram { get; set; }
        public string language { get; set; }
        public string ip_address { get; set; }
        public string Verified { get; set; }
        public string lastseen { get; set; }
        public string showlastseen { get; set; }
        public string status { get; set; }
        public string active { get; set; }
        public string admin { get; set; }
        public string registered { get; set; }
        public string Phone_number { get; set; }
        public string is_pro { get; set; }
        public string pro_type { get; set; }
        public string joined { get; set; }
        public string timezone { get; set; }
        public string Url { get; set; }
        public string name { get; set; }
        public string Youtube { get; set; }

        public override string ToString()
        {
            return string.Format("Status : {0}, ID : {1}, Username: {2} , Password : {3}, Session: {4}, UserID: {5}", status, ID, Username, Email, Avatar, UserID);
        }
    }
}
