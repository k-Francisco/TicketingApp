package md5e0cf8bcb380589ab6d369eb064aed445;


public class CustomWebViewRenderer
	extends md51558244f76c53b6aeda52c8a337f2c37.WebViewRenderer
	implements
		mono.android.IGCUserPeer,
		android.webkit.ValueCallback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onReceiveValue:(Ljava/lang/Object;)V:GetOnReceiveValue_Ljava_lang_Object_Handler:Android.Webkit.IValueCallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("TicketingApp.Droid.Renderers.CustomWebViewRenderer, TicketingApp.Android", CustomWebViewRenderer.class, __md_methods);
	}


	public CustomWebViewRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == CustomWebViewRenderer.class)
			mono.android.TypeManager.Activate ("TicketingApp.Droid.Renderers.CustomWebViewRenderer, TicketingApp.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public CustomWebViewRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == CustomWebViewRenderer.class)
			mono.android.TypeManager.Activate ("TicketingApp.Droid.Renderers.CustomWebViewRenderer, TicketingApp.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public CustomWebViewRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == CustomWebViewRenderer.class)
			mono.android.TypeManager.Activate ("TicketingApp.Droid.Renderers.CustomWebViewRenderer, TicketingApp.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public void onReceiveValue (java.lang.Object p0)
	{
		n_onReceiveValue (p0);
	}

	private native void n_onReceiveValue (java.lang.Object p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
