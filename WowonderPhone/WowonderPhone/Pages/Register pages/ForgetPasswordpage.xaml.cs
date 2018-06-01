using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using WowonderPhone.Languish;

namespace WowonderPhone.Pages.Register_pages
{
    public partial class ForgetPasswordpage : ContentPage
    {
        public ForgetPasswordpage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
           
        }

        private void SendButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var device = Resolver.Resolve<IDevice>();
                var oNetwork = device.Network; // Create Interface to Network-functions
                var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                if (!xx)
                {
                    SendEmailFunction().ConfigureAwait(false);
                }
                else
                {
                    UserDialogs.Instance.ShowError(AppResources.Label_CheckYourInternetConnection, 2000);
                }
            }
            catch (Exception)
            {
                
            }
           
        }

        public async Task SendEmailFunction()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (!EmailEntry.Text.Contains("@"))
                    {
                        await DisplayAlert(AppResources.Label_Error, AppResources.Label_Please_Write_your_full_email , AppResources.Label_OK);
                    }
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("email", EmailEntry.Text),
                    });

                    var response =
                        await
                            client.PostAsync(Settings.Website + "/app_api.php?application=phone&type=reset_pass",
                                formContent);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string apiStatus = data["api_status"].ToString();
                    if (apiStatus == "200")
                    {
                        UserDialogs.Instance.Toast(AppResources.Label_Email_Has_Been_Send);
                    }
                    else
                    {
                        JObject errors = JObject.FromObject(data["errors"]);
                        var errortext = errors["error_text"].ToString();
                        await DisplayAlert(AppResources.Label_Security, errortext, AppResources.Label_Retry);
                    }
                }
            }
            catch (Exception)
            {
                UserDialogs.Instance.ShowError(AppResources.Label_Error, 2000);
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
    }
}
