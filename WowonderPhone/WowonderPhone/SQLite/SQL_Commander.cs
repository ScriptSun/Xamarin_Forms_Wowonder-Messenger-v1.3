using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WowonderPhone.Classes;
using WowonderPhone.Dependencies;
using WowonderPhone.Pages;
using WowonderPhone.Pages.Tabs;
using WowonderPhone.SQLite.Tables;
using Xamarin.Forms;

namespace WowonderPhone.SQLite
{
    public class SQL_Commander
    {
        public static void InsertChatUsers(ChatActivityDB ChatActivity)
        {
           SQL_Entity.Connection.Insert(ChatActivity);
        }
        public static void InsertChatUsers(List<ChatActivityDB> ChatActivity)
        {
            SQL_Entity.Connection.Insert(ChatActivity);
        }
        public static void  UpdateChatUsers(ChatActivityDB ChatActivity)
        {
            SQL_Entity.Connection.Update(ChatActivity);
        }
        public static void DeleteChatUserRow(ChatActivityDB ChatActivity)
        {
            SQL_Entity.Connection.Delete(ChatActivity);
        }
        public static void ClearChatUserTable()
        {
            SQL_Entity.Connection.DeleteAll<ChatActivityDB>();
        }
        public static ChatActivityDB GetChatUser(string ID)
        {
            return SQL_Entity.Connection.Table<ChatActivityDB>().FirstOrDefault(c => c.UserID == ID);
        }
        public static ChatActivityDB GetChatUserByUsername(string Username)
        {
            return SQL_Entity.Connection.Table<ChatActivityDB>().FirstOrDefault(c => c.Username == Username);
        }
        public static ObservableCollection<ChatUsers> GetChatUsersCacheList()
        {
            try
            {
                var CachedChatlist = ChatActivityTab.ChatList;
                CachedChatlist.Clear();
                //var CacshedChatlist = new ObservableCollection<ChatUsers>();
                foreach (var Item in SQL_Entity.Connection.Table<ChatActivityDB>().ToList().OrderByDescending(a => a.TimeLong))

                    if (Item.SeenMessageOrNo == Settings.UnseenMesageColor)
                    {
                        if (Item.Isverifyed == null)
                        {
                            Item.Isverifyed = "False";
                        }
                        CachedChatlist.Insert(0, new ChatUsers()
                        {
                            Username = Item.Username,
                            profile_picture = ImageSource.FromFile(DependencyService.Get<IPicture>().GetPictureFromDisk(Item.ProfilePicture, Item.UserID)),
                            TextMessage = Item.TextMessage,
                            LastMessageDateTime = Item.LastMessageDateTime,
                            //verified = ChatUser_verified_bitmap,
                            SeenMessageOrNo = Item.SeenMessageOrNo,
                            ChekeSeen = Item.ChekeSeen,
                            UserID = Item.UserID,
                            lastseen = Item.lastseen,
                            Isverifyed = Item.Isverifyed,
                        });
                    }
                    else
                    {
                        if (Item.Isverifyed == null)
                        {
                            Item.Isverifyed = "False";
                        }

                        CachedChatlist.Add(new ChatUsers()
                        {
                            Username = Item.Username,
                            profile_picture = ImageSource.FromFile(DependencyService.Get<IPicture>().GetPictureFromDisk(Item.ProfilePicture, Item.UserID)),
                            TextMessage = Item.TextMessage,
                            LastMessageDateTime = Item.LastMessageDateTime,
                            //verified = ChatUser_verified_bitmap,
                            SeenMessageOrNo = Item.SeenMessageOrNo,
                            ChekeSeen = Item.ChekeSeen,
                            UserID = Item.UserID,
                            Isverifyed = Item.Isverifyed,
                        });
                    }



                return CachedChatlist;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static void DeletAllChatUsersList()
        {
            SQL_Entity.Connection.DeleteAll<ChatActivityDB>();
            foreach (var Activity in SQL_Entity.Connection.Table<ChatActivityDB>())
            {
                SQL_Entity.Connection.Delete(Activity);
            }
        }

        public static void InsertLoginCredentials(LoginTableDB Credentials)
        {
            SQL_Entity.Connection.Insert(Credentials);
        }
        public static void UpdateLoginCredentials(LoginTableDB Credentials)
        {
            SQL_Entity.Connection.Update(Credentials);
        }
        public static void DeleteLoginCredential(string Session)
        {
            var asd = SQL_Entity.Connection.Table<LoginTableDB>().FirstOrDefault(c => c.Session == Session);

            SQL_Entity.Connection.Delete(asd);
        }
        public static void ClearLoginCredentialsList()
        {
            SQL_Entity.Connection.DeleteAll<LoginTableDB>();
        }
        public static LoginTableDB GetLoginCredentials(string ID)
        {
            return SQL_Entity.Connection.Table<LoginTableDB>().FirstOrDefault(c => c.Status == ID);

        }
        public static LoginTableDB GetLoginCredentialsByUserID(string ID)
        {
            return SQL_Entity.Connection.Table<LoginTableDB>().FirstOrDefault(c => c.UserID == ID);

        }
        public static string GetLoginCredentialsStatus()
        {
            var Status = SQL_Entity.Connection.Table<LoginTableDB>().FirstOrDefault(c => c.Status == "Active" || c.Status == "Registered");
            var result = "NoSession";
            if (Status == null) { return result; }
            if (Status.Status != "Active") { result = "NoSession"; }
            else if (Status.Status == "Active") { result = "Active"; }

            if (Status.Status == "Registered")
            {
                result = "Registered";
            }
            else if (Status.Status != "Registered" && Status.Status != "Active")
            {
                result = "NoSession";
            }
            return result;
        }
        public static List<LoginTableDB> GetLoginCredentialsList()
        {

            return SQL_Entity.Connection.Table<LoginTableDB>().ToList();
        }
              
        public static void InsertContactUsers(ContactsTableDB ContactsTable)
        {
            SQL_Entity.Connection.Insert(ContactsTable);
        }
        public static void InsertContactUsers(List<ContactsTableDB> ContactsTable)
        {
            SQL_Entity.Connection.Insert(ContactsTable);
        }
        public static void UpdateContactUsers(ContactsTableDB ContactsTable)
        {
            SQL_Entity.Connection.Update(ContactsTable);
        }
        public static void DeleteContactRow(ContactsTableDB ContactsTable)
        {
            SQL_Entity.Connection.Delete(ContactsTable);
        }
        public static void ClearContactTable()
        {
            SQL_Entity.Connection.DeleteAll<ContactsTableDB>();
        }
        public static ContactsTableDB GetContactUser(string ID)
        {
            return SQL_Entity.Connection.Table<ContactsTableDB>().FirstOrDefault(c => c.UserID == ID);
        }
        public static ContactsTableDB GetContactUserByUsername(string Username)
        {
            return SQL_Entity.Connection.Table<ContactsTableDB>().FirstOrDefault(c => c.Username == Username);
        }
        public static ObservableCollection<ChatContacts> GetContactCacheList()
        {
            try
            {
                var CachedContactlist = ContactsTab.ChatContactsList;
                CachedContactlist.Clear();

                foreach (var Item in SQL_Entity.Connection.Table<ContactsTableDB>().ToList())
                {
                    CachedContactlist.Add(new ChatContacts()
                    {
                        Username = Item.Username,
                        profile_picture = ImageSource.FromFile(DependencyService.Get<IPicture>().GetPictureFromDisk(Item.ProfilePicture, Item.UserID)),
                        TextMessage = Item.TextMessage,
                        LastMessageDateTime = Item.LastMessageDateTime,
                        //verified = ChatUser_verified_bitmap,
                        UserID = Item.UserID,
                        Platform = Item.Platform
                    });

                }
                return CachedContactlist;
            }
            catch
            {
                return null;
            }
        }
        public static ObservableCollection<ChatContacts> GetContactSearchList(string name)
        {
            try
            {
                var SearchContactlist = SearchPage.ChatContactsList;
                if (SearchContactlist.Count > 0)
                {
                    SearchContactlist.Clear();
                }

                foreach (var Itemss in SQL_Entity.Connection.Table<ContactsTableDB>().ToList().Where(a => a.Name.ToLower().Contains(name)))
                {
                    SearchContactlist.Add(new ChatContacts()
                    {
                        Username = Itemss.Username,
                        profile_picture =
                            ImageSource.FromFile(
                                DependencyService.Get<IPicture>()
                                    .GetPictureFromDisk(Itemss.ProfilePicture, Itemss.UserID)),
                        TextMessage = Itemss.TextMessage,
                        LastMessageDateTime = Itemss.LastMessageDateTime,
                        //verified = ChatUser_verified_bitmap,
                        UserID = Itemss.UserID
                    });
                }

                return SearchContactlist;
            }
            catch
            {
                return null;
            }
        }
        public static void DeletAllChatContacsList()
        {
            SQL_Entity.Connection.DeleteAll<ContactsTableDB>();
            foreach (var contact in SQL_Entity.Connection.Table<ContactsTableDB>().ToList())
            {
                SQL_Entity.Connection.Delete(contact);
            }
        }

        public static void InsertProfileCredentials(LoginUserProfileTableDB ProfileCredentials)
        {
            SQL_Entity.Connection.Insert(ProfileCredentials);
        }
        public static void UpdateProfileCredentials(LoginUserProfileTableDB ProfileCredentials)
        {
            SQL_Entity.Connection.Update(ProfileCredentials);
        }
        public static void DeleteProfileRow(LoginUserProfileTableDB ProfileCredentials)
        {
            SQL_Entity.Connection.Delete(ProfileCredentials);
        }
        public static void DeleteProfileCredential(string userid)
        {
            var asd = SQL_Entity.Connection.Table<LoginTableDB>().FirstOrDefault(c => c.UserID == userid);

            SQL_Entity.Connection.Delete(asd);
        }
        public static void ClearProfileCredentialsList()
        {
            SQL_Entity.Connection.DeleteAll<LoginUserProfileTableDB>();
        }
        public static LoginUserProfileTableDB GetProfileCredentialsById(string ID)
        {
            return SQL_Entity.Connection.Table<LoginUserProfileTableDB>().FirstOrDefault(c => c.UserID == ID);

        }
        public static string GetProfileCredentials()
        {
            var Status = SQL_Entity.Connection.Table<LoginUserProfileTableDB>().FirstOrDefault(c => c.status == "Active" || c.status == "Registered");
            var result = "NoSession";
            if (Status == null) { return result; }
            if (Status.status != "Active") { result = "NoSession"; }
            else if (Status.status == "Active") { result = "Active"; }

            if (Status.status == "Registered")
            {
                result = "Registered";
            }
            else if (Status.status != "Registered")
            {
                result = "NoSession";
            }
            return result;

        }
        public static List<LoginUserProfileTableDB> GetProfileCredentialsList()
        {

            return SQL_Entity.Connection.Table<LoginUserProfileTableDB>().ToList();
        }
               
        public static void InsertMessage(MessagesDB MessagesDB)
        {

            SQL_Entity.Connection.Insert(MessagesDB);

        }
        public static string CheckMessage(string messageid)
        {
            try
            {
                var ss = SQL_Entity.Connection.ExecuteScalar<int>("SELECT COUNT(ID) FROM MessagesDB WHERE message_id =" + messageid);
                if (ss == 0)
                {
                    return "0";
                }
                return "1";
            }
            catch (Exception)
            {
                return "1";
            }

        }
        public static void UpdateMessage(MessagesDB MessagesDB)
        {
            SQL_Entity.Connection.Update(MessagesDB);
        }
        public static void DeleteMessage(string from_id, string to_id)
        {
            try
            {
                SQL_Entity.Connection.Query<MessagesDB>("Delete FROM MessagesDB WHERE ((from_id =" + from_id + " and to_id=" + to_id + ") OR (from_id =" + to_id + " and to_id=" + from_id + "))");
            }
            catch (Exception)
            {
            }
        }
        public static void ClearMessageList()
        {
            SQL_Entity.Connection.DeleteAll<MessagesDB>();
            //Connection.DropTable<MessagesDB>();

        }
        public static MessagesDB GetMessages(string ID)
        {
            return SQL_Entity.Connection.Table<MessagesDB>().FirstOrDefault(c => c.from_id == ID);

        }
        public static MessagesDB GetMessagesbyMessageID(int ID)
        {
            return SQL_Entity.Connection.Table<MessagesDB>().FirstOrDefault(c => c.message_id == ID);

        }
        public static string GetMessages()
        {

            return "";
        }
        public static IEnumerable<MessagesDB> GetMessageList(string from_id, string to_id, string before_message_id)
        {
            try
            {
                var before_q = "";
                if (before_message_id != "0")
                {
                    before_q = "AND message_id < " + before_message_id + " AND message_id <> " + before_message_id + " ";
                }

                var ss1 = SQL_Entity.Connection.Query<MessagesDB>("SELECT * FROM MessagesDB WHERE ((from_id =" + from_id + " and to_id=" + to_id + ") OR (from_id =" + to_id + " and to_id=" + from_id + ")) " + before_q);

                var query_limit_from = ss1.Count - 25;
                if (query_limit_from < 1)
                {
                    query_limit_from = 0;
                }

                var Query = "SELECT * FROM MessagesDB WHERE ((from_id =" + from_id + " and to_id=" + to_id +
                          ") OR (from_id =" + to_id + " and to_id=" + from_id + ")) " + before_q +
                          " ORDER BY message_id ASC LIMIT " + query_limit_from + ", 25";

                var Result = SQL_Entity.Connection.Query<MessagesDB>(Query);

                return Result;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static void InsertNotifiDBCredentials(NotifiDB NotifiDB)
        {
            SQL_Entity.Connection.Insert(NotifiDB);
        }
        public static void UpdateNotifiDBCredentials(NotifiDB NotifiDB)
        {
            SQL_Entity.Connection.Update(NotifiDB);
        }
        public static void DeleteNotifiDBCredential(int Id)
        {
            var asd = SQL_Entity.Connection.Table<NotifiDB>().FirstOrDefault(c => c.ID == Id);

            SQL_Entity.Connection.Delete(asd);
        }
        public static void ClearNotifiDBCredentialsList()
        {
            SQL_Entity.Connection.DeleteAll<NotifiDB>();
        }
        public static NotifiDB GetNotifiDBCredentialsById(int ID)
        {
            return SQL_Entity.Connection.Table<NotifiDB>().FirstOrDefault(c => c.messageid == ID);

        }
              
        public static void InsertPrivacyDBCredentials(PrivacyDB PrivacyDB)
        {
            SQL_Entity.Connection.Insert(PrivacyDB);
        }
        public static void UpdatePrivacyDBCredentials(PrivacyDB PrivacyDB)
        {
            var PRV = SQL_Entity.Connection.Table<PrivacyDB>().FirstOrDefault(c => c.UserID == PrivacyDB.UserID);
            PRV.UserID = PrivacyDB.UserID;
            PRV.WhoCanFollowMe = PrivacyDB.WhoCanFollowMe;
            PRV.WhoCanMessageMe = PrivacyDB.WhoCanMessageMe;
            PRV.WhoCanSeeMyBirday = PrivacyDB.WhoCanSeeMyBirday;
            SQL_Entity.Connection.Update(PRV);
        }
        public static void DeletePrivacyDBRow(PrivacyDB PrivacyDB)
        {
            SQL_Entity.Connection.Delete(PrivacyDB);
        }
        public static void DeletePrivacyDBCredential(string userid)
        {
            var asd = SQL_Entity.Connection.Table<PrivacyDB>().FirstOrDefault(c => c.UserID == userid);

            SQL_Entity.Connection.Delete(asd);
        }
        public static void ClearPrivacyDBCredentialsList()
        {
            SQL_Entity.Connection.DeleteAll<PrivacyDB>();
        }
        public static PrivacyDB GetPrivacyDBCredentialsById(string ID)
        {
            return SQL_Entity.Connection.Table<PrivacyDB>().FirstOrDefault(c => c.UserID == ID);

        }

        public static void InsertSearchFilter(SearchFilterDB SearchFilterDB)
        {
            SQL_Entity.Connection.Insert(SearchFilterDB);
        }
        public static void InsertSearchFilter(List<SearchFilterDB> SearchFilterDB)
        {
            SQL_Entity.Connection.Insert(SearchFilterDB);
        }
        public static void UpdateSearchFilter(SearchFilterDB SearchFilterDB)
        {
            SQL_Entity.Connection.Update(SearchFilterDB);
        }
        public static void DeleteSearchFilter(SearchFilterDB SearchFilterDB)
        {
            SQL_Entity.Connection.Delete(SearchFilterDB);
        }
        public static void ClearSearchFilterTable()
        {
            SQL_Entity.Connection.DeleteAll<SearchFilterDB>();
        }
        public static SearchFilterDB GetSearchFilterById(string ID)
        {
            return SQL_Entity.Connection.Table<SearchFilterDB>().FirstOrDefault(c => c.UserID == ID);
        }
        public static SearchFilterDB GetSearchFilterUsername(string Username)
        {
            return SQL_Entity.Connection.Table<SearchFilterDB>().FirstOrDefault(c => c.UserID == Username);
        }
    }         
}
