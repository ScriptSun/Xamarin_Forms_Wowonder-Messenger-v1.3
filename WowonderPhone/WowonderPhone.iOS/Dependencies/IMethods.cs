using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Contacts;
using ContactsUI;
using Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ObjCRuntime;
using QuickLook;
using StoreKit;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(WowonderPhone.iOS.Dependencies.IMethods))]
namespace WowonderPhone.iOS.Dependencies
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
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
				    {	   
						var fileinfo = new FileInfo(FilePath);
					   var previewController = new QuickLook.QLPreviewController
					   {
						   DataSource = new PreviewControllerDataSource(fileinfo.FullName, fileinfo.Name)
					   };

					   var controller = FindNavigationController();

					   controller?.PresentViewController(previewController, true, null);
				    });
                    
                }
                else if (Directory == "Galary")
                {
                   
                   Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>                                            
				   { 
						string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);
					   string Image = image.Split('/').Last();
					   string jpgFilename = System.IO.Path.Combine(documentsPath + "/" + WowonderPhone.Settings.ApplicationName + "/", Image);
					   var fileinfo = new FileInfo(jpgFilename);
					   var previewController = new QuickLook.QLPreviewController
					   {
						   DataSource = new PreviewControllerDataSource(fileinfo.FullName, fileinfo.Name)
						};
					   
					   var controller = FindNavigationController();
					   controller?.PresentViewController(previewController, true, null);
				   });
                }
                else
                {
                   Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
				   { 

					   var fileinfo = new FileInfo(image);
					   var previewController = new QuickLook.QLPreviewController
					   {
						   DataSource = new PreviewControllerDataSource(fileinfo.FullName, fileinfo.Name)
					   };

					   var controller = FindNavigationController();

					   controller?.PresentViewController(previewController, true, null);
				   });
                }
            }
            catch (Exception)
            {

            }

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


		public void OpenMessengerApp(string packegename)
		{
			try
			{
				bool isSimulator = Runtime.Arch == Arch.SIMULATOR;
				NSUrl itunesLink;
				if (isSimulator)
				{
					itunesLink = new NSUrl("https://itunes.apple.com/us/genre/ios/"+packegename );
				}
				else
				{
					itunesLink = new NSUrl("itms://itunes.apple.com");
				}
				UIApplication.SharedApplication.OpenUrl(itunesLink, new NSDictionary() { }, null);
			}
			catch
			{
				
			}
		}

		public void SaveContactName(string Number, string Name)
		{
			try
			{

				var store = new CNContactStore();
				var contact = new CNMutableContact();
				var cellPhone = new CNLabeledValue<CNPhoneNumber>(CNLabelPhoneNumberKey.Mobile, new CNPhoneNumber(Number));
				var phoneNumber = new[] { cellPhone };
				contact.PhoneNumbers = phoneNumber;
				contact.GivenName = Name;
				
                var predicate = CNContact.GetPredicateForContacts(Name);
                var fetchKeys = new NSString[] { CNContactKey.GivenName, CNContactKey.FamilyName };

				NSError error;
				var contacts = store.GetUnifiedContacts(predicate, fetchKeys, out error);
                var view = CNContactViewController.FromNewContact(contact);


                var authStatus = CNContactStore.GetAuthorizationStatus(CNEntityType.Contacts);
				if (authStatus == CNAuthorizationStatus.Denied || authStatus == CNAuthorizationStatus.Restricted)
				{
					
					var alertController = UIAlertController.Create("Contacts Permission Required\", \"This app requires permission to access your contacts. \" +\n \"Please go to Settings>AppName and enable access.", "", UIAlertControllerStyle.Alert);
					alertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
					UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alertController, true, null);

				}

				var navController = new UINavigationController(view);
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(navController, true, null);

			}
			catch (Exception)
			{

			}
		}

        private void PresentViewController(CNContactViewController view, bool v, object p)
        {
         
        }

        public void OpenWebsiteUrl(string Website)
        {
            try
            {
                //Its working on anouther code
            }
            catch (Exception)
            {

            }
        }

        public static string filepath;
        public string UploudAttachment(Stream stream, string FileNameAttachment, string user_id, string recipient_id, string Session, string TextMsg, string time2)
        {
            try
            {
                var values = new NameValueCollection();
                values["user_id"] = user_id;
                values["recipient_id"] = recipient_id;
                values["s"] = Session;
                values["text"] = TextMsg;
                values["send_time"] = time2;


                //Copy to Wowonder Folder application Just For IOS
				//Important to show the image on the Chatwindow when the user clicks
				string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);
				var DirectoryPath = documentsPath + "/" + WowonderPhone.Settings.ApplicationName + "/";
				string ImageName = FileNameAttachment.Split('/').Last();
				string ImageFullPath = Path.Combine(DirectoryPath, ImageName);
				System.IO.File.Copy(FileNameAttachment, ImageFullPath);
				filepath = ImageFullPath;

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

public class DocumentItem : QLPreviewItem
{
	private readonly string _uri;

	public DocumentItem(string title, string uri)
	{
		ItemTitle = title;
		_uri = uri;
	}

	public override string ItemTitle { get; }

	public override NSUrl ItemUrl => NSUrl.FromFilename(_uri);
}

	public class PreviewControllerDataSource : QLPreviewControllerDataSource
{
	private readonly string _url;
	private readonly string _filename;

	public PreviewControllerDataSource(string url, string filename)
	{
		_url = url;
		_filename = filename;
	}

	public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index)
	{
		return new DocumentItem(_filename, _url);
	}

	public override nint PreviewItemCount(QLPreviewController controller)
	{
		return 1;
	}
}

}
