//
// AVFoundation.cs: 
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2011, Novell, Inc.
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
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using MonoMac.CoreFoundation;
using MonoMac.CoreVideo;

namespace MonoMac.AVFoundation {

	public partial class AVVideoSettings {
		public AVVideoSettings ()
		{
		}

		public AVVideoSettings (CVPixelFormatType formatType)
		{
			PixelFormat = formatType;
		}
		
		public CVPixelFormatType? PixelFormat { get; set; }
		
		public NSDictionary ToDictionary ()
		{
			if (!PixelFormat.HasValue)
				return null;

			return NSDictionary.FromObjectAndKey (new NSNumber ((int) PixelFormat.Value), CVPixelBuffer.PixelFormatTypeKey);
		}
	}
	
	public partial class AVCaptureVideoDataOutput {
		public void SetSampleBufferDelegateAndQueue (AVCaptureVideoDataOutputSampleBufferDelegate sampleBufferDelegate, DispatchQueue queue)
		{
			SetSampleBufferDelegate (sampleBufferDelegate, queue == null ? IntPtr.Zero : queue.Handle);
		}

		public AVVideoSettings VideoSettings {
			set {
				WeakVideoSettings = value == null ? null : value.ToDictionary ();
			}
			get {
				var dict = WeakVideoSettings;
				NSObject val;
				if (dict.TryGetValue (CVPixelBuffer.PixelFormatTypeKey, out val) && val is NSNumber){
					var number = val as NSNumber;
					return new AVVideoSettings ((CVPixelFormatType) number.Int32Value);
				}
				return new AVVideoSettings ();
			}
		}
	}
}