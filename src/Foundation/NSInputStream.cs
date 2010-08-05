using System;
using System.Runtime.InteropServices;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {
	public partial class NSInputStream : NSStream {
		static IntPtr selReadMaxLength = Selector.sel_registerName ("read:maxLength:");

		public int Read (byte [] buffer, uint len) {
			return objc_msgSend (Handle, selReadMaxLength, buffer, len);
		}

		[DllImport ("/usr/lib/libobjc.dylib")]
		static extern int objc_msgSend (IntPtr handle, IntPtr sel, [In, Out] byte [] buffer, uint len);
	}
}	
