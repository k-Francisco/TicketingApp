using Foundation;
using System.Text;
using TicketingApp.Services;

namespace TicketingApp.iOS.Platform_Specific_Implementations
{
    public class GrabCookie : ICookieGrab
    {
        public string GrabCookies(string url)
        {
            NSHttpCookieStorage cookieStorage = NSHttpCookieStorage.SharedStorage;
            var cookies = cookieStorage.CookiesForUrl(new NSUrl(url));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < cookies.Length; i++)
            {
                builder.Append(cookies[i].Name.ToString() + "=" + cookies[i].Value);

                if (cookies.Length - 1 != i)
                    builder.Append(";");
            }

            return builder.ToString();
        }
    }
}