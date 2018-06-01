using System;
using Contacts;
using ContactsUI;
using CoreFoundation;
using Foundation;
using UIKit;

namespace WowonderPhone.iOS
{
    public class QuickContacts
    {
		public partial class ViewController : UIViewController
		{
			protected ViewController(IntPtr handle) : base(handle)
			{

			}

			public override void ViewDidLoad()
			{
				base.ViewDidLoad();
			}

            public  void showcontact(CNContactViewController dd){
				//var view = CNContactViewController.FromContact(contact);

				// Display the view
				PresentViewController(dd, true, null);
            }
		
		}
    }
}
