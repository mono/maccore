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

namespace MonoMac.ImageIO {
	
	public class CGImageSource : INativeObject, IDisposable
	{
		internal IntPtr handle;

		// invoked by marshallers
		internal CGImageSource (IntPtr handle) : this (handle, false)
		{
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal CGImageSource (IntPtr handle, bool owns)
		{
			this.handle = handle;
			if (!owns)
				CFObject.CFRetain (handle);
		}
		
		~CGImageSource ()
		{
			Dispose (false);
		}
		
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public IntPtr Handle {
			get { return handle; }
		}
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CFObject.CFRelease (handle);
				handle = IntPtr.Zero;
			}
		}
				
		[DllImport (Constants.ImageIOLibrary)]
		extern static IntPtr CGImageSourceCreateWithURL(IntPtr url, IntPtr options);
		public static CGImageSource CreateWithURL (NSUrl url, NSDictionary options)
		{
			if (url == null)
				throw new ArgumentNullException ("url");
			
			return new CGImageSource (CGImageSourceCreateWithURL (url.Handle, options == null ? IntPtr.Zero : options.Handle));
		}
		
		[DllImport (Constants.ImageIOLibrary)]
		extern static IntPtr CGImageSourceCreateImageAtIndex(IntPtr isrc, int index, IntPtr options);
		public static CGImage CreateImageAtIndex (CGImageSource isrc, int index, NSDictionary options)
		{
			if (isrc == null)
				throw new ArgumentNullException ("isrc");
			
			return new CGImage(CGImageSourceCreateImageAtIndex(isrc.Handle, index, options == null ? IntPtr.Zero : options.Handle));
		}
	}
}
