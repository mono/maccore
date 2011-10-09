//
// CIFilter.cs: Extensions
//
// Copyright 2011 Xamarin Inc.
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
#if !MONOMAC
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
#endif

namespace MonoMac.CoreImage {
	public partial class CIFilter {
		public static string [] FilterNamesInCategories (params string [] categories)
		{
			return _FilterNamesInCategories (categories);
		}

		public NSObject this [NSString key] {
			get {
				return ValueForKey (key);
			}
			set {
				SetValueForKey (value, key);
			}
		}

		internal NSObject ValueForKey (string key)
		{
			using (var nskey = new NSString (key))
				return ValueForKey (nskey);
		}

		internal void SetValue (string key, NSObject value)
		{
			using (var nskey = new NSString (key)){
				SetValueForKey (value, nskey);
			}
		}
		
		internal static IntPtr CreateFilter (string name)
		{
			using (var nsname = new NSString (name))
				return MonoTouch.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (class_ptr, selFilterWithName, nsname.Handle);
		}

		// helper methods
		internal void SetFloat (string key, float value)
		{
			using (var nskey = new NSString (key))
				SetValueForKey (new NSNumber (value), nskey);
		}

		internal float GetFloat (string key)
		{
			using (var nskey = new NSString (key)){
				var v = ValueForKey (nskey);
				if (v is NSNumber)
					return (v as NSNumber).FloatValue;
				return 0;
			}
		}

		internal CIVector GetVector (string key)
		{
			return ValueForKey (key) as CIVector;
		}

		internal CIColor GetColor (string key)
		{
			return ValueForKey (key) as CIColor;
		}
		
		internal CIImage GetInputImage ()
		{
			return ValueForKey (CIFilterInputKey.Image) as CIImage;
		}

		internal void SetInputImage (CIImage value)
		{
			SetValueForKey (value, CIFilterInputKey.Image);
		}

		internal CIImage GetBackgroundImage ()
		{
			using (var nsstr = new NSString ("inputBackgroundImage"))
				return ValueForKey (nsstr) as CIImage;
		}

		internal void SetBackgroundImage (CIImage value)
		{
			using (var nsstr = new NSString ("inputBackgroundImage"))
				SetValueForKey (value, nsstr);
		}				
	}

	public class CIAdditionCompositing : CIFilter {
		public CIAdditionCompositing () : base (CreateFilter ("CIAdditionCompositing")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
	}
	
	public class CIAffineTransform : CIFilter {
		public CIAffineTransform () : base (CreateFilter ("CIAffineTransform")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
#if !MONOMAC
		public CGAffineTransform Transform {
			get {
				var val = ValueForKey ("inputTransform");
				if (val is NSValue)
					return (val as NSValue).CGAffineTransformValue;
				return new CGAffineTransform (1, 0, 0, 1, 0, 0);
			}
			set {
				SetValue ("inputTransform", NSValue.FromCGAffineTransform (value));
			}
		}
#endif
	}
	
	public class CICheckerboardGenerator : CIFilter {
		public CICheckerboardGenerator () : base (CreateFilter ("CICheckerboardGenerator")) {}
	
		public CIVector Center {
			get {
				return GetVector ("inputCenter");
			}
			set {
				SetValue ("inputCenter", value);
			}
		}
		
		public CIColor Color0 {
			get {
				return GetColor("inputColor0");
			}
			set {
				SetValue("inputColor0", value);
			}
		}
		
		public CIColor Color1 {
			get {
				return GetColor("inputColor1");
			}
			set {
				SetValue("inputColor1", value);
			}
		}
		
		public float Width {
			get {
				return GetFloat("inputWidth");
			}
			set {
				SetFloat ("inputWidth", value);
			}
		}
		
		public float Sharpness {
			get {
				return GetFloat("inputSharpness");
			}
			set {
				SetFloat("inputSharpness", value);
			}
		}
		
	}
	
	public class CIColorBlendMode : CIFilter {
		public CIColorBlendMode () : base (CreateFilter ("CIColorBlendMode")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CIColorBurnBlendMode : CIFilter {
		public CIColorBurnBlendMode () : base (CreateFilter ("CIColorBurnBlendMode")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CIColorControls : CIFilter {
		public CIColorControls () : base (CreateFilter ("CIColorControls")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float Saturation {
			get {
				return GetFloat("inputSaturation");
			}
			set {
				SetFloat("inputSaturation", value);
			}
		}
		
		public float Brightness {
			get {
				return GetFloat("inputBrightness");
			}
			set {
				SetFloat("inputBrightness", value);
			}
		}
		
		public float Contrast {
			get {
				return GetFloat("inputContrast");
			}
			set {
				SetFloat("inputContrast", value);
			}
		}
		
	}
	
	public class CIColorCube : CIFilter {
		public CIColorCube () : base (CreateFilter ("CIColorCube")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float CubeDimension {
			get {
				return GetFloat("inputCubeDimension");
			}
			set {
				SetFloat("inputCubeDimension", value);
			}
		}
		
		public NSData CubeData {
			get {
				return ValueForKey ("inputCubeData") as NSData;
			}
			set {
				SetValue ("inputCubeData", value);
			}
		}
		
	}
	
	public class CIColorDodgeBlendMode : CIFilter {
		public CIColorDodgeBlendMode () : base (CreateFilter ("CIColorDodgeBlendMode")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CIColorInvert : CIFilter {
		public CIColorInvert () : base (CreateFilter ("CIColorInvert")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
	}
	
	public class CIColorMatrix : CIFilter {
		public CIColorMatrix () : base (CreateFilter ("CIColorMatrix")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIVector RVector {
			get {
				return GetVector("inputRVector");
			}
			set {
				SetValue("inputRVector", value);
			}
		}
		
		public CIVector GVector {
			get {
				return GetVector("inputGVector");
			}
			set {
				SetValue("inputGVector", value);
			}
		}
		
		public CIVector BVector {
			get {
				return GetVector("inputBVector");
			}
			set {
				SetValue("inputBVector", value);
			}
		}
		
		public CIVector AVector {
			get {
				return GetVector("inputAVector");
			}
			set {
				SetValue("inputAVector", value);
			}
		}
		
		public CIVector BiasVector {
			get {
				return GetVector("inputBiasVector");
			}
			set {
				SetValue("inputBiasVector", value);
			}
		}
		
	}
	
	public class CIColorMonochrome : CIFilter {
		public CIColorMonochrome () : base (CreateFilter ("CIColorMonochrome")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIColor Color {
			get {
				return GetColor("inputColor");
			}
			set {
				SetValue("inputColor", value);
			}
		}
		
		public float Intensity {
			get {
				return GetFloat("inputIntensity");
			}
			set {
				SetFloat("inputIntensity", value);
			}
		}
		
	}
	
	public class CIConstantColorGenerator : CIFilter {
		public CIConstantColorGenerator () : base (CreateFilter ("CIConstantColorGenerator")) {}
	
		public CIColor Color {
			get {
				return GetColor("inputColor");
			}
			set {
				SetValue("inputColor", value);
			}
		}
		
	}
	
	public class CICrop : CIFilter {
		public CICrop () : base (CreateFilter ("CICrop")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIVector Rectangle {
			get {
				return GetVector("inputRectangle");
			}
			set {
				SetValue("inputRectangle", value);
			}
		}
		
	}
	
	public class CIDarkenBlendMode : CIFilter {
		public CIDarkenBlendMode () : base (CreateFilter ("CIDarkenBlendMode")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CIDifferenceBlendMode : CIFilter {
		public CIDifferenceBlendMode () : base (CreateFilter ("CIDifferenceBlendMode")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CIExclusionBlendMode : CIFilter {
		public CIExclusionBlendMode () : base (CreateFilter ("CIExclusionBlendMode")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CIExposureAdjust : CIFilter {
		public CIExposureAdjust () : base (CreateFilter ("CIExposureAdjust")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float EV {
			get {
				return GetFloat("inputEV");
			}
			set {
				SetFloat("inputEV", value);
			}
		}
		
	}
	
	public class CIFalseColor : CIFilter {
		public CIFalseColor () : base (CreateFilter ("CIFalseColor")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIColor Color0 {
			get {
				return GetColor("inputColor0");
			}
			set {
				SetValue("inputColor0", value);
			}
		}
		
		public CIColor Color1 {
			get {
				return GetColor("inputColor1");
			}
			set {
				SetValue("inputColor1", value);
			}
		}
		
	}
	
	public class CIGammaAdjust : CIFilter {
		public CIGammaAdjust () : base (CreateFilter ("CIGammaAdjust")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float Power {
			get {
				return GetFloat("inputPower");
			}
			set {
				SetFloat("inputPower", value);
			}
		}
		
	}
	
	public class CIGaussianGradient : CIFilter {
		public CIGaussianGradient () : base (CreateFilter ("CIGaussianGradient")) {}
	
		public CIVector Center {
			get {
				return GetVector("inputCenter");
			}
			set {
				SetValue("inputCenter", value);
			}
		}
		
		public CIColor Color0 {
			get {
				return GetColor("inputColor0");
			}
			set {
				SetValue("inputColor0", value);
			}
		}
		
		public CIColor Color1 {
			get {
				return GetColor("inputColor1");
			}
			set {
				SetValue("inputColor1", value);
			}
		}
		
		public float Radius {
			get {
				return GetFloat("inputRadius");
			}
			set {
				SetFloat("inputRadius", value);
			}
		}
		
	}
	
	public class CIHardLightBlendMode : CIFilter {
		public CIHardLightBlendMode () : base (CreateFilter ("CIHardLightBlendMode")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CIHighlightShadowAdjust : CIFilter {
		public CIHighlightShadowAdjust () : base (CreateFilter ("CIHighlightShadowAdjust")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float ShadowAmount {
			get {
				return GetFloat("inputShadowAmount");
			}
			set {
				SetFloat("inputShadowAmount", value);
			}
		}
		
		public float HighlightAmount {
			get {
				return GetFloat("inputHighlightAmount");
			}
			set {
				SetFloat("inputHighlightAmount", value);
			}
		}
		
	}
	
	public class CIHueAdjust : CIFilter {
		public CIHueAdjust () : base (CreateFilter ("CIHueAdjust")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float Angle {
			get {
				return GetFloat("inputAngle");
			}
			set {
				SetFloat("inputAngle", value);
			}
		}
		
	}
	
	public class CIHueBlendMode : CIFilter {
		public CIHueBlendMode () : base (CreateFilter ("CIHueBlendMode")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CILightenBlendMode : CIFilter {
		public CILightenBlendMode () : base (CreateFilter ("CILightenBlendMode")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CILinearGradient : CIFilter {
		public CILinearGradient () : base (CreateFilter ("CILinearGradient")) {}
	
		public CIVector Point0 {
			get {
				return GetVector("inputPoint0");
			}
			set {
				SetValue("inputPoint0", value);
			}
		}
		
		public CIVector Point1 {
			get {
				return GetVector("inputPoint1");
			}
			set {
				SetValue("inputPoint1", value);
			}
		}
		
		public CIColor Color0 {
			get {
				return GetColor("inputColor0");
			}
			set {
				SetValue("inputColor0", value);
			}
		}
		
		public CIColor Color1 {
			get {
				return GetColor("inputColor1");
			}
			set {
				SetValue("inputColor1", value);
			}
		}
		
	}
	
	public class CILuminosityBlendMode : CIFilter {
		public CILuminosityBlendMode () : base (CreateFilter ("CILuminosityBlendMode")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CIMaximumCompositing : CIFilter {
		public CIMaximumCompositing () : base (CreateFilter ("CIMaximumCompositing")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CIMinimumCompositing : CIFilter {
		public CIMinimumCompositing () : base (CreateFilter ("CIMinimumCompositing")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CIMultiplyBlendMode : CIFilter {
		public CIMultiplyBlendMode () : base (CreateFilter ("CIMultiplyBlendMode")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CIMultiplyCompositing : CIFilter {
		public CIMultiplyCompositing () : base (CreateFilter ("CIMultiplyCompositing")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CIOverlayBlendMode : CIFilter {
		public CIOverlayBlendMode () : base (CreateFilter ("CIOverlayBlendMode")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CIRadialGradient : CIFilter {
		public CIRadialGradient () : base (CreateFilter ("CIRadialGradient")) {}
	
		public CIVector Center {
			get {
				return GetVector("inputCenter");
			}
			set {
				SetValue("inputCenter", value);
			}
		}
		
		public float Radius0 {
			get {
				return GetFloat("inputRadius0");
			}
			set {
				SetFloat("inputRadius0", value);
			}
		}
		
		public float Radius1 {
			get {
				return GetFloat("inputRadius1");
			}
			set {
				SetFloat("inputRadius1", value);
			}
		}
		
		public CIColor Color0 {
			get {
				return GetColor("inputColor0");
			}
			set {
				SetValue("inputColor0", value);
			}
		}
		
		public CIColor Color1 {
			get {
				return GetColor("inputColor1");
			}
			set {
				SetValue("inputColor1", value);
			}
		}
		
	}
	
	public class CISaturationBlendMode : CIFilter {
		public CISaturationBlendMode () : base (CreateFilter ("CISaturationBlendMode")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CIScreenBlendMode : CIFilter {
		public CIScreenBlendMode () : base (CreateFilter ("CIScreenBlendMode")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CISepiaTone : CIFilter {
		public CISepiaTone () : base (CreateFilter ("CISepiaTone")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float Intensity {
			get {
				return GetFloat("inputIntensity");
			}
			set {
				SetFloat("inputIntensity", value);
			}
		}
		
	}
	
	public class CISoftLightBlendMode : CIFilter {
		public CISoftLightBlendMode () : base (CreateFilter ("CISoftLightBlendMode")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CISourceAtopCompositing : CIFilter {
		public CISourceAtopCompositing () : base (CreateFilter ("CISourceAtopCompositing")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CISourceInCompositing : CIFilter {
		public CISourceInCompositing () : base (CreateFilter ("CISourceInCompositing")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CISourceOutCompositing : CIFilter {
		public CISourceOutCompositing () : base (CreateFilter ("CISourceOutCompositing")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CISourceOverCompositing : CIFilter {
		public CISourceOverCompositing () : base (CreateFilter ("CISourceOverCompositing")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIImage BackgroundImage {
			get {
				return GetBackgroundImage ();
			}
			set {
				SetBackgroundImage (value);
			}
		}
		
	}
	
	public class CIStraightenFilter : CIFilter {
		public CIStraightenFilter () : base (CreateFilter ("CIStraightenFilter")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float Angle {
			get {
				return GetFloat("inputAngle");
			}
			set {
				SetFloat("inputAngle", value);
			}
		}
		
	}
	
	public class CIStripesGenerator : CIFilter {
		public CIStripesGenerator () : base (CreateFilter ("CIStripesGenerator")) {}
	
		public CIVector Center {
			get {
				return GetVector("inputCenter");
			}
			set {
				SetValue("inputCenter", value);
			}
		}
		
		public CIColor Color0 {
			get {
				return GetColor("inputColor0");
			}
			set {
				SetValue("inputColor0", value);
			}
		}
		
		public CIColor Color1 {
			get {
				return GetColor("inputColor1");
			}
			set {
				SetValue("inputColor1", value);
			}
		}
		
		public float Width {
			get {
				return GetFloat("inputWidth");
			}
			set {
				SetFloat("inputWidth", value);
			}
		}
		
		public float Sharpness {
			get {
				return GetFloat("inputSharpness");
			}
			set {
				SetFloat("inputSharpness", value);
			}
		}
		
	}
	
	public class CITemperatureAndTint : CIFilter {
		public CITemperatureAndTint () : base (CreateFilter ("CITemperatureAndTint")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIVector Neutral {
			get {
				return GetVector("inputNeutral");
			}
			set {
				SetValue("inputNeutral", value);
			}
		}
		
		public CIVector TargetNeutral {
			get {
				return GetVector("inputTargetNeutral");
			}
			set {
				SetValue("inputTargetNeutral", value);
			}
		}
	}
	
	public class CIToneCurve : CIFilter {
		public CIToneCurve () : base (CreateFilter ("CIToneCurve")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		public CIVector Point0 {
			get {
				return GetVector("inputPoint0");
			}
			set {
				SetValue("inputPoint0", value);
			}
		}
		
		public CIVector Point1 {
			get {
				return GetVector("inputPoint1");
			}
			set {
				SetValue("inputPoint1", value);
			}
		}
		
		public CIVector Point2 {
			get {
				return GetVector("inputPoint2");
			}
			set {
				SetValue("inputPoint2", value);
			}
		}
		
		public CIVector Point3 {
			get {
				return GetVector("inputPoint3");
			}
			set {
				SetValue("inputPoint3", value);
			}
		}
		
		public CIVector Point4 {
			get {
				return GetVector("inputPoint4");
			}
			set {
				SetValue("inputPoint4", value);
			}
		}
		
	}
	
	public class CIVibrance : CIFilter {
		public CIVibrance () : base (CreateFilter ("CIVibrance")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float Amount {
			get {
				return GetFloat("inputAmount");
			}
			set {
				SetFloat("inputAmount", value);
			}
		}
		
	}
	
	public class CIVignette : CIFilter {
		public CIVignette () : base (CreateFilter ("CIVignette")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public float Intensity {
			get {
				return GetFloat("inputIntensity");
			}
			set {
				SetFloat("inputIntensity", value);
			}
		}
		
		public float Radius {
			get {
				return GetFloat("inputRadius");
			}
			set {
				SetFloat("inputRadius", value);
			}
		}
		
	}
	
	public class CIWhitePointAdjust : CIFilter {
		public CIWhitePointAdjust () : base (CreateFilter ("CIWhitePointAdjust")) {}
	
		public CIImage Image {
			get {
				return GetInputImage ();
			}
			set {
				SetInputImage (value);
			}
		}
		
		public CIColor Color {
			get {
				return GetColor("inputColor");
			}
			set {
				SetValue("inputColor", value);
			}
		}
	}
}