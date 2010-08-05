// 
// NSMutableAttributedString.cs: Helpers and overloads for NSMutableAttributedString members.
//
// Authors: Mono Team
//     
// Copyright 2010 Novell, Inc
//

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
	}
}
