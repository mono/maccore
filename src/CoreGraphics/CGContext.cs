// 
// CGContext.cs: Implements the managed CGContext
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

	public enum CGLineJoin {
		Miter,
		Round,
		Bevel
	}
	
	public enum CGLineCap {
		Butt,
		Round,
		Square
	}
	
	public enum CGPathDrawingMode {
		Fill,
		EOFill,
		Stroke,
		FillStroke,
		EOFillStroke
	}
	
	public enum CGTextDrawingMode {
		Fill,
		Stroke,
		FillStroke,
		Invisible,
		FillClip,
		StrokeClip,
		FillStrokeClip,
		Clip
	}
	
	public enum CGTextEncoding {
		FontSpecific,
		MacRoman
	}

	public enum CGInterpolationQuality {
		Default,
		None,	
		Low,	
		High,
		Medium		       /* Yes, in this order, since Medium was added in 4 */
	}
	
	public enum CGBlendMode {
		Normal,
		Multiply,
		Screen,
		Overlay,
		Darken,
		Lighten,
		ColorDodge,
		ColorBurn,
		SoftLight,
		HardLight,
		Difference,
		Exclusion,
		Hue,
		Saturation,
		Color,
		Luminosity,
		
		Clear,
		Copy,	
		SourceIn,	
		SourceOut,	
		SourceAtop,		
		DestinationOver,	
		DestinationIn,	
		DestinationOut,	
		DestinationAtop,	
		XOR,		
		PlusDarker,		
		PlusLighter		
	}

	public class CGContext : INativeObject, IDisposable {
		internal IntPtr handle;

		public CGContext (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				throw new Exception ("Invalid parameters to context creation");

			CGContextRetain (handle);
			this.handle = handle;
		}

		internal CGContext ()
		{
		}
		
		[Preserve (Conditional=true)]
		internal CGContext (IntPtr handle, bool owns)
		{
			if (!owns)
				CGContextRetain (handle);

			if (handle == IntPtr.Zero)
				throw new Exception ("Invalid handle");
			
			this.handle = handle;
		}

		~CGContext ()
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
		extern static void CGContextRelease (IntPtr handle);
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextRetain (IntPtr handle);
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CGContextRelease (handle);
				handle = IntPtr.Zero;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSaveGState (IntPtr context);
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextRestoreGState (IntPtr context);
		
		public void SaveState ()
		{
			CGContextSaveGState (handle);
		}

		public void RestoreState ()
		{
			CGContextRestoreGState (handle);
		}

		//
		// Transformation matrix
		//

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextScaleCTM (IntPtr ctx, CGFloat sx, CGFloat sy);
		public void ScaleCTM (float sx, float sy)
		{
			CGContextScaleCTM (handle, sx, sy);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextTranslateCTM (IntPtr ctx, CGFloat tx, CGFloat ty);
		public void TranslateCTM (float tx, float ty)
		{
			CGContextTranslateCTM (handle, tx, ty);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextRotateCTM (IntPtr ctx, CGFloat angle);
		public void RotateCTM (float angle)
		{
			CGContextRotateCTM (handle, angle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextConcatCTM (IntPtr ctx, CGAffineTransform transform);
		public void ConcatCTM (CGAffineTransform transform)
		{
			CGContextConcatCTM (handle, transform);
		}

		// Settings
		[DllImport (Constants.CoreGraphicsLibrary)]		
		extern static void CGContextSetLineWidth(IntPtr c, CGFloat width);
		public void SetLineWidth (float w)
		{
			CGContextSetLineWidth (handle, w);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetLineCap(IntPtr c, CGLineCap cap);
		public void SetLineCap (CGLineCap cap)
		{
			CGContextSetLineCap (handle, cap);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetLineJoin(IntPtr c, CGLineJoin join);
		public void SetLineJoin (CGLineJoin join)
		{
			CGContextSetLineJoin (handle, join);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetMiterLimit(IntPtr c, CGFloat limit);
		public void SetMiterLimit (float limit)
		{
			CGContextSetMiterLimit (handle, limit);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetLineDash(IntPtr c, CGFloat phase, CGFloat [] lengths, IntPtr count);
		public void SetLineDash (float phase, float [] lengths)
		{
			SetLineDash (phase, lengths, lengths.Length);
		}

		public void SetLineDash (float phase, float [] lengths, int n)
		{
			if (lengths == null)
				throw new ArgumentNullException ("lengths");
			if (n > lengths.Length)
				throw new ArgumentNullException ("n");
			CGFloat[] _lengths = new CGFloat[lengths.Length];
			Array.Copy (lengths, _lengths, lengths.Length);
			CGContextSetLineDash (handle, phase, _lengths, new IntPtr(n));
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetFlatness(IntPtr c, CGFloat flatness);
		public void SetFlatness (float flatness)
		{
			CGContextSetFlatness (handle, flatness);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetAlpha(IntPtr c, CGFloat alpha);
		public void SetAlpha (float alpha)
		{
			CGContextSetAlpha (handle, alpha);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetBlendMode(IntPtr context, CGBlendMode mode);
		public void SetBlendMode (CGBlendMode mode)
		{
			CGContextSetBlendMode (handle, mode);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGAffineTransform CGContextGetCTM (IntPtr c);
		public CGAffineTransform GetCTM ()
		{
			return CGContextGetCTM (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextBeginPath(IntPtr c);
		public void BeginPath ()
		{
			CGContextBeginPath (handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextMoveToPoint(IntPtr c, CGFloat x, CGFloat y);
		public void MoveTo (float x, float y)
		{
			CGContextMoveToPoint (handle, x, y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddLineToPoint(IntPtr c, CGFloat x, CGFloat y);
		public void AddLineToPoint (float x, float y)
		{
			CGContextAddLineToPoint (handle, x, y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddCurveToPoint(IntPtr c, CGFloat cp1x, CGFloat cp1y, CGFloat cp2x, CGFloat cp2y, CGFloat x, CGFloat y);
		public void AddCurveToPoint (float cp1x, float cp1y, float cp2x, float cp2y, float x, float y)
		{
			CGContextAddCurveToPoint (handle, cp1x, cp1y, cp2x, cp2y, x, y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddQuadCurveToPoint(IntPtr c, CGFloat cpx, CGFloat cpy, CGFloat x, CGFloat y);
		public void AddQuadCurveToPoint (float cpx, float cpy, float x, float y)
		{
			CGContextAddQuadCurveToPoint (handle, cpx, cpy, x, y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClosePath(IntPtr c);
		public void ClosePath ()
		{
			CGContextClosePath (handle);
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddRect(IntPtr c, NSRect rect);
		public void AddRect (RectangleF rect)
		{
#if MAC64
			CGContextAddRect (handle, new NSRect(rect));
#else
			CGContextAddRect (handle, rect);
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddRects(IntPtr c, NSRect [] rects, IntPtr size_t_count) ;
		public void AddRects (RectangleF [] rects)
		{
#if MAC64
			NSRect[] _rects = new NSRect[rects.Length];
			for( int i=0; i<rects.Length; i++ )
				_rects[i] = new NSRect(rects[i]);
			CGContextAddRects (handle, _rects, new IntPtr(rects.Length));
#else
			CGContextAddRects (handle, rects, new IntPtr(rects.Length));
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddLines(IntPtr c, NSPoint [] points, IntPtr size_t_count) ;
		public void AddLines (PointF [] points)
		{
#if MAC64
			NSPoint[] _points = new NSPoint[points.Length];
			for( int i=0; i<points.Length; i++ )
				_points[i] = new NSPoint(points[i]);
			CGContextAddLines (handle, _points, new IntPtr(points.Length));
#else
			CGContextAddLines (handle, points, new IntPtr(points.Length));
#endif
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddEllipseInRect(IntPtr context, NSRect rect);
		public void AddEllipseInRect (RectangleF rect)
		{
#if MAC64
			CGContextAddEllipseInRect (handle, new NSRect(rect));
#else
			CGContextAddEllipseInRect (handle, rect);
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddArc(IntPtr c, CGFloat x, CGFloat y, CGFloat radius, CGFloat startAngle, CGFloat endAngle, int clockwise);
		public void AddArc (float x, float y, float radius, float startAngle, float endAngle, bool clockwise)
		{
			CGContextAddArc (handle, x, y, radius, startAngle, endAngle, clockwise ? 1 : 0);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddArcToPoint(IntPtr c, CGFloat x1, CGFloat y1, CGFloat x2, CGFloat y2, CGFloat radius);
		public void AddArcToPoint (float x1, float y1, float x2, float y2, float radius)
		{
			CGContextAddArcToPoint (handle, x1, y1, x2, y2, radius);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddPath(IntPtr context, IntPtr path_ref);
		public void AddPath (CGPath path)
		{
			CGContextAddPath (handle, path.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextReplacePathWithStrokedPath(IntPtr c);
		public void ReplacePathWithStrokedPath ()
		{
			CGContextReplacePathWithStrokedPath (handle);
		}

		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGContextIsPathEmpty(IntPtr c);
		public bool IsPathEmpty ()
		{
			return CGContextIsPathEmpty (handle) != 0;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSPoint CGContextGetPathCurrentPoint(IntPtr c);
		public PointF GetPathCurrentPoint ()
		{
#if MAC64
			NSPoint rc = CGContextGetPathCurrentPoint (handle);
			return new PointF((float)rc.X, (float)rc.Y);
#else
			return CGContextGetPathCurrentPoint (handle);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSRect CGContextGetPathBoundingBox(IntPtr c);
		public RectangleF GetPathBoundingBox ()
		{
#if MAC64
			NSRect rc = CGContextGetPathBoundingBox (handle);
			return new RectangleF((float)rc.Origin.X, (float)rc.Origin.Y, (float)rc.Width, (float)rc.Height);
#else
			return CGContextGetPathBoundingBox (handle);
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGContextPathContainsPoint(IntPtr context, NSPoint point, CGPathDrawingMode mode);
		public bool PathContainsPoint (PointF point, CGPathDrawingMode mode)
		{
#if MAC64
			return CGContextPathContainsPoint (handle, new NSPoint(point), mode) != 0;
#else
			return CGContextPathContainsPoint (handle, point, mode) != 0;
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawPath(IntPtr c, CGPathDrawingMode mode);
		public void DrawPath (CGPathDrawingMode mode)
		{
			CGContextDrawPath (handle, mode);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFillPath(IntPtr c);
		public void FillPath ()
		{
			CGContextFillPath (handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextEOFillPath(IntPtr c);
		public void EOFillPath ()
		{
			CGContextEOFillPath (handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokePath(IntPtr c);
		public void StrokePath ()
		{
			CGContextStrokePath (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFillRect(IntPtr c, NSRect rect);
		public void FillRect (RectangleF rect)
		{
#if MAC64
			CGContextFillRect (handle, new NSRect(rect));
#else
			CGContextFillRect (handle, rect);
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFillRects(IntPtr c, NSRect [] rects, IntPtr size_t_count);
		public void ContextFillRects (RectangleF [] rects)
		{
#if MAC64
			NSRect[] _rects = new NSRect[rects.Length];
			for( int i=0; i<rects.Length; i++ )
				_rects[i] = new NSRect(rects[i]);
			CGContextFillRects (handle, _rects, new IntPtr(rects.Length));
#else
			CGContextFillRects (handle, rects, new IntPtr(rects.Length));
#endif
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeRect(IntPtr c, NSRect rect);
		public void StrokeRect (RectangleF rect)
		{
#if MAC64
			CGContextStrokeRect (handle, new NSRect(rect));
#else
			CGContextStrokeRect (handle, rect);
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeRectWithWidth(IntPtr c, NSRect rect, CGFloat width);
		public void StrokeRectWithWidth (RectangleF rect, float width)
		{
#if MAC64
			CGContextStrokeRectWithWidth (handle, new NSRect(rect), width);
#else
			CGContextStrokeRectWithWidth (handle, rect, width);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClearRect(IntPtr c, NSRect rect);
		public void ClearRect (RectangleF rect)
		{
#if MAC64
			CGContextClearRect (handle, new NSRect(rect));
#else
			CGContextClearRect (handle, rect);
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFillEllipseInRect(IntPtr context, NSRect rect);
		public void FillEllipseInRect (RectangleF rect)
		{
#if MAC64
			CGContextFillEllipseInRect (handle, new NSRect(rect));
#else
			CGContextFillEllipseInRect (handle, rect);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeEllipseInRect(IntPtr context, NSRect rect);
		public void StrokeEllipseInRect (RectangleF rect)
		{
#if MAC64
			CGContextStrokeEllipseInRect (handle, new NSRect(rect));
#else
			CGContextStrokeEllipseInRect (handle, rect);
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeLineSegments(IntPtr c, NSPoint [] points, IntPtr size_t_count);
		public void StrokeLineSegments (PointF [] points)
		{
#if MAC64
			NSPoint[] _points = new NSPoint[points.Length];
			for( int i=0; i<points.Length; i++ )
				_points[i] = new NSPoint(points[i]);
			CGContextStrokeLineSegments (handle, _points, new IntPtr(points.Length));
#else
			CGContextStrokeLineSegments (handle, points, new IntPtr(points.Length));
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClip(IntPtr c);
		public void Clip ()
		{
			CGContextClip (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextEOClip(IntPtr c);
		public void EOClip ()
		{
			CGContextEOClip (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClipToMask(IntPtr c, NSRect rect, IntPtr mask);
		public void ClipToMask (RectangleF rect, CGImage mask)
		{
#if MAC64
			CGContextClipToMask (handle, new NSRect(rect), mask.handle);
#else
			CGContextClipToMask (handle, rect, mask.handle);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSRect CGContextGetClipBoundingBox(IntPtr c);
		public RectangleF GetClipBoundingBox ()
		{
#if MAC64
			NSRect rc = CGContextGetClipBoundingBox (handle);
			return new RectangleF((float)rc.Origin.X, (float)rc.Origin.Y, (float)rc.Width, (float)rc.Height);
#else
			return CGContextGetClipBoundingBox (handle);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClipToRect(IntPtr c, NSRect rect);
		public void ClipToRect (RectangleF rect)
		{
#if MAC64
			CGContextClipToRect (handle, new NSRect(rect));
#else
			CGContextClipToRect (handle, rect);
#endif
		}
		       
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClipToRects(IntPtr c, NSRect [] rects, IntPtr size_t_count);
		public void ClipToRects (RectangleF [] rects)
		{
#if MAC64
			NSRect[] _rects = new NSRect[rects.Length];
			for( int i=0; i<rects.Length; i++ )
				_rects[i] = new NSRect(rects[i]);
			CGContextClipToRects (handle, _rects, new IntPtr(rects.Length));
#else
			CGContextClipToRects (handle, rects, new IntPtr(rects.Length));
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFillColorWithColor(IntPtr c, IntPtr color);
		public void SetFillColor (CGColor color)
		{
			CGContextSetFillColorWithColor (handle, color.handle);
		}
		
		[Advice ("Use SetFillColor() instead.")]
		public void SetFillColorWithColor (CGColor color)
		{
			CGContextSetFillColorWithColor (handle, color.handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetStrokeColorWithColor(IntPtr c, IntPtr color);
		public void SetStrokeColor (CGColor color)
		{
			CGContextSetStrokeColorWithColor (handle, color.handle);
		}
		
		[Advice ("Use SetStrokeColor() instead.")]
		public void SetStrokeColorWithColor (CGColor color)
		{
			CGContextSetStrokeColorWithColor (handle, color.handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFillColorSpace(IntPtr context, IntPtr space);
		public void SetFillColorSpace (CGColorSpace space)
		{
			CGContextSetFillColorSpace (handle, space.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetStrokeColorSpace(IntPtr context, IntPtr space);
		public void SetStrokeColorSpace (CGColorSpace space)
		{
			CGContextSetStrokeColorSpace (handle, space.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFillColor(IntPtr context, CGFloat [] components);
		public void SetFillColor (float [] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");
#if MAC64
			CGFloat[] _components = new CGFloat[components.Length];
			Array.Copy(components, _components, components.Length);
			CGContextSetFillColor (handle, _components);
#else
			CGContextSetFillColor (handle, components);
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetStrokeColor(IntPtr context, CGFloat [] components);
		public void SetStrokeColor (float [] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");
#if MAC64
			CGFloat[] _components = new CGFloat[components.Length];
			Array.Copy(components, _components, components.Length);
			CGContextSetStrokeColor (handle, _components);
#else
			CGContextSetStrokeColor (handle, components);
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFillPattern(IntPtr context, IntPtr pattern, CGFloat [] components);
		public void SetFillPattern (CGPattern pattern, float [] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");
#if MAC64
			CGFloat[] _components = new CGFloat[components.Length];
			Array.Copy(components, _components, components.Length);
			CGContextSetFillPattern (handle, pattern.handle, _components);
#else
			CGContextSetFillPattern (handle, pattern.handle, components);
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetStrokePattern(IntPtr context, IntPtr pattern, CGFloat [] components);
		public void SetStrokePattern (CGPattern pattern, float [] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");
#if MAC64
			CGFloat[] _components = new CGFloat[components.Length];
			Array.Copy(components, _components, components.Length);
			CGContextSetStrokePattern (handle, pattern.handle, _components);
#else
			CGContextSetStrokePattern (handle, pattern.handle, components);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetPatternPhase(IntPtr context, NSSize phase);
		public void SetPatternPhase (SizeF phase)
		{
#if MAC64
			CGContextSetPatternPhase (handle, new NSSize(phase));
#else
			CGContextSetPatternPhase (handle, phase);
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetGrayFillColor(IntPtr context, CGFloat gray, CGFloat alpha);
		public void SetFillColor (float gray, float alpha)
		{
			CGContextSetGrayFillColor (handle, gray, alpha);
		}
		
		[Advice ("Use SetFillColor() instead.")]
		public void SetGrayFillColor (float gray, float alpha)
		{
			CGContextSetGrayFillColor (handle, gray, alpha);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetGrayStrokeColor(IntPtr context, CGFloat gray, CGFloat alpha);
		public void SetStrokeColor (float gray, float alpha)
		{
			CGContextSetGrayStrokeColor (handle, gray, alpha);
		}
		
		[Advice ("Use SetStrokeColor() instead.")]
		public void SetGrayStrokeColor (float gray, float alpha)
		{
			CGContextSetGrayStrokeColor (handle, gray, alpha);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetRGBFillColor(IntPtr context, CGFloat red, CGFloat green, CGFloat blue, CGFloat alpha);
		public void SetFillColor (float red, float green, float blue, float alpha)
		{
			CGContextSetRGBFillColor (handle, red, green, blue, alpha);
		}
		
		[Advice ("Use SetFillColor() instead.")]
		public void SetRGBFillColor (float red, float green, float blue, float alpha)
		{
			CGContextSetRGBFillColor (handle, red, green, blue, alpha);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetRGBStrokeColor(IntPtr context, CGFloat red, CGFloat green, CGFloat blue, CGFloat alpha);
		public void SetStrokeColor (float red, float green, float blue, float alpha)
		{
			CGContextSetRGBStrokeColor (handle, red, green, blue, alpha);
		}
		
		[Advice ("Use SetStrokeColor() instead.")]
		public void SetRGBStrokeColor (float red, float green, float blue, float alpha)
		{
			CGContextSetRGBStrokeColor (handle, red, green, blue, alpha);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetCMYKFillColor(IntPtr context, CGFloat cyan, CGFloat magenta, CGFloat yellow, CGFloat black, CGFloat alpha);
		public void SetFillColor (float cyan, float magenta, float yellow, float black, float alpha)
		{
			CGContextSetCMYKFillColor (handle, cyan, magenta, yellow, black, alpha);
		}
		
		[Advice ("Use SetFillColor() instead.")]
		public void SetCMYKFillColor (float cyan, float magenta, float yellow, float black, float alpha)
		{
			CGContextSetCMYKFillColor (handle, cyan, magenta, yellow, black, alpha);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetCMYKStrokeColor(IntPtr context, CGFloat cyan, CGFloat magenta, CGFloat yellow, CGFloat black, CGFloat alpha);
		public void SetStrokeColor (float cyan, float magenta, float yellow, float black, float alpha)
		{
			CGContextSetCMYKStrokeColor (handle, cyan, magenta, yellow, black, alpha);
		}
		
		[Advice ("Use SetStrokeColor() instead.")]
		public void SetCMYKStrokeColor (float cyan, float magenta, float yellow, float black, float alpha)
		{
			CGContextSetCMYKStrokeColor (handle, cyan, magenta, yellow, black, alpha);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetRenderingIntent(IntPtr context, CGColorRenderingIntent intent);
		public void SetRenderingIntent (CGColorRenderingIntent intent)
		{
			CGContextSetRenderingIntent (handle, intent);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawImage(IntPtr c, NSRect rect, IntPtr image);
		public void DrawImage (RectangleF rect, CGImage image)
		{
#if MAC64
			CGContextDrawImage (handle, new NSRect(rect), image.handle);
#else
			CGContextDrawImage (handle, rect, image.handle);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawTiledImage(IntPtr c, NSRect rect, IntPtr image);
		public void DrawTiledImage (RectangleF rect, CGImage image)
		{
#if MAC64
			CGContextDrawTiledImage (handle, new NSRect(rect), image.handle);
#else
			CGContextDrawTiledImage (handle, rect, image.handle);
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGInterpolationQuality CGContextGetInterpolationQuality(IntPtr context);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetInterpolationQuality(IntPtr context, CGInterpolationQuality quality);
		
		public CGInterpolationQuality  InterpolationQuality {
			get {
				return CGContextGetInterpolationQuality (handle);
			}

			set {
				CGContextSetInterpolationQuality (handle, value);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetShadowWithColor(IntPtr context, NSSize offset, CGFloat blur, IntPtr color);
		public void SetShadowWithColor (SizeF offset, float blur, CGColor color)
		{
#if MAC64
			CGContextSetShadowWithColor (handle, new NSSize(offset), blur, color.handle);
#else
			CGContextSetShadowWithColor (handle, offset, blur, color.handle);
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetShadow(IntPtr context, NSSize offset, CGFloat blur);
		public void SetShadow (SizeF offset, float blur)
		{
#if MAC64
			CGContextSetShadow (handle, new NSSize(offset), blur);
#else
			CGContextSetShadow (handle, offset, blur);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawLinearGradient(IntPtr context, IntPtr gradient, NSPoint startPoint, NSPoint endPoint, CGGradientDrawingOptions options);
		public void DrawLinearGradient (CGGradient gradient, PointF startPoint, PointF endPoint, CGGradientDrawingOptions options)
		{
#if MAC64
			CGContextDrawLinearGradient (handle, gradient.handle, new NSPoint(startPoint), new NSPoint(endPoint), options);
#else
			CGContextDrawLinearGradient (handle, gradient.handle, startPoint, endPoint, options);
#endif
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawRadialGradient (IntPtr context, IntPtr gradient, NSPoint startCenter, CGFloat startRadius,
								NSPoint endCenter, CGFloat endRadius, CGGradientDrawingOptions options);
		public void DrawRadialGradient (CGGradient gradient, PointF startCenter, float startRadius, PointF endCenter, float endRadius, CGGradientDrawingOptions options)
		{
#if MAC64
			CGContextDrawRadialGradient (handle, gradient.handle, new NSPoint(startCenter), startRadius, new NSPoint(endCenter), endRadius, options);
#else
			CGContextDrawRadialGradient (handle, gradient.handle, startCenter, startRadius, endCenter, endRadius, options);
#endif
		}
		
#if !COREBUILD
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawShading(IntPtr context, IntPtr shading);
		public void DrawShading (CGShading shading)
		{
			CGContextDrawShading (handle, shading.handle);
		}
#endif		

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetCharacterSpacing(IntPtr context, CGFloat spacing);
		public void SetCharacterSpacing (float spacing)
		{
			CGContextSetCharacterSpacing (handle, spacing);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetTextPosition(IntPtr c, CGFloat x, CGFloat y);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSPoint CGContextGetTextPosition(IntPtr context);

		public PointF TextPosition {
			get {
#if MAC64
				NSPoint rc = CGContextGetTextPosition (handle);
				return new PointF((float)rc.X, (float)rc.Y);
#else
				return CGContextGetTextPosition (handle);
#endif
			}

			set {
				CGContextSetTextPosition (handle, value.X, value.Y);
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetTextMatrix(IntPtr c, CGAffineTransform t);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGAffineTransform CGContextGetTextMatrix(IntPtr c);
		public CGAffineTransform TextMatrix {
			get {
				return CGContextGetTextMatrix (handle);
			}
			set {
				CGContextSetTextMatrix (handle, value);
			}
			
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetTextDrawingMode(IntPtr c, CGTextDrawingMode mode);
		public void SetTextDrawingMode (CGTextDrawingMode mode)
		{
			CGContextSetTextDrawingMode (handle, mode);
		}
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFont(IntPtr c, IntPtr font);
		public void SetFont (CGFont font)
		{
			CGContextSetFont (handle, font.handle);
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFontSize(IntPtr c, CGFloat size);
		public void SetFontSize (float size)
		{
			CGContextSetFontSize (handle, size);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSelectFont(IntPtr c, string name, CGFloat size, CGTextEncoding textEncoding);
		public void SelectFont (string name, float size, CGTextEncoding textEncoding)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			CGContextSelectFont (handle, name, size, textEncoding);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphsAtPositions(IntPtr context, ushort [] glyphs, NSPoint [] positions, IntPtr size_t_count);
		public void ShowGlyphsAtPositions (ushort [] glyphs, PointF [] positions, int size_t_count)
		{
			if (positions == null)
				throw new ArgumentNullException ("positions");
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
#if MAC64
			NSPoint[] _positions = new NSPoint[positions.Length];
			for( int i=0; i<positions.Length; i++ )
				_positions[i] = new NSPoint(positions[i]);
			CGContextShowGlyphsAtPositions (handle, glyphs, _positions, new IntPtr(size_t_count));
#else
			CGContextShowGlyphsAtPositions (handle, glyphs, positions, new IntPtr(size_t_count));
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowText(IntPtr c, string s, IntPtr size_t_length);
		public void ShowText (string str, int count)
		{
			if (str == null)
				throw new ArgumentNullException ("str");
			if (count > str.Length)
				throw new ArgumentException ("count");
			CGContextShowText (handle, str, new IntPtr(count));
		}

		public void ShowText (string str)
		{
			if (str == null)
				throw new ArgumentNullException ("str");
			CGContextShowText (handle, str, new IntPtr(str.Length));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowText(IntPtr c, byte[] bytes, IntPtr size_t_length);
		public void ShowText (byte[] bytes, int count)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");
			if (count > bytes.Length)
				throw new ArgumentException ("count");
			CGContextShowText (handle, bytes, new IntPtr(count));
		}
		
		public void ShowText (byte[] bytes)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");
			CGContextShowText (handle, bytes, new IntPtr(bytes.Length));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowTextAtPoint(IntPtr c, CGFloat x, CGFloat y, string str, IntPtr size_t_length);
		public void ShowTextAtPoint (float x, float y, string str, int length)
		{
			if (str == null)
				throw new ArgumentNullException ("str");
			CGContextShowTextAtPoint (handle, x, y, str, new IntPtr(length));
		}

		public void ShowTextAtPoint (float x, float y, string str)
		{
			if (str == null)
				throw new ArgumentNullException ("str");
			CGContextShowTextAtPoint (handle, x, y, str, new IntPtr(str.Length));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowTextAtPoint(IntPtr c, CGFloat x, CGFloat y, byte[] bytes, IntPtr size_t_length);
		public void ShowTextAtPoint (float x, float y, byte[] bytes, int length)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");
			CGContextShowTextAtPoint (handle, x, y, bytes, new IntPtr(length));
		}
		
		public void ShowTextAtPoint (float x, float y, byte[] bytes)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");
			CGContextShowTextAtPoint (handle, x, y, bytes, new IntPtr(bytes.Length));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphs(IntPtr c, ushort [] glyphs, IntPtr size_t_count);
		public void ShowGlyphs (ushort [] glyphs)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			CGContextShowGlyphs (handle, glyphs, new IntPtr(glyphs.Length));
		}

		public void ShowGlyphs (ushort [] glyphs, int count)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			if (count > glyphs.Length)
				throw new ArgumentException ("count");
			CGContextShowGlyphs (handle, glyphs, new IntPtr(count));
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphsAtPoint(IntPtr context, CGFloat x, CGFloat y, ushort [] glyphs, IntPtr size_t_count);
		public void ShowGlyphsAtPoint (float x, float y, ushort [] glyphs, int count)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			if (count > glyphs.Length)
				throw new ArgumentException ("count");
			CGContextShowGlyphsAtPoint (handle, x, y, glyphs, new IntPtr(count));
		}

		public void ShowGlyphsAtPoint (float x, float y, ushort [] glyphs)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			
			CGContextShowGlyphsAtPoint (handle, x, y, glyphs, new IntPtr(glyphs.Length));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphsWithAdvances(IntPtr c, ushort [] glyphs, NSSize [] advances, IntPtr size_t_count);
		public void ShowGlyphsWithAdvances (ushort [] glyphs, SizeF [] advances, int count)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			if (advances == null)
				throw new ArgumentNullException ("advances");
			if (count > glyphs.Length || count > advances.Length)
				throw new ArgumentException ("count");
#if MAC64
			NSSize[] _advances = new NSSize[advances.Length];
			for( int i=0; i<advances.Length; i++ )
				_advances[i] = new NSSize(advances[i]);
			CGContextShowGlyphsWithAdvances (handle, glyphs, _advances, new IntPtr(count));
#else
			CGContextShowGlyphsWithAdvances (handle, glyphs, advances, new IntPtr(count));
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawPDFPage(IntPtr c, IntPtr page);
		public void DrawPDFPage (CGPDFPage page)
		{
			CGContextDrawPDFPage (handle, page.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGContextBeginPage(IntPtr c, ref NSRect mediaBox);
		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGContextBeginPage(IntPtr c, IntPtr zero);
		public void BeginPage (RectangleF? rect)
		{
			if (rect.HasValue){
				RectangleF v = rect.Value;
#if MAC64
				NSRect _v = new NSRect(v);
				CGContextBeginPage (handle, ref _v);
#else
				CGContextBeginPage (handle, ref v);
#endif
			} else {
				CGContextBeginPage (handle, IntPtr.Zero);
			}
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextEndPage(IntPtr c);
		public void EndPage ()
		{
			CGContextEndPage (handle);
		}
		
		//[DllImport (Constants.CoreGraphicsLibrary)]
		//extern static IntPtr CGContextRetain(IntPtr c);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFlush(IntPtr c);
		public void Flush ()
		{
			CGContextFlush (handle);
		}
		

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSynchronize(IntPtr c);
		public void Synchronize ()
		{
			CGContextSynchronize (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetShouldAntialias(IntPtr c, int shouldAntialias);
		public void SetShouldAntialias (bool shouldAntialias)
		{
			CGContextSetShouldAntialias (handle, shouldAntialias ? 1 : 0);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetAllowsAntialiasing(IntPtr context, int allowsAntialiasing);
		public void SetAllowsAntialiasing (bool allowsAntialiasing)
		{
			CGContextSetAllowsAntialiasing (handle, allowsAntialiasing ? 1 : 0);
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetShouldSmoothFonts(IntPtr c, int shouldSmoothFonts);
		public void SetShouldSmoothFonts (bool shouldSmoothFonts)
		{
			CGContextSetShouldSmoothFonts (handle, shouldSmoothFonts ? 1 : 0);
		}
		
		//[DllImport (Constants.CoreGraphicsLibrary)]
		//extern static void CGContextBeginTransparencyLayer(IntPtr context, CFDictionaryRef auxiliaryInfo);
		//[DllImport (Constants.CoreGraphicsLibrary)]
		//extern static void CGContextBeginTransparencyLayerWithRect(IntPtr context, RectangleF rect, CFDictionaryRef auxiliaryInfo)
		//[DllImport (Constants.CoreGraphicsLibrary)]
		//extern static void CGContextEndTransparencyLayer(IntPtr context);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGAffineTransform CGContextGetUserSpaceToDeviceSpaceTransform(IntPtr context);
		public CGAffineTransform GetUserSpaceToDeviceSpaceTransform()
		{
			return CGContextGetUserSpaceToDeviceSpaceTransform (handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSPoint CGContextConvertPointToDeviceSpace(IntPtr context, NSPoint point);
		public PointF PointToDeviceSpace (PointF point)
		{
#if MAC64
			NSPoint rc = CGContextConvertPointToDeviceSpace (handle, new NSPoint(point));
			return new PointF((float)rc.X, (float)rc.Y);
#else
			return CGContextConvertPointToDeviceSpace (handle, point);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSPoint CGContextConvertPointToUserSpace(IntPtr context, NSPoint point);
		public PointF ConvertPointToUserSpace (PointF point)
		{
#if MAC64
			NSPoint rc = CGContextConvertPointToUserSpace (handle, new NSPoint(point));
			return new PointF((float)rc.X, (float)rc.Y);
#else
			return CGContextConvertPointToUserSpace (handle, point);
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSSize CGContextConvertSizeToDeviceSpace(IntPtr context, NSSize size);
		public SizeF ConvertSizeToDeviceSpace (SizeF size)
		{
#if MAC64
			NSSize rc = CGContextConvertSizeToDeviceSpace (handle, new NSSize(size));
			return new SizeF((float)rc.Width, (float)rc.Height);
#else
			return CGContextConvertSizeToDeviceSpace (handle, size);
#endif
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSSize CGContextConvertSizeToUserSpace(IntPtr context, NSSize size);
		public SizeF ConvertSizeToUserSpace (SizeF size)
		{
#if MAC64
			NSSize rc = CGContextConvertSizeToUserSpace (handle, new NSSize(size));
			return new SizeF((float)rc.Width, (float)rc.Height);
#else
			return CGContextConvertSizeToUserSpace (handle, size);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSRect CGContextConvertRectToDeviceSpace(IntPtr context, NSRect rect);
		public RectangleF ConvertRectToDeviceSpace (RectangleF rect)
		{
#if MAC64
			NSRect rc = CGContextConvertRectToDeviceSpace (handle, new NSRect(rect));
			return new RectangleF((float)rc.Origin.X, (float)rc.Origin.Y, (float)rc.Width, (float)rc.Height);
#else
			return CGContextConvertRectToDeviceSpace (handle, rect);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static NSRect CGContextConvertRectToUserSpace(IntPtr context, NSRect rect);
		public RectangleF ConvertRectToUserSpace (RectangleF rect)
		{
#if MAC64
			NSRect rc = CGContextConvertRectToUserSpace (handle, new NSRect(rect));
			return new RectangleF((float)rc.Origin.X, (float)rc.Origin.Y, (float)rc.Width, (float)rc.Height);
#else
			return CGContextConvertRectToUserSpace (handle, rect);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawLayerInRect (IntPtr context, NSRect rect, IntPtr layer);
		public void DrawLayer (CGLayer layer, RectangleF rect)
		{
			if (layer == null)
				throw new ArgumentNullException ("layer");
#if MAC64
			CGContextDrawLayerInRect (handle, new NSRect(rect), layer.Handle);
#else
			CGContextDrawLayerInRect (handle, rect, layer.Handle);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawLayerAtPoint (IntPtr context, NSPoint rect, IntPtr layer);

		public void DrawLayer (CGLayer layer, PointF point)
		{
			if (layer == null)
				throw new ArgumentNullException ("layer");
#if MAC64
			CGContextDrawLayerAtPoint (handle, new NSPoint(point), layer.Handle);
#else
			CGContextDrawLayerAtPoint (handle, point, layer.Handle);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextCopyPath (IntPtr context);

		[Since (4,0)]
		public CGPath CopyPath ()
		{
			var r = CGContextCopyPath (handle);

			return new CGPath (r, true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextSetAllowsFontSmoothing (IntPtr context, bool allows);
		[Since (4,0)]
		public void SetAllowsFontSmoothing (bool allows)
		{
			CGContextSetAllowsFontSmoothing (handle, allows);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextSetAllowsFontSubpixelPositioning (IntPtr context, bool allows);
		[Since (4,0)]
		public void SetAllowsSubpixelPositioning (bool allows)
		{
			CGContextSetAllowsFontSubpixelPositioning (handle, allows);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextSetAllowsFontSubpixelQuantization (IntPtr context, bool allows);
		[Since (4,0)]
		public void SetAllowsFontSubpixelQuantization (bool allows)
		{
			CGContextSetAllowsFontSubpixelQuantization (handle, allows);
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextSetShouldSubpixelPositionFonts (IntPtr context, bool should);
		[Since (4,0)]
		public void SetShouldSubpixelPositionFonts (bool shouldSubpixelPositionFonts)
		{
			CGContextSetShouldSubpixelPositionFonts (handle, shouldSubpixelPositionFonts);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextSetShouldSubpixelQuantizeFonts (IntPtr context, bool should);
		[Since (4,0)]
		public void ShouldSubpixelQuantizeFonts (bool shouldSubpixelQuantizeFonts)
		{
			CGContextSetShouldSubpixelQuantizeFonts (handle, shouldSubpixelQuantizeFonts);
		}

#if !COREBUILD
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextBeginTransparencyLayer (IntPtr context, IntPtr dictionary);
		public void BeginTransparencyLayer ()
		{
			CGContextBeginTransparencyLayer (handle, IntPtr.Zero);
		}
		
		public void BeginTransparencyLayer (NSDictionary auxiliaryInfo = null)
		{
			CGContextBeginTransparencyLayer (handle, auxiliaryInfo == null ? IntPtr.Zero : auxiliaryInfo.Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextBeginTransparencyLayerWithRect (IntPtr context, NSRect rect, IntPtr dictionary);
		public void BeginTransparencyLayer (RectangleF rectangle, NSDictionary auxiliaryInfo = null)
		{
#if MAC64
			CGContextBeginTransparencyLayerWithRect (handle, new NSRect(rectangle), auxiliaryInfo == null ? IntPtr.Zero : auxiliaryInfo.Handle);
#else
			CGContextBeginTransparencyLayerWithRect (handle, rectangle, auxiliaryInfo == null ? IntPtr.Zero : auxiliaryInfo.Handle);
#endif
		}

		public void BeginTransparencyLayer (RectangleF rectangle)
		{
#if MAC64
			CGContextBeginTransparencyLayerWithRect (handle, new NSRect(rectangle), IntPtr.Zero);
#else
			CGContextBeginTransparencyLayerWithRect (handle, rectangle, IntPtr.Zero);
#endif
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static IntPtr CGContextEndTransparencyLayer (IntPtr context);
		public void EndTransparencyLayer ()
		{
			CGContextEndTransparencyLayer (handle);
		}
#endif
	}
}
