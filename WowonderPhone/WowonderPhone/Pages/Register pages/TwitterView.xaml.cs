using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WowonderPhone.Controls;
using WowonderPhone.SQLite;
using WowonderPhone.SQLite.Tables;
using Xamarin.Forms;

namespace WowonderPhone.Pages.Register_pages
{
    public partial class TwitterView : ContentPage
    {
        public TwitterView()
        {
            InitializeComponent();
            HashSet = RandomString(80);
            WebView1.Source = Settings.Website +
                              "/app_api.php?application=phone&type=user_login_with&provider=Twitter&hash=" + HashSet;
            TimerLogin().ConfigureAwait(false);
        }
       
        public static string HashSet = "";
        public static string Result = "false";
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXdsdaawerthklmnbvcxer46gfdsYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<string> FacebookLogin()
        {
            try
            {
                using (var client = new HttpClient())
                {

                    var response =
                        await client.GetAsync(Settings.Website + "/app_api.php?application=phone&type=check_hash&hash_id=" + HashSet);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string apiStatus = data["api_status"].ToString();
                    if (apiStatus == "200")
                    {
                        JObject userdata = JObject.FromObject(data["user_data"]);
                        Settings.User_id = userdata["user_id"].ToString();
                        Settings.Session = userdata["session_id"].ToString();

                       
                            LoginTableDB LoginData = new LoginTableDB();
                            LoginData.Session = Settings.Session;
                            LoginData.UserID = Settings.User_id;
                            LoginData.Status = "Active";
                        SQL_Commander.InsertLoginCredentials(LoginData);
                        
                        Result = "True";


                        Functions.SearchFilterlist.Clear();
                        Functions.GetRandomUsers().ConfigureAwait(false);

                        Device.BeginInvokeOnMainThread(() => {
                            try
                            {
                                Navigation.PushAsync(new RegisterFriends());
                            }
                            catch (Exception)
                            {
                                Navigation.PushModalAsync(new RegisterFriends());
                            }
                        });

                    }
                }
            }
            catch
            {

            }

            return "true";
        }


        public async Task<string> TimerLogin()
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                for (int i = 0; i < 5; i++)
                {
                    while (Result == "false")
                    {
                        await Task.Delay(3000);
                        await FacebookLogin();
                        if (Result == "True")
                        {

                            return "Stop";
                        }

                    }
                }
            }
            catch
            {

            }

            return "true";
        }

        private void WebView1_OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            loadingPanel.IsVisible = false;
        }
    }
}
