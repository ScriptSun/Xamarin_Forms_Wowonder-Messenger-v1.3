
using Android.Content;

using WowonderPhone.Dependencies;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(WowonderPhone.Droid.Dependencies.ClipboardService))]
namespace WowonderPhone.Droid.Dependencies
{
    public class ClipboardService : IClipboardService
    {
        public void CopyToClipboard(string text)
        {
            var clipboardManager = (ClipboardManager)Forms.Context.GetSystemService(Context.ClipboardService);

            // Create a new Clip
            ClipData clip = ClipData.NewPlainText("xxx_title", text);

            // Copy the text
            clipboardManager.PrimaryClip = clip;

            MainActivity.AndroidClipboardManager.Text = text;
        }
    } 
}