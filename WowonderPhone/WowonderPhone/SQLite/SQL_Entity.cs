using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WowonderPhone.SQLite.Tables;
using Xamarin.Forms;

namespace WowonderPhone.SQLite
{
    public class SQL_Entity
    {
        public static SQLiteConnection Connection;

        public static void Connect()
        {
            var config = DependencyService.Get<SQLiteData>();
            Connection = new SQLiteConnection(config.Platform, Path.Combine(config.DirectoriDB, "MainDataSocial.db3"));

            Connection.CreateTable<LoginTableDB>();
            Connection.CreateTable<NotifiDB>();
            Connection.CreateTable<SearchFilterDB>();
            Connection.CreateTable<PrivacyDB>();
            Connection.CreateTable<MessagesDB>();
            Connection.CreateTable<LoginUserProfileTableDB>();
            Connection.CreateTable<ContactsTableDB>();
            Connection.CreateTable<ChatActivityDB>();
        }

    }
}
