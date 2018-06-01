using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WowonderPhone.Languish;
using WowonderPhone.SQLite;
using Xamarin.Forms;
using XLabs;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using Xamarin.Forms.Xaml;

namespace WowonderPhone.Pages.Timeline_Pages.SettingsNavPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Account_Page : ContentPage
    {

        public string Username = "";
        public string Email = "";
        public string Phone_number = "";
        public string GenderStatus = "";

        public Account_Page()
        {
            InitializeComponent();

            StyleChanger();

            try
            {
               
                    var DataRow = SQL_Commander.GetProfileCredentialsById(Settings.User_id);

                    UsernameEntry.Text = DataRow.Username;
                    EmailEntry.Text = DataRow.Email;
                    PhoneEntry.Text = DataRow.Phone_number;

                    Username = DataRow.Username;
                    Email = DataRow.Email;
                    Phone_number = DataRow.Phone_number;

                    if (DataRow.Gender == "Male" || DataRow.Gender == "male" || DataRow.Gender == AppResources.Label_Male)
                    {
                        Male.Checked = true;
                        GenderStatus = "male";
                        Female.Checked = false;
                    }
                    else
                    {
                        Male.Checked = false;
                        Female.Checked = true;
                        GenderStatus = "female";
                    }
               
            }
            catch (Exception)
            {

            }
        }

        private void Male_OnCheckedChanged(object sender, EventArgs<bool> e)
        {
            if (Male.Checked)
            {
                Female.Checked = false;
            }
        }

        private void Female_OnCheckedChanged(object sender, EventArgs<bool> e)
        {
            if (Female.Checked)
            {
                Male.Checked = false;
            }
        }

        public void StyleChanger()
        {
            PhoneEntry.Placeholder = AppResources.Label_Empty;
            EmailEntry.Placeholder = AppResources.Label_Empty;
            UsernameEntry.Placeholder = AppResources.Label_Empty;
            CurrentPasswordEntry.Placeholder = AppResources.Label_Empty;
            NewpasswordEntry.Placeholder = AppResources.Label_Empty;
            RepeatPasswordEntry.Placeholder = AppResources.Label_Empty;
        }
        

        private async void Save_OnClicked(object sender, EventArgs e)
        {
            try
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

                    if (NewpasswordEntry.Text != RepeatPasswordEntry.Text)
                    {
                        await DisplayAlert(AppResources.Label_Error, AppResources.Label_Your_Password_Dont_Match, AppResources.Label_OK);
                        return;
                    }
                    else
                    {
                        #region Password Update

                        if (NewpasswordEntry.Text != null && RepeatPasswordEntry.Text != null &&
                            CurrentPasswordEntry.Text != null)
                        {

                            using (var client = new HttpClient())
                            {
                                var Passwords = new Dictionary<string, string>
                                {
                                    {"current_password", CurrentPasswordEntry.Text},
                                    {"new_password", NewpasswordEntry.Text},
                                    {"repeat_new_password", RepeatPasswordEntry.Text}
                                };

                                string Pass = JsonConvert.SerializeObject(Passwords);

                                var formContent = new FormUrlEncodedContent(new[]
                                {
                                    new KeyValuePair<string, string>("user_id", Settings.User_id),
                                    new KeyValuePair<string, string>("type", "password_settings"),
                                    new KeyValuePair<string, string>("s", Settings.Session),
                                    new KeyValuePair<string, string>("user_data", Pass)
                                });

                                var response =
                                    await
                                        client.PostAsync(
                                            Settings.Website + "/app_api.php?application=phone&type=u_user_data",
                                            formContent);
                                response.EnsureSuccessStatusCode();
                                string json = await response.Content.ReadAsStringAsync();
                                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                                string apiStatus = data["api_status"].ToString();
                                if (apiStatus == "200")
                                {
                                    Save.Text = AppResources.Label_Saved;
                                }

                                else if (apiStatus == "500")
                                {

                                    var errors_sugParse = JObject.Parse(json).SelectToken("errors").ToString();
                                    Object usersobj = JsonConvert.DeserializeObject(errors_sugParse);
                                    JArray errors = JArray.Parse(errors_sugParse);


                                    foreach (var error in errors)
                                    {
                                        await DisplayAlert(AppResources.Label_Error, error.ToString(), AppResources.Label_OK);
                                    }

                                }
                                else
                                {

                                }
                            }
                        }

                        #endregion

                        #region General Settings

                        string Gender = "male";
                        if (Female.Checked == true)
                        {
                            Gender = "female";
                        }

                        if (UsernameEntry.Text != Username || EmailEntry.Text != Email ||
                            PhoneEntry.Text != Phone_number ||
                            Gender != GenderStatus)
                        {
                            var dictionary = new Dictionary<string, string>
                            {
                                {"username", UsernameEntry.Text},
                                {"email", EmailEntry.Text},
                                {"gender", Gender},
                                {"phone_number", PhoneEntry.Text},
                            };

                            string Data = JsonConvert.SerializeObject(dictionary);

                            using (var client = new HttpClient())
                            {
                                var formContent = new FormUrlEncodedContent(new[]
                                {
                                    new KeyValuePair<string, string>("user_id", Settings.User_id),
                                    new KeyValuePair<string, string>("type", "general_settings"),
                                    new KeyValuePair<string, string>("s", Settings.Session),
                                    new KeyValuePair<string, string>("user_data", Data)
                                });

                                var response =
                                    await client.PostAsync(Settings.Website + "/app_api.php?application=phone&type=u_user_data", formContent);
                                response.EnsureSuccessStatusCode();

                                string json = await response.Content.ReadAsStringAsync();
                                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                                string apiStatus = data["api_status"].ToString();
                                if (apiStatus == "200")
                                {
                                   
                                        var Update = SQL_Commander.GetProfileCredentialsById(Settings.User_id);
                                        Update.Email = EmailEntry.Text;
                                        Update.Username = UsernameEntry.Text;
                                        Update.Phone_number = PhoneEntry.Text;
                                        Update.Gender = Gender;
                                    SQL_Commander.UpdateProfileCredentials(Update);

                                    


                                    Save.Text = AppResources.Label_Saved;
                                }

                                else if (apiStatus == "500")
                                {

                                    var errors_sugParse = JObject.Parse(json).SelectToken("errors").ToString();
                                    Object usersobj = JsonConvert.DeserializeObject(errors_sugParse);
                                    JArray errors = JArray.Parse(errors_sugParse);

                                    foreach (var error in errors)
                                    {
                                        await DisplayAlert(AppResources.Label_Error, error.ToString(), AppResources.Label_OK);
                                    }

                                }
                                else
                                {

                                }
                            }
                        }

                        #endregion
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
