using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Plugin.Contacts;
using Plugin.Contacts.Abstractions;
using WowonderPhone.Classes;
using WowonderPhone.Dependencies;
using WowonderPhone.Pages;
using WowonderPhone.Pages.Tabs;
using WowonderPhone.SQLite;
using Xamarin.Forms;
using WowonderPhone.SQLite.Tables;
using Acr.UserDialogs;
using WowonderPhone.Languish;

namespace WowonderPhone.Controls
{
    public class Functions
    {
        
       
        public static ObservableCollection<GroupedChatList> grouped = new ObservableCollection<GroupedChatList>();
        public static ObservableCollection<GroupedRandomContacts> groupedRandomlist =new ObservableCollection<GroupedRandomContacts>();
        public static ObservableCollection<SearchResult> SearchFilterlist = new ObservableCollection<SearchResult>();

        public static ObservableCollection<MessageViewModal> Messages = new ObservableCollection<MessageViewModal>();
        public static string NumberOfcontacts;
        public static string Aftercontact = "0";





        public static async Task<string> GetChatContacts(string userid, string session)
        {
            #region Client Respone

            try
            {
                using (var client = new HttpClient())
                {

                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user_id", Settings.User_id),
                        new KeyValuePair<string, string>("user_profile_id", Settings.User_id),
                        new KeyValuePair<string, string>("s", Settings.Session),
                        new KeyValuePair<string, string>("after_user_id", Aftercontact),
                        new KeyValuePair<string, string>("list_type", "all")

                    });
                    var response =
                        await
                            client.PostAsync(Settings.Website + "/app_api.php?application=phone&type=get_users_friends",
                                formContent).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string apiStatus = data["api_status"].ToString();
                    string ThemeUrl = data["theme_url"].ToString();

                    #endregion

                    if (apiStatus == "200")
                    {

                        var users = JObject.Parse(json).SelectToken("users").ToString();
                        var usersOnline = JObject.Parse(json).SelectToken("online").ToString();
                        JArray ChatusersOnlineGroup = JArray.Parse(usersOnline);
                        JArray Chatusers = JArray.Parse(users);

                        if (Chatusers.Count == 0)
                        {
                            Aftercontact = "0";
                            ContactsTab.ChatContactsList.Add(new ChatContacts()
                            {
                                Username = "  " + "  " + "  " + AppResources.FN_NoUsersAreAvailble,

                                Name = "  " + "  " + "  " + AppResources.FN_NoUsersAreAvailble

                            });

                          

                            return null;
                        }
                        ContactsTab.ChatContactsList.Clear();

                        foreach (var Onlines in ChatusersOnlineGroup)
                        {
                            JObject ChatlistUserdata = JObject.FromObject(Onlines);
                            var ChatUser_User_ID = ChatlistUserdata["user_id"].ToString();
                            var ChatUser_avatar = ChatlistUserdata["profile_picture"].ToString();
                            var ChatUser_name = ChatlistUserdata["name"].ToString();
                            var ChatUser_lastseen = ChatlistUserdata["lastseen"].ToString();
                            var ChatUser_lastseen_Time_Text = ChatlistUserdata["lastseen_time_text"].ToString();
                            var ChatUser_verified = ChatlistUserdata["verified"].ToString();
                            var UserPlatform = ChatlistUserdata["user_platform"].ToString();

                            Aftercontact = ChatUser_User_ID;

                            if (UserPlatform == "phone")
                            {
                                UserPlatform = "Mobile";
                            }
                            if (UserPlatform == "web")
                            {
                                UserPlatform = "Web";
                            }
                            if (UserPlatform == "windows")
                            {
                                UserPlatform = "Desktop";
                            }

                            #region Saving image

                            var ImageMediaFile =
                                ImageSource.FromFile(DependencyService.Get<IPicture>()
                                    .GetPictureFromDisk(ChatUser_avatar, ChatUser_User_ID));
                            if (DependencyService.Get<IPicture>().GetPictureFromDisk(ChatUser_avatar, ChatUser_User_ID) == "File Dont Exists")
                            {
                                ImageMediaFile = "loading.jpg";
                                DependencyService.Get<IPicture>().SavePictureToDisk(ChatUser_avatar, ChatUser_User_ID);
                            }

                            var OnlineOfflineIcon = ImageSource.FromFile("");
                            if (Settings.Show_Online_Oflline_Icon)
                            {
                                if (ChatUser_lastseen == "on")
                                {
                                    OnlineOfflineIcon =
                                        ImageSource.FromFile(
                                            DependencyService.Get<IPicture>()
                                                .GetPictureFromDisk(ThemeUrl + "/img/windows_app/online.png", "Icons"));
                                    if (
                                        DependencyService.Get<IPicture>()
                                            .GetPictureFromDisk(ThemeUrl + "/img/windows_app/online.png", "Icons") ==
                                        "File Dont Exists")
                                    {
                                        OnlineOfflineIcon = new UriImageSource
                                        {
                                            Uri = new Uri(ThemeUrl + "/img/windows_app/online.png")
                                        };
                                        DependencyService.Get<IPicture>()
                                            .SavePictureToDisk(ThemeUrl + "/img/windows_app/online.png", "Icons");
                                    }

                                }
                                else
                                {
                                    OnlineOfflineIcon =
                                        ImageSource.FromFile(
                                            DependencyService.Get<IPicture>()
                                                .GetPictureFromDisk(ThemeUrl + "/img/windows_app/offline.png", "Icons"));
                                    if (
                                        DependencyService.Get<IPicture>()
                                            .GetPictureFromDisk(ThemeUrl + "/img/windows_app/offline.png", "Icons") ==
                                        "File Dont Exists")
                                    {
                                        OnlineOfflineIcon = new UriImageSource
                                            { Uri = new Uri(ThemeUrl + "/img/windows_app/offline.png") };
                                        DependencyService.Get<IPicture>()
                                            .SavePictureToDisk(ThemeUrl + "/img/windows_app/offline.png", "Icons");
                                    }
                                }

                            }

                            #endregion

                            if (ChatUser_lastseen == "on")
                            {
                                ContactsTab.ChatContactsList.Add(new ChatContacts()
                                {
                                    Username = ChatUser_name,
                                    lastseen = OnlineOfflineIcon,
                                    Name = ChatUser_name,
                                    SeenMessageOrNo = ChatUser_lastseen,
                                    profile_picture = ImageMediaFile,
                                    LastMessageDateTime = AppResources.Label_Online,
                                    //verified = ChatUser_verified_bitmap,
                                    UserID = ChatUser_User_ID,
                                    Platform = UserPlatform
                                });

                            }
                            #region adding to Sqlite table  

                           
                                var contact = SQL_Commander.GetContactUser(ChatUser_User_ID);

                                #region Update contact information

                                if (contact != null)
                                {
                                    if (contact.UserID == ChatUser_User_ID &&
                                        ((contact.Name != ChatUser_name) || (contact.ProfilePicture != ChatUser_avatar) ||
                                         (contact.Username != ChatUser_name) ||
                                         (contact.LastMessageDateTime != ChatUser_lastseen_Time_Text) ||
                                         (contact.Platform != UserPlatform)))
                                    {
                                        if ((contact.ProfilePicture != ChatUser_avatar))
                                        {
                                            SQL_Commander.DeleteContactRow(contact);
                                            DependencyService.Get<IPicture>()
                                                .DeletePictureFromDisk(contact.ProfilePicture, ChatUser_User_ID);
                                            SQL_Commander.InsertContactUsers(new ContactsTableDB()
                                            {
                                                UserID = ChatUser_User_ID,
                                                Name = ChatUser_name,
                                                ProfilePicture = ChatUser_avatar,
                                                SeenMessageOrNo = ChatUser_lastseen,
                                                LastMessageDateTime = ChatUser_lastseen_Time_Text,
                                                Username = ChatUser_name,
                                                Platform = UserPlatform
                                            });
                                        }

                                        contact.UserID = ChatUser_User_ID;
                                        contact.Name = ChatUser_name;
                                        contact.ProfilePicture = ChatUser_avatar;
                                        contact.SeenMessageOrNo = ChatUser_lastseen;
                                        contact.LastMessageDateTime = ChatUser_lastseen_Time_Text;
                                        contact.Username = ChatUser_name;
                                        contact.Platform = UserPlatform;

                                        SQL_Commander.UpdateContactUsers(contact);


                                    }

                                }
                                #endregion

                                #region Add contact if dont exits

                                else
                                {
                                    SQL_Commander.InsertContactUsers(new ContactsTableDB()
                                    {
                                        UserID = ChatUser_User_ID,
                                        Name = ChatUser_name,
                                        ProfilePicture = ChatUser_avatar,
                                        SeenMessageOrNo = ChatUser_lastseen,
                                        LastMessageDateTime = ChatUser_lastseen_Time_Text,
                                        Username = ChatUser_name,
                                        Platform = UserPlatform
                                    });

                                }

                                #endregion

                            

                            #endregion
                        }


                        foreach (var Offline in Chatusers)
                        {
                            JObject ChatlistUserdata = JObject.FromObject(Offline);
                            var ChatUser_User_ID = ChatlistUserdata["user_id"].ToString();
                            var ChatUser_avatar = ChatlistUserdata["profile_picture"].ToString();
                            var ChatUser_name = ChatlistUserdata["name"].ToString();
                            var ChatUser_lastseen = ChatlistUserdata["lastseen"].ToString();
                            var ChatUser_lastseen_Time_Text = ChatlistUserdata["lastseen_time_text"].ToString();
                            var ChatUser_verified = ChatlistUserdata["verified"].ToString();
                            var UserPlatform = ChatlistUserdata["user_platform"].ToString();
                            Aftercontact = ChatUser_User_ID;


                            if (UserPlatform == "phone")
                            {
                                UserPlatform = "Mobile";
                            }
                            if (UserPlatform == "web")
                            {
                                UserPlatform = "Web";
                            }
                            if (UserPlatform == "windows")
                            {
                                UserPlatform = "Desktop";
                            }


                            #region Saving image

                            var ImageMediaFile =
                                ImageSource.FromFile(DependencyService.Get<IPicture>()
                                    .GetPictureFromDisk(ChatUser_avatar, ChatUser_User_ID));
                            if (
                                DependencyService.Get<IPicture>().GetPictureFromDisk(ChatUser_avatar, ChatUser_User_ID) ==
                                "File Dont Exists")
                            {
                                ImageMediaFile = "loading.jpg";
                                DependencyService.Get<IPicture>().SavePictureToDisk(ChatUser_avatar, ChatUser_User_ID);
                            }
                            var OnlineOfflineIcon = ImageSource.FromFile("");
                            if (Settings.Show_Online_Oflline_Icon)
                            {
                                if (ChatUser_lastseen == "on")
                                {
                                    OnlineOfflineIcon =
                                        ImageSource.FromFile(
                                            DependencyService.Get<IPicture>()
                                                .GetPictureFromDisk(ThemeUrl + "/img/windows_app/online.png", "Icons"));
                                    if (
                                        DependencyService.Get<IPicture>()
                                            .GetPictureFromDisk(ThemeUrl + "/img/windows_app/online.png", "Icons") ==
                                        "File Dont Exists")
                                    {
                                        OnlineOfflineIcon = new UriImageSource
                                        {
                                            Uri = new Uri(ThemeUrl + "/img/windows_app/online.png")
                                        };
                                        DependencyService.Get<IPicture>()
                                            .SavePictureToDisk(ThemeUrl + "/img/windows_app/online.png", "Icons");
                                    }

                                }
                                else
                                {
                                    OnlineOfflineIcon =
                                        ImageSource.FromFile(
                                            DependencyService.Get<IPicture>()
                                                .GetPictureFromDisk(ThemeUrl + "/img/windows_app/offline.png", "Icons"));
                                    if (
                                        DependencyService.Get<IPicture>()
                                            .GetPictureFromDisk(ThemeUrl + "/img/windows_app/offline.png", "Icons") ==
                                        "File Dont Exists")
                                    {
                                        OnlineOfflineIcon = new UriImageSource
                                            { Uri = new Uri(ThemeUrl + "/img/windows_app/offline.png") };
                                        DependencyService.Get<IPicture>()
                                            .SavePictureToDisk(ThemeUrl + "/img/windows_app/offline.png", "Icons");
                                    }
                                }

                            }

                            #endregion


                            #region Data Adding


                            ContactsTab.ChatContactsList.Add(new ChatContacts()
                            {
                                Username = ChatUser_name,
                                lastseen = OnlineOfflineIcon,
                                Name = ChatUser_name,
                                SeenMessageOrNo = ChatUser_lastseen,
                                profile_picture = ImageMediaFile,
                                LastMessageDateTime = AppResources.Label_LastSeen + " " + ChatUser_lastseen_Time_Text,
                                //verified = ChatUser_verified_bitmap,
                                UserID = ChatUser_User_ID,
                                Platform = UserPlatform
                            });



                            #region adding to Sqlite table  

                           
                                var contact = SQL_Commander.GetContactUser(ChatUser_User_ID);

                                #region Update contact information

                                if (contact != null)
                                {
                                    if (contact.UserID == ChatUser_User_ID &&
                                        ((contact.Name != ChatUser_name) || (contact.ProfilePicture != ChatUser_avatar) ||
                                         (contact.Username != ChatUser_name) ||
                                         (contact.LastMessageDateTime != ChatUser_lastseen_Time_Text) ||
                                         (contact.Platform != UserPlatform)))
                                    {
                                        if ((contact.ProfilePicture != ChatUser_avatar))
                                        {
                                            SQL_Commander.DeleteContactRow(contact);
                                            DependencyService.Get<IPicture>()
                                                .DeletePictureFromDisk(contact.ProfilePicture, ChatUser_User_ID);
                                            SQL_Commander.InsertContactUsers(new ContactsTableDB()
                                            {
                                                UserID = ChatUser_User_ID,
                                                Name = ChatUser_name,
                                                ProfilePicture = ChatUser_avatar,
                                                SeenMessageOrNo = ChatUser_lastseen,
                                                LastMessageDateTime = ChatUser_lastseen_Time_Text,
                                                Username = ChatUser_name,
                                                Platform = UserPlatform
                                            });
                                        }

                                        contact.UserID = ChatUser_User_ID;
                                        contact.Name = ChatUser_name;
                                        contact.ProfilePicture = ChatUser_avatar;
                                        contact.SeenMessageOrNo = ChatUser_lastseen;
                                        contact.LastMessageDateTime = ChatUser_lastseen_Time_Text;
                                        contact.Username = ChatUser_name;
                                        contact.Platform = UserPlatform;

                                        SQL_Commander.UpdateContactUsers(contact);


                                    }

                                }
                                #endregion

                                #region Add contact if dont exits

                                else
                                {
                                    SQL_Commander.InsertContactUsers(new ContactsTableDB()
                                    {
                                        UserID = ChatUser_User_ID,
                                        Name = ChatUser_name,
                                        ProfilePicture = ChatUser_avatar,
                                        SeenMessageOrNo = ChatUser_lastseen,
                                        LastMessageDateTime = ChatUser_lastseen_Time_Text,
                                        Username = ChatUser_name,
                                        Platform = UserPlatform
                                    });

                                }

                                #endregion

                            }

                            #endregion

                            #endregion

                        

                    }
                    else if (apiStatus == "400")
                    {
                        ContactsTab.ChatContactsList.Add(new ChatContacts()
                        {
                            Username = "  " + "  " + "  " + AppResources.FN_NoUsersAreAvailble,

                            Name = "  " + "  " + "  " + AppResources.FN_NoUsersAreAvailble

                        });
                        json = AppResources.Label_Error;
                    }

                    return json;

                }
            }
            catch (Exception)
            {
                return AppResources.Label_Error;
            }

        }




        public static async Task<string> GetLoginUserProfile(string userid, string session)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user_id", Settings.User_id),
                        new KeyValuePair<string, string>("user_profile_id", userid),
                        new KeyValuePair<string, string>("s", Settings.Session)
                    });

                    var response =
                        await
                            client.PostAsync(Settings.Website + "/app_api.php?application=phone&type=get_user_data",
                                formContent);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string apiStatus = data["api_status"].ToString();
                    if (apiStatus == "200")
                    {
                        JObject userdata = JObject.FromObject(data["user_data"]);
                        Settings.UserFullName = userdata["name"].ToString();

                        var avatar = userdata["avatar"].ToString();
                        var cover = userdata["cover"].ToString();
                        var First_name = userdata["first_name"].ToString();
                        var Last_name = userdata["last_name"].ToString();
                        var About = userdata["about"].ToString();
                        var Website = userdata["website"].ToString();
                        var School = userdata["school"].ToString();
                        var name = userdata["name"].ToString();
                        var username = userdata["username"].ToString();
                        var gender = userdata["gender"].ToString();
                        var birthday = userdata["birthday"].ToString();
                        var email = userdata["email"].ToString();
                        var address = userdata["address"].ToString();
                        var user_id = userdata["user_id"].ToString();
                        var url = userdata["url"].ToString();

                        About = System.Net.WebUtility.HtmlDecode(About);
                        address = System.Net.WebUtility.HtmlDecode(address);

                        var Imagecover =
                            ImageSource.FromFile(DependencyService.Get<IPicture>().GetPictureFromDisk(cover, user_id));
                        if (DependencyService.Get<IPicture>().GetPictureFromDisk(cover, user_id) == "File Dont Exists")
                        {
                            Imagecover = new UriImageSource
                            {
                                Uri = new Uri(cover)
                            };
                            DependencyService.Get<IPicture>().SavePictureToDisk(cover, user_id);
                        }

                        var Imageavatar =
                            ImageSource.FromFile(DependencyService.Get<IPicture>().GetPictureFromDisk(avatar, user_id));
                        if (DependencyService.Get<IPicture>().GetPictureFromDisk(avatar, user_id) == "File Dont Exists")
                        {
                            Imageavatar = new UriImageSource
                            {
                                Uri = new Uri(avatar)
                            };
                            DependencyService.Get<IPicture>().SavePictureToDisk(avatar, user_id);
                        }

                       
                            var contact = SQL_Commander.GetProfileCredentialsById(user_id);
                            if (contact != null)
                            {
                                if (contact.UserID == user_id &&
                                    ((contact.Cover != cover) || (contact.Avatar != avatar) ||
                                     (contact.Birthday != birthday) || (contact.name != name)
                                     || (contact.Username != username) || (contact.First_name != First_name) ||
                                     (contact.Last_name != Last_name) || (contact.About != About) ||
                                     (contact.Website != Website)
                                     || (contact.School != School)))
                                {
                                    //Datas.DeleteProfileRow(contact);
                                    if ((contact.Avatar != avatar))
                                    {
                                        DependencyService.Get<IPicture>().DeletePictureFromDisk(contact.Avatar, user_id);
                                    }
                                    if ((contact.Cover != cover))
                                    {
                                        DependencyService.Get<IPicture>().DeletePictureFromDisk(contact.Cover, user_id);
                                    }

                                    contact.UserID = user_id;
                                    contact.name = name;
                                    contact.Avatar = avatar;
                                    contact.Cover = cover;
                                    contact.Birthday = birthday;
                                    contact.Address = address;
                                    contact.Gender = gender;
                                    contact.Email = email;
                                    contact.Username = username;
                                    contact.First_name = First_name;
                                    contact.Last_name = Last_name;
                                    contact.About = About;
                                    contact.Website = Website;
                                    contact.School = School;

                                SQL_Commander.UpdateProfileCredentials(contact);

                                }
                            }
                            else
                            {
                            SQL_Commander.InsertProfileCredentials(new LoginUserProfileTableDB()
                                {
                                    UserID = user_id,
                                    name = name,
                                    Avatar = avatar,
                                    Cover = cover,
                                    Birthday = birthday,
                                    Address = address,
                                    Gender = gender,
                                    Email = email,
                                    Username = username,
                                    First_name = First_name,
                                    Last_name = Last_name,
                                    About = About,
                                    Website = Website,
                                    School = School
                                });

                            }

                        
                    }
                    else if (apiStatus == "400")
                    {
                        json = AppResources.Label_Error;
                    }
                    return json;
                }
            }
            catch (Exception)
            {

                return AppResources.Label_Error;
            }

        }



        public static async Task SyncContactsFromPhone()
        {
            try
            {

                var Dictionary = new Dictionary<string, string>();
                try
                {

                    if (await CrossContacts.Current.RequestPermission())
                    {
                        CrossContacts.Current.PreferContactAggregation = false;
                        await Task.Run(() =>
                        {
                            if (CrossContacts.Current.Contacts == null)
                                return;
                            var max = 0;
                            foreach (Contact contact in CrossContacts.Current.Contacts)
                            {
                                max += 1;
                                if (max == Settings.LimitContactGetFromPhone)
                                {
                                    continue;
                                }
                                foreach (var Number in contact.Phones)
                                {
                                    if (!Dictionary.ContainsKey(contact.FirstName + ' ' + contact.LastName))
                                    {
                                        Dictionary.Add(contact.FirstName + ' ' + contact.LastName, Number.Number);
                                    }
                                    break;
                                }
                            }
                        });
                    }

                }
                catch
                {
                }
                string Data = JsonConvert.SerializeObject(Dictionary.ToArray());

                UserDialogs.Instance.Toast("Loading from your phone contacts");

                using (var client = new HttpClient())
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("user_id", Settings.User_id),
                    new KeyValuePair<string, string>("contacts", Data),
                    new KeyValuePair<string, string>("s", Settings.Session)
                });



                    var response =
                        await
                            client.PostAsync(Settings.Website + "/app_api.php?application=phone&type=get_suggestions",
                                formContent);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string apiStatus = data["api_status"].ToString();
                    string ThemeUrl = data["theme_url"].ToString();

                    if (apiStatus == "200")
                    {
                        SearchFilterlist.Clear();
                        var users_sugParse = JObject.Parse(json).SelectToken("users_sug").ToString();
                        JArray Chatusers_sug = JArray.Parse(users_sugParse);

                        if (Chatusers_sug.Count > 0)
                        {
                            foreach (var userssug in Chatusers_sug)
                            {
                                JObject ChatlistUserdata = JObject.FromObject(userssug);
                                var ChatUser_User_ID = ChatlistUserdata["user_id"].ToString();
                                var ChatUser_avatar = ChatlistUserdata["profile_picture"].ToString();
                                var ChatUser_name = ChatlistUserdata["name"].ToString();
                                var ChatUser_lastseen = ChatlistUserdata["lastseen"].ToString();
                                var ChatUser_lastseen_Time_Text = ChatlistUserdata["lastseen_time_text"].ToString();
                                var ChatUser_verified = ChatlistUserdata["verified"].ToString();

                                #region Images

                                var ImageMediaFile =
                                    ImageSource.FromFile(DependencyService.Get<IPicture>()
                                        .GetPictureFromDisk(ChatUser_avatar, ChatUser_User_ID));
                                if (
                                    DependencyService.Get<IPicture>()
                                        .GetPictureFromDisk(ChatUser_avatar, ChatUser_User_ID) == "File Dont Exists")
                                {
                                    ImageMediaFile = new UriImageSource
                                    {
                                        Uri = new Uri(ChatUser_avatar)
                                    };
                                    //DependencyService.Get<IPicture>().SavePictureToDisk(ChatUser_avatar, ChatUser_User_ID);
                                }

                                var OnlineOfflineIcon = ImageSource.FromFile("");


                                #endregion

                                var status = AppResources.Label_AddFriend;
                                if (Settings.ConnectivitySystem == "1")
                                {
                                    status = AppResources.Label_Follow;
                                }

                                if (ChatUser_lastseen == "on")
                                {

                                    SearchFilterlist.Add(new SearchResult()
                                    {
                                        BigLabel = ChatUser_name,
                                        lastseen = OnlineOfflineIcon,
                                        Name = ChatUser_name,
                                        SeenMessageOrNo = ChatUser_lastseen,
                                        profile_picture = ImageMediaFile,
                                        MiniLabel = AppResources.Label_Online,
                                        //verified = ChatUser_verified_bitmap,
                                        ResultID = ChatUser_User_ID,
                                        connectivitySystem = status,
                                        ResultType = "Users",
                                        ButtonColor = Settings.ButtonColorNormal,
                                        ButtonTextColor = Settings.ButtonTextColorNormal,
                                        ButtonImage = Settings.Add_Icon,
                                        ResultButtonAvailble = "true"

                                    });

                                }
                                else
                                {
                                    SearchFilterlist.Add(new SearchResult()
                                    {
                                        BigLabel = ChatUser_name,
                                        lastseen = OnlineOfflineIcon,
                                        Name = ChatUser_name,
                                        SeenMessageOrNo = ChatUser_lastseen,
                                        profile_picture = ImageMediaFile,
                                        MiniLabel = AppResources.Label_LastSeen + " " + ChatUser_lastseen_Time_Text,
                                        //verified = ChatUser_verified_bitmap,
                                        ResultID = ChatUser_User_ID,
                                        connectivitySystem = status,
                                        ResultType = "Users",
                                        ButtonColor = Settings.ButtonColorNormal,
                                        ButtonTextColor = Settings.ButtonTextColorNormal,
                                        ButtonImage = Settings.Add_Icon,
                                        ResultButtonAvailble = "true"
                                    });

                                }
                            }
                        }

                    }
                    else if (apiStatus == "400")
                    {
                        json = AppResources.Label_Error;
                    }

                    if (SearchFilterlist.Count == 0)
                    {
                        SearchFilterlist.Add(new SearchResult()
                        {
                            BigLabel = "  " + AppResources.FN_NoContactPhoneAreAvailble,
                            Name = "  " + AppResources.FN_NoContactPhoneAreAvailble,
                            ButtonColor = Settings.ButtonColorNormal,
                            ButtonTextColor = Settings.ButtonTextColorNormal,
                            ButtonImage = Settings.Add_Icon,
                            ResultButtonAvailble = "false"
                        });
                    }
                }
            }
            catch
            {
            }

        }

        public static async Task<string> GetRandomUsers()
        {

            try
            {
                using (var client = new HttpClient())
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("user_id", Settings.User_id),
                    new KeyValuePair<string, string>("s", Settings.Session)
                });
                    // UserDialogs.Instance.Toast("Loading Random Users");
                    var response = await client.PostAsync(Settings.Website + "/app_api.php?application=phone&type=get_suggestions", formContent);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string apiStatus = data["api_status"].ToString();
                    string ThemeUrl = data["theme_url"].ToString();

                    if (apiStatus == "200")
                    {
                        SearchFilterlist.Clear();
                        var users = JObject.Parse(json).SelectToken("users_random").ToString();
                        JArray Chatusers = JArray.Parse(users);

                        foreach (var ChatUser in Chatusers)
                        {
                            JObject ChatlistUserdata = JObject.FromObject(ChatUser);
                            var ChatUser_User_ID = ChatlistUserdata["user_id"].ToString();
                            var ChatUser_avatar = ChatlistUserdata["profile_picture"].ToString();
                            var ChatUser_name = ChatlistUserdata["name"].ToString();
                            var ChatUser_lastseen = ChatlistUserdata["lastseen"].ToString();
                            var ChatUser_lastseen_Time_Text = ChatlistUserdata["lastseen_time_text"].ToString();
                            var ChatUser_verified = ChatlistUserdata["verified"].ToString();


                            var ImageMediaFile =
                                ImageSource.FromFile(
                                    DependencyService.Get<IPicture>()
                                        .GetPictureFromDisk(ChatUser_avatar, ChatUser_User_ID));
                            if (
                                DependencyService.Get<IPicture>().GetPictureFromDisk(ChatUser_avatar, ChatUser_User_ID) ==
                                "File Dont Exists")
                            {
                                ImageMediaFile = new UriImageSource
                                {
                                    Uri = new Uri(ChatUser_avatar)
                                };
                                //DependencyService.Get<IPicture>().SavePictureToDisk(ChatUser_avatar, ChatUser_User_ID);
                            }

                            var OnlineOfflineIcon = ImageSource.FromFile("");



                            #region Data Adding

                            var status = AppResources.Label_AddFriend;
                            if (Settings.ConnectivitySystem == "1")
                            {
                                status = AppResources.Label_Follow;
                            }

                            if (ChatUser_lastseen == "on")
                            {
                                SearchFilterlist.Add(new SearchResult()
                                {
                                    BigLabel = ChatUser_name,
                                    ResultType = "Users",
                                    lastseen = OnlineOfflineIcon,
                                    Name = ChatUser_name,
                                    SeenMessageOrNo = ChatUser_lastseen,
                                    profile_picture = ImageMediaFile,
                                    MiniLabel = AppResources.Label_Online,
                                    //verified = ChatUser_verified_bitmap,
                                    ResultID = ChatUser_User_ID,
                                    connectivitySystem = status,
                                    ButtonColor = Settings.ButtonColorNormal,
                                    ButtonTextColor = Settings.ButtonTextColorNormal,
                                    ButtonImage = Settings.Add_Icon,
                                    ResultButtonAvailble = "true"
                                });

                            }
                            else
                            {
                                SearchFilterlist.Add(new SearchResult()
                                {
                                    BigLabel = ChatUser_name,
                                    lastseen = OnlineOfflineIcon,
                                    Name = ChatUser_name,
                                    ResultType = "Users",
                                    SeenMessageOrNo = ChatUser_lastseen,
                                    profile_picture = ImageMediaFile,
                                    MiniLabel = AppResources.Label_LastSeen + " " + ChatUser_lastseen_Time_Text,
                                    //verified = ChatUser_verified_bitmap,
                                    ResultID = ChatUser_User_ID,
                                    connectivitySystem = status,
                                    ButtonColor = Settings.ButtonColorNormal,
                                    ButtonTextColor = Settings.ButtonTextColorNormal,
                                    ButtonImage = Settings.Add_Icon,
                                    ResultButtonAvailble = "true"
                                });

                            }


                            #endregion
                        }

                    }
                    else if (apiStatus == "400")
                    {
                        json = AppResources.Label_Error;
                    }



                    if (SearchFilterlist.Count == 0)
                    {
                        SearchFilterlist.Add(new SearchResult()
                        {
                            BigLabel = "  " + "  " + "  " + AppResources.FN_NoUsersAreAvailble,
                            Name = "  " + "  " + "  " + AppResources.FN_NoUsersAreAvailble,
                            ButtonColor = Settings.ButtonColorNormal,
                            ButtonTextColor = Settings.ButtonTextColorNormal,
                            ButtonImage = Settings.Add_Icon,
                            ResultType = "Users",
                            ResultButtonAvailble = "false"
                        });
                    }

                    return json;

                }
            }
            catch
            {
                return AppResources.Label_Error;
            }
        }

        public static void UpdatelastIdMessage(JArray ChatMessages, string filepath)
        {
            try
            {
             

                foreach (var MessageInfo in ChatMessages)
                {
                    JObject ChatlistUserdata = JObject.FromObject(MessageInfo);
                    
                    var Blal = ChatlistUserdata["messageUser"];
                    var avatar = Blal["avatar"].ToString();
                    var Position = MessageInfo["position"].ToString();
                    var TextMsg2 = MessageInfo["text"].ToString();
                    var Type = MessageInfo["type"].ToString();
                    var TimeMsg = MessageInfo["time_text"].ToString();
                    var send_time = MessageInfo["send_time"].ToString();
                    var type_two = MessageInfo["type_two"].ToString();
                    
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    var from_id = MessageInfo["from_id"].ToString();
                    var to_id = MessageInfo["to_id"].ToString();
                    var media = MessageInfo["media"].ToString();
                    var mediaFileName = MessageInfo["mediaFileName"].ToString();
                    var deleted_one = MessageInfo["mediaFileName"].ToString();
                    var deleted_two = MessageInfo["mediaFileName"].ToString();
                    var msgID = MessageInfo["id"].ToString();
                    int Messageidx = Int32.Parse(msgID);

                    var Updater = Messages.Where(d => d.messageID == send_time).FirstOrDefault();

                    if (TextMsg2 == "")
                    {
                        //https://wowonder.s3.amazonaws.com/upload/photos/2017/06/dTiriuKhzsgkeWX3TMnM_Sticker10.jpg
                    }
                    else
                    {
                        TextMsg2 = DecodeString(TextMsg2);
                    }

                    if (Type == "right_image")
                    {
                        if (Updater != null)
                        {
                            Updater.messageID = msgID;
                            Updater.CreatedAt = TimeMsg;
                            Updater.Type = Type;
                            Updater.Position = "right";
                            Updater.ImageMedia = ImageSource.FromFile(filepath);
                            Updater.ImageUrl = filepath;
                            Updater.DownloadFileUrl = filepath;
                        }

                       
                        if (SQL_Commander.CheckMessage(msgID) == "0")
                        {
                            SQL_Commander.InsertMessage(new MessagesDB
                            {
                                message_id = Messageidx,
                                from_id = from_id,
                                to_id = to_id,
                                media = filepath,
                                mediafilename = filepath,
                                deleted_one = deleted_one,
                                deleted_two = deleted_two,
                                text = DecodeString(TextMsg2),
                                time = TimeMsg,
                                type = Type,
                                position = Position,
                                avatar = avatar,
                                DownloadFileUrl = filepath,
                            });
                        }
                    }
                    else if (Type == "right_text")
                    {
                        
                            if (Updater != null)
                            {
                                Updater.messageID = msgID;
                                Updater.CreatedAt = TimeMsg;
                                Updater.Type = Type;
                                Updater.Position = "right";
                                Updater.ImageMedia = avatar;

                            }

                            var Cheke = ChatActivityTab.ChatList.FirstOrDefault(a => a.UserID == to_id);
                            if (ChatActivityTab.ChatList.Contains(Cheke))
                            {
                                var q = ChatActivityTab.ChatList.IndexOf(ChatActivityTab.ChatList.Where(X => X.UserID == to_id).FirstOrDefault());
                                if (q > -1)
                                {
                                    var Text = TextMsg2;
                                    if (TextMsg2 == "")
                                    {
                                        Text = "Media File";
                                    }
                                    Cheke.TextMessage = Text;
                                    Cheke.ChekeSeen = "false";
                                    ChatActivityTab.ChatList.Move(q, 0);
                                }

                            }

                           
                            if (SQL_Commander.CheckMessage(msgID) == "0")
                            {
                            SQL_Commander.InsertMessage(new MessagesDB
                                {
                                    message_id = Messageidx,
                                    from_id = from_id,
                                    to_id = to_id,
                                    media = filepath,
                                    mediafilename = mediaFileName,
                                    deleted_one = deleted_one,
                                    deleted_two = deleted_two,
                                    text = DecodeString(TextMsg2),
                                    time = TimeMsg,
                                    type = Type,
                                    position = Position,
                                    avatar = avatar,
                                    DownloadFileUrl = "",
                                });
                            }
                        
                    }
                    else if (Type == "right_sticker")
                    {

                        if (Updater != null)
                        {
                            Updater.messageID = msgID;
                            Updater.CreatedAt = TimeMsg;
                            Updater.Type = "right_sticker";
                            Updater.Position = "right";
                            //Updater.ImageMedia = avatar;

                        }

                        var Cheke = ChatActivityTab.ChatList.FirstOrDefault(a => a.UserID == to_id);
                        if (ChatActivityTab.ChatList.Contains(Cheke))
                        {
                            var q = ChatActivityTab.ChatList.IndexOf(ChatActivityTab.ChatList.Where(X => X.UserID == to_id).FirstOrDefault());
                            if (q > -1)
                            {
                                var Text = TextMsg2;
                                if (TextMsg2 == "")
                                {
                                    Text = Settings.FN_StickerFileEmoji;
                                }
                                Cheke.TextMessage = Text;
                                Cheke.ChekeSeen = "false";
                                ChatActivityTab.ChatList.Move(q, 0);
                            }

                        }

                        if (SQL_Commander.CheckMessage(msgID) == "0")
                        {
                            SQL_Commander.InsertMessage(new MessagesDB
                            {
                                message_id = Messageidx,
                                from_id = from_id,
                                to_id = to_id,
                                media = media,
                                mediafilename = mediaFileName,
                                deleted_one = deleted_one,
                                deleted_two = deleted_two,
                                text = DecodeString(TextMsg2),
                                time = TimeMsg,
                                type = "right_sticker",
                                position = Position,
                                avatar = avatar,
                                DownloadFileUrl = media,
                            });
                        }

                    }
                    else if (Type == "right_contact")
                    {
                        if (Updater != null)
                        {
                            Updater.messageID = msgID;
                            Updater.CreatedAt = TimeMsg;
                            Updater.Type = "right_contact";
                            Updater.Position = "right";
                            Updater.ImageMedia = avatar;

                            var Cheke = ChatActivityTab.ChatList.FirstOrDefault(a => a.UserID == to_id);
                            if (ChatActivityTab.ChatList.Contains(Cheke))
                            {
                                var q = ChatActivityTab.ChatList.IndexOf(ChatActivityTab.ChatList.Where(X => X.UserID == to_id).FirstOrDefault());
                                if (q > -1)
                                {
                                    var Text = TextMsg2;
                                    if (TextMsg2 == "")
                                    {
                                        Text = Settings.FN_ContatactFileEmoji;
                                    }
                                    Cheke.TextMessage = Text;
                                    Cheke.ChekeSeen = "false";
                                    ChatActivityTab.ChatList.Move(q, 0);
                                }

                            }

                            if (SQL_Commander.CheckMessage(msgID) == "0")
                            {
                                SQL_Commander.InsertMessage(new MessagesDB
                                {
                                    message_id = Messageidx,
                                    from_id = from_id,
                                    to_id = to_id,
                                    media = "UserContact.png",
                                    mediafilename = mediaFileName,
                                    deleted_one = deleted_one,
                                    deleted_two = deleted_two,
                                    text = Updater.Content.ToString(),
                                    time = TimeMsg,
                                    type = "right_contact",
                                    position = Position,
                                    avatar = avatar,
                                    DownloadFileUrl = "",
                                    contactName = Updater.Content.ToString(),
                                    contactNumber = Updater.ContactNumber.ToString()
                                });

                            }
                        }

                    }

                    else if (Type == "right_video" || Type == "left_video")
                    {
                        if (Updater != null && Type == "right_video")
                        {

                            Updater.messageID = msgID;
                            Updater.CreatedAt = TimeMsg;
                            Updater.Type = Type;
                            Updater.Position = "right";
                            Updater.MediaName = AppResources.Label_VideoFile;
                            Updater.DownloadFileUrl = filepath;
                        }
                        else if (Updater != null && Type == "left_audio")
                        {
                            Updater.messageID = msgID;
                            Updater.CreatedAt = TimeMsg;
                            Updater.Position = "left";
                            Updater.DownloadFileUrl = filepath;
                            Updater.MediaName = AppResources.Label_VideoFile;

                        }

                        if (SQL_Commander.CheckMessage(msgID) == "0")
                        {
                            SQL_Commander.InsertMessage(new MessagesDB
                            {
                                message_id = Messageidx,
                                from_id = from_id,
                                to_id = to_id,
                                media = media,
                                mediafilename = mediaFileName,
                                deleted_one = deleted_one,
                                deleted_two = deleted_two,
                                time = TimeMsg,
                                type = Type,
                                position = Position,
                                avatar = avatar,
                                ImageMedia = Settings.MS_Videoicon,
                                DownloadFileUrl = filepath,
                                MediaName = AppResources.Label_VideoFile
                            });
                        }

                       }

                    else if (Type == "right_audio" || Type == "left_audio")
                        {
                            if (Updater != null && Type == "right_audio")
                            {
                                Updater.messageID = msgID;
                                Updater.CreatedAt = TimeMsg;
                                Updater.Position = "right";
                                Updater.DownloadFileUrl = filepath;
                                Updater.MediaName = AppResources.Label_SoundFile;
                                Updater.ImageMedia = Settings.MS_PlayWhiteicon;
                            }
                            else if (Updater != null && Type == "left_audio")
                            {
                                Updater.messageID = msgID;
                                Updater.CreatedAt = TimeMsg;
                                Updater.Position = "left";
                                Updater.DownloadFileUrl = filepath;
                                Updater.MediaName = AppResources.Label_SoundFile;
                                Updater.ImageMedia = Settings.MS_PlayBlackicon;
                            }
                           
                            if (SQL_Commander.CheckMessage(msgID) == "0")
                            {
                            SQL_Commander.InsertMessage(new MessagesDB
                                {
                                    message_id = Messageidx,
                                    from_id = from_id,
                                    to_id = to_id,
                                    media = media,
                                    mediafilename = mediaFileName,
                                    deleted_one = deleted_one,
                                    deleted_two = deleted_two,
                                    time = TimeMsg,
                                    type = Type,
                                    position = Position,
                                    avatar = avatar,
                                    ImageMedia = Settings.MS_PlayWhiteicon,
                                    DownloadFileUrl = filepath,
                                    MediaName = AppResources.Label_SoundFile
                                });
                            }
                        }
                   
                }
                
            }
            catch (Exception)
            {

            }
           
        }

        public static string IsUrlValid(string url)
        {
            try
            {
                string pattern =
                    @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
                Regex reg = new Regex(pattern, RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
                Match m = reg.Match(url);
                while (m.Success)
                {
                    //do things with your matching text 
                    m = m.NextMatch();
                    break;
                }
                if (reg.IsMatch(url))
                {
                    return "http://" + m.Value;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }

        }

        public static void UpdateSoundWhenFinshed(string messageID)
        {
            try
            {
                var Updater = Messages.Where(d => d.messageID == messageID).FirstOrDefault();

                if (Updater.Type == "left_audio")
                {
                    Updater.messageID = messageID;
                    Updater.MediaName = AppResources.Label_SoundFile;
                    Updater.ImageMedia = Settings.MS_PlayBlackicon;
                    Updater.SliderSoundValue = "0";
                }
                else
                {
                    Updater.messageID = messageID;
                    Updater.MediaName = AppResources.Label_SoundFile;
                    Updater.ImageMedia = Settings.MS_PlayWhiteicon;
                    Updater.SliderSoundValue = "0";
                }
            }
            catch (Exception)
            {

            }


        }

        public static void UpdateSoundDoraitionSlider(string messageID, string slidervalue, string durationsound)
        {
            try
            {
                var Updater = Messages.Where(d => d.messageID == messageID).FirstOrDefault();

                if (Updater.Type == "left_audio")
                {
                    if (Updater.SliderMaxDuration != durationsound)
                    {
                        Updater.MediaName = AppResources.Label_Playing;
                        Updater.SliderMaxDuration = durationsound;
                    }
                    Updater.SliderSoundValue = slidervalue;
                }
            }
            catch (Exception)
            {

            }

        }

        public static void UpdateSoundAfterDownload(string messageID, string path)
        {
            try
            {
                var Updater = Messages.Where(d => d.messageID == messageID).FirstOrDefault();

                if (Updater != null)
                {
                    
                    int MessageiNT = Int32.Parse(messageID);
                    var Getmessages = SQL_Commander.GetMessagesbyMessageID(MessageiNT);

                    if (Updater.Type == "left_audio")
                    {
                        Updater.MediaName = AppResources.Label_SoundFile;
                        Updater.ImageMedia = Settings.MS_PlayBlackicon;
                        Updater.DownloadFileUrl = path;
                        if (Getmessages != null)
                        {
                            Getmessages.DownloadFileUrl = path;
                            Getmessages.MediaName = AppResources.Label_SoundFile;
                            Getmessages.ImageMedia = Settings.MS_PlayBlackicon;
                            SQL_Commander.UpdateMessage(Getmessages);
                        }
                    }
                    else if (Updater.Type == "right_audio")
                    {
                        Updater.MediaName = AppResources.Label_SoundFile;
                        Updater.ImageMedia = Settings.MS_PlayWhiteicon;
                        Updater.DownloadFileUrl = path;

                        if (Getmessages != null)
                        {
                            Getmessages.DownloadFileUrl = path;
                            Getmessages.MediaName = AppResources.Label_SoundFile;
                            Getmessages.ImageMedia = Settings.MS_PlayWhiteicon;
                            SQL_Commander.UpdateMessage(Getmessages);
                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }
         }

        public static void UpdateVideoAfterDownload(string messageID, string path)
        {
            try
            {
                var Updater = Messages.Where(d => d.messageID == messageID).FirstOrDefault();

                if (Updater != null)
                {
                   
                    int MessageiNT = Int32.Parse(messageID);
                    var Getmessages = SQL_Commander.GetMessagesbyMessageID(MessageiNT);

                    if (Updater.Type == "left_video")
                    {
                        Updater.MediaName = AppResources.Label_VideoFile;
                        Updater.ImageMedia = Settings.MS_VideoBlackicon;
                        Updater.DownloadFileUrl = path;
                        if (Getmessages != null)
                        {
                            Getmessages.DownloadFileUrl = path;
                            Getmessages.MediaName = AppResources.Label_VideoFile;
                            Getmessages.ImageMedia = Settings.MS_VideoBlackicon;
                            SQL_Commander.UpdateMessage(Getmessages);
                        }
                    }
                    else if (Updater.Type == "right_video")
                    {
                        Updater.MediaName = AppResources.Label_VideoFile;
                        Updater.ImageMedia = Settings.MS_Videoicon;
                        Updater.DownloadFileUrl = path;

                        if (Getmessages != null)
                        {
                            Getmessages.DownloadFileUrl = path;
                            Getmessages.MediaName = AppResources.Label_VideoFile;
                            Getmessages.ImageMedia = Settings.MS_Videoicon;
                            SQL_Commander.UpdateMessage(Getmessages);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public static void UpdateDownloadPercentage(string messageID ,int Value)
        {
            var Updater = Messages.Where(d => d.messageID == messageID).FirstOrDefault();

            if (Updater != null)
            {
                if (Updater.Type == "left_video" || Updater.Type == "right_video")
                {
                    var PREC = "";
                    if (Value == 100)
                    {
                        PREC = "";
                    }
                    else
                    {
                        PREC = "(" + Value + "%)";
                    }
                    Updater.MediaName = AppResources.Label_Downloading;
                    Updater.Percentage = PREC;
                }
                if (Updater.Type == "left_audio" || Updater.Type == "right_audio")
                {
                    var PREC = "";
                    if (Value == 100)
                    {
                        PREC = "";
                    }
                    else
                    {
                        PREC = "(" + Value + "%)";
                    }
                    Updater.MediaName = AppResources.Label_Downloading;
                    Updater.Percentage = PREC;
                }
            }
        }

        public static int ListInfoResizer(string About)
        {

            if (About.Length <= 40)
            {
                return 550;
            }
            else if (About.Length <= 90)
            {
                return 5165;
            }
            else if (About.Length <= 130)
            {
                return 575;
            }
            else if (About.Length <= 175)
            {
                return 585;
            }
            else if (About.Length <= 205)
            {
                return 595;
            }
            else if (About.Length <= 250)
            {
                return 205;
            }
            else if (About.Length <= 290)
            {
                return 215;
            }
            else if (About.Length <= 335)
            {
                return 225;
            }
            else if (About.Length <= 350)
            {
                return 235;
            }
            else if (About.Length <= 375)
            {
                return 243;
            }
            else if (About.Length <= 390)
            {
                return 247;
            }
            else if (About.Length <= 405)
            {
                return 259;
            }
            else if (About.Length <= 445)
            {
                return 275;
            }
            else if (About.Length <= 485)
            {
                return 292;
            }
            else if (About.Length <= 525)
            {
                return 308;
            }
            else if (About.Length <= 555)
            {
                return 315;
            }
            else if (About.Length <= 570)
            {
                return 320;
            }
            else if (About.Length <= 590)
            {
                return 330;
            }
            else if (About.Length <= 610)
            {
                return 345;
            }
            else if (About.Length <= 650)
            {
                return 360;
            }
            else if (About.Length <= 685)
            {
                return 380;
            }
            else if (About.Length <= 695)
            {
                return 395;
            }
            else if (About.Length <= 711)
            {
                return 410;
            }
            else if (About.Length <= 735)
            {
                return 420;
            }
            else
            {
                return 0;
            }
        }

        public static string DecodeString(string Content)
        {
            var Decoded = System.Net.WebUtility.HtmlDecode(Content);
            var Stringformater = Decoded.Replace(":)", "\ud83d\ude0a").Replace(";)", "\ud83d\ude09")
                .Replace("0)", "\ud83d\ude07").Replace("(<", "\ud83d\ude02").Replace(":D", "\ud83d\ude01").Replace("*_*", "\ud83d\ude0d")
                .Replace("(<", "\ud83d\ude02").Replace("<3", "\ud83d\u2764").Replace("/_)", "\ud83d\ude0f").Replace("-_-", "\ud83d\ude11")
                .Replace(":-/", "\ud83d\ude15").Replace(":*", "\ud83d\ude18").Replace(":_p", "\ud83d\ude1b").Replace(":p", "\ud83d\ude1c")
                .Replace("x(", "\ud83d\ude20").Replace("X(", "\ud83d\ude21").Replace(":_(", "\ud83d\ude22").Replace("<5", "\ud83d\u2B50")
                .Replace(":0", "\ud83d\ude31").Replace("B)", "\ud83d\ude0e").Replace("o(", "\ud83d\ude27").Replace("</3", "\uD83D\uDC94")
                .Replace(":o", "\ud83d\ude26").Replace("o(", "\ud83d\ude27").Replace(":__(", "\ud83d\ude2d").Replace("!_", "\uD83D\u2757")
                .Replace("<br> <br>", " ").Replace("<br>", " ").Replace("<a href=", "").Replace("target=", "").Replace("_blank", "")
                .Replace(@"""", "").Replace("</a>", "").Replace("class=hash", "").Replace("rel=nofollow>", "").Replace("<p>", "").Replace("</p>", "")
                .Replace("</body>", "").Replace("<body>", "").Replace("<div>", "").Replace("<div ", "").Replace("</div>", "").Replace("<iframe", "")
                .Replace("</iframe>", "").Replace("<table", "").Replace("</table>", " ");

            return Stringformater;
        }

        public static string EncodeString(string Content)
        {
            var Stringformater = Content.Replace("\ud83d\ude0a", " :)")
                .Replace("\ud83d\ude09", " ;)")
                .Replace("\ud83d\ude07", " 0)")
                .Replace("\ud83d\ude01", " :D ")
                .Replace("\ud83d\ude0d", " *_* ")
                .Replace("\ud83d\ude02", " (< ")
                .Replace("\ud83d\ude02", " (< ")
                .Replace("\ud83d\ude0f", "/_)")
                .Replace("\ud83d\ude11", "-_- ")
                .Replace("\ud83d\ude15", ":-/ ")
                .Replace("\ud83d\ude18", ":* ")
                .Replace("\ud83d\ude1b", ":_p ")
                .Replace("\ud83d\ude1c", ":p ")
                .Replace("\ud83d\u2764", "<3")
                .Replace("\ud83d\ude20", "x( ")
                .Replace("\ud83d\ude21", "X( ")
                .Replace("\ud83d\ude22", ":_( ")
                .Replace("\uD83D\u2757", "!_")
                .Replace("\ud83d\ude31", ":0 ")
                .Replace("\ud83d\ude0e", "B) ")
                .Replace("\ud83d\ude27", "o( ")
                .Replace("\uD83D\uDC94", "</3")
                .Replace("\ud83d\ude26", ":o ")
                .Replace("\ud83d\ude27", "o( ")
                .Replace("\ud83d\ude2d", ":__( ")
                .Replace("\ud83d\u2B50", "<5");


            return Stringformater;
        }
    }
}
