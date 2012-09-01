//
// NSString2.cs: Support code added after the generator has run for NSString
// 
// Copyright 2009-2010, Novell, Inc.
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
using System.Collections;
using System.Runtime.InteropServices;

using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {

	public partial class NSString {
		//static Selector selDataUsingEncoding = new Selector ("dataUsingEncoding:");
		static IntPtr selDataUsingEncodingAllow = Selector.sel_registerName ("dataUsingEncoding:allowLossyConversion:");
		static IntPtr selInitWithDataEncoding = Selector.sel_registerName ("initWithData:encoding:");

		[Obsolete("Use Encode instead")]
		public NSData DataUsingEncoding (NSStringEncoding enc)
		{
			return new NSData (Messaging.IntPtr_objc_msgSend_int_int (Handle, selDataUsingEncodingAllow, (int) enc, 0));
		}

		[Obsolete("Use Encode instead")]
		public NSData DataUsingEncoding (NSStringEncoding enc, bool allowLossyConversion)
		{
			return new NSData (Messaging.IntPtr_objc_msgSend_int_int (Handle, selDataUsingEncodingAllow, (int) enc, allowLossyConversion ? 1 : 0));
		}

		public NSData Encode (NSStringEncoding enc)
		{
			return new NSData (Messaging.IntPtr_objc_msgSend_int_int (Handle, selDataUsingEncodingAllow, (int) enc, 0));
		}

		public NSData Encode (NSStringEncoding enc, bool allowLossyConversion)
		{
			return new NSData (Messaging.IntPtr_objc_msgSend_int_int (Handle, selDataUsingEncodingAllow, (int) enc, allowLossyConversion ? 1 : 0));
		}

		public static NSString FromData (NSData data, NSStringEncoding encoding)
		{
			// Allocate a string, do not init
			IntPtr h = Messaging.IntPtr_objc_msgSend (Class.GetHandle ("NSString"), Selector.Alloc);
			h = Messaging.IntPtr_objc_msgSend_IntPtr_int (h, selInitWithDataEncoding, data.Handle, (int)encoding);
			return new NSString (h);
		}


		// [Export ("initWithData:encoding:")]
		// IntPtr Constructor (NSData data, NSStringEncoding encoding);
		// 
		// [Export ("initWithContentsOfURL:encoding:error")]
		// IntPtr Constructor (NSUrl url, NSStringEncoding encoding, out NSError error);
		// 
		// [Export ("initWithContentsOfURL:usedEncoding:error:")]
		// IntPtr Constructor (NSUrl url, out NSStringEncoding encoding, out NSError error);
		// 
		// [Export ("initWithBytes:length:encoding")]
		// IntPtr Constructor (IntPtr bytes, int length, NSStringEncoding encoding);
		
		public char this [int idx] {
			get {
				return _characterAtIndex (idx);
			}
		}
	}
}
