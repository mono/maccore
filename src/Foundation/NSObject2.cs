using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {

	public partial class NSObject {
		[Export ("encodeWithCoder:")]
		public virtual void EncodeTo (NSCoder coder)
		{
			if (coder == null)
				throw new ArgumentNullException ("coder");
			
			if (IsDirectBinding) {
                                Messaging.void_objc_msgSend_intptr (this.Handle, selAwakeFromNib, coder.Handle);
                        } else {
                                Messaging.void_objc_msgSendSuper_intptr (this.SuperHandle, selAwakeFromNib, coder.Handle);
                        }
		}
	}
}
