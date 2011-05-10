// 
// CTFont.cs: Implements the managed CTFont
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
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

using MonoMac.ObjCRuntime;
using MonoMac.CoreFoundation;
using MonoMac.CoreGraphics;
using MonoMac.Foundation;

using CGGlyph = System.UInt16;

namespace MonoMac.CoreText {

	[Since (3,2)]
	[Flags]
	public enum CTFontOptions {
		Default = 0,
		PreventAutoActivation = 1 << 0,
		PreferSystemFont      = 1 << 2,
		IncludeDisabled       = 1 << 7,
	}

	[Since (3,2)]
	public enum CTFontUIFontType : uint {
		None                         = unchecked ((uint)(-1)),
		User                         =  0,
		UserFixedPitch               =  1,
		System                       =  2,
		EmphasizedSystem             =  3,
		SmallSystem                  =  4,
		SmallEmphasizedSystem        =  5,
		MiniSystem                   =  6,
		MiniEmphasizedSystem         =  7,
		Views                        =  8,
		Application                  =  9,
		Label                        = 10,
		MenuTitle                    = 11,
		MenuItem                     = 12,
		MenuItemMark                 = 13,
		MenuItemCmdKey               = 14,
		WindowTitle                  = 15,
		PushButton                   = 16,
		UtilityWindowTitle           = 17,
		AlertHeader                  = 18,
		SystemDetail                 = 19,
		EmphasizedSystemDetail       = 20,
		Toolbar                      = 21,
		SmallToolbar                 = 22,
		Message                      = 23,
		Palette                      = 24,
		ToolTip                      = 25,
		ControlContent               = 26,
	}

	[Since (3,2)]
	public enum CTFontTable : uint {
		BaselineBASE               = 0x42415345,  // 'BASE'
		PostscriptFontProgram      = 0x43464620,  // 'CFF '
		DigitalSignature           = 0x44534947,  // 'DSIG'
		EmbeddedBitmap             = 0x45424454,  // 'EBDT'
		EmbeddedBitmapLocation     = 0x45424c43,  // 'EBLC'
		EmbeddedBitmapScaling      = 0x45425343,  // 'EBSC'
		GlyphDefinition            = 0x47444546,  // 'GDEF'
		GlyphPositioning           = 0x47504f53,  // 'GPOS'
		GlyphSubstitution          = 0x47535542,  // 'GSUB'
		JustificationJSTF          = 0x4a535446,  // 'JSTF'
		LinearThreshold            = 0x4c545348,  // 'LTSH'
		WindowsSpecificMetrics     = 0x4f532f32,  // 'OS2 '
		Pcl5Data                   = 0x50434c54,  // 'PCLT'
		VerticalDeviceMetrics      = 0x56444d58,  // 'VDMX'
		VerticalOrigin             = 0x564f5247,  // 'VORG'
		GlyphReference             = 0x5a617066,  // 'Zapf'
		AccentAttachment           = 0x61636e74,  // 'Acnt'
		AxisVariation              = 0x61766172,  // 'Avar'
		BitmapData                 = 0x62646174,  // 'Bdat'
		BitmapFontHeader           = 0x62686564,  // 'Bhed'
		BitmapLocation             = 0x626c6f63,  // 'Bloc'
		BaselineBsln               = 0x62736c6e,  // 'Bsln'
		CharacterToGlyphMapping    = 0x636d6170,  // 'Cmap'
		ControlValueTableVariation = 0x63766172,  // 'Cvar'
		ControlValueTable          = 0x63767420,  // 'Cvt '
		FontDescriptor             = 0x66647363,  // 'Fdsc'
		LayoutFeature              = 0x66656174,  // 'Feat'
		FontMetrics                = 0x666d7478,  // 'Fmtx'
		FontProgram                = 0x6670676d,  // 'Fpgm'
		FontVariation              = 0x66766172,  // 'Fvar'
		GridFitting                = 0x67617370,  // 'Gasp'
		GlyphData                  = 0x676c7966,  // 'Glyf'
		GlyphVariation             = 0x67766172,  // 'Gvar'
		HorizontalDeviceMetrics    = 0x68646d78,  // 'Hdmx'
		FontHeader                 = 0x68656164,  // 'Head'
		HorizontalHeader           = 0x68686561,  // 'Hhea'
		HorizontalMetrics          = 0x686d7478,  // 'Hmtx'
		HorizontalStyle            = 0x68737479,  // 'Hsty'
		JustificationJust          = 0x6a757374,  // 'Just'
		Kerning                    = 0x6b65726e,  // 'Kern'
	        ExtendedKerning            = 0x6b657278,  // 'Kerx'
		LigatureCaret              = 0x6c636172,  // 'Lcar'
		IndexToLocation            = 0x6c6f6361,  // 'Loca'
		MaximumProfile             = 0x6d617870,  // 'Maxp'
		Morph                      = 0x6d6f7274,  // 'Mort'
		ExtendedMorph              = 0x6d6f7278,  // 'Morx'
		Name                       = 0x6e616d65,  // 'Name'
		OpticalBounds              = 0x6f706264,  // 'Opbd'
		PostScriptInformation      = 0x706f7374,  // 'Post'
		ControlValueTableProgram   = 0x70726570,  // 'Prep'
		Properties                 = 0x70726f70,  // 'Prop'
		Tracking                   = 0x7472616b,  // 'Trak'
		VerticalHeader             = 0x76686561,  // 'Vhea'
		VerticalMetrics            = 0x766d7478,  // 'Vmtx'
		SBitmapData 		   = 0x73626974, // 'sbit'
		SExtendedBitmapData        = 0x73626978, // 'sbix'
	}

	[Since (3,2)]
	[Flags]
	public enum CTFontTableOptions : uint {
		None              = 0,
		ExcludeSynthetic  = (1 << 0),
	}

	[Since (3,2)]
	public class CTFontFeatureKey {

		public static readonly NSString Identifier;
		public static readonly NSString Name;
		public static readonly NSString Exclusive;
		public static readonly NSString Selectors;

		static CTFontFeatureKey ()
		{
			var handle = Dlfcn.dlopen (Constants.CoreTextLibrary, 0);
			if (handle == IntPtr.Zero)
				return;
			try {
				Identifier  = Dlfcn.GetStringConstant (handle, "kCTFontFeatureTypeIdentifierKey");
				Name        = Dlfcn.GetStringConstant (handle, "kCTFontFeatureTypeNameKey");
				Exclusive   = Dlfcn.GetStringConstant (handle, "kCTFontFeatureTypeExclusiveKey");
				Selectors   = Dlfcn.GetStringConstant (handle, "kCTFontFeatureTypeSelectorsKey");
			}
			finally {
				Dlfcn.dlclose (handle);
			}
		}
	}

	[Since (3,2)]
	public class CTFontFeatures {

		public CTFontFeatures ()
			: this (new NSMutableDictionary ())
		{
		}

		public CTFontFeatures (NSDictionary dictionary)
		{
			if (dictionary == null)
				throw new ArgumentNullException ("dictionary");
			Dictionary = dictionary;
		}

		public NSDictionary Dictionary {get; private set;}

		// TODO: what kind of NSNumber?
		public NSNumber Identifier {
			get {return (NSNumber) Dictionary [CTFontFeatureKey.Identifier];}
			set {Adapter.SetValue (Dictionary, CTFontFeatureKey.Identifier, value);}
		}

		public string Name {
			get {return Adapter.GetStringValue (Dictionary, CTFontFeatureKey.Name);}
			set {Adapter.SetValue (Dictionary, CTFontFeatureKey.Name, value);}
		}

		public bool Exclusive {
			get {
				return CFDictionary.GetBooleanValue (Dictionary.Handle, 
						CTFontFeatureKey.Exclusive.Handle);
			}
			set {
				CFMutableDictionary.SetValue (Dictionary.Handle, 
						CTFontFeatureKey.Exclusive.Handle, 
						value);
			}
		}

		public IEnumerable<CTFontFeatureSelectors> Selectors {
			get {
				return Adapter.GetNativeArray (Dictionary, CTFontFeatureKey.Selectors,
						d => new CTFontFeatureSelectors ((NSDictionary) Runtime.GetNSObject (d)));
			}
			set {
				List<CTFontFeatureSelectors> v;
				if (value == null || (v = new List<CTFontFeatureSelectors> (value)).Count == 0) {
					Adapter.SetValue (Dictionary, CTFontFeatureKey.Selectors, (NSObject) null);
					return;
				}
				Adapter.SetValue (Dictionary, CTFontFeatureKey.Selectors,
						NSArray.FromNSObjects (v.ConvertAll (e => (NSObject) e.Dictionary)));
			}
		}
	}

	[Since (3,2)]
	public class CTFontFeatureSelectorKey {

		public static readonly NSString Identifier;
		public static readonly NSString Name;
		public static readonly NSString Default;
		public static readonly NSString Setting;

		static CTFontFeatureSelectorKey ()
		{
			var handle = Dlfcn.dlopen (Constants.CoreTextLibrary, 0);
			if (handle == IntPtr.Zero)
				return;
			try {
				Identifier    = Dlfcn.GetStringConstant (handle, "kCTFontFeatureSelectorIdentifierKey");
				Name          = Dlfcn.GetStringConstant (handle, "kCTFontFeatureSelectorNameKey");
				Default       = Dlfcn.GetStringConstant (handle, "kCTFontFeatureSelectorDefaultKey");
				Setting       = Dlfcn.GetStringConstant (handle, "kCTFontFeatureSelectorSettingKey");
			}
			finally {
				Dlfcn.dlclose (handle);
			}
		}
	}

	[Since (3,2)]
	public class CTFontFeatureSelectors {

		public CTFontFeatureSelectors ()
			: this (new NSMutableDictionary ())
		{
		}

		public CTFontFeatureSelectors (NSDictionary dictionary)
		{
			if (dictionary == null)
				throw new ArgumentNullException ("dictionary");
			Dictionary = dictionary;
		}

		public NSDictionary Dictionary {get; private set;}

		// TODO: what kind of number?
		public NSNumber Identifier {
			get {return (NSNumber) Dictionary [CTFontFeatureSelectorKey.Identifier];}
			set {Adapter.SetValue (Dictionary, CTFontFeatureSelectorKey.Identifier, value);}
		}

		public string Name {
			get {return Adapter.GetStringValue (Dictionary, CTFontFeatureSelectorKey.Name);}
			set {Adapter.SetValue (Dictionary, CTFontFeatureSelectorKey.Name, value);}
		}

		public bool Default {
			get {
				return CFDictionary.GetBooleanValue (Dictionary.Handle, 
						CTFontFeatureSelectorKey.Default.Handle);
			}
			set {
				CFMutableDictionary.SetValue (Dictionary.Handle, 
						CTFontFeatureSelectorKey.Default.Handle,
						value);
			}
		}

		public bool Setting {
			get {
				return CFDictionary.GetBooleanValue (Dictionary.Handle, 
						CTFontFeatureSelectorKey.Setting.Handle);
			}
			set {
				CFMutableDictionary.SetValue (Dictionary.Handle, 
						CTFontFeatureSelectorKey.Setting.Handle,
						value);
			}
		}
	}

	[Since (3,2)]
	public class CTFontFeatureSettings {

		public CTFontFeatureSettings ()
			: this (new NSMutableDictionary ())
		{
		}

		public CTFontFeatureSettings (NSDictionary dictionary)
		{
			if (dictionary == null)
				throw new ArgumentNullException ("dictionary");
			Dictionary = dictionary;
		}

		public NSDictionary Dictionary {get; private set;}

		// TODO: what kind of number?
		public NSNumber TypeIdentifier {
			get {return (NSNumber) Dictionary [CTFontFeatureKey.Identifier];}
			set {Adapter.SetValue (Dictionary, CTFontFeatureKey.Identifier, value);}
		}

		// TODO: what kind of number?
		public NSNumber SelectorIdentifier {
			get {return (NSNumber) Dictionary [CTFontFeatureSelectorKey.Identifier];}
			set {Adapter.SetValue (Dictionary, CTFontFeatureSelectorKey.Identifier, value);}
		}
	}

	[Since (3,2)]
	public static class CTFontVariationAxisKey {

		public static readonly NSString Identifier;
		public static readonly NSString MinimumValue;
		public static readonly NSString MaximumValue;
		public static readonly NSString DefaultValue;
		public static readonly NSString Name;

		static CTFontVariationAxisKey ()
		{
			var handle = Dlfcn.dlopen (Constants.CoreTextLibrary, 0);
			if (handle == IntPtr.Zero)
				return;
			try {
				Identifier    = Dlfcn.GetStringConstant (handle, "kCTFontVariationAxisIdentifierKey");
				MinimumValue  = Dlfcn.GetStringConstant (handle, "kCTFontVariationAxisMinimumValueKey");
				MaximumValue  = Dlfcn.GetStringConstant (handle, "kCTFontVariationAxisMaximumValueKey");
				DefaultValue  = Dlfcn.GetStringConstant (handle, "kCTFontVariationAxisDefaultValueKey");
				Name          = Dlfcn.GetStringConstant (handle, "kCTFontVariationAxisNameKey");
			}
			finally {
				Dlfcn.dlclose (handle);
			}
		}
	}

	[Since (3,2)]
	public class CTFontVariationAxes {

		public CTFontVariationAxes ()
			: this (new NSMutableDictionary ())
		{
		}

		public CTFontVariationAxes (NSDictionary dictionary)
		{
			if (dictionary == null)
				throw new ArgumentNullException ("dictionary");
			Dictionary = dictionary;
		}

		public NSDictionary Dictionary {get; private set;}

		// TODO: what kind of number?
		public NSNumber Identifier {
			get {return (NSNumber) Dictionary [CTFontVariationAxisKey.Identifier];}
			set {Adapter.SetValue (Dictionary, CTFontVariationAxisKey.Identifier, value);}
		}

		// TODO: what kind of number?
		public NSNumber MinimumValue {
			get {return (NSNumber) Dictionary [CTFontVariationAxisKey.MinimumValue];}
			set {Adapter.SetValue (Dictionary, CTFontVariationAxisKey.MinimumValue, value);}
		}

		// TODO: what kind of number?
		public NSNumber MaximumValue {
			get {return (NSNumber) Dictionary [CTFontVariationAxisKey.MaximumValue];}
			set {Adapter.SetValue (Dictionary, CTFontVariationAxisKey.MaximumValue, value);}
		}

		// TODO: what kind of number?
		public NSNumber DefaultValue {
			get {return (NSNumber) Dictionary [CTFontVariationAxisKey.DefaultValue];}
			set {Adapter.SetValue (Dictionary, CTFontVariationAxisKey.DefaultValue, value);}
		}

		public string Name {
			get {return Adapter.GetStringValue (Dictionary, CTFontVariationAxisKey.Name);}
			set {Adapter.SetValue (Dictionary, CTFontVariationAxisKey.Name, value);}
		}
	}

	[Since (3,2)]
	public class CTFontVariation {

		public CTFontVariation ()
			: this (new NSMutableDictionary ())
		{
		}

		public CTFontVariation (NSDictionary dictionary)
		{
			if (dictionary == null)
				throw new ArgumentNullException ("dictionary");
			Dictionary = dictionary;
		}

		public NSDictionary Dictionary {get; private set;}
	}

	[Since (3,2)]
	public partial class CTFont : INativeObject, IDisposable {

#region Font Creation
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreateWithName (IntPtr name, float size, IntPtr matrix);
		public CTFont (string name, float size)
		{
			if (name == null)
				throw ConstructorError.ArgumentNull (this, "name");
			using (NSString n = new NSString (name))
				handle = CTFontCreateWithName (n.Handle, size, IntPtr.Zero);
			if (handle == IntPtr.Zero)
				throw ConstructorError.Unknown (this);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreateWithName (IntPtr name, float size, ref CGAffineTransform matrix);
		public CTFont (string name, float size, ref CGAffineTransform matrix)
		{
			if (name == null)
				throw ConstructorError.ArgumentNull (this, "name");
			using (CFString n = name)
				handle = CTFontCreateWithName (n.Handle, size, ref matrix);
			if (handle == IntPtr.Zero)
				throw ConstructorError.Unknown (this);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreateWithFontDescriptor (IntPtr descriptor, float size, IntPtr matrix);
		public CTFont (CTFontDescriptor descriptor, float size)
		{
			if (descriptor == null)
				throw ConstructorError.ArgumentNull (this, "descriptor");
			handle = CTFontCreateWithFontDescriptor (descriptor.Handle, size, IntPtr.Zero);
			if (handle == IntPtr.Zero)
				throw ConstructorError.Unknown (this);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreateWithFontDescriptor (IntPtr descriptor, float size, ref CGAffineTransform matrix);
		public CTFont (CTFontDescriptor descriptor, float size, ref CGAffineTransform matrix)
		{
			if (descriptor == null)
				throw ConstructorError.ArgumentNull (this, "descriptor");
			handle = CTFontCreateWithFontDescriptor (descriptor.Handle, size, ref matrix);
			if (handle == IntPtr.Zero)
				throw ConstructorError.Unknown (this);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreateWithNameAndOptions (IntPtr name, float size, IntPtr matrix, CTFontOptions options);
		public CTFont (string name, float size, CTFontOptions options)
		{
			if (name == null)
				throw ConstructorError.ArgumentNull (this, "name");
			using (CFString n = name)
				handle = CTFontCreateWithNameAndOptions (n.Handle, size, IntPtr.Zero, options);
			if (handle == IntPtr.Zero)
				throw ConstructorError.Unknown (this);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreateWithNameAndOptions (IntPtr name, float size, ref CGAffineTransform matrix, CTFontOptions options);
		public CTFont (string name, float size, ref CGAffineTransform matrix, CTFontOptions options)
		{
			if (name == null)
				throw ConstructorError.ArgumentNull (this, "name");
			using (CFString n = name)
				handle = CTFontCreateWithNameAndOptions (n.Handle, size, ref matrix, options);
			if (handle == IntPtr.Zero)
				throw ConstructorError.Unknown (this);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreateWithFontDescriptorAndOptions (IntPtr descriptor, float size, IntPtr matrix, CTFontOptions options);
		public CTFont (CTFontDescriptor descriptor, float size, CTFontOptions options)
		{
			if (descriptor == null)
				throw ConstructorError.ArgumentNull (this, "descriptor");
			handle = CTFontCreateWithFontDescriptorAndOptions (descriptor.Handle,
					size, IntPtr.Zero, options);
			if (handle == IntPtr.Zero)
				throw ConstructorError.Unknown (this);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreateWithFontDescriptorAndOptions (IntPtr descriptor, float size, ref CGAffineTransform matrix, CTFontOptions options);
		public CTFont (CTFontDescriptor descriptor, float size, CTFontOptions options, ref CGAffineTransform matrix)
		{
			if (descriptor == null)
				throw ConstructorError.ArgumentNull (this, "descriptor");
			handle = CTFontCreateWithFontDescriptorAndOptions (descriptor.Handle,
					size, ref matrix, options);
			if (handle == IntPtr.Zero)
				throw ConstructorError.Unknown (this);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreateWithGraphicsFont (IntPtr cgfontRef, float size, ref CGAffineTransform affine, IntPtr attrs);

		[DllImport (Constants.CoreTextLibrary, EntryPoint="CTFontCreateWithGraphicsFont")]
		static extern IntPtr CTFontCreateWithGraphicsFont2 (IntPtr cgfontRef, float size, IntPtr affine, IntPtr attrs);
		
		public CTFont (CGFont font, float size, CGAffineTransform transform, CTFontDescriptor descriptor)
		{
			if (font == null)
				throw new ArgumentNullException ("font");
			handle = CTFontCreateWithGraphicsFont (font.Handle, size, ref transform, descriptor == null ? IntPtr.Zero : descriptor.Handle);
			if (handle == IntPtr.Zero)
				throw ConstructorError.Unknown (this);
		}

		public CTFont (CGFont font, float size, CTFontDescriptor descriptor)
		{
			if (font == null)
				throw new ArgumentNullException ("font");
			handle = CTFontCreateWithGraphicsFont2 (font.Handle, size, IntPtr.Zero, descriptor == null ? IntPtr.Zero : descriptor.Handle);
			if (handle == IntPtr.Zero)
				throw ConstructorError.Unknown (this);
		}

		public CTFont (CGFont font, float size, CGAffineTransform transform)
		{
			if (font == null)
				throw new ArgumentNullException ("font");
			handle = CTFontCreateWithGraphicsFont (font.Handle, size, ref transform, IntPtr.Zero);
			if (handle == IntPtr.Zero)
				throw ConstructorError.Unknown (this);
		}
		
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreateUIFontForLanguage (CTFontUIFontType uiType, float size, IntPtr language);
		public CTFont (CTFontUIFontType uiType, float size, string language)
		{
			if (language == null)
				throw ConstructorError.ArgumentNull (this, "language");
			using (CFString l = language)
				handle = CTFontCreateUIFontForLanguage (uiType, size, l.Handle);
			if (handle == IntPtr.Zero)
				throw ConstructorError.Unknown (this);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreateCopyWithAttributes (IntPtr font, float size, IntPtr matrix, IntPtr attributues);
		public CTFont WithAttributes (float size, CTFontDescriptor attributes)
		{
			if (attributes == null)
				throw new ArgumentNullException ("attributes");
			return CreateFont (CTFontCreateCopyWithAttributes (handle, size, IntPtr.Zero, attributes.Handle));
		}

		static CTFont CreateFont (IntPtr h)
		{
			if (h == IntPtr.Zero)
				return null;
			return new CTFont (h, true);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreateCopyWithAttributes (IntPtr font, float size, ref CGAffineTransform matrix, IntPtr attributues);
		public CTFont WithAttributes (float size, CTFontDescriptor attributes, ref CGAffineTransform matrix)
		{
			if (attributes == null)
				throw new ArgumentNullException ("attributes");
			return CreateFont (CTFontCreateCopyWithAttributes (handle, size, ref matrix, attributes.Handle));
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreateCopyWithSymbolicTraits (IntPtr font, float size, IntPtr matrix, CTFontSymbolicTraits symTraitValue, CTFontSymbolicTraits symTraitMask);
		public CTFont WithSymbolicTraits (float size, CTFontSymbolicTraits symTraitValue, CTFontSymbolicTraits symTraitMask)
		{
			return CreateFont (
					CTFontCreateCopyWithSymbolicTraits (handle, size, IntPtr.Zero, symTraitValue, symTraitMask));
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreateCopyWithSymbolicTraits (IntPtr font, float size, ref CGAffineTransform matrix, CTFontSymbolicTraits symTraitValue, CTFontSymbolicTraits symTraitMask);
		public CTFont WithSymbolicTraits (float size, CTFontSymbolicTraits symTraitValue, CTFontSymbolicTraits symTraitMask, ref CGAffineTransform matrix)
		{
			return CreateFont (
					CTFontCreateCopyWithSymbolicTraits (handle, size, ref matrix, symTraitValue, symTraitMask));
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreateCopyWithFamily (IntPtr font, float size, IntPtr matrix, IntPtr family);
		public CTFont WithFamily (float size, string family)
		{
			if (family == null)
				throw new ArgumentNullException ("family");
			using (CFString f = family)
				return CreateFont (CTFontCreateCopyWithFamily (handle, size, IntPtr.Zero, f.Handle));
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreateCopyWithFamily (IntPtr font, float size, ref CGAffineTransform matrix, IntPtr family);
		public CTFont WithFamily (float size, string family, ref CGAffineTransform matrix)
		{
			if (family == null)
				throw new ArgumentNullException ("family");
			using (CFString f = family)
				return CreateFont (CTFontCreateCopyWithFamily (handle, size, ref matrix, f.Handle));
		}

#endregion

#region Font Cascading

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreateForString (IntPtr currentFont, IntPtr @string, NSRange range);
		public CTFont ForString (string value, NSRange range)
		{
			if (value == null)
				throw new ArgumentNullException ("value");
			using (CFString v = value)
				return CreateFont (CTFontCreateForString (handle, v.Handle, range));
		}

#endregion

#region Font Accessors

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCopyFontDescriptor (IntPtr font);
		public CTFontDescriptor GetFontDescriptor ()
		{
			var h = CTFontCopyFontDescriptor (handle);
			if (h == IntPtr.Zero)
				return null;
			return new CTFontDescriptor (h, true);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCopyAttribute (IntPtr font, IntPtr attribute);
		public NSObject GetAttribute (NSString attribute)
		{
			if (attribute == null)
				throw new ArgumentNullException ("attribute");
			return Runtime.GetNSObject (CTFontCopyAttribute (handle, attribute.Handle));
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern float CTFontGetSize (IntPtr font);
		public float Size {
			get {return CTFontGetSize (handle);}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern CGAffineTransform CTFontGetMatrix (IntPtr font);
		public CGAffineTransform Matrix {
			get {return CTFontGetMatrix (handle);}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern CTFontSymbolicTraits CTFontGetSymbolicTraits (IntPtr font);
		public CTFontSymbolicTraits SymbolicTraits {
			get {return CTFontGetSymbolicTraits (handle);}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCopyTraits (IntPtr font);
		public CTFontTraits GetTraits ()
		{
			var d = (NSDictionary) Runtime.GetNSObject (CTFontCopyTraits (handle));
			if (d == null)
				return null;
			d.Release ();
			return new CTFontTraits (d);
		}

#endregion

#region Font Names
		static string GetStringAndRelease (IntPtr cfStringRef)
		{
			if (cfStringRef == IntPtr.Zero)
				return null;
			using (var s = new CFString (cfStringRef, true))
				return s;
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCopyPostScriptName (IntPtr font);
		public string PostScriptName {
			get {return GetStringAndRelease (CTFontCopyPostScriptName (handle));}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCopyFamilyName (IntPtr font);
		public string FamilyName {
			get {return GetStringAndRelease (CTFontCopyFamilyName (handle));}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCopyFullName (IntPtr font);
		public string FullName {
			get {return GetStringAndRelease (CTFontCopyFullName (handle));}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCopyDisplayName (IntPtr font);
		public string DisplayName {
			get {return GetStringAndRelease (CTFontCopyDisplayName (handle));}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCopyName (IntPtr font, IntPtr nameKey);
		public string GetName (CTFontNameKey nameKey)
		{
			return GetStringAndRelease (CTFontCopyName (handle, CTFontNameKeyId.ToId (nameKey).Handle));
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCopyLocalizedName (IntPtr font, IntPtr nameKey);
		public string GetLocalizedName (CTFontNameKey nameKey)
		{
			return GetStringAndRelease (CTFontCopyLocalizedName (handle, CTFontNameKeyId.ToId (nameKey).Handle));
		}
#endregion

#region Font Encoding
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCopyCharacterSet (IntPtr font);
		public NSCharacterSet CharacterSet {
			get {
				var cs = (NSCharacterSet) Runtime.GetNSObject (CTFontCopyCharacterSet (handle));
				if (cs == null)
					return null;
				cs.Release ();
				return cs;
			}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern uint CTFontGetStringEncoding (IntPtr font);
		public uint StringEncoding {
			get {return CTFontGetStringEncoding (handle);}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCopySupportedLanguages (IntPtr font);
		public string[] GetSupportedLanguages ()
		{
			var cfArrayRef = CTFontCopySupportedLanguages (handle);
			if (cfArrayRef == IntPtr.Zero)
				return new string [0];
			var languages = NSArray.ArrayFromHandle<string> (cfArrayRef, CFString.FetchString);
			CFObject.CFRelease (cfArrayRef);
			return languages;
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern bool CTFontGetGlyphsForCharacters (IntPtr font, [In] char[] characters, [Out] CGGlyph[] glyphs, int count);
		public bool GetGlyphsForCharacters (char[] characters, CGGlyph[] glyphs, int count)
		{
			AssertCount (count);
			AssertLength ("characters", characters, count);
			AssertLength ("glyphs",     characters, count);

			return CTFontGetGlyphsForCharacters (handle, characters, glyphs, count);
		}

		public bool GetGlyphsForCharacters (char[] characters, CGGlyph[] glyphs)
		{
			return GetGlyphsForCharacters (characters, glyphs, Math.Min (characters.Length, glyphs.Length));
		}

		static void AssertCount (int count)
		{
			if (count < 0)
				throw new ArgumentOutOfRangeException ("count", "cannot be negative");
		}

		static void AssertLength<T>(string name, T[] array, int count)
		{
			AssertLength (name, array, count, false);
		}

		static void AssertLength<T>(string name, T[] array, int count, bool canBeNull)
		{
			if (canBeNull && array == null)
				return;
			if (array == null)
				throw new ArgumentNullException (name);
			if (array.Length < count)
				throw new ArgumentException (string.Format ("{0}.Length cannot be < count", name), name);
		}
#endregion

#region Font Metrics
		[DllImport (Constants.CoreTextLibrary)]
		static extern float CTFontGetAscent (IntPtr font);
		public float AscentMetric {
			get {return CTFontGetAscent (handle);}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern float CTFontGetDescent (IntPtr font);
		public float DescentMetric {
			get {return CTFontGetDescent (handle);}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern float CTFontGetLeading (IntPtr font);
		public float LeadingMetric {
			get {return CTFontGetLeading (handle);}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern uint CTFontGetUnitsPerEm (IntPtr font);
		public uint UnitsPerEmMetric {
			get {return CTFontGetUnitsPerEm (handle);}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern int CTFontGetGlyphCount (IntPtr font);
		public int GlyphCount {
			get {return CTFontGetGlyphCount (handle);}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern RectangleF CTFontGetBoundingBox (IntPtr font);
		public RectangleF BoundingBox {
			get {return CTFontGetBoundingBox (handle);}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern float CTFontGetUnderlinePosition (IntPtr font);
		public float UnderlinePosition {
			get {return CTFontGetUnderlinePosition (handle);}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern float CTFontGetUnderlineThickness (IntPtr font);
		public float UnderlineThickness {
			get {return CTFontGetUnderlineThickness (handle);}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern float CTFontGetSlantAngle (IntPtr font);
		public float SlantAngle {
			get {return CTFontGetSlantAngle (handle);}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern float CTFontGetCapHeight (IntPtr font);
		public float CapHeightMetric {
			get {return CTFontGetCapHeight (handle);}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern float CTFontGetXHeight (IntPtr font);
		public float XHeightMetric {
			get {return CTFontGetXHeight (handle);}
		}
#endregion

#region Font Glyphs
		[DllImport (Constants.CoreTextLibrary)]
		static extern CGGlyph CTFontGetGlyphWithName (IntPtr font, IntPtr glyphName);
		public CGGlyph GetGlyphWithName (string glyphName)
		{
			if (glyphName == null)
				throw new ArgumentNullException ("glyphName");
			using (NSString n = new NSString (glyphName))
				return CTFontGetGlyphWithName (handle, n.Handle);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern RectangleF CTFontGetBoundingRectsForGlyphs (IntPtr font, CTFontOrientation orientation, [In] CGGlyph[] glyphs, [Out] RectangleF[] boundingRects, int count);
		public RectangleF GetBoundingRects (CTFontOrientation orientation, CGGlyph[] glyphs, RectangleF[] boundingRects, int count)
		{
			AssertCount (count);
			AssertLength ("glyphs",         glyphs, count);
			AssertLength ("boundingRects",  boundingRects, count, true);

			return CTFontGetBoundingRectsForGlyphs (handle, orientation, glyphs, boundingRects, count);
		}

		public RectangleF GetBoundingRects (CTFontOrientation orientation, CGGlyph[] glyphs)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			return GetBoundingRects (orientation, glyphs, null, glyphs.Length);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern double CTFontGetAdvancesForGlyphs (IntPtr font, CTFontOrientation orientation, [In] CGGlyph[] glyphs, [Out] SizeF[] advances, int count);
		public double GetAdvancesForGlyphs (CTFontOrientation orientation, CGGlyph[] glyphs, SizeF[] advances, int count)
		{
			AssertCount (count);
			AssertLength ("glyphs",   glyphs, count);
			AssertLength ("advances", advances, count, true);

			return CTFontGetAdvancesForGlyphs (handle, orientation, glyphs, advances, count);
		}

		public double GetAdvancesForGlyphs (CTFontOrientation orientation, CGGlyph[] glyphs)
		{
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			return GetAdvancesForGlyphs (orientation, glyphs, null, glyphs.Length);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern void CTFontGetVerticalTranslationsForGlyphs (IntPtr font, [In] CGGlyph[] glyphs, [Out] SizeF[] translations, int count);
		public void GetVerticalTranslationsForGlyphs (CGGlyph[] glyphs, SizeF[] translations, int count)
		{
			AssertCount (count);
			AssertLength ("glyphs",       glyphs, count);
			AssertLength ("translations", translations, count);

			CTFontGetVerticalTranslationsForGlyphs (handle, glyphs, translations, count);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreatePathForGlyph (IntPtr font, CGGlyph glyph, IntPtr transform);
		public CGPath GetPathForGlyph (CGGlyph glyph)
		{
			var h = CTFontCreatePathForGlyph (handle, glyph, IntPtr.Zero);
			if (h == IntPtr.Zero)
				return null;
			return new CGPath (h, true);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCreatePathForGlyph (IntPtr font, CGGlyph glyph, ref CGAffineTransform transform);
		public CGPath GetPathForGlyph (CGGlyph glyph, ref CGAffineTransform transform)
		{
			var h = CTFontCreatePathForGlyph (handle, glyph, ref transform);
			if (h == IntPtr.Zero)
				return null;
			return new CGPath (h, true);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern void CTFontDrawGlyphs (IntPtr font, [In] CGGlyph [] glyphs, [In] PointF [] positions, int count, IntPtr context);

		[Since(4,2)]
		public void DrawGlyphs (CGContext context, CGGlyph [] glyphs, PointF [] positions)
		{
			if (context == null)
				throw new ArgumentNullException ("context");
			if (glyphs == null)
				throw new ArgumentNullException ("glyphs");
			if (positions == null)
				throw new ArgumentNullException ("positions");
			int gl = glyphs.Length;
			if (gl != positions.Length)
				throw new ArgumentException ("array sizes fo context and glyphs differ");
			CTFontDrawGlyphs (handle, glyphs, positions, gl, context.Handle);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern int CTFontGetLigatureCaretPositions (IntPtr handle, CGGlyph glyph, [Out] float [] positions, int max);

		[Since(4,2)]
		public int GetLigatureCaretPositions (CGGlyph glyph, float [] positions)
		{
			if (positions == null)
				throw new ArgumentNullException ("positions");
			return CTFontGetLigatureCaretPositions (handle, glyph, positions, positions.Length);
		}
#endregion

#region Font Variations
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCopyVariationAxes (IntPtr font);
		public CTFontVariationAxes[] GetVariationAxes ()
		{
			var cfArrayRef = CTFontCopyVariationAxes (handle);
			if (cfArrayRef == IntPtr.Zero)
				return new CTFontVariationAxes [0];
			var axes = NSArray.ArrayFromHandle (cfArrayRef,
					d => new CTFontVariationAxes ((NSDictionary) Runtime.GetNSObject (d)));
			CFObject.CFRelease (cfArrayRef);
			return axes;
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCopyVariation (IntPtr font);
		public CTFontVariation GetVariation ()
		{
			var cfDictionaryRef = CTFontCopyVariation (handle);
			if (cfDictionaryRef == IntPtr.Zero)
				return null;
			return new CTFontVariation ((NSDictionary) Runtime.GetNSObject (cfDictionaryRef));
		}
#endregion

#region Font Features
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCopyFeatures (IntPtr font);
		public CTFontFeatures[] GetFeatures ()
		{
			var cfArrayRef = CTFontCopyFeatures (handle);
			if (cfArrayRef == IntPtr.Zero)
				return new CTFontFeatures [0];
			var features = NSArray.ArrayFromHandle (cfArrayRef,
					d => new CTFontFeatures ((NSDictionary) Runtime.GetNSObject (d)));
			CFObject.CFRelease (cfArrayRef);
			return features;
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCopyFeatureSettings (IntPtr font);
		public CTFontFeatureSettings[] GetFeatureSettings ()
		{
			var cfArrayRef = CTFontCopyFeatureSettings (handle);
			if (cfArrayRef == IntPtr.Zero)
				return new CTFontFeatureSettings [0];
			var featureSettings = NSArray.ArrayFromHandle (cfArrayRef,
					d => new CTFontFeatureSettings ((NSDictionary) Runtime.GetNSObject (d)));
			CFObject.CFRelease (cfArrayRef);
			return featureSettings;
		}
#endregion

#region Font Conversion
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCopyGraphicsFont (IntPtr font, IntPtr attributes);
		public CGFont ToCGFont (CTFontDescriptor attributes)
		{
			var h = CTFontCopyGraphicsFont (handle, attributes == null ? IntPtr.Zero : attributes.Handle);
			if (h == IntPtr.Zero)
				return null;
			return new CGFont (h, true);
		}

		public CGFont ToCGFont ()
		{
			return ToCGFont (null);
		}
#endregion

#region Font Tables
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCopyAvailableTables (IntPtr font, CTFontTableOptions options);
		public CTFontTable[] GetAvailableTables (CTFontTableOptions options)
		{
			var cfArrayRef = CTFontCopyAvailableTables (handle, options);
			if (cfArrayRef == IntPtr.Zero)
				return new CTFontTable [0];
			var tables = NSArray.ArrayFromHandle (cfArrayRef, v => {
					return (CTFontTable) (uint) v;
			});
			CFObject.CFRelease (cfArrayRef);
			return tables;
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontCopyTable (IntPtr font, CTFontTable table, CTFontTableOptions options);
		public NSData GetFontTableData (CTFontTable table, CTFontTableOptions options)
		{
			IntPtr cfDataRef = CTFontCopyTable (handle, table, options);
			if (cfDataRef == IntPtr.Zero)
				return null;
			var d = new NSData (cfDataRef);
			d.Release ();
			return d;
		}
#endregion

		public override string ToString ()
		{
			return FullName;
		}

		[DllImport (Constants.CoreTextLibrary, EntryPoint="CTFontGetTypeID")]
		public extern static int GetTypeID ();
	}
}
