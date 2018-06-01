//WOWONDER ANDROID MESSENGER V1.3
//==============================
//This is A sample of xamarin forms chat application 
//This solution is not allowed to be shared on Google play 
//You can get the styles and Xaml codes and use it as you like 
//Devloped by Elin Doughouz
//Follow me on linkded >> https://www.linkedin.com/in/elin-doughouz-1b7458131
//Follow me on Facebook >> https://www.facebook.com/Elindoughous


using WowonderPhone.Languish;
using Xamarin.Forms;

namespace WowonderPhone
{

	
    //1-Add Your logo From Solution Explorer go to WowonderPhone.Android > Resources > Remove logo.png 
    //2-Right click on Resources folder > Add > Existing Item > pick your own logo.png
	//Same thing for all images and icons


    public class Settings
    {
        public static string Website = "https://demo.wowonder.com/";
        public static string ApplicationName = "WoWonder";
        public static string Version = "1.3";

        public static string API_ID = "144235f5702cb70fa6c3f48842738e35";
        public static string API_KEY = "b37bb1cd53d0bc21c13c1644e98a58ca";

        public static string ConnectivitySystem = "1";

        public static bool AcceptTerms_Display = true;

        ///*********************************************************
        
        public static bool Show_Facebook_Login = true;
        public static bool Show_Google_Login = true;
        public static bool Show_Vkontakte_Login = false;
        public static bool Show_Instagram_Login = false;
        public static bool Show_Twitter_Login = false;

        ///*********************************************************
        
        /// APP Default Style
        public static string MainColor = "#a84849";
        public static string MainPage_HeaderBackround_Color = "#444";
        public static string MainPage_HeaderText_Color = "#ffff";

        

        public static string Background_Image_WelcomePage = "bg2.jpg";
        public static string Background_Image_RegisterPage = "signupBackground.jpg";
        public static string Background_Image_LoginPage = "signup_bg.png";
        public static string Logo_Image_WelcomePage = "logo.png";
        public static string Logo_Image_LoginPage = "logo.png";


        public static string Timeline_Package_Name = "com.facebook.orca"; //APK name on Google Play

        //Android Display
        ///*********************************************************
        public static bool TurnFullScreenOn = false;
        public static bool BuginAndroidAutoresize = false; //For auto resize the screen on the Chat window

        //Bypass Web Erros
        ///*********************************************************
      
        public static bool TurnTrustFailureOn_WebException = false;
        public static bool TurnSecurityProtocolType3072On = false;

        ///*********************************************************

        //Main Icons
        public static string Like_Icon = "Like_Icon.png";
        public static string Add_Icon = "Plus.png";
        public static string CheckMark_Icon = "Checkmark.png";
        public static string AddUser_Icon = "P_AddUser.png";

        ///*********************************************************

        //Button Colors
        public static string ButtonColorNormal = MainColor; // change it to Hex if you want
        public static string ButtonTextColorNormal = "#fff";
        public static string ButtonLightColor = "#E7E7E7";
        public static string ButtonTextLightColor = "#444";


        //Going Message Template
        public static string MS_GoingBackroundBox_Color = MainColor;
        public static string MS_PlayWhiteicon = "PlayW.png";
        public static string MS_PauseWhiteicon = "PauseW.png";
        public static string MS_DownloadWhiteicon = "DownloadW.png";
        public static string MS_UploadWhiteicon = "UploadW.png";
        public static string MS_DownloadingWhiteicon = "DownloadingG.png";
        public static string MS_UploadingImagePicture = "UploadImage.png";
        public static string MS_Videoicon = "Video.png";

        //Comming Message Tamplate
        public static string MS_CommingBackroundBox_Color = "#EFEFF4";
        public static string MS_PlayBlackicon = "PlayB.png";
        public static string MS_PauseBlackicon = "PauseB.png";
        public static string MS_DownloadBlackicon = "DownloadB.png";
        public static string MS_UploadBlackicon = "UploadW.png";
        public static string MS_DownloadingBlackicon = "DownloadingG.png";
        public static string MS_VideoBlackicon = "VideoBlack.png";

     
        //User Profile Control
        public static string PR_AboutDefault = AppResources.Label_Hey_There +" "+ ApplicationName + " !!";
        public static string Welcome_to = AppResources.Label_Welcome_to_App + " " + ApplicationName;
        public static string InviteSMSText = AppResources.Label_invite_sms_text + " " + ApplicationName;

        
        //ChatActivty Page
        public static string UnseenMesageColor = "#ededed";

        public static bool Show_Online_Oflline_Icon = true;

        public static bool Delete_Old_Profile_Picture_From_Phone = false;

        public static bool DownloadSoundFiles_Automatically_Without_Click = false;

        public static int RefreshChatActivitiesSecounds = 8000; // 8 secounds
        public static int MessageRequestSpeed = 6000; // 6 secounds
        public static int LimitContactGetFromPhone = 300;

        
        //Recourd Sound Style & Text
        public static string MW_Recourd_ImageButton = "RecourdImage.png";
        public static string MW_Recourd_StopImageButton = "stop.png";
      
      
        
        public static string FN_SoundFileEmoji = "\uD83C\uDF99 " + AppResources.Label_Sound_File;
        public static string FN_ImageFileEmoji = "\uD83D\uDCF7 " + AppResources.Label_Image_File;
        public static string FN_VideoFileEmoji = "\ud83d\udcf9 " + AppResources.Label_Video_File;
        public static string FN_StickerFileEmoji = "\uD83D\uDD16 " + AppResources.Label_Sticker;
        public static string FN_ContatactFileEmoji = "\uD83D\uDCBC " + AppResources.Label_Contact_Number;

            
        //Privacy & Settings Page
       
        public static string PR_SaveButton_Color = MainColor;
        public static string PR_SaveButtonText_Color = "#ffffff";
      

        /// About Page >>>>>>>>>>>
        public static string AboutText = "The " + ApplicationName + " name associated trademarks and logos and the  logo are trademarkes of " + ApplicationName + " related entities.\r\n";
        public static string CopyrightText = "Warning the porgram is protected by copyright law and international treaties, unauthorized reproduction or distribution of this program , or any portion of it may result in severe civil criminal penalties and will be prosecuted to the maximum extend possible under the law";
        public static string PrivacyPolicyText = "This Privacy Policy governs the manner in which " + ApplicationName + " collects, uses, maintains and discloses information collected from users (each, a \"User\") of the " + Website + " . This privacy policy applies to the Application and all products and services offered by " + ApplicationName + ".";
        public static string SharingYourPersonalInformationText = "We do not sell, trade, or rent Users personal identification information to others. We may share generic aggregated demographic information not linked to any personal identification information regarding visitors and users with our business partners, trusted affiliates and advertisers for the purposes outlined above.We may use third party service providers to help us operate our business and the Site or administer activities on our behalf, such as sending out newsletters or surveys. We may share your information with these third parties for those limited purposes provided that you have given us your permission";



        //Don't Modify here >>>>>>>>>>>

        public static string MainPage_HeaderTextLabel = ApplicationName;
        public static string Onesignal_APP_ID = "";
        public static string Session { get; set; }
        public static string User_id { get; set; }
        public static string ProfilePicture { get; set; }
        public static string Username { get; set; }
        public static string Device_ID = "";
        public static string UserFullName { get; set; }
        public static ImageSource Coverimage { get; set; }
        public static ImageSource Avatarimage { get; set; }
        public static string SearchByGenderValue { get; set; }
        public static string SearchByProfilePictureValue { get; set; }
        public static string SearchByStatusValue { get; set; }
        public static bool ReLogin { get; set; } = false;
        public static bool NotificationVibrate { get; set; } = true;
        public static bool NotificationSound { get; set; } = true;
        public static bool NotificationPopup { get; set; } = true;
        public static string NotificationLedColor { get; set; } = MainColor;
        public static string NotificationLedColorName { get; set; } = "Default";
        public static string Label_Default = AppResources.Label_Default;
      

    }
}
