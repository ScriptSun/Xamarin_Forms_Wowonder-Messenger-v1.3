using System;
using System.Linq;
using WowonderPhone.Dependencies;
using Xamarin.Forms;
using System.IO;
using System.Net;
using Foundation;
using UIKit;
using WowonderPhone.iOS.Dependencies;

[assembly: Dependency(typeof(WowonderPhone.iOS.Dependencies.Video))]
namespace WowonderPhone.iOS.Dependencies
{
	public class Video : IVideo
	{
		//private MediaRecorder recorder;
		private string VideoFile;

		public System.IO.Stream GetRecordedVideo(string Userid)
		{
			var dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);
			var DirectoryPath = dir + "/" + WowonderPhone.Settings.ApplicationName + "/" + VideoFile;

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
			var dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);
			var DirectoryPath = dir + "/" + WowonderPhone.Settings.ApplicationName + "/" + VideoFile;
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
			var dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);
			var DirectoryPath = dir + "/" + WowonderPhone.Settings.ApplicationName + "/" + VideoFile;

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

		public void SaveVideoToGalery(string Url, string Userid, string messageid)
		{
			try
			{
				var dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);
				var DirectoryPath = dir + "//" + Userid;
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

							    UIVideo.SaveToPhotosAlbum(Video, delegate (string p, NSError error)
								{
								if (error == null)
								{
								  Console.WriteLine("Saved");
								}
								 else
								{
								  Console.WriteLine("Error in saving.");
								}
							 });


							Controls.Functions.UpdateVideoAfterDownload(messageid, Video);
						}
						catch (Exception)
						{

						}

					};
				}
				else
				{
					Controls.Functions.UpdateVideoAfterDownload(messageid, Video);
				}
			}
			catch (Exception)
			{

			}
		}
		public string VedioPlay(string VidioUrl)
		{
			try { 
			        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
				   {
					   var fileinfo = new FileInfo(VidioUrl);
					   var previewController = new QuickLook.QLPreviewController
					   {
						   DataSource = new PreviewControllerDataSource(fileinfo.FullName, fileinfo.Name)
					   };
					   var controller = FindNavigationController();
					   controller?.PresentViewController(previewController, true, null);
				   });
			
			}
			catch (Exception)
			{

			}

			return null;

		}

		private string Messageid;


		public static String GetTimestamp(DateTime value)
		{
			return value.ToString("yyyyMMddHHmmssffff");
		}

		public UINavigationController FindNavigationController()
		{
			foreach (var window in UIApplication.SharedApplication.Windows)
			{
				if (window.RootViewController.NavigationController != null)
				{
					return window.RootViewController.NavigationController;
				}

				var value = CheckSubs(window.RootViewController.ChildViewControllers);
				if (value != null)
					return value;
			}
			return null;
		}



		public UINavigationController CheckSubs(UIViewController[] controllers)
		{
			foreach (var controller in controllers)
			{
				if (controller.NavigationController != null)
				{
					return controller.NavigationController;
				}

				var value = CheckSubs(controller.ChildViewControllers);

				return value;
			}

			return null;
		}

	}
}
