using System;
using MonoMac.CoreText;
#if !MONOMAC
using MonoMac.UIKit;
#endif
namespace MonoMac.Foundation {
	public partial class NSAttributedString {

		public NSAttributedString (string str, CTStringAttributes attributes)
			: this (str, attributes != null ? attributes.Dictionary : null)
		{
		}

		public NSAttributedString Substring (int start, int len)
		{
			return Substring (new NSRange (start, len));
		}

		static NSDictionary ToDictionary (UIFont font = null,
						  UIColor foregroundColor = null,
						  UIColor backgroundColor = null,
						  UIColor strokeColor = null,
						  NSParagraphStyle paragraphStyle = null,
						  NSLigatureType ligatures = NSLigatureType.Default,
						  float kerning = 0,
						  NSUnderlineStyle underlineStyle = NSUnderlineStyle.None,
						  NSShadow shadow = null,
						  float strokeWidth = 0,
						  NSUnderlineStyle strikethroughStyle = NSUnderlineStyle.None)
		{
			NSObject [] keys = new NSObject [11];
			NSObject [] values = new NSObject [11];
			int i = 0;
			if (font != null){
				keys [i] = NSAttributedString.FontAttributeName;
				values [i++] = font;
			}
			if (foregroundColor != null){
				keys [i] = NSAttributedString.ForegroundColorAttributeName;
				values [i++] = foregroundColor;
			}
			if (backgroundColor != null){
				keys [i] = NSAttributedString.BackgroundColorAttributeName;
				values [i++] = backgroundColor;
			}
			if (strokeColor != null){
				keys [i] = NSAttributedString.StrokeColorAttributeName;
				values [i++] = strokeColor;
			}
			if (paragraphStyle != null){
				keys [i] = NSAttributedString.ParagraphStyleAttributeName;
				values [i++] = paragraphStyle;
			}
			if (ligatures != NSLigatureType.Default){
				keys [i] = NSAttributedString.LigatureAttributeName;
				values [i++] = new NSNumber ((int) ligatures);
			}
			if (kerning != 0){
				keys [i] = NSAttributedString.KernAttributeName;
				values [i++] = new NSNumber (kerning);
			}
			if (underlineStyle != NSUnderlineStyle.None){
				keys [i] = NSAttributedString.UnderlineStyleAttributeName;
				values [i++] = new NSNumber ((int) underlineStyle);
			}
			if (shadow != null){
				keys [i] = NSAttributedString.ShadowAttributeName;
				values [i++] = shadow;
			}
			if (strokeWidth != 0){
				keys [i] = NSAttributedString.StrokeWidthAttributeName;
				values [i++] = new NSNumber (strokeWidth);
			}
			if (strikethroughStyle != NSUnderlineStyle.None){
				keys [i] = NSAttributedString.UnderlineStyleAttributeName;
				values [i++] = new NSNumber ((int) strikethroughStyle);
			}
			if (i > 0)
				return NSDictionary.FromObjectsAndKeys (values, keys, i);
			else
				return null;
		}				

		public NSAttributedString (string str,
					   UIFont font = null,
					   UIColor foregroundColor = null,
					   UIColor backgroundColor = null,
					   UIColor strokeColor = null,
					   NSParagraphStyle paragraphStyle = null,
					   NSLigatureType ligatures = NSLigatureType.Default,
					   float kerning = 0,
					   NSUnderlineStyle underlineStyle = NSUnderlineStyle.None,
					   NSShadow shadow = null,
					   float strokeWidth = 0,
					   NSUnderlineStyle strikethroughStyle = NSUnderlineStyle.None)
		: this (str, ToDictionary (font, foregroundColor, backgroundColor, strokeColor, paragraphStyle, ligatures, kerning, underlineStyle, shadow, strokeWidth, strikethroughStyle))
		{
		}
				
							      
	}
}
