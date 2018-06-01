using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Share;
using Plugin.Share.Abstractions;
using WowonderPhone.Controls;
using Xamarin.Forms;

namespace WowonderPhone.Pages.CustomCells
{
    public partial class GoingMessage : ViewCell
    {
        public GoingMessage()
        {
            InitializeComponent();

           
        }
        private async void Share(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
           
            await CrossShare.Current.Share(new ShareMessage()
            {
                Text = mi.CommandParameter.ToString(),
                Title = "Share",

            });

            if (CrossShare.Current.SupportsClipboard)
            {
                await CrossShare.Current.SetClipboardText(mi.CommandParameter.ToString());
            }

        }

        private async void Copy(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            if (CrossShare.Current.SupportsClipboard)
            {
                await CrossShare.Current.SetClipboardText(mi.CommandParameter.ToString());
            }
        }
    }
}
