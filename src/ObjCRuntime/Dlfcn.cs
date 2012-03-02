//
// Dlfcn.cs: Support for looking up symbols in shared libraries
//
// Authors:
//   Jonathan Pryor:
//   Miguel de Icaza.
//
// Copyright 2009-2010, Novell, Inc.
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
using MonoMac.Foundation;

namespace MonoMac.ObjCRuntime {
	
	public static class Dlfcn {

		[DllImport (Constants.SystemLibrary)]
		public static extern int dlclose (IntPtr handle);

		[DllImport (Constants.SystemLibrary)]
		public static extern IntPtr dlopen (string path, int mode);

		[DllImport (Constants.SystemLibrary)]
		public static extern IntPtr dlsym (IntPtr handle, string symbol);

		[DllImport (Constants.SystemLibrary)]
		public static extern string dlerror ();

		public static NSString GetStringConstant (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return null;
			var actual = Marshal.ReadIntPtr (indirect);
			if (actual == IntPtr.Zero)
				return null;
			return (NSString) Runtime.GetNSObject (actual);
		}

		public static IntPtr GetIndirect (IntPtr handle, string symbol)
		{
			return dlsym (handle, symbol);
		}

#if !GENERATOR && !MONOMAC_BOOTSTRAP
		public static NSNumber GetNSNumber (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return null;
			var actual = Marshal.ReadIntPtr (indirect);
			if (actual == IntPtr.Zero)
				return null;
			return (NSNumber) Runtime.GetNSObject (actual);
		}
#endif

		public static int GetInt32 (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return 0;
			return Marshal.ReadInt32 (indirect);
		}

		public static IntPtr GetIntPtr (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return IntPtr.Zero;
			return Marshal.ReadIntPtr (indirect);
		}

		public static double GetDouble (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return 0;
			unsafe {
				double *d = (double *) indirect;

				return *d;
			}
		}

		public static float GetFloat (IntPtr handle, string symbol)
		{
			var indirect = dlsym (handle, symbol);
			if (indirect == IntPtr.Zero)
				return 0;
			unsafe {
				float *d = (float *) indirect;

				return *d;
			}
		}
		
		internal static int SlowGetInt32 (string lib, string symbol)
		{
			var handle = dlopen (lib, 0);
			if (handle == IntPtr.Zero)
				return 0;
			try {
				return GetInt32 (handle, symbol);
			} finally {
				dlclose (handle);
			}
		}

		internal static IntPtr SlowGetIntPtr (string lib, string symbol)
		{
			var handle = dlopen (lib, 0);
			if (handle == IntPtr.Zero)
				return IntPtr.Zero;
			try {
				return GetIntPtr (handle, symbol);
			} finally {
				dlclose (handle);
			}
		}

		internal static double SlowGetDouble (string lib, string symbol)
		{
			var handle = dlopen (lib, 0);
			if (handle == IntPtr.Zero)
				return 0;
			try {
				return GetDouble (handle, symbol);
			} finally {
				dlclose (handle);
			}
		}

		internal static NSString SlowGetStringConstant (string lib, string symbol)
		{
			var handle = dlopen (lib, 0);
			if (handle == IntPtr.Zero)
				return null;
			try {
				return GetStringConstant (handle, symbol);
			} finally {
				dlclose (handle);
			}
		}
	}

}
