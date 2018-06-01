
using PropertyChanged;
using Xamarin.Forms;

namespace WowonderPhone.Classes
{
    [ImplementPropertyChanged]
    public class SearchResult
    {
        public int ID { get; set; }
        public string ResultID { get; set; }
        public string ResultType { get; set; }
        public string BigLabel { get; set; }
        public string Name { get; set; }
        public ImageSource profile_picture { get; set; }
        public ImageSource verified { get; set; }
        public ImageSource lastseen { get; set; }
        public string SeenMessageOrNo { get; set; }
        public string Url { get; set; }
        public string TextMessage { get; set; }
        public string MiniLabel { get; set; }
        public string Title { get; set; }
        public string Follow { get; set; }
        public string ResultButtonAvailble { get; set; }
        public string connectivitySystem { get; set; }
        public string ButtonColor { get; set; }
        public string ButtonTextColor { get; set; }
        public string ButtonImage { get; set; }
        public string ThumbnailHeight { get; set; }


        public SearchResult()
        {

        }
    }

}
