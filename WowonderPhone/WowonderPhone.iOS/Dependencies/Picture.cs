using System;
using System.Linq;
using WowonderPhone.Dependencies;
using Xamarin.Forms;
using System.IO;
using System.Net;
using Foundation;
using UIKit;

[assembly: Dependency(typeof(WowonderPhone.iOS.Dependencies.Picture))]
namespace WowonderPhone.iOS.Dependencies
{
    public class Picture : IPicture
    {
        public void SavePictureToDisk(string Url, string Userid)
        {
            try
            {

                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var DirectoryPath = documentsPath + "//" + Userid;
                string ImageName = Url.Split('/').Last();
                if (ImageName == "d-avatar.jpg" || ImageName == "d-cover.jpg")
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
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);
            var DirectoryPath = documentsPath + "/" + WowonderPhone.Settings.ApplicationName + "/";
            string ImageName = Url.Split('/').Last();
            string ImageFullPath = Path.Combine(DirectoryPath, ImageName);

            if (!File.Exists(ImageFullPath))
            {
                var webClient = new WebClient();
                var url = new Uri(Url);
                webClient.DownloadDataAsync(url);
                webClient.DownloadDataCompleted += (s, e) => {
                    var bytes = e.Result;
                    if (!Directory.Exists(DirectoryPath))
                    { Directory.CreateDirectory(DirectoryPath); }

                    File.WriteAllBytes(ImageFullPath, bytes);

                    var chartImage = new UIImage(NSData.FromFile(ImageFullPath));

                    chartImage.SaveToPhotosAlbum((image, error) =>
                    {
                        if (error != null)
                        {
                            Console.WriteLine(error.ToString());
                        }
                    });
                };
            }

           
        }

        public string GetPictureFromGalery(string Url)
        {
            try
            {
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);
                string Image = Url.Split('/').Last();
                string jpgFilename = System.IO.Path.Combine(documentsPath + "/" + WowonderPhone.Settings.ApplicationName + "/", Image);
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