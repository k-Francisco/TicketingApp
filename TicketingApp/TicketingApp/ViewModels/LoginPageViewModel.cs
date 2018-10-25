using Prism.Commands;
using Prism.Navigation;
using SpevoCore.Services.Token_Service;
using System;
using TicketingApp.Services;
using Xamarin.Forms;

namespace TicketingApp.ViewModels
{
    public class LoginPageViewModel
    {
        private readonly ICookieGrab _grabCookieService;
        private readonly ITokenService _tokenService;
        private readonly INavigationService _navigationService;

        public string LoginSiteUrl {
            get { return App.SiteUrl; }
        }

        private bool _doneSavingCookies { get; set; }

        public LoginPageViewModel(INavigationService navigationService, ICookieGrab grabCookieService, ITokenService tokenService)
        {
            _grabCookieService = grabCookieService;
            _tokenService = tokenService;
            _navigationService = navigationService;
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
                        if (!_doneSavingCookies)
                        {
                            var cookies = _grabCookieService.GrabCookies(LoginSiteUrl);
                            var domain = LoginSiteUrl.Replace("https://","").Split(new char[] { '/' });
                            _doneSavingCookies = _tokenService.SaveCookies(cookies, domain[0]);
                        }
                        else
                        {
                            await _navigationService.NavigateAsync(new Uri("app:///NavigationPage/TicketsPage", UriKind.Absolute));
                            
                        }
                        
                    });
                }

                return _webNavigatingCommand;
            }
        }
    }
}