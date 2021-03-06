﻿using ICICIMerchant.Common;
using ICICIMerchant.DBHelper;
using ICICIMerchant.Helper;
using ICICIMerchant.Model;
using System;
using System.Threading.Tasks;
using Windows.System;
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
    public sealed partial class LoginView : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public LoginView()
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

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text != string.Empty)
            {
                myLoader.IsActive = true;
                btnLogin.IsEnabled = false;
                txtUsername.IsEnabled = false;
                pwdPassword.IsEnabled = false;
                if (pwdPassword.Password != string.Empty)
                {
                    #region Old code
                    //var result = EncryptionUtility.AES_Encrypt("Jitendra", "Jadav");
                    //var decrypt = EncryptionUtility.AES_Decrypt("dAYV8p7FHE8JjdXx2A6msg==", "Jadav");
                    //var npostData = "grant_type=password&client_id=client1&client_secret=secret&username=" +
                    //    EncryptionUtility.Encrypt(txtUsername.Text, DBHandler.encryptiPass) + "&password=" +
                    //    EncryptionUtility.Encrypt(pwdPassword.Password, DBHandler.encryptiPass) + "&sec=" +
                    //    EncryptionUtility.Encrypt(txtUsername.Text, DBHandler.encryptiPassSec);

                    #endregion

                    string postData = string.Empty;
                    try
                    {
                        postData = "grant_type=password&client_id=client1&client_secret=secret&username="
                                + EncryptionProvider.Encrypt(txtUsername.Text, DBHandler.key1, DBHandler.ivKey)
                                + "&password=" + EncryptionProvider.Encrypt(pwdPassword.Password, DBHandler.key1, DBHandler.ivKey)
                                + "&sec=" + EncryptionProvider.Encrypt(txtUsername.Text, DBHandler.key2, DBHandler.ivKey);
                    }
                    catch (Exception)
                    {
                    }

                    #region Old code
                    //EncryptionHelper objEncryptionHelper = new EncryptionHelper();
                    //var npostData = "grant_type=password&client_id=client1&client_secret=secret&username=" +
                    //    objEncryptionHelper.Encrypt(txtUsername.Text, DBHandler.key1,DBHandler.ivKey) + "&password=" +
                    //    objEncryptionHelper.Encrypt(pwdPassword.Password, DBHandler.key1, DBHandler.ivKey) + "&sec=" +
                    //    objEncryptionHelper.Encrypt(txtUsername.Text, DBHandler.key2, DBHandler.ivKey);
                    //var test1 = EncryptionProvider.Encrypt("62577044", "test");

                    #endregion

                    var loginResponse = await Task.WhenAny(MakeHttpWebRequestPostCall.Generic_Service_Call(postData, DBHandler.url + DBHandler.login_url_paddup, true));
                    LoginModel loginModel = SerializeDeserialize.Deserialize<LoginModel>(loginResponse.Result);

                    if (loginModel != null)
                    {
                        loginModel.TID = txtUsername.Text;
                        loginModel.TPIN = pwdPassword.Password;
                        SuspensionManager.SessionState["loginModel"] = loginModel;
                        Frame.Navigate(typeof(HomeView));
                        myLoader.IsActive = false;
                        btnLogin.IsEnabled = true;
                        txtUsername.IsEnabled = true;
                        pwdPassword.IsEnabled = true;

                    }
                    else
                    {
                        MessageDialog msgDlg = new MessageDialog("Please enter valid TID & TPIN");
                        await msgDlg.ShowAsync();
                        myLoader.IsActive = false;
                        btnLogin.IsEnabled = true;
                        txtUsername.IsEnabled = true;
                        pwdPassword.IsEnabled = true;

                    } 
                }
                else
                {
                    MessageDialog msgDlg = new MessageDialog("Please Enter TPIN");
                    await msgDlg.ShowAsync();
                    myLoader.IsActive = false;
                    btnLogin.IsEnabled = true;
                    txtUsername.IsEnabled = true;
                    pwdPassword.IsEnabled = true;

                }
            }
            else
            {
                MessageDialog msgDlg = new MessageDialog("Please Enter TID");
                await msgDlg.ShowAsync();
                myLoader.IsActive = false;
                btnLogin.IsEnabled = true;
                txtUsername.IsEnabled = true;
                pwdPassword.IsEnabled = true;

            }
        }
    }

}
