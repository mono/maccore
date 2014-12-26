// 
// CTRun.cs: Implements the managed CTFrame
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
using MonoMac.CoreFoundation;
using MonoMac.CoreGraphics;

namespace MonoMac.CoreText {

	[Since (3,2)]
	public enum CTRunStatus {
		NoStatus = 0,
		RightToLeft = (1 << 0),
		NonMonotonic = (1 << 1),
		HasNonIdentityMatrix = (1 << 2)
	}

	[Since (3,2)]
	public class CTRun : CFType {
		internal CTRun (IntPtr handle)
			: this (handle, false)
		{
		}

		internal CTRun (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}
		
		~CTRun ()
		{
			Dispose (false);
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static void CTRunDraw (IntPtr h, IntPtr context, NSRange range);
		public void Draw (CGContext context, NSRange range)
		{
			CTRunDraw (Handle, context.Handle, range);
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static void CTRunGetAdvances (IntPtr h, NSRange range, [In, Out] SizeF [] buffer);
		public SizeF[] GetAdvances (NSRange range, SizeF[] buffer)
		{
			buffer = GetBuffer (range, buffer);

			CTRunGetAdvances (Handle, range, buffer);

			return buffer;
		}

		T[] GetBuffer<T> (NSRange range, T[] buffer)
		{
			var glyphCount = GlyphCount;

			if (buffer != null && range.Length != 0 && buffer.Length < range.Length)
				throw new ArgumentException ("buffer.Length must be >= range.Length.", "buffer");
			if (buffer != null && range.Length == 0 && buffer.Length < glyphCount)
				throw new ArgumentException ("buffer.Length must be >= GlyphCount.", "buffer");

			return buffer ?? new T [range.Length == 0 ? glyphCount : range.Length];
		}

		public SizeF [] GetAdvances (NSRange range) {
			return GetAdvances (range, null);
		}

		public SizeF [] GetAdvances ()
		{
			return GetAdvances (new NSRange (0, 0), null);
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static IntPtr CTRunGetAttributes (IntPtr handle);

		public CTStringAttributes GetAttributes ()
		{
			var d = (NSDictionary) Runtime.GetNSObject (CTRunGetAttributes (Handle));
			return d == null ? null : new CTStringAttributes (d);
		}


		[DllImport (Constants.CoreTextLibrary)]
		extern static int CTRunGetGlyphCount (IntPtr handle);
		public int GlyphCount {
			get {
				return CTRunGetGlyphCount (Handle);
			}
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static void CTRunGetGlyphs (IntPtr h, NSRange range, [In, Out] ushort [] buffer);
		public ushort[] GetGlyphs (NSRange range, ushort[] buffer)
		{
			buffer = GetBuffer (range, buffer);

			CTRunGetGlyphs (Handle, range, buffer);

			return buffer;
		}

		public ushort [] GetGlyphs (NSRange range) {
			return GetGlyphs (range, null);
		}

		public ushort [] GetGlyphs ()
		{
			return GetGlyphs (new NSRange (0, 0), null);
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static RectangleF CTRunGetImageBounds (IntPtr h, IntPtr context, NSRange range);
		public RectangleF GetImageBounds (CGContext context, NSRange range) {
			return CTRunGetImageBounds (Handle, context.Handle, range);
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static void CTRunGetPositions (IntPtr h, NSRange range, [In, Out] PointF [] buffer);
		public PointF [] GetPositions (NSRange range, PointF[] buffer)
		{
			buffer = GetBuffer (range, buffer);

			CTRunGetPositions (Handle, range, buffer);

			return buffer;
		}

		public PointF [] GetPositions (NSRange range) {
			return GetPositions (range, null);
		}

		public PointF [] GetPositions ()
		{
			return GetPositions (new NSRange (0, 0), null);
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static CTRunStatus CTRunGetStatus (IntPtr handle);
		public CTRunStatus Status {
			get {
				return CTRunGetStatus (Handle);
			}
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static void CTRunGetStringIndices (IntPtr h, NSRange range, [In, Out] int [] buffer);
		public int [] GetStringIndices (NSRange range, int[] buffer)
		{
			buffer = GetBuffer (range, buffer);

			CTRunGetStringIndices (Handle, range, buffer);

			return buffer;
		}

		public int [] GetStringIndices (NSRange range) {
			return GetStringIndices (range, null);
		}

		public int [] GetStringIndices ()
		{
			return GetStringIndices (new NSRange (0, 0), null);
		}

		[DllImport (Constants.CoreTextLibrary)]
		extern static NSRange CTRunGetStringRange (IntPtr handle);
		public NSRange StringRange {
			get {
				return CTRunGetStringRange (Handle);
			}
		}
		
		[DllImport (Constants.CoreTextLibrary)]
		extern static CGAffineTransform CTRunGetTextMatrix (IntPtr handle);
		public CGAffineTransform TextMatrix {
			get {
				return CTRunGetTextMatrix (Handle);
			}
		}
		
		[DllImport (Constants.CoreTextLibrary)]
		extern static double CTRunGetTypographicBounds (IntPtr h, NSRange range, out float ascent, out float descent, out float leading);
		[DllImport (Constants.CoreTextLibrary)]
		extern static double CTRunGetTypographicBounds (IntPtr h, NSRange range, IntPtr ascent, IntPtr descent, IntPtr leading);
		public double GetTypographicBounds (NSRange range, out float ascent, out float descent, out float leading) {
			return CTRunGetTypographicBounds (Handle, range, out ascent, out descent, out leading);
		}

		public double GetTypographicBounds ()
		{
			NSRange range = new NSRange () { Location = 0, Length = 0 };
			return CTRunGetTypographicBounds (Handle, range, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
		}
	}
}

