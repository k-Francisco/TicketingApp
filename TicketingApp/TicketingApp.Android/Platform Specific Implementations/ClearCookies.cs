using Android.Webkit;
using TicketingApp.Services;

namespace TicketingApp.Droid.Platform_Specific_Implementations
{
    public class ClearCookies : IClearCookies
    {
        public void ClearAllCookies()
        {
            var cookieManager = CookieManager.Instance;
            cookieManager.RemoveAllCookie();
        }
    }
}