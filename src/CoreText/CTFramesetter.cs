// 
// CTFramesetter.cs: Implements the managed CTFramesetter
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
	public class CTFramesetter : CFType {
		internal CTFramesetter (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}

		~CTFramesetter ()
		{
			Dispose (false);
		}

#region Framesetter Creation
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFramesetterCreateWithAttributedString (IntPtr @string);
		public CTFramesetter (NSAttributedString value)
		{
			if (value == null)
				throw ConstructorError.ArgumentNull (this, "value");
			Handle = CTFramesetterCreateWithAttributedString (value.Handle);
			if (Handle == IntPtr.Zero)
				throw ConstructorError.Unknown (this);
		}
#endregion

#region Frame Creation
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFramesetterCreateFrame (IntPtr framesetter, NSRange stringRange, IntPtr path, IntPtr frameAttributes);
		public CTFrame GetFrame (NSRange stringRange, CGPath path, CTFrameAttributes frameAttributes)
		{
			if (path == null)
				throw new ArgumentNullException ("path");
			var frame = CTFramesetterCreateFrame (Handle, stringRange, path.Handle,
					frameAttributes == null ? IntPtr.Zero : frameAttributes.Dictionary.Handle);
			if (frame == IntPtr.Zero)
				return null;
			return new CTFrame (frame, true);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFramesetterGetTypesetter (IntPtr framesetter);
		public CTTypesetter GetTypesetter ()
		{
			var h = CTFramesetterGetTypesetter (Handle);

			if (h == IntPtr.Zero)
				return null;
			return new CTTypesetter (h, false);
		}
#endregion

#region Frame Sizing
		[DllImport (Constants.CoreTextLibrary)]
		static extern SizeF CTFramesetterSuggestFrameSizeWithConstraints (
				IntPtr framesetter, NSRange stringRange, IntPtr frameAttributes, SizeF constraints, out NSRange fitRange);
		public SizeF SuggestFrameSize (NSRange stringRange, CTFrameAttributes frameAttributes, SizeF constraints, out NSRange fitRange)
		{
			return CTFramesetterSuggestFrameSizeWithConstraints (
					Handle, stringRange,
					frameAttributes == null ? IntPtr.Zero : frameAttributes.Dictionary.Handle,
					constraints, out fitRange);
		}
#endregion
	}
}

