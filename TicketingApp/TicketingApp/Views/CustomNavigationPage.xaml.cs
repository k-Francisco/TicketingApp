using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace TicketingApp.Views
{
    public partial class CustomNavigationPage : Xamarin.Forms.NavigationPage
    {
        public CustomNavigationPage()
        {
            InitializeComponent();
            On<iOS>().SetPrefersLargeTitles(true);
        }
    }
}
