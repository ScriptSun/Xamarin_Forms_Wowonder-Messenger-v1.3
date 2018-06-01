using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WowonderPhone.Classes;
using WowonderPhone.Controls;
using WowonderPhone.Dependencies;
using WowonderPhone.SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WowonderPhone.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public static ObservableCollection<ChatContacts> ChatContactsList = new ObservableCollection<ChatContacts>();
      

        public SearchPage()
        {
            InitializeComponent();

            if (ChatContactsList.Count > 0)
            {
                ImageSearch.IsVisible = false;
                LabelSearch.IsVisible = false;
            }
            else
            {
                ImageSearch.IsVisible = true;
                LabelSearch.IsVisible = true;
            } 
            
            ChatContactListview.ItemsSource = ChatContactsList;
        }

        private void SearchBar_OnSearchButtonPressed(object sender, EventArgs e)
        {
            try
            {
                if (SearchBarCo.Text != "")
                {
                    ChatContactsList.Clear();
                   
                        var ss = SQL_Commander.GetContactSearchList(SearchBarCo.Text);
                        ChatContactListview.ItemsSource = ss;
                    
                }
            }
            catch (Exception)
            {

            }
          

        }

        private async void SearchBarCo_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (SearchBarCo.Text != "")
                {
                    LabelSearch.IsVisible = false;
                    ImageSearch.IsVisible = false;

                    await Task.Factory.StartNew(() =>
                    {
                        if (SearchBarCo.Text != "")
                        {
                            try
                            {
                                ChatContactsList.Clear();
                               
                                    ChatContactsList = SQL_Commander.GetContactSearchList(SearchBarCo.Text);
                               
                            }
                            catch (Exception)
                            {

                            }
                        }
                    });
                   
                }
               
            }
            catch (Exception)
            {
                
            }
        }

        private void ChatContactListview_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                ChatContactListview.SelectedItem = null;
                var user = e.Item as ChatContacts;
                var recipient_id = user.UserID;
                var UserName = user.Username;
                Functions.Messages.Clear();
               
                Navigation.PushAsync(new ChatWindow(UserName, recipient_id, ""));
            }
            catch (Exception)
            {
                
            }
        }

        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
            ChatContactsList.Clear();
        }
    }
}
