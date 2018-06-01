using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using WowonderPhone.Controls;
using WowonderPhone.Languish;
using Xamarin.Forms;
using XLabs.Forms.Controls;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using Xamarin.Forms.Xaml;

namespace WowonderPhone.Pages.Timeline_Pages.SettingsNavPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BlockedUsersPage : ContentPage
    {
        public BlockedUsersPage()
        {
            InitializeComponent();


            this.Title = AppResources.Label_Blocked_Users;
            var device = Resolver.Resolve<IDevice>();
            var oNetwork = device.Network; // Create Interface to Network-functions
            var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
            if (!xx)
            {
                if (BlockedUsersListItemsCollection.Count > 0)
                {
                    BlockedUsersListItemsCollection.Clear();
                }
                UserDialogs.Instance.Toast(AppResources.Label_Loading + " " + AppResources.Label_Blocked_Users);
                Get_BlockedList().ConfigureAwait(false);
                BlockListview.ItemsSource = BlockedUsersListItemsCollection;
            }
            else
            {
                UserDialogs.Instance.ShowError(AppResources.Label_You_are_still_Offline);
            }


        }

        [ImplementPropertyChanged]
        public class BlockedUsers
        {
            public string User_id { get; set; }
            public string Username { get; set; }
            public string Name { get; set; }
            public ImageSource Image { get; set; }
            public string LastSeen { get; set; }
            public string ButtonBGColor { get; set; }
            public string BlockTextColor { get; set; }
            public string BlockButtonText { get; set; }

        }

        public static ObservableCollection<BlockedUsers> BlockedUsersListItemsCollection =
            new ObservableCollection<BlockedUsers>();

        public async Task Get_BlockedList()
        {
            try
            {
                //MasterMainSlidePage.NotificationsList.Clear();

                using (var client = new HttpClient())
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user_id", Settings.User_id),
                        new KeyValuePair<string, string>("s", Settings.Session)
                    });

                    var response =
                        await
                            client.PostAsync(
                                Settings.Website + "/app_api.php?application=phone&type=get_blocked_users", formContent);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string apiStatus = data["api_status"].ToString();
                    if (apiStatus == "200")
                    {
                        #region Notification

                        var Parser = JObject.Parse(json).SelectToken("blocked_users").ToString();
                        JArray BlockedArray = JArray.Parse(Parser);

                        if (BlockedArray.Count == 0)
                        {
                            EmptyBlockedPage.IsVisible = true;
                            BlockListview.IsVisible = false;
                        }
                        foreach (var Blocked in BlockedArray)
                        {

                            JObject Data = JObject.FromObject(Blocked);
                            var User_id = Data["user_id"].ToString();
                            var Username = Data["username"].ToString();
                            var Avatar = Data["profile_picture"].ToString();
                            var Name = Data["name"].ToString();
                            var lastseen_time_text = Data["lastseen_time_text"].ToString();

                            BlockedUsersListItemsCollection.Add(new BlockedUsers()
                            {
                                Name = Name,
                                User_id = User_id,
                                Username = Username,
                                Image = new UriImageSource
                                {
                                    Uri = new Uri(Avatar),
                                    CachingEnabled = true,
                                    CacheValidity = new TimeSpan(2, 0, 0, 0)
                                },
                                LastSeen = lastseen_time_text,
                                ButtonBGColor = Settings.ButtonLightColor,
                                BlockButtonText = AppResources.Label_Blocked,
                                BlockTextColor = Settings.ButtonTextLightColor,

                            });


                        }

                    }

                    #endregion
                }
            }
            catch (Exception)
            {

            }
        }

        private void BlockListview_OnItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void BlockListview_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            BlockListview.SelectedItem = null;
        }

        private void ActionButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var mi = ((ImageButton)sender);
                if (mi != null)
                {
                    var getuser = BlockedUsersListItemsCollection.Where(a => a.User_id == mi.CommandParameter.ToString()).ToList().FirstOrDefault();
                    if (getuser != null)
                    {
                        if (getuser.BlockButtonText == AppResources.Label_Blocked)
                        {
                            if (getuser.User_id == mi.CommandParameter.ToString())
                            {
                                getuser.ButtonBGColor = Settings.ButtonColorNormal;
                                getuser.BlockTextColor = Settings.ButtonTextColorNormal;
                                getuser.BlockButtonText = AppResources.Label_Block;
                                SendUnBlockRequest(mi.CommandParameter.ToString()).ConfigureAwait(false);
                            }
                        }
                        else
                        {
                            if (getuser.User_id == mi.CommandParameter.ToString())
                            {
                                getuser.ButtonBGColor = Settings.ButtonLightColor;
                                getuser.BlockTextColor = Settings.ButtonTextLightColor;
                                getuser.BlockButtonText = AppResources.Label_Blocked;
                                SendBlockRequest(mi.CommandParameter.ToString()).ConfigureAwait(false);
                            }
                        }

                    }

                }
            }
            catch (Exception)
            {
              return;
            }
           
        }

        public static async Task<string> SendBlockRequest(string recipient_id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user_id", Settings.User_id),
                        new KeyValuePair<string, string>("block_type", "block"),
                         new KeyValuePair<string, string>("recipient_id", recipient_id),
                        new KeyValuePair<string, string>("s", Settings.Session)
                    });

                    var response =
                        await
                            client.PostAsync(Settings.Website + "/app_api.php?application=phone&type=block_user",
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

        public static async Task<string> SendUnBlockRequest(string recipient_id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user_id", Settings.User_id),
                        new KeyValuePair<string, string>("block_type", "un-block"),
                           new KeyValuePair<string, string>("recipient_id", recipient_id),

                        new KeyValuePair<string, string>("s", Settings.Session)
                    });

                    var response =
                        await
                            client.PostAsync(Settings.Website + "/app_api.php?application=phone&type=block_user",
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
    }
}
