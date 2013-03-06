// 
// CGPath.cs: Implements the managed CGPath
//
// Authors: Mono Team
//     
// Copyright 2009 Novell, Inc
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
	public enum CGPathElementType {
		MoveToPoint,
		AddLineToPoint,
		AddQuadCurveToPoint,
		AddCurveToPoint,
		CloseSubpath
	}

	public struct CGPathElement {
		public CGPathElementType Type;

		public CGPathElement (int t)
		{
			Type = (CGPathElementType) t;
			Point1 = Point2 = Point3 = new PointF (0,0);
		}
		
		// Set for MoveToPoint, AddLineToPoint, AddQuadCurveToPoint, AddCurveToPoint
		public PointF Point1;

		// Set for AddQuadCurveToPoint, AddCurveToPoint
		public PointF Point2;

		// Set for AddCurveToPoint
		public PointF Point3;
	}
	
	public class CGPath : INativeObject, IDisposable {
		internal IntPtr handle;

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateMutable();
		public CGPath ()
		{
			handle = CGPathCreateMutable ();
		}

#if !MONOMAC
		[Since (5,0)]
		public CGPath (CGPath reference, CGAffineTransform transform)
		{
			if (reference == null)
				throw new ArgumentNullException ("reference");
			handle = CGPathCreateMutableCopyByTransformingPath (reference.Handle, ref transform);
		}
#endif
	
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateMutableCopy(IntPtr path);
		public CGPath (CGPath basePath)
		{
			if (basePath == null)
				throw new ArgumentNullException ("basePath");
			handle = CGPathCreateMutableCopy (basePath.handle);
		}

		//
		// For use by marshallrs
		//
		public CGPath (IntPtr handle)
		{
			CGPathRetain (handle);
			this.handle = handle;
		}

		// Indicates that we own it `owns'
		[Preserve (Conditional=true)]
		internal CGPath (IntPtr handle, bool owns)
		{
			if (!owns)
				CGPathRetain (handle);
			
			this.handle = handle;
		}
		
		~CGPath ()
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
		extern static void CGPathRelease (IntPtr handle);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathRetain (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CGPathRelease (handle);
				handle = IntPtr.Zero;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPathEqualToPath(IntPtr path1, IntPtr path2);

		public static bool operator == (CGPath path1, CGPath path2)
		{
			return Object.Equals (path1, path2);
		}

		public static bool operator != (CGPath path1, CGPath path2)
		{
			return !Object.Equals (path1, path2);
		}

		public override int GetHashCode ()
		{
			return handle.GetHashCode ();
		}

		public override bool Equals (object o)
		{
			CGPath other = o as CGPath;
			if (other == null)
				return false;

			return CGPathEqualToPath (this.handle, other.handle);
		}
       
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathMoveToPoint(IntPtr path, ref CGAffineTransform m, CGFloat x, CGFloat y);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathMoveToPoint(IntPtr path, IntPtr zero, CGFloat x, CGFloat y);
		public void MoveToPoint (float x, float y)
		{
			CGPathMoveToPoint (handle, IntPtr.Zero, x, y);

		}

		public void MoveToPoint (PointF point)
		{
			CGPathMoveToPoint (handle, IntPtr.Zero, point.X, point.Y);
		}
		
		public void MoveToPoint (CGAffineTransform transform, float x, float y)
		{
			CGPathMoveToPoint (handle, ref transform, x, y);
		}

		public void MoveToPoint (CGAffineTransform transform, PointF point)
		{
			CGPathMoveToPoint (handle, ref transform, point.X, point.Y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddLineToPoint(IntPtr path, ref CGAffineTransform m, CGFloat x, CGFloat y);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddLineToPoint(IntPtr path, IntPtr m, CGFloat x, CGFloat y);

		[Advice ("Use AddLineToPoint instead")] // Bad name
		public void CGPathAddLineToPoint (float x, float y)
		{
			AddLineToPoint (x, y);
		}

		public void AddLineToPoint (float x, float y)
		{
			CGPathAddLineToPoint (handle, IntPtr.Zero, x, y);
		}

		public void AddLineToPoint (PointF point)
		{
			CGPathAddLineToPoint (handle, IntPtr.Zero, point.X, point.Y);
		}
		
		[Advice ("Use AddLineToPoint instead")] // Bad name
		public void CGPathAddLineToPoint (CGAffineTransform transform, float x, float y)
		{
			AddLineToPoint (transform, x, y);
		}

		public void AddLineToPoint (CGAffineTransform transform, float x, float y)
		{
			CGPathAddLineToPoint (handle, ref transform, x, y);
		}

		public void AddLineToPoint (CGAffineTransform transform, PointF point)
		{
			CGPathAddLineToPoint (handle, ref transform, point.X, point.Y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddQuadCurveToPoint(IntPtr path, ref CGAffineTransform m, CGFloat cpx, CGFloat cpy, CGFloat x, CGFloat y);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddQuadCurveToPoint(IntPtr path, IntPtr zero, CGFloat cpx, CGFloat cpy, CGFloat x, CGFloat y);
		public void AddQuadCurveToPoint (float cpx, float cpy, float x, float y)
		{
			CGPathAddQuadCurveToPoint (handle, IntPtr.Zero, cpx, cpy, x, y);
		}

		public void AddQuadCurveToPoint (CGAffineTransform transform, float cpx, float cpy, float x, float y)
		{
			CGPathAddQuadCurveToPoint (handle, ref transform, cpx, cpy, x, y);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddCurveToPoint(IntPtr path, ref CGAffineTransform m, CGFloat cp1x, CGFloat cp1y, CGFloat cp2x, CGFloat cp2y, CGFloat x, CGFloat y);
		public void AddCurveToPoint (CGAffineTransform transform, float cp1x, float cp1y, float cp2x, float cp2y, float x, float y)
		{
			CGPathAddCurveToPoint (handle, ref transform, cp1x, cp1y, cp2x, cp2y, x, y);
		}

		public void AddCurveToPoint (CGAffineTransform transform, PointF cp1, PointF cp2, PointF point)
		{
			CGPathAddCurveToPoint (handle, ref transform, cp1.X, cp1.Y, cp2.X, cp2.Y, point.X, point.Y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddCurveToPoint(IntPtr path, IntPtr zero, CGFloat cp1x, CGFloat cp1y, CGFloat cp2x, CGFloat cp2y, CGFloat x, CGFloat y);
		public void AddCurveToPoint (float cp1x, float cp1y, float cp2x, float cp2y, float x, float y)
		{
			CGPathAddCurveToPoint (handle, IntPtr.Zero, cp1x, cp1y, cp2x, cp2y, x, y);
		}
			
		public void AddCurveToPoint (PointF cp1, PointF cp2, PointF point)
		{
			CGPathAddCurveToPoint (handle, IntPtr.Zero, cp1.X, cp1.Y, cp2.X, cp2.Y, point.X, point.Y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathCloseSubpath(IntPtr path);
		public void CloseSubpath ()
		{
			CGPathCloseSubpath (handle);
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddRect(IntPtr path, ref CGAffineTransform m, NSRect rect);
		public void AddRect (CGAffineTransform transform, RectangleF rect)
		{
#if MAC64
			CGPathAddRect (handle, ref transform, new NSRect(rect));
#else
			CGPathAddRect (handle, ref transform, rect);
#endif
		}
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddRect(IntPtr path, IntPtr zero, NSRect rect);
		public void AddRect (RectangleF rect)
		{
#if MAC64
			CGPathAddRect (handle, IntPtr.Zero, new NSRect(rect));
#else
			CGPathAddRect (handle, IntPtr.Zero, rect);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddRects(IntPtr path, ref CGAffineTransform m, NSRect [] rects, IntPtr size_t_count);
		public void AddRects (CGAffineTransform m, RectangleF [] rects)
		{
#if MAC64
			NSRect[] _rects = new NSRect[rects.Length];
			for( int i=0; i<rects.Length; i++ )
				_rects[i] = new NSRect(rects[i]);
			CGPathAddRects (handle, ref m, _rects, new IntPtr(rects.Length));
#else
			CGPathAddRects (handle, ref m, rects, new IntPtr(rects.Length));
#endif
		}
		public void AddRects (CGAffineTransform m, RectangleF [] rects, int count)
		{
			if (count > rects.Length)
				throw new ArgumentException ("counts");
#if MAC64
			NSRect[] _rects = new NSRect[rects.Length];
			for( int i=0; i<rects.Length; i++ )
				_rects[i] = new NSRect(rects[i]);
			CGPathAddRects (handle, ref m, _rects, new IntPtr(rects.Length));
#else
			CGPathAddRects (handle, ref m, rects, new IntPtr(count));
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddRects(IntPtr path, IntPtr Zero, NSRect [] rects, IntPtr size_t_count);
		public void AddRects (RectangleF [] rects)
		{
#if MAC64
			NSRect[] _rects = new NSRect[rects.Length];
			for( int i=0; i<rects.Length; i++ )
				_rects[i] = new NSRect(rects[i]);
			CGPathAddRects (handle, IntPtr.Zero, _rects, new IntPtr(rects.Length));
#else
			CGPathAddRects (handle, IntPtr.Zero, rects, new IntPtr(rects.Length));
#endif
		}
		public void AddRects (RectangleF [] rects, int count)
		{
			if (count > rects.Length)
				throw new ArgumentException ("count");
#if MAC64
			NSRect[] _rects = new NSRect[rects.Length];
			for( int i=0; i<rects.Length; i++ )
				_rects[i] = new NSRect(rects[i]);
			CGPathAddRects (handle, IntPtr.Zero, _rects, new IntPtr(count));
#else
			CGPathAddRects (handle, IntPtr.Zero, rects, new IntPtr(count));
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddLines(IntPtr path, ref CGAffineTransform m, NSPoint [] points, IntPtr size_t_count);
		public void AddLines (CGAffineTransform m, PointF [] points)
		{
#if MAC64
			NSPoint[] _points = new NSPoint[points.Length];
			for( int i=0; i<points.Length; i++ )
				_points[i] = new NSPoint(points[i]);
			CGPathAddLines (handle, ref m, _points, new IntPtr(points.Length));
#else
			CGPathAddLines (handle, ref m, points, new IntPtr(points.Length));
#endif
		}
		public void AddLines (CGAffineTransform m, PointF [] points, int count)
		{
			if (count > points.Length)
				throw new ArgumentException ("count");
#if MAC64
			NSPoint[] _points = new NSPoint[points.Length];
			for( int i=0; i<points.Length; i++ )
				_points[i] = new NSPoint(points[i]);
			CGPathAddLines (handle, ref m, _points, new IntPtr(count));
#else
			CGPathAddLines (handle, ref m, points, new IntPtr(count));
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddLines(IntPtr path, IntPtr zero, NSPoint [] points, IntPtr size_t_count);
		public void AddLines (PointF [] points)
		{
			AddLines (points, points.Length);
		}
		public void AddLines (PointF [] points, int count)
		{
			if (count > points.Length)
				throw new ArgumentException ("count");
#if MAC64
			NSPoint[] _points = new NSPoint[points.Length];
			for( int i=0; i<points.Length; i++ )
				_points[i] = new NSPoint(points[i]);
			CGPathAddLines (handle, IntPtr.Zero, _points, new IntPtr(count));
#else
			CGPathAddLines (handle, IntPtr.Zero, points, new IntPtr(count));
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddEllipseInRect(IntPtr path, ref CGAffineTransform m, NSRect rect);
		public void AddEllipseInRect (CGAffineTransform m, RectangleF rect)
		{
#if MAC64
			CGPathAddEllipseInRect (handle, ref m, new NSRect(rect));
#else
			CGPathAddEllipseInRect (handle, ref m, rect);
#endif
		}
		
		[Obsolete ("Use AddEllipseInRect instead")]
		public void AddElipseInRect (CGAffineTransform m, RectangleF rect)
		{
			AddEllipseInRect (m, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddEllipseInRect(IntPtr path, IntPtr zero, NSRect rect);
		public void AddElipseInRect (RectangleF rect)
		{
#if MAC64
			CGPathAddEllipseInRect (handle, IntPtr.Zero, new NSRect(rect));
#else
			CGPathAddEllipseInRect (handle, IntPtr.Zero, rect);
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddArc(IntPtr path, ref CGAffineTransform m, CGFloat x, CGFloat y, CGFloat radius, CGFloat startAngle, CGFloat endAngle, bool clockwise);
		public void AddArc (CGAffineTransform m, float x, float y, float radius, float startAngle, float endAngle, bool clockwise)
		{
			CGPathAddArc (handle, ref m, x, y, radius, startAngle, endAngle, clockwise);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddArc(IntPtr path, IntPtr zero, CGFloat x, CGFloat y, CGFloat radius, CGFloat startAngle, CGFloat endAngle, bool clockwise);
		public void AddArc (float x, float y, float radius, float startAngle, float endAngle, bool clockwise)
		{
			CGPathAddArc (handle, IntPtr.Zero, x, y, radius, startAngle, endAngle, clockwise);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddArcToPoint(IntPtr path, ref CGAffineTransform m, CGFloat x1, CGFloat y1, CGFloat x2, CGFloat y2, CGFloat radius);
		public void AddArcToPoint (CGAffineTransform m, float x1, float y1, float x2, float y2, float radius)
		{
			CGPathAddArcToPoint (handle, ref m, x1, y1, x2, y2, radius);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddArcToPoint(IntPtr path, IntPtr zero, CGFloat x1, CGFloat y1, CGFloat x2, CGFloat y2, CGFloat radius);
		public void AddArcToPoint (float x1, float y1, float x2, float y2, float radius)
		{
			CGPathAddArcToPoint (handle, IntPtr.Zero, x1, y1, x2, y2, radius);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddRelativeArc(IntPtr path, ref CGAffineTransform m, CGFloat x, CGFloat y, CGFloat radius, CGFloat startAngle, CGFloat delta);
		public void AddRelativeArc (CGAffineTransform m, float x, float y, float radius, float startAngle, float delta)
		{
			CGPathAddRelativeArc (handle, ref m, x, y, radius, startAngle, delta);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddRelativeArc(IntPtr path, IntPtr zero, CGFloat x, CGFloat y, CGFloat radius, CGFloat startAngle, CGFloat delta);
		public void AddRelativeArc (float x, float y, float radius, float startAngle, float delta)
		{
			CGPathAddRelativeArc (handle, IntPtr.Zero, x, y, radius, startAngle, delta);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddPath(IntPtr path1, ref CGAffineTransform m, IntPtr path2);
		public void AddPath (CGAffineTransform t, CGPath path2)
		{
			if (path2 == null)
				throw new ArgumentNullException ("path2");
			CGPathAddPath (handle, ref t, path2.handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathAddPath(IntPtr path1, IntPtr zero, IntPtr path2);
		public void AddPath (CGPath path2)
		{
			if (path2 == null)
				throw new ArgumentNullException ("path2");
			CGPathAddPath (handle, IntPtr.Zero, path2.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGPathIsEmpty(IntPtr path);
		public bool IsEmpty {
			get {
				return CGPathIsEmpty (handle) != 0;
			}
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGPathIsRect(IntPtr path, out NSRect rect);
		public bool IsRect (out RectangleF rect)
		{
#if MAC64
			NSRect _rect;
			bool rc = CGPathIsRect (handle, out _rect) != 0;
			rect = new RectangleF((float)_rect.Origin.X, (float)_rect.Origin.Y, (float)_rect.Width, (float)_rect.Height);
			return rc;
#else
			return CGPathIsRect (handle, out rect) != 0;
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSPoint CGPathGetCurrentPoint(IntPtr path);
		public PointF CurrentPoint {
			get {
#if MAC64
				NSPoint rc = CGPathGetCurrentPoint (handle);
				return new PointF((float)rc.X, (float)rc.Y);
#else
				return CGPathGetCurrentPoint (handle);
#endif
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSRect CGPathGetBoundingBox(IntPtr path);
		public RectangleF BoundingBox {
			get {
#if MAC64
				NSRect rc = CGPathGetBoundingBox (handle);
				return new RectangleF((float)rc.Origin.X, (float)rc.Origin.Y, (float)rc.Width, (float)rc.Height);
#else
				return CGPathGetBoundingBox (handle);
#endif
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSRect CGPathGetPathBoundingBox(IntPtr path);
		public RectangleF PathBoundingBox {
			get {
#if MAC64
				NSRect rc = CGPathGetPathBoundingBox (handle);
				return new RectangleF((float)rc.Origin.X, (float)rc.Origin.Y, (float)rc.Width, (float)rc.Height);
#else
				return CGPathGetPathBoundingBox (handle);
#endif
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPathContainsPoint(IntPtr path, ref CGAffineTransform m, NSPoint point, bool eoFill);
		public bool ContainsPoint (CGAffineTransform m, PointF point, bool eoFill)
		{
#if MAC64
			return CGPathContainsPoint (handle, ref m, new NSPoint(point), eoFill);
#else
			return CGPathContainsPoint (handle, ref m, point, eoFill);
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static bool CGPathContainsPoint(IntPtr path, IntPtr zero, NSPoint point, bool eoFill);
		public bool ContainsPoint (PointF point, bool eoFill)
		{
#if MAC64
			return CGPathContainsPoint (handle, IntPtr.Zero, new NSPoint(point), eoFill);
#else
			return CGPathContainsPoint (handle, IntPtr.Zero, point, eoFill);
#endif
		}

		//typedef void (*CGPathApplierFunction)(void *info, const CGPathElement *element);
		public delegate void ApplierFunction (CGPathElement element);
		delegate void CGPathApplierFunction (IntPtr info, IntPtr CGPathElementPtr);
		
#if !MONOMAC
		[MonoPInvokeCallback (typeof (CGPathApplierFunction))]
#endif
		static void ApplierCallback (IntPtr info, IntPtr element_ptr)
		{
			GCHandle gch = GCHandle.FromIntPtr (info);
			CGPathElement element = new CGPathElement (Marshal.ReadInt32 (element_ptr, 0));
			ApplierFunction func = (ApplierFunction) gch.Target;

			NSPoint pt;
			IntPtr ptr = Marshal.ReadIntPtr (element_ptr, 4);
			int ptsize = Marshal.SizeOf (typeof (NSPoint));

			switch (element.Type){
			case CGPathElementType.CloseSubpath:
				break;
#if MAC64
			case CGPathElementType.MoveToPoint:
			case CGPathElementType.AddLineToPoint:
				pt = (NSPoint) Marshal.PtrToStructure (ptr, typeof (NSPoint));
				element.Point1 = new PointF((float)pt.X, (float)pt.Y);
				break;
				
			case CGPathElementType.AddQuadCurveToPoint:
				pt = (NSPoint) Marshal.PtrToStructure (ptr, typeof (NSPoint));
				element.Point1 = new PointF((float)pt.X, (float)pt.Y);
				pt = (NSPoint) Marshal.PtrToStructure (((IntPtr) (((long)ptr) + ptsize)), typeof (NSPoint));
				element.Point2 = new PointF((float)pt.X, (float)pt.Y);
				break;
				
			case CGPathElementType.AddCurveToPoint:
				pt = (NSPoint) Marshal.PtrToStructure (ptr, typeof (NSPoint));
				element.Point1 = new PointF((float)pt.X, (float)pt.Y);
				pt = (NSPoint) Marshal.PtrToStructure (((IntPtr) (((long)ptr) + ptsize)), typeof (NSPoint));
				element.Point2 = new PointF((float)pt.X, (float)pt.Y);
				pt = (NSPoint) Marshal.PtrToStructure (((IntPtr) (((long)ptr) + (2*ptsize))), typeof (NSPoint));
				element.Point3 = new PointF((float)pt.X, (float)pt.Y);
				break;
#else
			case CGPathElementType.MoveToPoint:
			case CGPathElementType.AddLineToPoint:
				element.Point1 = (PointF) Marshal.PtrToStructure (ptr, typeof (PointF));
				break;

			case CGPathElementType.AddQuadCurveToPoint:
				element.Point1 = (PointF) Marshal.PtrToStructure (ptr, typeof (PointF));
				element.Point2 = (PointF) Marshal.PtrToStructure (((IntPtr) (((long)ptr) + ptsize)), typeof (PointF));
				break;

			case CGPathElementType.AddCurveToPoint:
				element.Point1 = (PointF) Marshal.PtrToStructure (ptr, typeof (PointF));
				element.Point2 = (PointF) Marshal.PtrToStructure (((IntPtr) (((long)ptr) + ptsize)), typeof (PointF));
				element.Point3 = (PointF) Marshal.PtrToStructure (((IntPtr) (((long)ptr) + (2*ptsize))), typeof (PointF));
				break;
#endif
			}
			func (element);
		}


		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPathApply(IntPtr path, IntPtr info, CGPathApplierFunction function);
		public void Apply (ApplierFunction func)
		{
			GCHandle gch = GCHandle.Alloc (func);
			CGPathApply (handle, GCHandle.ToIntPtr (gch), ApplierCallback);
			gch.Free ();
		}

		static CGPath MakeMutable (IntPtr source)
		{
			var mutable = CGPathCreateMutableCopy (source);
			CGPathRelease (source);

			return new CGPath (mutable, true);
		}

#if !MONOMAC
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateCopyByDashingPath (IntPtr handle, ref CGAffineTransform transform, CGFloat [] phase, IntPtr count);

		[Since(5,0)]
		public CGPath CopyByDashingPath (CGAffineTransform transform, float [] phase)
		{
#if MAC64
			CGFloat[] _phase = null;
			if( phase!=null )
			{
				_phase = new CGFloat[phase.Length];
				Array.Copy(phase, _phase, phase.Length);
			}
			return MakeMutable (CGPathCreateCopyByDashingPath (handle, ref transform, _phase, phase == null ? IntPtr.Zero : new IntPtr(phase.Length)));
#else
			return MakeMutable (CGPathCreateCopyByDashingPath (handle, ref transform, phase, phase == null ? IntPtr.Zero : new IntPtr(phase.Length)));
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateCopyByDashingPath (IntPtr handle, IntPtr transform, CGFloat [] phase, IntPtr count);

		[Since(5,0)]
		public CGPath CopyByDashingPath (float [] phase)
		{
#if MAC64
			CGFloat[] _phase = null;
			if( phase!=null )
			{
				_phase = new CGFloat[phase.Length];
				Array.Copy(phase, _phase, phase.Length);
			}
			return MakeMutable (CGPathCreateCopyByDashingPath (handle, IntPtr.Zero, _phase, phase == null ? IntPtr.Zero : new IntPtr(phase.Length)));
#else
			return MakeMutable (CGPathCreateCopyByDashingPath (handle, IntPtr.Zero, phase, phase == null ? IntPtr.Zero : new IntPtr(phase.Length)));
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateCopyByStrokingPath (IntPtr handle, ref CGAffineTransform transform, CGFloat lineWidth, CGLineCap lineCap, CGLineJoin lineJoin, CGFloat miterLimit);

		[Since(5,0)]
		public CGPath CopyByStrokingPath (CGAffineTransform transform, float lineWidth, CGLineCap lineCap, CGLineJoin lineJoin, float miterLimit)
		{
			return MakeMutable (CGPathCreateCopyByStrokingPath (handle, ref transform, lineWidth, lineCap, lineJoin, miterLimit));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateCopyByStrokingPath (IntPtr handle, IntPtr zero, CGFloat lineWidth, CGLineCap lineCap, CGLineJoin lineJoin, CGFloat miterLimit);

		[Since(5,0)]
		public CGPath CopyByStrokingPath (float lineWidth, CGLineCap lineCap, CGLineJoin lineJoin, float miterLimit)
		{
			return MakeMutable (CGPathCreateCopyByStrokingPath (handle, IntPtr.Zero, lineWidth, lineCap, lineJoin, miterLimit));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateMutableCopyByTransformingPath (IntPtr handle, ref CGAffineTransform transform);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateWithEllipse (NSRect boundingRect, ref CGAffineTransform transform);

		[Since (5,0)]
		static public CGPath EllipseFromRect (RectangleF boundingRect, CGAffineTransform transform)
		{
#if MAC64
			return MakeMutable (CGPathCreateWithEllipse (new NSRect(boundingRect), ref transform));
#else
			return MakeMutable (CGPathCreateWithEllipse (boundingRect, ref transform));
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateWithRect (NSRect boundingRect, ref CGAffineTransform transform);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGPathCreateWithRect (NSRect boundingRect, IntPtr transform);
		
		[Since (5,0)]
		static public CGPath FromRect (RectangleF rectangle, CGAffineTransform transform)
		{
#if MAC64
			return MakeMutable (CGPathCreateWithRect (new NSRect(rectangle), ref transform));
#else
			return MakeMutable (CGPathCreateWithRect (rectangle, ref transform));
#endif
		}

		[Since (5,0)]
		static public CGPath FromRect (RectangleF rectangle)
		{
#if MAC64
			return MakeMutable (CGPathCreateWithRect (new NSRect(rectangle), IntPtr.Zero));
#else
			return MakeMutable (CGPathCreateWithRect (rectangle, IntPtr.Zero));
#endif
		}
#endif
	}
}
