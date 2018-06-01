using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using WowonderPhone.Dependencies;
using Xamarin.Forms;

namespace WowonderPhone.Languish
{
    public class L10n
    {
        public static void SetLocale()
        {
            DependencyService.Get<ILocale>().SetLocale();
        }

        /// <remarks>
        /// Maybe we can cache this info rather than querying every time
        /// </remarks>
        public static string Locale()
        {
            return DependencyService.Get<ILocale>().GetCurrent();
        }

        public static string Localize(string key, string comment)
        {

            var netLanguage = Locale();
            // Platform-specific
            ResourceManager temp = new ResourceManager("WowonderPhone.Languish.AppResources", typeof(L10n).GetTypeInfo().Assembly);
            Debug.WriteLine("Localize " + key);
            string result = temp.GetString(key, new CultureInfo(netLanguage));

            return result;
        }
    }
}
