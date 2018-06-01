using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Plugin.Contacts;
using Plugin.Contacts.Abstractions;
using PropertyChanged;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using WowonderPhone.Controls;
using WowonderPhone.Classes;
using WowonderPhone.Languish;
using System.Net.Http;

namespace WowonderPhone.Pages
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactsPhonePage : ContentPage
    {
        [ImplementPropertyChanged]
        public class PhoneContacts
        {
            public int ID { get; set; }
            public string Phone { get; set; }
            public string Name { get; set; }
            public string JsonData { get; set; }
            
        }

        public static ObservableCollection<PhoneContacts> PhoneContactsItemsCollection = new ObservableCollection<PhoneContacts>();

        private ChatWindow CHATWINDOW;
        public ContactsPhonePage(ChatWindow Page)
        {
           
            InitializeComponent();
            CHATWINDOW = Page;

            if (PhoneContactsItemsCollection.Count == 0)
            {
                GetPhoneContacts();
            }
            else
            {
                ActionsListview.ItemsSource = PhoneContactsItemsCollection;
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
                                   

                                    var Cheker = PhoneContactsItemsCollection.FirstOrDefault(a => a.Name == contact.FirstName + ' ' + contact.LastName);
                                    if (Cheker == null)
                                    {
                                        string Data = JsonConvert.SerializeObject(Dictionary.ToArray().FirstOrDefault(a => a.Key == contact.FirstName + ' ' + contact.LastName));
                                        PhoneContactsItemsCollection.Add(new PhoneContacts
                                        {
                                            Name = contact.FirstName + ' ' + contact.LastName,Phone = Number.Number ,JsonData = Data

                                        });
                                    }
                                    
                                    break;
                                }
                            }
                        });
                    }

                    ActionsListview.ItemsSource = PhoneContactsItemsCollection;
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
            try { 
            var item = e.Item as PhoneContacts;
            string nameContact= item.Name;
            if (item.Name.Length > 14)
            {
                nameContact = item.Name.Substring(0, item.Name.Length - 2);
            }
            if (item.Name.Length > 17)
            {
                nameContact =  item.Name.Substring(0, item.Name.Length - 6);
            }
            if (item.Name.Length > 20)
            {
                nameContact =  item.Name.Substring(0, item.Name.Length - 8);
            }
            if (item.Name.Length > 23)
            {
                nameContact = item.Name.Substring(0, item.Name.Length - 11);
            }
            if (item.Name.Length > 26)
            {
                nameContact =  item.Name.Substring(0, item.Name.Length - 14);
            }
            if (item.Name.Length > 28)
            {
                nameContact = item.Name.Substring(0, item.Name.Length - item.Name.Length / 2);
            }

            Functions.Messages.Add(new MessageViewModal
            {
                Content = nameContact,
                Type = "right_contact",
                messageID = CHATWINDOW.time2,
                CreatedAt = AppResources.Label_Uploading,
                ImageUrl = "UserContact.png",
                MediaName = AppResources.Label_Uploading,
                GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color,
                ContactNumber=item.Phone,
                ContactJson=item.JsonData
            });

            //Functions.Messages.Add(new MessageViewModal
            //{
            //    Content = nameContact,
            //    Type = "left_contact",
            //    messageID = CHATWINDOW.time2,
            //    CreatedAt = AppResources.Label_Uploading,
            //    ImageUrl = "UserContact.png",
            //    MediaName = AppResources.Label_Uploading,
            //    CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color,
            //    ContactNumber = item.Phone,
            //    ContactJson = item.JsonData,
            //    UserImage = "UserContact.png",
            //    Position = "left"
            //});

             Task.Factory.StartNew(() =>
             {
                CHATWINDOW.SendMessageTask(item.JsonData, "1","", "").ConfigureAwait(false);
             });

             Navigation.PopAsync();
             CHATWINDOW.MoveToLastMessage();
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
            PhoneContactsItemsCollection.Clear();
            GetPhoneContacts();
        }

    }

    
}
