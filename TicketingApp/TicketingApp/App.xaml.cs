using Prism;
using Prism.Ioc;
using SpevoCore.Services;
using SpevoCore.Services.API_Service;
using SpevoCore.Services.Sharepoint_API;
using SpevoCore.Services.Token_Service;
using TicketingApp.ViewModels;
using TicketingApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TicketingApp
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public const string SiteUrl = "https://dataoutsource.sharepoint.com/sites/ticketing";
        
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            if (TokenService.GetInstance.IsAlreadyLoggedIn())
                await NavigationService.NavigateAsync("NavigationPage/TicketsPage");
            else
                await NavigationService.NavigateAsync("LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //views and viewmodels
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<TicketsPage, TicketsPageViewModel>();
            containerRegistry.RegisterForNavigation<InvoicePage, InvoicePageViewModel>();
            containerRegistry.RegisterForNavigation<TicketDetailsPage, TicketDetailsPageViewModel>();

            //instances
            containerRegistry.RegisterInstance<ITokenService>(TokenService.GetInstance);
            containerRegistry.RegisterInstance<IApiManager>(ApiManager.GetInstance);
        }
    }
}
