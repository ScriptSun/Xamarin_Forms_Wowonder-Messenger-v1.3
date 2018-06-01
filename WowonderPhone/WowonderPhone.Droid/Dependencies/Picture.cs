using System;
using System.Linq;
using WowonderPhone.Dependencies;
using Xamarin.Forms;
using System.IO;
using System.Net;
using Android.Content;
using Android.Media;
using Android.Media.Midi;
using Java.Net;
using WowonderPhone.Controls;
using WowonderPhone.Pages.Tabs;

[assembly: Dependency(typeof(WowonderPhone.Droid.Dependencies.Picture))]

namespace WowonderPhone.Droid.Dependencies
{
    public class Picture : IPicture
    {
        public async void SavePictureToDisk(string Url, string Userid)
        {
            try
            {
                //"profile_picture": "https://wowonder.s3.amazonaws.com/upload/photos/d-avatar.jpg",
                //"cover_picture": "https://wowonder.s3.amazonaws.com/upload/photos/d-cover.jpg",
              

                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var DirectoryPath = documentsPath + "//" + Userid;
                string ImageName = Url.Split('/').Last();
                if(ImageName== "d-avatar.jpg"|| ImageName == "d-cover.jpg")
                {
                    documentsPath = documentsPath + "//DefaultImage";
                    string Image = Path.Combine(documentsPath, ImageName);
                    if (!File.Exists(Image))
                    {
                        var webClient = new WebClient();
                        var url = new Uri(Url);
                        webClient.DownloadDataAsync(url);
                        webClient.DownloadDataCompleted += (s, e) =>
                        {
                            try
                            {
                                var bytes = e.Result;
                                if (!Directory.Exists(documentsPath))
                                { Directory.CreateDirectory(documentsPath); }

                                File.WriteAllBytes(Image, bytes);
                            }
                            catch (Exception)
                            {
                            }

                        };
                    }
                }
                else
                {
                string Image = Path.Combine(DirectoryPath, ImageName);
                if (!File.Exists(Image))
                {
                    var webClient = new WebClient();
                    var url = new Uri(Url);
                    webClient.DownloadDataAsync(url);
                    webClient.DownloadDataCompleted += (s, e) =>
                    {
                        try
                        {
                            var bytes = e.Result;
                            if (!Directory.Exists(DirectoryPath))
                            { Directory.CreateDirectory(DirectoryPath); }

                            File.WriteAllBytes(Image, bytes);

                       
                                
                                var getuser = ContactsTab.ChatContactsList.Where(a => a.UserID == Userid).ToList().FirstOrDefault();
                                if (getuser != null)
                                {
                                    getuser.profile_picture = Image;
                                }
                        }
                        catch (Exception)
                        {
                        }
                       
                    };
                }
                }
            }
            catch (Exception)
            {
               
            }
          
          
        }

        public string GetPictureFromDisk(string Url, string Userid)
        {
            try
            {
               
                var documentsDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/" + Userid;
                string Image = Url.Split('/').Last();
                if (Image == "d-avatar.jpg" || Image == "d-cover.jpg")
                {
                    documentsDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "//DefaultImage";
                    string Filename = System.IO.Path.Combine(documentsDirectory, Image);
                    if (File.Exists(Filename))
                    {
                        return Filename;
                    }
                    else
                    {
                        return "File Dont Exists";
                    }
                }
                else
                {
                    string jpgFilename = System.IO.Path.Combine(documentsDirectory, Image);
                    if (File.Exists(jpgFilename))
                    {
                        return jpgFilename;
                    }
                    else
                    {
                        return "File Dont Exists";
                    }
                }
               
            }
            catch (Exception)
            {
                return "File Dont Exists";
            }

        }

        public void DeletePictureFromDisk(string Url, string Userid)
        {
            try
            {
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var DirectoryPath = documentsPath + "//" + Userid;
                string ImageName = Url.Split('/').Last();
                string Image = Path.Combine(DirectoryPath, ImageName);
                if (File.Exists(Image))
                {
                    File.Delete(Image);
                }

            }
            catch (Exception)
            {
               
            }

        }

        public void SavePictureToGalery(string Url)
        {
            try
            {
                var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
                var DirectoryPath = dir + "/" + WowonderPhone.Settings.ApplicationName + "/";
                string ImageName = Url.Split('/').Last();
                string ImageFullPath = Path.Combine(DirectoryPath, ImageName);

                if (!File.Exists(ImageFullPath))
                {
                    var webClient = new WebClient();
                    var url = new Uri(Url);
                    webClient.DownloadDataAsync(url);
                    webClient.DownloadDataCompleted += (s, e) =>
                    {
                        var bytes = e.Result;
                        if (!Directory.Exists(DirectoryPath))
                        {
                            Directory.CreateDirectory(DirectoryPath);
                        }
                        File.WriteAllBytes(ImageFullPath, bytes);
                        var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                        mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(ImageFullPath)));
                        Xamarin.Forms.Forms.Context.SendBroadcast(mediaScanIntent);
                    };
                }
            }
            catch (Exception)
            {
                
            }
            
        }

        public string GetPictureFromGalery(string Url)
        {
            try
            {
                var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
                string Image = Url.Split('/').Last();
                string jpgFilename = System.IO.Path.Combine(dir + "/" + WowonderPhone.Settings.ApplicationName + "/",
                    Image);
                if (File.Exists(jpgFilename))
                {
                    return jpgFilename;
                }
                else
                {
                    return "File Dont Exists";
                }
            }
            catch (Exception)
            {
                return "File Dont Exists";
            }

        }
    }
}