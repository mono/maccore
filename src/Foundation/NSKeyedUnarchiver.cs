using System;
using System.Runtime.InteropServices;
using MonoMac.CoreFoundation;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {

	public partial class NSKeyedUnarchiver {

		public static void GlobalSetClass (Class kls, string codedName)
		{
			if (codedName == null)
				throw new ArgumentNullException ("codedName");
			if (kls == null)
				throw new ArgumentNullException ("kls");
			
			using (var nsname = new NSString (codedName))
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (class_ptr, selSetClassForClassName_, kls.Handle, nsname.Handle);
		}

		public static Class GlobalGetClass (string codedName)
		{
			if (codedName == null)
				throw new ArgumentNullException ("codedName");
			using (var nsname = new NSString (codedName))
				return new Class (
						MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (
							class_ptr, selClassForClassName_, nsname.Handle));
		}

	}
}
