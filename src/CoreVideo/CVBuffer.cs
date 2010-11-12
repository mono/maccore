// 
// CVBuffer.cs: Implements the managed CVBuffer
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
	public class CVBuffer : INativeObject, IDisposable {
		public static readonly NSString MovieTimeKey;
		public static readonly NSString TimeValueKey;
		public static readonly NSString TimeScaleKey;
		public static readonly NSString PropagatedAttachmentsKey;
		public static readonly NSString NonPropagatedAttachmentsKey;

		static CVBuffer ()
		{
			var handle = Dlfcn.dlopen (Constants.CoreVideoLibrary, 0);
			if (handle == IntPtr.Zero)
				return;
			try {
				MovieTimeKey = Dlfcn.GetStringConstant (handle, "kCVBufferMovieTimeKey");
				TimeValueKey = Dlfcn.GetStringConstant (handle, "kCVBufferTimeValueKey");
				TimeScaleKey = Dlfcn.GetStringConstant (handle, "kCVBufferTimeScaleKey");
				PropagatedAttachmentsKey = Dlfcn.GetStringConstant (handle, "kCVBufferPropagatedAttachmentsKey");
				NonPropagatedAttachmentsKey = Dlfcn.GetStringConstant (handle, "kCVBufferNonPropagatedAttachmentsKey");
			}
			finally {
				Dlfcn.dlclose (handle);
			}
		}

		IntPtr handle;

		internal CVBuffer ()
		{
		}

		internal CVBuffer (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				throw new Exception ("Invalid parameters to context creation");

			CVBufferRetain (handle);
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal CVBuffer (IntPtr handle, bool owns)
		{
			if (!owns)
				CVBufferRetain (handle);

			this.handle = handle;
		}

		~CVBuffer ()
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
		extern static void CVBufferRelease (IntPtr handle);
		
		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVBufferRetain (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CVBufferRelease (handle);
				handle = IntPtr.Zero;
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVBufferRemoveAllAttachments (IntPtr buffer);
		public void RemoveAllAttachments ()
		{
			CVBufferRemoveAllAttachments (handle);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVBufferRemoveAttachment (IntPtr buffer, IntPtr key);
		public void RemoveAttachment (NSString key)
		{
			CVBufferRemoveAttachment (handle, key.Handle);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVBufferGetAttachment (IntPtr buffer, IntPtr key, out CVAttachmentMode attachmentMode);
		public NSObject GetAttachment (NSString key, out CVAttachmentMode attachmentMode)
		{
			return Runtime.GetNSObject (CVBufferGetAttachment (handle, key.Handle, out attachmentMode));
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVBufferGetAttachments (IntPtr buffer, CVAttachmentMode attachmentMode);
		public NSDictionary GetAttachments (CVAttachmentMode attachmentMode)
		{
			return (NSDictionary) Runtime.GetNSObject (CVBufferGetAttachments (handle, attachmentMode));
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVBufferPropagateAttachments (IntPtr sourceBuffer, IntPtr destinationBuffer);
		public void PropogateAttachments (CVBuffer destinationBuffer)
		{
			CVBufferPropagateAttachments (handle, destinationBuffer.Handle);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVBufferSetAttachment (IntPtr buffer, IntPtr key, IntPtr @value, CVAttachmentMode attachmentMode);
		public void SetAttachment (NSString key, NSObject @value, CVAttachmentMode attachmentMode)
		{
			CVBufferSetAttachment (handle, key.Handle, @value.Handle, attachmentMode);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVBufferSetAttachments (IntPtr buffer, IntPtr theAttachments, CVAttachmentMode attachmentMode);
		public void SetAttachments (NSDictionary theAttachments, CVAttachmentMode attachmentMode)
		{
			CVBufferSetAttachments (handle, theAttachments.Handle, attachmentMode);
		}
	}
}
