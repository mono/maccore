//
// CFRunLoop.cs: Main Loop
//
// Authors:
//    Miguel de Icaza (miguel@novell.com)
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

using System;
using System.Runtime.InteropServices;

using MonoMac.ObjCRuntime;
using MonoMac.Foundation;

namespace MonoMac.CoreFoundation {

	public enum CFRunLoopExitReason {
		Finished = 1,
		Stopped = 2,
		TimedOut = 3,
		HandledSource = 4
	}

#if false // this will eventually need to be exposed for CFNetwork.ExecuteAutoConfiguration*()
	public class CFRunLoopSource : INativeObject, IDisposable {
		internal IntPtr handle;

		internal CFRunLoopSource (IntPtr handle)
		{
			CFObject.CFRetain (handle);
			this.handle = handle;
		}

		~CFRunLoopSource ()
		{
			Dispose (false);
		}

		// TODO: Bind struct CFRunLoopSourceContext and its callbacks
		//[DllImport (Constants.CoreFoundationLibrary)]
		//extern static IntPtr CFRunLoopSourceCreate (IntPtr allocator, int order, IntPtr context);
		//public static CFRunLoopSource Create (int order, CFRunLoopSourceContext context)
		//{
		//	IntPtr source = CFRunLoopSourceCreate (IntPtr.Zero, order, context);
		//
		//	if (source != IntPtr.Zero)
		//		return new CFRunLoopSource (source);
		//
		//	return null;
		//}

		public IntPtr Handle {
			get {
				return handle;
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		// FIXME: CFRunLoopSourceGetOrder() returns CFIndex which is a typedef for 'signed long'
		extern static int CFRunLoopSourceGetOrder (IntPtr source);
		public int Order {
			get {
				return CFRunLoopSourceGetOrder (handle);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static void CFRunLoopSourceInvalidate (IntPtr source);
		public void Invalidate ()
		{
			CFRunLoopSourceInvalidate (handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static int CFRunLoopSourceIsValid (IntPtr source);
		public bool IsValid {
			get {
				return CFRunLoopSourceIsValid (handle) != 0;
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static void CFRunLoopSourceSignal (IntPtr source);
		public void Signal ()
		{
			CFRunLoopSourceSignal (handle);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero) {
				CFObject.CFRelease (handle);
				handle = IntPtr.Zero;
			}
		}
	}
#endif

	public class CFRunLoop : INativeObject, IDisposable {
		static IntPtr CoreFoundationLibraryHandle = Dlfcn.dlopen (Constants.CoreFoundationLibrary, 0);
		internal IntPtr handle;

		static NSString _CFDefaultRunLoopMode;
		public static NSString CFDefaultRunLoopMode {
			get {
				if (_CFDefaultRunLoopMode == null)
					_CFDefaultRunLoopMode = Dlfcn.GetStringConstant (CoreFoundationLibraryHandle, ModeDefault);
				return _CFDefaultRunLoopMode;
			}
		}

		static NSString _CFRunLoopCommonModes;
		public static NSString CFRunLoopCommonModes {
			get {
				if (_CFRunLoopCommonModes == null)
					_CFRunLoopCommonModes = Dlfcn.GetStringConstant (CoreFoundationLibraryHandle, ModeCommon);
				return _CFRunLoopCommonModes;
			}
		}

		// Note: This is a broken binding... we do not know what the values of the constant strings are, just their variable names and things are done by comparing CFString pointers, not a string compare anyway.
		public const string ModeDefault = "kCFRunLoopDefaultMode";
		public const string ModeCommon = "kCFRunLoopCommonModes";
		
		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFRunLoopGetCurrent ();

		static public CFRunLoop Current {
			get {
				return new CFRunLoop (CFRunLoopGetCurrent ());
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFRunLoopGetMain ();
		
		static public CFRunLoop Main {
			get {
				return new CFRunLoop (CFRunLoopGetMain ());
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static void CFRunLoopRun ();
		public void Run ()
		{
			CFRunLoopRun ();
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static void CFRunLoopStop (IntPtr loop);
		public void Stop ()
		{
			CFRunLoopStop (handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static void CFRunLoopWakeUp (IntPtr loop);
		public void WakeUp ()
		{
			CFRunLoopWakeUp (handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static int CFRunLoopIsWaiting (IntPtr loop);
		public bool IsWaiting {
			get {
				return CFRunLoopIsWaiting (handle) != 0;
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static int CFRunLoopRunInMode (IntPtr mode, double seconds, int returnAfterSourceHandled);
		public CFRunLoopExitReason RunInMode (NSString mode, double seconds, bool returnAfterSourceHandled)
		{
			if (mode == null)
				throw new ArgumentNullException ("mode");

			return (CFRunLoopExitReason) CFRunLoopRunInMode (mode.Handle, seconds, returnAfterSourceHandled ? 1 : 0);
		}

		[Obsolete ("Use the NSString version of CFRunLoop.RunInMode() instead.")]
		public CFRunLoopExitReason RunInMode (string mode, double seconds, bool returnAfterSourceHandled)
		{
			if (mode == null)
				throw new ArgumentNullException ("mode");

			CFString s = new CFString (mode);

			var v = CFRunLoopRunInMode (s.Handle, seconds, returnAfterSourceHandled ? 1 : 0);
			s.Dispose ();

			return (CFRunLoopExitReason) v;
		}

#if false // will eventually be needed by CFNetwork.ExecuteAutoConfiguration*()
		[DllImport (Constants.CoreFoundationLibrary)]
		extern static void CFRunLoopAddSource (IntPtr loop, IntPtr source, IntPtr mode);
		public void AddSource (CFRunLoopSource source, NSString mode)
		{
			if (mode == null)
				throw new ArgumentNullException ("mode");

			CFRunLoopAddSource (handle, source.Handle, mode.Handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static bool CFRunLoopContainsSource (IntPtr loop, IntPtr source, IntPtr mode);
		public bool ContainsSource (CFRunLoopSource source, NSString mode)
		{
			if (mode == null)
				throw new ArgumentNullException ("mode");

			return CFRunLoopContainsSource (handle, source.Handle, mode.Handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static bool CFRunLoopRemoveSource (IntPtr loop, IntPtr source, IntPtr mode);
		public bool RemoveSource (CFRunLoopSource source, NSString mode)
		{
			if (mode == null)
				throw new ArgumentNullException ("mode");

			return CFRunLoopRemoveSource (handle, source.Handle, mode.Handle);
		}
#endif

		internal CFRunLoop (IntPtr handle)
		{
			CFObject.CFRetain (handle);
			this.handle = handle;
		}

		~CFRunLoop ()
		{
			Dispose (false);
		}

		public IntPtr Handle {
			get {
				return handle;
			}
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CFObject.CFRelease (handle);
				handle = IntPtr.Zero;
			}
		}

		public static bool operator == (CFRunLoop a, CFRunLoop b)
		{
			return Object.Equals (a, b);
		}

		public static bool operator != (CFRunLoop a, CFRunLoop b)
		{
			return !Object.Equals (a, b);
		}

		public override int GetHashCode ()
		{
			return handle.GetHashCode ();
		}

		public override bool Equals (object other)
		{
			CFRunLoop cfother = other as CFRunLoop;
			if (cfother == null)
				return false;

			return cfother.Handle == Handle;
		}
	}
}
