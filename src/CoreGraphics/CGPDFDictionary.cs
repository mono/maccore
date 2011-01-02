// 
// CGPDFDictionary.cs: Implements the managed CGPDFDictionary binding
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
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using System.Drawing;
using MonoMac.CoreFoundation;

namespace MonoMac.CoreGraphics {
	public class CGPDFDictionary : INativeObject {
		internal IntPtr handle;

		public IntPtr Handle {
			get { return handle; }
		}

		/* invoked by marshallers */
		public CGPDFDictionary (IntPtr handle)
		{
			this.handle = handle;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPDFDictionaryGetCount (IntPtr handle);
		
		public int Count {
			get {
				return (int) CGPDFDictionaryGetCount (handle);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPDFDictionaryGetBoolean (IntPtr handle, string key, out bool result);

		public bool GetBoolean (string key, out bool result)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			return CGPDFDictionaryGetBoolean (handle, key, out result);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPDFDictionaryGetInteger (IntPtr handle, string key, out int result);

		public bool GetInt (string key, out int result)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			return CGPDFDictionaryGetInteger (handle, key, out result);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPDFDictionaryGetNumber (IntPtr handle, string key, out float result);

		public bool GetFloat (string key, out float result)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			return CGPDFDictionaryGetNumber (handle, key, out result);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPDFDictionaryGetName (IntPtr handle, string key, out IntPtr result);

		public bool GetName (string key, out string result)
		{
			IntPtr res;
			
			if (key == null)
				throw new ArgumentNullException ("key");
			var r = CGPDFDictionaryGetName (handle, key, out res);
			if (!r){
				result = null;
				return false;
			}
			result = Marshal.PtrToStringAnsi (res);
			return true;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPDFDictionaryGetDictionary (IntPtr handle, string key, out IntPtr result);

		public bool GetDictionary (string key, out CGPDFDictionary result)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			IntPtr res;
			var r = CGPDFDictionaryGetDictionary (handle, key, out res);
			if (!r){
				result = null;
				return false;
			}
			result = new CGPDFDictionary (res);
			return true;
		}

		// TODO: GetString -> returns a CGPDFString
		// TODO: GetArray -> arrays of CGPDF objects
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPDFDictionaryGetStream (IntPtr handle, string key, out IntPtr result);

		public bool GetStream (string key, out CGPDFStream result)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			IntPtr ptr;
			var r = CGPDFDictionaryGetStream (handle, key, out ptr); 
			if (!r){
				result = null;
				return false;
			}
			result = new CGPDFStream (ptr);
			return true;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPDFDictionaryGetArray (IntPtr handle, string key, out IntPtr result);

		public bool GetArray (string key, out CGPDFArray array)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			IntPtr ptr;
			var r = CGPDFDictionaryGetArray (handle, key, out ptr);
			if (!r){
				array = null;
				return false;
			}
			array = new CGPDFArray (ptr);
			return true;
		}

		//[DllImport (Constants.CoreGraphicsLibrary)]
		// Returns a CGPDFString
		//extern static bool CGPDFDictionaryGetString (IntPtr handle, string key, out IntPtr result);

		// TODO: Apply function
	}
}
