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
            string expiryDate = "";

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < cookies.Length; i++)
            {
                builder.Append(cookies[i].Name.ToString() + "=" + cookies[i].Value);

                if (cookies[i].Name.ToString().Equals("rtFa"))
                    expiryDate = ";CookieExpire=" + cookies[i].ExpiresDate.ToString();

                if (cookies.Length - 1 != i)
                    builder.Append(";");
                else
                    builder.Append(expiryDate);
            }

            return builder.ToString();
        }
    }
}