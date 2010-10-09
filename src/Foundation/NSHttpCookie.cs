using MonoMac.ObjCRuntime;
using System;

namespace MonoMac.Foundation {
	public partial class NSHttpCookie {
		public static NSString KeyName;
		public static NSString KeyValue;
		public static NSString KeyOriginURL;
		public static NSString KeyVersion;
		public static NSString KeyDomain;
		public static NSString KeyPath;
		public static NSString KeySecure;
		public static NSString KeyExpires;
		public static NSString KeyComment;
		public static NSString KeyCommentURL;
		public static NSString KeyDiscard;
		public static NSString KeyMaximumAge;
		public static NSString KeyPort;

		static NSHttpCookie ()
		{
			var handle = Dlfcn.dlopen (Constants.FoundationLibrary, 0);
			if (handle == IntPtr.Zero)
				return;

			try {
				KeyName = Dlfcn.GetStringConstant (handle, "NSHTTPCookieName");
				KeyValue = Dlfcn.GetStringConstant (handle, "NSHTTPCookieValue");
				KeyOriginURL = Dlfcn.GetStringConstant (handle, "NSHTTPCookieOriginURL");
				KeyVersion = Dlfcn.GetStringConstant (handle, "NSHTTPCookieVersion");
				KeyDomain = Dlfcn.GetStringConstant (handle, "NSHTTPCookieDomain");
				KeyPath = Dlfcn.GetStringConstant (handle, "NSHTTPCookiePath");
				KeySecure = Dlfcn.GetStringConstant (handle, "NSHTTPCookieSecure");
				KeyExpires = Dlfcn.GetStringConstant (handle, "NSHTTPCookieExpires");
				KeyComment = Dlfcn.GetStringConstant (handle, "NSHTTPCookieComment");
				KeyCommentURL = Dlfcn.GetStringConstant (handle, "NSHTTPCookieCommentURL");
				KeyDiscard = Dlfcn.GetStringConstant (handle, "NSHTTPCookieDiscard");
				KeyMaximumAge = Dlfcn.GetStringConstant (handle, "NSHTTPCookieMaximumAge");
				KeyPort = Dlfcn.GetStringConstant (handle, "NSHTTPCookiePort");
			} finally {
				Dlfcn.dlclose (handle);
			}
		}
	}
}