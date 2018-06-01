using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;
using WowonderPhone.Droid.SQLite;
using WowonderPhone.SQLite;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;


[assembly: Dependency(typeof(WowonderPhone.Droid.SQLite.SQLiteConnect))]

namespace WowonderPhone.Droid.SQLite
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
                    DirectoriPrivate = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

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
                    Platforma = new SQLitePlatformAndroid();

                }
                return Platforma;
            }
        }
    }
}