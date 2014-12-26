// 
// CTFrame.cs: Implements the managed CTFrame
//
// Authors: Mono Team
//     
// Copyright 2010 Novell, Inc
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
using System.Drawing;
using System.Runtime.InteropServices;

using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
using MonoMac.CoreGraphics;

namespace MonoMac.CoreText {

	[Since (3,2)]
	[Flags]
	public enum CTFrameProgression : uint {
		TopToBottom = 0,
		RightToLeft = 1
	}

	[Since (4,2)]
	public enum CTFramePathFillRule {
		EvenOdd,
		WindingNumber
	}

	[Since (3,2)]
	public static class CTFrameAttributeKey {

		public static readonly NSString Progression;

		[Since (4,2)]
		public static readonly NSString PathFillRule;
		[Since (4,2)]
		public static readonly NSString PathWidth;
		[Since (4,3)]
		public static readonly NSString ClippingPaths;
		[Since (4,3)]
		public static readonly NSString PathClippingPath;
		
		static CTFrameAttributeKey ()
		{
			var handle = Dlfcn.dlopen (Constants.CoreTextLibrary, 0);
			if (handle == IntPtr.Zero)
				return;
			try {
				Progression = Dlfcn.GetStringConstant (handle, "kCTFrameProgressionAttributeName");
				PathFillRule = Dlfcn.GetStringConstant (handle, "kCTFramePathFillRuleAttributeName");
				PathWidth = Dlfcn.GetStringConstant (handle, "kCTFramePathWidthAttributeName");
				ClippingPaths = Dlfcn.GetStringConstant (handle, "kCTFrameClippingPathsAttributeName");
				PathClippingPath = Dlfcn.GetStringConstant (handle, "kCTFramePathClippingPathAttributeName");
			}
			finally {
				Dlfcn.dlclose (handle);
			}
		}
	}

	[Since (3,2)]
	public class CTFrameAttributes {

		public CTFrameAttributes ()
			: this (new NSMutableDictionary ())
		{
		}

		public CTFrameAttributes (NSDictionary dictionary)
		{
			if (dictionary == null)
				throw new ArgumentNullException ("dictionary");
			Dictionary = dictionary;
		}

		public NSDictionary Dictionary {get; private set;}

		public CTFrameProgression? Progression {
			get {
				var value = Adapter.GetUInt32Value (Dictionary, CTFrameAttributeKey.Progression);
				return !value.HasValue ? null : (CTFrameProgression?) value.Value;
			}
			set {
				Adapter.SetValue (Dictionary, CTFrameAttributeKey.Progression,
						value.HasValue ? (uint?) value.Value : null);
			}
		}
	}

	[Since (3,2)]
	public class CTFrame : CFType {
		internal CTFrame (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}

		~CTFrame ()
		{
			Dispose (false);
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static NSRange CTFrameGetStringRange (IntPtr handle);
		[DllImport (Constants.CoreTextLibrary)]
		extern static NSRange CTFrameGetVisibleStringRange (IntPtr handle);
		
		public NSRange GetStringRange ()
		{
			return CTFrameGetStringRange (Handle);
		}

		public NSRange GetVisibleStringRange ()
		{
			return CTFrameGetVisibleStringRange (Handle);
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static IntPtr CTFrameGetPath (IntPtr handle);
		
		public CGPath GetPath ()
		{
			IntPtr h = CTFrameGetPath (Handle);
			return h == IntPtr.Zero ? null : new CGPath (h, false);
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static IntPtr CTFrameGetFrameAttributes (IntPtr handle);

		public CTFrameAttributes GetFrameAttributes ()
		{
			var attrs = (NSDictionary) Runtime.GetNSObject (CTFrameGetFrameAttributes (Handle));
			return attrs == null ? null : new CTFrameAttributes (attrs);
		}
		
		[DllImport (Constants.CoreTextLibrary)]
		extern static IntPtr CTFrameGetLines (IntPtr handle);

		public CTLine [] GetLines ()
		{
			var cfArrayRef = CTFrameGetLines (Handle);
			if (cfArrayRef == IntPtr.Zero)
				return new CTLine [0];

			return NSArray.ArrayFromHandleFunc<CTLine> (cfArrayRef, (p) => {
					// We need to take a ref, since we dispose it later.
					return new CTLine (p, false);
				});
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static void CTFrameGetLineOrigins(IntPtr handle, NSRange range, [Out] PointF[] origins);
		public void GetLineOrigins (NSRange range, PointF[] origins)
		{
			if (origins == null)
				throw new ArgumentNullException ("origins");
			if (range.Length != 0 && origins.Length < range.Length)
				throw new ArgumentException ("origins must contain at least range.Length elements.", "origins");
			else if (origins.Length < CFArray.GetCount (CTFrameGetLines (Handle)))
				throw new ArgumentException ("origins must contain at least GetLines().Length elements.", "origins");
			CTFrameGetLineOrigins (Handle, range, origins);
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static void CTFrameDraw (IntPtr handle, IntPtr context);

		public void Draw (CGContext ctx)
		{
			if (ctx == null)
				throw new ArgumentNullException ("ctx");

			CTFrameDraw (Handle, ctx.Handle);
		}
	}
}

