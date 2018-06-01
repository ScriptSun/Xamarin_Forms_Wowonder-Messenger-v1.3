using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WowonderPhone.Dependencies;
using WowonderPhone.Languish;
using WowonderPhone.SQLite;
using WowonderPhone.SQLite.Tables;
using Xamarin.Forms;

namespace WowonderPhone.Controls
{
    public class LoginUserFunctions
    {
        public static async Task<string> GetSessionProfileData(string userid, string session)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("user_id", Settings.User_id),
                        new KeyValuePair<string, string>("user_profile_id", userid),
                        new KeyValuePair<string, string>("s", Settings.Session)
                    });

                    var response =
                        await
                            client.PostAsync(Settings.Website + "/app_api.php?application=phone&type=get_user_data",
                                formContent);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string apiStatus = data["api_status"].ToString();
                    if (apiStatus == "200")
                    {
                        JObject userdata = JObject.FromObject(data["user_data"]);
                        Settings.UserFullName = userdata["name"].ToString();

                        var avatar = userdata["avatar"].ToString();
                        var cover = userdata["cover"].ToString();
                        var First_name = userdata["first_name"].ToString();
                        var Last_name = userdata["last_name"].ToString();
                        var About = userdata["about"].ToString();
                        var Website = userdata["website"].ToString();
                        var School = userdata["school"].ToString();
                        var name = userdata["name"].ToString();
                        var username = userdata["username"].ToString();
                        var gender = userdata["gender"].ToString();
                        var birthday = userdata["birthday"].ToString();
                        var email = userdata["email"].ToString();
                        var address = userdata["address"].ToString();
                        var user_id = userdata["user_id"].ToString();
                        var url = userdata["url"].ToString();

                        var facebook = userdata["facebook"].ToString();
                        var google = userdata["google"].ToString();
                        var twitter = userdata["twitter"].ToString();
                        var linkedin = userdata["linkedin"].ToString();
                        var youtube = userdata["youtube"].ToString();
                        var vk = userdata["vk"].ToString();
                        var instagram = userdata["instagram"].ToString();


                        About = System.Net.WebUtility.HtmlDecode(About);
                        address = System.Net.WebUtility.HtmlDecode(address);

                        Settings.UserFullName = name;

                        if (DependencyService.Get<IPicture>().GetPictureFromDisk(cover, user_id) == "File Dont Exists")
                        {
                            Settings.Coverimage = new UriImageSource
                            {
                                Uri = new Uri(cover)
                            };

                            DependencyService.Get<IPicture>().SavePictureToDisk(cover, user_id);
                        }
                        else
                        {
                            Settings.Coverimage = DependencyService.Get<IPicture>().GetPictureFromDisk(cover, user_id);
                        }


                        if (DependencyService.Get<IPicture>().GetPictureFromDisk(avatar, user_id) == "File Dont Exists")
                        {
                            Settings.Avatarimage = new UriImageSource
                            {
                                Uri = new Uri(avatar)
                            };
                            DependencyService.Get<IPicture>().SavePictureToDisk(avatar, user_id);
                        }
                        else
                        {
                            Settings.Avatarimage = DependencyService.Get<IPicture>().GetPictureFromDisk(avatar, user_id);
                        }

                       
                            var contact = SQL_Commander.GetProfileCredentialsById(user_id);
                            if (contact != null)
                            {
                                if (contact.UserID == user_id &&
                                    ((contact.Cover != cover) || (contact.Avatar != avatar) ||
                                     (contact.Birthday != birthday) || (contact.name != name)
                                     || (contact.Username != username) || (contact.First_name != First_name) ||
                                     (contact.Last_name != Last_name) || (contact.About != About) ||
                                     (contact.Website != Website)
                                     || (contact.School != School)))
                                {
                                    //Datas.DeleteProfileRow(contact);
                                    if ((contact.Avatar != avatar))
                                    {
                                        DependencyService.Get<IPicture>().DeletePictureFromDisk(contact.Avatar, user_id);
                                    }
                                    if ((contact.Cover != cover))
                                    {
                                        DependencyService.Get<IPicture>().DeletePictureFromDisk(contact.Cover, user_id);
                                    }

                                    contact.UserID = user_id;
                                    contact.name = name;
                                    contact.Avatar = avatar;
                                    contact.Cover = cover;
                                    contact.Birthday = birthday;
                                    contact.Address = address;
                                    contact.Gender = gender;
                                    contact.Email = email;
                                    contact.Username = username;
                                    contact.First_name = First_name;
                                    contact.Last_name = Last_name;
                                    contact.About = About;
                                    contact.Website = Website;
                                    contact.School = School;
                                    contact.vk = vk;
                                    contact.Facebook = facebook;
                                    contact.Google = google;
                                    contact.Linkedin = linkedin;
                                    contact.Youtube = youtube;
                                    contact.Twitter = twitter;
                                    contact.instagram = instagram;
                                SQL_Commander.UpdateProfileCredentials(contact);

                                }
                            }
                            else
                            {
                            SQL_Commander.InsertProfileCredentials(new LoginUserProfileTableDB()
                                {
                                    UserID = user_id,
                                    name = name,
                                    Avatar = avatar,
                                    Cover = cover,
                                    Birthday = birthday,
                                    Address = address,
                                    Gender = gender,
                                    Email = email,
                                    Username = username,
                                    First_name = First_name,
                                    Last_name = Last_name,
                                    About = About,
                                    Website = Website,
                                    School = School,
                                    Facebook = facebook,
                                    vk = vk,
                                    Google = google,
                                    Youtube = youtube,
                                    instagram = instagram,
                                    Linkedin = linkedin,
                                    Twitter = twitter

                                });

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
    }
}
