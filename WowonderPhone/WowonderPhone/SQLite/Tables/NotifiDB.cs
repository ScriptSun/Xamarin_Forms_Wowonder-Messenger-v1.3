using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace WowonderPhone.SQLite.Tables
{
    public class NotifiDB
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int messageid { get; set; }
        public int Seen { get; set; }
    }
}
