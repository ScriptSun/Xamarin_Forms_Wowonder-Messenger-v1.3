using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WowonderPhone.Languish;
using WowonderPhone.SQLite;
using WowonderPhone.SQLite.Tables;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WowonderPhone.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchFilterPage : ContentPage
    {
      
        public SearchFilterPage()
        {
            InitializeComponent();
            this.Title = AppResources.Label_SearchSettings;
            Gender.Text = AppResources.Label_SearchByGender;
            StatusLabel.Text = AppResources.Label_Status;
            ProfileLabel.Text = AppResources.Label_Profile_Picture;
            Save.Text = AppResources.Label_Save;
            Section.Title = AppResources.Label_SearchBoxFilter;

            SearchBy.Items.Add(AppResources.Label_All);
            SearchBy.Items.Add(AppResources.Label_Male);
            SearchBy.Items.Add(AppResources.Label_Female);
            ProfilePicture.Items.Add(AppResources.Label_All);
            ProfilePicture.Items.Add(AppResources.Label_Yes);
            ProfilePicture.Items.Add(AppResources.Label_NO);
            Status.Items.Add(AppResources.Label_All);
            Status.Items.Add(AppResources.Label_Online);
            Status.Items.Add(AppResources.Label_Offline);


           
                var FilterResult = SQL_Commander.GetSearchFilterById(Settings.User_id);
                if (FilterResult == null)
                {
                    SearchFilterDB NewSettingsFiltter = new SearchFilterDB();
                    NewSettingsFiltter.UserID = Settings.User_id;
                    NewSettingsFiltter.Gender = 0;
                    NewSettingsFiltter.ProfilePicture = 0;
                    NewSettingsFiltter.Status = 0;
                SQL_Commander.InsertSearchFilter(NewSettingsFiltter);

                    ProfilePicture.SelectedIndex = 0;
                    SearchBy.SelectedIndex = 0;
                    Status.SelectedIndex = 0;
                }
                else
                {
                    ProfilePicture.SelectedIndex = FilterResult.ProfilePicture;
                    SearchBy.SelectedIndex = FilterResult.Gender;
                    Status.SelectedIndex = FilterResult.Status;
                }
            
        }

        private void Save_OnClicked(object sender, EventArgs e)
        {
            
                var FilterResult = SQL_Commander.GetSearchFilterById(Settings.User_id);

                FilterResult.ProfilePicture = ProfilePicture.SelectedIndex ;
                FilterResult.Gender = SearchBy.SelectedIndex;
                FilterResult.Status = Status.SelectedIndex;
            SQL_Commander.UpdateSearchFilter(FilterResult);
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                if (SearchBy.SelectedIndex == 0)
                {
                    Settings.SearchByGenderValue = "";
                }
                if (SearchBy.SelectedIndex == 1)
                {
                    Settings.SearchByGenderValue = "male";
                }
                if (SearchBy.SelectedIndex == 2)
                {
                    Settings.SearchByGenderValue = "female";
                }
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                if (Status.SelectedIndex == 0)
                {
                    Settings.SearchByStatusValue = "";
                }
                if (Status.SelectedIndex == 1)
                {
                    Settings.SearchByStatusValue = "on";
                }
                if (Status.SelectedIndex == 2)
                {
                    Settings.SearchByStatusValue = "off";
                }
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                if (ProfilePicture.SelectedIndex == 0)
                {
                    Settings.SearchByProfilePictureValue = "";
                }
                if (ProfilePicture.SelectedIndex == 1)
                {
                    Settings.SearchByProfilePictureValue = "yes";
                }
                if (ProfilePicture.SelectedIndex == 2)
                {
                    Settings.SearchByProfilePictureValue = "no";
                }
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
               Save.Text = AppResources.Label_Saved; ;
            }
        
    }
}
