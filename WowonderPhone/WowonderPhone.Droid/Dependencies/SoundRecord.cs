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

[assembly: Dependency(typeof(WowonderPhone.Droid.Dependencies.SoundRecord))]
namespace WowonderPhone.Droid.Dependencies
{
    public class SoundRecord : ISoundRecord
    {
        private MediaRecorder recorder;
        private string SoundFile;

        public void RecordingFunction(string Action, string Userid)
        {          
            if (Action == "Start")
            {
                try
                {
                    
                    var dir =Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
                    SoundFile = GetTimestamp(DateTime.Now) + ".mp3";
                    var DirectoryPath =
                        new Java.IO.File(dir + "/" + WowonderPhone.Settings.ApplicationName + "/" + SoundFile);
                    if (!Directory.Exists(dir + "/" + WowonderPhone.Settings.ApplicationName))
                    {
                        Directory.CreateDirectory(dir + "/" + WowonderPhone.Settings.ApplicationName);
                    }

                    recorder = new MediaRecorder();
                    recorder.Reset();
                    recorder.SetAudioSource(AudioSource.Mic);
                    recorder.SetOutputFormat(OutputFormat.Default);
                    recorder.SetAudioEncoder(AudioEncoder.AmrNb);
                    recorder.SetOutputFile(DirectoryPath.AbsolutePath);
                    recorder.Prepare();
                    recorder.Start();
                }
                catch (Exception)
                {
                    
                }
            }
            else
            {
                recorder.Stop();
                recorder.Release();
            }
        }

        public System.IO.Stream GetRecordedSound(string Userid)
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
        public string GetRecordedSoundPath(string Userid)
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
        public string DeleteRecordedSoundPath(string Userid)
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

        public void SaveAudioToDisk(string Url, string Userid ,string messageid)
        {
            try
            {
                var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);

                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var DirectoryPath = documentsPath + "//" + Userid;
                string AudioName = Url.Split('/').Last();
                string Audio = Path.Combine(DirectoryPath, AudioName);
                if (!File.Exists(Audio))
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

                            File.WriteAllBytes(Audio, bytes);

                            Controls.Functions.UpdateSoundAfterDownload(messageid , Audio);
                        }
                        catch (Exception)
                        {

                        }

                    };
                }
                else
                {
                    Controls.Functions.UpdateSoundAfterDownload(messageid , Audio);
                }
            }
            catch (Exception)
            {

            }
        }

        protected Android.Media.MediaPlayer player;
        public static System.Timers.Timer TimerSound;
        public string SoundPlay(string SoundName , string messageid , string messegeCondition ,string status, string CurrentSliderValue)
        {
            try
            {
                if (SoundName == "served")
                {
                    player = new Android.Media.MediaPlayer();
                    var ffd = Xamarin.Forms.Forms.Context.Assets.OpenFd("served.mp3");
                    player.Reset();
                    player.Prepared += (s, e) => { player.Start(); };
                    player.SetDataSource(ffd.FileDescriptor);
                    player.Prepare();
                }
                else
                {
                    if (status == "Play")
                    {
                        player = new Android.Media.MediaPlayer();
                        player.Reset();
                        player.Prepared += (s, e) => { player.Start(); };
                        player.SetDataSource(SoundName);
                        player.Prepare();
                        Messageid = messageid;
                        if (messegeCondition == "right_audio")
                        {
                            TimerSound = new System.Timers.Timer();
                            TimerSound.Interval = 1000;
                            TimerSound.Elapsed += TimerSound_Elapsed;
                            TimerSound.Start();
                        }
                        else
                        {
                            TimerSound = new System.Timers.Timer();
                            TimerSound.Interval = 1000;
                            TimerSound.Elapsed += TimerSound_ElapsedSlider;
                            TimerSound.Start();
                        }
                    }
                    else if (status == "Stop")
                    {
                        player.Stop();
                        TimerSound.Stop();
                    }
                    else if (status == "Pause")
                    {
                        player.Pause();
                        TimerSound.Stop();
                    }
                    else if (status == "PauseAfterplay")
                    {


                        try
                        {
                            var Converter = CurrentSliderValue.Substring(0, CurrentSliderValue.Length - 3); ;
                            int CurrentValue = 0;
                            CurrentValue = Int32.Parse(Converter);
                            player.SeekTo(CurrentValue);
                            player.Start();
                            TimerSound.Start();
                        }
                        catch (Exception)
                        {

                        }
                    }
                }

                return player.Duration.ToString();
            }
            catch (Exception)
            {
                return null;
            }
           

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