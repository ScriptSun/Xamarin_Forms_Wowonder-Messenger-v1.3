using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Media;
using WowonderMobile.Controls;
using WowonderPhone.Controls;
using WowonderPhone.Dependencies;
using WowonderPhone.Languish;
using WowonderPhone.Pages.Timeline_Pages;
using WowonderPhone.SQLite;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using Xamarin.Forms.Xaml;

namespace WowonderPhone.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyProfilePage : ContentPage
    {

        public static string Firstname_E = "";
        public static string LastName_E = "";
        public static string AboutmeEntry_E = "";
        public static string LocationEntery_E = "";
        public static string Username_E = "";
        public static string Facebook_E = "";
        public static string Google_E = "";
        public static string Linkedin_E = "";
        public static string VK_E = "";
        public static string Instagram_E = "";
        public static string Twitter_E = "";
        public static string Youtube_E = "";


        public MyProfilePage()
        {
            InitializeComponent();
            try
            {
                    var DataRow = SQL_Commander.GetProfileCredentialsById(Settings.User_id);
                    AvatarImage.Source = DependencyService.Get<IPicture>().GetPictureFromDisk(DataRow.Avatar, Settings.User_id);
                    CoverImage.Source = DependencyService.Get<IPicture>().GetPictureFromDisk(DataRow.Cover, Settings.User_id);
                    UploadAvatar.BackgroundColor = Color.FromHex(Settings.MainColor);
                    UploadCover.BackgroundColor = Color.FromHex(Settings.MainColor);

                    Firstname.Text = DataRow.First_name;
                    LastName.Text = DataRow.Last_name;
                    AboutmeEntry.Text = DataRow.About;
                    LocationEntery.Text = DataRow.Address;
                    Username.Text = DataRow.name;
                    FacebookEntery.Text = DataRow.Facebook;
                    GoogleEntery.Text = DataRow.Google;
                    LinkedinEntery.Text = DataRow.Linkedin;
                    VkEntery.Text = DataRow.vk;
                    InstagramEntery.Text = DataRow.instagram;
                    TwitterEntery.Text = DataRow.Twitter;
                    YoutubeEntery.Text = DataRow.Youtube;
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    Firstname_E = DataRow.First_name;
                    LastName_E = DataRow.Last_name;
                    AboutmeEntry_E = DataRow.About;
                    LocationEntery_E = DataRow.Address;
                    Username_E = DataRow.name;
                    Facebook_E = DataRow.Facebook;
                    Google_E = DataRow.Google;
                    Linkedin_E = DataRow.Linkedin;
                    VK_E = DataRow.vk;
                    Instagram_E = DataRow.instagram;
                    Twitter_E = DataRow.Twitter;
                    Youtube_E = DataRow.Youtube;

                    var tapGestureRecognizer = new TapGestureRecognizer();
                    var tapGestureRecognizerCover = new TapGestureRecognizer();

                    tapGestureRecognizer.Tapped += (s, ee) =>
                    {
                        DependencyService.Get<IMethods>().OpenImage("Disk", DataRow.Avatar, DataRow.UserID);
                    };
                    tapGestureRecognizerCover.Tapped += (s, ee) =>
                    {
                        DependencyService.Get<IMethods>().OpenImage("Disk", DataRow.Cover, DataRow.UserID);
                    };

                    AvatarImage.GestureRecognizers.Add(tapGestureRecognizer);
                    CoverImage.GestureRecognizers.Add(tapGestureRecognizerCover);

                    LoginUserFunctions.GetSessionProfileData(Settings.User_id, Settings.Session).ConfigureAwait(false);
               
            }
            catch (Exception)
            {
            }

        }

        private async void UploadAvatar_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Oops", "You Cannot pick an image", AppResources.Label_OK);
                    return;
                }
                var file = await CrossMedia.Current.PickPhotoAsync();

                if (file == null)
                    return;

                var device = Resolver.Resolve<IDevice>();
                var oNetwork = device.Network; // Create Interface to Network-functions
                var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                if (xx == true)
                {
                    await DisplayAlert(AppResources.Label_CannotUpload, AppResources.Label_CheckYourInternetConnection, AppResources.Label_OK);
                }
                else
                {
                    var streamReader = new StreamReader(file.GetStream());
                    var bytes = default(byte[]);
                    using (var memstream = new MemoryStream())
                    {
                        streamReader.BaseStream.CopyTo(memstream);
                        bytes = memstream.ToArray();
                    }
                    string MimeTipe = MimeType.GetMimeType(bytes, file.Path);

                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        UploadPhoto(file.GetStream(), file.Path, MimeTipe, "avatar");
                    });

                    AvatarImage.Source = ImageSource.FromFile(file.Path);
                    Settings.Avatarimage = file.Path;
                }
            }
            catch (Exception)
            {

            }


        }

        public async Task<string> UploadPhoto(Stream stream, string image, string Mimetype, string image_type)
        {
            try
            {
                string Imagename = image.Split('/').Last();
                StreamContent scontent = new StreamContent(stream);
                scontent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = Imagename,
                    Name = "image"
                };
                scontent.Headers.ContentType = new MediaTypeHeaderValue(Mimetype);

                var client = new HttpClient();
                var multi = new MultipartFormDataContent();
                var values = new[]
                {
                    new KeyValuePair<string, string>("user_id", Settings.User_id),
                    new KeyValuePair<string, string>("s", Settings.Session),
                     new KeyValuePair<string, string>("image_type",image_type )
                };
                foreach (var keyValuePair in values)
                {
                    multi.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                }
                multi.Add(scontent);
                client.BaseAddress = new Uri(Settings.Website);
                var result = client.PostAsync("/app_api.php?application=phone&type=set_profile_picture", multi).Result;
                string json = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                string apiStatus = data["api_status"].ToString();

                string ImageFile = data["avatar"].ToString();

                if (apiStatus == "200")
                {
                    
                        var DataRow = SQL_Commander.GetProfileCredentialsById(Settings.User_id);
                        if (image_type == "avatar")
                        {
                            DataRow.Avatar = ImageFile;
                        SQL_Commander.UpdateProfileCredentials(DataRow);
                            DependencyService.Get<IPicture>().SavePictureToDisk(ImageFile, Settings.User_id);
                            Settings.Avatarimage = image;

                        }
                        if (image_type == "cover")
                        {
                            DataRow.Cover = ImageFile;
                        SQL_Commander.UpdateProfileCredentials(DataRow);
                            DependencyService.Get<IPicture>().SavePictureToDisk(ImageFile, Settings.User_id);
                            Settings.Coverimage = image;

                        }

                    

                    return "Success";
                }
                else
                {
                    return AppResources.Label_Error;
                }
            }
            catch (Exception e)
            {
                return AppResources.Label_Error;
            }
        }

        private async void UploadCover_OnClicked(object sender, EventArgs e)
        {

            try
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Oops", "You Cannot pick an image", AppResources.Label_OK);
                    return;
                }
                var file = await CrossMedia.Current.PickPhotoAsync();

                if (file == null)
                    return;

                var device = Resolver.Resolve<IDevice>();
                var oNetwork = device.Network; // Create Interface to Network-functions
                var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                if (xx == true)
                {
                    await DisplayAlert("Cannot Upload", AppResources.Label_CheckYourInternetConnection, AppResources.Label_OK);
                }
                else
                {
                    var streamReader = new StreamReader(file.GetStream());
                    var bytes = default(byte[]);
                    using (var memstream = new MemoryStream())
                    {
                        streamReader.BaseStream.CopyTo(memstream);
                        bytes = memstream.ToArray();
                    }
                    string MimeTipe = MimeType.GetMimeType(bytes, file.Path);

                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        UploadPhoto(file.GetStream(), file.Path, MimeTipe, "cover").ConfigureAwait(false);
                    });

                    CoverImage.Source = ImageSource.FromFile(file.Path);
                    Settings.Coverimage = file.Path;
                }
            }
            catch (Exception)
            {

            }
        }

        private async Task<string> UpdateSettings(string Data)
        {
            try
            {

                using (var client = new HttpClient())
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user_id", Settings.User_id),
                        new KeyValuePair<string, string>("type", "profile_settings"),
                        new KeyValuePair<string, string>("s", Settings.Session),
                        new KeyValuePair<string, string>("user_data", Data)
                    });

                    var response =  await client.PostAsync(Settings.Website + "/app_api.php?application=phone&type=u_user_data", formContent).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string apiStatus = data["api_status"].ToString();
                    if (apiStatus == "200")
                    {
                       
                        var DataRow = SQL_Commander.GetProfileCredentialsById(Settings.User_id);
                        DataRow.About = AboutmeEntry.Text;
                        DataRow.Last_name = LastName.Text;
                        DataRow.First_name = Firstname.Text;
                        DataRow.Facebook = FacebookEntery.Text;
                        DataRow.Google = GoogleEntery.Text;
                        DataRow.Linkedin = LinkedinEntery.Text;
                        DataRow.instagram = InstagramEntery.Text;
                        DataRow.vk = VkEntery.Text;
                        DataRow.Youtube = YoutubeEntery.Text;
                        SQL_Commander.UpdateProfileCredentials(DataRow);

                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }


        private void MyProfilePage_OnDisappearing(object sender, EventArgs e)
        {
            try
            {
                if(Firstname.Text == null){ Firstname.Text = ""; }
                if (LastName.Text == null){LastName.Text = "";}
                if (AboutmeEntry.Text == null) { AboutmeEntry.Text = "";}
                if (LocationEntery.Text == null) {LocationEntery.Text = ""; }
               
                if (Firstname.Text != Firstname_E || LastName.Text != LastName_E || AboutmeEntry.Text != AboutmeEntry_E

                    || FacebookEntery.Text != Facebook_E || VkEntery.Text != VK_E || GoogleEntery.Text != Google_E || InstagramEntery.Text != Instagram_E || YoutubeEntery.Text != Youtube_E
                    || TwitterEntery.Text != Twitter_E || LinkedinEntery.Text != Linkedin_E

                    )
                {
                    var dictionary = new Dictionary<string, string>
                    {
                      {"first_name", Firstname.Text},
                      {"last_name", LastName.Text},
                      {"about", AboutmeEntry.Text},
                      {"facebook", FacebookEntery.Text},
                      {"google", GoogleEntery.Text},
                      {"linkedin", LinkedinEntery.Text},
                      {"vk", VkEntery.Text},
                      {"instagram", InstagramEntery.Text},
                      {"twitter", TwitterEntery.Text},
                      {"youtube", YoutubeEntery.Text},
                    };

                    UpdateSettings(JsonConvert.SerializeObject(dictionary)).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
            }
        }

        private void ViwSettingsButton_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage());
        }
    }
}
