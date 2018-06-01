using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WowonderPhone.Classes;
using WowonderPhone.Controls;
using WowonderPhone.Dependencies;
using WowonderPhone.Pages.CustomCells;
using WowonderPhone.SQLite;
using Xamarin.Forms;
using XLabs.Forms.Controls;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using WowonderPhone.Languish;

namespace WowonderPhone.Pages.Register_pages
{
    public partial class RegisterFriends : ContentPage
    {
        public static string SearchFilter = "All";

        public static List<SearchResult> RecomendedFilterlist = new List<SearchResult>();

        public RegisterFriends()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            if (Settings.ConnectivitySystem == "1")
            {
                AddButton.Text = AppResources.Label_Follow_All_And_Next;

                AddButton.BackgroundColor = Color.FromHex(Settings.MainColor);
            }
            else
            {
                AddButton.Text = AppResources.Label_Add_All_And_Next;

                AddButton.BackgroundColor = Color.FromHex(Settings.MainColor);
            }
            try
            {
                AnimateIn();

                var device = Resolver.Resolve<IDevice>();
                var oNetwork = device.Network;
                var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                if (xx == true)
                {
                    UserDialogs.Instance.ShowError(AppResources.Label_CheckYourInternetConnection, 2000);
                }
                else
                {


                    GetRandomUsers().ConfigureAwait(false);

                }
            }
            catch (Exception)
            {

            }

        }

        public async Task AnimateIn()
        {
            await Task.WhenAll(new[]
            {
                AnimateItem(AddButton, 700),
            });
        }

        private async Task AnimateItem(View uiElement, uint duration)
        {
            if (uiElement == null)
            {
                return;
            }

            await Task.WhenAll(new Task[]
            {
                uiElement.ScaleTo(1.5, duration, Easing.CubicIn),
                uiElement.FadeTo(1, duration/2, Easing.CubicInOut).ContinueWith(
                    _ =>
                    {
                        // Queing on UI to workaround an issue with Forms 2.1
                        Device.BeginInvokeOnMainThread(() => {
                            uiElement.ScaleTo(1, duration, Easing.CubicOut);
                        });
                    })
            });
        }

        private void PopulateUsersLists(List<SearchResult> UserList)
        {
            var lastHeight = "74";
            var y = 0;
            var column = LeftColumn;
            var productTapGestureRecognizer = new TapGestureRecognizer();
            productTapGestureRecognizer.Tapped += OnImageTapped;

            for (var i = 0; i < UserList.Count; i++)
            {
                var item = new UsersImageGrid();

                if (i == 1 || i == 4 || i == 7 || i == 10 || i == 13 || i == 16 || i == 19)
                {
                    column = LeftColumn;
                    y++;
                }
                else if (i == 2 || i == 5 || i == 8 || i == 11 || i == 14 || i == 17 || i == 20)
                {
                    column = CenterColumn;
                }
                else
                {

                    column = RightColumn;
                }

                UserList[i].ThumbnailHeight = lastHeight;
                item.BindingContext = UserList[i];
                item.GestureRecognizers.Add(productTapGestureRecognizer);
                column.Children.Add(item);

            }
            ButtonStack.IsVisible = true;
        }

        private async void OnImageTapped(object sender, EventArgs e)
        {
            //var selectedItem = (SearchResult)((UsersImageGrid)sender).BindingContext;

            //var ProfileView = new UserProfilePage(selectedItem.ResultID, "")
            //{
            //    BindingContext = selectedItem
            //};

            //await Navigation.PushAsync(ProfileView);
        }

        public async Task<string> GetRandomUsers()
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
                        RecomendedFilterlist.Clear();
                        var users = JObject.Parse(json).SelectToken("users_random").ToString();
                        JArray Chatusers = JArray.Parse(users);

                        foreach (var ChatUser in Chatusers)
                        {
                            JObject ChatlistUserdata = JObject.FromObject(ChatUser);
                            var ChatUser_User_ID = ChatlistUserdata["user_id"].ToString();
                            var ChatUser_avatar = ChatlistUserdata["profile_picture"].ToString();
                            var ChatUser_name = ChatlistUserdata["first_name"].ToString();

                            var ImageMediaFile = new UriImageSource
                            {
                                Uri = new Uri(ChatUser_avatar)
                            };

                            if (ChatUser_name == "")
                            {
                                ChatUser_name = ChatlistUserdata["username"].ToString();
                            }
                            #region Data Adding

                            var status = AppResources.Label_AddFriend;
                            if (Settings.ConnectivitySystem == "1")
                            {
                                status = AppResources.Label_Follow;
                            }

                            RecomendedFilterlist.Add(new SearchResult()
                            {
                                BigLabel = ChatUser_name,
                                ResultType = "Users",
                                Name = ChatUser_name,
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

                            #endregion
                        }

                    }
                    else if (apiStatus == "400")
                    {
                        json = AppResources.Label_Error;
                        Loaderspinner.IsVisible = false;
                    }



                    if (RecomendedFilterlist.Count == 0)
                    {
                        //RecomendedFilterlist.Add(new SearchResult()
                        //{

                        //};
                    }
                    Loaderspinner.IsVisible = false;
                    PopulateUsersLists(RecomendedFilterlist);
                    return json;

                }
            }
            catch
            {
                Loaderspinner.IsVisible = false;
                UserDialogs.Instance.ShowError(AppResources.Label_Connection_Lost, 2000);
                return AppResources.Label_Error;
            }
        }

        public async Task<string> FollowAll()
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

                    var response =
                        await
                            client.PostAsync(
                                Settings.Website + "/app_api.php?application=phone&type=get_suggestions&follow=1",
                                formContent);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string apiStatus = data["api_status"].ToString();
                    if (apiStatus == "200")
                    {
                        return null;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
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

        private void OnAddClicked(object sender, EventArgs e)
        {
            try
            {
                FollowAll().ConfigureAwait(false);
                App.GetMainPage();

            }
            catch (Exception)
            {
                
            }
        }
    }
}