// 
// CFDictionary.cs: P/Invokes for CFDictionary, CFMutableDictionary
//
// Authors: Mono Team
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
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreFoundation {

	[Since (3,2)]
	static class CFDictionary {

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFDictionaryGetValue (IntPtr theDict, IntPtr key);
		public static IntPtr GetValue (IntPtr theDict, IntPtr key)
		{
			return CFDictionaryGetValue (theDict, key);
		}

		public static bool GetBooleanValue (IntPtr theDict, IntPtr key)
		{
			var value = GetValue (theDict, key);
			if (value == IntPtr.Zero)
				return false;
			return CFBoolean.GetValue (value);
		}
	}

	static class CFMutableDictionary {

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static void CFDictionarySetValue (IntPtr theDict, IntPtr key, IntPtr value);
		public static void SetValue (IntPtr theDict, IntPtr key, IntPtr value)
		{
			CFDictionarySetValue (theDict, key, value);
		}

		public static void SetValue (IntPtr theDict, IntPtr key, bool value)
		{
			SetValue (theDict, key, value ? CFBoolean.True.Handle : CFBoolean.False.Handle);
		}
	}
}
