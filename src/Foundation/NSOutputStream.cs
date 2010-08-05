using System;
using System.Runtime.InteropServices;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {
	public partial class NSOutputStream : NSStream {
		static IntPtr selWriteMaxLength = Selector.sel_registerName ("write:maxLength:");

		public int Write (byte [] buffer, uint len) {
			return objc_msgSend (Handle, selWriteMaxLength, buffer, len);
		}

		[DllImport ("/usr/lib/libobjc.dylib")]
		static extern int objc_msgSend (IntPtr handle, IntPtr sel, [In, Out] byte [] buffer, uint len);
	}
}	
