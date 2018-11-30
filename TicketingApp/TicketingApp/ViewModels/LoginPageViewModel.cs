using Acr.UserDialogs;
using Prism.AppModel;
using Prism.Commands;
using Prism.Navigation;
using SpevoCore.Services.Token_Service;
using System;
using TicketingApp.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TicketingApp.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly ICookieGrab _grabCookieService;
        private readonly ITokenService _tokenService;

        private string _loginSiteUrl;
        public string LoginSiteUrl {
            get { return _loginSiteUrl; }
            set { SetProperty(ref _loginSiteUrl, value); }
        }

        private bool _doneSavingCookies { get; set; }

        public LoginPageViewModel(INavigationService navigationService, ICookieGrab grabCookieService, ITokenService tokenService)
            : base(navigationService,null,null)
        {
            _grabCookieService = grabCookieService;
            _tokenService = tokenService;
            LoginSiteUrl = App.SiteUrl;

            Connectivity.ConnectivityChanged += ConnectionChanged;
        }

        private void ConnectionChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (IsConnected)
            {
                LoginSiteUrl = "";
                LoginSiteUrl = App.SiteUrl;
            }
            else
            {
                PageDialog.Alert("Please connect your device to the internet", "No Connection", "OK");
            }
        }

        private DelegateCommand<WebNavigatingEventArgs> _webNavigatingCommand;
        public DelegateCommand<WebNavigatingEventArgs> WebNavigatingCommand
        {
            get
            {
                if (_webNavigatingCommand == null)
                {
                    _webNavigatingCommand = new DelegateCommand<WebNavigatingEventArgs>(async (e) =>
                    {
                        PageDialog.ShowLoading("", MaskType.Black);
                        if(Connectivity.NetworkAccess == NetworkAccess.Internet)
                        {
                            if (!_doneSavingCookies)
                            {
                                var cookies = _grabCookieService.GrabCookies(LoginSiteUrl);
                                var domain = LoginSiteUrl.Replace("https://", "").Split(new char[] { '/' });
                                _doneSavingCookies = _tokenService.SaveCookies(cookies, domain[0]);
                            }
                            else
                            {
                                await NavigationService.NavigateAsync(new Uri("app:///NavigationPage/TicketsPage", UriKind.Absolute));
                            }
                            PageDialog.HideLoading();
                        }
                        else
                        {
                            e.Cancel = true;
                            PageDialog.HideLoading();
                            PageDialog.Alert("Please connect your device to the internet", "No Connection", "OK");
                        }
                        
                    });
                }

                return _webNavigatingCommand;
            }
        }
    }
}