using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WowonderPhone.Languish
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return null;
            Debug.WriteLine("Provide: " + Text);
            // Do your translation lookup here, using whatever method you require
            var translated = L10n.Localize(Text, Text);

            return translated;
        }
    }
}
