//
// Dispatch.cs: Support for Grand Central Dispatch framework
//
// Authors:
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2010 Novell, Inc.
//
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
using MonoMac.Foundation;

namespace MonoMac.CoreFoundation {

	public enum DispatchQueuePriority {
		High = 2,
		Default = 0,
		Low = -2,
	}
	
	public class DispatchObject : INativeObject, IDisposable  {
		internal IntPtr handle;

		//
		// Constructors and lifecycle
		//
		[Preserve (Conditional = true)]
		internal DispatchObject (IntPtr handle, bool owns)
		{
			if (handle == IntPtr.Zero)
				throw new ArgumentNullException ("handle");
			
			this.handle = handle;
			if (!owns)
				dispatch_retain (handle);
		}

		internal DispatchObject ()
		{
		}      
		
		[DllImport ("libc")]
		extern static IntPtr dispatch_release (IntPtr o);

		[DllImport ("libc")]
		extern static IntPtr dispatch_retain (IntPtr o);

               ~DispatchObject ()
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
             
                protected virtual void Dispose (bool disposing)
                {
                        if (handle != IntPtr.Zero){
                                dispatch_release (handle);
                                handle = IntPtr.Zero;
                        }
                }

		public static bool operator == (DispatchObject a, DispatchObject b)
		{
			if (a == null){
				if (b == null)
					return true;
				return false;
			} else {
				if (b == null)
					return false;
				return a.handle == b.handle;
			}
		}

		public static bool operator != (DispatchObject a, DispatchObject b)
		{
			if (a == null){
				if (b == null)
					return false;
				return true;
			} else {
				if (b == null)
					return true;
				return a.handle != b.handle;
			}
		}

		public override bool Equals (object other)
		{
			var od = other as DispatchQueue;
			if (od == null)
				return false;

			return od.handle == handle;
		}

		public override int GetHashCode ()
		{
			return (int) handle;
		}

		void Check ()
		{
			if (handle == IntPtr.Zero)
				throw new ObjectDisposedException ("DispatchQueue");
		}
			
		//
		// Properties and methods
		//
		[DllImport ("libc")]
		extern static void dispatch_suspend (IntPtr o);
		public void Suspend ()
		{
			Check ();
			dispatch_suspend (handle);
		}

		[DllImport ("libc")]
		extern static void dispatch_resume (IntPtr o);
		
		public void Resume ()
		{
			Check ();
			dispatch_resume (handle);
		}

		[DllImport ("libc")]
		extern static IntPtr dispatch_get_context (IntPtr o);

		[DllImport ("libc")]
		extern static void dispatch_set_context (IntPtr o, IntPtr ctx);

		public IntPtr Context {
			get {
				Check ();
				return dispatch_get_context (handle);
			}
			set {
				Check ();
				dispatch_set_context (handle, value);
			}
		}
	}

	public class DispatchQueue : DispatchObject  {
		[Preserve (Conditional = true)]
		internal DispatchQueue (IntPtr handle, bool owns) : base (handle, owns)
		{
		}

		public DispatchQueue (IntPtr handle) : base (handle, false)
		{
		}
		
		public DispatchQueue (string label) : base ()
		{
			// Initialized in owned state for the queue.
			handle = dispatch_queue_create (label, IntPtr.Zero);
			if (handle == IntPtr.Zero)
				throw new Exception ("Error creating dispatch queue");
		}
		
		//
		// Properties and methods
		//

		public string Label {
			get {
				if (handle == IntPtr.Zero)
					throw new ObjectDisposedException ("DispatchQueue");
				
				return dispatch_queue_get_label (handle);
			}
		}

		public static DispatchQueue CurrentQueue {
			get {
				return new DispatchQueue (dispatch_get_current_queue (), false);
			}
		}

		public static DispatchQueue GetGlobalQueue (DispatchQueuePriority priority)
		{
			return new DispatchQueue (dispatch_get_global_queue ((IntPtr) priority, IntPtr.Zero), false);
		}

		public static DispatchQueue DefaultGlobalQueue {
			get {
				return new DispatchQueue (dispatch_get_global_queue ((IntPtr) DispatchQueuePriority.Default, IntPtr.Zero), false);
			}
		}
#if !MONOMAC	
		static IntPtr main_q;
#endif
		static object lockobj = new object ();

		public static DispatchQueue MainQueue {
			get {
#if !MONOMAC
				if (Runtime.Arch == Arch.DEVICE) {
					lock (lockobj) {
						if (main_q == IntPtr.Zero) {
							var h = Dlfcn.dlopen ("/usr/lib/libSystem.dylib", 0x0);
							main_q = Dlfcn.GetIndirect (h, "_dispatch_main_q");
							Dlfcn.dlclose (h);
						}
					}

					return new DispatchQueue (main_q, false);
				} else
#endif
					return new DispatchQueue (dispatch_get_main_queue (), false);
			}
		}


		//
		// Dispatching
		//
		delegate void dispatch_callback_t (IntPtr context);
		static dispatch_callback_t static_dispatch = new dispatch_callback_t (static_dispatcher_to_managed);
		
		[MonoPInvokeCallback (typeof (dispatch_callback_t))]
		static void static_dispatcher_to_managed (IntPtr context)
		{
			GCHandle gch = GCHandle.FromIntPtr (context);
                        var action = gch.Target as NSAction;
			if (action != null)
				action ();
			gch.Free ();
		}
		
		public void DispatchAsync (NSAction action)
		{
			if (action == null)
				throw new ArgumentNullException ("action");
			
			dispatch_async_f (handle, (IntPtr) GCHandle.Alloc (action), static_dispatch);
		}

		public void DispatchSync (NSAction action)
		{
			if (action == null)
				throw new ArgumentNullException ("action");
			
			dispatch_sync_f (handle, (IntPtr) GCHandle.Alloc (action), static_dispatch);
		}
		//
		// Native methods
		//
		[DllImport ("libc")]
		extern static IntPtr dispatch_queue_create (string label, IntPtr attr);

		[DllImport ("libc")]
		extern static void dispatch_async_f (IntPtr queue, IntPtr context, dispatch_callback_t dispatch);

		[DllImport ("libc")]
		extern static void dispatch_sync_f (IntPtr queue, IntPtr context, dispatch_callback_t dispatch);

		[DllImport ("libc")]
		extern static IntPtr dispatch_get_current_queue ();

		[DllImport ("libc")]
		extern static IntPtr dispatch_get_global_queue (IntPtr priority, IntPtr flags);

		[DllImport ("libc")]
		extern static IntPtr dispatch_get_main_queue ();

		[DllImport ("libc")]
		extern static string dispatch_queue_get_label (IntPtr queue);

#if MONOMAC
		//
		// Not to be used by apps that use UIApplicationMain, NSApplicationMain or CFRunLoopRun,
		// so not available on Monotouch
		//
		[DllImport ("libc")]
		static extern IntPtr dispatch_main ();

		public static void MainIteration ()
		{
			dispatch_main ();
		}
#endif
	
	}
}
