// Copyright 2011 - 2013 Xamarin Inc
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
	public class NSObjectFlag {
		public static readonly NSObjectFlag Empty;
		
		NSObjectFlag () {}
	}

	public partial class NSObject {
		static readonly IntPtr selConformsToProtocol = Selector.GetHandle ("conformsToProtocol:");
		static readonly IntPtr selEncodeWithCoder = Selector.GetHandle ("encodeWithCoder:");
		static readonly IntPtr selAwakeFromNib = Selector.GetHandle ("awakeFromNib");
		static readonly IntPtr selRespondsToSelector = Selector.GetHandle ("respondsToSelector:");
		
		IntPtr handle;
		IntPtr super;
		bool disposed;

		protected bool IsDirectBinding;

#if COREBUILD
		static readonly IntPtr class_ptr = Class.GetHandle ("NSObject");
		public virtual IntPtr ClassHandle  { get { return class_ptr; } }
#endif
		
		[Export ("init")]
		public NSObject () {
			bool alloced = AllocIfNeeded ();
			InitializeObject (alloced);
		}
		
		// This is just here as a constructor chain that can will
		// only do Init at the most derived class.
		public NSObject (NSObjectFlag x)
		{
			bool alloced = AllocIfNeeded ();
			InitializeObject (alloced);
		}
		
		public NSObject (IntPtr handle) : this (handle, false) {
		}
		
		public NSObject (IntPtr handle, bool alloced) {
			this.handle = handle;
			InitializeObject (alloced);
		}
		
		~NSObject () {
			Dispose (false);
		}
		
		public void Dispose () {
			Dispose (true);
			GC.SuppressFinalize (this);
		}

#if !COREBUILD
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
#endif

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

		[Export ("respondsToSelector:")]
		public virtual bool RespondsToSelector (Selector sel) {
			if (IsDirectBinding) {
				return Messaging.bool_objc_msgSend_intptr (this.Handle, selRespondsToSelector, sel.Handle);
			} else {
				return Messaging.bool_objc_msgSendSuper_intptr (this.SuperHandle, selRespondsToSelector, sel.Handle);
			}
		}
		
		[Export ("doesNotRecognizeSelector:")]
		public virtual void DoesNotRecognizeSelector (Selector sel) {
			Messaging.void_objc_msgSendSuper_intptr (SuperHandle, Selector.DoesNotRecognizeSelector, sel.Handle);
		}
		
		internal void Release () {
			Messaging.void_objc_msgSend (handle, Selector.Release);
		}
		
		internal void Retain () {
			Messaging.void_objc_msgSend (handle, Selector.Retain);
		}
		
		public IntPtr SuperHandle {
			get {
				if (super == IntPtr.Zero) {
					super = Marshal.AllocHGlobal (Marshal.SizeOf (typeof (objc_super)));
					unsafe {
						objc_super *sup = (objc_super *) super;
						sup->receiver = handle;
						sup->super = ClassHandle;
					}
				}
				return super;
			}
		}
		
		public IntPtr Handle {
			get { return handle; }
			set {
				if (handle == value)
					return;
				
				if (handle != IntPtr.Zero)
					Runtime.UnregisterNSObject (handle);
				
				handle = value;
				
				if (handle != IntPtr.Zero)
					Runtime.RegisterNSObject (this, handle);
			}
		}
		
		private bool AllocIfNeeded () {
			if (handle == IntPtr.Zero) {
				handle = Messaging.intptr_objc_msgSend (Class.GetHandle (this.GetType ()), Selector.Alloc);
				return true;
			}
			return false;
		}
		
		private IntPtr GetObjCIvar (string name) {
			IntPtr native;
			
			object_getInstanceVariable (handle, name, out native);
			
			return native;
		}
		
		public NSObject GetNativeField (string name) {
			IntPtr field = GetObjCIvar (name);
			
			if (field == IntPtr.Zero)
				return null;
			return Runtime.GetNSObject (field);
		}
		
		private void SetObjCIvar (string name, IntPtr value) {
			object_setInstanceVariable (handle, name, value);
		}
		
		public void SetNativeField (string name, NSObject value) {
			if (value == null)
				SetObjCIvar (name, IntPtr.Zero);
			else
				SetObjCIvar (name, value.Handle);
		}
		
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static void object_getInstanceVariable (IntPtr obj, string name, out IntPtr val);

		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static void object_setInstanceVariable (IntPtr obj, string name, IntPtr val);
		
		struct objc_super {
			public IntPtr receiver;
			public IntPtr super;
		}
		
		[Export ("performSelector:withObject:afterDelay:")]
		public virtual void PerformSelector (Selector sel, NSObject obj, double delay) {
			if (sel == null)
				throw new ArgumentNullException ("sel");
			if (IsDirectBinding) {
				Messaging.void_objc_msgSend_intptr_intptr_double (this.Handle, Selector.PerformSelectorWithObjectAfterDelay, sel.Handle, obj == null ? IntPtr.Zero : obj.Handle, delay);
			} else {
				Messaging.void_objc_msgSendSuper_intptr_intptr_double (this.SuperHandle, Selector.PerformSelectorWithObjectAfterDelay, sel.Handle, obj == null ? IntPtr.Zero : obj.Handle, delay);
			}
		}
		
		[Export ("awakeFromNib")]
		public virtual void AwakeFromNib ()
		{
			if (IsDirectBinding) {
				Messaging.void_objc_msgSend (this.Handle, selAwakeFromNib);
			} else {
				Messaging.void_objc_msgSendSuper (this.SuperHandle, selAwakeFromNib);
			}
		}
		
		private void InvokeOnMainThread (Selector sel, NSObject obj, bool wait)
		{
			Messaging.void_objc_msgSend_intptr_intptr_bool (this.Handle, Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone, sel.Handle, obj == null ? IntPtr.Zero : obj.Handle, wait);
		}
		
		public void BeginInvokeOnMainThread (Selector sel, NSObject obj)
		{
			InvokeOnMainThread (sel, obj, false);
		}
		
		public void InvokeOnMainThread (Selector sel, NSObject obj)
		{
			InvokeOnMainThread (sel, obj, true);
		}
		
		public void BeginInvokeOnMainThread (NSAction action)
		{
			var d = new NSAsyncActionDispatcher (action);
			Messaging.void_objc_msgSend_intptr_intptr_bool (d.Handle, Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone, 
			                                                NSActionDispatcher.Selector.Handle, d.Handle, false);
		}
		
		public void InvokeOnMainThread (NSAction action)
		{
			using (var d = new NSActionDispatcher (action)) {
				Messaging.void_objc_msgSend_intptr_intptr_bool (d.Handle, Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone, 
				                                                NSActionDispatcher.Selector.Handle, d.Handle, true);
			}
		}
		
#if !COREBUILD
		internal static readonly IntPtr retainCount = Selector.GetHandle ("retainCount");
		
		[Export ("retainCount")]
		public virtual int RetainCount {
			get {
				if (IsDirectBinding) {
					return Messaging.int_objc_msgSend (this.Handle, retainCount);
				} else {
					return Messaging.int_objc_msgSendSuper (this.SuperHandle, retainCount);
				}
			}
		}
#endif

#if !COREBUILD
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
#endif

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
		
		internal void ClearHandle ()
		{
			handle = IntPtr.Zero;
		}
		
		protected virtual void Dispose (bool disposing) {
			if (disposed)
				return;
			disposed = true;
			
			if (handle != IntPtr.Zero) {
				if (disposing) {
					ReleaseManagedRef ();
				} else {
					NSObject_Disposer.Add (this);
				}
			}
			if (super != IntPtr.Zero) {
				Marshal.FreeHGlobal (super);
				super = IntPtr.Zero;
			}
		}

		[Register ("__NSObject_Disposer")]
		[Preserve (AllMembers=true)]
		internal class NSObject_Disposer : NSObject {
			static readonly List <NSObject> drainList1 = new List<NSObject> ();
			static readonly List <NSObject> drainList2 = new List<NSObject> ();
			static List <NSObject> handles = drainList1;
			static readonly IntPtr selDrain = Selector.GetHandle ("drain:");
			
			static readonly IntPtr class_ptr = Class.GetHandle ("__NSObject_Disposer");
			
			static readonly object lock_obj = new object ();
			
			private NSObject_Disposer ()
			{
				// Disable default ctor, there should be no instances of this class.
			}
			
			static internal void Add (NSObject handle) {
				bool call_drain;
				lock (lock_obj) {
					handles.Add (handle);
					call_drain = handles.Count == 1;
				}
				if (!call_drain)
					return;
				Messaging.void_objc_msgSend_intptr_intptr_bool (class_ptr, Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone, selDrain, IntPtr.Zero, false);
			}
			
			[Export ("drain:")]
			static  void Drain (NSObject ctx) {
				List<NSObject> drainList;
				
				lock (lock_obj) {
					drainList = handles;
					if (handles == drainList1)
						handles = drainList2;
					else
						handles = drainList1;
				}
				
				foreach (NSObject x in drainList)
					x.ReleaseManagedRef ();
				drainList.Clear();
			}
		}
	}
}
