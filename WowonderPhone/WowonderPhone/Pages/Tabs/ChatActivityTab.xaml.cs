using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WowonderPhone.Controls;
using WowonderPhone.SQLite;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WowonderPhone.Classes;
using WowonderPhone.Dependencies;
using WowonderPhone.SQLite.Tables;
using XLabs.Platform.Services.Geolocation;
using WowonderPhone.Languish;
using Xamarin.Forms.Xaml;

namespace WowonderPhone.Pages.Tabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatActivityTab : ContentPage
    {

        public string TaskWork = "Working";
        public static string NotifiStoper = "0";
        public static ObservableCollection<ChatUsers> ChatList = new ObservableCollection<ChatUsers>();

        public ChatActivityTab()
        {
            InitializeComponent();
            try
            {
                var device = Resolver.Resolve<IDevice>();
                var oNetwork = device.Network;
                var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
               
                var ResultData = SQL_Commander.GetChatUsersCacheList();

                if (xx == true)
                {

                    if (ResultData.Count > 0)
                    {
                        ChatActivityListview.ItemsSource = ResultData;
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (ChatActivityListview.IsVisible == true)
                            {
                                ChatActivityListview.IsVisible = false;
                                EmptyChatPage.IsVisible = true;
                            }
                        });
                    }   
                

                }
                else
                {

                    if (ResultData.Count > 0)
                    {
                        ChatActivityListview.ItemsSource = ResultData;
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (ChatActivityListview.IsVisible == true)
                            {
                                ChatActivityListview.IsVisible = false;
                                EmptyChatPage.IsVisible = true;
                            }
                        });
                        
                    }
                }

                UpdateChatActivityFunctionTask().ConfigureAwait(false);
            }
            catch (Exception)
            {

            }

          
        }

        private async void ChatActivityListview_OnRefreshing(object sender, EventArgs e)
        {
            try
            {
                var device = Resolver.Resolve<IDevice>();
                var oNetwork = device.Network;
                var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                if (xx == false)
                {
                    ChatList.Clear();
                    ChatActivityListview.ItemsSource = null;
                    var _myData = await GetChatActivity(Settings.User_id, Settings.Session);
                    ChatActivityListview.ItemsSource = ChatList;

                }
                ChatActivityListview.EndRefresh();
            }
            catch (Exception)
            {

            }

        }

        private async void OnDeletemessage(object sender, EventArgs e)
        {
            try
            {
                var mi = ((MenuItem)sender);
                var answer = await DisplayAlert("Delete chat with '" + mi.CommandParameter + "' ?", null, "Yes", "No");
                if (answer)
                {
                    #region Delete from list

                    var UsertoDelete = ChatList.Where(a => a.Username == mi.CommandParameter.ToString())
                        .ToList()
                        .FirstOrDefault();
                  


                    if (UsertoDelete != null)
                    {
                        ChatList.Remove(UsertoDelete);
                      

                    }

                    

                        var Userid = SQL_Commander.GetChatUserByUsername(UsertoDelete.Username);
                        if (Userid != null)
                        {
                            SQL_Commander.DeleteChatUserRow(Userid);
                        }

                        SQL_Commander.DeleteMessage(Settings.User_id, Userid.UserID);
                       
                        DeleteconversationFromServer(Userid.UserID).ConfigureAwait(false);
                   
                    

                    
                    #endregion
                }

            }
            catch (Exception)
            {

            }
        
        }

        public async Task<string> DeleteconversationFromServer(string UserProfileid)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user_id", Settings.User_id),
                        new KeyValuePair<string, string>("recipient_id", UserProfileid),
                        new KeyValuePair<string, string>("s", Settings.Session)

                    });

                    var response =
                        await
                            client.PostAsync(Settings.Website + "/app_api.php?application=phone&type=delete_messages",
                                formContent).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string apiStatus = data["api_status"].ToString();

                    if (apiStatus == "200")
                    {
                    }
                }
            }
            catch (Exception)
            {

            }
            return "Deleted";
        }

        private void ChatActivityListview_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ChatActivityListview.SelectedItem = null;
        }

        private void ChatActivityListview_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                
                ChatActivityListview.SelectedItem = null;
                var user = e.Item as ChatUsers;
                if (user.Username == "  " + "  " + "  " + AppResources.Label_NoAnyChatActivityAvailable)
                {
                    return;
                }
                try
                {
                    if (user.SeenMessageOrNo != "Transparent")
                    {
                        user.SeenMessageOrNo = "Transparent";
                        DependencyService.Get<ILocalNotificationService>().CloseNotification(user.UserID);
                        user.SeenMessageOrNo = "Transparent";
                    }
                }
                catch (Exception)
                {
                   
                }
               
                var recipient_id = user.UserID;
                var UserName = user.Username;
                Functions.Messages.Clear();


                Navigation.PushAsync(new ChatWindow(UserName, recipient_id, "ChatActivity"));
            }
            catch (Exception)
            {
                
            }
           
        }

        public async Task<string> UpdateChatActivityFunctionTask()
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                for (int i = 0; i < 5; i++)
                {
                    while (TaskWork == "Working")
                    {
                        await Task.Delay(Settings.RefreshChatActivitiesSecounds);
                        var device = Resolver.Resolve<IDevice>();
                        var oNetwork = device.Network;
                        var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                        if (xx == false)
                        {
                            await GetChatActivity(Settings.User_id, Settings.Session);              
                        }
                        if (TaskWork == "Stop")
                        {
                            return "Stop";
                        }
                    }
                    if (TaskWork == "Stop")
                    {
                        return "Stop";
                    }
                }
            }
            catch
            {

            }


            return "true";
        }

        public async Task<string> GetChatActivity(string userid, string session)
        {
            try
            {
                #region Client Respone

                using (var client = new HttpClient())
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user_id", Settings.User_id),
                        new KeyValuePair<string, string>("user_profile_id", Settings.User_id),
                        new KeyValuePair<string, string>("s", Settings.Session),
                        new KeyValuePair<string, string>("list_type", "all")
                    });

                    var response =
                        await
                            client.PostAsync(Settings.Website + "/app_api.php?application=phone&type=get_users_list",
                                formContent).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string apiStatus = data["api_status"].ToString();

                    #endregion

                    if (apiStatus == "200")
                    {
                        // Functions.ChatList.Clear();

                        #region Respone Success

                        var Users = data["users"];
                        string ThemeUrl = data["theme_url"].ToString();
                        var users = JObject.Parse(json).SelectToken("users").ToString();
                        Object obj = JsonConvert.DeserializeObject(users);
                        JArray Chatusers = JArray.Parse(users);
                        if (Chatusers.Count <= 0)
                        {

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                ChatActivityListview.IsVisible = false;
                                EmptyChatPage.IsVisible = true;
                            });

                            return null;
                        }
                        else
                        {
                            ChatActivityListview.ItemsSource = ChatList;
                        }

                        //var ListTodelete = new ChatActivityFunctions();
                        // ListTodelete.ClearChatUserTable();
                        foreach (var ChatUser in Chatusers)
                        {


                            JObject ChatlistUserdata = JObject.FromObject(ChatUser);
                            var ChatUser_User_ID = ChatlistUserdata["user_id"].ToString();
                            var ChatUser_avatar = ChatlistUserdata["profile_picture"].ToString();
                            var ChatUser_name = ChatlistUserdata["name"].ToString();
                            var ChatUser_lastseen = ChatlistUserdata["lastseen"].ToString();
                            // var ChatUser_lastseen_Time_Text = ChatlistUserdata["lastseen_time_text"].ToString();
                            var ChatUser_verified = ChatlistUserdata["verified"].ToString();


                            JObject ChatlistuserLastMessage = JObject.FromObject(ChatlistUserdata["last_message"]);
                            var listuserLastMessage_Text = ChatlistuserLastMessage["text"].ToString();
                            var listuserLastMessage_date_time = ChatlistuserLastMessage["date_time"].ToString();
                            var SeenCo = ChatlistuserLastMessage["seen"].ToString();
                            var fromId = ChatlistuserLastMessage["from_id"].ToString();
                            var Messageid = ChatlistuserLastMessage["id"].ToString();
                            var Time = ChatlistuserLastMessage["time"].ToString();
                            var mediaFileName = ChatlistuserLastMessage["media"].ToString();

                            listuserLastMessage_Text = Functions.DecodeString(listuserLastMessage_Text);
                            var MediafileIcon = "";
                            if (mediaFileName.Contains("soundFile"))
                            {
                                MediafileIcon = Settings.FN_SoundFileEmoji;
                            }
                            else if (mediaFileName.Contains("image"))
                            {
                                MediafileIcon = Settings.FN_ImageFileEmoji;
                            }
                            else if (mediaFileName.Contains("video"))
                            {
                                MediafileIcon = Settings.FN_VideoFileEmoji;
                            }
                            else if(mediaFileName.Contains("sticker"))
                            {
                                MediafileIcon = Settings.FN_StickerFileEmoji;
                            }

                            
                            if (listuserLastMessage_Text == "")
                            {
                                listuserLastMessage_Text = MediafileIcon;
                            }
                            else if(listuserLastMessage_Text.Contains("{Key:"))
                            {
                                listuserLastMessage_Text = Settings.FN_ContatactFileEmoji;
                            }
                            else
                            {
                                listuserLastMessage_Text = Functions.DecodeString(listuserLastMessage_Text);
                            }

                            var color = "Transparent";
                            var Chekicon = "false";

                            if (fromId != Settings.User_id)
                            {
                                if (SeenCo == "0")
                                {
                                    int Messageidx = Int32.Parse(Messageid);
                                    var Style = "Text";
                                    color = Settings.UnseenMesageColor;

                                   
                                        var Exits = SQL_Commander.GetNotifiDBCredentialsById(Messageidx);
                                        if (Exits == null)
                                        {
                                        SQL_Commander.InsertNotifiDBCredentials(new NotifiDB
                                            {
                                                messageid = Messageidx,
                                                Seen = 0
                                            });
                                        }

                                        var Exits2 = SQL_Commander.GetNotifiDBCredentialsById(Messageidx);
                                        if (Exits2.Seen == 0 && NotifiStoper != ChatUser_User_ID)
                                        {
                                            var Iconavatr =
                                                DependencyService.Get<IPicture>()
                                                    .GetPictureFromDisk(ChatUser_avatar, ChatUser_User_ID);
                                            DependencyService.Get<ILocalNotificationService>()
                                                .CreateLocalNotification(listuserLastMessage_Text, ChatUser_name,
                                                    Iconavatr, ChatUser_User_ID, Style);
                                            Exits2.Seen = 1;
                                        SQL_Commander.UpdateNotifiDBCredentials(Exits2);
                                        }
                                    
                                }
                            }
                            else
                            {
                                if (SeenCo != "0")
                                {
                                    Chekicon = "true";
                                }
                            }

                            var Imagepath = DependencyService.Get<IPicture>().GetPictureFromDisk(ChatUser_avatar, ChatUser_User_ID);
                            var ImageMediaFile = ImageSource.FromFile(Imagepath);

                            if (
                                DependencyService.Get<IPicture>().GetPictureFromDisk(ChatUser_avatar, ChatUser_User_ID) ==
                                "File Dont Exists")
                            {
                                ImageMediaFile = "Noprofile.png";
                                DependencyService.Get<IPicture>().SavePictureToDisk(ChatUser_avatar, ChatUser_User_ID);
                            }

                            #region Show_Online_Oflline_Icon

                            ImageSource OnlineOfflineIcon = ImageSource.FromFile("");
                            var OnOficon = "";
                            if (Settings.Show_Online_Oflline_Icon)
                            {
                                if (ChatUser_lastseen == "on")
                                {
                                    OnOficon = DependencyService.Get<IPicture>().GetPictureFromDisk(ThemeUrl + "/img/windows_app/online.png", "Icons");
                                    OnlineOfflineIcon = ImageSource.FromFile(OnOficon);



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
                                    OnOficon = DependencyService.Get<IPicture>().GetPictureFromDisk(ThemeUrl + "/img/windows_app/offline.png", "Icons");
                                    OnlineOfflineIcon = ImageSource.FromFile(OnOficon);

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

                            if (ChatUser_verified == "1")
                            {
                                ChatUser_verified = "True";
                            }
                            else
                            {
                                ChatUser_verified = "False";
                            }
                            var ChekeForNotAvailbleLabel = ChatList.FirstOrDefault(a => a.Username == "  " + "  " + "  " + AppResources.Label_NoAnyChatActivityAvailable);
                            if (ChatList.Contains(ChekeForNotAvailbleLabel))
                            {
                                ChatList.Remove(ChekeForNotAvailbleLabel);
                            }

                           var Cheke = ChatList.FirstOrDefault(a => a.UserID == ChatUser_User_ID);
                            if (ChatList.Contains(Cheke))
                            {
                                //Cheke for online/offline Status
                                if (Cheke.OnOficon != OnOficon)
                                {
                                    Cheke.OnOficon = OnOficon;
                                    Cheke.lastseen = OnlineOfflineIcon;
                                }

                                if (Cheke.TextMessage != listuserLastMessage_Text)
                                {
                                    var q = ChatList.IndexOf(ChatList.Where(X => X.UserID == ChatUser_User_ID).FirstOrDefault());
                                    if (q > -1)
                                    {
                                        Cheke.TextMessage = listuserLastMessage_Text;
                                        Cheke.Username = ChatUser_name;
                                        Cheke.TimeLong = Time;
                                        ChatList.Move(q, 0);
                                    }

                                }
                                //Change  User image if the user changed his avatar picture
                                if (Cheke.profile_picture_Url != Imagepath)
                                {
                                    Cheke.profile_picture_Url = Imagepath;
                                    Cheke.profile_picture = ImageMediaFile;
                                }
                                if (Cheke.SeenMessageOrNo != color || Cheke.LastMessageDateTime != listuserLastMessage_date_time)
                                {
                                    Cheke.SeenMessageOrNo = color;
                                    Cheke.LastMessageDateTime = listuserLastMessage_date_time;
                                }
                                if (Cheke.ChekeSeen != Chekicon)
                                {
                                    Cheke.ChekeSeen = Chekicon;
                                }


                            }
                            else
                            {
                                ChatList.Insert(0, new ChatUsers()
                                {
                                    Username = ChatUser_name,
                                    lastseen = OnlineOfflineIcon,
                                    profile_picture = ImageMediaFile,
                                    TextMessage = listuserLastMessage_Text,
                                    LastMessageDateTime = listuserLastMessage_date_time,
                                    UserID = ChatUser_User_ID,
                                    SeenMessageOrNo = color,
                                    ChekeSeen = Chekicon,
                                    OnOficon = OnOficon,
                                    TimeLong = Time,
                                    profile_picture_Url = Imagepath,
                                    Isverifyed = ChatUser_verified
                                });
                            }

                            
                                var contact = SQL_Commander.GetChatUser(ChatUser_User_ID);
                                if (contact != null)
                                {
                                    if (contact.UserID == ChatUser_User_ID &
                                        ((contact.Name != ChatUser_name) || (contact.ProfilePicture != ChatUser_avatar) ||
                                         (contact.Username != ChatUser_name) || (contact.ChekeSeen != Chekicon)
                                         || (contact.SeenMessageOrNo != color) || (contact.Isverifyed != ChatUser_verified) || (contact.TimeLong != Time)))
                                    {

                                        if ((contact.ProfilePicture != ChatUser_avatar))
                                        {
                                            if (Settings.Delete_Old_Profile_Picture_From_Phone)
                                            {
                                                DependencyService.Get<IPicture>().DeletePictureFromDisk(contact.ProfilePicture, ChatUser_User_ID);
                                            }

                                        }

                                        contact.UserID = ChatUser_User_ID;
                                        contact.LastMessageDateTime = listuserLastMessage_date_time;
                                        contact.Name = ChatUser_name;
                                        contact.ProfilePicture = ChatUser_avatar;
                                        contact.SeenMessageOrNo = color;
                                        contact.TextMessage = listuserLastMessage_Text;
                                        contact.Username = ChatUser_name;
                                        contact.ChekeSeen = Chekicon;
                                        contact.TimeLong = Time;
                                        contact.Isverifyed = ChatUser_verified;
                                    SQL_Commander.UpdateChatUsers(contact);
                                    }

                                }
                                else
                                {
                                    SQL_Commander.InsertChatUsers(new ChatActivityDB()
                                    {
                                        UserID = ChatUser_User_ID,
                                        LastMessageDateTime = listuserLastMessage_date_time,
                                        Name = ChatUser_name,
                                        ProfilePicture = ChatUser_avatar,
                                        SeenMessageOrNo = color,
                                        TextMessage = listuserLastMessage_Text,
                                        Username = ChatUser_name,
                                        ChekeSeen = Chekicon,
                                        TimeLong = Time,
                                        Isverifyed = ChatUser_verified,
                                        lastseen = OnlineOfflineIcon.ToString()
                                    });
                                }
                            
                        }

                       
                        #endregion
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (ChatActivityListview.IsVisible == false)
                            {
                                ChatActivityListview.IsVisible = true;
                                EmptyChatPage.IsVisible = false;
                            }
                        });

                      

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


        private void ChatActivityTab_OnAppearing(object sender, EventArgs e)
        {
            var url = $"https://demo.wowonder.com/foxx3000";

            var entry = new AppLinkEntry
            {
                Title = "foxx3000",
                Description = "Wowonder next genaration to the last earth",
                AppLinkUri = new Uri(url, UriKind.RelativeOrAbsolute),
                IsLinkActive = true,
                Thumbnail = ImageSource.FromFile("Icon.png")
            };

            entry.KeyValues.Add("contentType", "Session");
            entry.KeyValues.Add("appName", "Alidoughouz");
            entry.KeyValues.Add("companyName", "Xamarin");
            Application.Current.AppLinks.RegisterLink(entry);
        }
    }
}

