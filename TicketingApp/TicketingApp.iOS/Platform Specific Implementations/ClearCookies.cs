using Foundation;
using TicketingApp.Services;

namespace TicketingApp.iOS.Platform_Specific_Implementations
{
    public class ClearCookies : IClearCookies
    {
        public void ClearAllCookies()
        {
            NSHttpCookieStorage CookieStorage = NSHttpCookieStorage.SharedStorage;
            foreach (var cookie in CookieStorage.Cookies)
                CookieStorage.DeleteCookie(cookie);
        }
    }
}