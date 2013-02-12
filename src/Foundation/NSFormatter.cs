//
// Authors:
//   Jean-Louis Fuchs
//
// Copyright 2012
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

namespace MonoMac.Foundation {

	public abstract class NSFormatterDelegate : NSFormatter {

		public NSFormatterDelegate (IntPtr handle) : base (handle)
		{
		}
		
		[Export ("initWithCoder:")]
		public NSFormatterDelegate (NSCoder coder) : base (coder)
		{
		}

		[Export ("getObjectValue:forString:errorDescription:")]
		private bool _ObjectFor (IntPtr objectFor, string forString, IntPtr errorDescription)
		{
			string errorDescriptionString = null;
			NSObject outObjectFor = null;
			bool ret = ObjectFor (ref outObjectFor, forString, errorDescription != IntPtr.Zero, ref errorDescriptionString);
			if (outObjectFor != null) {
				Marshal.WriteIntPtr (objectFor, outObjectFor.Handle);
			}
			if (errorDescription != IntPtr.Zero && string.IsNullOrEmpty (errorDescriptionString) == false) {
				NSString errorDescriptionObj = new NSString (errorDescriptionString);
				Marshal.WriteIntPtr (errorDescription, errorDescriptionObj.Handle);
			}
			return ret;
		}
		
		public override abstract string StringFor (NSObject value);
		public abstract bool ObjectFor (ref NSObject objectFor, string forString, bool needsErrorDescription, ref string errorDescription);

	}
}
