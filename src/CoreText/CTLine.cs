// 
// CTLine.cs: Implements the managed CTLine
//
// Authors: Mono Team
//     
// Copyright 2010 Novell, Inc
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
using MonoMac.CoreFoundation;
using MonoMac.CoreGraphics;

namespace MonoMac.CoreText {

	[Since (3,2)]
	public enum CTLineTruncation {
		Start = 0,
		End = 1,
		Middle = 2
	}

	public enum CTLineBoundsOptions {
		ExcludeTypographicLeading  = 1 << 0,
		ExcludeTypographicShifts   = 1 << 1,
		UseHangingPunctuation      = 1 << 2,
		UseGlyphPathBounds         = 1 << 3,
		UseOpticalBounds           = 1 << 4
    }
	
	[Since (3,2)]
	public class CTLine : CFType {
		internal CTLine (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}

		~CTLine ()
		{
			Dispose (false);
		}

#region Line Creation
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTLineCreateWithAttributedString (IntPtr @string);
		public CTLine (NSAttributedString value)
		{
			if (value == null)
				throw ConstructorError.ArgumentNull (this, "value");

			Handle = CTLineCreateWithAttributedString (value.Handle);

			if (Handle == IntPtr.Zero)
				throw ConstructorError.Unknown (this);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTLineCreateTruncatedLine (IntPtr line, double width, CTLineTruncation truncationType, IntPtr truncationToken);
		public CTLine GetTruncatedLine (double width, CTLineTruncation truncationType, CTLine truncationToken)
		{
			var h = CTLineCreateTruncatedLine (Handle, width, truncationType,
					truncationToken == null ? IntPtr.Zero : truncationToken.Handle);
			if (h == IntPtr.Zero)
				return null;
			return new CTLine (h, true);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTLineCreateJustifiedLine (IntPtr line, float justificationFactor, double justificationWidth);
		public CTLine GetJustifiedLine (float justificationFactor, double justificationWidth)
		{
			var h = CTLineCreateJustifiedLine (Handle, justificationFactor, justificationWidth);
			if (h == IntPtr.Zero)
				return null;
			return new CTLine (h, true);
		}
#endregion

#region Line Access
		[DllImport (Constants.CoreTextLibrary)]
		static extern int CTLineGetGlyphCount (IntPtr line);
		public int GlyphCount {
			get {return CTLineGetGlyphCount (Handle);}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTLineGetGlyphRuns (IntPtr line);
		public CTRun[] GetGlyphRuns ()
		{
			var cfArrayRef = CTLineGetGlyphRuns (Handle);
			if (cfArrayRef == IntPtr.Zero)
				return new CTRun [0];

			return NSArray.ArrayFromHandle (cfArrayRef, 
					v => new CTRun (v, false));
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern NSRange CTLineGetStringRange (IntPtr line);
		public NSRange StringRange {
			get {return CTLineGetStringRange (Handle);}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern double CTLineGetPenOffsetForFlush (IntPtr line, float flushFactor, double flushWidth);
		public double GetPenOffsetForFlush (float flushFactor, double flushWidth)
		{
			return CTLineGetPenOffsetForFlush (Handle, flushFactor, flushWidth);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern void CTLineDraw (IntPtr line, IntPtr context);
		public void Draw (CGContext context)
		{
			if (context == null)
				throw new ArgumentNullException ("context");
			CTLineDraw (Handle, context.Handle);
		}
#endregion

#region Line Measurement
		[DllImport (Constants.CoreTextLibrary)]
		static extern RectangleF CTLineGetImageBounds (IntPtr line, IntPtr context);
		public RectangleF GetImageBounds (CGContext context)
		{
			if (context == null)
				throw new ArgumentNullException ("context");
			return CTLineGetImageBounds (Handle, context.Handle);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern RectangleF CTLineGetBoundsWithOptions (IntPtr line, CTLineBoundsOptions options);
		[Since (6,0)]
		public RectangleF GetBounds (CTLineBoundsOptions options)
		{
			return CTLineGetBoundsWithOptions (Handle, options);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern double CTLineGetTypographicBounds (IntPtr line, out float ascent, out float descent, out float leading);
		public double GetTypographicBounds (out float ascent, out float descent, out float leading)
		{
			return CTLineGetTypographicBounds (Handle, out ascent, out descent, out leading);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern double CTLineGetTypographicBounds (IntPtr line, IntPtr ascent, IntPtr descent, IntPtr leading);
		public double GetTypographicBounds ()
		{
			return CTLineGetTypographicBounds (Handle, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern double CTLineGetTrailingWhitespaceWidth (IntPtr line);
		public double TrailingWhitespaceWidth {
			get {return CTLineGetTrailingWhitespaceWidth (Handle);}
		}
#endregion

#region Line Caret Positioning and Highlighting
		[DllImport (Constants.CoreTextLibrary)]
		static extern int CTLineGetStringIndexForPosition (IntPtr line, PointF position);
		public int GetStringIndexForPosition (PointF position)
		{
			return CTLineGetStringIndexForPosition (Handle, position);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern float CTLineGetOffsetForStringIndex (IntPtr line, int charIndex, out float secondaryOffset);
		public float GetOffsetForStringIndex (int charIndex, out float secondaryOffset)
		{
			return CTLineGetOffsetForStringIndex (Handle, charIndex, out secondaryOffset);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern float CTLineGetOffsetForStringIndex (IntPtr line, int charIndex, IntPtr secondaryOffset);
		public float GetOffsetForStringIndex (int charIndex)
		{
			return CTLineGetOffsetForStringIndex (Handle, charIndex, IntPtr.Zero);
		}
#endregion
	}
}

