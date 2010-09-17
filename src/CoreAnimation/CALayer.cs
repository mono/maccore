// 
// UIView.cs: Implements the managed UIView
//
// Authors:
//   Geoff Norton.
//     
// Copyright 2009 Novell, Inc
//
using System;

using MonoMac.Foundation; 
using MonoMac.ObjCRuntime;
using MonoMac.CoreGraphics;

namespace MonoMac.CoreAnimation {

	public partial class CALayer {
		static IntPtr selInitWithLayer = Selector.sel_registerName ("initWithLayer:");

		[Export ("initWithLayer:")]
		public CALayer (CALayer other)
		{
			if (this.GetType () == typeof (CALayer)){
				Messaging.intptr_objc_msgSend_intptr (Handle, selInitWithLayer, other.Handle);
			} else {
				Messaging.intptr_objc_msgSendSuper_intptr (SuperHandle, selInitWithLayer, other.Handle);
				Clone (other);
			}
		}

		public virtual void Clone (CALayer other)
		{
			// Subclasses must copy any instance values that they care from other
		}

		[Obsolete ("Use BeginTime instead")]
		public double CFTimeInterval {
			get { return BeginTime; }
			set { BeginTime = value; }
		}
	}

	public partial class CADisplayLink {
		public static CADisplayLink Create (NSAction action)
		{
			var d = new NSActionDispatcher (action);
			return Create (d, NSActionDispatcher.Selector);
		}
	}

	public partial class CAAnimation {
		[Obsolete ("Use BeginTime instead")]
		public double CFTimeInterval {
			get { return BeginTime; }
			set { BeginTime = value; }
		}
	}
}


