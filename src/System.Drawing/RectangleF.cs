//
// Extension methods for the RectangelF class useful on OSX.
//
// Authors:
//   Miguel de Icaza
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
//
using System;
using System.Runtime.InteropServices;
using MonoMac;
namespace System.Drawing {

	public static class RectangleFExt {
		public enum Edge {
			MinX, MinY, MaxX, MaxY
		}
		
		[DllImport (Constants.FoundationLibrary)]
		extern static RectangleF NSIntegralRect (RectangleF input);

		public static RectangleF IntegralRect (this RectangleF rect)
		{
			return NSIntegralRect (rect);
		}

		[DllImport (Constants.FoundationLibrary)]
		extern static RectangleF NSInsetRect (RectangleF input, float dx, float dy);

		public static RectangleF Inset (this RectangleF rect, float dx, float dy)
		{
			return NSInsetRect (rect, dx, dy);
		}

		[DllImport (Constants.FoundationLibrary)]
		extern static void NSDivideRect (RectangleF source, out RectangleF slice, out RectangleF rem, float amount, Edge edge);
		
		public static void DivideRect (this RectangleF rect, out RectangleF slice, out RectangleF rem, float amount, Edge edge)
		{
			NSDivideRect (rect, out slice, out rem, amount, edge);
		}
	}
}