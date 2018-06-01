using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WowonderPhone.Controls;
using WowonderPhone.Dependencies;
using WowonderPhone.Pages.Timeline_Pages;
using WowonderPhone.SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;

namespace WowonderPhone.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterMain : TabbedPage
    {
        public MasterMain()
        {
            //NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            GetMyprofile();
        }

        private void Logout_OnClicked(object sender, EventArgs e)
        {
          
           SQL_Commander.ClearLoginCredentialsList();  
           SQL_Commander.ClearProfileCredentialsList();           
           SQL_Commander.ClearMessageList();     
           SQL_Commander.DeletAllChatUsersList();
           SQL_Commander.DeletAllChatUsersList();

           WowonderPhone.Settings.ReLogin = true;
           Navigation.PopAsync();
           Navigation.PushAsync(new MainPage());
        }

        private void Searchcontact_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SearchPage());
        }

        private void Newfriends_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Search_Page());
        }

        private void Settings_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage());
        }

        private void MyProfile_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MyProfilePage());
        }

        public void GetMyprofile()
        {
            try
            {

               
                    var TapGestureRecognizerSettings = new TapGestureRecognizer();
                    TapGestureRecognizerSettings.Tapped += async (s, ee) =>
                    {
                        Timeline_Pages.SettingsPage SettingsPage = new Timeline_Pages.SettingsPage();
                        await Navigation.PushAsync(SettingsPage);
                    };

                    var Profile = SQL_Commander.GetProfileCredentialsById(WowonderPhone.Settings.User_id);
                    var device = Resolver.Resolve<IDevice>();
                    var oNetwork = device.Network;
                    var InternetConnection = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;

                    if (InternetConnection)
                    {
                        if (Profile == null)
                        {
                            WowonderPhone.Settings.Avatarimage = "default_profile_6_400x400.png";
                           
                            //AvatarImage.BorderColor = Color.FromHex("#c72508");
                        }
                        else
                        {
                            var GetAvatarAvailability =
                                DependencyService.Get<IPicture>().GetPictureFromDisk(Profile.Avatar, WowonderPhone.Settings.User_id);
                            if (GetAvatarAvailability != "File Dont Exists")
                            {
                                WowonderPhone.Settings.Avatarimage = GetAvatarAvailability;
                                WowonderPhone.Settings.UserFullName = Profile.name;
                            }
                        }
                    }
                    else
                    {
                        if (Profile != null)
                        {
                            var GetAvatarAvailability =
                                DependencyService.Get<IPicture>().GetPictureFromDisk(Profile.Avatar, WowonderPhone.Settings.User_id);
                            if (GetAvatarAvailability != "File Dont Exists")
                            {
                                WowonderPhone.Settings.Avatarimage = GetAvatarAvailability;
                                WowonderPhone.Settings.UserFullName = Profile.name;
                            }
                            else
                            {


                                //AvatarImage.BorderColor = Color.FromHex("#6CDB26");
                                WowonderPhone.Settings.UserFullName = Profile.name;

                            }

                        }
                        else
                        {
                            LoginUserFunctions.GetSessionProfileData(WowonderPhone.Settings.User_id, WowonderPhone.Settings.Session) .ConfigureAwait(false);
                        }
                    }
                
            }
            catch (Exception)
            {

            }
        }
    }
}
