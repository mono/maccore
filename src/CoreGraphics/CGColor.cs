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
		extern static IntPtr CGColorCreate(IntPtr space, float [] components);

		public CGColor (CGColorSpace colorspace, float [] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");
			if (colorspace == null)
				throw new ArgumentNullException ("colorspace");
			if (colorspace.handle == IntPtr.Zero)
				throw new ObjectDisposedException ("colorspace");
			
			handle = CGColorCreate (colorspace.handle, components);
		}

		[DllImport(Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorCreateGenericGray(float gray, float alpha);
		public CGColor (float gray, float alpha)
		{
			handle = CGColorCreateGenericGray (gray, alpha);
		}

		[DllImport(Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorCreateGenericRGB (float red, float green, float blue, float alpha);
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
		extern static IntPtr CGColorCreateWithPattern(IntPtr space, IntPtr pattern, float [] components);
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

			handle = CGColorCreateWithPattern (colorspace.handle, pattern.handle, components);
			if (handle == IntPtr.Zero)
				throw new ArgumentException ();
		}

		[DllImport(Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGColorCreateCopyWithAlpha(IntPtr color, float alpha);
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
		extern static unsafe float *CGColorGetComponents(IntPtr color);
		public float [] Components {
			get {
				int n = NumberOfComponents;
				float [] result = new float [n];
				unsafe {
					float *cptr = CGColorGetComponents (handle);

					for (int i = 0; i < n; i++){
						result [i] = cptr [i];
					}
				}
				return result;
			}
		}

		[DllImport(Constants.CoreGraphicsLibrary)]
		extern static float CGColorGetAlpha(IntPtr color);
		public float Alpha {
			get {
				return CGColorGetAlpha (handle);
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
