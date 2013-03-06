// 
// CGGeometry.cs: CGGeometry.h helpers
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
using System.Drawing;
using System.Runtime.InteropServices;

using MonoMac;
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;

#if MAC64
using NSInteger = System.Int64;
using NSUInteger = System.UInt64;
using CGFloat = System.Double;
#else
using NSInteger = System.Int32;
using NSUInteger = System.UInt32;
using NSPoint = System.Drawing.PointF;
using NSSize = System.Drawing.SizeF;
using NSRect = System.Drawing.RectangleF;
using CGFloat = System.Single;
#endif

namespace MonoMac.CoreGraphics {

	[Since (3,2)]
	public enum CGRectEdge {
		MinXEdge,
		MinYEdge,
		MaxXEdge,
		MaxYEdge,
	}

	[Since (3,2)]
	public static class RectangleFExtensions {

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern CGFloat CGRectGetMinX (NSRect rect);
		public static float GetMinX (this RectangleF self)
		{
#if MAC64
			return (float)CGRectGetMinX(new NSRect(self));
#else
			return CGRectGetMinX (self);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern CGFloat CGRectGetMidX (NSRect rect);
		public static float GetMidX (this RectangleF self)
		{
#if MAC64
			return (float)CGRectGetMidX(new NSRect(self));
#else
			return CGRectGetMidX (self);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern CGFloat CGRectGetMaxX (NSRect rect);
		public static float GetMaxX (this RectangleF self)
		{
#if MAC64
			return (float)CGRectGetMaxX(new NSRect(self));
#else
			return CGRectGetMaxX (self);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern CGFloat CGRectGetMinY (NSRect rect);
		public static float GetMinY (this RectangleF self)
		{
#if MAC64
			return (float)CGRectGetMinY(new NSRect(self));
#else
			return CGRectGetMinY (self);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern CGFloat CGRectGetMidY (NSRect rect);
		public static float GetMidY (this RectangleF self)
		{
#if MAC64
			return (float)CGRectGetMidY(new NSRect(self));
#else
			return CGRectGetMidY (self);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern CGFloat CGRectGetMaxY (NSRect rect);
		public static float GetMaxY (this RectangleF self)
		{
#if MAC64
			return (float)CGRectGetMaxY(new NSRect(self));
#else
			return CGRectGetMaxY (self);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern NSRect CGRectStandardize (NSRect rect);
		public static RectangleF Standardize (this RectangleF self)
		{
#if MAC64
			NSRect rc = CGRectStandardize (new NSRect(self));
			return new RectangleF((float)rc.Origin.X, (float)rc.Origin.Y, (float)rc.Width, (float)rc.Height);
#else
			return CGRectStandardize (self);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern bool CGRectIsNull (NSRect rect);
		public static bool IsNull (this RectangleF self)
		{
#if MAC64
			return CGRectIsNull (new NSRect(self));
#else
			return CGRectIsNull (self);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern bool CGRectIsInfinite (NSRect rect);
		public static bool IsInfinite (this RectangleF self)
		{
#if MAC64
			return CGRectIsNull (new NSRect(self));
#else
			return CGRectIsNull (self);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern NSRect CGRectInset (NSRect rect, CGFloat dx, CGFloat dy);
		public static RectangleF Inset (this RectangleF self, float dx, float dy)
		{
#if MAC64
			NSRect rc = CGRectInset (new NSRect(self), dx, dy);
			return new RectangleF((float)rc.Origin.X, (float)rc.Origin.Y, (float)rc.Width, (float)rc.Height);
#else
			return CGRectInset (self, dx, dy);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern NSRect CGRectIntegral (NSRect rect);
		public static RectangleF Integral (this RectangleF self)
		{
#if MAC64
			NSRect rc = CGRectIntegral (new NSRect(self));
			return new RectangleF((float)rc.Origin.X, (float)rc.Origin.Y, (float)rc.Width, (float)rc.Height);
#else
			return CGRectIntegral (self);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern NSRect CGRectUnion (NSRect r1, NSRect r2);
		public static RectangleF UnionWith (this RectangleF self, RectangleF other)
		{
#if MAC64
			NSRect rc = CGRectUnion (new NSRect(self), new NSRect(other));
			return new RectangleF((float)rc.Origin.X, (float)rc.Origin.Y, (float)rc.Width, (float)rc.Height);
#else
			return CGRectUnion (self, other);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGRectDivide (NSRect rect, out NSRect slice, out NSRect remainder, CGFloat amount, CGRectEdge edge);
		public static void Divide (this RectangleF self, float amount, CGRectEdge edge, out RectangleF slice, out RectangleF remainder)
		{
#if MAC64
			NSRect _slice, _remainder;
			CGRectDivide (new NSRect(self), out _slice, out _remainder, amount, edge);
			slice = new RectangleF((float)_slice.Origin.X, (float)_slice.Origin.Y, (float)_slice.Width, (float)_slice.Height);
			remainder = new RectangleF((float)_remainder.Origin.X, (float)_remainder.Origin.Y, (float)_remainder.Width, (float)_remainder.Height);
#else
			CGRectDivide (self, out slice, out remainder, amount, edge);
#endif
		}
	}
}

