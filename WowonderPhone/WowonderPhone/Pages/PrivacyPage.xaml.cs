using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WowonderPhone.SQLite;
using WowonderPhone.SQLite.Tables;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using WowonderPhone.Languish;
using Xamarin.Forms.Xaml;

namespace WowonderPhone.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrivacyPage : ContentPage
    {
        public PrivacyPage()
        {
            InitializeComponent();

            Save.BackgroundColor = Color.FromHex(Settings.PR_SaveButton_Color);
            Save.TextColor = Color.FromHex(Settings.PR_SaveButtonText_Color);
			FolowOrFriendLabel.Text = AppResources.Label_WhoCan_Follow_me;
			WhoCanSeemybirthdaylabel.Text = AppResources.Label_WhoCan_See_my_Birthday;
			WhoCanmessgamelabel.Text = AppResources.Label_WhoCan_Message_me;

			if (Settings.ConnectivitySystem == "0")
			{
				FolowOrFriendLabel.Text = AppResources.Label_WhoCan_Add_me;
			}


          
                var data = SQL_Commander.GetPrivacyDBCredentialsById(Settings.User_id);
                if (data != null)
                {
                    WhocamessagemePicker.SelectedIndex = data.WhoCanMessageMe;
                    Whocanfollowme.SelectedIndex = data.WhoCanFollowMe;
                    Whocanseemybirthday.SelectedIndex = data.WhoCanSeeMyBirday;
                }
                else
                {
                    WhocamessagemePicker.SelectedIndex = 0;
                    Whocanfollowme.SelectedIndex = 0;
                    Whocanseemybirthday.SelectedIndex = 0;
                }

            
           
                var Status = SQL_Commander.GetLoginCredentialsByUserID(Settings.User_id);
                if (Status != null)
                {
                    if (Status.MyConnectionStatus == "0")
                    {
                        SwitchCellOnline.On = true;
                    }
                    else
                    {
                        SwitchCellOnline.On = false;
                    }
                }
                else
                {
                    SwitchCellOnline.On = true;
                }
           
        }

        private async void SwitchCellOnline_OnOnChanged(object sender, ToggledEventArgs e)
        {
            try
            {
                var device = Resolver.Resolve<IDevice>();
                var oNetwork = device.Network;
                var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                if (xx == true)
                {
                    await DisplayAlert(AppResources.Label_Error, AppResources.Label_CheckYourInternetConnection, AppResources.Label_OK);
                }
                else
                {
                    var StatusNumber = "0";
                    if (SwitchCellOnline.On)
                    {
                        StatusNumber = "0";
                        SwitchCellOnline.Text = AppResources.Label_Online;
                    }
                    else
                    {
                        StatusNumber = "1";
                        SwitchCellOnline.Text = AppResources.Label_Offline;
                    }
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            var Status = new Dictionary<string, string> { { "status", StatusNumber } };
                            string StatusActive = JsonConvert.SerializeObject(Status);
                            var formContent = new FormUrlEncodedContent(new[]
                            {
                                new KeyValuePair<string, string>("user_id", Settings.User_id),
                                new KeyValuePair<string, string>("type", "online_status"),
                                new KeyValuePair<string, string>("s", Settings.Session),
                                new KeyValuePair<string, string>("user_data", StatusActive)
                            });

                            var response = await client.PostAsync(Settings.Website + "/app_api.php?application=phone&type=update_user_data", formContent);
                            response.EnsureSuccessStatusCode();
                            string json = await response.Content.ReadAsStringAsync();
                            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                            string apiStatus = data["api_status"].ToString();
                            if (apiStatus == "200")
                            {
                                
                                    var StatusConnection = SQL_Commander.GetLoginCredentialsByUserID(Settings.User_id);
                                    StatusConnection.MyConnectionStatus = StatusNumber;

                                SQL_Commander.UpdateLoginCredentials(StatusConnection);
                                
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }


                }
            }
            catch (Exception)
            {

            }
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
                    #region Send Data 

                    using (var client = new HttpClient())
                    {
                        var Passwords = new Dictionary<string, string>
                        {
                            {"message_privacy", WhocamessagemePicker.SelectedIndex.ToString()},
                            {"follow_privacy", Whocanfollowme.SelectedIndex.ToString()},
                            {"birth_privacy", Whocanseemybirthday.SelectedIndex.ToString()}
                        };

                        string Pass = JsonConvert.SerializeObject(Passwords);
                        var formContent = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("user_id", Settings.User_id),
                            new KeyValuePair<string, string>("type", "privacy_settings"),
                            new KeyValuePair<string, string>("s", Settings.Session),
                            new KeyValuePair<string, string>("user_data", Pass)
                        });

                        var response =
                            await
                                client.PostAsync(
                                    Settings.Website + "/app_api.php?application=phone&type=update_user_data",
                                    formContent);
                        response.EnsureSuccessStatusCode();
                        string json = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                        string apiStatus = data["api_status"].ToString();
                        if (apiStatus == "200")
                        {

                           
								var ID = SQL_Commander.GetPrivacyDBCredentialsById(Settings.User_id);
								if (ID != null)
								{
                                SQL_Commander.UpdatePrivacyDBCredentials(new PrivacyDB()
									{
										UserID = Settings.User_id,
										WhoCanSeeMyBirday = Whocanseemybirthday.SelectedIndex,
										WhoCanFollowMe = Whocanfollowme.SelectedIndex,
										WhoCanMessageMe = WhocamessagemePicker.SelectedIndex
									});
								}
								else {
                                SQL_Commander.InsertPrivacyDBCredentials(new PrivacyDB()
									{
										UserID = Settings.User_id,
										WhoCanSeeMyBirday = Whocanseemybirthday.SelectedIndex,
										WhoCanFollowMe = Whocanfollowme.SelectedIndex,
										WhoCanMessageMe = WhocamessagemePicker.SelectedIndex
									});
								}
                            
                            Save.Text = "Saved";    
                        }
                        else
                        {
                            Save.Text = "Save";
                        }
                    }
                }

                #endregion

                }
            catch (Exception)
            {

            }
        }

        private void Switch_OnToggled(object sender, ToggledEventArgs e)
        {
            
        }
    }
}
