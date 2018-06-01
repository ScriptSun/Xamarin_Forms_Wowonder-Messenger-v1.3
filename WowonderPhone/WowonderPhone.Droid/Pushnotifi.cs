using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using WowonderPhone.Pages;
using WowonderPhone.Pages.Register_pages;
using Xamarin.Forms;
using TaskStackBuilder = Android.App.TaskStackBuilder;

namespace WowonderPhone.Droid
{
    [Activity(Label = "Second Activity")]
    public class SecondActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            PrivacyPage PrivacyPage = new PrivacyPage();
            NavigationPage ss = new NavigationPage();
            ss.PushAsync(PrivacyPage);
            //App.Current.MainPage.Navigation.PushAsync(new UploudPicPage());
            //App.GetMainPage("second");
        }
    }
}