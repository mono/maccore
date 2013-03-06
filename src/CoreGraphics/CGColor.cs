// 
// CGColor.cs: Implements the managed CGColor
//
// Authors: Mono Team
//     
// Copyright 2009 Novell, Inc
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

using MonoMac.ObjCRuntime;
using MonoMac.CoreFoundation;
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

	public class CGColor : INativeObject, IDisposable {
		internal IntPtr handle;
		
		~CGColor ()
		{
			Dispose (false);
		}

		//
		// Never call from this class, so we need to take a ref
		//
		public CGColor (IntPtr handle)
		{
			this.handle = handle;
			CGColorRetain (handle);
		}

		[Preserve (Conditional=true)]
		internal CGColor (IntPtr handle, bool owns)
		{
			if (!owns)
				CGColorRetain (handle);

			this.handle = handle;
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public IntPtr Handle {
			get { return handle; }
		}

		[DllImport(Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorCreate(IntPtr space, CGFloat [] components);

		public CGColor (CGColorSpace colorspace, float [] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");
			if (colorspace == null)
				throw new ArgumentNullException ("colorspace");
			if (colorspace.handle == IntPtr.Zero)
				throw new ObjectDisposedException ("colorspace");
			CGFloat[] _components = new CGFloat[components.Length];
			Array.Copy (components, _components, components.Length);
			handle = CGColorCreate (colorspace.handle, _components);
		}

		[DllImport(Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorCreateGenericGray(CGFloat gray, CGFloat alpha);
		public CGColor (float gray, float alpha)
		{
			handle = CGColorCreateGenericGray (gray, alpha);
		}

		[DllImport(Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorCreateGenericRGB (CGFloat red, CGFloat green, CGFloat blue, CGFloat alpha);
		public CGColor (float red, float green, float blue, float alpha)
		{
			handle = CGColorCreateGenericRGB (red, green, blue, alpha);
		}

		public CGColor (float red, float green, float blue)
		{
			handle = CGColorCreateGenericRGB (red, green, blue, 1.0f);
		}

		[DllImport(Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorGetConstantColor(IntPtr cfstring_colorName);
		public CGColor (string name)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			
			using (var s = new CFString (name)){
				handle = CGColorGetConstantColor (s.handle);
				if (handle == IntPtr.Zero)
					throw new ArgumentException ("name");
			}
		}

		[DllImport(Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorCreateWithPattern(IntPtr space, IntPtr pattern, CGFloat [] components);
		public CGColor (CGColorSpace colorspace, CGPattern pattern, float [] components)
		{
			if (colorspace == null)
				throw new ArgumentNullException ("colorspace");
			if (colorspace.handle == IntPtr.Zero)
				throw new ObjectDisposedException ("colorspace");
			if (pattern == null)
				throw new ArgumentNullException ("pattern");
			if (components == null)
				throw new ArgumentNullException ("components");

			CGFloat[] _components = new CGFloat[components.Length];
			Array.Copy (components, _components, components.Length);
			handle = CGColorCreateWithPattern (colorspace.handle, pattern.handle, _components);
			if (handle == IntPtr.Zero)
				throw new ArgumentException ();
		}

		[DllImport(Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorCreateCopyWithAlpha(IntPtr color, CGFloat alpha);
		public CGColor (CGColor source, float alpha)
		{
			if (source == null)
				throw new ArgumentNullException ("source");
			if (source.handle == IntPtr.Zero)
				throw new ObjectDisposedException ("source");
			
			handle = CGColorCreateCopyWithAlpha (source.handle, alpha);
		}

		[DllImport(Constants.CoreGraphicsLibrary)]
		extern static bool CGColorEqualToColor(IntPtr color1, IntPtr color2);

		public static bool operator == (CGColor color1, CGColor color2)
		{
			return Object.Equals (color1, color2);
		}

		public static bool operator != (CGColor color1, CGColor color2)
		{
			return !Object.Equals (color1, color2);
		}

		public override int GetHashCode ()
		{
			return handle.GetHashCode ();
		}

		public override bool Equals (object o)
		{
			CGColor other = o as CGColor;
			if (other == null)
				return false;

			return CGColorEqualToColor (this.handle, other.handle);
		}

		[DllImport(Constants.CoreGraphicsLibrary)]
		extern static int CGColorGetNumberOfComponents(IntPtr color);
		public int NumberOfComponents {
			get {
				return CGColorGetNumberOfComponents (handle);
			}
		}

		[DllImport(Constants.CoreGraphicsLibrary)]
		extern static unsafe CGFloat *CGColorGetComponents(IntPtr color);
		public float [] Components {
			get {
				int n = NumberOfComponents;
				float [] result = new float[n];
				unsafe {
					CGFloat *cptr = CGColorGetComponents (handle);

					for (int i = 0; i < n; i++){
						result [i] = (float)cptr [i];
					}
				}
				return result;
			}
		}

		[DllImport(Constants.CoreGraphicsLibrary)]
		extern static CGFloat CGColorGetAlpha(IntPtr color);
		public float Alpha {
			get {
				return (float)CGColorGetAlpha (handle);
			}
		}
		
		[DllImport(Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorGetColorSpace(IntPtr color);
		public CGColorSpace ColorSpace {
			get {
				return new CGColorSpace (CGColorGetColorSpace (handle));
			}
		}
		
		[DllImport(Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorGetPattern(IntPtr color);
		public CGPattern Pattern {
			get {
				return new CGPattern (CGColorGetPattern (handle));
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGColorRetain (IntPtr handle);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGColorRelease (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CGColorRelease (handle);
				handle = IntPtr.Zero;
			}
		}
	}
}
