// 
// CGPatterncs.cs: Implements the managed CGPattern
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

	public enum CGPatternTiling {
		NoDistortion,
		ConstantSpacingMinimalDistortion,
		ConstantSpacing
	}

	delegate void DrawPatternCallback (IntPtr voidptr, IntPtr cgcontextref);
	delegate void ReleaseInfoCallback (IntPtr voidptr);

	[StructLayout (LayoutKind.Sequential)]
	struct CGPatternCallbacks {
		internal uint version;
		internal DrawPatternCallback draw;
		internal ReleaseInfoCallback release;
	}
		
	public class CGPattern : INativeObject, IDisposable {
		internal IntPtr handle;

		/* invoked by marshallers */
		public CGPattern (IntPtr handle)
		{
			this.handle = handle;
			CGPatternRetain (this.handle);
		}

		[Preserve (Conditional=true)]
		internal CGPattern (IntPtr handle, bool owns)
		{
			this.handle = handle;
			if (!owns)
				CGPatternRetain (this.handle);
		}
		
		// This is what we expose on the API
		public delegate void DrawPattern (CGContext ctx);
		DrawPattern draw_pattern;
		[DllImport(Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPatternCreate(IntPtr info, NSRect bounds, CGAffineTransform matrix,
						     CGFloat xStep, CGFloat yStep,
						     CGPatternTiling tiling, bool isColored,
						     ref CGPatternCallbacks callbacks);

		CGPatternCallbacks callbacks;
		GCHandle gch;
		
		public CGPattern (RectangleF bounds, CGAffineTransform matrix, float xStep, float yStep, CGPatternTiling tiling, bool isColored, DrawPattern drawPattern)
		{
			if (drawPattern == null)
				throw new ArgumentNullException ("drawPattern");

			callbacks.draw = DrawCallback;
			callbacks.release = ReleaseCallback;
			callbacks.version = 0;
			this.draw_pattern = drawPattern;

			gch = GCHandle.Alloc (this);
#if MAC64
			handle = CGPatternCreate (GCHandle.ToIntPtr (gch) , new NSRect(bounds), matrix, xStep, yStep, tiling, isColored, ref callbacks);
#else
			handle = CGPatternCreate (GCHandle.ToIntPtr (gch) , bounds, matrix, xStep, yStep, tiling, isColored, ref callbacks);
#endif
		}

		IntPtr last_cgcontext_ptr;
		WeakReference last_cgcontext;

#if !MONOMAC
		[MonoPInvokeCallback (typeof (DrawPatternCallback))]
#endif
		static void DrawCallback (IntPtr voidptr, IntPtr cgcontextptr)
		{
			GCHandle gch = GCHandle.FromIntPtr (voidptr);
			CGPattern container = (CGPattern) gch.Target;
			CGContext ctx = null;
			
			if (cgcontextptr == container.last_cgcontext_ptr)
				ctx = container.last_cgcontext.Target as CGContext;

			if (ctx == null){
				ctx = new CGContext (cgcontextptr);
				container.last_cgcontext = new WeakReference (ctx);
				container.last_cgcontext_ptr = cgcontextptr;
			}
			container.draw_pattern (ctx);
		}

#if !MONOMAC
		[MonoPInvokeCallback (typeof (ReleaseInfoCallback))]
#endif
		static void ReleaseCallback (IntPtr voidptr)
		{
			GCHandle gch = GCHandle.FromIntPtr (voidptr);
			gch.Free ();
		}
		
		~CGPattern ()
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
		extern static void CGPatternRelease (IntPtr handle);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPatternRetain (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CGPatternRelease (handle);
				handle = IntPtr.Zero;
			}
			last_cgcontext = null;
		}
	}
}
