using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WowonderPhone.Controls;
using WowonderPhone.Dependencies;
using WowonderPhone.Languish;
using WowonderPhone.SQLite;
using WowonderPhone.SQLite.Tables;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using Xamarin.Forms.Xaml;

namespace WowonderPhone.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfilePage : ContentPage
    {
        private string F_Still_NotFriend = "";
        private string F_Request = "";
        private string F_AlreadyFriend = "";

        public static string U_UserID = "";
        public static string S_About = "";
        public static string S_Website = "";
        public static string S_Name = "";
        public static string S_Username = "";
        public static string S_Gender = "";
        public static string S_Email = "";
        public static string S_Birthday = "";
        public static string S_Address = "";
        public static string S_URL = "";
        public static string S_School = "";
        public static string S_Working = "";
        public static string S_Facebook = "";
        public static string S_Google = "";
        public static string S_Twitter = "";
        public static string S_Linkedin = "";
        public static string S_Youtube = "";
        public static string S_VK = "";
        public static string S_Instagram = "";
        public static string S_Can_follow = "";


        public class Userprofiletems
        {
            public string Label { get; set; }
            public string Icon { get; set; }
            public string Color { get; set; }
        }

        public static ObservableCollection<Userprofiletems> UserprofileListItems = new ObservableCollection<Userprofiletems>();

        public UserProfilePage(string userid)
        {
            InitializeComponent();

            GetUserProfileFromChach(userid);

        }


        public void GetUserProfileFromChach(string userid)
        {
            try
            {
                UserprofileListItems.Clear();
                var device = Resolver.Resolve<IDevice>();
                var oNetwork = device.Network; // Create Interface to Network-functions
                var xx = oNetwork.InternetConnectionStatus() == NetworkStatus.NotReachable;
               
                    var Profile = SQL_Commander.GetContactUser(userid);
                    if (Profile == null)
                    {
                        if (xx == true)
                        {

                            UserDialogs.Instance.Toast(AppResources.Label_CheckYourInternetConnection);
                        }
                        else
                        {
                            GetUserProfile(userid).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        this.Title = Profile.Name;
                        Username.Text = Profile.Name;

                        S_About = Profile.About;
                        S_Website = Profile.Website;
                        S_Name = Profile.Name;
                        S_Username = Profile.Username;
                        S_Gender = Profile.Gender;
                        S_Email = Profile.Email;
                        S_Birthday = Profile.Birthday;
                        S_Address = Profile.Address;
                        S_URL = Profile.Url;
                        S_School = Profile.School;
                        S_Working = Profile.Working;
                        S_Facebook = Profile.Facebook;
                        S_Google = Profile.Google;
                        S_Twitter = Profile.Twitter;
                        S_Linkedin = Profile.Linkedin;
                        S_VK = Profile.vk;
                        S_Instagram = Profile.instagram;

                        if (UserProfilePage.S_About != "")
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = UserProfilePage.S_About,
                                Icon = "\uf040",
                                Color = "#c5c9c8"
                            });
                        }
                        else
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = AppResources.Label_About_Me,
                                Icon = "\uf040",
                                Color = "#c5c9c8"
                            });
                        }

                        UserprofileListItems.Add(new Userprofiletems()
                        {
                            Label = UserProfilePage.S_Gender,
                            Icon = "\uf224",
                            Color = "#c5c9c8"
                        });

                        if (UserProfilePage.S_Birthday != "")
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = UserProfilePage.S_Birthday,
                                Icon = "\uf133",
                                Color = "#c5c9c8"
                            });
                        }

                        UserprofileListItems.Add(new Userprofiletems()
                        {
                            Label = UserProfilePage.S_Address,
                            Icon = "\uf041",
                            Color = "#c5c9c8"
                        });

                        UserprofileListItems.Add(new Userprofiletems()
                        {
                            Label = UserProfilePage.S_Website,
                            Icon = "\uf0ac",
                            Color = "#c5c9c8"
                        });

                        UserprofileListItems.Add(new Userprofiletems()
                        {
                            Label = UserProfilePage.S_School,
                            Icon = "\uf19d",
                            Color = "#c5c9c8"
                        });

                        UserprofileListItems.Add(new Userprofiletems()
                        {
                            Label = UserProfilePage.S_Working,
                            Icon = "\uf0b1",
                            Color = "#c5c9c8"
                        });

                        if (UserProfilePage.S_Facebook != "")
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = UserProfilePage.S_Facebook,
                                Icon = "\uf09a",
                                Color = "#00487b"
                            });

                        }
                        if (UserProfilePage.S_Google != "")
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = UserProfilePage.S_Google,
                                Icon = "\uf0d5",
                                Color = "#be2020"
                            });
                        }

                        if (UserProfilePage.S_VK != "")
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = UserProfilePage.S_VK,
                                Icon = "\uf189",
                                Color = "#326c95"
                            });
                        }

                        if (UserProfilePage.S_Twitter != "")
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = UserProfilePage.S_Twitter,
                                Icon = "\uf099",
                                Color = "#5a89aa"
                            });
                        }

                        if (UserProfilePage.S_Youtube != "")
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = UserProfilePage.S_Youtube,
                                Icon = "\uf16a",
                                Color = "#be2020"
                            });
                        }

                        if (UserProfilePage.S_Linkedin != "")
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = UserProfilePage.S_Linkedin,
                                Icon = "\uf0e1",
                                Color = "#5a89aa"
                            });
                        }

                        if (UserProfilePage.S_Instagram != "")
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = UserProfilePage.S_Instagram,
                                Icon = "\uf16d",
                                Color = "#5a89aa"
                            });
                        }

                        LastseenLabel.Text = Profile.lastseen;
                        UserInfoList.ItemsSource = UserprofileListItems;
                        AvatarImage.Source = ImageSource.FromFile(DependencyService.Get<IPicture>().GetPictureFromDisk(Profile.Avatar, userid));
                        CoverImage.Source = ImageSource.FromFile(DependencyService.Get<IPicture>().GetPictureFromDisk(Profile.Cover, userid));
                        var tapGestureRecognizer = new TapGestureRecognizer();

                        tapGestureRecognizer.Tapped += (s, ee) =>
                        {
                            DependencyService.Get<IMethods>().OpenImage("Disk", Profile.Avatar, userid);
                        };

                        if (AvatarImage.GestureRecognizers.Count > 0)
                        {
                            AvatarImage.GestureRecognizers.Clear();
                        }

                        AvatarImage.GestureRecognizers.Add(tapGestureRecognizer);


                        //Start updating the profile again
                        if (xx != true)
                        {
                            GetUserProfile(userid).ConfigureAwait(false);
                        }
                    }
                
            }
            catch (Exception)
            {

            }
        }
        public async Task<string> GetUserProfile(string userid)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user_id", Settings.User_id),
                        new KeyValuePair<string, string>("user_profile_id",userid),
                        new KeyValuePair<string, string>("s",Settings.Session)
                    });

                    var response = await client.PostAsync(Settings.Website + "/app_api.php?application=phone&type=get_user_data", formContent);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string apiStatus = data["api_status"].ToString();
                    if (apiStatus == "200")
                    {
                        JObject userdata = JObject.FromObject(data["user_data"]);
                        //Settings.UserFullName = userdata["name"].ToString();

                        var avatar = userdata["avatar"].ToString();
                        var cover = userdata["cover"].ToString();
                        var First_name = userdata["first_name"].ToString();
                        var Last_name = userdata["last_name"].ToString();
                        var Website = userdata["website"].ToString();
                        var user_id = userdata["user_id"].ToString();
                        var Lastseen = userdata["lastseen_time_text"].ToString();
                        var url = userdata["url"].ToString();
                        var is_following = userdata["is_following"].ToString();
                        var Pro_Type = userdata["pro_type"].ToString();
                        var Is_Pro = userdata["is_pro"].ToString();
                        var Is_blocked = userdata["is_blocked"].ToString();


                        S_About = Functions.DecodeString(userdata["about"].ToString());

                        S_Website = userdata["website"].ToString();
                        S_Name = userdata["name"].ToString();
                        S_Username = userdata["username"].ToString();
                        S_Gender = userdata["gender"].ToString();
                        S_Email = userdata["email"].ToString();
                        S_Birthday = userdata["birthday"].ToString();
                        S_Address = userdata["address"].ToString();
                        S_URL = userdata["url"].ToString();
                        S_School = userdata["school"].ToString();
                        S_Working = userdata["working"].ToString();
                        S_Facebook = userdata["facebook"].ToString();
                        S_Google = userdata["google"].ToString();
                        S_Twitter = userdata["twitter"].ToString();
                        S_Linkedin = userdata["linkedin"].ToString();
                        S_Youtube = userdata["youtube"].ToString();
                        S_VK = userdata["vk"].ToString();
                        S_Instagram = userdata["instagram"].ToString();
                        S_Can_follow = userdata["can_follow"].ToString();

                        if (Is_Pro == "1")
                        {
                            if (Pro_Type == "1")
                            {
                                StarIcon.IsVisible = true;
                            }
                            else if (Pro_Type == "2")
                            {
                                HotIcon.IsVisible = true;
                            }
                            else if (Pro_Type == "3")
                            {
                                UltimaIcon.IsVisible = true;
                            }
                            else if (Pro_Type == "4")
                            {
                                VIPIcon.IsVisible = true;
                            }
                        }
                        if (Is_blocked == "true")
                        {
                           
                        }

                        CoverImage.Source = ImageSource.FromFile(DependencyService.Get<IPicture>().GetPictureFromDisk(cover, user_id));
                        if (DependencyService.Get<IPicture>().GetPictureFromDisk(cover, user_id) == "File Dont Exists")
                        {
                            CoverImage.Source = new UriImageSource { Uri = new Uri(cover) };
                            DependencyService.Get<IPicture>().SavePictureToDisk(cover, user_id);
                        }

                        AvatarImage.Source = ImageSource.FromFile(DependencyService.Get<IPicture>().GetPictureFromDisk(avatar, user_id));
                        if (DependencyService.Get<IPicture>().GetPictureFromDisk(avatar, user_id) == "File Dont Exists")
                        {
                            AvatarImage.Source = new UriImageSource { Uri = new Uri(avatar) };
                            DependencyService.Get<IPicture>().SavePictureToDisk(avatar, user_id);
                        }

                        if (Website == "") { Website = AppResources.Label_Unavailable; }
                        if (S_School == "")
                        {
                            S_School = AppResources.Label_Askme;
                        }
                        if (S_Birthday == "" || S_Birthday.Contains("00"))
                        {
                            S_Birthday = AppResources.Label_Askme;
                        }
                        if (S_About == "" || S_About == " ")
                        {
                            S_About = Settings.PR_AboutDefault;
                        }
                        if (S_Address == "")
                        {
                            S_Address = AppResources.Label_Unavailable;
                        }
                        if (S_Gender == "")
                        {
                            S_Gender = AppResources.Label_Unavailable;
                        }
                        if (S_Working == "")
                        {
                            S_Working = AppResources.Label_Unavailable;
                        }
                        if (S_Website == "")
                        {
                            S_Website = AppResources.Label_Unavailable;
                        }

                        
                        LastseenLabel.Text = Lastseen;

                        if (Lastseen == "online" || Lastseen == "Online" || Lastseen.Contains("sec") || Lastseen.Contains("Sec"))
                        {
                            LastseenLabel.Text = AppResources.Label_Online;
                        }

                        

                        Username.Text = S_Name;

                        if (UserprofileListItems.Count > 0)
                        {
                            UserprofileListItems.Clear();
                        }

                        if (UserProfilePage.S_About != "")
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = UserProfilePage.S_About,
                                Icon = "\uf040",
                                Color = "#c5c9c8"
                            });
                        }
                        else
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = AppResources.Label_About_Me,
                                Icon = "\uf040",
                                Color = "#c5c9c8"
                            });
                        }

                        UserprofileListItems.Add(new Userprofiletems()
                        {
                            Label = UserProfilePage.S_Gender,
                            Icon = "\uf224",
                            Color = "#c5c9c8"
                        });

                        if (UserProfilePage.S_Birthday != "")
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = UserProfilePage.S_Birthday,
                                Icon = "\uf133",
                                Color = "#c5c9c8"
                            });
                        }

                        UserprofileListItems.Add(new Userprofiletems()
                        {
                            Label = UserProfilePage.S_Address,
                            Icon = "\uf041",
                            Color = "#c5c9c8"
                        });

                       

                        UserprofileListItems.Add(new Userprofiletems()
                        {
                            Label = UserProfilePage.S_Website,
                            Icon = "\uf0ac",
                            Color = "#c5c9c8"
                        });

                        UserprofileListItems.Add(new Userprofiletems()
                        {
                            Label = UserProfilePage.S_School,
                            Icon = "\uf19d",
                            Color = "#c5c9c8"
                        });

                        UserprofileListItems.Add(new Userprofiletems()
                        {
                            Label = UserProfilePage.S_Working,
                            Icon = "\uf0b1",
                            Color = "#c5c9c8"
                        });

                        if (UserProfilePage.S_Facebook != "")
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = UserProfilePage.S_Facebook,
                                Icon = "\uf09a",
                                Color = "#00487b"
                            });

                        }
                        if (UserProfilePage.S_Google != "")
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = UserProfilePage.S_Google,
                                Icon = "\uf0d5",
                                Color = "#be2020"
                            });
                        }

                        if (UserProfilePage.S_VK != "")
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = UserProfilePage.S_VK,
                                Icon = "\uf189",
                                Color = "#326c95"
                            });
                        }

                        if (UserProfilePage.S_Twitter != "")
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = UserProfilePage.S_Twitter,
                                Icon = "\uf099",
                                Color = "#5a89aa"
                            });
                        }

                        if (UserProfilePage.S_Youtube != "")
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = UserProfilePage.S_Youtube,
                                Icon = "\uf16a",
                                Color = "#be2020"
                            });
                        }

                        if (UserProfilePage.S_Linkedin != "")
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = UserProfilePage.S_Linkedin,
                                Icon = "\uf0e1",
                                Color = "#5a89aa"
                            });
                        }

                        if (UserProfilePage.S_Instagram != "")
                        {
                            UserprofileListItems.Add(new Userprofiletems()
                            {
                                Label = UserProfilePage.S_Instagram,
                                Icon = "\uf16d",
                                Color = "#5a89aa"
                            });
                        }

                        UserInfoList.ItemsSource = UserprofileListItems;
                        this.Title = S_Name;


                        //UserInfoList.HeightRequest = Functions.ListInfoResizer(S_About);


                      
                            var contact = SQL_Commander.GetContactUser(user_id);
                            if (contact != null)
                            {
                                if (contact.UserID == user_id && ((contact.Cover != cover) || (contact.Avatar != avatar) || (contact.Birthday != S_Birthday) || (contact.Name != S_Name)
                                                                  || (contact.Username != S_Username) || (contact.First_name != First_name) || (contact.Last_name != Last_name) || (contact.lastseen != Lastseen) || (contact.About != S_About) || (contact.Website != Website)
                                                                  || (contact.School != S_School)))
                                {

                                    if ((contact.Avatar != avatar))
                                    {
                                        DependencyService.Get<IPicture>().DeletePictureFromDisk(contact.Avatar, user_id);
                                    }
                                    if ((contact.Cover != cover))
                                    {
                                        DependencyService.Get<IPicture>().DeletePictureFromDisk(contact.Cover, user_id);
                                    }

                                    contact.UserID = user_id;
                                    contact.Name = S_Name;
                                    contact.Avatar = avatar;
                                    contact.Cover = cover;
                                    contact.Birthday = S_Birthday;
                                    contact.Address = S_Address;
                                    contact.Gender = S_Gender;
                                    contact.Email = S_Email;
                                    contact.Username = S_Username;
                                    contact.First_name = First_name;
                                    contact.Last_name = Last_name;
                                    contact.About = S_About;
                                    contact.Website = Website;
                                    contact.School = S_School;
                                    contact.Youtube = S_Youtube;
                                    contact.Facebook = S_Facebook;
                                    contact.Twitter = S_Twitter;
                                    contact.Linkedin = S_Linkedin;
                                    contact.Google = S_Google;
                                    contact.instagram = S_Instagram;
                                    SQL_Commander.UpdateContactUsers(contact);
                                }
                            }
                            else
                            {
                                ContactsTableDB contactt = new ContactsTableDB();
                                contactt.UserID = user_id;
                                contactt.Name = S_Name;
                                contactt.Avatar = avatar;
                                contactt.Cover = cover;
                                contactt.Birthday = S_Birthday;
                                contactt.Address = S_Address;
                                contactt.Gender = S_Gender;
                                contactt.Email = S_Email;
                                contactt.Username = S_Username;
                                contactt.First_name = First_name;
                                contactt.Last_name = Last_name;
                                contactt.About = S_About;
                                contactt.Website = Website;
                                contactt.School = S_School;
                                contactt.lastseen = Lastseen;
                                contactt.Youtube = S_Youtube;
                                contactt.Facebook = S_Facebook;
                                contactt.Twitter = S_Twitter;
                                contactt.Linkedin = S_Linkedin;
                                contactt.Google = S_Google;
                                contactt.instagram = S_Instagram;

                                SQL_Commander.InsertContactUsers(contactt);
                            }

                        

                    }
                    else if (apiStatus == "400")
                    {
                        json = AppResources.Label_Error;
                    }
                    return json;
                }
            }
            catch (Exception)
            {

                return AppResources.Label_Error;
            }


        }

        private void UserProfilePage_OnDisappearing(object sender, EventArgs e)
        {
            
        }

        private void CopyUrlButton_OnClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IClipboardService>().CopyToClipboard(S_URL);
        }

        private void UserInfoList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            UserInfoList.SelectedItem = null;
        }

        private void UserInfoList_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            
        }
        private void ShowmoreButton_OnClicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new HyberdPostViewer("User", ""));
        }

        private void ActionButton_OnClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IMethods>().OpenMessengerApp(Settings.Timeline_Package_Name);
        }
    }
}
