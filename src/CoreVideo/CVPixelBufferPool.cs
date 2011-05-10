// 
// CVPixelBufferPool.cs: Implements the managed CVPixelBufferPool
//
// Authors: Mono Team
//     
// Copyright 2010 Novell, Inc
//
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;

namespace MonoMac.CoreVideo {

	[Since (4,0)]
	public class CVPixelBufferPool : INativeObject, IDisposable {
		public static readonly NSString MinimumBufferCountKey;
		public static readonly NSString MaximumBufferAgeKey;

		static CVPixelBufferPool ()
		{
			var handle = Dlfcn.dlopen (Constants.CoreVideoLibrary, 0);
			if (handle == IntPtr.Zero)
				return;
			try {
				MinimumBufferCountKey = Dlfcn.GetStringConstant (handle, "kCVPixelBufferPoolMinimumBufferCountKey");
				MaximumBufferAgeKey = Dlfcn.GetStringConstant (handle, "kCVPixelBufferPoolMaximumBufferAgeKey");
			}
			finally {
				Dlfcn.dlclose (handle);
			}
		}

		IntPtr handle;

		internal CVPixelBufferPool (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				throw new Exception ("Invalid parameters to context creation");

			CVPixelBufferPoolRetain (handle);
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal CVPixelBufferPool (IntPtr handle, bool owns)
		{
			if (!owns)
				CVPixelBufferPoolRetain (handle);

			this.handle = handle;
		}

		~CVPixelBufferPool ()
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
	
		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVPixelBufferPoolRelease (IntPtr handle);
		
		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVPixelBufferPoolRetain (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CVPixelBufferPoolRelease (handle);
				handle = IntPtr.Zero;
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static int CVPixelBufferPoolGetTypeID ();
		public int TypeID {
			get {
				return CVPixelBufferPoolGetTypeID ();
			}
		}

#if !COREBUILD
		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVPixelBufferPoolGetPixelBufferAttributes (IntPtr pool);
		public NSDictionary PixelBufferAttributes {
			get {
				return (NSDictionary) Runtime.GetNSObject (CVPixelBufferPoolGetPixelBufferAttributes (handle));
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVPixelBufferPoolGetAttributes (IntPtr pool);
		public NSDictionary Attributes {
			get {
				return (NSDictionary) Runtime.GetNSObject (CVPixelBufferPoolGetAttributes (handle));
			}
		}
#endif

		[DllImport (Constants.CoreVideoLibrary)]
		extern static CVReturn CVPixelBufferPoolCreatePixelBuffer (IntPtr allocator, IntPtr pixelBufferPool, IntPtr pixelBufferOut);
		public CVPixelBuffer CreatePixelBuffer ()
		{
			IntPtr pixelBufferOut = Marshal.AllocHGlobal (Marshal.SizeOf (typeof (IntPtr)));
			CVReturn ret = CVPixelBufferPoolCreatePixelBuffer (IntPtr.Zero, handle, pixelBufferOut);

			if (ret != CVReturn.Success) {
				Marshal.FreeHGlobal (pixelBufferOut);
				throw new Exception ("CVPixelBufferPoolCreatePixelBuffer returned " + ret.ToString ());
			}

			CVPixelBuffer pixelBuffer = new CVPixelBuffer (Marshal.ReadIntPtr (pixelBufferOut));
			Marshal.FreeHGlobal (pixelBufferOut);
			return pixelBuffer;
		}

#if !COREBUILD
		[DllImport (Constants.CoreVideoLibrary)]
		extern static CVReturn CVPixelBufferPoolCreate (IntPtr allocator, IntPtr poolAttributes, IntPtr pixelBufferAttributes, IntPtr poolOut);
		public CVPixelBufferPool (NSDictionary poolAttributes, NSDictionary pixelBufferAttributes)
		{
			IntPtr poolOut = Marshal.AllocHGlobal (Marshal.SizeOf (typeof (IntPtr)));
			CVReturn ret = CVPixelBufferPoolCreate (IntPtr.Zero, poolAttributes.Handle, pixelBufferAttributes.Handle, poolOut);

			if (ret != CVReturn.Success) {
				Marshal.FreeHGlobal (poolOut);
				throw new Exception ("CVPixelBufferPoolCreate returned " + ret.ToString ());
			}

			this.handle = Marshal.ReadIntPtr (poolOut);
			Marshal.FreeHGlobal (poolOut);
		}
#endif
	}
}
