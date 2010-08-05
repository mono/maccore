using MonoMac.ObjCRuntime;
using System;

namespace MonoMac.Foundation {
	public partial class NSHttpCookieStorage {
		public static NSString CookiesChangedNotification;
		public static NSString AcceptPolicyChangedNotification;

		static NSHttpCookieStorage ()
		{
			var handle = Dlfcn.dlopen (Constants.FoundationLibrary, 0);
			if (handle == IntPtr.Zero)
				return;

			try {
				CookiesChangedNotification = Dlfcn.GetStringConstant (handle, "NSHTTPCookieManagerAcceptPolicyChangedNotification");
				AcceptPolicyChangedNotification = Dlfcn.GetStringConstant (handle, "NSHTTPCookieManagerCookiesChangedNotification");
			} finally {
				Dlfcn.dlclose (handle);
			}
		}
	}
}