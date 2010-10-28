using System;
using System.Runtime.InteropServices;

namespace MonoMac.CoreFoundation {
	public class CFType {
		[DllImport (Constants.CoreFoundationLibrary, EntryPoint="CFGetTypeID")]
		public static extern int GetTypeID (IntPtr typeRef);
	}
}