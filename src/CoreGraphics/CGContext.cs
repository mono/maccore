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
		extern static void CGContextScaleCTM (IntPtr ctx, float sx, float sy);
		public void ScaleCTM (float sx, float sy)
		{
			CGContextScaleCTM (handle, sx, sy);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextTranslateCTM (IntPtr ctx, float tx, float ty);
		public void TranslateCTM (float tx, float ty)
		{
			CGContextTranslateCTM (handle, tx, ty);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextRotateCTM (IntPtr ctx, float angle);
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
		extern static void CGContextSetLineWidth(IntPtr c, float width);
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
		extern static	void CGContextSetMiterLimit(IntPtr c, float limit);
		public void SetMiterLimit (float limit)
		{
			CGContextSetMiterLimit (handle, limit);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetLineDash(IntPtr c, float phase, float [] lengths, int count);
		public void SetLineDash (float phase, float [] lengths)
		{
			int n = lengths == null ? 0 : lengths.Length;
			CGContextSetLineDash (handle, phase, lengths, n);
		}

		public void SetLineDash (float phase, float [] lengths, int n)
		{
			if (lengths == null)
				n = 0;
			else if (n < 0 || n > lengths.Length)
				throw new ArgumentException ("n");
			CGContextSetLineDash (handle, phase, lengths, n);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetFlatness(IntPtr c, float flatness);
		public void SetFlatness (float flatness)
		{
			CGContextSetFlatness (handle, flatness);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static	void CGContextSetAlpha(IntPtr c, float alpha);
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
		extern static void CGContextMoveToPoint(IntPtr c, float x, float y);
		public void MoveTo (float x, float y)
		{
			CGContextMoveToPoint (handle, x, y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddLineToPoint(IntPtr c, float x, float y);
		public void AddLineToPoint (float x, float y)
		{
			CGContextAddLineToPoint (handle, x, y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddCurveToPoint(IntPtr c, float cp1x, float cp1y, float cp2x, float cp2y, float x, float y);
		public void AddCurveToPoint (float cp1x, float cp1y, float cp2x, float cp2y, float x, float y)
		{
			CGContextAddCurveToPoint (handle, cp1x, cp1y, cp2x, cp2y, x, y);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddQuadCurveToPoint(IntPtr c, float cpx, float cpy, float x, float y);
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
		extern static void CGContextAddRect(IntPtr c, RectangleF rect);
		public void AddRect (RectangleF rect)
		{
			CGContextAddRect (handle, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddRects(IntPtr c, RectangleF [] rects, int size_t_count) ;
		public void AddRects (RectangleF [] rects)
		{
			CGContextAddRects (handle, rects, rects.Length);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddLines(IntPtr c, PointF [] points, int size_t_count) ;
		public void AddLines (PointF [] points)
		{
			CGContextAddLines (handle, points, points.Length);
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddEllipseInRect(IntPtr context, RectangleF rect);
		public void AddEllipseInRect (RectangleF rect)
		{
			CGContextAddEllipseInRect (handle, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddArc(IntPtr c, float x, float y, float radius, float startAngle, float endAngle, int clockwise);
		public void AddArc (float x, float y, float radius, float startAngle, float endAngle, bool clockwise)
		{
			CGContextAddArc (handle, x, y, radius, startAngle, endAngle, clockwise ? 1 : 0);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextAddArcToPoint(IntPtr c, float x1, float y1, float x2, float y2, float radius);
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
		extern static PointF CGContextGetPathCurrentPoint(IntPtr c);
		public PointF GetPathCurrentPoint ()
		{
			return CGContextGetPathCurrentPoint (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static RectangleF CGContextGetPathBoundingBox(IntPtr c);
		public RectangleF GetPathBoundingBox ()
		{
			return CGContextGetPathBoundingBox (handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static int CGContextPathContainsPoint(IntPtr context, PointF point, CGPathDrawingMode mode);
		public bool PathContainsPoint (PointF point, CGPathDrawingMode mode)
		{
			return CGContextPathContainsPoint (handle, point, mode) != 0;
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
		extern static void CGContextFillRect(IntPtr c, RectangleF rect);
		public void FillRect (RectangleF rect)
		{
			CGContextFillRect (handle, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFillRects(IntPtr c, RectangleF [] rects, int size_t_count);
		public void ContextFillRects (RectangleF [] rects)
		{
			CGContextFillRects (handle, rects, rects.Length);
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeRect(IntPtr c, RectangleF rect);
		public void StrokeRect (RectangleF rect)
		{
			CGContextStrokeRect (handle, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeRectWithWidth(IntPtr c, RectangleF rect, float width);
		public void StrokeRectWithWidth (RectangleF rect, float width)
		{
			CGContextStrokeRectWithWidth (handle, rect, width);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClearRect(IntPtr c, RectangleF rect);
		public void ClearRect (RectangleF rect)
		{
			CGContextClearRect (handle, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextFillEllipseInRect(IntPtr context, RectangleF rect);
		public void FillEllipseInRect (RectangleF rect)
		{
			CGContextFillEllipseInRect (handle, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeEllipseInRect(IntPtr context, RectangleF rect);
		public void StrokeEllipseInRect (RectangleF rect)
		{
			CGContextStrokeEllipseInRect (handle, rect);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextStrokeLineSegments(IntPtr c, PointF [] points, int size_t_count);
		public void StrokeLineSegments (PointF [] points)
		{
			CGContextStrokeLineSegments (handle, points, points.Length);
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
		extern static void CGContextClipToMask(IntPtr c, RectangleF rect, IntPtr mask);
		public void ClipToMask (RectangleF rect, CGImage mask)
		{
			CGContextClipToMask (handle, rect, mask.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static RectangleF CGContextGetClipBoundingBox(IntPtr c);
		public RectangleF GetClipBoundingBox ()
		{
			return CGContextGetClipBoundingBox (handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClipToRect(IntPtr c, RectangleF rect);
		public void ClipToRect (RectangleF rect)
		{
			CGContextClipToRect (handle, rect);
		}
		       
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextClipToRects(IntPtr c, RectangleF [] rects, int size_t_count);
		public void ClipToRects (RectangleF [] rects)
		{
			CGContextClipToRects (handle, rects, rects.Length);
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
		extern static void CGContextSetFillColor(IntPtr context, float [] components);
		public void SetFillColor (float [] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");
			CGContextSetFillColor (handle, components);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetStrokeColor(IntPtr context, float [] components);
		public void SetStrokeColor (float [] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");
			CGContextSetStrokeColor (handle, components);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetFillPattern(IntPtr context, IntPtr pattern, float [] components);
		public void SetFillPattern (CGPattern pattern, float [] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");
			CGContextSetFillPattern (handle, pattern.handle, components);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetStrokePattern(IntPtr context, IntPtr pattern, float [] components);
		public void SetStrokePattern (CGPattern pattern, float [] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");
			CGContextSetStrokePattern (handle, pattern.handle, components);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetPatternPhase(IntPtr context, SizeF phase);
		public void SetPatternPhase (SizeF phase)
		{
			CGContextSetPatternPhase (handle, phase);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetGrayFillColor(IntPtr context, float gray, float alpha);
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
		extern static void CGContextSetGrayStrokeColor(IntPtr context, float gray, float alpha);
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
		extern static void CGContextSetRGBFillColor(IntPtr context, float red, float green, float blue, float alpha);
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
		extern static void CGContextSetRGBStrokeColor(IntPtr context, float red, float green, float blue, float alpha);
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
		extern static void CGContextSetCMYKFillColor(IntPtr context, float cyan, float magenta, float yellow, float black, float alpha);
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
		extern static void CGContextSetCMYKStrokeColor(IntPtr context, float cyan, float magenta, float yellow, float black, float alpha);
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
		extern static void CGContextDrawImage(IntPtr c, RectangleF rect, IntPtr image);
		public void DrawImage (RectangleF rect, CGImage image)
		{
			CGContextDrawImage (handle, rect, image.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawTiledImage(IntPtr c, RectangleF rect, IntPtr image);
		public void DrawTiledImage (RectangleF rect, CGImage image)
		{
			CGContextDrawTiledImage (handle, rect, image.handle);
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
		extern static void CGContextSetShadowWithColor(IntPtr context, SizeF offset, float blur, IntPtr color);
		public void SetShadowWithColor (SizeF offset, float blur, CGColor color)
		{
			CGContextSetShadowWithColor (handle, offset, blur, color.handle);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetShadow(IntPtr context, SizeF offset, float blur);
		public void SetShadow (SizeF offset, float blur)
		{
			CGContextSetShadow (handle, offset, blur);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawLinearGradient(IntPtr context, IntPtr gradient, PointF startPoint, PointF endPoint, CGGradientDrawingOptions options);
		public void DrawLinearGradient (CGGradient gradient, PointF startPoint, PointF endPoint, CGGradientDrawingOptions options)
		{
			CGContextDrawLinearGradient (handle, gradient.handle, startPoint, endPoint, options);
		}
			
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawRadialGradient (IntPtr context, IntPtr gradient, PointF startCenter, float startRadius,
								PointF endCenter, float endRadius, CGGradientDrawingOptions options);
		public void DrawRadialGradient (CGGradient gradient, PointF startCenter, float startRadius, PointF endCenter, float endRadius, CGGradientDrawingOptions options)
		{
			CGContextDrawRadialGradient (handle, gradient.handle, startCenter, startRadius, endCenter, endRadius, options);
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
		extern static void CGContextSetCharacterSpacing(IntPtr context, float spacing);
		public void SetCharacterSpacing (float spacing)
		{
			CGContextSetCharacterSpacing (handle, spacing);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSetTextPosition(IntPtr c, float x, float y);
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static PointF CGContextGetTextPosition(IntPtr context);

		public PointF TextPosition {
			get {
				return CGContextGetTextPosition (handle);
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
		extern static void CGContextSetFontSize(IntPtr c, float size);
		public void SetFontSize (float size)
		{
			CGContextSetFontSize (handle, size);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextSelectFont(IntPtr c, string name, float size, CGTextEncoding textEncoding);
		public void SelectFont (string name, float size, CGTextEncoding textEncoding)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			CGContextSelectFont (handle, name, size, textEncoding);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphsAtPositions(IntPtr context, ushort [] glyphs, PointF [] positions, int size_t_count);
		public void ShowGlyphsAtPositions (ushort [] glyphs, PointF [] positions, int size_t_count)
		{
			if (positions == null)
				throw new ArgumentNullException ("positions");
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			CGContextShowGlyphsAtPositions (handle, glyphs, positions, size_t_count);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowText(IntPtr c, string s, int size_t_length);
		public void ShowText (string str, int count)
		{
			if (str == null)
				throw new ArgumentNullException ("str");
			if (count > str.Length)
				throw new ArgumentException ("count");
			CGContextShowText (handle, str, count);
		}

		public void ShowText (string str)
		{
			if (str == null)
				throw new ArgumentNullException ("str");
			CGContextShowText (handle, str, str.Length);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowText(IntPtr c, byte[] bytes, int size_t_length);
		public void ShowText (byte[] bytes, int count)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");
			if (count > bytes.Length)
				throw new ArgumentException ("count");
			CGContextShowText (handle, bytes, count);
		}
		
		public void ShowText (byte[] bytes)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");
			CGContextShowText (handle, bytes, bytes.Length);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowTextAtPoint(IntPtr c, float x, float y, string str, int size_t_length);
		public void ShowTextAtPoint (float x, float y, string str, int length)
		{
			if (str == null)
				throw new ArgumentNullException ("str");
			CGContextShowTextAtPoint (handle, x, y, str, length);
		}

		public void ShowTextAtPoint (float x, float y, string str)
		{
			if (str == null)
				throw new ArgumentNullException ("str");
			CGContextShowTextAtPoint (handle, x, y, str, str.Length);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowTextAtPoint(IntPtr c, float x, float y, byte[] bytes, int size_t_length);
		public void ShowTextAtPoint (float x, float y, byte[] bytes, int length)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");
			CGContextShowTextAtPoint (handle, x, y, bytes, length);
		}
		
		public void ShowTextAtPoint (float x, float y, byte[] bytes)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");
			CGContextShowTextAtPoint (handle, x, y, bytes, bytes.Length);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphs(IntPtr c, ushort [] glyphs, int size_t_count);
		public void ShowGlyphs (ushort [] glyphs)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			CGContextShowGlyphs (handle, glyphs, glyphs.Length);
		}

		public void ShowGlyphs (ushort [] glyphs, int count)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			if (count > glyphs.Length)
				throw new ArgumentException ("count");
			CGContextShowGlyphs (handle, glyphs, count);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphsAtPoint(IntPtr context, float x, float y, ushort [] glyphs, int size_t_count);
		public void ShowGlyphsAtPoint (float x, float y, ushort [] glyphs, int count)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			if (count > glyphs.Length)
				throw new ArgumentException ("count");
			CGContextShowGlyphsAtPoint (handle, x, y, glyphs, count);
		}

		public void ShowGlyphsAtPoint (float x, float y, ushort [] glyphs)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			
			CGContextShowGlyphsAtPoint (handle, x, y, glyphs, glyphs.Length);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextShowGlyphsWithAdvances(IntPtr c, ushort [] glyphs, SizeF [] advances, int size_t_count);
		public void ShowGlyphsWithAdvances (ushort [] glyphs, SizeF [] advances, int count)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			if (advances == null)
				throw new ArgumentNullException ("advances");
			if (count > glyphs.Length || count > advances.Length)
				throw new ArgumentException ("count");
			CGContextShowGlyphsWithAdvances (handle, glyphs, advances, count);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawPDFPage(IntPtr c, IntPtr page);
		public void DrawPDFPage (CGPDFPage page)
		{
			CGContextDrawPDFPage (handle, page.handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGContextBeginPage(IntPtr c, ref RectangleF mediaBox);
		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGContextBeginPage(IntPtr c, IntPtr zero);
		public void BeginPage (RectangleF? rect)
		{
			if (rect.HasValue){
				RectangleF v = rect.Value;
				CGContextBeginPage (handle, ref v);
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
		extern static PointF CGContextConvertPointToDeviceSpace(IntPtr context, PointF point);
		public PointF PointToDeviceSpace (PointF point)
		{
			return CGContextConvertPointToDeviceSpace (handle, point);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static PointF CGContextConvertPointToUserSpace(IntPtr context, PointF point);
		public PointF ConvertPointToUserSpace (PointF point)
		{
			return CGContextConvertPointToUserSpace (handle, point);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static SizeF CGContextConvertSizeToDeviceSpace(IntPtr context, SizeF size);
		public SizeF ConvertSizeToDeviceSpace (SizeF size)
		{
			return CGContextConvertSizeToDeviceSpace (handle, size);
		}
		
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static SizeF CGContextConvertSizeToUserSpace(IntPtr context, SizeF size);
		public SizeF ConvertSizeToUserSpace (SizeF size)
		{
			return CGContextConvertSizeToUserSpace (handle, size);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static RectangleF CGContextConvertRectToDeviceSpace(IntPtr context, RectangleF rect);
		public RectangleF ConvertRectToDeviceSpace (RectangleF rect)
		{
			return CGContextConvertRectToDeviceSpace (handle, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static RectangleF CGContextConvertRectToUserSpace(IntPtr context, RectangleF rect);
		public RectangleF ConvertRectToUserSpace (RectangleF rect)
		{
			return CGContextConvertRectToUserSpace (handle, rect);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawLayerInRect (IntPtr context, RectangleF rect, IntPtr layer);
		public void DrawLayer (CGLayer layer, RectangleF rect)
		{
			if (layer == null)
				throw new ArgumentNullException ("layer");
			CGContextDrawLayerInRect (handle, rect, layer.Handle);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGContextDrawLayerAtPoint (IntPtr context, PointF rect, IntPtr layer);

		public void DrawLayer (CGLayer layer, PointF point)
		{
			if (layer == null)
				throw new ArgumentNullException ("layer");
			CGContextDrawLayerAtPoint (handle, point, layer.Handle);
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
		extern static IntPtr CGContextBeginTransparencyLayerWithRect (IntPtr context, RectangleF rect, IntPtr dictionary);
		public void BeginTransparencyLayer (RectangleF rectangle, NSDictionary auxiliaryInfo = null)
		{
			CGContextBeginTransparencyLayerWithRect (handle, rectangle, auxiliaryInfo == null ? IntPtr.Zero : auxiliaryInfo.Handle);
		}

		public void BeginTransparencyLayer (RectangleF rectangle)
		{
			CGContextBeginTransparencyLayerWithRect (handle, rectangle, IntPtr.Zero);
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
