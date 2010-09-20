using System;
using System.Drawing;
using System.Runtime.InteropServices;

using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
using MonoMac.CoreGraphics;

namespace MonoMac.ImageIO {
	
	public class CGImageSource : INativeObject, IDisposable
	{
		internal IntPtr handle;

		// invoked by marshallers
		internal CGImageSource (IntPtr handle)
			: this (handle, false)
		{
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal CGImageSource (IntPtr handle, bool owns)
		{
			this.handle = handle;
			if (!owns)
				CFObject.CFRetain (handle);
		}
		
		~CGImageSource ()
		{
			Dispose (false);
		}
		
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public IntPtr Handle {
			get { return handle; }
		}
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CFObject.CFRelease (handle);
				handle = IntPtr.Zero;
			}
		}
				
		[DllImport (Constants.IOImageLibrary)]
		extern static IntPtr CGImageSourceCreateWithURL(IntPtr url, IntPtr options);
		public static CGImageSource CreateWithURL (CFUrl url, IntPtr options)
		{
			return new CGImageSource(CGImageSourceCreateWithURL(url.Handle, options));
		}
		
		[DllImport (Constants.IOImageLibrary)]
		extern static IntPtr CGImageSourceCreateImageAtIndex(IntPtr isrc, int index, IntPtr options);
		public static CGImage CreateImageAtIndex (CGImageSource isrc, int index, IntPtr options)
		{
			return new CGImage(CGImageSourceCreateImageAtIndex(isrc.Handle, index, options));
		}
	}
}