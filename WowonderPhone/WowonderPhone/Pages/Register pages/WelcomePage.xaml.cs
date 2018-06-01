using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace WowonderPhone.Pages.Register_pages
{
    public partial class WelcomePage : ContentPage
    {
        private bool isPageLoaded = true;

        public WelcomePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

        }

        private void RegisterButtonTapped(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushModalAsync(new RegisterPage());
            }
            catch (Exception)
            {

                Navigation.PushAsync(new RegisterPage());
            }
           
        }

        private void LoginButtonTapped(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushModalAsync(new MainPage());
            }
            catch (Exception)
            {

                Navigation.PushAsync(new MainPage());
            }
           
        }


        private void WelcomePage_OnAppearing(object sender, EventArgs e)
        {
            
        }
    }
}
