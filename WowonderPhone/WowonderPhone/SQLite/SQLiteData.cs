using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WowonderPhone.SQLite
{
    public interface SQLiteData
    {
        string DirectoriDB { get; }
        ISQLitePlatform Platform { get; }
    }
}
