//
// Copyright 2012 Xamarin
//
using System;
using System.Runtime.InteropServices;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreFoundation {
	public class CFType {
		[DllImport (Constants.CoreFoundationLibrary, EntryPoint="CFGetTypeID")]
		public static extern int GetTypeID (IntPtr typeRef);
	}

	public interface ICFType : INativeObject {
	}
}