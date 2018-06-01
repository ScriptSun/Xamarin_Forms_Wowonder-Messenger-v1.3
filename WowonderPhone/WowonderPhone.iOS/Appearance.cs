using Xamarin.Forms.Platform.iOS;
using UIKit;

namespace UXDivers.Artina.Grial
{
    public class Appearance
    {
      
		public static UIColor AccentColor = ExportedColors.AccentColor.ToUIColor();
		public static UIColor TextColor = ExportedColors.InverseTextColor.ToUIColor();

		public static void Configure()
		{
			UINavigationBar.Appearance.BarTintColor = UIColor.Blue;
			UINavigationBar.Appearance.TintColor = UIColor.Blue;
			UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes
			{
				ForegroundColor = TextColor,
			};

			UIProgressView.Appearance.ProgressTintColor = AccentColor;

			UISlider.Appearance.MinimumTrackTintColor = AccentColor;
			UISlider.Appearance.MaximumTrackTintColor = AccentColor;
			UISlider.Appearance.ThumbTintColor = AccentColor;

			UISwitch.Appearance.OnTintColor = AccentColor;

			UITableViewHeaderFooterView.Appearance.TintColor = UIColor.Blue;

			UITableView.Appearance.SectionIndexBackgroundColor = AccentColor;
			UITableView.Appearance.SeparatorColor = AccentColor;

			UITabBar.Appearance.TintColor = AccentColor;

			UITextField.Appearance.TintColor = AccentColor;

			UIButton.Appearance.TintColor = UIColor.Blue;
			UIButton.Appearance.SetTitleColor(AccentColor, UIControlState.Normal);


		}
    }
}
