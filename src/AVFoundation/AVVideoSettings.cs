// 
// AVVideoSettings.cs: Implements strongly typed access for AV video settings
//
// Authors: Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2012, Xamarin Inc.
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
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;
using MonoMac.CoreVideo;

namespace MonoMac.AVFoundation {

	public enum AVVideoCodec
	{
		H264 = 1,
		JPEG = 2
	}

	public enum AVVideoScalingMode
	{
		Fit,
		Resize,
		ResizeAspect,
		ResizeAspectFill
	}

	public class AVVideoSettingsUncompressed : PixelBufferAttributes
	{
#if !COREBUILD
		public AVVideoSettingsUncompressed ()
		{
		}

		public AVVideoSettingsUncompressed (NSDictionary dictionary)
			: base (dictionary)
		{
		}

		public AVVideoScalingMode? ScalingMode {
			set {
				NSString v;
				switch (value) {
				case AVVideoScalingMode.Fit:
					v = AVVideoScalingModeKey.Fit;
					break;
				case AVVideoScalingMode.Resize:
					v = AVVideoScalingModeKey.Resize;
					break;
				case AVVideoScalingMode.ResizeAspect:
					v = AVVideoScalingModeKey.ResizeAspect;
					break;
				case AVVideoScalingMode.ResizeAspectFill:
					v = AVVideoScalingModeKey.ResizeAspectFill;
					break;
				case null:
					v = null;
					break;
				default:
					throw new ArgumentException ("value");
				}

				if (v == null)
					RemoveValue (AVVideo.ScalingModeKey);
				else
					SetNativeValue (AVVideo.ScalingModeKey, v);
			}
		}
#endif
	}

	public class AVVideoSettingsCompressed : DictionaryContainer
	{
#if !COREBUILD
		public AVVideoSettingsCompressed ()
			: base (new NSMutableDictionary ())
		{
		}

		public AVVideoSettingsCompressed (NSDictionary dictionary)
			: base (dictionary)
		{
		}

		public AVVideoCodec? Codec {
			set {
				NSString v;
				switch (value) {
				case AVVideoCodec.H264:
					v = AVVideo.CodecH264;
					break;
				case AVVideoCodec.JPEG:
					v = AVVideo.CodecJPEG;
					break;
				case null:
					v = null;
					break;
				default:
					throw new ArgumentException ("value");
				}

				if (v == null)
					RemoveValue (AVVideo.CodecKey);
				else
					SetNativeValue (AVVideo.CodecKey, v);
			}
		}

		public int? Width {
			set {
				SetNumberValue (AVVideo.WidthKey, value);
			}
			get {
				return GetInt32Value (AVVideo.WidthKey);
			}
		}

		public int? Height {
			set {
				SetNumberValue (AVVideo.HeightKey, value);
			}
			get {
				return GetInt32Value (AVVideo.HeightKey);
			}
		}

		public AVVideoScalingMode? ScalingMode {
			set {
				NSString v;
				switch (value) {
				case AVVideoScalingMode.Fit:
					v = AVVideoScalingModeKey.Fit;
					break;
				case AVVideoScalingMode.Resize:
					v = AVVideoScalingModeKey.Resize;
					break;
				case AVVideoScalingMode.ResizeAspect:
					v = AVVideoScalingModeKey.ResizeAspect;
					break;
				case AVVideoScalingMode.ResizeAspectFill:
					v = AVVideoScalingModeKey.ResizeAspectFill;
					break;
				case null:
					v = null;
					break;
				default:
					throw new ArgumentException ("value");
				}

				if (v == null)
					RemoveValue (AVVideo.ScalingModeKey);
				else
					SetNativeValue (AVVideo.ScalingModeKey, v);
			}
		}
#endif
	}
}

