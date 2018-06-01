using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Newtonsoft.Json;
using Plugin.Contacts;
using Plugin.Contacts.Abstractions;
using Plugin.Messaging;
using WowonderPhone.Classes;
using WowonderPhone.Controls;
using WowonderPhone.Languish;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WowonderPhone.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InviteFriendsPage : ContentPage
    {
        public InviteFriendsPage()
        {
            InitializeComponent();

            if (ContactsPhonePage.PhoneContactsItemsCollection.Count == 0)
            {
                GetPhoneContacts();
            }
            else
            {
                ActionsListview.ItemsSource = ContactsPhonePage.PhoneContactsItemsCollection;
            }
        }

        public async void GetPhoneContacts()
        {
            try
            {

                try
                {
                    UserDialogs.Instance.ShowLoading("Loading...");
                    var Dictionary = new Dictionary<string, string>();
                    if (await CrossContacts.Current.RequestPermission())
                    {
                        CrossContacts.Current.PreferContactAggregation = false;
                        await Task.Run(() =>
                        {
                            if (CrossContacts.Current.Contacts == null)
                                return;
                            var max = 0;
                            foreach (Contact contact in CrossContacts.Current.Contacts)
                            {
                                foreach (var Number in contact.Phones)
                                {
                                    if (!Dictionary.ContainsKey(contact.FirstName + ' ' + contact.LastName))
                                    {
                                        Dictionary.Add(contact.FirstName + ' ' + contact.LastName, Number.Number);
                                    }


                                    var Cheker = ContactsPhonePage.PhoneContactsItemsCollection.FirstOrDefault(a => a.Name == contact.FirstName + ' ' + contact.LastName);
                                    if (Cheker == null)
                                    {
                                        string Data = JsonConvert.SerializeObject(Dictionary.ToArray().FirstOrDefault(a => a.Key == contact.FirstName + ' ' + contact.LastName));
                                        ContactsPhonePage.PhoneContactsItemsCollection.Add(new ContactsPhonePage.PhoneContacts
                                        {
                                            Name = contact.FirstName + ' ' + contact.LastName,
                                            Phone = Number.Number,
                                            JsonData = Data
                                        });
                                    }

                                    break;
                                }
                            }
                        });
                    }

                    ActionsListview.ItemsSource = ContactsPhonePage.PhoneContactsItemsCollection;
                    UserDialogs.Instance.HideLoading();
                }
                catch
                {
                }
            }
            catch (Exception e)
            {

            }
        }
        private void ActionsListview_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var item = e.Item as ContactsPhonePage.PhoneContacts;

                var smsMessenger = MessagingPlugin.SmsMessenger;
                if (smsMessenger.CanSendSms)
                    smsMessenger.SendSms(item.Phone, Settings.InviteSMSText);
               // Navigation.PopAsync();
                
            }
            catch
            {

            }
        }

        private void ActionsListview_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ActionsListview.SelectedItem = null;
        }

        private void RefreshContacts_Clicked(object sender, EventArgs e)
        {
            ContactsPhonePage.PhoneContactsItemsCollection.Clear();
            GetPhoneContacts();
        }
    }
}
