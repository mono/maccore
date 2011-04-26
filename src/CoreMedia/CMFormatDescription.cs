// 
// CMFormatDescription.cs: Implements the managed CMFormatDescription
//
// Authors: Mono Team
//     
// Copyright 2010 Novell, Inc
//
using System;
using System.Runtime.InteropServices;

using MonoMac;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreMedia {

	[Since (4,0)]
	public class CMFormatDescription : INativeObject, IDisposable {
		internal IntPtr handle;

		internal CMFormatDescription (IntPtr handle)
		{
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal CMFormatDescription (IntPtr handle, bool owns)
		{
			if (!owns)
				CFObject.CFRetain (handle);

			this.handle = handle;
		}
		
		~CMFormatDescription ()
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
		
		/*[DllImport(Constants.CoreMediaLibrary)]
		extern static CFPropertyListRef CMFormatDescriptionGetExtension (
		   CMFormatDescriptionRef desc,
		   CFStringRef extensionKey
		);*/
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMFormatDescriptionGetExtensions (IntPtr handle);

#if !COREBUILD
		
		public NSDictionary GetExtensions ()
		{
			var cfDictRef = CMFormatDescriptionGetExtensions (handle);
			if (cfDictRef == IntPtr.Zero)
			{
				return null;
			}
			else
			{
				return new NSDictionary (cfDictRef, false);
			}
		}

#endif
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static uint CMFormatDescriptionGetMediaSubType (IntPtr handle);
		
		public uint MediaSubType
		{
			get
			{
				return CMFormatDescriptionGetMediaSubType (handle);
			}
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMMediaType CMFormatDescriptionGetMediaType (IntPtr handle);
		
		public CMMediaType MediaType
		{
			get
			{
				return CMFormatDescriptionGetMediaType (handle);
			}
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static int CMFormatDescriptionGetTypeID ();
		
		public static int GetTypeID ()
		{
			return CMFormatDescriptionGetTypeID ();
		}
	}
}
