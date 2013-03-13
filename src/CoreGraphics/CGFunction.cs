// 
// CGFunction.cs: Implements the managed CGCuntion
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

	public class CGFunction : INativeObject, IDisposable {
		internal IntPtr handle;
		GCHandle gch;
		CGFunctionEvaluate evaluate;
		
		// invoked by marshallers
		internal CGFunction (IntPtr handle)
			: this (handle, false)
		{
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal CGFunction (IntPtr handle, bool owns)
		{
			this.handle = handle;
			if (!owns)
				CGFunctionRetain (handle);
		}
		
		~CGFunction ()
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
		extern static void CGFunctionRelease (IntPtr handle);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGFunctionRetain (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CGFunctionRelease (handle);
				handle = IntPtr.Zero;
				gch.Free ();
				evaluate = null;
			}
		}

		unsafe delegate void CGFunctionEvaluateCallback (IntPtr info, CGFloat *data, CGFloat *outData);
			
		[StructLayout (LayoutKind.Sequential)]
		struct CGFunctionCallbacks {
			public uint version;
			public CGFunctionEvaluateCallback evaluate;
			public IntPtr release;
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGFunctionCreate (IntPtr data, IntPtr domainCount, CGFloat [] domain, IntPtr rangeDomain, CGFloat [] range, ref CGFunctionCallbacks callbacks);
		
		unsafe public delegate void CGFunctionEvaluate (CGFloat *data, CGFloat *outData);

		public unsafe CGFunction (float [] domain, float [] range, CGFunctionEvaluate callback)
		{
			if (domain != null){
				if ((domain.Length % 2) != 0)
					throw new ArgumentException ("The domain array must consist of pairs of values", "domain");
			}
			if (range != null) {
				if ((range.Length % 2) != 0)
					throw new ArgumentException ("The range array must consist of pairs of values", "range");
			}
			if (callback == null)
				throw new ArgumentNullException ("callback");

			this.evaluate = callback;

			CGFunctionCallbacks cbacks;
			cbacks.version = 0;
			cbacks.evaluate = new CGFunctionEvaluateCallback (EvaluateCallback);
			cbacks.release = IntPtr.Zero;

			gch = GCHandle.Alloc (this);
#if MAC64
			CGFloat[] _domain = new CGFloat[domain.Length];
			Array.Copy(domain, _domain, domain.Length);
			CGFloat[] _range = new CGFloat[range.Length];
			Array.Copy(range, _range, range.Length);
			handle = CGFunctionCreate (GCHandle.ToIntPtr (gch), domain != null ? new IntPtr(domain.Length/2) : IntPtr.Zero,
			                           _domain, range != null ? new IntPtr(range.Length/2) : IntPtr.Zero, _range, ref cbacks);
#else
			handle = CGFunctionCreate (GCHandle.ToIntPtr (gch), domain != null ? new IntPtr(domain.Length/2) : IntPtr.Zero,
			                           domain, range != null ? new IntPtr(range.Length/2) : IntPtr.Zero, range, ref cbacks);
#endif	
		}

#if !MONOMAC
		[MonoPInvokeCallback (typeof (CGFunctionEvaluateCallback))]
#endif
		unsafe static void EvaluateCallback (IntPtr info, CGFloat *input, CGFloat *output)
		{
			GCHandle lgc = GCHandle.FromIntPtr (info);
			CGFunction container = (CGFunction) lgc.Target;

			container.evaluate (input, output);
		}
	}
}
