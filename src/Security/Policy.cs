// 
// Policy.cs: Implements the managed SecPolicy wrapper.
//
// Authors: Miguel de Icaza
//     
// Copyright 2010 Novell, Inc
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
using MonoMac.CoreFoundation;
using MonoMac.Foundation;

namespace MonoMac.Security {
	public class SecPolicy : INativeObject, IDisposable {
		IntPtr handle;

		internal SecPolicy (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				throw new Exception ("Invalid parameters to context creation");

			CFRetain (handle);
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal SecPolicy (IntPtr handle, bool owns)
		{
			if (!owns)
				CFRetain (handle);

			this.handle = handle;
		}

		~SecPolicy ()
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

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static void CFRelease (IntPtr handle);

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static void CFRetain (IntPtr handle);

		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CFRelease (handle);
				handle = IntPtr.Zero;
			}
		}

		[DllImport (Constants.SecurityLibrary, EntryPoint="SecPolicyGetTypeID")]
		public extern static int GetTypeID ();

		public static bool operator == (SecPolicy a, SecPolicy b)
		{
			if (((object)a) == null)
				return ((object)b) == null;
			else if ((object)b == null)
				return false;

			return a.Handle == b.Handle;
		}

		public static bool operator != (SecPolicy a, SecPolicy b)
		{
			if (((object)a) == null)
				return ((object)b) != null;
			else if (((object)b) == null)
				return true;
			return a.Handle != b.Handle;
		}

		public override bool Equals (object other)
		{
			var o = other as SecPolicy;
			return this == o;
		}

		public override int GetHashCode ()
		{
			return (int) Handle;
		}
	}
}
