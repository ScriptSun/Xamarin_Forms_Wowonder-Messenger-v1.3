using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WowonderPhone.Languish;
using WowonderPhone.Pages.Timeline_Pages.SettingsNavPages;
using WowonderPhone.SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;

namespace WowonderPhone.Pages.Timeline_Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public class GeneralItems
        {
            public int ID { get; set; }
            public string Label { get; set; }
            public string Icon { get; set; }
            public string Nav { get; set; }
        }

        public class HelpItems
        {
            public int ID { get; set; }
            public string Label { get; set; }
            public ImageSource Icon { get; set; }
            public string Nav { get; set; }
        }

        public class LogoutItems
        {
            public int ID { get; set; }
            public string Label { get; set; }
            public ImageSource Icon { get; set; }
            public string Nav { get; set; }
        }
        public class EditItems
        {
            public int ID { get; set; }
            public string Label { get; set; }
            public ImageSource Icon { get; set; }
            public string Nav { get; set; }
        }

        public static ObservableCollection<GeneralItems> GeneralListItems = new ObservableCollection<GeneralItems>();
        public static ObservableCollection<HelpItems> HelpListItems = new ObservableCollection<HelpItems>();
        public static ObservableCollection<LogoutItems> LogoutListItems = new ObservableCollection<LogoutItems>();
        public static ObservableCollection<EditItems> EditListItems = new ObservableCollection<EditItems>();


        public SettingsPage()
        {
            InitializeComponent();

            Username.Text = Settings.UserFullName;
            AvatarImage.Source = Settings.Avatarimage;

            var device = Resolver.Resolve<IDevice>();
            var oNetwork = device.Network; // Create Interface to Network-functions
            var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
            if (!xx)
            {
                Onlinelabel.Text = AppResources.Label_Online;
            }
            else
            {
                Onlinelabel.Text = AppResources.Label_Offline;
            }
            


            if (EditListItems.Count <= 0 && HelpListItems.Count <= 0 && LogoutListItems.Count <= 0 &&
                GeneralListItems.Count <= 0)
            {
                EditListItems.Add(new EditItems()
                {
                    Label = AppResources.Label_Edit_Profile,
                });

                

                LogoutListItems.Add(new LogoutItems()
                {
                    Label = AppResources.Label_Logout,
                });

                

                HelpListItems.Add(new HelpItems()
                {
                    Label = AppResources.Label_Help,
                    Icon = "\uf059",
                });

                HelpListItems.Add(new HelpItems()
                {
                    Label = AppResources.Label_Report_TO_us,
                    Icon = "\uf024",
                });

                GeneralListItems.Add(new GeneralItems()
                {
                    Label = AppResources.Label_Notifications,
                    Icon = "\uf0f3",
                });


                GeneralListItems.Add(new GeneralItems()
                {
                    Label = AppResources.Label_Account,
                    Icon = "\uf007",
                });
                GeneralListItems.Add(new GeneralItems()
                {
                    Label = AppResources.Label_Privacy,
                    Icon = "\uf06e",
                });
                GeneralListItems.Add(new GeneralItems()
                {
                    Label = AppResources.Label_Blocked_Users,
                    Icon = "\uf056",
                });
                GeneralListItems.Add(new GeneralItems()
                {
                    Label = AppResources.Label_invite,
                    Icon = "\uf0e0",
                });

            }
            GeneralList.ItemsSource = GeneralListItems;
            EditList.ItemsSource = EditListItems;
            LogoutList.ItemsSource = LogoutListItems;
            HelpList.ItemsSource = HelpListItems;
        }

       

        private void GeneralList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            GeneralList.SelectedItem = null;
        }

        private void HelpList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            HelpList.SelectedItem = null;
        }

        private void LogoutList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            LogoutList.SelectedItem = null;
        }

        private async void GeneralList_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var Item = e.Item as GeneralItems;

            if (Item.Label == AppResources.Label_Blocked_Users)
            {
                await Navigation.PushAsync(new BlockedUsersPage());
            }
            else if (Item.Label == AppResources.Label_Account)
            {
                Account_Page AccountPage = new Account_Page();
                await Navigation.PushAsync(AccountPage);
            }
            else if (Item.Label == AppResources.Label_Privacy)
            {          
                await Navigation.PushAsync( new PrivacyPage());
            }
            else if (Item.Label == AppResources.Label_Notifications)
            {
                Notification_Page NotificationPage = new Notification_Page();
                await Navigation.PushAsync(NotificationPage);
            }
            else if (Item.Label == AppResources.Label_invite)
            {
                InviteFriendsPage InviteFriendsPage = new InviteFriendsPage();
                await Navigation.PushAsync(InviteFriendsPage);
            }
        }
       
        private void EditList_OnItemSelectedList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            EditList.SelectedItem = null;
        }

        private async void EditList_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var Item = e.Item as EditItems;
            
            if (Item.Label == AppResources.Label_Edit_Profile)
            {
                MyProfilePage GeneralPage = new MyProfilePage();
                await Navigation.PushAsync(GeneralPage);
            }
        }

        private  void LogoutList_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var Item = e.Item as LogoutItems;

            if (Item.Label == AppResources.Label_Logout)
            {
                Logout().ConfigureAwait(false);

                SQL_Commander.ClearLoginCredentialsList();


                SQL_Commander.ClearProfileCredentialsList();
                

                SQL_Commander.ClearMessageList();
                
             
                    SQL_Commander.DeletAllChatUsersList();
                
                
                    SQL_Commander.ClearNotifiDBCredentialsList();
                
               
                    SQL_Commander.ClearPrivacyDBCredentialsList();
               
               

                    SQL_Commander.ClearChatUserTable();
                    SQL_Commander.DeletAllChatUsersList();

                    try

                    {
                        WowonderPhone.Settings.ReLogin = true;
                        Navigation.PopAsync();
                        App.GetLoginPage();

                    }
                    catch
                    {
                        Navigation.RemovePage(this);
                        App.GetLoginPage();

                    }

                  
               
            }
        }

        private void HelpList_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var Item = e.Item as HelpItems;
            if(Item.Label== AppResources.Label_Help)
            {
                Navigation.PushAsync(new HelpPage());
            }
           else if (Item.Label == AppResources.Label_Report_TO_us)
            {
                Device.OpenUri(new Uri(Settings.Website+ "/contact-us"));
            }
        }

        public static async Task Logout()
        {
            using (var client = new HttpClient())
            {

                var formContent = new FormUrlEncodedContent(new[]
                {
                   new KeyValuePair<string, string>("user_id", Settings.User_id),
                   new KeyValuePair<string, string>("s", Settings.Session),
               });
                var response = await client.PostAsync(Settings.Website + "/app_api.php?application=phone&type=logout", formContent).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                string apiStatus = data["api_status"].ToString();
                if (apiStatus == "200")
                {

                }
           }
        }
    }
}
