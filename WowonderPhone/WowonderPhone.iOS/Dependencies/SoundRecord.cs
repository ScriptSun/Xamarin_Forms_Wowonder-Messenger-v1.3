using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Timers;
using AudioToolbox;
using AVFoundation;
using Foundation;
using WowonderPhone.Dependencies;
using Xamarin.Forms;
using MediaPlayer;

[assembly: Dependency(typeof(WowonderPhone.iOS.Dependencies.SoundRecord))]
namespace WowonderPhone.iOS.Dependencies
{
	//private MediaRecorder recorder;


	public class SoundRecord : ISoundRecord
	{
		private string SoundFile;
		//private AVAudioRecorder recorder = null;
		// public NSError error = new NSError(new NSString("com.xamarin"), 1);

		private AVAudioRecorder recorder;



		public void RecordingFunction(string Action, string Userid)
		{
			if (Action == "Start")
			{
				try
				{
					NSError error;
					NSUrl url;
					NSDictionary settings;
					//recorder = new AVAudioRecorder();

					var audioSession = AVAudioSession.SharedInstance();
					var err = audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord);
					if (err != null)
					{

						return;
					}
					err = audioSession.SetActive(true);
					if (err != null)
					{

						return;
					}



					string fileName = string.Format("Myfile{0}.wav", DateTime.Now.ToString("yyyyMMddHHmmss"));
					string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
					string libraryPath = Path.Combine(documentsPath, "..", "Library");
					var audioFilePath = Path.Combine(libraryPath + WowonderPhone.Settings.ApplicationName + "/", fileName);

					if (!Directory.Exists(audioFilePath))
					{
						Directory.CreateDirectory(audioFilePath + WowonderPhone.Settings.ApplicationName);
					}

					SoundFile = fileName;

					url = NSUrl.FromFilename(audioFilePath);
					NSObject[] values = new NSObject[] {
					NSNumber.FromFloat (44100.0f),
					NSNumber.FromInt32 ((int)AudioToolbox.AudioFormatType.LinearPCM),
						NSNumber.FromInt32 (2),
						NSNumber.FromInt32 (16),
						NSNumber.FromBoolean (false),
						NSNumber.FromBoolean (false)
					};

					NSObject[] keyss = new NSObject[] {
						AVAudioSettings.AVSampleRateKey,
						AVAudioSettings.AVFormatIDKey,
						AVAudioSettings.AVNumberOfChannelsKey,
						AVAudioSettings.AVLinearPCMBitDepthKey,
						AVAudioSettings.AVLinearPCMIsBigEndianKey,
						AVAudioSettings.AVLinearPCMIsFloatKey
					};


					settings = NSDictionary.FromObjectsAndKeys(values, keyss);

					//Set recorder parameters 
					recorder = AVAudioRecorder.Create(url, new AudioSettings(settings), out error);

					recorder.PrepareToRecord();
					recorder.Record();
				}
				catch (Exception)
				{

				}
			}
			else
			{
				recorder.Stop();
				recorder.Dispose();
			}
		}

		public System.IO.Stream GetRecordedSound(string Userid)
		{
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			//string libraryPath = Path.Combine(documentsPath, "..", "Library");
			string audioFilePath = documentsPath + "//" + WowonderPhone.Settings.ApplicationName + "/" + SoundFile;

			if (File.Exists(audioFilePath))
			{
				byte[] datass = File.ReadAllBytes(audioFilePath);
				System.IO.Stream dsd = File.OpenRead(audioFilePath);

				return dsd;
			}
			else
			{
				return null;
			}
		}

		public string GetRecordedSoundPath(string Userid)
		{

			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			//string audioFilePath = Path.GetTempPath() + WowonderPhone.Settings.ApplicationName + "/" + SoundFile;
			string audioFilePath = documentsPath + "//" + WowonderPhone.Settings.ApplicationName + "/" + SoundFile;
			if (File.Exists(audioFilePath))
			{
				return audioFilePath;
			}
			else
			{
				return null;
			}
		}

		public string DeleteRecordedSoundPath(string Userid)
		{
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			//string libraryPath = Path.Combine(documentsPath, "..", "Library");
			string audioFilePath = documentsPath + "//" + WowonderPhone.Settings.ApplicationName + "/" + SoundFile;
			
			if (File.Exists(audioFilePath))
			{
				File.Delete(audioFilePath);

				return "Deleted";
			}
			else
			{
				return "Not exits";
			}
		}

		public void SaveAudioToDisk(string Url, string Userid, string messageid)
		{
			try
			{

				string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
				//var DirectoryPath = documentsPath + "//" + Userid;
				string AudioName = Url.Split('/').Last();
				string audioFilePath = documentsPath + "//" + WowonderPhone.Settings.ApplicationName + "/" + AudioName;
				//string Audio = Path.Combine(DirectoryPath, AudioName);
				if (!File.Exists(audioFilePath))
				{
					var webClient = new WebClient();
					var url = new Uri(Url);
					webClient.DownloadDataAsync(url);
					webClient.DownloadDataCompleted += (s, e) =>
					{
						try
						{
							var bytes = e.Result;
							
							File.WriteAllBytes(audioFilePath, bytes);

							Controls.Functions.UpdateSoundAfterDownload(messageid, audioFilePath);
						}
						catch (Exception)
						{

						}

					};
				}
				else
				{
					Controls.Functions.UpdateSoundAfterDownload(messageid, audioFilePath);
				}
			}
			catch (Exception)
			{

			}
		}

		private MPMusicPlayerController player;
		public static System.Timers.Timer TimerSound;

		private AVAudioPlayer backgroundMusic;
		private string backgroundSong="";
		#region Computed Properties
		public float BackgroundMusicVolume
		{
			get { return backgroundMusic.Volume; }
			set { backgroundMusic.Volume = value; }
		}

		public bool MusicOn { get; set; } = true;
		public float MusicVolume { get; set; } = 0.5f;

		public bool EffectsOn { get; set; } = true;
		public float EffectsVolume { get; set; } = 1.0f;
		#endregion




		public void ActivateAudioSession()
		{
			// Initialize Audio
			var session = AVAudioSession.SharedInstance();
			session.SetCategory(AVAudioSessionCategory.Ambient);
			session.SetActive(true);
		}

		public void DeactivateAudioSession()
		{
			var session = AVAudioSession.SharedInstance();
			session.SetActive(false);
		}

		public void ReactivateAudioSession()
		{
			var session = AVAudioSession.SharedInstance();
			session.SetActive(true);
		}
		public string SoundPlay(string SoundName, string messageid, string messegeCondition, string status,
			string CurrentSliderValue)
		{
			// MPMediaItem Now =  player.NowPlayingItem;
			if (SoundName == "served")
			{
				player = new MPMusicPlayerController();

				var ffd = new NSUrl("Sounds/" + "served.mp3");
				player.PrepareToPlay();


				player.Play(); ;

				//player.SetDataSource(ffd.FileDescriptor);
				//player.Prepare();
			}
			else
			{
				if (status == "Play")
				{
					ActivateAudioSession();
					NSUrl songURL;
					if (backgroundMusic != null)
					{
						//Stop and dispose of any background music
						backgroundMusic.Stop();
						backgroundMusic.Dispose();
					}

					songURL = new NSUrl(SoundName);
					NSError err;

					var mp3 = AudioToolbox.AudioFile.Open(SoundName, AudioFilePermission.Read, AudioFileType.MP3);
					if (mp3 != null)
					{ 
					   
					}
						
					 using (var backgroundMusic = AVAudioPlayer.FromUrl(Foundation.NSUrl.FromString(SoundName), out err))
					{
						Console.WriteLine(err);
					}

						//backgroundMusic = AVAudioPlayer.FromUrl(NSUrl.FromFilename(SoundName, out err));

					//backgroundMusic.FinishedPlaying += player_FinishedPlaying;
					backgroundMusic.PrepareToPlay();
					backgroundMusic.Play();

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
						var Converter = CurrentSliderValue.Substring(0, CurrentSliderValue.Length - 3);
						int CurrentValue = 0;
						CurrentValue = Int32.Parse(Converter);
						//player.SeekTo(CurrentValue);
						//player.Start();
						TimerSound.Start();
					}
					catch (Exception)
					{

					}
				}
			}

			return null;
		}


		private string Messageid;
		private void TimerSound_Elapsed(object sender, ElapsedEventArgs e)
		{
			try
			{

			}
			catch (Exception)
			{

			}

		}

		private void TimerSound_ElapsedSlider(object sender, ElapsedEventArgs e)
		{
			try
			{

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
