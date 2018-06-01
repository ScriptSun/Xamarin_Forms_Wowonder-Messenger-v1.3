using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Android.Media;
using Android.Media.Midi;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WowonderPhone.Dependencies;
using Xamarin.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Timers;

[assembly: Dependency(typeof(WowonderPhone.Droid.Dependencies.Video))]
namespace WowonderPhone.Droid.Dependencies
{
    public class Video : IVideo
    {
        private MediaRecorder recorder;
        private string SoundFile;

    

        public System.IO.Stream GetRecordedVideo(string Userid)
        {
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
            var DirectoryPath = dir + "/" + WowonderPhone.Settings.ApplicationName + "/" + SoundFile;

            if (File.Exists(DirectoryPath))
            {
                byte[] datass = File.ReadAllBytes(DirectoryPath);
                System.IO.Stream dsd = File.OpenRead(DirectoryPath);
               
                return dsd;
            }
            else
            {
                return null;
            }

        }
        public string GetVideoPath(string Userid)
        {
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
            var DirectoryPath = dir + "/" + WowonderPhone.Settings.ApplicationName + "/" + SoundFile;

            if (File.Exists(DirectoryPath))
            {         
                return DirectoryPath;
            }
            else
            {
                return null;
            }

        }
        public string DeleteVideoPath(string Userid)
        {
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
            var DirectoryPath = dir + "/" + WowonderPhone.Settings.ApplicationName + "/" + SoundFile;
           
            if (File.Exists(DirectoryPath))
            {
                File.Delete(DirectoryPath);

                return "Deleted";
            }
            else
            {
                return "Not exits";
            }

        }

        public void SaveVideoToGalery(string Url, string Userid ,string messageid)
        {
            try
            {
                var documentsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
                var DirectoryPath = documentsPath + "//" + Userid;
                string VideoName = Url.Split('/').Last();
                string Video = Path.Combine(DirectoryPath, VideoName);
                if (!File.Exists(Video))
                {
                    var webClient = new WebClient();
                    var url = new Uri(Url);
                    webClient.DownloadDataAsync(url);
                    

                    webClient.DownloadProgressChanged += (s, e) =>
                    {
                        var ss = webClient.ResponseHeaders["Content-Length"];
                        double bytesIn = double.Parse(e.BytesReceived.ToString());
                        var sss = Convert.ToString(bytesIn);
                        double totalBytes = double.Parse(e.TotalBytesToReceive.ToString()); 
                        var ss1 = Convert.ToString(totalBytes);
                        double percentage = bytesIn / totalBytes * 100;
                        var ss2 = Convert.ToString(percentage);
                        var Value = int.Parse(Math.Truncate(percentage).ToString());
                        Controls.Functions.UpdateDownloadPercentage(messageid, Value);
                    };

                    webClient.DownloadDataCompleted += (s, e) =>
                    {
                        try
                        {
                            var bytes = e.Result;
                            if (!Directory.Exists(DirectoryPath))
                            {
                                Directory.CreateDirectory(DirectoryPath);
                            }
                            
                            File.WriteAllBytes(Video, bytes);
                            var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                            mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(Video)));
                            Xamarin.Forms.Forms.Context.SendBroadcast(mediaScanIntent);
                            Controls.Functions.UpdateVideoAfterDownload(messageid , Video);
                        }
                        catch (Exception)
                        {

                        }

                    };
                }
                else
                {
                    Controls.Functions.UpdateSoundAfterDownload(messageid , Video);
                }
            }
            catch (Exception)
            {

            }
        }

        protected Android.Media.MediaPlayer player;
        public static System.Timers.Timer TimerSound;
        public string VedioPlay(string VidioUrl )
        {

            //var intent = new Intent();
            // var bundle = new Bundle();
            /// bundle.PutString("url", VidioUrl);
            //intent.PutExtras(bundle); ;
            Java.IO.File file = new Java.IO.File(VidioUrl);
            file.SetReadable(true);
            Android.Net.Uri uri = Android.Net.Uri.FromFile(file);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, "video/*");
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
            ((MainActivity)Xamarin.Forms.Forms.Context).StartActivity(intent);
            //Xamarin.Forms.Forms.Context.StartActivity(intent);
            return null;

        }

        private string Messageid;
        private void TimerSound_Elapsed(object sender, ElapsedEventArgs e)
        {
           try
            {
                if (player.CurrentPosition + 50 >= player.Duration)
                {
                    TimerSound.Stop();

                    Controls.Functions.UpdateSoundWhenFinshed(Messageid);
                }
            }
            catch (Exception)
            {
              
            }

        }

        private void TimerSound_ElapsedSlider(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (player.CurrentPosition + 50 >= player.Duration)
                {
                    TimerSound.Stop();
                    Controls.Functions.UpdateSoundWhenFinshed(Messageid);
                }
                else
                {
                    Controls.Functions.UpdateSoundDoraitionSlider(Messageid, player.CurrentPosition.ToString(), player.Duration.ToString());
                }
            }   
             catch (Exception)
            {

            }

        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}