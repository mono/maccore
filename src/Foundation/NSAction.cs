//
// Copyright 2009-2010, Novell, Inc.
// Copyright 2012 Xamarin Inc.
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

using System;
using System.Runtime.InteropServices;
using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {

	public delegate void NSAction ();

	// Use this for synchronous operations
	[Register ("__MonoMac_NSActionDispatcher")]
	internal class NSActionDispatcher : NSObject {

		public static Selector Selector = new Selector ("apply");

		NSAction action;

		public NSActionDispatcher (NSAction action)
		{
			this.action = action;
		}

		[Export ("apply")]
		[Preserve (Conditional = true)]
		public void Apply ()
		{
			action ();
		}
	}

	// Use this for asynchronous operations
	[Register ("__MonoMac_NSAsyncActionDispatcher")]
	internal class NSAsyncActionDispatcher : NSObject {
		GCHandle gch;
		NSAction action;

		// This ctor is so that the runtime can create a new instance of this class
		// if ObjC wants to call release on an instance we've already called Dispose on.
		// Since we detach the handle from the managed instance when Dispose is called,
		// there is no way we can get the existing managed instance (which has possibly 
		// been freed anyway) when ObjC calls release (which ends up in NSObject.NativeRelease).
		[Obsolete ("Do not use")]
		public NSAsyncActionDispatcher (IntPtr handle)
			: base (handle)
		{
		}

		public NSAsyncActionDispatcher (NSAction action)
		{
			this.action = action;
			gch = GCHandle.Alloc (this);
		}

		[Export ("apply")]
		[Preserve (Conditional = true)]
		public void Apply ()
		{
			try {
				action ();
			} finally {
				action = null; // this is a one-shot dispatcher
				gch.Free ();
				Dispose ();
			}
		}
	}
}

