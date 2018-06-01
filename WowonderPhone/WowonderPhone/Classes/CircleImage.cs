using System;
using Xamarin.Forms;

namespace WowonderPhone
{
	
		public class CircleImage : Image
		{
			public static readonly BindableProperty FileSourceProperty =
				BindableProperty.Create<CircleImage, string>(p => p.FileSource, "");

			
			public string FileSource
			{
				get { return (string)base.GetValue(FileSourceProperty); }
				set { base.SetValue(FileSourceProperty, value); }
			}

			public static readonly BindableProperty BorderColorProperty =
				BindableProperty.Create<CircleImage, Color>(p => p.BorderColor, Color.White);

			public Color BorderColor
			{
				get { return (Color)base.GetValue(BorderColorProperty); }
				set { base.SetValue(BorderColorProperty, value); }
			}

			public static readonly BindableProperty HasBorderProperty =
				BindableProperty.Create<CircleImage, bool>(p => p.HasBorder, false);

			public bool HasBorder
			{
				get { return (bool)base.GetValue(HasBorderProperty); }
				set { base.SetValue(HasBorderProperty, value); }
			}
		}

}
