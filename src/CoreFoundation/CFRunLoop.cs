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

namespace MonoMac.CoreFoundation {

	public enum CFRunLoopExitReason {
		Finished = 1,
		Stopped = 2,
		TimedOut = 3,
		HandledSource = 4
	}
	
	public class CFRunLoop : INativeObject, IDisposable {
		internal IntPtr handle;
		
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
		extern static bool CFRunLoopIsWaiting (IntPtr loop);
		public bool IsWaiting {
			get {
				return CFRunLoopIsWaiting (handle);
			}
		}
		
		[DllImport (Constants.CoreFoundationLibrary)]
		extern static int CFRunLoopRunInMode (IntPtr cfstring_mode, double interval, int return_after_source_handled);
		public CFRunLoopExitReason RunInMode (string mode, double interval, bool returnAfterSourceHandled)
		{
			CFString s = mode == null ? null : new CFString (mode);

			var v = CFRunLoopRunInMode (s == null ? IntPtr.Zero : s.Handle, interval, returnAfterSourceHandled ? 1 : 0);
			if (s != null)
				s.Dispose ();
			
			return (CFRunLoopExitReason) v;
		}
		
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
