// 
// CVBuffer.cs: Implements the managed CVBuffer
//
// Authors: Miguel de Icaza
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
using System.Runtime.InteropServices;
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
using System.Drawing;

namespace MonoMac.CoreVideo {

	public class CVBuffer : INativeObject, IDisposable {
		IntPtr handle;
		internal CVBuffer (IntPtr handle)
		{
			this.handle = handle;
			CVBufferRetain (handle);
		}

		[Preserve (Conditional=true)]
		internal CVBuffer (IntPtr handle, bool owns)
		{
			this.handle = handle;
			if (!owns)
				CVBufferRetain (this.handle);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static IntPtr CVBufferRetain (IntPtr handle);

		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVBufferRelease (IntPtr handle);

		~CVBuffer ()
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
				CVBufferRelease (handle);
				handle = IntPtr.Zero;
			}
		}
	}
}
