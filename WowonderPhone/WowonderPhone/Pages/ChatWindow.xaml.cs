using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;
using WowonderPhone.Classes;
using WowonderPhone.Controls;
using WowonderPhone.Dependencies;
using Xamarin.Forms;
 
using WowonderMobile.Controls;
using WowonderPhone.Languish;
using WowonderPhone.Pages.CustomCells;
using WowonderPhone.Pages.Tabs;
using WowonderPhone.SQLite;
using WowonderPhone.SQLite.Tables;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using TextChangedEventArgs = Xamarin.Forms.TextChangedEventArgs;
using Acr.UserDialogs;
using Xamarin.Forms.Xaml;

namespace WowonderPhone.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatWindow : ContentPage
    {
        public static string Status = "";
        public static string Recipient_id;
        public static string Username;
        private string TaskWork = "";
        public static string TaskHandle = "Working";
        public static Int32 unixTimestamp = (Int32) (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        public string time2 = unixTimestamp.ToString();
        public bool Loader = true;

        public IEnumerable<object> ChatList { get; private set; }

        public ChatWindow(string username, string userid,string Referencefrom)
        {
           
            Recipient_id = userid;
            try
            {

            InitializeComponent();
            this.Title = username;
            Username = username;
            Recourdgif.Source= Settings.MW_Recourd_ImageButton;
            RecourdSecoundsLabel.Text = AppResources.Label_Recourd_Start;
            Wo_API.recipient_id = Recipient_id;

            this.Title = username;
            MessagesListView.ItemsSource = Functions.Messages;
           

             RecourdButtonClicks();
             ChatActivityTab.NotifiStoper = Recipient_id;

            if (Referencefrom == "ChatActivity")
            {
                EmptyLabel2.Text = AppResources.Label_Loading_Messages_from_server;
            }

             EmtyChat.IsVisible = true;
             
             MessageWorker(Recipient_id).ConfigureAwait(false);
             TabClicks();
            }
            catch (Exception e)
            {
                
            }



        }

        public void AddStickersFunction(string num)
        {
         try
            { 
             Gifts.IsVisible = false;

            Functions.Messages.Add(new MessageViewModal
            {
                Content = Textbox.Text,
                Type = "right_sticker",
                messageID = time2,
                CreatedAt = AppResources.Label_Uploading,
                ImageUrl = Stickers.GetStickerImage(num),
                ImageMedia = Stickers.GetStickerImage(num),
            });

                Task.Factory.StartNew(() =>
                {
                    SendMessageTask("", "", Stickers.GetStickerImage(num), "sticker" + num).ConfigureAwait(false);
                });
               
              var lastItem = MessagesListView.ItemsSource.OfType<object>().Last();
              MessagesListView.ScrollTo(lastItem, ScrollToPosition.MakeVisible, true);
            }
            catch
            {

            }
        }

        public void TabClicks()
        {
            var st1 = new TapGestureRecognizer();
            var st2 = new TapGestureRecognizer();
            var st3 = new TapGestureRecognizer();
            var st4 = new TapGestureRecognizer();
            var st5 = new TapGestureRecognizer();
            var st6 = new TapGestureRecognizer();
            var st7 = new TapGestureRecognizer();
            var st8 = new TapGestureRecognizer();
            var st9 = new TapGestureRecognizer();
            var st10 = new TapGestureRecognizer();
            var st11 = new TapGestureRecognizer();
            var st12 = new TapGestureRecognizer();
            var st13 = new TapGestureRecognizer();
            var st14 = new TapGestureRecognizer();
            var st15 = new TapGestureRecognizer();
            var st16 = new TapGestureRecognizer();
            var st17 = new TapGestureRecognizer();
            var st18 = new TapGestureRecognizer();
            var st19 = new TapGestureRecognizer();
            var st20 = new TapGestureRecognizer();
            var st21 = new TapGestureRecognizer();
            var st22 = new TapGestureRecognizer();
            var st23 = new TapGestureRecognizer();
            var st24 = new TapGestureRecognizer();
            var st25 = new TapGestureRecognizer();
            var st26 = new TapGestureRecognizer();
            var st27 = new TapGestureRecognizer();
            var st28 = new TapGestureRecognizer();
            var st29 = new TapGestureRecognizer();
            var st30 = new TapGestureRecognizer();
            var st31 = new TapGestureRecognizer();
            var st32 = new TapGestureRecognizer();
            var st33 = new TapGestureRecognizer();
            var st34 = new TapGestureRecognizer();
            var st35 = new TapGestureRecognizer();
            var st36 = new TapGestureRecognizer();
            var st37 = new TapGestureRecognizer();
            var st38 = new TapGestureRecognizer();
            var st39 = new TapGestureRecognizer();
            var st40 = new TapGestureRecognizer();

            st1.Tapped += (s, ee) =>{AddStickersFunction("1");};
            st2.Tapped += (s, ee) => { AddStickersFunction("2"); };
            st3.Tapped += (s, ee) => { AddStickersFunction("3"); };
            st4.Tapped += (s, ee) => { AddStickersFunction("4"); };
            st5.Tapped += (s, ee) => { AddStickersFunction("5"); };
            st6.Tapped += (s, ee) => { AddStickersFunction("6"); };
            st7.Tapped += (s, ee) => { AddStickersFunction("7"); };
            st8.Tapped += (s, ee) => { AddStickersFunction("8"); };
            st9.Tapped += (s, ee) => { AddStickersFunction("9"); };
            st10.Tapped += (s, ee) => { AddStickersFunction("10"); };
            st11.Tapped += (s, ee) => { AddStickersFunction("11"); };
            st12.Tapped += (s, ee) => { AddStickersFunction("12"); };
            st13.Tapped += (s, ee) => { AddStickersFunction("13"); };
            st14.Tapped += (s, ee) => { AddStickersFunction("14"); };
            st15.Tapped += (s, ee) => { AddStickersFunction("15"); };
            st16.Tapped += (s, ee) => { AddStickersFunction("16"); };
            st17.Tapped += (s, ee) => { AddStickersFunction("17"); };
            st18.Tapped += (s, ee) => { AddStickersFunction("18"); };
            st19.Tapped += (s, ee) => { AddStickersFunction("19"); };
            st20.Tapped += (s, ee) => { AddStickersFunction("20"); };
            st21.Tapped += (s, ee) => { AddStickersFunction("21"); };
            st22.Tapped += (s, ee) => { AddStickersFunction("22"); };
            st23.Tapped += (s, ee) => { AddStickersFunction("23"); };
            st24.Tapped += (s, ee) => { AddStickersFunction("24"); };
            st25.Tapped += (s, ee) => { AddStickersFunction("25"); };
            st26.Tapped += (s, ee) => { AddStickersFunction("26"); };
            st27.Tapped += (s, ee) => { AddStickersFunction("27"); };
            st28.Tapped += (s, ee) => { AddStickersFunction("28"); };
            st29.Tapped += (s, ee) => { AddStickersFunction("29"); };
            st30.Tapped += (s, ee) => { AddStickersFunction("30"); };
            st31.Tapped += (s, ee) => { AddStickersFunction("31"); };
            st32.Tapped += (s, ee) => { AddStickersFunction("32"); };
            st33.Tapped += (s, ee) => { AddStickersFunction("33"); };
            st34.Tapped += (s, ee) => { AddStickersFunction("34"); };
            st35.Tapped += (s, ee) => { AddStickersFunction("35"); };
            st36.Tapped += (s, ee) => { AddStickersFunction("36"); };
            st37.Tapped += (s, ee) => { AddStickersFunction("37"); };
            st38.Tapped += (s, ee) => { AddStickersFunction("38"); };
            st39.Tapped += (s, ee) => { AddStickersFunction("39"); };
            st40.Tapped += (s, ee) => { AddStickersFunction("40"); };

           
            Sticker1.GestureRecognizers.Add(st1);
            Sticker2.GestureRecognizers.Add(st2);
            Sticker3.GestureRecognizers.Add(st3);
            Sticker4.GestureRecognizers.Add(st4);
            Sticker5.GestureRecognizers.Add(st5);
            Sticker6.GestureRecognizers.Add(st6);
            Sticker7.GestureRecognizers.Add(st7);
            Sticker8.GestureRecognizers.Add(st8);
            Sticker9.GestureRecognizers.Add(st9);
            Sticker10.GestureRecognizers.Add(st10);
            Sticker11.GestureRecognizers.Add(st11);
            Sticker12.GestureRecognizers.Add(st12);
            Sticker13.GestureRecognizers.Add(st13);
            Sticker14.GestureRecognizers.Add(st14);
            Sticker15.GestureRecognizers.Add(st15);
            Sticker16.GestureRecognizers.Add(st16);
            Sticker17.GestureRecognizers.Add(st17);
            Sticker18.GestureRecognizers.Add(st18);
            Sticker19.GestureRecognizers.Add(st19);
            Sticker20.GestureRecognizers.Add(st20);
            Sticker21.GestureRecognizers.Add(st21);
            Sticker22.GestureRecognizers.Add(st22);
            Sticker23.GestureRecognizers.Add(st23);
            Sticker24.GestureRecognizers.Add(st24);
            Sticker25.GestureRecognizers.Add(st25);
            Sticker26.GestureRecognizers.Add(st26);
            Sticker27.GestureRecognizers.Add(st27);
            Sticker28.GestureRecognizers.Add(st28);
            Sticker29.GestureRecognizers.Add(st29);
            Sticker30.GestureRecognizers.Add(st30);
            Sticker31.GestureRecognizers.Add(st31);
            Sticker32.GestureRecognizers.Add(st32);
            Sticker33.GestureRecognizers.Add(st33);
            Sticker34.GestureRecognizers.Add(st34);
            Sticker35.GestureRecognizers.Add(st35);
            Sticker36.GestureRecognizers.Add(st36);
            Sticker37.GestureRecognizers.Add(st37);
            Sticker38.GestureRecognizers.Add(st38);
            Sticker39.GestureRecognizers.Add(st39);
            Sticker40.GestureRecognizers.Add(st40);



            var PhotoL = new TapGestureRecognizer();
            var VideoL = new TapGestureRecognizer();
            var ContactL = new TapGestureRecognizer();
            var StickerL = new TapGestureRecognizer();

            PhotoL.Tapped += TakePicture_OnClicked;
            VideoL.Tapped += TakeVideoButton_OnClicked;
            ContactL.Tapped += TakeContactButton_OnClicked;
            StickerL.Tapped += TakeGiftButton_OnClicked;

            Photolabel.GestureRecognizers.Add(PhotoL);
            Videolabel.GestureRecognizers.Add(VideoL);
            Contactlabel.GestureRecognizers.Add(ContactL);
            Stickerlabel.GestureRecognizers.Add(StickerL);
        }

        public void RecourdButtonClicks()
        {
            var tapGestureRecognizerCover = new TapGestureRecognizer();
            tapGestureRecognizerCover.Tapped += async (s, ee) =>
            {

                try
                {
                    if (Status == "Started")
                    {
                        DependencyService.Get<ISoundRecord>().RecordingFunction("Stop", Recipient_id);
                        Status = "Stoped";
                        Recourdgif.Source = Settings.MW_Recourd_ImageButton;
                        RecourdSecoundsLabel.Text = AppResources.Label_Recourd_Start;
                        DependencyService.Get<ISoundRecord>().SoundPlay("served", "", "", "Play", "0");
                        var answer = await DisplayAlert(AppResources.Label_SoundRecord, AppResources.Label_WouldYouLikeToSendYourSound, AppResources.Label_Yes, AppResources.Label_NO);

                        if (answer)
                        {
                            var device = Resolver.Resolve<IDevice>();
                            var oNetwork = device.Network; // Create Interface to Network-functions
                            var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                            if (xx == true)
                            {
                                await DisplayAlert(AppResources.Label_CannotSend, AppResources.Label_CheckYourInternetConnection, AppResources.Label_OK);
                            
                                DependencyService.Get<ISoundRecord>().DeleteRecordedSoundPath(Recipient_id);
                            }
                            else
                            {
                                var SoundPath = DependencyService.Get<ISoundRecord>().GetRecordedSoundPath(Recipient_id);
                                var SoundStream = DependencyService.Get<ISoundRecord>().GetRecordedSound(Recipient_id);
                                unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                                time2 = unixTimestamp.ToString();
                                var time = DateTime.Now.ToString("hh:mm");
								if (SoundStream != null)
								{
									Functions.Messages.Add(new MessageViewModal
									{
										//Content = Textbox.Text,
										Type = "right_audio",
										MediaName = AppResources.Label_Uploading,
										messageID = time2,
										CreatedAt = time,
										Media = SoundPath,
										DownloadFileUrl = SoundPath,
										ImageMedia = Settings.MS_UploadWhiteicon,
										Position = "right",
										GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color

									});

									var lastItem = MessagesListView.ItemsSource.OfType<object>().Last();
									MessagesListView.ScrollTo(lastItem, ScrollToPosition.MakeVisible, true);
									DependencyService.Get<IMethods>().UploudAttachment(SoundStream, SoundPath, Settings.User_id, Recipient_id, Settings.Session, Textbox.Text, time2);
								}
                                
                            }

                        }
                        else
                        {
                            DependencyService.Get<ISoundRecord>().DeleteRecordedSoundPath(Recipient_id);
                        }
                    }

                    else
                    {
                        DependencyService.Get<ISoundRecord>().SoundPlay("served", "", "", "Play", "0");
                        RecourdSecoundsLabel.Text = AppResources.Label_Recording;
                        Recourdgif.Source = Settings.MW_Recourd_StopImageButton;
                        Status = "Started";
                        await Task.Delay(1000);
                        DependencyService.Get<ISoundRecord>().RecordingFunction("Start", Recipient_id);
                    }
                }
                catch 
                {

                }
            };    
            Recourdgif.GestureRecognizers.Add(tapGestureRecognizerCover);
        }

        private void MyListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            MessagesListView.SelectedItem = null;
          
            MessageViewModal item = (MessageViewModal) e.Item;

            if (item.Position == "left")
            {
                if (item.Type == "left_video")
                {
                    var Videopath = item.DownloadFileUrl;
                    if (item.MediaName == AppResources.Label_DownloadVideo)
                    {
                        var device = Resolver.Resolve<IDevice>();
                        var oNetwork = device.Network; // Create Interface to Network-functions
                        var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                        if (xx == true)
                        {
                            DisplayAlert(AppResources.Label_CannotDownload, AppResources.Label_CheckYourInternetConnection, AppResources.Label_OK);
                        }
                        else
                        {
                            item.ImageMedia = Settings.MS_DownloadingWhiteicon;
                            item.MediaName = AppResources.Label_Downloading;
                            DependencyService.Get<IVideo>().SaveVideoToGalery(item.Media, item.from_id, item.messageID);
                        }
                    }
                    else if (item.MediaName == AppResources.Label_VideoFile)
                    {
                        if (Videopath != null)
                        {
                            DependencyService.Get<IVideo>().VedioPlay(Videopath);
                        }
                    }
                   
                }
                if (item.Type == "left_audio")
                {
                    var soundpath = item.DownloadFileUrl;
                    if (item.MediaName == AppResources.Label_DownloadSound)
                    {
                        var device = Resolver.Resolve<IDevice>();
                        var oNetwork = device.Network; // Create Interface to Network-functions
                        var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                        if (xx == true)
                        {
                            DisplayAlert(AppResources.Label_CannotDownload, AppResources.Label_CheckYourInternetConnection, AppResources.Label_OK);
                        }
                        else
                        {
                            item.ImageMedia = Settings.MS_DownloadingWhiteicon;
                            item.MediaName = AppResources.Label_Downloading;
                            DependencyService.Get<ISoundRecord>()
                                .SaveAudioToDisk(item.Media, item.from_id, item.messageID);
                        }
                    }
                    else if(item.MediaName == AppResources.Label_SoundFile)
                    {
                        if (soundpath != null)
                        {
                            item.MediaName = AppResources.Label_Playing;
                            item.ImageMedia = Settings.MS_PauseBlackicon;
                            DependencyService.Get<ISoundRecord>().SoundPlay(soundpath, item.messageID ,item.Type ,"Play", item.SliderSoundValue);
                        }
                    }
                    else if (item.MediaName == AppResources.Label_Playing)
                    {
                        DependencyService.Get<ISoundRecord>().SoundPlay(soundpath, item.messageID, item.Type, "Pause" , item.SliderSoundValue);
                        item.MediaName = AppResources.Label_SoundPause;
                        item.ImageMedia = Settings.MS_PlayBlackicon;
                    }
                    else if (item.MediaName == AppResources.Label_SoundPause)
                    {
                        DependencyService.Get<ISoundRecord>().SoundPlay(soundpath, item.messageID, item.Type, "PauseAfterplay" , item.SliderSoundValue);
                        item.MediaName = AppResources.Label_Playing;
                        item.ImageMedia = Settings.MS_PauseBlackicon;
                    }

                }
                if (item.Type == "left_image")
                {
                    if (item.ImageMedia != null)
                    {
                        var ImageMediaFile = ImageSource.FromFile(DependencyService.Get<IPicture>().GetPictureFromGalery(item.ImageUrl));
                        var ss = Functions.Messages.Where(a => a.ImageUrl == item.ImageUrl).FirstOrDefault();
                        if (ss.ImageMedia.ToString().Length != ImageMediaFile.ToString().Length)
                        {
                            ss.ImageMedia = ImageMediaFile;
                        }
                        if (ImageMediaFile != null)
                        {
                            DependencyService.Get<IMethods>().OpenImage("Galary", item.ImageUrl, "0");
                        }
                    }
                }
                if (item.Type == "left_contact")
                {
                    DependencyService.Get<IMethods>().SaveContactName(item.ContactNumber, item.Content);
                }
                    
            }
            else
            {
                if (item.Type == "right_video")
                {
                    var Videopath = item.DownloadFileUrl;
                    if (item.MediaName == AppResources.Label_DownloadVideo)
                    {
                        var device = Resolver.Resolve<IDevice>();
                        var oNetwork = device.Network; // Create Interface to Network-functions
                        var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                        if (xx == true)
                        {
                            DisplayAlert(AppResources.Label_CannotDownload, AppResources.Label_CheckYourInternetConnection, AppResources.Label_OK);
                        }
                        else
                        {
                            item.ImageMedia = Settings.MS_DownloadingWhiteicon;
                            item.MediaName = AppResources.Label_Downloading;
                            DependencyService.Get<IVideo>().SaveVideoToGalery(item.Media, item.from_id, item.messageID);
                        }
                    }
                    else if (item.MediaName == AppResources.Label_VideoFile)
                    {
                        if (Videopath != null)
                        {
                            DependencyService.Get<IVideo>().VedioPlay(Videopath);
                        }
                    }
                }
                if (item.Type == "//right_audio")
                {
                    var soundpath = item.DownloadFileUrl;
                    if (item.MediaName == AppResources.Label_DownloadSound)
                    {
                        var device = Resolver.Resolve<IDevice>();
                        var oNetwork = device.Network; // Create Interface to Network-functions
                        var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                        if (xx == true)
                        {
                            DisplayAlert(AppResources.Label_CannotDownload, AppResources.Label_CheckYourInternetConnection, AppResources.Label_OK);
                        }
                        else
                        {
                            item.ImageMedia = Settings.MS_DownloadingWhiteicon;
                            item.MediaName = AppResources.Label_Downloading;
                            DependencyService.Get<ISoundRecord>().SaveAudioToDisk(item.Media, item.from_id, item.messageID);
                        }

                       
                    }
                    else if (item.MediaName == AppResources.Label_SoundFile)
                    {
                        if (soundpath != null)
                        {
                            item.MediaName = AppResources.Label_Playing;
                            item.ImageMedia = Settings.MS_PauseWhiteicon;
                            DependencyService.Get<ISoundRecord>().SoundPlay(soundpath, item.messageID ,item.Type , "Play", item.SliderSoundValue);
                        }
                    }               
                    else if (item.MediaName == AppResources.Label_Playing)
                    {
                        DependencyService.Get<ISoundRecord>().SoundPlay(soundpath, item.messageID, item.Type, "Pause", item.SliderSoundValue);
                        item.MediaName = AppResources.Label_SoundFile;
                        item.ImageMedia = Settings.MS_PlayWhiteicon;
                    }
                   

                }
                if (item.Type == "right_image")
                {
                    if (item.ImageMedia != null)
                    {
                      
                      DependencyService.Get<IMethods>().OpenImage("Selected", item.DownloadFileUrl, "0");
                        
                    }
                }  
            }
               

            var Url =  Functions.IsUrlValid(item.Content);
            if (Url != "")
            {
                //DependencyService.Get<IMethods>().OpenWebsiteUrl(Url);
				var URI = new Uri(Url);
				Device.OpenUri(URI);
            }
          

        }

        private void MyListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            MessagesListView.SelectedItem = null;

        }

        private void MessagesListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
           

        }

        private void MessagesListView_OnFocused(object sender, FocusEventArgs e)
        {

        }

        private async void MenuItem_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserProfilePage(Recipient_id));
        }

        public async Task SendMessageTask(string Text , string Contact,string sticker,string stickerID)
        {
            try
            {
                var respondArray = await Wo_API.Send_User_Message(Text, Contact, sticker, stickerID);
                if (respondArray != null)
                {
                    Functions.UpdatelastIdMessage(respondArray, sticker);
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e); 
            }
        }

        public void MoveToLastMessage()
        {
            var lastItem = MessagesListView.ItemsSource.OfType<object>().Last();
            MessagesListView.ScrollTo(lastItem, ScrollToPosition.MakeVisible, true);
            if (Options.IsVisible)
            {
                Options.IsVisible = false;
            }
            if (EmtyChat.IsVisible)
            {
                EmtyChat.IsVisible = false;
            }
        }


        private void SendButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (Textbox.Text != "")
                {
                    Task.Delay(1000);
                    Loader = false;
                    unixTimestamp = (Int32) (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    time2 = unixTimestamp.ToString();
                    var time = DateTime.Now.ToString("hh:mm");

                    Functions.Messages.Add(new MessageViewModal
                    {
                        Content = Textbox.Text,
                        Type = "right_text",
                        messageID = time2,
                        CreatedAt = "",
                        GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color

                    });

                    var lastItem = MessagesListView.ItemsSource.OfType<object>().Last();
                    MessagesListView.ScrollTo(lastItem, ScrollToPosition.MakeVisible, true);

                    if (Textbox.Text != "")
                    {


                        var MessageText = Functions.EncodeString(Textbox.Text);
                        Textbox.Text = ""; 

                        if (MessageText != String.Empty)
                        {
                            if (EmtyChat.IsVisible)
                            {
                                EmtyChat.IsVisible = false;
                            }

                            Task.Factory.StartNew(() =>
                            {
                              SendMessageTask(MessageText,"","","").ConfigureAwait(false);
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

        }

        public async Task MessageUpdater(string userid, string recipient_id, string LastMessageid)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (LastMessageid == time2)
                    {
                        return;
                    }
                    TaskHandle = "Stop";
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user_id", Settings.User_id),
                        new KeyValuePair<string, string>("recipient_id", recipient_id),
                        new KeyValuePair<string, string>("before_message_id", "0"),
                        new KeyValuePair<string, string>("after_message_id", LastMessageid),
                        new KeyValuePair<string, string>("s", Settings.Session)
                    });


                    var response = await client.PostAsync(
                        Settings.Website + "/app_api.php?application=phone&type=get_user_messages", formContent);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string apiStatus = data["api_status"].ToString();

                    var typping = data["typing"].ToString();
                    try
                    {
                        if (typping == "1")
                        {
                            Title = AppResources.Label_Typping;
                        }
                        else
                        {
                            Title = Username;
                        }
                    }
                    catch (Exception)
                    {

                    }

                    if (apiStatus == "200")
                    {
                        var messages = JObject.Parse(json).SelectToken("messages").ToString();
                        JArray ChatMessages = JArray.Parse(messages);
                        if (ChatMessages.Count == 0)
                        {
                            return;
                        }
                        if (ChatMessages.Count > 0)
                        {
                            foreach (var MessageInfo in ChatMessages)
                            {
                               MessageController.MessageType_Insert(MessageInfo,recipient_id);
                            }

                            if (ChatMessages.Count > 0)
                            {
                                var lastItem = MessagesListView.ItemsSource.OfType<object>().Last();
                                MessagesListView.ScrollTo(lastItem, ScrollToPosition.MakeVisible, true);
                            }
                            TaskHandle = "CanStart";
                        }

                    }
                }
            }
            catch (Exception)
            {
                return;
            }
       
          }

        private async Task<string> MessageFunctionTask(string userid, string recipient_id, string session)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                for (int i = 0; i < 5; i++)
                {
                    while (TaskWork == "Working")
                    {
                        await Task.Delay(Settings.MessageRequestSpeed);
                        var LastMessage = Functions.Messages.Last().messageID;

                        await MessageUpdater(userid, recipient_id, LastMessage);

                        if (TaskWork == "Stop")
                        {
                            return "Stop";
                        }
                    }
                    if (TaskWork == "Stop")
                    {
                        return "Stop";
                    }
                }
            }
            catch(Exception)
            {
                return "false";
            }
            return "true";
        }

        public async Task<string> MessageLoadmore(string recipient_id, string BeforeId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user_id", Settings.User_id),
                        new KeyValuePair<string, string>("recipient_id", recipient_id),
                        new KeyValuePair<string, string>("before_message_id", BeforeId),
                        new KeyValuePair<string, string>("after_message_id", "0"),
                        new KeyValuePair<string, string>("s", Settings.Session)

                    });

                    var response = await client.PostAsync( Settings.Website + "/app_api.php?application=phone&type=get_user_messages", formContent);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string apiStatus = data["api_status"].ToString();
                    if (apiStatus == "200")
                    {
                        var messages = JObject.Parse(json).SelectToken("messages").ToString();
                        JArray ChatMessages = JArray.Parse(messages);

                        foreach (var MessageInfo in ChatMessages)
                        {
                            #region  Objects

                            JObject ChatlistUserdata = JObject.FromObject(MessageInfo);
                            var Blal = ChatlistUserdata["messageUser"];
                            var avatar = Blal["avatar"].ToString();
                            var from_id = MessageInfo["from_id"].ToString();
                            var to_id = MessageInfo["to_id"].ToString();
                            var media = MessageInfo["media"].ToString();
                            var mediaFileName = MessageInfo["mediaFileName"].ToString();
                            var deleted_one = MessageInfo["mediaFileName"].ToString();
                            var deleted_two = MessageInfo["mediaFileName"].ToString();
                            var Messageid = MessageInfo["id"].ToString();
                            int Messageidx = Int32.Parse(Messageid);
                            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                            var Position = MessageInfo["position"].ToString();
                            var TextMsg = MessageInfo["text"].ToString();
                            var Type = MessageInfo["type"].ToString();
                            var TimeMsg = MessageInfo["time_text"].ToString();
                            var msgID = MessageInfo["id"].ToString();
                            var isVisible = true;
                            if (TextMsg == "")
                            {
                                isVisible = false;
                            }
                            else
                            {
                                TextMsg = WebUtility.HtmlDecode(TextMsg);
                                var Emoji = Functions.DecodeString(TextMsg);
                                TextMsg = Emoji;
                            }

                            #endregion

                            #region Insert to database
                            var ImageMedia = "";
                            var DownloadFileUrl = "NotDownload";
                            if (Type == "left_audio")
                            {
                                ImageMedia = Settings.MS_DownloadBlackicon;
                                mediaFileName = AppResources.Label_DownloadSound;
                            }
                            else if (Type == "right_audio")
                            {
                                ImageMedia = Settings.MS_DownloadWhiteicon;
                                mediaFileName = AppResources.Label_DownloadSound;
                            }
                            else if (Type == "right_video")
                            {
                                ImageMedia = Settings.MS_DownloadWhiteicon;
                                mediaFileName = AppResources.Label_DownloadVideo;
                            }
                            else if (Type == "left_video")
                            {
                                ImageMedia = Settings.MS_DownloadBlackicon;
                                mediaFileName = AppResources.Label_DownloadVideo;
                            }
                          
                            if (SQL_Commander.CheckMessage(Messageid) == "0")
                            {
                                if (Type == "//right_audio" || Type == "//left_audio")
                                {
                                    SQL_Commander.InsertMessage(new MessagesDB
                                    {
                                        message_id = Messageidx,
                                        from_id = from_id,
                                        to_id = to_id,
                                        media = media,
                                        mediafilename = mediaFileName,
                                        deleted_one = deleted_one,
                                        deleted_two = deleted_two,
                                        text = TextMsg,
                                        time = TimeMsg,
                                        type = Type,
                                        position = Position,
                                        avatar = avatar,
                                        ImageMedia = ImageMedia,
                                        MediaName = mediaFileName,
                                        DownloadFileUrl = DownloadFileUrl
                                    });
                                }
                                else if (Type == "right_video" || Type == "left_video")
                                {
                                    SQL_Commander.InsertMessage(new MessagesDB
                                    {
                                        message_id = Messageidx,
                                        from_id = from_id,
                                        to_id = to_id,
                                        media = media,
                                        mediafilename = mediaFileName,
                                        deleted_one = deleted_one,
                                        deleted_two = deleted_two,
                                        text = TextMsg,
                                        time = TimeMsg,
                                        type = Type,
                                        position = Position,
                                        avatar = avatar,
                                        ImageMedia = ImageMedia,
                                        MediaName = AppResources.Label_DownloadVideo,
                                        DownloadFileUrl = DownloadFileUrl
                                    });
                                }
                                else if (Type == "right_contact" || Type == "left_contact")
                                {
                                    TimeMsg = TimeMsg.Replace("{", "").Replace("}", "");
                                    string[] Con = TimeMsg.Split(',');

                                    SQL_Commander.InsertMessage(new MessagesDB
                                    {
                                        message_id = Messageidx,
                                        from_id = from_id,
                                        to_id = to_id,
                                        media = "UserContact.png",
                                        mediafilename = "UserContact.png",
                                        deleted_one = deleted_one,
                                        deleted_two = deleted_two,
                                        text = Con[0],
                                        time = TimeMsg,
                                        type = Type,
                                        position = Position,
                                        avatar = avatar,
                                        ImageMedia = "UserContact.png",
                                        MediaName = mediaFileName,
                                        contactName = Con[0],
                                        contactNumber = Con[1],
                                        DownloadFileUrl = DownloadFileUrl
                                    });
                                }
                                else if (Type == "left_sticker" || Type == "right_sticker")
                                {
                                    SQL_Commander.InsertMessage(new MessagesDB
                                    {
                                        message_id = Messageidx,
                                        from_id = from_id,
                                        to_id = to_id,
                                        media = media,
                                        mediafilename = mediaFileName,
                                        deleted_one = deleted_one,
                                        deleted_two = deleted_two,
                                        text = TextMsg,
                                        time = TimeMsg,
                                        type = Type,
                                        position = Position,
                                        avatar = avatar,
                                        ImageMedia = ImageMedia,
                                        MediaName = mediaFileName,
                                        DownloadFileUrl = DownloadFileUrl
                                    });
                                }
                                else
                                {

                                    SQL_Commander.InsertMessage(new MessagesDB
                                    {
                                        message_id = Messageidx,
                                        from_id = from_id,
                                        to_id = to_id,
                                        media = media,
                                        mediafilename = mediaFileName,
                                        deleted_one = deleted_one,
                                        deleted_two = deleted_two,
                                        text = TextMsg,
                                        time = TimeMsg,
                                        type = Type,
                                        position = Position,
                                        avatar = avatar,
                                        ImageMedia = ImageMedia,
                                        MediaName = mediaFileName,
                                        DownloadFileUrl = DownloadFileUrl
                                    });
                                }

                                #endregion

                                if (Position == "left")
                                {
                                    var ImageProfile =
                                        ImageSource.FromFile(DependencyService.Get<IPicture>()
                                            .GetPictureFromDisk(avatar, recipient_id));

                                    #region left_text

                                    if (Type == "left_text")
                                    {
                                  
                                       Functions.Messages.Insert(0, new MessageViewModal()
                                         {
                                                Content = TextMsg,
                                                Type = Type,
                                                Position = Position,
                                                CreatedAt = TimeMsg,
                                                messageID = msgID,
                                                UserImage = ImageProfile,
                                                CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color
                                         });
                                     
                                        
                                    }

                                    #endregion

                                    #region Left Image

                                    if (Type == "left_image")
                                    {
                                        var ImageUrl = MessageInfo["media"].ToString();
                                        DependencyService.Get<IPicture>().SavePictureToGalery(ImageUrl);
                                        var ImageMediaFile =
                                            ImageSource.FromFile(
                                                DependencyService.Get<IPicture>().GetPictureFromGalery(ImageUrl));

                                        if (DependencyService.Get<IPicture>().GetPictureFromGalery(ImageUrl) ==
                                            "File Dont Exists")
                                        {
                                            ImageMediaFile = new UriImageSource
                                            {
                                                Uri = new Uri(ImageUrl)
                                            };
                                        }

                                        Functions.Messages.Insert(0, new MessageViewModal()
                                        {
                                            UserImage = ImageProfile,
                                            Visibilty = isVisible,
                                            Content = TextMsg,
                                            Type = Type,
                                            Position = Position,
                                            CreatedAt = TimeMsg,
                                            ImageMedia = ImageMediaFile,
                                            messageID = msgID,
                                            ImageUrl = ImageUrl,
                                            CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color
                                        });

                                    }

                                    #endregion

                                    #region Left Sound

                                    if (Type == "//left_audio")
                                    {
                                        var audioUrl = MessageInfo["media"].ToString();
                                        if (Settings.DownloadSoundFiles_Automatically_Without_Click)
                                        {
                                            DependencyService.Get<ISoundRecord>().SaveAudioToDisk(audioUrl, Settings.User_id, msgID);
                                        }
                                        Functions.Messages.Insert(0, new MessageViewModal()
                                        {
                                            UserImage = ImageProfile,
                                            Visibilty = isVisible,
                                            Content = TextMsg,
                                            Type = Type,
                                            Position = Position,
                                            CreatedAt = TimeMsg,
                                            messageID = msgID,
                                            Media = audioUrl,
                                            ImageMedia = Settings.MS_DownloadBlackicon,
                                            DownloadFileUrl = "NotDownload",
                                            MediaName = AppResources.Label_DownloadSound,
                                            from_id = from_id,
                                            SliderSoundValue = "0",
                                            SliderMaxDuration = "100",
                                            CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color
                                        });

                                    }

                                    #endregion

                                    #region Left Video

                                    if (Type == "left_video")
                                    {
                                        var VideoUrl = MessageInfo["media"].ToString();
                                        Functions.Messages.Insert(0, new MessageViewModal()
                                        {
                                            UserImage = ImageProfile,
                                            Visibilty = isVisible,
                                            Content = TextMsg,
                                            Type = Type,
                                            Position = Position,
                                            CreatedAt = TimeMsg,
                                            messageID = msgID,
                                            Media = VideoUrl,
                                            ImageMedia = Settings.MS_DownloadBlackicon,
                                            DownloadFileUrl = "NotDownload",
                                            MediaName = AppResources.Label_DownloadVideo,
                                            from_id = from_id,
                                            SliderSoundValue = "0",
                                            SliderMaxDuration = "100",
                                            CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color
                                        });
                                    }

                                    #endregion

                                    #region left_contact

                                    if (Type == "left_contact")
                                    {
                                        TextMsg = TextMsg.Replace("{", "").Replace("}", "").Replace("Key:", "").Replace("Value:", "");
                                        string[] Con = TextMsg.Split(',');
                                        var dd = Con[0].ToString();
                                        Functions.Messages.Insert(0,new MessageViewModal()
                                        {
                                            Content = Con[0],
                                            Type = Type,
                                            Position = Position,
                                            CreatedAt = TimeMsg,
                                            messageID = msgID,
                                            ImageMedia = "UserContact.png",
                                            ContactName = Con[0].ToString(),
                                            ContactNumber = Con[1].ToString(),
                                            CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color,
                                            UserImage = ImageProfile,
                                        });
                                    }

                                    #endregion

                                    #region left_sticker

                                    if (Type == "left_sticker")
                                    {

                                        Functions.Messages.Add(new MessageViewModal()
                                        {
                                            Type = Type,
                                            Position = Position,
                                            CreatedAt = TimeMsg,
                                            messageID = msgID,
                                            ImageMedia = media,
                                            CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color,
                                            UserImage = ImageProfile,
                                        });
                                    }

                                    #endregion
                                }
                                else
                                {
                                    var ImageProfile =
                                        ImageSource.FromFile(DependencyService.Get<IPicture>()
                                            .GetPictureFromDisk(avatar, Settings.User_id));
                                    if (TextMsg == "")
                                    {
                                        isVisible = false;
                                    }

                                    #region right_text

                                    if (Type == "right_text")
                                    {

                                        Functions.Messages.Insert(0, new MessageViewModal()
                                        {
                                            Content = TextMsg,
                                            Type = Type,
                                            Position = Position,
                                            CreatedAt = TimeMsg,
                                            messageID = msgID,
                                            UserImage = ImageProfile,
                                            GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                                        });
                                    }

                                    #endregion

                                    #region right Image

                                    if (Type == "right_image")
                                    {
                                        var ImageUrl = MessageInfo["media"].ToString();

                                        var ImageMediaFile =
                                            ImageSource.FromFile(
                                                DependencyService.Get<IPicture>()
                                                    .GetPictureFromDisk(ImageUrl, Settings.User_id));
                                        if (
                                            DependencyService.Get<IPicture>()
                                                .GetPictureFromDisk(ImageUrl, Settings.User_id) == "File Dont Exists")
                                        {
                                            DependencyService.Get<IPicture>()
                                                .SavePictureToDisk(ImageUrl, Settings.User_id);
                                            ImageMediaFile = new UriImageSource {Uri = new Uri(ImageUrl)};
                                        }

                                        Functions.Messages.Insert(0, new MessageViewModal()
                                        {
                                            UserImage = ImageProfile,
                                            Visibilty = isVisible,
                                            Content = TextMsg,
                                            Type = Type,
                                            Position = Position,
                                            CreatedAt = TimeMsg,
                                            ImageMedia = ImageMediaFile,
                                            messageID = msgID,
                                            ImageUrl = ImageUrl,
                                            GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                                        });

                                    }

                                    #endregion

                                    #region right Sound

                                    if (Type == "//right_audio")
                                    {
                                        var audioUrl = MessageInfo["media"].ToString();
                                        if (Settings.DownloadSoundFiles_Automatically_Without_Click)
                                        {
                                            DependencyService.Get<ISoundRecord>().SaveAudioToDisk(audioUrl, Settings.User_id, msgID);
                                        }

                                        Functions.Messages.Insert(0, new MessageViewModal()
                                        {
                                            UserImage = ImageProfile,
                                            Visibilty = isVisible,
                                            Content = TextMsg,
                                            Type = Type,
                                            Position = Position,
                                            CreatedAt = TimeMsg,
                                            messageID = msgID,
                                            Media = audioUrl,
                                            ImageMedia = Settings.MS_DownloadWhiteicon,
                                            DownloadFileUrl = "NotDownload",
                                            MediaName = AppResources.Label_DownloadSound,
                                            from_id = from_id,
                                            SliderSoundValue = "0",
                                            SliderMaxDuration = "100",
                                            GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                                        });

                                    }

                                    #endregion

                                    #region right Video
                                    if (Type == "right_video")
                                    {
                                        var VideoUrl = MessageInfo["media"].ToString();
                                        if (Settings.DownloadSoundFiles_Automatically_Without_Click)
                                        {
                                           
                                        }
                                        Functions.Messages.Insert(0, new MessageViewModal()
                                        {
                                            UserImage = ImageProfile,
                                            Visibilty = isVisible,
                                            Content = TextMsg,
                                            Type = Type,
                                            Media = VideoUrl,
                                            Position = Position,
                                            CreatedAt = TimeMsg,
                                            messageID = msgID,
                                            ImageMedia = Settings.MS_DownloadWhiteicon,
                                            DownloadFileUrl = "NotDownload",
                                            MediaName = AppResources.Label_DownloadVideo,
                                            from_id = from_id,
                                            SliderSoundValue = "0",
                                            SliderMaxDuration = "100",
                                            GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                                        });
                                    }

                                    #endregion

                                    #region right_contact

                                    if (Type == "right_contact")
                                    {
                                        TextMsg = TextMsg.Replace("{", "").Replace("}", "").Replace("Key:", "").Replace("Value:", "");
                                        string[] Con = TextMsg.Split(',');

                                        Functions.Messages.Insert(0,new MessageViewModal()
                                        {
                                            Content = Con[0],
                                            Type = Type,
                                            Position = Position,
                                            CreatedAt = TimeMsg,
                                            messageID = msgID,
                                            ImageMedia = "UserContact.png",
                                            ContactName = Con[0],
                                            ContactNumber = Con[1],
                                            GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color,
                                            UserImage = ImageProfile,
                                        });
                                    }
                                    #endregion

                                    #region right_sticker

                                    if (Type == "right_sticker")
                                    {

                                        Functions.Messages.Add(new MessageViewModal()
                                        {
                                            Type = Type,
                                            Position = Position,
                                            CreatedAt = TimeMsg,
                                            messageID = msgID,
                                            ImageMedia = media,
                                            UserImage = ImageProfile,
                                        });
                                    }

                                    #endregion
                                }
                            }


                        }
                    }
                    return json;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
       
        private void ChatWindow_OnDisappearing(object sender, EventArgs e)
        {
            //if(Navigation.PopAsync(ChatWindow))
            TaskWork = "Stop";
            ChatActivityTab.NotifiStoper = "0";
        }

        private async void MessagesListView_OnRefreshing(object sender, EventArgs e)
        {
            try
            {
                var device = Resolver.Resolve<IDevice>();
                var oNetwork = device.Network;
                var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
                var FirstMessage = Functions.Messages.First();

                if (xx == false)
                {
                    LoadmoreFromData(FirstMessage.messageID);
                    FirstMessage = Functions.Messages.First();

                    await MessageLoadmore(Recipient_id, FirstMessage.messageID);

                }
                else
                {
                    LoadmoreFromData(FirstMessage.messageID);

                }
                MessagesListView.EndRefresh();
            }
            catch (Exception)
            {
            }

        }

        public void LoadmoreFromData(string recipient_id)
        {
            try
            {
               
                    var FirstMessage = Functions.Messages.First();
                    var messages = SQL_Commander.GetMessageList(Settings.User_id, Recipient_id, FirstMessage.messageID);
                    if (messages.Count() > 0)
                    {
                        foreach (var Message in messages)
                        {
                            var isVisible = true;
                            if (Message.text == "")
                            {
                                isVisible = false;
                            }

                            if (Message.position == "left")
                            {
                                var ImageProfile = ImageSource.FromFile( DependencyService.Get<IPicture>().GetPictureFromDisk(Message.avatar, recipient_id));
                                if (DependencyService.Get<IPicture>().GetPictureFromDisk(Message.avatar, recipient_id) == "File Dont Exists")
                                {
                                    ImageProfile = new UriImageSource
                                    {
                                        Uri = new Uri(Message.avatar)
                                    };
                                    DependencyService.Get<IPicture>().SavePictureToDisk(Message.avatar, recipient_id);
                                }



                                #region left_text

                                if (Message.type == "left_text")
                                {

                                    Functions.Messages.Insert(0, new MessageViewModal()
                                    {
                                        Content = Message.text,
                                        Type = Message.type,
                                        Position = Message.position,
                                        CreatedAt = Message.time,
                                        messageID = Message.message_id.ToString(),
                                        UserImage = ImageProfile,
                                        CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color

                                    });
                                }

                                #endregion

                                #region Left Image

                                if (Message.type == "left_image")
                                {
                                    var ImageUrl = Message.media;
                                    DependencyService.Get<IPicture>().SavePictureToGalery(ImageUrl);
                                    var ImageMediaFile =
                                        ImageSource.FromFile(
                                            DependencyService.Get<IPicture>().GetPictureFromGalery(ImageUrl));

                                    if (DependencyService.Get<IPicture>().GetPictureFromGalery(ImageUrl) ==
                                        "File Dont Exists")
                                    {
                                        ImageMediaFile = new UriImageSource
                                        {
                                            Uri = new Uri(ImageUrl)
                                        };
                                    }

                                    Functions.Messages.Insert(0, new MessageViewModal()
                                    {
                                        UserImage = ImageProfile,
                                        Visibilty = isVisible,
                                        Content = Message.text,
                                        Type = Message.type,
                                        Position = Message.position,
                                        CreatedAt = Message.time,
                                        ImageMedia = ImageMediaFile,
                                        messageID = Message.message_id.ToString(),
                                        ImageUrl = ImageUrl,
                                        CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color
                                    });

                                }

                                #endregion

                                #region Left Sound

                                if (Message.type == "//left_audio")
                                {

                                    Functions.Messages.Insert(0, new MessageViewModal()
                                    {
                                        UserImage = ImageProfile,
                                        Type = Message.type,
                                        Position = Message.position,
                                        CreatedAt = Message.time,
                                        messageID = Message.message_id.ToString(),
                                        Media = Message.media,
                                        DownloadFileUrl = Message.DownloadFileUrl,
                                        ImageMedia = Message.ImageMedia,
                                        MediaName = Message.MediaName,
                                        SliderSoundValue = "0",
                                        SliderMaxDuration = "100",
                                        CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color

                                    });

                                }

                                #endregion

                                #region Left Video

                                if (Message.type == "left_video")
                                {
                                    Functions.Messages.Insert(0, new MessageViewModal()
                                    {
                                        UserImage = ImageProfile,
                                        Type = Message.type,
                                        Position = Message.position,
                                        CreatedAt = Message.time,
                                        messageID = Message.message_id.ToString(),
                                        Media = Message.media,
                                        DownloadFileUrl = Message.DownloadFileUrl,
                                        ImageMedia = Message.ImageMedia,
                                        MediaName = Message.MediaName,
                                        SliderSoundValue = "0",
                                        SliderMaxDuration = "100",
                                        CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color
                                    });

                                }

                                #endregion

                                #region left_contact

                                if (Message.type == "left_contact")
                                {
                                    Functions.Messages.Add(new MessageViewModal()
                                    {
                                        Content = Message.text,
                                        Type = Message.type,
                                        Position = Message.position,
                                        CreatedAt = Message.time,
                                        messageID = Message.message_id.ToString(),
                                        UserImage = ImageProfile,
                                        ContactNumber = Message.contactNumber,
                                        ContactName = Message.contactName,
                                        ImageMedia = "UserContact.png",
                                        CommingBackroundBoxColor = Settings.MS_CommingBackroundBox_Color
                                    });
                                }

                                #endregion
                            }
                            else
                            {
                                var ImageProfile =
                                    ImageSource.FromFile(
                                        DependencyService.Get<IPicture>()
                                            .GetPictureFromDisk(Message.avatar, Settings.User_id));

                                #region right_text

                                if (Message.type == "right_text")
                                {

                                    Functions.Messages.Insert(0, new MessageViewModal()
                                    {
                                        Content = Message.text,
                                        Type = Message.type,
                                        Position = Message.position,
                                        CreatedAt = Message.time,
                                        messageID = Message.message_id.ToString(),
                                        UserImage = ImageProfile,
                                        GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                                    });
                                }

                                #endregion

                                #region right Image

                                if (Message.type == "right_image")
                                {
                                    var ImageUrl = Message.media;
                                    var ImageMediaFile =
                                        ImageSource.FromFile(
                                            DependencyService.Get<IPicture>()
                                                .GetPictureFromDisk(ImageUrl, Settings.User_id));
                                    if (
                                        DependencyService.Get<IPicture>().GetPictureFromDisk(ImageUrl, Settings.User_id) ==
                                        "File Dont Exists")
                                    {
                                        DependencyService.Get<IPicture>().SavePictureToDisk(ImageUrl, Settings.User_id);
                                        ImageMediaFile = new UriImageSource {Uri = new Uri(ImageUrl)};
                                    }

                                    Functions.Messages.Insert(0, new MessageViewModal()
                                    {
                                        UserImage = ImageProfile,
                                        Visibilty = isVisible,
                                        Content = Message.text,
                                        Type = Message.type,
                                        Position = Message.position,
                                        CreatedAt = Message.time,
                                        ImageMedia = ImageMediaFile,
                                        messageID = Message.message_id.ToString(),
                                        ImageUrl = ImageUrl,
                                        GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                                    });

                                }

                                #endregion

                                #region right Sound
                                if (Message.type == "//right_audio")
                                {

                                    Functions.Messages.Insert(0, new MessageViewModal()
                                    {
                                        UserImage = ImageProfile,
                                        Type = Message.type,
                                        Position = Message.position,
                                        CreatedAt = Message.time,
                                        messageID = Message.message_id.ToString(),
                                        Media = Message.MediaName,
                                        ImageMedia = Message.ImageMedia,
                                        DownloadFileUrl = Message.DownloadFileUrl,
                                        MediaName = Message.MediaName,
                                        SliderSoundValue = "0",
                                        SliderMaxDuration = "100",
                                        GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                                    });

                                }
                                #endregion

                                #region right Video
                                if (Message.type == "right_video")
                                {

                                    Functions.Messages.Insert(0, new MessageViewModal()
                                    {
                                        UserImage = ImageProfile,
                                        Type = Message.type,
                                        Position = Message.position,
                                        CreatedAt = Message.time,
                                        messageID = Message.message_id.ToString(),
                                        Media = Message.MediaName,
                                        ImageMedia = Message.ImageMedia,
                                        DownloadFileUrl = Message.DownloadFileUrl,
                                        MediaName = Message.MediaName,
                                        SliderSoundValue = "0",
                                        SliderMaxDuration = "100",
                                        GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                                    });

                                }

                                #endregion

                                #region right contact

                                if (Message.type == "right_contact")
                                {
                                    Functions.Messages.Add(new MessageViewModal()
                                    {
                                        UserImage = ImageProfile,
                                        Type = Message.type,
                                        Position = Message.position,
                                        CreatedAt = Message.time,
                                        messageID = Message.message_id.ToString(),
                                        Media = Message.media,
                                        ImageMedia = "UserContact.png",
                                        DownloadFileUrl = Message.DownloadFileUrl,
                                        MediaName = Message.MediaName,
                                        GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color,
                                        ContactNumber = Message.contactNumber,
                                        Content = Message.contactName,
                                        ContactName = Message.contactName,
                                    });

                                }

                                #endregion
                            }
                        }
                    }
                
            }
            catch (Exception)
            {
                
            }

        }

        public async Task MessageWorker(string recipientid)
        {
            try
            {
                MessageController.MessageType_InsertFrom_DataBase(recipientid);
                if (MessagesListView.ItemsSource.OfType<object>().Count()>0)
                {
                    var lastItem = MessagesListView.ItemsSource.OfType<object>().Last();
                    MessagesListView.ScrollTo(lastItem, ScrollToPosition.MakeVisible, true);
                    EmtyChat.IsVisible = false;
                }

                var LastID = Functions.Messages.LastOrDefault();
                var lastidmessage = "0";

                if (LastID != null)
                {
                    lastidmessage = LastID.messageID;
                }

                var apIchatMessages = await Wo_API.Get_User_Messages(lastidmessage);

                if (apIchatMessages != null)
                {
                    foreach (var messageInfo in apIchatMessages)
                    {
                        MessageController.MessageType_Insert(messageInfo, recipientid);
                    }

                    if (MessagesListView.ItemsSource.OfType<object>().Count() > 0)
                    {
                        var lastItem = MessagesListView.ItemsSource.OfType<object>().Last();
                        MessagesListView.ScrollTo(lastItem, ScrollToPosition.MakeVisible, true);
                        EmtyChat.IsVisible = false;
                    }
                }
            }
            catch (Exception)
            {
                EmtyChat.IsVisible = false;
            }
        }


        private void ChatWindow_OnAppearing(object sender, EventArgs e)
        {
            TaskWork = "Working";
            ChatActivityTab.NotifiStoper = Recipient_id;
            MessageFunctionTask(Settings.User_id, Recipient_id, Settings.Session).ConfigureAwait(false);
        }

      

        private void Textbox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Textbox.Text == "")
            {
                SendButton.IsVisible = true;
                RecourdButton.IsVisible = true;
                
            }
            else
            {
                SendButton.IsVisible = true;
                RecourdButton.IsVisible = false;
                
            }

            Gifts.IsVisible = false;
            Recourd.IsVisible = false;
        }

        private async void TakePicture_OnClicked(object sender, EventArgs e)
        {
            try
            {

            var action = await DisplayActionSheet(AppResources.Label_Photo, AppResources.Label_Cancel, null, AppResources.Label_Choose_from_Galery, AppResources.Label_Take_a_Picture);
            if (action == AppResources.Label_Choose_from_Galery)
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
                    await DisplayAlert(AppResources.Label_CannotDownload, AppResources.Label_CheckYourInternetConnection, AppResources.Label_OK);
                }
                else
                {
                    unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    time2 = unixTimestamp.ToString();
                    var time = DateTime.Now.ToString("hh:mm");
                    Functions.Messages.Add(new MessageViewModal
                    {
                        Content = Textbox.Text,
                        Type = "right_image",
                        messageID = time2,
                        CreatedAt = AppResources.Label_Uploading,
                        ImageUrl = file.Path,
                        ImageMedia = file.Path,
                        DownloadFileUrl = file.Path,
                        GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                    });
                    DependencyService.Get<IMethods>().UploudAttachment(file.GetStream(), file.Path, Settings.User_id, Recipient_id, Settings.Session, Textbox.Text, time2);
                    if (EmtyChat.IsVisible)
                    {
                        EmtyChat.IsVisible = false;
                    }
                    var lastItem = MessagesListView.ItemsSource.OfType<object>().Last();
                    MessagesListView.ScrollTo(lastItem, ScrollToPosition.MakeVisible, true);
                }


                }
                else if (action == AppResources.Label_Take_a_Picture)
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera avaialble.", AppResources.Label_OK);
                    return;
                }
                var time = DateTime.Now.ToString("hh:mm");
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                    CompressionQuality = 92,
                    SaveToAlbum = true,
                    Name = time + "Picture.jpg",

                });

                if (file == null)
                    return;

                unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                time2 = unixTimestamp.ToString();
                if (file.GetStream() != null)
                {
                    Functions.Messages.Add(new MessageViewModal
                    {
                        Content = Textbox.Text,
                        Type = "right_image",
                        messageID = time2,
                        CreatedAt = AppResources.Label_Uploading,
                        ImageUrl = file.Path,
                        ImageMedia = Settings.MS_UploadingImagePicture,
                        GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                    });
                    DependencyService.Get<IMethods>()
                        .UploudAttachment(file.GetStream(), file.AlbumPath, Settings.User_id, Recipient_id,
                            Settings.Session, Textbox.Text, time2);
                    if (EmtyChat.IsVisible)
                    {
                        EmtyChat.IsVisible = false;
                    }
                        var lastItem = MessagesListView.ItemsSource.OfType<object>().Last();
                    MessagesListView.ScrollTo(lastItem, ScrollToPosition.MakeVisible, true);
                }

                } 
            }

            catch
            {
                
            }
        }

        private async void EeButton_OnClicked(object sender, EventArgs e)
        {
           
            if (Recourd.IsVisible)
            {
                Recourd.IsVisible = false;
            }
            if (Gifts.IsVisible)
            {
                Gifts.IsVisible = false;
            }
            if (Options.IsVisible)
            {
                Options.IsVisible = false;
            }
            else
            {
                Options.IsVisible = true;
                await AnimateIn();
            }
        }

        private async void TakeVideoButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var action = await DisplayActionSheet(AppResources.Label_Video, AppResources.Label_Cancel, null, AppResources.Label_Choose_from_Galery, AppResources.Label_Recourd_Video);


                if (action == AppResources.Label_Choose_from_Galery)
                {
                    try
                    {
                        if (CrossMedia.Current.IsPickVideoSupported)
                        {
                            var file = await CrossMedia.Current.PickVideoAsync();

                            if (file == null)
                                return;

                            unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                            time2 = unixTimestamp.ToString();
                            if (file.GetStream() != null)
                            {
                                Functions.Messages.Add(new MessageViewModal
                                {
                                    Content = Textbox.Text,
                                    Type = "right_video",
                                    messageID = time2,
                                    //CreatedAt = Settings.Label_Uploading,
                                    ImageUrl = file.Path,
                                    DownloadFileUrl = file.Path,
                                    MediaName = AppResources.Label_Uploading,
                                    ImageMedia = Settings.MS_Videoicon,
                                    GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                                });

                                DependencyService.Get<IMethods>()
                                    .UploudAttachment(file.GetStream(), file.Path, Settings.User_id, Recipient_id,
                                        Settings.Session,
                                        Textbox.Text, time2);
                                if (EmtyChat.IsVisible)
                                {
                                    EmtyChat.IsVisible = false;
                                }

                                var lastItem = MessagesListView.ItemsSource.OfType<object>().Last();
                                MessagesListView.ScrollTo(lastItem, ScrollToPosition.MakeVisible, true);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                else if (action == AppResources.Label_Recourd_Video)
                {
                        try
                        {
                            await CrossMedia.Current.Initialize();

                            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
                            {
                                await DisplayAlert("No Camera", ":( No camera avaialble.", AppResources.Label_OK);
                                return;
                            }

                            var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
                            {
                                Name = "video.mp4",
                                Directory = "DefaultVideos",
                                SaveToAlbum = true
                            });

                            if (file == null)
                                return;

                            var time = DateTime.Now.ToString("hh:mm");
                            unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                            time2 = unixTimestamp.ToString();
                            if (file.GetStream() != null)
                            {
                                Functions.Messages.Add(new MessageViewModal
                                {
                                    Content = Textbox.Text,
                                    Type = "right_video",
                                    messageID = time2,
                                    //CreatedAt = Settings.Label_Uploading,
                                    ImageUrl = file.Path,
                                    DownloadFileUrl = file.Path,
                                    MediaName = AppResources.Label_Uploading,
                                    ImageMedia = Settings.MS_Videoicon,
                                    GoingBackroundBoxColor = Settings.MS_GoingBackroundBox_Color
                                });
                                DependencyService.Get<IMethods>()
                                    .UploudAttachment(file.GetStream(), file.AlbumPath, Settings.User_id, Recipient_id,
                                        Settings.Session, Textbox.Text, time2);
                                if (EmtyChat.IsVisible)
                                {
                                    EmtyChat.IsVisible = false;
                                }
                            var lastItem = MessagesListView.ItemsSource.OfType<object>().Last();
                                MessagesListView.ScrollTo(lastItem, ScrollToPosition.MakeVisible, true);
                            }
                        }
                        catch
                        {

                        }
                    
                }



            }
            catch
            {

            }
        }

        private void TakeContactButton_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ContactsPhonePage(this));
        }

        private void Textbox_OnFocused(object sender, FocusEventArgs e)
        {
            if (Options.IsVisible)
            {
                Options.IsVisible = false;
            }
            if (Gifts.IsVisible)
            {
                Gifts.IsVisible = false;
            }
            if (Recourd.IsVisible)
            {
                Recourd.IsVisible = false;
            }
        }

        private async void RecourdButton_OnClicked(object sender, EventArgs e)
        {
            if (Recourd.IsVisible)
            {
                Recourd.IsVisible = false;
            }
            else
            {
                Recourd.IsVisible = true;
            }
            if (Gifts.IsVisible)
            {
                Gifts.IsVisible = false;
            }
            if (Options.IsVisible)
            {
                Options.IsVisible = false;
            }
           await AnimatSoundIn();
        }

        private async void TakeGiftButton_OnClicked(object sender, EventArgs e)
        {
            if (Gifts.IsVisible)
            {
                Options.IsVisible = false;
                Gifts.IsVisible = false;
            }
            else
            {
                Options.IsVisible = false;
                Gifts.IsVisible = true;
            }
            await AnimateStickersIn();
        }

        private void BlockUser_Clicked(object sender, EventArgs e)
        {
            SendBlockRequest(Recipient_id).ConfigureAwait(false);
        }

        public async Task<string> SendBlockRequest(string recipient_id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user_id", Settings.User_id),
                        new KeyValuePair<string, string>("block_type", "block"),
                         new KeyValuePair<string, string>("recipient_id", recipient_id),
                        new KeyValuePair<string, string>("s", Settings.Session)
                    });

                    var response =
                        await
                            client.PostAsync(Settings.Website + "/app_api.php?application=phone&type=block_user",
                                formContent).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string apiStatus = data["api_status"].ToString();
                    if (apiStatus == "200")
                    {
                       // UserDialogs.Instance.Toast("User " + Username +" Has Been Blocked ");
                        var UsertoDelete = ChatActivityTab.ChatList.Where(a => a.UserID == recipient_id).FirstOrDefault();

                        if (UsertoDelete != null)
                        {
                            ChatActivityTab.ChatList.Remove(UsertoDelete);
                        }

                       

                            var Userid = SQL_Commander.GetChatUserByUsername(UsertoDelete.Username);
                            if (Userid != null)
                            {
                                SQL_Commander.DeleteChatUserRow(Userid);
                            }


                            SQL_Commander.DeleteMessage(Settings.User_id, Userid.UserID);
                          

                            var getuser = ContactsTab.ChatContactsList.Where(a => a.UserID == recipient_id).FirstOrDefault();

                            if (getuser != null)
                            {
                                ContactsTab.ChatContactsList.Remove(getuser);

                               
                                    var contact = SQL_Commander.GetContactUser(recipient_id);
                                    if (contact != null)
                                    {
                                        SQL_Commander.DeleteContactRow(contact);
                                    }
                                
                            }

                            
                        }
                        return "Succes";

                    
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

        public async Task AnimateIn()
        {
            await Task.WhenAll(new[] {
                AnimateItem (Photolabel, 400),
                AnimateItem (Contactlabel, 400),
                AnimateItem (Videolabel, 400),
                AnimateItem (TakePhotoButton, 600),
                AnimateItem (TakeVideoButton, 700),
                AnimateItem (TakeContactButton, 800),
                AnimateItem (TakeGiftButton, 800),
            });
        }

        public async Task AnimatSoundIn()
        {
            await Task.WhenAll(new[] {
                AnimateItem (Recourdgif, 700),
            });
        }

        public async Task AnimateStickersIn()
        {
            await Task.WhenAll(new[] {
                AnimateItem (Sticker1, 600),
                AnimateItem (Sticker2, 600),
                AnimateItem (Sticker3, 600),
                AnimateItem (Sticker4, 600),
                AnimateItem (Sticker5, 600),
                AnimateItem (Sticker6, 600),
                AnimateItem (Sticker7, 600),
                AnimateItem (Sticker8, 400),
                AnimateItem (Sticker9, 400),
                AnimateItem (Sticker10, 400),
                AnimateItem (Sticker11, 400),
                AnimateItem (Sticker12, 400),
                AnimateItem (Sticker13, 400),
                AnimateItem (Sticker14, 400),
                AnimateItem (Sticker15, 400),
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
    }
}
