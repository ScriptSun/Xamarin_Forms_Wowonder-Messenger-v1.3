using System;
using System.Threading.Tasks;

using WowonderPhone.Controls;
using WowonderPhone.Languish;
using WowonderPhone.SQLite;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;


namespace WowonderPhone.Pages.Register_pages
{
    public partial class WalkThrough_Page1 : ContentPage
    {
        public WalkThrough_Page1()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            //Gifplayer.Source = Settings.GIF_ProUpgrade;

            try
            {
                var device = Resolver.Resolve<IDevice>();
                var oNetwork = device.Network;
                var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                if (xx == true)
                {

                }
                else
                {
                    Span1.Text = AppResources.Label_Loading_User_Contacts;
                    LoadContacts();
                }
            }
            catch (Exception)
            {

            }

        }


        public async void LoadContacts()
        {
            await Functions.GetChatContacts(Settings.User_id, Settings.Session);
            Span1.Text = AppResources.Label_User_Contacts_Loaded;
            Spanchek1.Text = "\uf05d";
            await Task.Delay(1000);
            Span2.Text = AppResources.Label_Loading_User_Data;
            Spanchek2.Text = "\uf0da";
            await LoginUserFunctions.GetSessionProfileData(Settings.User_id, Settings.Session);
            await Task.Delay(3000);
            Span2.Text = AppResources.Label_User_Data_Loaded;
            Spanchek2.Text = "\uf05d";
            await Task.Delay(1000);
            //Span3.Text = AppResources.Label_Loading_User_Settings;
            Spanchek3.Text = "\uf0da";
            //await GetCommunities();
            await Task.Delay(6000);
           // Span3.Text = AppResources.Label_User_Settings_Loaded;
            Spanchek3.Text = "\uf05d";
           // AnimationView.IsPlaying = false;
           // AnimationView.Pause();
            isPageLoaded = false;
           // AnimationView.IsVisible = false;

            NextButton.Text = AppResources.Label_Finish;
            //AnimationView2.IsVisible = true;
            //AnimationView2.Play();

        }

        private void NextButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (NextButton.Text == AppResources.Label_Finish)
                {
                    

                        var CredentialStatus = SQL_Commander.GetLoginCredentialsStatus();
                        if (CredentialStatus == "Registered")
                        {
                            var Credential = SQL_Commander.GetLoginCredentials("Registered");
                            Credential.Status = "Active";
                        SQL_Commander.UpdateLoginCredentials(Credential);
                            Settings.Session = Credential.Session;
                            Settings.User_id = Credential.UserID;
                            Settings.Username = Credential.Username;
                        }

                    
                    App.GetMainPage();
                }

            }
            catch (Exception)
            {
                Navigation.PushModalAsync(new RegisterFriends());
            }
        }

        private bool isPageLoaded = true;

        private void WalkThrough_Page1_OnAppearing(object sender, EventArgs e)
        {
            try
            {
                //AnimationView.Play();

                Device.StartTimer(TimeSpan.FromMilliseconds(2500), () =>
                {
                    if (isPageLoaded)
                    {
                       // AnimationView.Play();
                    }

                    return isPageLoaded;
                });
            }
            catch (Exception)
            {
              
            }
           

        }

        
        private void WalkThrough_Page1_OnDisappearing(object sender, EventArgs e)
        {
            isPageLoaded = false;
        }
    }
}
