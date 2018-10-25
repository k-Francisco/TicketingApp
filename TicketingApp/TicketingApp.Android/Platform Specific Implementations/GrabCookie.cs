using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
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