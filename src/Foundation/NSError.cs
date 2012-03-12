//
// Copyright 2010, Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//
// Simple class for passing NSErrors as EventArgs
//
using System;
using System.Diagnostics;
using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {
	
	public class NSErrorEventArgs : EventArgs {
		public NSErrorEventArgs (NSError error)
		{
			Error = error;
		}

		public NSError Error { get; private set; }
	}

	public partial class NSError : NSObject {
#if !COREBUILD
		[Obsolete ("Creating NSErrors without arguments can cause your application to crash unexpectedly if you return it back to Objective-C")]
		[Export ("init")]
		public NSError () : base (NSObjectFlag.Empty)
		{
			Debug.WriteLine ("Warning: you created an NSError without a domain, this can crash your application if you return this to Objective-C");
			if (IsDirectBinding) {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);
			} else {
				Handle = MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);
			}
		}
		
		public static NSError FromDomain (NSString domain, int code)
		{
			return FromDomain (domain, code, null);
		}

		public NSError (NSString domain, int code) : this (domain, code, null)
		{
		}
		public override string ToString ()
		{
			return LocalizedDescription;
		}
#endif
	}
}
