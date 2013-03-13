// 
// CGAffineTransform.cs: Implements the managed side
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

	[StructLayout(LayoutKind.Sequential)]
	public struct CGAffineTransform {
		CGFloat a,b,c,d,tx,ty;

		public float xx {
			get { return (float)a; }
			set { a = value; }
		}
		public float yx {
			get { return (float)b; }
			set { b = value; }
		}
		public float xy {
			get { return (float)c; }
			set { c = value; }
		}
		public float yy {
			get { return (float)d; }
			set { d = value; }
		}
		public float x0 {
			get { return (float)tx; }
			set { tx = value; }
		}
		public float y0 {
			get { return (float)ty; }
			set { ty = value; }
		}

		//
		// Constructors
		//
		public CGAffineTransform (float xx, float yx, float xy, float yy, float x0, float y0)
		{
			this.a = xx;
			this.b = yx;
			this.c = xy;
			this.d = yy;
			this.tx = x0;
			this.ty = y0;
		}
		
		// Identity
		public static CGAffineTransform MakeIdentity ()
		{
			return new CGAffineTransform (1, 0, 0, 1, 0, 0);
		}
		
		public static CGAffineTransform MakeRotation (float angle)
		{
			return new CGAffineTransform (
				(float)Math.Cos (angle), (float)Math.Sin (angle),
				(float)-Math.Sin (angle), (float)Math.Cos (angle),
				0, 0);
		}

		public static CGAffineTransform MakeScale (float sx, float sy)
		{
			return new CGAffineTransform (sx, 0, 0, sy, 0, 0);
		}

		public static CGAffineTransform MakeTranslation (float tx, float ty)
		{
			return new CGAffineTransform (1, 0, 0, 1, tx, ty);
		}

		//
		// Operations
		//
		public static CGAffineTransform Multiply (CGAffineTransform a, CGAffineTransform b)
		{
			return new CGAffineTransform ((float)(a.xx * b.xx + a.yx * b.xy),
						      (float)(a.xx * b.yx + a.yx * b.yy),
						      (float)(a.xy * b.xx + a.yy * b.xy),
						      (float)(a.xy * b.yx + a.yy * b.yy),
						      (float)(a.x0 * b.xx + a.y0 * b.xy + b.x0),
						      (float)(a.x0 * b.yx + a.y0 * b.yy + b.y0));
		}

		public void Multiply (CGAffineTransform b)
		{
			var a = this;
			xx = a.xx * b.xx + a.yx * b.xy;
			yx = a.xx * b.yx + a.yx * b.yy;
			xy = a.xy * b.xx + a.yy * b.xy;
			yy = a.xy * b.yx + a.yy * b.yy;
			x0 = a.x0 * b.xx + a.y0 * b.xy + b.x0;
			y0 = a.x0 * b.yx + a.y0 * b.yy + b.y0;
		}
		
		public void Scale (float sx, float sy)
		{
			Multiply (MakeScale (sx, sy));
		}

		public void Translate (float tx, float ty)
		{
			Multiply (MakeTranslation (tx, ty));
		}

		public void Rotate (float angle)
		{
			Multiply (MakeRotation (angle));
		}
		
		public bool IsIdentity {
			get {
				return xx == 1 && yx == 0 && xy == 0 && yy == 1 && x0 == 0 && y0 == 0;
			}
		}
		
                public override String ToString ()
                {
                        String s = String.Format ("xx:{0:##0.0#} yx:{1:##0.0#} xy:{2:##0.0#} yy:{3:##0.0#} x0:{4:##0.0#} y0:{5:##0.0#}", xx, yx, xy, yy, x0, y0);
                        return s;
                }
                
                public static bool operator == (CGAffineTransform lhs, CGAffineTransform rhs)
                {
                        return (lhs.xx == rhs.xx && lhs.xy == rhs.xy &&
                                lhs.yx == rhs.yx && lhs.yy == rhs.yy &&
                                lhs.x0 == rhs.x0 && lhs.y0 == rhs.y0 );
                }
                
                public static bool operator != (CGAffineTransform lhs, CGAffineTransform rhs)
                {
                        return !(lhs==rhs);
                }

		public static CGAffineTransform operator * (CGAffineTransform a, CGAffineTransform b)
		{
			return new CGAffineTransform ((float)(a.xx * b.xx + a.yx * b.xy),
						      (float)(a.xx * b.yx + a.yx * b.yy),
						      (float)(a.xy * b.xx + a.yy * b.xy),
						      (float)(a.xy * b.yx + a.yy * b.yy),
						      (float)(a.x0 * b.xx + a.y0 * b.xy + b.x0),
						      (float)(a.x0 * b.yx + a.y0 * b.yy + b.y0));
		}
                
                public override bool Equals(object o)
                {
                        if (! (o is CGAffineTransform))
                                return false;
                        else
                                return (this == (CGAffineTransform) o);
                }
                
                public override int GetHashCode()
                {
                        return  (int)this.xx ^ (int)this.xy ^
                                (int)this.yx ^ (int)this.yy ^
                                (int)this.x0 ^ (int)this.y0;
                }
                
		public PointF TransformPoint (PointF point)
		{
			return new PointF ((float)(xx * point.X + xy * point.Y + x0),
					   (float)(yx * point.X + yy * point.Y + y0));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		public extern static NSRect CGRectApplyAffineTransform (NSRect rect, CGAffineTransform t);

		public NSRect TransformRect (NSRect rect)
		{
			return CGRectApplyAffineTransform (rect, this);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		public extern static CGAffineTransform CGAffineTransformInvert (CGAffineTransform t);

		public CGAffineTransform Invert ()
		{
			return CGAffineTransformInvert (this);
		}
	}
}
