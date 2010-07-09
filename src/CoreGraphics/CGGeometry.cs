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
		static extern float CGRectGetMinX (RectangleF rect);
		public static float GetMinX (this RectangleF self)
		{
			return CGRectGetMinX (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern float CGRectGetMidX (RectangleF rect);
		public static float GetMidX (this RectangleF self)
		{
			return CGRectGetMidX (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern float CGRectGetMaxX (RectangleF rect);
		public static float GetMaxX (this RectangleF self)
		{
			return CGRectGetMaxX (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern float CGRectGetMinY (RectangleF rect);
		public static float GetMinY (this RectangleF self)
		{
			return CGRectGetMinY (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern float CGRectGetMidY (RectangleF rect);
		public static float GetMidY (this RectangleF self)
		{
			return CGRectGetMidY (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern float CGRectGetMaxY (RectangleF rect);
		public static float GetMaxY (this RectangleF self)
		{
			return CGRectGetMaxY (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern RectangleF CGRectStandardize (RectangleF rect);
		public static RectangleF Standardize (this RectangleF self)
		{
			return CGRectStandardize (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern bool CGRectIsNull (RectangleF rect);
		public static bool IsNull (this RectangleF self)
		{
			return CGRectIsNull (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern bool CGRectIsInfinite (RectangleF rect);
		public static bool IsInfinite (this RectangleF self)
		{
			return CGRectIsNull (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern RectangleF CGRectInset (RectangleF rect, float dx, float dy);
		public static RectangleF Inset (this RectangleF self, float dx, float dy)
		{
			return CGRectInset (self, dx, dy);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern RectangleF CGRectIntegral (RectangleF rect);
		public static RectangleF Integral (this RectangleF self)
		{
			return CGRectIntegral (self);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern RectangleF CGRectUnion (RectangleF r1, RectangleF r2);
		public static RectangleF UnionWith (this RectangleF self, RectangleF other)
		{
			return CGRectUnion (self, other);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGRectDivide (RectangleF rect, out RectangleF slice, out RectangleF remainder, float amount, CGRectEdge edge);
		public static void Divide (this RectangleF self, float amount, CGRectEdge edge, out RectangleF slice, out RectangleF remainder)
		{
			CGRectDivide (self, out slice, out remainder, amount, edge);
		}
	}
}

