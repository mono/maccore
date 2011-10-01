//
// coreimage.cs: Definitions for CoreImage
//
// Copyright 2010, Novell, Inc.
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

//
//
// TODO: CIFilter, eliminate the use of the NSDictionary and use
// strongly typed accessors.
//
// TODO:
// CIImageProvider     - informal protocol, 
// CIPluginInterface   - informal protocol
// CIRAWFilter         - informal protocol
// CIVector
//
using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using MonoMac.CoreGraphics;
using MonoMac.CoreImage;
using MonoMac.CoreVideo;
#if !MONOMAC
using MonoTouch.OpenGLES;
using MonoTouch.UIKit;
#endif

namespace MonoMac.CoreImage {

	[BaseType (typeof (NSObject))]
	public interface CIColor {
		[Static]
		[Export ("colorWithCGColor:")]
		CIColor FromCGColor (CGColor c);

		[Static]
		[Export ("colorWithRed:green:blue:alpha:")]
		CIColor FromRgba (float r, float g, float b, float a);

		[Static]
		[Export ("colorWithRed:green:blue:")]
		CIColor FromRgb (float r, float g, float b);

		[Static]
		[Export ("colorWithString:")]
		CIColor FromString (string representation);

		[Export ("initWithCGColor:")]
		IntPtr Constructor (CGColor c);

		[Export ("numberOfComponents")]
		int NumberOfComponents { get; }

		// FIXME: bdining
		//[Export ("components")]
		//const CGFloat Components ();

		[Export ("alpha")]
		float Alpha { get; }

		[Export ("colorSpace")]
		CGColorSpace ColorSpace { get; }

		[Export ("red")]
		float Red { get; }

		[Export ("green")]
		float Green { get; }

		[Export ("blue")]
		float Blue { get; }

		[Export ("stringRepresentation")]
		string StringRepresentation ();

#if !MONOMAC
		[Export ("initWithColor:")]
		IntPtr Constructor (UIColor color);
#endif
	}

        [BaseType (typeof (NSObject))]
	public interface CIContext {
		// When we bind OpenGL add these:
		//[Export ("contextWithCGLContext:pixelFormat:colorSpace:options:")]
		//CIContext ContextWithCGLContextpixelFormatcolorSpaceoptions (CGLContextObj ctx, CGLPixelFormatObj pf, CGColorSpaceRef cs, NSDictionary dict, );

		[Internal, Static]
		[Export ("contextWithCGContext:options:")]
		CIContext FromContext (CGContext ctx, [NullAllowed] NSDictionary options);

#if !MONOMAC
		[Static]
		[Export ("contextWithEAGLContext:")]
		CIContext FromContext (EAGLContext eaglContext);

		[Static, Internal]
		[Export ("contextWithOptions:")]
		CIContext FromOptions (NSDictionary dictionary);

		[Export ("render:toCVPixelBuffer:")]
		void Render (CIImage image, CVPixelBuffer buffer);

		[Export ("render:toCVPixelBuffer:bounds:colorSpace:")]
		void Render (CIImage image, CVPixelBuffer buffer, RectangleF rectangle, CGColorSpace cs);

		[Export ("inputImageMaximumSize")]
		SizeF InputImageMaximumSize { get; }

		[Export ("outputImageMaximumSize")]
		SizeF OutputImageMaximumSize { get; }
#endif

		[Export ("drawImage:atPoint:fromRect:")]
		void DrawImage (CIImage image, PointF atPoint, RectangleF fromRect);

		[Export ("drawImage:inRect:fromRect:")]
		void DrawImage (CIImage image, RectangleF inRectangle, RectangleF fromRectangle);

		[Export ("createCGImage:fromRect:")]
		CGImage CreateCGImage (CIImage image, RectangleF fromRectangle);

		[Export ("createCGImage:fromRect:format:colorSpace:")]
		CGImage CreateCGImage (CIImage image, RectangleF fromRect, int ciImageFormat, CGColorSpace colorSpace);

		[Internal, Export ("createCGLayerWithSize:info:")]
		CGLayer CreateCGLayer (SizeF size, [NullAllowed] NSDictionary info);

		[Export ("render:toBitmap:rowBytes:bounds:format:colorSpace:")]
		void RenderToBitmap (CIImage image, IntPtr bitmapPtr, int bytesPerRow, RectangleF bounds, int bitmapFormat, CGColorSpace colorSpace);

		//[Export ("render:toIOSurface:bounds:colorSpace:")]
		//void RendertoIOSurfaceboundscolorSpace (CIImage im, IOSurfaceRef surface, RectangleF r, CGColorSpaceRef cs, );

#if MONOMAC
		[Export ("reclaimResources")]
		void ReclaimResources ();

		[Export ("clearCaches")]
		void ClearCaches ();
#endif

		[Internal, Field ("kCIContextOutputColorSpace", "+CoreImage")]
		NSString OutputColorSpace { get; }

		[Internal, Field ("kCIContextWorkingColorSpace", "+CoreImage")]
		NSString WorkingColorSpace { get; }
		
		[Internal, Field ("kCIContextUseSoftwareRenderer", "+CoreImage")]
		NSString UseSoftwareRenderer { get; }
	}

	[BaseType (typeof (NSObject))]
	public interface CIFilter {
		[Export ("inputKeys")]
		string [] InputKeys { get; }

		[Export ("outputKeys")]
		string [] OutputKeys { get; }

		[Export ("setDefaults")]
		void SetDefaults ();

		[Export ("attributes")]
		NSDictionary Attributes { get; }

		[Export ("name")]
		string Name { get; set;}

		[Static]
		[Export ("filterWithName:")]
		CIFilter FromName (string name);

		[Static]
		[Export ("filterNamesInCategory:")]
		string [] FilterNamesInCategory (string category);

		[Static]
		[Export ("filterNamesInCategories:"), Internal]
		string [] _FilterNamesInCategories (string [] categories);

#if MONOMAC
		[Export ("apply:arguments:options:")]
		CIImage Apply (CIKernel k, NSArray args, NSDictionary options);

		[Static]
		[Export ("registerFilterName:constructor:classAttributes:")]
		void RegisterFilterName (string name, NSObject constructorObject, NSDictionary classAttributes);

		[Static]
		[Export ("localizedNameForFilterName:")]
		string FilterLocalizedName (string filterName);

		[Static]
		[Export ("localizedNameForCategory:")]
		string CategoryLocalizedName (string category);

		[Static]
		[Export ("localizedDescriptionForFilterName:")]
		string FilterLocalizedDescription (string filterName);

		[Static]
		[Export ("localizedReferenceDocumentationForFilterName:")]
		NSUrl FilterLocalizedReferenceDocumentation (string filterName);
#else
		[Export ("outputImage")]
		CIImage OutputImage { get; }
#endif

		[Field ("kCIAttributeFilterName", "+CoreImage")]
		NSString AttributeFilterName  { get; }

		[Field ("kCIAttributeFilterDisplayName", "+CoreImage")]
		NSString AttributeFilterDisplayName  { get; }

#if MONOMAC
		[Field ("kCIAttributeDescription", "+CoreImage")]
		NSString AttributeDescription  { get; }

		[Field ("kCIAttributeReferenceDocumentation", "+CoreImage")]
		NSString AttributeReferenceDocumentation  { get; }
#endif

		[Field ("kCIAttributeFilterCategories", "+CoreImage")]
		NSString AttributeFilterCategories  { get; }

		[Field ("kCIAttributeClass", "+CoreImage")]
		NSString AttributeClass  { get; }

		[Field ("kCIAttributeType", "+CoreImage")]
		NSString AttributeType  { get; }

		[Field ("kCIAttributeMin", "+CoreImage")]
		NSString AttributeMin  { get; }

		[Field ("kCIAttributeMax", "+CoreImage")]
		NSString AttributeMax  { get; }

		[Field ("kCIAttributeSliderMin", "+CoreImage")]
		NSString AttributeSliderMin  { get; }

		[Field ("kCIAttributeSliderMax", "+CoreImage")]
		NSString AttributeSliderMax  { get; }

		[Field ("kCIAttributeDefault", "+CoreImage")]
		NSString AttributeDefault  { get; }

		[Field ("kCIAttributeIdentity", "+CoreImage")]
		NSString AttributeIdentity  { get; }

		[Field ("kCIAttributeName", "+CoreImage")]
		NSString AttributeName  { get; }

		[Field ("kCIAttributeDisplayName", "+CoreImage")]
		NSString AttributeDisplayName  { get; }

#if MONOMAC
		[Field ("kCIUIParameterSet", "+CoreImage")]
		NSString UIParameterSet  { get; }

		[Field ("kCIUISetBasic", "+CoreImage")]
		NSString UISetBasic  { get; }

		[Field ("kCIUISetIntermediate", "+CoreImage")]
		NSString UISetIntermediate  { get; }

		[Field ("kCIUISetAdvanced", "+CoreImage")]
		NSString UISetAdvanced  { get; }

		[Field ("kCIUISetDevelopment", "+CoreImage")]
		NSString UISetDevelopment  { get; }
#endif

		[Field ("kCIAttributeTypeTime", "+CoreImage")]
		NSString AttributeTypeTime  { get; }

		[Field ("kCIAttributeTypeScalar", "+CoreImage")]
		NSString AttributeTypeScalar  { get; }

		[Field ("kCIAttributeTypeDistance", "+CoreImage")]
		NSString AttributeTypeDistance  { get; }

		[Field ("kCIAttributeTypeAngle", "+CoreImage")]
		NSString AttributeTypeAngle  { get; }

		[Field ("kCIAttributeTypeBoolean", "+CoreImage")]
		NSString AttributeTypeBoolean  { get; }

		[Field ("kCIAttributeTypeInteger", "+CoreImage")]
		NSString AttributeTypeInteger  { get; }

		[Field ("kCIAttributeTypeCount", "+CoreImage")]
		NSString AttributeTypeCount  { get; }

		[Field ("kCIAttributeTypePosition", "+CoreImage")]
		NSString AttributeTypePosition  { get; }

		[Field ("kCIAttributeTypeOffset", "+CoreImage")]
		NSString AttributeTypeOffset  { get; }

		[Field ("kCIAttributeTypePosition3", "+CoreImage")]
		NSString AttributeTypePosition3  { get; }

		[Field ("kCIAttributeTypeRectangle", "+CoreImage")]
		NSString AttributeTypeRectangle  { get; }

		[Field ("kCIAttributeTypeOpaqueColor", "+CoreImage")]
		NSString AttributeTypeOpaqueColor  { get; }

#if MONOMAC
		[Field ("kCIAttributeTypeGradient", "+CoreImage")]
		NSString AttributeTypeGradient  { get; }
#else
		[Field ("kCIAttributeTypeImage", "+CoreImage")]
		NSString AttributeTypeImage  { get; }

		[Field ("kCIAttributeTypeTransform", "+CoreImage")]
		NSString AttributeTypeTransform  { get; }
#endif
		[Field ("kCICategoryDistortionEffect", "+CoreImage")]
		NSString CategoryDistortionEffect  { get; }

		[Field ("kCICategoryGeometryAdjustment", "+CoreImage")]
		NSString CategoryGeometryAdjustment  { get; }

		[Field ("kCICategoryCompositeOperation", "+CoreImage")]
		NSString CategoryCompositeOperation  { get; }

		[Field ("kCICategoryHalftoneEffect", "+CoreImage")]
		NSString CategoryHalftoneEffect  { get; }

		[Field ("kCICategoryColorAdjustment", "+CoreImage")]
		NSString CategoryColorAdjustment  { get; }

		[Field ("kCICategoryColorEffect", "+CoreImage")]
		NSString CategoryColorEffect  { get; }

		[Field ("kCICategoryTransition", "+CoreImage")]
		NSString CategoryTransition  { get; }

		[Field ("kCICategoryTileEffect", "+CoreImage")]
		NSString CategoryTileEffect  { get; }

		[Field ("kCICategoryGenerator", "+CoreImage")]
		NSString CategoryGenerator  { get; }

		[Field ("kCICategoryReduction", "+CoreImage")]
		NSString CategoryReduction  { get; }

		[Field ("kCICategoryGradient", "+CoreImage")]
		NSString CategoryGradient  { get; }

		[Field ("kCICategoryStylize", "+CoreImage")]
		NSString CategoryStylize  { get; }

		[Field ("kCICategorySharpen", "+CoreImage")]
		NSString CategorySharpen  { get; }

		[Field ("kCICategoryBlur", "+CoreImage")]
		NSString CategoryBlur  { get; }

		[Field ("kCICategoryVideo", "+CoreImage")]
		NSString CategoryVideo  { get; }

		[Field ("kCICategoryStillImage", "+CoreImage")]
		NSString CategoryStillImage  { get; }

		[Field ("kCICategoryInterlaced", "+CoreImage")]
		NSString CategoryInterlaced  { get; }

		[Field ("kCICategoryNonSquarePixels", "+CoreImage")]
		NSString CategoryNonSquarePixels  { get; }

		[Field ("kCICategoryHighDynamicRange", "+CoreImage")]
		NSString CategoryHighDynamicRange  { get; }

		[Field ("kCICategoryBuiltIn", "+CoreImage")]
		NSString CategoryBuiltIn  { get; }

#if MONOMAC
		[Field ("kCICategoryFilterGenerator", "+CoreImage")]
		NSString CategoryFilterGenerator  { get; }

		[Field ("kCIApplyOptionExtent", "+CoreImage")]
		NSString ApplyOptionExtent  { get; }

		[Field ("kCIApplyOptionDefinition", "+CoreImage")]
		NSString ApplyOptionDefinition  { get; }

		[Field ("kCIApplyOptionUserInfo", "+CoreImage")]
		NSString ApplyOptionUserInfo  { get; }

		[Field ("kCIApplyOptionColorSpace", "+CoreImage")]
		NSString ApplyOptionColorSpace  { get; }
#endif
		[Field ("kCIOutputImageKey", "+CoreImage")]
		NSString OutputImageKey  { get; }

		[Field ("kCIInputBackgroundImageKey", "+CoreImage")]
		NSString InputBackgroundImageKey  { get; }

		[Field ("kCIInputImageKey", "+CoreImage")]
		NSString InputImageKey  { get; }

#if MONOMAC
		[Field ("kCIInputTimeKey", "+CoreImage")]
		NSString InputTimeKey  { get; }

		[Field ("kCIInputTransformKey", "+CoreImage")]
		NSString InputTransformKey  { get; }

		[Field ("kCIInputScaleKey", "+CoreImage")]
		NSString InputScaleKey  { get; }

		[Field ("kCIInputAspectRatioKey", "+CoreImage")]
		NSString InputAspectRatioKey  { get; }

		[Field ("kCIInputCenterKey", "+CoreImage")]
		NSString InputCenterKey  { get; }

		[Field ("kCIInputRadiusKey", "+CoreImage")]
		NSString InputRadiusKey  { get; }

		[Field ("kCIInputAngleKey", "+CoreImage")]
		NSString InputAngleKey  { get; }

		[Field ("kCIInputRefractionKey", "+CoreImage")]
		NSString InputRefractionKey  { get; }

		[Field ("kCIInputWidthKey", "+CoreImage")]
		NSString InputWidthKey  { get; }

		[Field ("kCIInputSharpnessKey", "+CoreImage")]
		NSString InputSharpnessKey  { get; }

		[Field ("kCIInputIntensityKey", "+CoreImage")]
		NSString InputIntensityKey  { get; }

		[Field ("kCIInputEVKey", "+CoreImage")]
		NSString InputEVKey  { get; }

		[Field ("kCIInputSaturationKey", "+CoreImage")]
		NSString InputSaturationKey  { get; }

		[Field ("kCIInputColorKey", "+CoreImage")]
		NSString InputColorKey  { get; }

		[Field ("kCIInputBrightnessKey", "+CoreImage")]
		NSString InputBrightnessKey  { get; }

		[Field ("kCIInputContrastKey", "+CoreImage")]
		NSString InputContrastKey  { get; }

		[Field ("kCIInputGradientImageKey", "+CoreImage")]
		NSString InputGradientImageKey  { get; }

		[Field ("kCIInputMaskImageKey", "+CoreImage")]
		NSString InputMaskImageKey  { get; }

		[Field ("kCIInputShadingImageKey", "+CoreImage")]
		NSString InputShadingImageKey  { get; }

		[Field ("kCIInputTargetImageKey", "+CoreImage")]
		NSString InputTargetImageKey  { get; }

		[Field ("kCIInputExtentKey", "+CoreImage")]
		NSString InputExtentKey  { get; }
#endif
	}

#if MONOMAC
	[BaseType (typeof (NSObject))]
	public interface CIFilterGenerator {
		[Static, Export ("filterGenerator")]
		CIFilterGenerator Create ();

		[Static]
		[Export ("filterGeneratorWithContentsOfURL:")]
		CIFilterGenerator FromUrl (NSUrl aURL);

		[Export ("initWithContentsOfURL:")]
		IntPtr Constructor (NSUrl aURL);

		[Export ("connectObject:withKey:toObject:withKey:")]
		void ConnectObject (NSObject sourceObject, string withSourceKey, NSObject targetObject, string targetKey);

		[Export ("disconnectObject:withKey:toObject:withKey:")]
		void DisconnectObject (NSObject sourceObject, string sourceKey, NSObject targetObject, string targetKey);

		[Export ("exportKey:fromObject:withName:")]
		void ExportKey (string key, NSObject targetObject, string exportedKeyName);

		[Export ("removeExportedKey:")]
		void RemoveExportedKey (string exportedKeyName);

		[Export ("exportedKeys")]
		NSDictionary ExportedKeys { get; }

		[Export ("setAttributes:forExportedKey:")]
		void SetAttributesforExportedKey (NSDictionary attributes, NSString exportedKey);

		[Export ("filter")]
		CIFilter CreateFilter ();

		[Export ("registerFilterName:")]
		void RegisterFilterName (string name);

		[Export ("writeToURL:atomically:")]
		bool Save (NSUrl toUrl, bool atomically);

		//Detected properties
		[Export ("classAttributes")]
		NSDictionary ClassAttributes { get; set; }

		[Field ("kCIFilterGeneratorExportedKey", "+CoreImage")]
		NSString ExportedKey { get; }

		[Field ("kCIFilterGeneratorExportedKeyTargetObject", "+CoreImage")]
		NSString ExportedKeyTargetObject { get; }

		[Field ("kCIFilterGeneratorExportedKeyName", "+CoreImage")]
		NSString ExportedKeyName { get; }
	}

	[BaseType (typeof (NSObject))]
	public interface CIFilterShape {
		[Export ("shapeWithRect:")]
		CIFilterShape FromRect (RectangleF rect);

		[Export ("initWithRect:")]
		IntPtr Constructor (RectangleF rect);

		[Export ("transformBy:interior:")]
		CIFilterShape Transform (CGAffineTransform transformation, bool interiorFlag);

		[Export ("insetByX:Y:")]
		CIFilterShape Inset (int dx, int dy);

		[Export ("unionWith:")]
		CIFilterShape Union (CIFilterShape other);

		[Export ("unionWithRect:")]
		CIFilterShape Union (RectangleF rectangle);

		[Export ("intersectWith:")]
		CIFilterShape Intersect (CIFilterShape other);

		[Export ("intersectWithRect:")]
		CIFilterShape IntersectWithRect (Rectangle rectangle);
	}
#endif
	
	[BaseType (typeof (NSObject))]
	public interface CIImage {
		[Static]
		[Export ("imageWithCGImage:")]
		CIImage FromCGImage (CGImage image);

		[Static]
		[Export ("imageWithCGImage:options:")]
		CIImage FromCGImage (CGImage image, NSDictionary d);

#if MONOMAC
		[Static]
		[Export ("imageWithCGLayer:")]
		CIImage FromLayer (CGLayer layer);

		[Static]
		[Export ("imageWithCGLayer:options:")]
		CIImage FromLayer (CGLayer layer, NSDictionary options);
#endif

		[Static]
		[Export ("imageWithBitmapData:bytesPerRow:size:format:colorSpace:")]
		CIImage FromData (NSData bitmapData, int bpr, SizeF size, int ciImageFormat, CGColorSpace colorspace);

#if MONOMAC
		[Static]
		[Export ("imageWithTexture:size:flipped:colorSpace:")]
		CIImage ImageWithTexturesizeflippedcolorSpace (int glTextureName, SizeF size, bool flag, CGColorSpace colorspace);
#endif

		[Static]
		[Export ("imageWithContentsOfURL:")]
		CIImage FromUrl (NSUrl url);

		[Static]
		[Export ("imageWithContentsOfURL:options:")]
		CIImage FromUrl (NSUrl url, NSDictionary d);

		[Static]
		[Export ("imageWithData:")]
		CIImage FromData (NSData data);

		[Static]
		[Export ("imageWithData:options:")]
		CIImage FromData (NSData data, NSDictionary d);

		[Static]
		[Export ("imageWithCVImageBuffer:")]
		CIImage FromImageBuffer (CVImageBuffer imageBuffer);

		//
		[Static]
		[Export ("imageWithCVImageBuffer:options:")]
		CIImage FromImageBuffer (CVImageBuffer imageBuffer, NSDictionary dict);

		//[Export ("imageWithIOSurface:")]
		//CIImage ImageWithIOSurface (IOSurfaceRef surface, );
		//
		//[Static]
		//[Export ("imageWithIOSurface:options:")]
		//CIImage ImageWithIOSurfaceoptions (IOSurfaceRef surface, NSDictionary d, );

		[Export ("imageWithColor:")]
		CIImage ImageWithColor (CIColor color);

		[Static]
		[Export ("emptyImage")]
		CIImage EmptyImage { get; }

		[Export ("initWithCGImage:")]
		IntPtr Constructor (CGImage image);

		[Export ("initWithCGImage:options:")]
		IntPtr Constructor (CGImage image, NSDictionary d);

		// FIXME: bindingneeded
		[Export ("initWithCGLayer:")]
		IntPtr Constructor (CGLayer layer);
		
		[Export ("initWithCGLayer:options:")]
		NSObject IntPtr (CGLayer layer, NSDictionary d);

		[Export ("initWithData:")]
		IntPtr Constructor (NSData data);

		[Export ("initWithData:options:")]
		IntPtr Constructor (NSData data, NSDictionary d);

		[Export ("initWithBitmapData:bytesPerRow:size:format:colorSpace:")]
		IntPtr Constructor (NSData d, int bpr, SizeF size, int f, CGColorSpace c);

		[Export ("initWithTexture:size:flipped:colorSpace:")]
		IntPtr Constructor (int glTextureName, SizeF size, bool flag, CGColorSpace cs);

		[Export ("initWithContentsOfURL:")]
		IntPtr Constructor (NSUrl url);

		[Export ("initWithContentsOfURL:options:")]
		IntPtr Constructor (NSUrl url, NSDictionary d);

		// FIXME: bindings
		//[Export ("initWithIOSurface:")]
		//NSObject InitWithIOSurface (IOSurfaceRef surface, );
		//
		//[Export ("initWithIOSurface:options:")]
		//NSObject InitWithIOSurfaceoptions (IOSurfaceRef surface, NSDictionary d, );
		//
		[Export ("initWithCVImageBuffer:")]
		IntPtr Constructor (CVImageBuffer imageBuffer);

		[Export ("initWithCVImageBuffer:options:")]
		IntPtr Constructor (CVImageBuffer imageBuffer, NSDictionary dict);

		[Export ("initWithColor:")]
		IntPtr Constructor (CIColor color);

		[Export ("imageByApplyingTransform:")]
		CIImage ImageByApplyingTransform (CGAffineTransform matrix);

		[Export ("imageByCroppingToRect:")]
		CIImage ImageByCroppingToRect (RectangleF r);

		[Export ("extent")]
		RectangleF Extent { get; }

		//[Export ("definition")]
		//CIFilterShape Definition ();

		[Export ("url")]
		NSUrl Url { get; }

		[Export ("colorSpace")]
		CGColorSpace ColorSpace { get; }

#if MONOMAC
		[Field ("kCIFormatARGB8")]
		int FormatARGB8 { get; }

		[Field ("kCIFormatRGBA16")]
		int FormatRGBA16 { get; }

		[Field ("kCIFormatRGBAf")]
		int FormatRGBAf { get; }

		[Field ("kCIFormatRGBAh")]
		int FormatRGBAh { get; }
#else
		[Field ("kCIFormatBGRA8")]
		int FormatBGRA8 { get; }

		[Field ("kCIFormatRGBA8")]
		int FormatRGBA8 { get; }

		// UIKit extensions
		[Export ("initWithImage:")]
		IntPtr Constructor (UIImage image);

		[Export ("initWithImage:options")]
		IntPtr Constructor (UIImage image, NSDictionary options);
#endif
		
	}

#if MONOMAC
	[BaseType (typeof (NSObject))]
	public interface CIImageAccumulator {
		[Export ("imageAccumulatorWithExtent:format:")]
		CIImageAccumulator FromRectangle (RectangleF rect, int ciImageFormat);

		[Export ("initWithExtent:format:")]
		IntPtr Constructor (RectangleF rectangle, int ciImageFormat);

		[Export ("extent")]
		RectangleF Extent { get; }

		[Export ("format")]
		int CIImageFormat { get; }

		[Export ("setImage:dirtyRect:")]
		void SetImageDirty (CIImage image, RectangleF dirtyRect);

		[Export ("clear")]
		void Clear ();

		//Detected properties
		[Export ("image")]
		CIImage Image { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public interface CIKernel {
		[Static, Export ("kernelsWithString:")]
		CIKernel [] FromProgram (string coreImageShaderProgram);

		[Export ("name")]
		string Name { get; }

		[Export ("setROISelector:")]
		void SetRegionOfInterestSelector (Selector aMethod);
	}

	[BaseType (typeof (NSObject))]
	public interface CIPlugIn {
		[Static]
		[Export ("loadAllPlugIns")]
		void LoadAllPlugIns ();

		[Static]
		[Export ("loadNonExecutablePlugIns")]
		void LoadNonExecutablePlugIns ();

		[Static]
		[Export ("loadPlugIn:allowNonExecutable:")]
		void LoadPlugIn (NSUrl pluginUrl, bool allowNonExecutable);
	}

	[BaseType (typeof (NSObject))]
	public interface CISampler {
		[Static, Export ("samplerWithImage:")]
		CISampler FromImage (CIImage sourceImage);

		[Internal, Static]
		[Export ("samplerWithImage:options:")]
		CISampler FromImage (CIImage sourceImag, NSDictionary options);

		[Export ("initWithImage:")]
		IntPtr Constructor (CIImage sourceImage);

		[Internal, Export ("initWithImage:options:")]
		NSObject Constructor (CIImage image, NSDictionary options);

		[Export ("definition")]
		CIFilterShape Definition { get; }

		[Export ("extent")]
		RectangleF Extent { get; }

		[Field ("kCISamplerAffineMatrix", "+CoreImage"), Internal]
		NSString AffineMatrix { get; }
		[Field ("kCISamplerWrapMode", "+CoreImage"), Internal]
		NSString WrapMode { get; }
		[Field ("kCISamplerFilterMode", "+CoreImage"), Internal]
		NSString FilterMode { get; }

		[Field ("kCISamplerWrapBlack", "+CoreImage"), Internal]
		NSString WrapBlack { get; }
		[Field ("kCISamplerWrapClamp", "+CoreImage"), Internal]
		NSString WrapClamp { get; }
		
		[Field ("kCISamplerFilterNearest", "+CoreImage"), Internal]
		NSString FilterNearest { get; }

		[Field ("kCISamplerFilterLinear", "+CoreImage"), Internal]
		NSString FilterLinear { get; }
	}
#endif
	
	[BaseType (typeof (NSObject))]
	interface CIVector {
		[Static, Internal, Export ("vectorWithValues:count:")]
		CIVector _FromValues (IntPtr values, int count);

		[Static]
		[Export ("vectorWithX:")]
		CIVector Create (float x);

		[Static]
		[Export ("vectorWithX:Y:")]
		CIVector Create (float x, float y);

		[Static]
		[Export ("vectorWithX:Y:Z:")]
		CIVector Create (float x, float y, float z);

		[Static]
		[Export ("vectorWithX:Y:Z:W:")]
		CIVector Create (float x, float y, float z, float w);

		[Static]
		[Export ("vectorWithString:")]
		CIVector FromString (string representation);

		[Internal, Export ("initWithValues:count:")]
		IntPtr Constructor (IntPtr values, int count);

		[Export ("initWithX:")]
		IntPtr Constructor(float x);

		[Export ("initWithX:Y:")]
		IntPtr Constructor (float x, float y);

		[Export ("initWithX:Y:Z:")]
		IntPtr Constructor (float x, float y, float z);

		[Export ("initWithX:Y:Z:W:")]
		IntPtr Constructor (float x, float y, float z, float w);

		[Export ("initWithString:")]
		IntPtr Constructor (string representation);

		[Export ("valueAtIndex:"), Internal]
		float ValueAtIndex (int index);

		[Export ("count")]
		int Count { get; }

		[Export ("X")]
		float X { get; }

		[Export ("Y")]
		float Y { get; }

		[Export ("Z")]
		float Z { get; }

		[Export ("W")]
		float W { get; }

		[Export ("stringRepresentation"), Internal]
		string StringRepresentation ();

	}

#if !MONOMAC
	[BaseType (typeof (NSObject))]
	interface CIDetector {
		[Static, Export ("detectorOfType:context:options:"), Internal]
		CIDetector FromType (NSString detectorType, CIContext context, [NullAllowed] NSDictionary options);

		[Export ("featuresInImage:")]
		CIFeature [] GetFeatures (CIImage image);

		[Export ("featuresInImage:options:")]
		CIFeature [] FeaturesInImage (CIImage image, NSDictionary options);

		[Field ("CIDetectorTypeFace"), Internal]
		NSString TypeFace { get; }
		
		[Field ("CIDetectorImageOrientation"), Internal]
		NSString ImageOrientation { get; }

		[Field ("CIDetectorAccuracy"), Internal]
		NSString Accuracy { get; }

		[Field ("CIDetectorAccuracyLow"), Internal]
		NSString AccuracyLow { get; }

		[Field ("CIDetectorAccuracyHigh"), Internal]
		NSString AccuracyHigh { get; }
	}
	
	[BaseType (typeof (NSObject))]
	interface CIFeature {
		[Export ("type")]
		NSString Type { get; }

		[Export ("bounds")]
		RectangleF Bounds { get; }
	}

	[BaseType (typeof (CIFeature))]
	interface CIFaceFeature {
		[Export ("hasLeftEyePosition")]
		bool HasLeftEyePosition { get; }
		
		[Export ("leftEyePosition")]
		PointF LeftEyePosition { get; }
		
		[Export ("hasRightEyePosition")]
		bool HasRightEyePosition { get; }
		
		[Export ("rightEyePosition")]
		PointF RightEyePosition { get; }
		
		[Export ("hasMouthPosition")]
		bool HasMouthPosition { get; }
		
		[Export ("mouthPosition")]
		PointF MouthPosition { get; }
	}
#endif
}
