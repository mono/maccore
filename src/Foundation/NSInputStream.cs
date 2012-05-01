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

		protected override void Dispose (bool disposing)
		{
			context.Release ();
			context.Info = IntPtr.Zero;
			
			base.Dispose (disposing);
		}
		
		[Export ("_setCFClientFlags:callback:context:")]
		public virtual bool SetCFClientFlags (CFStreamEventType inFlags, IntPtr inCallback, IntPtr inContextPtr)
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

		public void Notify (CFStreamEventType eventType)
		{
			if (!flags.HasFlag (eventType))
				return;

			context.Invoke (callback, Handle, eventType);
		}
	}
}	
