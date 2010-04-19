// 
// CGShading.cs: Implements the managed CGShading
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

namespace MonoMac.CoreGraphics {

	public class CGShading : INativeObject, IDisposable {
		internal IntPtr handle;

		internal CGShading (IntPtr handle)
		{
			this.handle = handle;
		}
		

		[DllImport(Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGShadingCreateAxial(IntPtr space, PointF start, PointF end, CGFunction function, bool extendStart, bool extendEnd);
		

		[DllImport(Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGShadingCreateRadial(IntPtr space, PointF start, CGFloat startRadius, PointF end, CGFloat endRadius, CGFunctionRef function, bool extendStart, bool extendEnd);

		~CGShading ()
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
	
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGShadingRelease (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CGShadingRelease (handle);
				handle = IntPtr.Zero;
			}
		}
	}
}
