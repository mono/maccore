//
// NSUrlConnection.cs:
// Author:
//   Miguel de Icaza
//

using System;
using System.Reflection;
using System.Collections;
using System.Runtime.InteropServices;

using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {

	public partial class NSUrlConnection {
                static Selector selSendSynchronousRequestReturningResponseError = new Selector ("sendSynchronousRequest:returningResponse:error:");
		
		unsafe static NSData SendSynchronousRequest (NSUrlRequest request, out NSUrlResponse response, NSError error)
		{
			IntPtr storage = IntPtr.Zero;

			void *p = &storage;
			IntPtr handle = (IntPtr) p;
			
			var res = Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr_IntPtr (
				class_ptr,
				selSendSynchronousRequestReturningResponseError.Handle,
				request.Handle,
				handle,
				error != null ? error.Handle : IntPtr.Zero);

			if (storage != IntPtr.Zero)
				response = (NSUrlResponse) Runtime.GetNSObject (storage);
			else
				response = null;
			
			return (NSData) Runtime.GetNSObject (res);
		}
	}
}
