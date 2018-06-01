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

using WowonderPhone.Controls;
using WowonderPhone.Dependencies;
using WowonderPhone.SQLite;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using WowonderMobile.Controls;
using WowonderPhone.Languish;

namespace WowonderPhone.Pages.Register_pages
{
    public partial class UploudPicPage : ContentPage
    {
        public UploudPicPage()
        {
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            ProfilePicture.BorderColor = Color.FromHex(Settings.MainColor);
            AnimateIn();
            try
            {
                var device = Resolver.Resolve<IDevice>();
                var oNetwork = device.Network;
                var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                if (xx == true)
                {

                }
                else
                {
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, ee) =>
                    {
                        Upload();
                    };
                    ProfilePicture.GestureRecognizers.Add(tapGestureRecognizer);
                }
            }
            catch (Exception)
            {

            }
        }

        private async void Upload()
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
                var oNetwork = device.Network;
                var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                if (xx == true)
                {
                    await DisplayAlert(AppResources.Label_CannotUpload, AppResources.Label_CheckYourInternetConnection, AppResources.Label_OK);
                }
                else
                {
                    ProfilePicture.Source = ImageSource.FromFile(file.Path);
                    BackImage.Source = ImageSource.FromFile(file.Path);

                    var streamReader = new StreamReader(file.GetStream());
                    var bytes = default(byte[]);
                    using (var memstream = new MemoryStream())
                    {
                        streamReader.BaseStream.CopyTo(memstream);
                        bytes = memstream.ToArray();
                    }
                    string MimeTipe = MimeType.GetMimeType(bytes, file.Path);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Loaderspinner.IsVisible = true;

                    });

                    Uploudbackground(file.GetStream(), file.Path, MimeTipe);
                }
            }
            catch (Exception)
            {


            }

        }

        public Task Task1;
        public static string Headertext { get; set; }
        public async void Uploudbackground(Stream stream, string image, string Mimetype)
        {
            await Task.Run(async () =>
                  UploadPhoto(stream, image, Mimetype, "avatar").ConfigureAwait(false)
               );
            Task.Delay(3000);
        }
        public async Task<string> UploadPhoto(Stream stream, string image, string Mimetype, string image_type)
        {
            try
            {
                Headertext = "sssssss";
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
                    new KeyValuePair<string, string>("image_type", image_type)
                };
                foreach (var keyValuePair in values)
                {
                    multi.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                }
                multi.Add(scontent);
                client.BaseAddress = new Uri(Settings.Website);

                var result = client.PostAsync("/app_api.php?application=phone&type=update_profile_picture", multi).Result;
                string json = result.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                string apiStatus = data["api_status"].ToString();

                string ImageFile = data["avatar"].ToString();

                if (apiStatus == "200")
                {
                  
                        var DataRow = SQL_Commander.GetProfileCredentialsById(Settings.User_id);
                        DataRow.Avatar = ImageFile;
                    SQL_Commander.UpdateProfileCredentials(DataRow);
                   
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Loaderspinner.IsVisible = false;

                    });

                    return null;
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Loaderspinner.IsVisible = false;

                    });
                    return null;
                }
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Loaderspinner.IsVisible = false;

                });
                return null;
            }
        }

        private void NextButton_OnClicked(object sender, EventArgs e)
        {

        }

        private void Ss_OnClicked(object sender, EventArgs e)
        {
            try
            {
                Upload();
            }
            catch (Exception)
            {


            }

        }

        private void NextButtonClicked(object sender, EventArgs e)
        {

            Device.BeginInvokeOnMainThread( async ()  =>
            {
                try
                {
                    

                        var CredentialStatus = SQL_Commander.GetLoginCredentialsStatus();
                        if (CredentialStatus == "Registered")
                        {
                            var Credential = SQL_Commander.GetLoginCredentials("Registered");
                            Credential.Status = "Active";
                        SQL_Commander.UpdateLoginCredentials(Credential);
                            Settings.Session = Credential.Session;
                            Settings.User_id = Credential.UserID;
                            Settings.Username = Credential.Username;
                        }

                    
                    await Navigation.PushAsync(new RegisterFriends());
                }
                catch (Exception)
                {

                    await Navigation.PushModalAsync(new RegisterFriends());
                }
                // Navigation.PushAsync(new WalkThrough_Page2());
                //Navigation.RemovePage(this);
            });


        }

        public async Task AnimateIn()
        {
            await Task.WhenAll(new[] {
                AnimateItem (ProfilePicture, 500),
                AnimateItem (Label1, 600),
               AnimateItem (Label2, 700),
            });
        }

        private async Task AnimateItem(View uiElement, uint duration)
        {
            if (uiElement == null)
            {
                return;
            }

            await Task.WhenAll(new Task[] {
                uiElement.ScaleTo(1.5, duration, Easing.CubicIn),
                uiElement.FadeTo(1, duration/2, Easing.CubicInOut).ContinueWith(
                    _ =>
                    { 
                        // Queing on UI to workaround an issue with Forms 2.1
                        Device.BeginInvokeOnMainThread(() => {
                            uiElement.ScaleTo(1, duration, Easing.CubicOut);
                        });
                    })
            });
        }
        private void ResetAnimation()
        {
            //ProfilePicture.Opacity = 0;
            //Label1.Opacity = 0;
            //Label2.Opacity = 0;
        }

        private void UploudPicPage_OnDisappearing(object sender, EventArgs e)
        {
            ResetAnimation();
        }
    }
}
