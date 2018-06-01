using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace WowonderPhone.Pages.CustomCells
{
    public partial class RoundedLabel : ContentView
    {
        public RoundedLabel()
        {
            InitializeComponent();
        }

        public static BindableProperty RoundedLabelBackgroundColorProperty =
            BindableProperty.Create(
                nameof(RoundedLabelBackgroundColor),
                typeof(Color),
                typeof(RoundedLabel),
                defaultValue: Color.Green,
                defaultBindingMode: BindingMode.OneWay
            );

        public Color RoundedLabelBackgroundColor
        {
            get { return (Color)GetValue(RoundedLabelBackgroundColorProperty); }
            set { SetValue(RoundedLabelBackgroundColorProperty, value); }
        }



        public static BindableProperty RoundedLabelTextColorProperty =
            BindableProperty.Create(
                nameof(RoundedLabelTextColor),
                typeof(Color),
                typeof(RoundedLabel),
                defaultValue: Color.White,
                defaultBindingMode: BindingMode.OneWay
            );

        public Color RoundedLabelTextColor
        {
            get { return (Color)GetValue(RoundedLabelTextColorProperty); }
            set { SetValue(RoundedLabelTextColorProperty, value); }
        }




        public static BindableProperty RoundedLabelTextProperty =
            BindableProperty.Create(
                nameof(RoundedLabelText),
                typeof(string),
                typeof(RoundedLabel),
                defaultValue: "",
                defaultBindingMode: BindingMode.OneWay
            );

        public string RoundedLabelText
        {
            get { return (string)GetValue(RoundedLabelTextProperty); }
            set { SetValue(RoundedLabelTextProperty, value); }
        }




        public static BindableProperty RoundedLabelPaddingProperty =
            BindableProperty.Create(
                nameof(RoundedLabelPadding),
                typeof(Thickness),
                typeof(RoundedLabel),
                defaultValue: new Thickness(6, 0),
                defaultBindingMode: BindingMode.OneWay
            );

        public Thickness RoundedLabelPadding
        {
            get { return (Thickness)GetValue(RoundedLabelPaddingProperty); }
            set { SetValue(RoundedLabelPaddingProperty, value); }
        }



        public static BindableProperty RoundedLabelCornerRadiusProperty =
            BindableProperty.Create(
                nameof(RoundedLabelCornerRadius),
                typeof(Double),
                typeof(RoundedLabel),
                defaultValue: 6.0,
                defaultBindingMode: BindingMode.OneWay
            );

        public Double RoundedLabelCornerRadius
        {
            get { return (Double)GetValue(RoundedLabelCornerRadiusProperty); }
            set { SetValue(RoundedLabelCornerRadiusProperty, value); }
        }



        public static BindableProperty RoundedLabelFontSizeProperty =
            BindableProperty.Create(
                nameof(RoundedLabelFontSize),
                typeof(Double),
                typeof(RoundedLabel),
                defaultValue: 10.0,
                defaultBindingMode: BindingMode.OneWay
            );

        public Double RoundedLabelFontSize
        {
            get { return (Double)GetValue(RoundedLabelFontSizeProperty); }
            set { SetValue(RoundedLabelFontSizeProperty, value); }
        }



        public static BindableProperty RoundedLabelFontAttributesProperty =
            BindableProperty.Create(
                nameof(RoundedLabelFontAttributes),
                typeof(Enum),
                typeof(RoundedLabel),
                defaultValue: null,
                defaultBindingMode: BindingMode.OneWay
            );

        public Enum RoundedLabelFontAttributes
        {
            get { return (Enum)GetValue(RoundedLabelFontAttributesProperty); }
            set { SetValue(RoundedLabelFontAttributesProperty, value); }
        }
    }
}
