
using System.Collections.ObjectModel;
using PropertyChanged;
using Xamarin.Forms;

namespace WowonderPhone.Classes
{
    [ImplementPropertyChanged]
    public class MessageViewModal
    {
        public string Content { get; set; }
        public string WidthWrap { get; set; }
        public bool Visibilty { get; set; }
        public string Type { get; set; }
        public string Position { get; set; }
        public string CreatedAt { get; set; }
        public string messageID { get; set; }
        public string MediaName { get; set; }
        public string Media { get; set; }
        public string MediaSize { get; set; }
        public string DownloadFileUrl { get; set; }
        public ImageSource UserImage { get; set; }
        public ImageSource ImageMedia { get; set; }
        public string ImageUrl { get; set; }
        public string Percentage { get; set; }
        public string from_id { get; set; }




        public string GoingBackroundBoxColor { get; set; }
        public string CommingBackroundBoxColor  { get; set; }

        public string SliderSoundValue { get; set; }
        public string SliderMaxDuration { get; set; }

        public string ContactNumber { get; set; }
        public string ContactJson { get; set; }
        public string ContactName { get; set; }
        

    }

   // 
}