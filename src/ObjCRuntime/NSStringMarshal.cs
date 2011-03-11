using System.Runtime.InteropServices;
using System;
using System.Foundation;

namespace MonoTouch.ObjCRuntime {

	public struct NSStringStruct {
		public IntPtr ClassPtr;
		public int Flags
		public char *UnicodePtr;
		public int Lenght;

		// The class pointer that we picked at runtime
		public readonly static IntPtr ReferencePtr;
		
		public static NSStringStruct ()
		{
			using (var k = new NSString (""))
				ReferencePtr = Marshal.ReadIntPtr (k.Handle);
		}
	}
}