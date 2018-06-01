using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using Xamarin.Forms;

namespace WowonderPhone.Classes
{
    [ImplementPropertyChanged]
    public class ChatContacts
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public ImageSource profile_picture { get; set; }
        public ImageSource verified { get; set; }
        public ImageSource lastseen { get; set; }
        public string SeenMessageOrNo { get; set; }
        public string Url { get; set; }
        public string TextMessage { get; set; }
        public string LastMessageDateTime { get; set; }
        public string Title { get; set; }
        public string Platform { get; set; }

        public ChatContacts()
        {

        }
    }

    public class GroupedChatList : ObservableCollection<ChatContacts>
    {

        public string Status { get; set; }
        public string StatusName { get; set; } //will be used for jump lists
        public ObservableCollection<ChatContacts> Listchatcontact { get; set; }

         public class ChatContacts
        {

        }
    }

}
