using System;

namespace MonoMac.Foundation {

	public partial class NSUrl {
		public override bool Equals (object t)
		{
			if (t == null)
				return false;
			
			if (t is NSUrl){
				return IsEqual ((NSUrl) t);
			}
			return false;
		}

		public override int GetHashCode ()
		{
			return (int) Handle;
		}

		// Converts from an NSURL to a System.Uri
		public static implicit operator Uri (NSUrl url)
		{
			if (url.RelativePath == url.Path)
				return new Uri (url.AbsoluteString, UriKind.Absolute);
			else
				return new Uri (url.RelativePath, UriKind.Relative);
		}

		public static implicit operator NSUrl (Uri uri)
		{
			if (uri.IsAbsoluteUri)
				return new NSUrl (uri.AbsolutePath);
			else
				return new NSUrl (uri.PathAndQuery);
		}

		public static NSUrl FromFilename (string url)
		{
			return new NSUrl (url, false);
		}
		
		public NSUrl MakeRelative (string url)
		{
			return _FromStringRelative (url, this);
		}

		public override string ToString ()
		{
			return AbsoluteString ?? base.ToString ();
		}

		public bool TryGetResource (string key, out NSObject value, out NSError error)
		{
			return GetResourceValue (out value, key, out error);
		}

		public bool TryGetResource (string key, out NSObject value)
		{
			NSError error;
			return GetResourceValue (out value, key, out error);
		}

		public bool SetResource (string key, NSObject value, out NSError error)
		{
			return SetResourceValue (value, key, out error);
		}

		public bool SetResource (string key, NSObject value)
		{
			NSError error;
			return SetResourceValue (value, key, out error);
		}
	}
}
