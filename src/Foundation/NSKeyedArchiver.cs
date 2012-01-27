using System;
using System.Runtime.InteropServices;
using MonoMac.CoreFoundation;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {

	public partial class NSKeyedArchiver {

		public static void GlobalSetClassName (string name, Class kls)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			if (kls == null)
				throw new ArgumentNullException ("kls");

			var nsname = new NSString (name);
			MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (class_ptr, selSetClassNameForClass_, nsname.Handle, kls.Handle);
			nsname.Dispose ();
		}

		public static string GlobalGetClassName (Class kls)
		{
			if (kls == null)
				throw new ArgumentNullException ("kls");
			return NSString.FromHandle (MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (class_ptr, selClassNameForClass_, kls.Handle));
		}

	}
}
