using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite.Net.Interop;

using WowonderPhone.iOS.SQLite;
using WowonderPhone.SQLite;
using SQLite.Net.Platform.XamarinIOS;
using Xamarin.Forms;


[assembly: Dependency(typeof(WowonderPhone.iOS.SQLite.SQLiteConnect))]

namespace WowonderPhone.iOS.SQLite
{
    class SQLiteConnect : SQLiteData
    {
        private string DirectoriPrivate;
        private ISQLitePlatform Platforma;

        public string DirectoriDB
        {
            get
            {
                if (string.IsNullOrEmpty(DirectoriPrivate))
                {
                    var der = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    DirectoriPrivate = System.IO.Path.Combine(der,".." ,"Library");
                }
                return DirectoriPrivate;
            }
        }

        public ISQLitePlatform Platform
        {
            get
            {
                if (Platforma == null)
                {
                    Platforma = new SQLitePlatformIOS();
                }
                return Platforma;
            }
        }
    }
}