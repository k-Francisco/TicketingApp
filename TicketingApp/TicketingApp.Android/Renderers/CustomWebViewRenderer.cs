using Android.Content;
using Android.OS;
using Android.Webkit;
using Java.Lang;
using TicketingApp.CustomControls;
using TicketingApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(CustomWebView),typeof(CustomWebViewRenderer))]
namespace TicketingApp.Droid.Renderers
{
    public class CustomWebViewRenderer : WebViewRenderer, IValueCallback
    {
        public CustomWebViewRenderer(Context context) : base(context)
        {
        }

        public void OnReceiveValue(Object value)
        {
            System.Diagnostics.Debug.WriteLine("onreceivevalue",value.ToString());
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            Android.Webkit.WebView webView = Control;
            if (webView == null)
                return;

            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
            {
                CookieManager.Instance.RemoveAllCookie();
            }
            else
            {
                CookieManager.Instance.RemoveAllCookies(this);
            }

            webView.ClearCache(true);
            webView.ClearHistory();
            webView.ClearFormData();
            webView.ClearMatches();
        }
    }
}