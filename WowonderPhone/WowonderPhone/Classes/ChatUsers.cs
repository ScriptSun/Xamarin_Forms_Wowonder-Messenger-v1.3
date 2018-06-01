using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using Xamarin.Forms;

namespace WowonderPhone.Classes
{
    [ImplementPropertyChanged]
    public class ChatUsers
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
        public string ChekeSeen  {get; set;}
        public string LastMessageDateTime { get; set; }
        public string Notifi { get; set; }
        public string OnOficon { get; set; }
        public string profile_picture_Url { get; set; }
        public string TimeLong { get; set; }
        public string Isverifyed { get; set; }

       


    }
}
