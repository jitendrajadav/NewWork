using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ICICIMerchant.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RateView : Page
    {
        public RateView()
        {
            this.InitializeComponent();
            this.Height = Window.Current.Bounds.Height / 2;
            this.Width = Window.Current.Bounds.Width;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void myRatingControl_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (myRatingControl != null)
                {
                    var value = myRatingControl.Value;
                }
            }
            catch (Exception)
            {
            }
        }

        private async void btnSubmitFeedback_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog msgDlg = new MessageDialog("Thank you for your feedback!");
            await msgDlg.ShowAsync();

        }

        private void btnCancelFeedback_Click(object sender, RoutedEventArgs e)
        {
            if (((Popup)((RateView)(((Grid)((ButtonBase)(sender)).Parent).Parent)).Parent) != null)
                ((Popup)((RateView)(((Grid)((ButtonBase)(sender)).Parent).Parent)).Parent).IsOpen = false;

        }
    }
}
