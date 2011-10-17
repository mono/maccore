//
// Authors:
//   Miguel de Icaza
//
// Copyright 2011, Xamarin, Inc.
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
using MonoMac.Foundation;
using MonoMac.CoreGraphics;
using System.Drawing;
using MonoMac.CoreFoundation;

namespace MonoMac.CoreImage {
	public partial class CIDetector {
		public static CIDetector CreateFaceDetector (CIContext context, bool highAccuracy)
		{
			// TypeFace is the only detector supported now
			using (var options = NSDictionary.FromObjectsAndKeys (new NSObject [] { highAccuracy ? AccuracyHigh : AccuracyLow },
									      new NSObject [] { Accuracy }))
				return FromType (TypeFace, context, options);
		}
		
		public CIFeature [] FeaturesInImage (CIImage image, CIImageOrientation orientation)
		{
			using (var options = NSDictionary.FromObjectsAndKeys (new NSObject [] { new NSNumber ((int) orientation) },
									      new NSObject [] { ImageOrientation })){
				return FeaturesInImage (image, options);
			}
		}
	}
}
