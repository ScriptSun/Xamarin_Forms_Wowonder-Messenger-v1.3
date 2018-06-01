
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Contacts;
using Plugin.Contacts.Abstractions;
using PropertyChanged;
using WowonderPhone.Classes;
using WowonderPhone.Controls;
using WowonderPhone.Dependencies;
using WowonderPhone.SQLite;
using WowonderPhone.SQLite.Tables;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using ImageSource = Xamarin.Forms.ImageSource;
using WowonderPhone.Languish;
using Xamarin.Forms.Xaml;

namespace WowonderPhone.Pages.Tabs
{


    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactsTab : ContentPage
    {



        public static ObservableCollection<ChatContacts> ChatContactsList = new ObservableCollection<ChatContacts>();

        public static string Aftercontact = "0";
        public  ContactsTab()
        {
            InitializeComponent();
          
            try
            {

                var device = Resolver.Resolve<IDevice>();
                var oNetwork = device.Network;
                var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                if (xx == true)
                {
                   
                        var GetContactList = SQL_Commander.GetContactCacheList();
                        if (GetContactList.Count >= 0)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                ContactListview.IsVisible = true;
                                EmptyContactPage.IsVisible = false;
                            });
                           

                        }
                        else
                        {
                           
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                ContactListview.IsVisible = false;
                                EmptyContactPage.IsVisible = true;
                            });
                            UserDialogs.Instance.Toast("Offline Mode");
                        }
                        ContactListview.ItemsSource = GetContactList;
                   
                }
                else
                {
                    ContactsLoader();
                }
            }
            catch (Exception)
            {

            }
            // ContactListview.ItemsSource = Functions.ChatContactsList;
        }

        private async void ContactListview_OnRefreshing(object sender, EventArgs e)
        {
            try
            {
                var device = Resolver.Resolve<IDevice>();
                var oNetwork = device.Network;
                var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                if (xx == false)
                {
                    //Functions.grouped.Clear();
                    //ContactListview.ItemsSource = null;
                    ContactListview.ItemsSource = null;
                    var myData = await GetChatContacts();
                    
                    ContactListview.ItemsSource = ChatContactsList;

                }
                ContactListview.EndRefresh();
            }
            catch (Exception)
            {
                ContactListview.EndRefresh();
            }
        }

      
        public async void ContactsLoader()
        {
            try
            {
                ContactListview.BeginRefresh();
                //ContactListview.ItemsSource = null;
                //await GetChatContacts(Settings.User_id, Settings.Session);
                //if (ChatContactsList.Count > 0)
                //{
                //    //ErrorPanel.IsVisible = false; 
                  
                //    ContactListview.ItemsSource = ChatContactsList;
                   
                //}
                //ContactListview.EndRefresh();
            }
            catch (Exception)
            {
                ContactListview.EndRefresh();
            }
           
        }


        private async void OnDelete(object sender, EventArgs e)
        {
            try
            {
                var mi = ((MenuItem) sender);
                var answer =
                    await DisplayAlert("Delete " + mi.CommandParameter + " from my contact list ?", null, "Yes", "No");
                if (answer)
                {
                    #region Delete from list

                 
             var getuser = ChatContactsList.Where(a => a.Name == mi.CommandParameter.ToString()).ToList().FirstOrDefault();
                if (getuser != null)
                {
                  if (getuser.Username == mi.CommandParameter.ToString())
                    {
                      ChatContactsList.Remove(getuser);
                    }
                }
                    #endregion

                    #region Delete from SqlTable (Cashe)

                  
                        var contact = SQL_Commander.GetContactUserByUsername(mi.CommandParameter.ToString());
                        if (contact != null)
                        {
                            SQL_Commander.DeleteContactRow(contact);
                        }

                        #region Delete from website

                        try
                        {
                            using (var client = new HttpClient())
                            {
                                var formContent = new FormUrlEncodedContent(new[]
                                {
                                    new KeyValuePair<string, string>("user_id", Settings.User_id),
                                    new KeyValuePair<string, string>("recipient_id", contact.UserID),
                                    new KeyValuePair<string, string>("s", Settings.Session)
                                });

                                var response =
                                    await
                                        client.PostAsync(
                                            Settings.Website + "/app_api.php?application=phone&type=follow_user",
                                            formContent).ConfigureAwait(false);
                                response.EnsureSuccessStatusCode();
                                string json = await response.Content.ReadAsStringAsync();
                                var data2 = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                                string apiStatus = data2["api_status"].ToString();
                                if (apiStatus == "200")
                                {

                                }
                            }
                        }
                        catch (Exception)
                        {

                        }
                   

                    #endregion
                }

                #endregion
              }
            catch (Exception)
            {
                
            }
         }
           

        private  void ContactListview_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                ContactListview.SelectedItem = null;
                var user = e.Item as ChatContacts;
                var recipient_id = user.UserID;
                var UserName = user.Username;
                Functions.Messages.Clear();
                Navigation.PushAsync(new ChatWindow(UserName, recipient_id.ToString(), ""));
            }
            catch (Exception)
            {
                
            }
        }


        private void ContactListview_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ContactListview.SelectedItem = null;
        }

        private void SearchButton_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Search_Page());
        }

        public static string NumberOfcontacts;




        public  async Task<string> GetChatContacts()
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
                            

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                ContactListview.IsVisible = false;
                                EmptyContactPage.IsVisible = true;
                            });

                            
                            UserDialogs.Instance.HideLoading();
                            return null;
                        }
                        ChatContactsList.Clear();

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
                                //ImageMediaFile = "loading.jpg";
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
                                ChatContactsList.Add(new ChatContacts()
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
                            if ( DependencyService.Get<IPicture>().GetPictureFromDisk(ChatUser_avatar, ChatUser_User_ID) == "File Dont Exists")
                            {
                                //ImageMediaFile = "loading.jpg";
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


                            ChatContactsList.Add(new ChatContacts()
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

                        UserDialogs.Instance.HideLoading();
                        if (ContactListview.IsVisible == false)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                ContactListview.IsVisible = true;
                                EmptyContactPage.IsVisible = false;
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
                ContactListview.EndRefresh();
                return AppResources.Label_Error;
            }

        }

      

        private async void RefreshButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var device = Resolver.Resolve<IDevice>();
                var oNetwork = device.Network;
                var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                if (xx == false)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ContactListview.IsVisible = true;
                        EmptyContactPage.IsVisible = false;
                    });
                   
                    await GetChatContacts();
                    ContactListview.ItemsSource = ChatContactsList;

                }
                ContactListview.EndRefresh();
            }
            catch (Exception)
            {
                ContactListview.EndRefresh();
            }
           

        }
    }
   
}
