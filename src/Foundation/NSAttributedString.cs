using MonoMac.CoreText;

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
		
	}
}
