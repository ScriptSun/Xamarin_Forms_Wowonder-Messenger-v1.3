using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WowonderPhone.Dependencies;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using File = System.IO.File;
using IOException = System.IO.IOException;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WowonderPhone.Pages;
using WowonderPhone.SQLite;
using WowonderPhone.SQLite.Tables;
using Android.Provider;

[assembly: Xamarin.Forms.Dependency(typeof(WowonderPhone.Droid.Dependencies.IMethods))]

namespace WowonderPhone.Droid.Dependencies
{
    public class IMethods : WowonderPhone.Dependencies.IMethods
    {
        public void OpenImage(string Directory, string image, string Userid)
        {
            try
            {
                if (Directory == "Disk")
                {
                    var documentsDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/" + Userid;
                    string Image = image.Split('/').Last();
                    string FilePath = System.IO.Path.Combine(documentsDirectory, Image);
                    var bytes = File.ReadAllBytes(FilePath);
                    var externalPath = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/MaterialsReg/" + FilePath;
                    DirectoryInfo dirInfo = System.IO.Directory.CreateDirectory(externalPath.Remove(externalPath.LastIndexOf('/')));
                    File.WriteAllBytes(externalPath, bytes);
                    Java.IO.File file = new Java.IO.File(externalPath);
                    file.SetReadable(true);
                    Android.Net.Uri uri = Android.Net.Uri.FromFile(file);
                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetDataAndType(uri, "image/*");
                    intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
                    ((MainActivity) Xamarin.Forms.Forms.Context).StartActivity(intent);
                }
                else if (Directory == "Galary")
                {
                    var dir =
                        Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
                    string Image = image.Split('/').Last();
                    string FilePath = System.IO.Path.Combine(dir + "/" + WowonderPhone.Settings.ApplicationName + "/",
                        Image);
                    Intent intent = new Intent();
                    intent.SetAction(Android.Content.Intent.ActionView);
                    Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(FilePath));
                    intent.SetDataAndType(uri, "image/*");
                    ((MainActivity) Xamarin.Forms.Forms.Context).StartActivity(intent);
                }
                else
                {
                    Intent intent = new Intent();
                    intent.SetAction(Android.Content.Intent.ActionView);
                    Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(image));
                    intent.SetDataAndType(uri, "image/*");
                    ((MainActivity)Xamarin.Forms.Forms.Context).StartActivity(intent);
                }
            }
            catch (Exception)
            {
                
            }
           
        }

        public void OpenWebsiteUrl(string Website)
        {
            try
            {
                var uri = Android.Net.Uri.Parse(Website);
                var intent = new Intent(Intent.ActionView, uri);
                ((MainActivity)Xamarin.Forms.Forms.Context).StartActivity(intent);
            }
            catch (Exception)
            {

            }
        }

        public void OpenMessengerApp(string packegename)
        {
            try
            {
                Intent intent = Android.App.Application.Context.PackageManager.GetLaunchIntentForPackage(packegename);
                // If not NULL run the app, if not, take the user to the app store
                if (intent != null)
                {   
                    intent.AddFlags(ActivityFlags.NewTask);
                    Forms.Context.StartActivity(intent);
                }
                else
                {
                    intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=" + packegename));
                    intent.AddFlags(ActivityFlags.NewTask);
                    Android.App.Application.Context.StartActivity(intent);
                }
            }
            catch (ActivityNotFoundException)
            {
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("http://play.google.com/store/apps/details?id=" + packegename));
                intent.AddFlags(ActivityFlags.NewTask);
                Android.App.Application.Context.StartActivity(intent);
            }
        }

        public void SaveContactName(string Number,string Name)
        {
            try
            {
                var intent = new Intent(Intent.ActionInsert);
                intent.SetType(ContactsContract.Contacts.ContentType);
                intent.PutExtra(ContactsContract.Intents.Insert.Name, Name);
                intent.PutExtra(ContactsContract.Intents.Insert.Phone, Number);
                ((MainActivity)Xamarin.Forms.Forms.Context).StartActivity(intent);
            }
            catch (Exception)
            {

            }
        }


        public static string filepath;

        public string UploudAttachment(Stream stream , string FileNameAttachment,  string user_id, string recipient_id ,string Session, string TextMsg , string time2)
        {
            try
            {
                var values = new NameValueCollection();
                values["user_id"] = user_id;
                values["recipient_id"] = recipient_id;
                values["s"] = Session;
                values["text"] = TextMsg;
                values["send_time"] = time2;
                filepath = FileNameAttachment;
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    var files = new[] { new UploadFile { Name = "message_file", Filename = System.IO.Path.GetFileName(FileNameAttachment), ContentType = "text/plain", Stream = stream } };
                    var result = UploadFiles(Settings.Website + "/app_api.php?application=phone&type=insert_new_message", files, values);

                });

                return "Ok";
            }
             catch (Exception)
            {
                return "Error";
            }
        }
        public async Task<byte[]> UploadFiles(string address, IEnumerable<UploadFile> files, NameValueCollection values)
        {
            try
            {
                var request = WebRequest.Create(address);
                request.Method = "POST";
                var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
                request.ContentType = "multipart/form-data; boundary=" + boundary;
                boundary = "--" + boundary;
               
                using (var requestStream = request.GetRequestStream())
                {
                    // Write the values
                    foreach (string name in values.Keys)
                    {
                        var buffer = Encoding.ASCII.GetBytes(boundary + System.Environment.NewLine);
                        requestStream.Write(buffer, 0, buffer.Length);
                        buffer = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, System.Environment.NewLine));
                        requestStream.Write(buffer, 0, buffer.Length);
                        buffer = Encoding.UTF8.GetBytes(values[name] + System.Environment.NewLine);
                        requestStream.Write(buffer, 0, buffer.Length);
                    }

                    // Write the files
                    foreach (var file in files)
                    {
                        var buffer = Encoding.ASCII.GetBytes(boundary + System.Environment.NewLine);
                        requestStream.Write(buffer, 0, buffer.Length);
                        buffer = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}", file.Name, file.Filename, System.Environment.NewLine));
                        requestStream.Write(buffer, 0, buffer.Length);
                        buffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", file.ContentType, System.Environment.NewLine));
                        requestStream.Write(buffer, 0, buffer.Length);
                        file.Stream.CopyTo(requestStream);
                        buffer = Encoding.ASCII.GetBytes(System.Environment.NewLine);
                        requestStream.Write(buffer, 0, buffer.Length);
                    }

                    var boundaryBuffer = Encoding.ASCII.GetBytes(boundary + "--");
                    requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
                  
                }
             
                using (var response = request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                 
                using (var stream = new MemoryStream())
                {

                    responseStream.CopyTo(stream);
                    var json = System.Text.Encoding.UTF8.GetString(stream.ToArray(), 0, stream.ToArray().Length);
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    string apiStatus = data["api_status"].ToString();
                    if (apiStatus == "200")
                    {
                        var messages = JObject.Parse(json).SelectToken("messages").ToString();
                        JArray ChatMessages = JArray.Parse(messages);
                        Controls.Functions.UpdatelastIdMessage(ChatMessages, filepath);
                    }

                    return stream.ToArray();
                }
            }

            catch (Exception)
            {
                return null;
            }
        }

        public class UploadFile
        {
            public UploadFile()
            {
                ContentType = "application/octet-stream";
            }
            public string Name { get; set; }
            public string Filename { get; set; }
            public string ContentType { get; set; }
            public Stream Stream { get; set; }
        }
    }
}