using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using WowonderPhone.Controls;
using WowonderPhone.Pages;
using WowonderPhone.Pages.Register_pages;
using WowonderPhone.SQLite;
using WowonderPhone.SQLite.Tables;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using WowonderPhone.Languish;
using WowonderMobile.Controls;

//WOWONDER ANDROID MESSENGER V1.3
//==============================
//This is A sample of xamarin forms chat application 
//This solution is not allowed to be shared on Google play 
//You can get the styles and Xaml codes and use it as you like 
//Devloped by Elin Doughouz
//Follow me on linkded >> https://www.linkedin.com/in/elin-doughouz-1b7458131
//Follow me on Facebook >> https://www.facebook.com/Elindoughous

namespace WowonderPhone
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            if (Settings.Show_Facebook_Login == true)
            {
                Facebook.IsVisible = true;
            }
            if (Settings.Show_Google_Login == true)
            {
                Google.IsVisible = true;
            }
            if (Settings.Show_Twitter_Login == true)
            {
                Twitter.IsVisible = true;
            }
            if (Settings.Show_Vkontakte_Login == true)
            {
                Vkontakte.IsVisible = true;
            }
            if (Settings.Show_Instagram_Login == true)
            {
                Instagram.IsVisible = true;
            }


        }


        async void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {


                var answer =
                    await
                        DisplayAlert(AppResources.Label_Security, AppResources.Label_WouldYouLikeToSaveYourPassword,
                            AppResources.Label_Yes, AppResources.Label_NO);
                var device = Resolver.Resolve<IDevice>();
                var oNetwork = device.Network; // Create Interface to Network-functions
                var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                if (xx == true)
                {
                    await DisplayAlert(AppResources.Label_Error, AppResources.Label_CheckYourInternetConnection,
                        AppResources.Label_OK);
                }
                else
                {

                    var StatusApiKey = "";
                    using (var client = new HttpClient())
                    {

                        Settings.Session = RandomString(70);
                        UserDialogs.Instance.ShowLoading(AppResources.Label_Loading, MaskType.Gradient);
                        var SettingsValues = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("windows_app_version", Settings.Version),

                        });

                        var responseSettings =
                            await client.PostAsync(Settings.Website + "/app_api.php?type=get_settings", SettingsValues);
                        responseSettings.EnsureSuccessStatusCode();
                        string jsonSettings = await responseSettings.Content.ReadAsStringAsync();
                        var dataSettings = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonSettings);
                        string apiStatusSettings = dataSettings["api_status"].ToString();
                        if (apiStatusSettings == "200")
                        {
                            JObject settings = JObject.FromObject(dataSettings["config"]);

                            if (Settings.API_ID == settings["widnows_app_api_id"].ToString() &&
                                Settings.API_KEY == settings["widnows_app_api_key"].ToString())
                            {
                                StatusApiKey = "true";
                            }
                            else
                            {
                                StatusApiKey = "false";
                            }

                            Settings.Onesignal_APP_ID = settings["push_id"].ToString();
                            OneSignalNotificationController.RegisterNotificationDevice();

                            if (settings["footer_background"].ToString() != "#aaa" || StatusApiKey == "false")
                            {
                                await DisplayAlert("Security", "1- API-KEY And API-ID are incorrect or" + MimeType.Wrong + MimeType.Wrong2, "Yes");
                                UserDialogs.Instance.HideLoading();
                                return;
                            }
                        }
                        if (StatusApiKey == "true")
                        {

                            var TimeZoneContry = "UTC";
                            try
                            {
                                var formContenst = new FormUrlEncodedContent(new[]
                                    {new KeyValuePair<string, string>("username", usernameEntry.Text),});

                                var responseTime = await client.PostAsync("http://ip-api.com/json/", formContenst);
                                string jsonres = await responseTime.Content.ReadAsStringAsync();

                                var datares = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonres);

                                string ResulttimeZone = datares["status"].ToString();
                                if (ResulttimeZone == "success")
                                    TimeZoneContry = datares["timezone"].ToString();
                            }
                            catch (Exception)
                            {

                            }

                            var formContent = new FormUrlEncodedContent(new[]
                            {
                                new KeyValuePair<string, string>("username", usernameEntry.Text),
                                new KeyValuePair<string, string>("password", passwordEntry.Text),
                                new KeyValuePair<string, string>("timezone", TimeZoneContry),
                                new KeyValuePair<string, string>("device_id", Settings.Device_ID),
                                new KeyValuePair<string, string>("s", Settings.Session)
                            });

                            var response =
                                await
                                    client.PostAsync(
                                        Settings.Website + "/app_api.php?application=phone&type=user_login",
                                        formContent);
                            response.EnsureSuccessStatusCode();
                            string json = await response.Content.ReadAsStringAsync();
                            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                            string apiStatus = data["api_status"].ToString();
                            if (apiStatus == "200")
                            {

                                Settings.User_id = data["user_id"].ToString();
                               
                                    SQL_Commander.ClearChatUserTable();
                              
                               
                                    SQL_Commander.ClearContactTable();
                              

                                await LoginUserFunctions.GetSessionProfileData(Settings.User_id, Settings.Session);

                                //await Functions.GetChatActivity(Settings.User_id, Settings.Session);


                                if (answer)
                                {
                                   
                                        LoginTableDB LoginData = new LoginTableDB();
                                        LoginData.ID = 1;
                                        LoginData.Password = passwordEntry.Text;
                                        LoginData.Username = usernameEntry.Text;
                                        LoginData.Session = Settings.Session;
                                        LoginData.UserID = Settings.User_id;
                                        LoginData.Onesignal_APP_ID = Settings.Onesignal_APP_ID;
                                        LoginData.Status = "Active";
                                        SQL_Commander.InsertLoginCredentials(LoginData);
                                    
                                        

                                }

                                UserDialogs.Instance.HideLoading();

                                try
                                {
                                    if (Settings.ReLogin)
                                    {
                                        //await Navigation.PopAsync();
                                        //await Navigation.PushAsync(new MasterMainSlidePage());

                                    }
                                    else
                                    {
                                        App.GetMainPage();
                                        UserDialogs.Instance.HideLoading();
                                    }

                                }
                                catch
                                {
                                    await Navigation.PopModalAsync();
                                }

                            }
                            else if (apiStatus == "400")
                            {
                                UserDialogs.Instance.HideLoading();
                                JObject errors = JObject.FromObject(data["errors"]);
                                var errortext = errors["error_text"].ToString();
                                var ErrorMSG =
                                    await
                                        DisplayAlert(AppResources.Label_Security, errortext, AppResources.Label_Retry,
                                            AppResources.Label_Forget_My_Password);
                                if (ErrorMSG == false)
                                {
                                    await Navigation.PushModalAsync(new ForgetPasswordpage());
                                }


                            }
                        }
                        else
                        {
                            await DisplayAlert("Security",  "1- API-KEY And API-ID are incorrect or" + MimeType.Wrong + MimeType.Wrong2, "Yes");
                            UserDialogs.Instance.HideLoading();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Server ERROR", ex.ToString(), "Yes");
                
            }
        }

        
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXdsdaawerthklmnbvcxer46gfdsYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }


        private void Facebook_OnClicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new FacebookView());
            }
            catch
            {
                Navigation.PushModalAsync(new FacebookView());
            }
        }

        private void Google_OnClicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new GoogleView());
            }
            catch
            {
                Navigation.PushModalAsync(new GoogleView());
            }
        }

        private void Vk_OnClicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new VkontakteView());
            }
            catch
            {
                Navigation.PushModalAsync(new VkontakteView());
            }
        }

        private void Instagram_OnClicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new InstegramView());
            }
            catch
            {
                Navigation.PushModalAsync(new InstegramView());
            }
        }

        private void TwitterButton_OnClicked_OnClicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new TwitterView());
            }
            catch
            {
                Navigation.PushModalAsync(new TwitterView());
            }
        }

        private void OnCloseButtonClicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PopAsync(false);
            }
            catch (Exception)
            {

                Navigation.PopModalAsync(true);
            }
           
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new ForgetPasswordpage());
            }
            catch
            {
                Navigation.PushModalAsync(new ForgetPasswordpage());
            }
        }

        
    }
}
