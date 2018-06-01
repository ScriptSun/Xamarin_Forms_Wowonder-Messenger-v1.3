using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WowonderPhone.Languish;
using Xamarin.Forms;

namespace WowonderPhone.Pages
{
    public partial class HelpPage : ContentPage
    {
        public HelpPage()
        {
            InitializeComponent();
            //WebView1.Source = Settings.Website + "/terms/about-us";
            SectionAbout.Title = AppResources.Label_About+" " + Settings.ApplicationName;
            CopyrightText.Text = Settings.CopyrightText;
            PrivacyPolicyText.Text = Settings.PrivacyPolicyText;
            AboutText.Text = Settings.AboutText;
          
        }

        private void WebView1_OnNavigating(object sender, WebNavigatingEventArgs e)
        {
           
        }
    }
}
