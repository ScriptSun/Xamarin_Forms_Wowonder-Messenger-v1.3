using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WowonderPhone.Classes;
using WowonderPhone.Controls;
using WowonderPhone.Dependencies;
using WowonderPhone.SQLite;
using Xamarin.Forms;
using XLabs.Forms.Controls;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using System.Collections.ObjectModel;
using Acr.UserDialogs;
using WowonderPhone.Languish;

using WowonderPhone.SQLite.Tables;
using Xamarin.Forms.Xaml;

namespace WowonderPhone.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Search_Page : ContentPage
    {
        public static string SearchFilter = AppResources.Label_Users;
      

        public Search_Page()
        {
            InitializeComponent();
            
            Functions.SearchFilterlist.Clear();
            ChatFriendsListview.ItemsSource = null;
            SearchFilterGet();
            
            var device = Resolver.Resolve<IDevice>();
            var oNetwork = device.Network;
            var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
            if (xx == false)
            {
                try
                {
                   
                }
                catch
                {

                }
            }
            else
            {
                ChatFriendsListview.IsVisible = false;
               
            }
            ChatFriendsListview.ItemsSource = Functions.SearchFilterlist;   
        }

        private void SearchBar_OnSearchButtonPressed(object sender, EventArgs e)
        {
            var device = Resolver.Resolve<IDevice>();
            var oNetwork = device.Network;
            var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
            if (xx == false)
            {
                try
                {
                    
                    Functions.SearchFilterlist.Clear();
                    ChatFriendsListview.ItemsSource = null;
                    var ss = SearchRequest(SearchBarCo.Text).ConfigureAwait(false);
                    ChatFriendsListview.ItemsSource = Functions.SearchFilterlist;
                    EmptySearchPage.IsVisible = false;
                    ChatFriendsListview.IsVisible = true;
                }
                catch
                {
                    
                }
            }
           
        }

        public void SearchFilterGet()
        {
            
                var FilterResult = SQL_Commander.GetSearchFilterById(Settings.User_id);
                if (FilterResult == null)
                {
                    Settings.SearchByGenderValue = "All";
                    Settings.SearchByProfilePictureValue = "All";
                    Settings.SearchByStatusValue = "All";
                }
                else
                {
                    if (FilterResult.Gender == 0)
                    {
                        Settings.SearchByGenderValue = "";
                    }
                    if (FilterResult.Gender == 1)
                    {
                        Settings.SearchByGenderValue = "male";
                    }
                    if (FilterResult.Gender == 2)
                    {
                        Settings.SearchByGenderValue = "female";
                    }
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    if (FilterResult.Status == 0)
                    {
                        Settings.SearchByStatusValue = "";
                    }
                    if (FilterResult.Status == 1)
                    {
                        Settings.SearchByStatusValue = "on";
                    }
                    if (FilterResult.Status == 2)
                    {
                        Settings.SearchByStatusValue = "off";
                    }
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    if (FilterResult.ProfilePicture == 0)
                    {
                        Settings.SearchByProfilePictureValue = "";
                    }
                    if (FilterResult.ProfilePicture == 1)
                    {
                        Settings.SearchByProfilePictureValue = "yes";
                    }
                    if (FilterResult.ProfilePicture == 2)
                    {
                        Settings.SearchByProfilePictureValue = "no";
                    }
                }
           
        }
        private void SearchBarCo_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            EmptySearchPage.IsVisible = false;
            ChatFriendsListview.IsVisible = true;
        }

        
      

        private void ActionButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var mi = ((ImageButton)sender);
                if (SearchFilter == AppResources.Label_Users)
                {
                    #region Add Friend
                    SendFriendRequest(mi.CommandParameter.ToString()).ConfigureAwait(false);
                    var getuser = Functions.SearchFilterlist.Where(a => a.ResultID == mi.CommandParameter.ToString()).ToList().FirstOrDefault();
                    if (getuser != null)
                    {
                        if (getuser.connectivitySystem == AppResources.Label_Follow || getuser.connectivitySystem == AppResources.Label_AddFriend)
                        {
                            if (getuser.ResultID == mi.CommandParameter.ToString())
                            {
                                if (getuser.connectivitySystem == AppResources.Label_AddFriend)
                                {
                                    getuser.connectivitySystem = AppResources.Label_Requested;
                                }
                                else
                                {
                                    getuser.connectivitySystem = AppResources.Label_Following;
                                }
                                getuser.ButtonColor = Settings.ButtonLightColor;
                                getuser.ButtonTextColor = Settings.ButtonTextLightColor;
                                getuser.ButtonImage = Settings.CheckMark_Icon;
                            }
                        }
                        else
                        {
                            if (getuser.ResultID == mi.CommandParameter.ToString())
                            {
                                if (getuser.connectivitySystem == AppResources.Label_Requested)
                                {
                                    getuser.connectivitySystem = AppResources.Label_AddFriend;
                                }
                                else
                                {
                                    getuser.connectivitySystem = AppResources.Label_Follow;
                                }
                                getuser.ButtonColor = Settings.ButtonColorNormal;
                                getuser.ButtonTextColor = Settings.ButtonTextColorNormal;
                                getuser.ButtonImage = Settings.Add_Icon;
                            }
                        }
                    }
                    #endregion
                }
               

            }
            catch
            {

            }
        }


        public static async Task<string> SendFriendRequest(string recipient_id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user_id", Settings.User_id),
                        new KeyValuePair<string, string>("recipient_id", recipient_id),
                        new KeyValuePair<string, string>("s", Settings.Session)
                    });

                    var response =
                        await
                            client.PostAsync(Settings.Website + "/app_api.php?application=phone&type=follow_user",
                                formContent).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string apiStatus = data["api_status"].ToString();
                    if (apiStatus == "200")
                    {
                        return "Succes";

                    }
                }
            }
            catch (Exception)
            {
                
            }
            return null;
        }

        public async Task<string> SearchRequest(string search_key)
        {
            try
            {
               
                UserDialogs.Instance.ShowLoading(AppResources.Label_Loading, MaskType.None);
                if (SearchFilter == AppResources.Label_Users)
                {
                    #region Search Users
                    using (var client = new HttpClient())
                    {
                        var formContent = new FormUrlEncodedContent(new[]
                        {
                        new KeyValuePair<string, string>("user_id", Settings.User_id),
                        new KeyValuePair<string, string>("search_key", search_key),
                        new KeyValuePair<string, string>("s", Settings.Session),
                        new KeyValuePair<string, string>("gender", Settings.SearchByGenderValue),
                        new KeyValuePair<string, string>("image", Settings.SearchByProfilePictureValue),
                        new KeyValuePair<string, string>("status", Settings.SearchByStatusValue)
                    });

                        var response =
                            await
                                client.PostAsync(
                                    Settings.Website + "/app_api.php?application=phone&type=search_public_users",
                                    formContent).ConfigureAwait(false);
                        response.EnsureSuccessStatusCode();
                        string json = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                        string apiStatus = data["api_status"].ToString();
                        if (apiStatus == "200")
                        {

                            var Users = data["users"];
                            string ThemeUrl = data["theme_url"].ToString();
                            var users = JObject.Parse(json).SelectToken("users").ToString();
                            Object obj = JsonConvert.DeserializeObject(users);
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
                                var ChatUser_Isfollowed = ChatlistUserdata["is_following"].ToString();

                                var ImageMediaFile = ImageSource.FromUri(new Uri(ChatUser_avatar)); ;

                                var ImageAvatar = ImageSource.FromUri(new Uri(ChatUser_avatar));
                                if (ChatUser_avatar.Contains("d-avatar.jpg"))
                                {
                                    
                                   // ImageMediaFile = "P_Man.png";
                                }
                                else
                                {
                                    ImageMediaFile = new UriImageSource
                                    {
                                        Uri = new Uri(ChatUser_avatar)
                                    };
                                }
                                


                                #region Show_Online_Oflline_Icon

                                var OnlineOfflineIcon = ImageSource.FromFile("");
                               

                                #endregion

                                var status = AppResources.Label_AddFriend;
                                if (Settings.ConnectivitySystem == "1")
                                {
                                    status = AppResources.Label_Follow;
                                }

                                if (ChatUser_Isfollowed == "no")
                                {

                                    if (ChatUser_lastseen == "on")
                                    {

                                        Functions.SearchFilterlist.Add(new SearchResult()
                                        {
                                            BigLabel = ChatUser_name,
                                            lastseen = OnlineOfflineIcon,
                                            Name = ChatUser_name,
                                            SeenMessageOrNo = ChatUser_lastseen,
                                            profile_picture = ImageMediaFile,
                                            MiniLabel = AppResources.Label_Online,
                                            ResultID = ChatUser_User_ID,
                                            connectivitySystem = status,
                                            ResultType = AppResources.Label_Users,
                                            ButtonColor = Settings.ButtonColorNormal,
                                            ButtonTextColor = Settings.ButtonTextColorNormal,
                                            ButtonImage = Settings.Add_Icon,
                                            ResultButtonAvailble = "true"

                                        });

                                    }
                                    else
                                    {
                                       
                                       
                                    Functions.SearchFilterlist.Add(new SearchResult()
                                        {
                                            BigLabel = ChatUser_name,
                                            lastseen = OnlineOfflineIcon,
                                            Name = ChatUser_name,
                                            ResultType = AppResources.Label_Users,
                                            SeenMessageOrNo = ChatUser_lastseen,
                                            profile_picture = ImageMediaFile,
                                            MiniLabel = AppResources.Label_LastSeen + " " + ChatUser_lastseen_Time_Text,
                                            ResultID = ChatUser_User_ID,
                                            connectivitySystem = status,
                                            ButtonColor = Settings.ButtonColorNormal,
                                            ButtonTextColor = Settings.ButtonTextColorNormal,
                                            ButtonImage = Settings.Add_Icon,
                                            ResultButtonAvailble = "true"
                                        });

                                    }
                                }
                                else
                                {
                                    var Added = AppResources.Label_Requested;
                                    if (Settings.ConnectivitySystem == "1")
                                    {
                                        Added = AppResources.Label_Following;
                                    }

                                    Functions.SearchFilterlist.Add(new SearchResult()
                                    {
                                        BigLabel = ChatUser_name,
                                        lastseen = OnlineOfflineIcon,
                                        Name = ChatUser_name,
                                        ResultType = AppResources.Label_Users,
                                        SeenMessageOrNo = ChatUser_lastseen,
                                        profile_picture = ImageMediaFile,
                                        MiniLabel = AppResources.Label_LastSeen + " " + ChatUser_lastseen_Time_Text,
                                        ResultID = ChatUser_User_ID,
                                        connectivitySystem = Added,
                                        ButtonColor = Settings.ButtonLightColor,
                                        ButtonTextColor = Settings.ButtonTextLightColor,
                                        ButtonImage = Settings.CheckMark_Icon,
                                        ResultButtonAvailble = "true"
                                    });
                                }
                            }
                        }
                    }
                    #endregion
                }
              UserDialogs.Instance.HideLoading();
            }
            catch (Exception)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.ShowError(AppResources.Label_Connection_Lost, 2000);
            }
            
            return null;
        }


        private void ChatFriendsListview_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                ChatFriendsListview.SelectedItem = null;
                var Item = e.Item as SearchResult;
                var resultID = Item.ResultID;
                var UserName = Item.BigLabel;

                if (Item.ResultType == AppResources.Label_Users)
                {
                    Navigation.PushAsync(new UserProfilePage(resultID));
                }
               
            }
            catch (Exception)
            {
                
            }

    }

        private void ChatFriendsListview_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
                
        }

        private void ChatFriendsListview_OnRefreshing(object sender, EventArgs e)
        {
            var device = Resolver.Resolve<IDevice>();
            var oNetwork = device.Network;
            var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
            if (xx == false)
            {

                try
                {
                    this.Title = AppResources.Label_Loading;
                    var ss = Functions.GetRandomUsers().ConfigureAwait(false);
                }
                catch
                {

                }
            }
            ChatFriendsListview.ItemsSource = Functions.SearchFilterlist;
            ChatFriendsListview.EndRefresh();
        }

        

        private void ChatFriendsListview_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ChatFriendsListview.SelectedItem = null;
        }

        private  void Sync_OnClicked_OnClicked(object sender, EventArgs e)
        {
            var device = Resolver.Resolve<IDevice>();
            var oNetwork = device.Network;
            var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
            if (xx == false)
            {

                try
                {
                    ChatFriendsListview.IsVisible = true;
                   
                    this.Title = AppResources.Label_Loading;
                    Functions.SyncContactsFromPhone().ConfigureAwait(false);
                }
                catch
                {

                }
            }
            ChatFriendsListview.ItemsSource = Functions.SearchFilterlist;
            ChatFriendsListview.EndRefresh();
            
          
        }

        private void Fillter_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SearchFilterPage());
        }

        private void Recommended_OnClicked_OnClicked(object sender, EventArgs e)
        {
            var device = Resolver.Resolve<IDevice>();
            var oNetwork = device.Network;
            var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
            if (xx == false)
            {

                try
                {
                    ChatFriendsListview.IsVisible = true;
                    var ss = Functions.GetRandomUsers().ConfigureAwait(false);
                }
                catch
                {

                }
            }
            ChatFriendsListview.ItemsSource = Functions.SearchFilterlist;
            ChatFriendsListview.EndRefresh();
        }

        private void AddFriends_OnDisappearing(object sender, EventArgs e)
        {
            UserDialogs.Instance.HideLoading();
        }

        private void SearchButton_OnClicked(object sender, EventArgs e)
        {
            Functions.SearchFilterlist.Clear();
            ChatFriendsListview.ItemsSource = null;
            UserDialogs.Instance.Toast(AppResources.Label_Loading);
            var ss = Functions.GetRandomUsers().ConfigureAwait(false);
            ChatFriendsListview.ItemsSource = Functions.SearchFilterlist;
            EmptySearchPage.IsVisible = false;
            ChatFriendsListview.IsVisible = true;
        }
    }
}
