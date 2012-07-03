// 
// CFAllocator.cs
//
// Authors:
//    Rolf Bjarne Kvinge
//     
// Copyright 2012 Xamarin Inc
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
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreFoundation {	
	static class CFAllocator {
		public static IntPtr Default;
		public static IntPtr SystemDefault;
		public static IntPtr Malloc;
		public static IntPtr MallocZone;
		public static IntPtr Null;
		public static IntPtr UseContext;
		
		static CFAllocator ()
		{
			var handle = Dlfcn.dlopen (Constants.CoreFoundationLibrary, 0);
			try {
				Default = Dlfcn.GetIntPtr (handle, "kCFAllocatorDefault");
				SystemDefault = Dlfcn.GetIntPtr (handle, "kCFAllocatorSystemDefault");
				Malloc = Dlfcn.GetIntPtr (handle, "kCFAllocatorMalloc");
				MallocZone = Dlfcn.GetIntPtr (handle, "kCFAllocatorMallocZone");
				Null = Dlfcn.GetIntPtr (handle, "kCFAllocatorNull");
				UseContext = Dlfcn.GetIntPtr (handle, "kCFAllocatorUseContext");
			} finally {
				Dlfcn.dlclose (handle);
			}
		}
	}
	
}