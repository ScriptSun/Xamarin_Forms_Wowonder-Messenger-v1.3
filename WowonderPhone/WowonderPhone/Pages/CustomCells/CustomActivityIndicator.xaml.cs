using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WowonderPhone.Pages.CustomCells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomActivityIndicator : ContentView
    {
        public CustomActivityIndicator()
        {
            InitializeComponent();
            spinY(Wrapper, 1000);
        }

        static void spinY(View child, uint duration)
        {
            var animation = new Animation(
                callback: d => child.RotationY = d,
                start: 0,
                end: 360,
                easing: Easing.Linear);

            animation.Commit(child, "Loop", length: duration, repeat: () => true);
        }
    }
}
