using Android.Webkit;
using TicketingApp.Droid.Platform_Specific_Implementations;
using TicketingApp.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(GrabCookie))]
namespace TicketingApp.Droid.Platform_Specific_Implementations
{
    public class GrabCookie : ICookieGrab
    {
        public string GrabCookies(string url)
        {
            CookieManager cookieManager = CookieManager.Instance;
            cookieManager.SetAcceptCookie(true);

            return cookieManager.GetCookie(url);
        }
    }
}