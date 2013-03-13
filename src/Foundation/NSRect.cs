//
// Copyright 2010, Novell, Inc.
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
using System.Runtime.InteropServices;

// For now, only support MAC64 for NSRect in order to make sure
// we didn't mess up the 32 bit build
#if MAC64
namespace MonoMac.Foundation {
	[StructLayout(LayoutKind.Sequential)]
	public struct NSRect {
		public NSRect(System.Drawing.RectangleF rect)
		{
			Origin.X = rect.Left;
			Origin.Y = rect.Top;
			Size.Width = rect.Width;
			Size.Height = rect.Height;
		}

		public NSPoint Origin;
		public NSSize Size;

#if MAC64
		public double Width { get { return Size.Width; } }
		public double Height { get { return Size.Height; } }
#else
		public float Width { get { return Size.Width; } }
		public float Height { get { return Size.Height; } }
#endif
	}
}
#endif