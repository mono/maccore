// 
// CMBlockBuffer.cs: Implements the managed CMBlockBuffer
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
	public class CMBlockBuffer : INativeObject, IDisposable {
		internal IntPtr handle;

		internal CMBlockBuffer (IntPtr handle)
		{
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal CMBlockBuffer (IntPtr handle, bool owns)
		{
			if (!owns)
				CFObject.CFRetain (handle);

			this.handle = handle;
		}
		
		~CMBlockBuffer ()
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
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static int CMBlockBufferCopyDataBytes (IntPtr handle, uint offsetToData, uint dataLength, IntPtr destination);
		
		public int CopyDataBytes (uint offsetToData, uint dataLength, IntPtr destination)
		{
			return CMBlockBufferCopyDataBytes (handle, offsetToData, dataLength, destination);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static uint CMBlockBufferGetDataLength (IntPtr handle);
		
		public uint DataLength
		{
			get
			{
				return CMBlockBufferGetDataLength (handle);
			}
		}
	}
}
