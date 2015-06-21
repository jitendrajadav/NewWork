using ICICIMerchant.Common;
using ICICIMerchant.DBHelper;
using ICICIMerchant.Helper;
using ICICIMerchant.Model;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ICICIMerchant.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StatementRequestView : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public StatementRequestView()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void cmbStatementType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            grdLastStatement.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            grdStatementOfDateRange.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            grdPrintedStatement.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            string value = ((ComboBoxItem)e.AddedItems[0]).Content.ToString();
            switch (value)
            {
                case "Last Statemtement":
                    grdLastStatement.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    break;
                case "Statement of a Date Range":
                    grdStatementOfDateRange.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    break;
                case "Printed Statement":
                    grdPrintedStatement.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private async void btnLastStatement_Click(object sender, RoutedEventArgs e)
        {
            string postData = string.Empty;

            try
            {
                postData = "tid=" + EncryptionProvider.Encrypt(((LoginModel)SuspensionManager.SessionState["loginModel"]).TID, DBHandler.key1, DBHandler.ivKey)
                            + "&type=" + EncryptionProvider.Encrypt("st", DBHandler.key1, DBHandler.ivKey)
                            + "&origin=" + EncryptionProvider.Encrypt("mobile", DBHandler.key1, DBHandler.ivKey)
                            + "&subType=" + EncryptionProvider.Encrypt("LS", DBHandler.key1, DBHandler.ivKey);
            }
            catch (Exception)
            {
            }

            var lastStatement = await Task.WhenAny(MakeHttpWebRequestPostCall.Generic_Service_Call(postData, DBHandler.url + DBHandler.general_url_paddup, false));
            MessageDialog md = new MessageDialog("Your request is registered with reference number: " + lastStatement.Result + ".This will be processed by " + DateTime.Today.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern) + "\nDo you want to give your feedback ", "Feedback");
            bool? result = null;
            md.Commands.Add(
               new UICommand("Yes", new UICommandInvokedHandler((cmd) => result = true)));
            md.Commands.Add(
               new UICommand("No", new UICommandInvokedHandler((cmd) => result = false)));

            await md.ShowAsync();
            if (result == true)
            {
                Popup nonParentPopup = null;
                // if we already have one showing, don't create another one
                if (nonParentPopup == null)
                {
                    // create the Popup in code
                    nonParentPopup = new Popup();

                    // we are creating this in code and need to handle multiple instances
                    // so we are attaching to the Popup.Closed event to remove our reference
                    nonParentPopup.Closed += (senderPopup, argsPopup) =>
                    {
                        nonParentPopup = null;
                    };
                    nonParentPopup.HorizontalOffset = 0;
                    nonParentPopup.VerticalOffset = Window.Current.Bounds.Height / 4;

                    // set the content to our RateView
                    nonParentPopup.Child = new RateView();

                    // open the Popup
                    nonParentPopup.IsOpen = true;
                }

            }
        }

        private async void btnStatementOfDateRange_Click(object sender, RoutedEventArgs e)
        {
            string postData = string.Empty;

            try
            {
                postData = "tid=" + EncryptionProvider.Encrypt(((LoginModel)SuspensionManager.SessionState["loginModel"]).TID, DBHandler.key1, DBHandler.ivKey)
                            + "&type=" + EncryptionProvider.Encrypt("st", DBHandler.key1, DBHandler.ivKey)
                            + "&origin=" + EncryptionProvider.Encrypt("mobile", DBHandler.key1, DBHandler.ivKey)
                            + "&subType=" + EncryptionProvider.Encrypt("SODR", DBHandler.key1, DBHandler.ivKey)
                            + "&fromDate=" + EncryptionProvider.Encrypt(dtSODRFrom.Date.ToString(), DBHandler.key1, DBHandler.ivKey)
                            + "&toDate=" + EncryptionProvider.Encrypt(dtSODRTo.Date.ToString(), DBHandler.key1, DBHandler.ivKey);
            }
            catch (Exception)
            {
            }

            var lastStatement = await Task.WhenAny(MakeHttpWebRequestPostCall.Generic_Service_Call(postData, DBHandler.url + DBHandler.general_url_paddup,false));
            MessageDialog msgDlg = new MessageDialog("Result is " + lastStatement.Result);
            await msgDlg.ShowAsync();
        }

        private async void btnPrintedStatement_Click(object sender, RoutedEventArgs e)
        {
            string postData = string.Empty;

            try
            {
                postData = "tid=" + EncryptionProvider.Encrypt(((LoginModel)SuspensionManager.SessionState["loginModel"]).TID, DBHandler.key1, DBHandler.ivKey)
                            + "&type=" + EncryptionProvider.Encrypt("st", DBHandler.key1, DBHandler.ivKey)
                            + "&origin=" + EncryptionProvider.Encrypt("mobile", DBHandler.key1, DBHandler.ivKey)
                            + "&subType=" + EncryptionProvider.Encrypt("PS", DBHandler.key1, DBHandler.ivKey)
                            + "&fromDate=" + EncryptionProvider.Encrypt(dtPSFrom.Date.ToString(), DBHandler.key1, DBHandler.ivKey)
                            + "&toDate=" + EncryptionProvider.Encrypt(dtPSTo.Date.ToString(), DBHandler.key1, DBHandler.ivKey);
            }
            catch (Exception)
            {
            }

            var lastStatement = await Task.WhenAny(MakeHttpWebRequestPostCall.Generic_Service_Call(postData, DBHandler.url + DBHandler.general_url_paddup,false));
            MessageDialog msgDlg = new MessageDialog("LS is " + lastStatement.Result);
            await msgDlg.ShowAsync();
        }

        private void btnHome_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HomeView));
        }

        private async void btnLogOut_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string postData = string.Empty;
            MessageDialog md = new MessageDialog("Are you sure you want to Logout ?", "Message");
            bool? result = null;
            md.Commands.Add(
               new UICommand("Yes", new UICommandInvokedHandler((cmd) => result = true)));
            md.Commands.Add(
               new UICommand("No", new UICommandInvokedHandler((cmd) => result = false)));

            await md.ShowAsync();
            if (result == true)
            {
                // do something   
                try
                {
                    postData = "tid=" + EncryptionProvider.Encrypt(((LoginModel)SuspensionManager.SessionState["loginModel"]).TID, DBHandler.key1, DBHandler.ivKey)
                            + "&type=" + EncryptionProvider.Encrypt("trm", DBHandler.key1, DBHandler.ivKey)
                            + "&origin=" + EncryptionProvider.Encrypt("mobile", DBHandler.key1, DBHandler.ivKey);
                }
                catch (Exception)
                {
                }

                var logOutResult = await Task.WhenAny(MakeHttpWebRequestPostCall.Generic_Service_Call(postData, DBHandler.url + DBHandler.general_url_paddup, false));
                if (logOutResult.Result != string.Empty)
                {
                    Frame.Navigate(typeof(LoginView));
                }
            }
        }
    }
}
