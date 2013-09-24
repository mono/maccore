//
// Copyright 2012 Xamarin
//
using System;
using System.Runtime.InteropServices;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreFoundation {
	public abstract class CFType : INativeObject, IDisposable
	{
		protected CFType ()
		{
		}

		[Preserve (Conditional = true)]
		internal CFType (IntPtr handle, bool owns)
		{
			if (handle == IntPtr.Zero)
				throw new ArgumentNullException ("handle");
			if (!owns)
				Retain (handle);
			Handle = handle;
		}

		public IntPtr Handle { get; internal set; }

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static uint CFGetTypeID (IntPtr typeRef);

		public static uint GetTypeID (IntPtr handle) {
			return CFGetTypeID (handle);
		}

		public uint GetTypeID () {
			return CFGetTypeID (Handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFCopyDescription (IntPtr ptr);


		public string Description {
			get {
				ThrowIfDisposed ();
				using (var s = new CFString (CFCopyDescription (Handle)))
					return s.ToString ();
			}
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected virtual void Dispose (bool disposing)
		{
			if (Handle != IntPtr.Zero){
				Release (Handle);
				Handle = IntPtr.Zero;
			}
		}

		protected void ThrowIfDisposed ()
		{
			if (Handle == IntPtr.Zero)
				throw new ObjectDisposedException (GetType ().Name);
		}

		[DllImport (Constants.CoreFoundationLibrary, EntryPoint = "CFRelease")]
		internal extern static void Release (IntPtr obj);

		[DllImport (Constants.CoreFoundationLibrary, EntryPoint = "CFRetain")]
		internal extern static IntPtr Retain (IntPtr obj);
	}
}
