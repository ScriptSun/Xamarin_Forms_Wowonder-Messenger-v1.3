using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using WowonderPhone.Languish;
using WowonderPhone.SQLite;
using Xamarin.Forms.Xaml;

namespace WowonderPhone.Pages.Timeline_Pages.SettingsNavPages
{
    [ImplementPropertyChanged]
    public class Ledtems
    {
        public string Checkicon { get; set; }
        public string ColorText { get; set; }
        public string CheckiconColor { get; set; }

    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Notification_Page : ContentPage
    {
        public static ObservableCollection<Ledtems> LedListItems = new ObservableCollection<Ledtems>();

        public Notification_Page()
        {
            InitializeComponent();
            LedListItems.Clear();
            
                //Data.ClearLoginCredentialsList();
                var SettingsNotify = SQL_Commander.GetLoginCredentialsByUserID(Settings.User_id);
                if (SettingsNotify != null)
                {
                    if (SettingsNotify.NotificationLedColor == null)
                    {
                        SettingsNotify.NotificationLedColor = Settings.MainColor;
                        SettingsNotify.NotificationLedColorName = AppResources.Label_Led_Color;


                        LedListItems.Add(new Ledtems()
                        {
                            Checkicon = "\uf111",
                            ColorText = AppResources.Label_Led_Color,
                            CheckiconColor = Settings.MainColor
                        });


                    }
                    else
                    {
                        LedListItems.Add(new Ledtems()
                        {
                            Checkicon = "\uf111",
                            ColorText = AppResources.Label_Led_Color,
                            CheckiconColor = SettingsNotify.NotificationLedColor
                        });


                    }

                    NotifyVibrate.IsToggled = Settings.NotificationVibrate;
                    NotifyPopup.IsToggled = Settings.NotificationPopup;
                    NotifySound.IsToggled = Settings.NotificationSound;
                }

           


            LedList.ItemsSource = LedListItems;
        }

        private void LedList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            LedList.SelectedItem = null;
        }

        private void LedList_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                Navigation.PushAsync(new LedColorsPage());
            }
            catch (Exception)
            {

            }

        }


        private void NotifyVibrate_OnToggled(object sender, ToggledEventArgs e)
        {

        }

        private void NotifySound_OnToggled(object sender, ToggledEventArgs e)
        {

        }

        private void NotifyPopup_OnToggled(object sender, ToggledEventArgs e)
        {

        }

        private void Notification_Page_OnDisappearing(object sender, EventArgs e)
        {
            try
            {
               
                var SettingsNotify = SQL_Commander.GetLoginCredentialsByUserID(Settings.User_id);
                var Coler = LedListItems.First();
                if (SettingsNotify != null)
                {
                    Settings.NotificationPopup = NotifyPopup.IsToggled;
                    SettingsNotify.NotificationPopup = NotifyPopup.IsToggled;
                    Settings.NotificationSound = NotifySound.IsToggled;
                    SettingsNotify.NotificationSound = NotifySound.IsToggled;
                    Settings.NotificationVibrate = NotifyVibrate.IsToggled;
                    SettingsNotify.NotificationVibrate = NotifyVibrate.IsToggled;
                    SettingsNotify.NotificationLedColor = Coler.CheckiconColor;
                    SettingsNotify.NotificationLedColorName = Coler.ColorText;
                    Settings.NotificationLedColor = Coler.CheckiconColor;
                    Settings.NotificationLedColorName = Coler.ColorText;
                    SQL_Commander.UpdateLoginCredentials(SettingsNotify);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
