using ICICIMerchant.Common;
using ICICIMerchant.DBHelper;
using ICICIMerchant.Helper;
using ICICIMerchant.Model;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ICICIMerchant.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TerminalQueryView : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        string terminalQueryValue = string.Empty;
        string terminalQueryErrorMessageValue = string.Empty;
        public TerminalQueryView()
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

        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string postData = string.Empty;
            if (terminalQueryValue == "Error message")
            {
                try
                {
                    postData = "tid=" + EncryptionProvider.Encrypt(((LoginModel)SuspensionManager.SessionState["loginModel"]).TID, DBHandler.key1, DBHandler.ivKey)
                               + "&type=" + EncryptionProvider.Encrypt("tr", DBHandler.key1, DBHandler.ivKey)
                               + "&origin=" + EncryptionProvider.Encrypt("mobile", DBHandler.key1, DBHandler.ivKey)
                               + "&subType=" + EncryptionProvider.Encrypt(terminalQueryValue, DBHandler.key1, DBHandler.ivKey)
                               + "&emSubType=" + EncryptionProvider.Encrypt(terminalQueryErrorMessageValue, DBHandler.key1, DBHandler.ivKey)
                               + "&alternateNo=" + EncryptionProvider.Encrypt(txtAlternameNo.Text, DBHandler.key1, DBHandler.ivKey)
                               + "&contactPerson=" + EncryptionProvider.Encrypt(txtAlternameName.Text, DBHandler.key1, DBHandler.ivKey)
                               + "&contactAddress=" + EncryptionProvider.Encrypt(txtAddress.Text, DBHandler.key1, DBHandler.ivKey)
                               + "&prefDate=" + EncryptionProvider.Encrypt(dtEMD.Date.ToString(), DBHandler.key1, DBHandler.ivKey)
                               + "&caseDescription=" + EncryptionProvider.Encrypt(txtIssueDescription.Text, DBHandler.key1, DBHandler.ivKey);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                try
                {
                    postData = "tid=" + EncryptionProvider.Encrypt(((LoginModel)SuspensionManager.SessionState["loginModel"]).TID, DBHandler.key1, DBHandler.ivKey)
                             + "&type=" + EncryptionProvider.Encrypt("tr", DBHandler.key1, DBHandler.ivKey)
                             + "&origin=" + EncryptionProvider.Encrypt("mobile", DBHandler.key1, DBHandler.ivKey)
                             + "&subType=" + EncryptionProvider.Encrypt(terminalQueryValue, DBHandler.key1, DBHandler.ivKey)
                             + "&alternateNo=" + EncryptionProvider.Encrypt(txtAlternameNo.Text, DBHandler.key1, DBHandler.ivKey)
                             + "&contactPerson=" + EncryptionProvider.Encrypt(txtAlternameName.Text, DBHandler.key1, DBHandler.ivKey)
                             + "&contactAddress=" + EncryptionProvider.Encrypt(txtAddress.Text, DBHandler.key1, DBHandler.ivKey)
                             + "&prefDate=" + EncryptionProvider.Encrypt(dtEMD.Date.ToString(), DBHandler.key1, DBHandler.ivKey)
                             + "&caseDescription=" + EncryptionProvider.Encrypt(txtIssueDescription.Text, DBHandler.key1, DBHandler.ivKey);
                }
                catch (Exception)
                {
                }
            }

            var terminalQueryResult = await Task.WhenAny(MakeHttpWebRequestPostCall.Generic_Service_Call(postData, DBHandler.url + DBHandler.general_url_paddup,false));
            MessageDialog msgDlg = new MessageDialog("Result is " + terminalQueryResult.Result);
            await msgDlg.ShowAsync();
           
            terminalQueryErrorMessageValue = string.Empty;
            terminalQueryValue = string.Empty;
        }

        private void cmbTerminalQuery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (((ComboBoxItem)e.AddedItems[0]).Content.ToString().ToLower() == "Error message".ToLower())
                {
                    cmbTerminalQueryErrorMessage.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                else
                {
                    cmbTerminalQueryErrorMessage.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                terminalQueryValue = ((ComboBoxItem)e.AddedItems[0]).Content.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void cmbTerminalQueryErrorMessage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                terminalQueryErrorMessageValue = ((ComboBoxItem)e.AddedItems[0]).Content.ToString();
            }
            catch (Exception)
            {
            }
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
