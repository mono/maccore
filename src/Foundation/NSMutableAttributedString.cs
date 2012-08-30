// 
// NSMutableAttributedString.cs: Helpers and overloads for NSMutableAttributedString members.
//
// Authors: Mono Team
//     
// Copyright 2010 Novell, Inc
//
#if !MONOMAC
using MonoMac.UIKit;
#endif

using MonoMac.CoreText;

namespace MonoMac.Foundation {

	public partial class NSMutableAttributedString {

		public NSMutableAttributedString (string str, CTStringAttributes attributes)
			: this (str, attributes == null ? null : attributes.Dictionary)
		{
		}

		public void SetAttributes (CTStringAttributes attrs, NSRange range)
		{
			SetAttributes (attrs == null ? null : attrs.Dictionary, range);
		}

		public void AddAttributes (CTStringAttributes attrs, NSRange range)
		{
			AddAttributes (attrs == null ? null : attrs.Dictionary, range);
		}

		public void Append (NSAttributedString first, params object [] rest)
		{
			Append (first);
			foreach (var obj in rest){
				if (obj is NSAttributedString)
					Append ((NSAttributedString) obj);
				else if (obj is string)
					Append (new NSAttributedString ((string) obj));
				else
					Append (new NSAttributedString (obj.ToString ()));

			}
		}
#if !MONOMAC
		public NSMutableAttributedString (string str, UIStringAttributes attributes)
		: this (str, attributes != null ? attributes.Dictionary : null)
		{
		}

		public NSMutableAttributedString (string str,
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
#endif
	}
}
