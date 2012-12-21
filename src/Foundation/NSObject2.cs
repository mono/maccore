// Copyright 2011, 2012 Xamarin Inc
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
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;

using MonoMac.ObjCRuntime;
#if !MONOMAC
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;
#endif
using MonoMac.CoreGraphics;

namespace MonoMac.Foundation {

	public partial class NSObject {
		static readonly IntPtr selConformsToProtocol = Selector.GetHandle ("conformsToProtocol:");
		static readonly IntPtr selEncodeWithCoder = Selector.GetHandle ("encodeWithCoder:");
		
		[Export ("encodeWithCoder:")]
		public virtual void EncodeTo (NSCoder coder)
		{
			if (coder == null)
				throw new ArgumentNullException ("coder");
			
			if (IsDirectBinding) {
				Messaging.void_objc_msgSend_intptr (this.Handle, selEncodeWithCoder, coder.Handle);
			} else {
				Messaging.void_objc_msgSendSuper_intptr (this.SuperHandle, selEncodeWithCoder, coder.Handle);
			}
		}

		[Export ("conformsToProtocol:")]
		[Preserve ()]
		public virtual bool ConformsToProtocol (IntPtr protocol)
		{
			bool does;
			
			if (IsDirectBinding) {
				does = Messaging.bool_objc_msgSend_intptr (this.Handle, selConformsToProtocol, protocol);
			} else {
				does = Messaging.bool_objc_msgSendSuper_intptr (this.SuperHandle, selConformsToProtocol, protocol);
			}

			if (does)
				return true;
			
			object [] adoptedProtocols = GetType ().GetCustomAttributes (typeof (AdoptsAttribute), true);
			foreach (AdoptsAttribute adopts in adoptedProtocols){
				if (adopts.ProtocolHandle == protocol)
					return true;
			}
			return false;
		}

		public static NSObject FromObject (object obj)
		{
			if (obj == null)
				return NSNull.Null;
			var t = obj.GetType ();
			if (t == typeof (NSObject) || t.IsSubclassOf (typeof (NSObject)))
				return (NSObject) obj;
			
			switch (Type.GetTypeCode (t)){
			case TypeCode.Boolean:
				return new NSNumber ((bool) obj);
			case TypeCode.Char:
				return new NSNumber ((ushort) (char) obj);
			case TypeCode.SByte:
				return new NSNumber ((sbyte) obj);
			case TypeCode.Byte:
				return new NSNumber ((byte) obj);
			case TypeCode.Int16:
				return new NSNumber ((short) obj);
			case TypeCode.UInt16:
				return new NSNumber ((ushort) obj);
			case TypeCode.Int32:
				return new NSNumber ((int) obj);
			case TypeCode.UInt32:
				return new NSNumber ((uint) obj);
			case TypeCode.Int64:
				return new NSNumber ((long) obj);
			case TypeCode.UInt64:
				return new NSNumber ((ulong) obj);
			case TypeCode.Single:
				return new NSNumber ((float) obj);
			case TypeCode.Double:
				return new NSNumber ((double) obj);
			case TypeCode.String:
				return new NSString ((string) obj);
			default:
				if (t == typeof (IntPtr))
					return NSValue.ValueFromPointer ((IntPtr) obj);

				if (t == typeof (SizeF))
					return NSValue.FromSizeF ((SizeF) obj);
				else if (t == typeof (RectangleF))
					return NSValue.FromRectangleF ((RectangleF) obj);
				else if (t == typeof (PointF))
					return NSValue.FromPointF ((PointF) obj);
#if !MONOMAC
				if (t == typeof (CGAffineTransform))
					return NSValue.FromCGAffineTransform ((CGAffineTransform) obj);
				else if (t == typeof (UIEdgeInsets))
					return NSValue.FromUIEdgeInsets ((UIEdgeInsets) obj);
				else if (t == typeof (CATransform3D))
					return NSValue.FromCATransform3D ((CATransform3D) obj);
#endif
				// last chance for types like CGPath, CGColor... that are not NSObject but are CFObject
				// see https://bugzilla.xamarin.com/show_bug.cgi?id=8458
				INativeObject native = (obj as INativeObject);
				if (native != null)
					return Runtime.GetNSObject (native.Handle);
				return null;
			}
		}

		public void SetValueForKeyPath (IntPtr handle, NSString keyPath)
		{
			if (keyPath == null)
				throw new ArgumentNullException ("keyPath");
			if (IsDirectBinding) {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selSetValueForKeyPath_, handle, keyPath.Handle);
			} else {
				MonoMac.ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr_IntPtr (this.SuperHandle, selSetValueForKeyPath_, handle, keyPath.Handle);
			}
			
		}

		public override string ToString ()
		{
			return Description ?? base.ToString ();
		}

                public virtual void Invoke (NSAction action, double delay)
                {
                        var d = new NSAsyncActionDispatcher (action);
                        PerformSelector (NSActionDispatcher.Selector, d, delay);
                }

                public virtual void Invoke (NSAction action, TimeSpan delay)
                {
                        var d = new NSAsyncActionDispatcher (action);
                        PerformSelector (NSActionDispatcher.Selector, d, delay.TotalSeconds);
                }
	}
}
