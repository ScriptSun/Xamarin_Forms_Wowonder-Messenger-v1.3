using System;
using System.Threading;
using Foundation;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Contacts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ObjCRuntime;
using QuickLook;

[assembly: Xamarin.Forms.Dependency(typeof(WowonderPhone.iOS.Dependencies.ILocale))]
namespace WowonderPhone.iOS.Dependencies
{

    public class ILocale : WowonderPhone.Dependencies.ILocale
    {
        public void SetLocale()
        {

            var iosLocaleAuto = NSLocale.AutoUpdatingCurrentLocale.LocaleIdentifier;
            var netLocale = iosLocaleAuto.Replace("_", "-");
            System.Globalization.CultureInfo ci;
            try
            {
                ci = new System.Globalization.CultureInfo(netLocale);
            }
            catch
            {
                ci = new System.Globalization.CultureInfo(GetCurrent());
            }
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }



        public string GetCurrent()
        {
            var iosLocaleAuto = NSLocale.AutoUpdatingCurrentLocale.LocaleIdentifier;
            var iosLanguageAuto = NSLocale.AutoUpdatingCurrentLocale.LanguageCode;
            var netLocale = iosLocaleAuto.Replace("_", "-");
            var netLanguage = iosLanguageAuto.Replace("_", "-");

            #region Debugging Info
            // prefer *Auto updating properties
            //          var iosLocale = NSLocale.CurrentLocale.LocaleIdentifier;
            //          var iosLanguage = NSLocale.CurrentLocale.LanguageCode;
            //          var netLocale = iosLocale.Replace ("_", "-");
            //          var netLanguage = iosLanguage.Replace ("_", "-");

            Console.WriteLine("nslocaleid:" + iosLocaleAuto);
            Console.WriteLine("nslanguage:" + iosLanguageAuto);
            Console.WriteLine("ios:" + iosLanguageAuto + " " + iosLocaleAuto);
            Console.WriteLine("net:" + netLanguage + " " + netLocale);

            System.Globalization.CultureInfo ci;
            try
            {
                ci = new System.Globalization.CultureInfo(netLocale);
            }
            catch
            {
                ci = new System.Globalization.CultureInfo(NSLocale.PreferredLanguages[0]);
            }
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            Console.WriteLine("thread:  " + Thread.CurrentThread.CurrentCulture);
            Console.WriteLine("threadui:" + Thread.CurrentThread.CurrentUICulture);
            #endregion

            if (NSLocale.PreferredLanguages.Length > 0)
            {
                var pref = NSLocale.PreferredLanguages[0];
                netLanguage = pref.Replace("_", "-");
                Console.WriteLine("preferred:" + netLanguage);
            }
            else
            {
                netLanguage = "en"; // default, shouldn't really happen :)
            }
            return netLanguage;
        }
    }
}
