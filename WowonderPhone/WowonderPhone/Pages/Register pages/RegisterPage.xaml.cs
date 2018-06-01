using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WowonderPhone.Controls;
using WowonderPhone.SQLite;
using WowonderPhone.SQLite.Tables;
using Xamarin.Forms;
using XLabs;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using WowonderPhone.Languish;
using WowonderMobile.Controls;

namespace WowonderPhone.Pages.Register_pages
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }

        private async void RegisterButton_OnClicked(object sender, EventArgs e)
        {
            var device = Resolver.Resolve<IDevice>();
            var oNetwork = device.Network; // Create Interface to Network-functions
            var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
            if (xx == true)
            {
                await DisplayAlert(AppResources.Label_Error, AppResources.Label_CheckYourInternetConnection, AppResources.Label_OK);
            }
            else
            {

                var clickresult = "true";
                if (Settings.AcceptTerms_Display)
                {
                    var result = await DisplayAlert(AppResources.Label_Terms, AppResources.Label_Terms_LongText, AppResources.Label_Accept_Terms, AppResources.Label_Dot_Agree);
                    if (result)
                    {
                        clickresult = "true";
                    }
                    else
                    {
                        clickresult = "false";
                    }
                }


                if (clickresult == "true")
                {

                    var StatusApiKey = "";
                    using (var client = new HttpClient())
                    {
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
                                await DisplayAlert("FATTAL ERROR", "1- API-KEY And API-ID are incorrect or" + MimeType.Wrong + MimeType.Wrong2, "OK");
                                UserDialogs.Instance.HideLoading();
                                return;
                            }
                        }
                        if (StatusApiKey == "true")
                        {
                            string Gender = "male";
                          

                            using (var client2 = new HttpClient())
                            {

                                Settings.Session = GetTimestamp(DateTime.Now);
                                var formContent = new FormUrlEncodedContent(new[]
                                {
                                   new KeyValuePair<string, string>("username", usernameEntry.Text),
                                   new KeyValuePair<string, string>("password", passwordEntry.Text),
                                   new KeyValuePair<string, string>("confirm_password", passwordConfirmEntry.Text),
                                   new KeyValuePair<string, string>("email", EmailEntry.Text),
                                   new KeyValuePair<string, string>("gender", Gender),
                                   new KeyValuePair<string, string>("device_id", Settings.Device_ID),
                                   new KeyValuePair<string, string>("s", Settings.Session)

                                 });

                                var response =
                                    await
                                        client2.PostAsync(
                                            Settings.Website + "/app_api.php?application=phone&type=user_registration", formContent);
                                response.EnsureSuccessStatusCode();
                                string json = await response.Content.ReadAsStringAsync();
                                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                                string apiStatus = data["api_status"].ToString();
                                if (apiStatus == "200")
                                {
                                    var successtype = data["success_type"].ToString();
                                  
                                        SQL_Commander.ClearContactTable();
                                

                                    if (successtype == "registered")
                                    {
                                        //Settings.Cookie = data["cookie"].ToString();
                                        Settings.User_id = data["user_id"].ToString();
                                        Settings.Session = data["session_id"].ToString();
                                       
                                            LoginTableDB LoginData = new LoginTableDB();
                                            LoginData.Password = passwordEntry.Text;
                                            LoginData.Username = usernameEntry.Text;
                                            LoginData.Session = Settings.Session;
                                            LoginData.UserID = Settings.User_id;
                                            // LoginData.Cookie = Settings.Cookie;
                                            LoginData.Status = "Registered";
                                        SQL_Commander.InsertLoginCredentials(LoginData);
                                        

                                        UserDialogs.Instance.HideLoading();

                                        try
                                        {
                                            await Navigation.PushAsync(new UploudPicPage());
                                        }
                                        catch (Exception)
                                        {

                                            await Navigation.PushModalAsync(new UploudPicPage());
                                        }

                                    }
                                    else if (successtype == "verification")
                                    {
                                        var Message = data["message"].ToString();
                                        UserDialogs.Instance.HideLoading();
                                        await DisplayAlert(AppResources.Label_Error, Message, AppResources.Label_OK);
                                    }
                                }
                                else if (apiStatus == "400")
                                {
                                    JObject errors = JObject.FromObject(data["errors"]);
                                    var errortext = errors["error_text"].ToString();
                                    await DisplayAlert(AppResources.Label_Error, errortext, AppResources.Label_OK);
                                    UserDialogs.Instance.HideLoading();
                                }

                            }
                        }
                    }
                }
                else
                {
                    await DisplayAlert(AppResources.Label_Terms, AppResources.Label_Terms_notAccepted, AppResources.Label_OK);
                }
            }
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
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

        private void Button_OnClicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PopAsync(false);
                Navigation.PushAsync(new MainPage());
            }
            catch (Exception)
            {

                Navigation.PopModalAsync(true);
                Navigation.PushModalAsync(new MainPage());
            }
        }
    }
}
