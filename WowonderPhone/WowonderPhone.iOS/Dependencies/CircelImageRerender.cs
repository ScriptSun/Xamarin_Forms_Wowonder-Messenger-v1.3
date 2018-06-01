using System;
using Xamarin.Forms;

using Xamarin.Forms.Platform.iOS;
using System.Drawing;
using WowonderPhone;
using WowonderPhone.iOS;

namespace WowonderPhone.iOS
{
	public class CircelImageRerender
	{
		public class CircleImageRenderer : ViewRenderer<CircleImage, UIKit.UIImageView>
		{
			UIKit.UIImageView imageView;
			protected override void OnElementChanged(ElementChangedEventArgs<CircleImage> e)
			{
				base.OnElementChanged(e);

				var rbv = e.NewElement;
				if (rbv != null)
				{

					var mainView = new UIKit.UIView();

					imageView = new UIKit.UIImageView(UIKit.UIImage.FromBundle(rbv.FileSource));
					imageView.Frame = new RectangleF
					(0, 0, (float)rbv.WidthRequest, (float)rbv.HeightRequest);
					imageView.Layer.CornerRadius = imageView.Frame.Size.Width / 2;
					if (rbv.HasBorder)
					{
						imageView.Layer.BorderColor = rbv.BorderColor.ToCGColor();
						imageView.Layer.BorderWidth = 2;
					}
					imageView.ClipsToBounds = true;

					mainView.Add(imageView);

					SetNativeControl((UIKit.UIImageView)mainView);
				}
			}

			protected override void OnElementPropertyChanged
			(object sender, System.ComponentModel.PropertyChangedEventArgs e)
			{
				base.OnElementPropertyChanged(sender, e);

				if (e.PropertyName == CircleImage.HasBorderProperty.PropertyName)
				{
					if (Element.HasBorder)
					{
						imageView.Layer.BorderWidth = 2;
						imageView.Layer.BorderColor = this.Element.BorderColor.ToCGColor();
					}
					else {
						imageView.Layer.BorderWidth = 0;
					}
				}
			}
		}
	}
}
