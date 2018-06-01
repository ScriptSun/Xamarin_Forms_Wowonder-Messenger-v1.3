using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using WowonderPhone.Languish;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WowonderPhone.Pages.Timeline_Pages.SettingsNavPages
{
    [ImplementPropertyChanged]
    public class LedColorstems
    {
        public string Checkicon { get; set; }
        public string ColorText { get; set; }
        public string CheckiconColor { get; set; }

    }
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LedColorsPage : ContentPage
    {
        public static ObservableCollection<LedColorstems> LedColorsListItems = new ObservableCollection<LedColorstems>();

        public LedColorsPage()
        {
            InitializeComponent();

            
            

            Addtolistcolors();
        }

        private void LedList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            LedList.SelectedItem = null;
        }

        public void Addtolistcolors()
        {
            try
            {
                LedColorsListItems.Add(new LedColorstems()
                {
                    Checkicon = "\uf192",
                    ColorText = AppResources.Label_Default,
                    CheckiconColor = Settings.MainColor
                });

                LedColorsListItems.Add(new LedColorstems()
                {
                    Checkicon = "\uf192",
                    ColorText = AppResources.Label_Green,
                    CheckiconColor = "#00FF00"
                });
                LedColorsListItems.Add(new LedColorstems()
                {
                    Checkicon = "\uf192",
                    ColorText = AppResources.Label_Blue,
                    CheckiconColor = "#0000FF"
                });

                LedColorsListItems.Add(new LedColorstems()
                {
                    Checkicon = "\uf192",
                    ColorText = AppResources.Label_Magenta,
                    CheckiconColor = "#FF00FF"
                });
                LedColorsListItems.Add(new LedColorstems()
                {
                    Checkicon = "\uf192",
                    ColorText = AppResources.Label_Orange,
                    CheckiconColor = "#ffa500"
                });
                LedColorsListItems.Add(new LedColorstems()
                {
                    Checkicon = "\uf192",
                    ColorText = AppResources.Label_Silver,
                    CheckiconColor = "#C0C0C0"
                });
                LedColorsListItems.Add(new LedColorstems()
                {
                    Checkicon = "\uf192",
                    ColorText = AppResources.Label_Yellow,
                    CheckiconColor = "#FFFF00"
                });
                LedColorsListItems.Add(new LedColorstems()
                {
                    Checkicon = "\uf192",
                    ColorText = AppResources.Label_Maroon,
                    CheckiconColor = "#800000"
                });
                LedColorsListItems.Add(new LedColorstems()
                {
                    Checkicon = "\uf192",
                    ColorText = AppResources.Label_Purple,
                    CheckiconColor = "#800080"
                });
                LedColorsListItems.Add(new LedColorstems()
                {
                    Checkicon = "\uf192",
                    ColorText = AppResources.Label_Red,
                    CheckiconColor = "#FF0000"
                });
            }
            catch (Exception)
            {
                
            }

            LedList.ItemsSource = LedColorsListItems;
        }
        private void LedList_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var Item = e.Item as LedColorstems;
                var SettingsLedColor = Notification_Page.LedListItems.First();
                SettingsLedColor.CheckiconColor = Item.CheckiconColor;
                Navigation.PopAsync(true);
            }
            catch (Exception)
            {

            }

        }

        private void LedColorsPage_OnDisappearing(object sender, EventArgs e)
        {
            LedColorsListItems.Clear();
        }
    }
}
