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
using System.Diagnostics;
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
				return MonoTouch.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (class_ptr, selFilterWithName_, nsname.Handle);
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

		// Calls the selName selector for cases where we do not have an instance created
		static internal string GetFilterName (IntPtr filterHandle)
		{
			return NSString.FromHandle (MonoTouch.ObjCRuntime.Messaging.IntPtr_objc_msgSend (filterHandle, CIFilter.selName));
		}
		
		internal static CIFilter FromName (string filterName, IntPtr handle)
		{
			switch (filterName){
			case "CIAdditionCompositing":
				return new CIAdditionCompositing (handle);
			case "CIAffineTransform":
				return new CIAffineTransform (handle);
			case "CICheckerboardGenerator":
				return new CICheckerboardGenerator (handle);
			case "CIColorBlendMode":
				return new CIColorBlendMode (handle);
			case "CIColorBurnBlendMode":
				return new CIColorBurnBlendMode (handle);
			case "CIColorControls":
				return new CIColorControls (handle);
			case "CIColorCube":
				return new CIColorCube (handle);
			case "CIColorDodgeBlendMode":
				return new CIColorDodgeBlendMode (handle);
			case "CIColorInvert":
				return new CIColorInvert (handle);
			case "CIColorMatrix":
				return new CIColorMatrix (handle);
			case "CIColorMonochrome":
				return new CIColorMonochrome (handle);
			case "CIConstantColorGenerator":
				return new CIConstantColorGenerator (handle);
			case "CICrop":
				return new CICrop (handle);
			case "CIDarkenBlendMode":
				return new CIDarkenBlendMode (handle);
			case "CIDifferenceBlendMode":
				return new CIDifferenceBlendMode (handle);
			case "CIExclusionBlendMode":
				return new CIExclusionBlendMode (handle);
			case "CIExposureAdjust":
				return new CIExposureAdjust (handle);
			case "CIFalseColor":
				return new CIFalseColor (handle);
			case "CIGammaAdjust":
				return new CIGammaAdjust (handle);
			case "CIGaussianGradient":
				return new CIGaussianGradient (handle);
			case "CIHardLightBlendMode":
				return new CIHardLightBlendMode (handle);
			case "CIHighlightShadowAdjust":
				return new CIHighlightShadowAdjust (handle);
			case "CIHueAdjust":
				return new CIHueAdjust (handle);
			case "CIHueBlendMode":
				return new CIHueBlendMode (handle);
			case "CILightenBlendMode":
				return new CILightenBlendMode (handle);
			case "CILinearGradient":
				return new CILinearGradient (handle);
			case "CILuminosityBlendMode":
				return new CILuminosityBlendMode (handle);
			case "CIMaximumCompositing":
				return new CIMaximumCompositing (handle);
			case "CIMinimumCompositing":
				return new CIMinimumCompositing (handle);
			case "CIMultiplyBlendMode":
				return new CIMultiplyBlendMode (handle);
			case "CIMultiplyCompositing":
				return new CIMultiplyCompositing (handle);
			case "CIOverlayBlendMode":
				return new CIOverlayBlendMode (handle);
			case "CIRadialGradient":
				return new CIRadialGradient (handle);
			case "CISaturationBlendMode":
				return new CISaturationBlendMode (handle);
			case "CIScreenBlendMode":
				return new CIScreenBlendMode (handle);
			case "CISepiaTone":
				return new CISepiaTone (handle);
			case "CISoftLightBlendMode":
				return new CISoftLightBlendMode (handle);
			case "CISourceAtopCompositing":
				return new CISourceAtopCompositing (handle);
			case "CISourceInCompositing":
				return new CISourceInCompositing (handle);
			case "CISourceOutCompositing":
				return new CISourceOutCompositing (handle);
			case "CISourceOverCompositing":
				return new CISourceOverCompositing (handle);
			case "CIStraightenFilter":
				return new CIStraightenFilter (handle);
			case "CIStripesGenerator":
				return new CIStripesGenerator (handle);
			case "CITemperatureAndTint":
				return new CITemperatureAndTint (handle);
			case "CIToneCurve":
				return new CIToneCurve (handle);
			case "CIVibrance":
				return new CIVibrance (handle);
			case "CIVignette":
				return new CIVignette (handle);
			case "CIWhitePointAdjust":
				return new CIWhitePointAdjust (handle);
			case "CIFaceBalance":
				return new CIFaceBalance (handle);
			default:
				throw new NotImplementedException (String.Format ("Unknown filter type returned: `{0}', returning a default CIFilter", filterName));
			}
		}
	}

	public class CIFaceBalance : CIFilter {

		public CIFaceBalance (IntPtr handle): base (handle) {}
		
	}
	
	public class CIAdditionCompositing : CIFilter {
		public CIAdditionCompositing () : base (CreateFilter ("CIAdditionCompositing")) {}
		public CIAdditionCompositing (IntPtr handle) : base (handle) {}
	
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
		public CIAffineTransform (IntPtr handle) : base (handle) {}
	
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
		public CICheckerboardGenerator (IntPtr handle) : base (handle) {}
	
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
		public CIColorBlendMode (IntPtr handle) : base (handle) {}
	
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
		public CIColorBurnBlendMode (IntPtr handle) : base (handle) {}
	
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
		public CIColorControls (IntPtr handle) : base (handle) {}
	
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
		public CIColorCube (IntPtr handle) : base (handle) {}
	
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
		public CIColorDodgeBlendMode (IntPtr handle) : base (handle) {}
	
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
		public CIColorInvert (IntPtr handle) : base (handle) {}
	
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
		public CIColorMatrix (IntPtr handle) : base (handle) {}
	
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
		public CIColorMonochrome (IntPtr handle) : base (handle) {}
	
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
		public CIConstantColorGenerator (IntPtr handle) : base (handle) {}
	
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
		public CICrop (IntPtr handle) : base (handle) {}
	
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
		public CIDarkenBlendMode (IntPtr handle) : base (handle) {}
	
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
		public CIDifferenceBlendMode (IntPtr handle) : base (handle) {}
	
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
		public CIExclusionBlendMode (IntPtr handle) : base (handle) {}
	
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
		public CIExposureAdjust (IntPtr handle) : base (handle) {}
	
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
		public CIFalseColor (IntPtr handle) : base (handle) {}
	
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
		public CIGammaAdjust (IntPtr handle) : base (handle) {}
	
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
		public CIGaussianGradient (IntPtr handle) : base (handle) {}
	
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
		public CIHardLightBlendMode (IntPtr handle) : base (handle) {}
	
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
		public CIHighlightShadowAdjust (IntPtr handle) : base (handle) {}
	
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
		public CIHueAdjust (IntPtr handle) : base (handle) {}
	
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
		public CIHueBlendMode (IntPtr handle) : base (handle) {}
	
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
		public CILightenBlendMode (IntPtr handle) : base (handle) {}
	
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
		public CILinearGradient (IntPtr handle) : base (handle) {}
	
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
		public CILuminosityBlendMode (IntPtr handle) : base (handle) {}
	
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
		public CIMaximumCompositing (IntPtr handle) : base (handle) {}
	
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
		public CIMinimumCompositing (IntPtr handle) : base (handle) {}
	
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
		public CIMultiplyBlendMode (IntPtr handle) : base (handle) {}
	
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
		public CIMultiplyCompositing (IntPtr handle) : base (handle) {}
	
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
		public CIOverlayBlendMode (IntPtr handle) : base (handle) {}
	
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
		public CIRadialGradient (IntPtr handle) : base (handle) {}
	
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
		public CISaturationBlendMode (IntPtr handle) : base (handle) {}
	
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
		public CIScreenBlendMode (IntPtr handle) : base (handle) {}
	
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
		public CISepiaTone (IntPtr handle) : base (handle) {}
	
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
		public CISoftLightBlendMode (IntPtr handle) : base (handle) {}
	
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
		public CISourceAtopCompositing (IntPtr handle) : base (handle) {}
	
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
		public CISourceInCompositing (IntPtr handle) : base (handle) {}
	
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
		public CISourceOutCompositing (IntPtr handle) : base (handle) {}
	
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
		public CISourceOverCompositing (IntPtr handle) : base (handle) {}
	
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
		public CIStraightenFilter (IntPtr handle) : base (handle) {}
	
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
		public CIStripesGenerator (IntPtr handle) : base (handle) {}
	
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
		public CITemperatureAndTint (IntPtr handle) : base (handle) {}
	
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
		public CIToneCurve (IntPtr handle) : base (handle) {}
	
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
		public CIVibrance (IntPtr handle) : base (handle) {}
	
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
		public CIVignette (IntPtr handle) : base (handle) {}
	
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
		public CIWhitePointAdjust (IntPtr handle) : base (handle) {}
	
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