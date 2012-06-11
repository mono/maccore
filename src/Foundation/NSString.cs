//
// Copyright 2010, Novell, Inc.
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
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {
	public enum NSStringEncoding : uint {
		ASCIIStringEncoding = 1,
		NEXTSTEP = 2,
		JapaneseEUC = 3,
		UTF8 = 4,
		ISOLatin1 = 5,
		Symbol = 6,
		NonLossyASCII = 7,
		ShiftJIS = 8,
		ISOLatin2 = 9,
		Unicode = 10,
		WindowsCP1251 = 11,
		WindowsCP1252 = 12,
		WindowsCP1253 = 13,
		WindowsCP1254 = 14,
		WindowsCP1250 = 15,
		ISO2022JP = 21,
		MacOSRoman = 30,
		UTF16BigEndian = 0x90000100,
		UTF16LittleEndian = 0x94000100,
		UTF32 = 0x8c000100,
		UTF32BigEndian = 0x98000100,
		UTF32LittleEndian = 0x9c000100,
	};
	
	public enum NSStringCompareOptions : uint {
		CaseInsensitiveSearch = 1,
		LiteralSearch = 2,
		BackwardsSearch = 4,
		AnchoredSearch = 8,
		NumericSearch = 64,
		DiacriticInsensitiveSearch = 128,
		WidthInsensitiveSearch = 256,
		ForcedOrderingSearch = 512,
		RegularExpressionSearch = 1024
	}

#if GENERATOR && !MONOMAC
	[Register ("NSString")]
#endif
	public partial class NSString : NSObject {
		static IntPtr selUTF8String = Selector.sel_registerName ("UTF8String");
		static IntPtr selInitWithUTF8String = Selector.sel_registerName ("initWithUTF8String:");
		static IntPtr selInitWithCharactersLength = Selector.sel_registerName ("initWithCharacters:length:");

#if COREBUILD
		static IntPtr class_ptr = Class.GetHandle ("NSString");
#endif
		
#if GENERATOR && !MONOMAC
		public NSString (IntPtr handle) : base (handle) {
		}
#endif

		public static IntPtr CreateNative (string str)
		{
			if (str == null)
				return IntPtr.Zero;
			
			unsafe {
				fixed (char *ptrFirstChar = str){
					var handle = Messaging.intptr_objc_msgSend (class_ptr, Selector.Alloc);
					handle = Messaging.intptr_objc_msgsend_intptr_int (handle, selInitWithCharactersLength, (IntPtr) ptrFirstChar, str.Length);
					return handle;
				}
			}
		}

		public static void ReleaseNative (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				return;
			Messaging.void_objc_msgSend (handle, Selector.Release);
		}

	
#if false
		public NSString (string str) {
			if (str == null)
				throw new ArgumentNullException ("str");

			IntPtr bytes = Marshal.StringToHGlobalAuto (str);

			Handle = (IntPtr) Messaging.intptr_objc_msgSend_intptr (Handle, selInitWithUTF8String, bytes);

			Marshal.FreeHGlobal (bytes);
		}
#else
	
		public NSString (string str) {
			if (str == null)
				throw new ArgumentNullException ("str");

			unsafe {
				fixed (char *ptrFirstChar = str){
					Handle = Messaging.intptr_objc_msgsend_intptr_int (Handle, selInitWithCharactersLength, (IntPtr) ptrFirstChar, str.Length);
				}
			}
		}
#endif
	
		public unsafe override string ToString ()
		{
			if (Handle == IntPtr.Zero)
				return null;
			return FromHandle (Handle);
		}

		public static implicit operator string (NSString str)
		{
			if (((object) str) == null)
				return null;
			return str.ToString ();
		}

		public static explicit operator NSString (string str)
		{
			if (str == null)
				return null;
			return new NSString (str);
		}

		public unsafe static string FromHandle (IntPtr usrhandle)
		{
			if (usrhandle == IntPtr.Zero)
				return null;
			
			return Marshal.PtrToStringAuto (Messaging.intptr_objc_msgSend (usrhandle, selUTF8String));
		}

		public static bool Equals (NSString a, NSString b)
                {
			if ((a as object) == (b as object))
				return true;

			if (((object) a) == null || ((object) b) == null)
				return false;

			if (a.Handle == b.Handle)
				return true;
#if GENERATOR || COREBUILD
			return a.ToString ().Equals (b.ToString ());
#else
			return a.IsEqualTo (b.Handle);
#endif
		}

		public static bool operator == (NSString a, NSString b)
		{
			return Equals (a, b);
		}
		
		public static bool operator != (NSString a, NSString b)
		{
			return !Equals (a, b);
		}

		public override bool Equals (Object obj)
		{
			return Equals (this, obj as NSString);
		}

		public override int GetHashCode ()
		{
			return (int) this.Handle;
		}
#if !MONOMAC && !COREBUILD
		[Obsolete ("Use the version with a `ref float actualFontSize`")]
		public System.Drawing.SizeF DrawString (System.Drawing.PointF point, float width, MonoTouch.UIKit.UIFont font, float minFontSize, float actualFontSize, MonoTouch.UIKit.UILineBreakMode breakMode, MonoTouch.UIKit.UIBaselineAdjustment adjustment)
		{
			float temp = actualFontSize;
			return DrawString (point, width, font, minFontSize, ref temp, breakMode, adjustment);
		}
#endif
	}
}
