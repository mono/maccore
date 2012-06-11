using System;
using System.Runtime.InteropServices;
using MonoMac.CoreFoundation;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {
	public partial class NSInputStream : NSStream {
		static IntPtr selReadMaxLength = Selector.sel_registerName ("read:maxLength:");

		CFStreamEventType flags;
		IntPtr callback;
		CFStreamClientContext context;

		public int Read (byte [] buffer, uint len) {
			return objc_msgSend (Handle, selReadMaxLength, buffer, len);
		}

		[DllImport ("/usr/lib/libobjc.dylib")]
		static extern int objc_msgSend (IntPtr handle, IntPtr sel, [In, Out] byte [] buffer, uint len);

		[Export ("read:maxLength:")]
		public virtual int Read (IntPtr buffer, uint len)
		{
			if (buffer == IntPtr.Zero)
				throw new ArgumentNullException ("buffer");
			
			int ret;
			if (IsDirectBinding) {
				ret = Messaging.int_objc_msgSend_IntPtr_UInt32 (this.Handle, selReadMaxLength, buffer, len);
			} else {
				ret = Messaging.int_objc_msgSendSuper_IntPtr_UInt32 (this.SuperHandle, selReadMaxLength, buffer, len);
			}
			
			return ret;
		}

		protected override void Dispose (bool disposing)
		{
			context.Release ();
			context.Info = IntPtr.Zero;
			
			base.Dispose (disposing);
		}
		
		[Export ("_setCFClientFlags:callback:context:")]
		protected virtual bool SetCFClientFlags (CFStreamEventType inFlags, IntPtr inCallback, IntPtr inContextPtr)
		{
			CFStreamClientContext inContext;
			
			if (inContextPtr == IntPtr.Zero)
				return false;
			
			inContext = (CFStreamClientContext) Marshal.PtrToStructure (inContextPtr, typeof (CFStreamClientContext));
			if (inContext.Version != 0)
				return false;
			
			context.Release ();
			context = inContext;
			context.Retain ();
			
			flags = inFlags;
			callback = inCallback;

			return true;
		}

		[Export ("getBuffer:length:")]
		protected unsafe virtual bool GetBuffer (out IntPtr buffer, out System.UInt32 len)
		{
			buffer = IntPtr.Zero;
			len = 0;
			return false;
		}

		public void Notify (CFStreamEventType eventType)
		{
			if ((flags & eventType) == 0)
				return;

			context.Invoke (callback, Handle, eventType);
		}
	}
}	
