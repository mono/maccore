// 
// CFProxySupport.cs: Implements the managed binding for CFProxySupport.h
//
// Authors: Jeffrey Stedfast <jeff@xamarin.com>
//     
// Copyright (C) 2011 Xamarin, Inc.
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
using System.Net;
using System.Runtime.InteropServices;
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;

namespace MonoMac.CoreFoundation {
	public enum CFProxyType {
		None,
		AutoConfigurationUrl,
		AutoConfigurationJavaScript,
		FTP,
		HTTP,
		HTTPS,
		SOCKS
	}
	
	public class CFProxy {
		NSDictionary settings;
		
		internal CFProxy (NSDictionary settings)
		{
			this.settings = settings;
		}
		
		#region Property Keys
		static NSString kCFProxyAutoConfigurationHTTPResponseKey;
		static NSString AutoConfigurationHTTPResponseKey {
			get {
				if (kCFProxyAutoConfigurationHTTPResponseKey == null)
					kCFProxyAutoConfigurationHTTPResponseKey = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFProxyAutoConfigurationHTTPResponseKey");
				
				return kCFProxyAutoConfigurationHTTPResponseKey;
			}
		}
		
		static NSString kCFProxyAutoConfigurationJavaScriptKey;
		static NSString AutoConfigurationJavaScriptKey {
			get {
				if (kCFProxyAutoConfigurationJavaScriptKey == null)
					kCFProxyAutoConfigurationJavaScriptKey = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFProxyAutoConfigurationJavaScriptKey");
				
				return kCFProxyAutoConfigurationJavaScriptKey;
			}
		}
		
		static NSString kCFProxyAutoConfigurationURLKey;
		static NSString AutoConfigurationURLKey {
			get {
				if (kCFProxyAutoConfigurationURLKey == null)
					kCFProxyAutoConfigurationURLKey = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFProxyAutoConfigurationURLKey");
				
				return kCFProxyAutoConfigurationURLKey;
			}
		}
		
		static NSString kCFProxyHostNameKey;
		static NSString HostNameKey {
			get {
				if (kCFProxyHostNameKey == null)
					kCFProxyHostNameKey = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFProxyHostNameKey");
				
				return kCFProxyHostNameKey;
			}
		}
		
		static NSString kCFProxyPasswordKey;
		static NSString PasswordKey {
			get {
				if (kCFProxyPasswordKey == null)
					kCFProxyPasswordKey = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFProxyPasswordKey");
				
				return kCFProxyPasswordKey;
			}
		}
		
		static NSString kCFProxyPortNumberKey;
		static NSString PortNumberKey {
			get {
				if (kCFProxyPortNumberKey == null)
					kCFProxyPortNumberKey = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFProxyPortNumberKey");
				
				return kCFProxyPortNumberKey;
			}
		}
		
		static NSString kCFProxyTypeKey;
		static NSString ProxyTypeKey {
			get {
				if (kCFProxyTypeKey == null)
					kCFProxyTypeKey = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFProxyTypeKey");
				
				return kCFProxyTypeKey;
			}
		}
		
		static NSString kCFProxyUsernameKey;
		static NSString UsernameKey {
			get {
				if (kCFProxyUsernameKey == null)
					kCFProxyUsernameKey = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFProxyUsernameKey");
				
				return kCFProxyUsernameKey;
			}
		}
		#endregion Property Keys
		
		#region Proxy Types
		static NSString kCFProxyTypeNone;
		static NSString CFProxyTypeNone {
			get {
				if (kCFProxyTypeNone == null)
					kCFProxyTypeNone = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFProxyTypeNone");
				
				return kCFProxyTypeNone;
			}
		}
		
		static NSString kCFProxyTypeAutoConfigurationURL;
		static NSString CFProxyTypeAutoConfigurationURL {
			get {
				if (kCFProxyTypeAutoConfigurationURL == null)
					kCFProxyTypeAutoConfigurationURL = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFProxyTypeAutoConfigurationURL");
				
				return kCFProxyTypeAutoConfigurationURL;
			}
		}
		
		static NSString kCFProxyTypeAutoConfigurationJavaScript;
		static NSString CFProxyTypeAutoConfigurationJavaScript {
			get {
				if (kCFProxyTypeAutoConfigurationJavaScript == null)
					kCFProxyTypeAutoConfigurationJavaScript = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFProxyTypeAutoConfigurationJavaScript");
				
				return kCFProxyTypeAutoConfigurationJavaScript;
			}
		}
		
		static NSString kCFProxyTypeFTP;
		static NSString CFProxyTypeFTP {
			get {
				if (kCFProxyTypeFTP == null)
					kCFProxyTypeFTP = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFProxyTypeFTP");
				
				return kCFProxyTypeFTP;
			}
		}
		
		static NSString kCFProxyTypeHTTP;
		static NSString CFProxyTypeHTTP {
			get {
				if (kCFProxyTypeHTTP == null)
					kCFProxyTypeHTTP = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFProxyTypeHTTP");
				
				return kCFProxyTypeHTTP;
			}
		}
		
		static NSString kCFProxyTypeHTTPS;
		static NSString CFProxyTypeHTTPS {
			get {
				if (kCFProxyTypeHTTPS == null)
					kCFProxyTypeHTTPS = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFProxyTypeHTTPS");
				
				return kCFProxyTypeHTTPS;
			}
		}
		
		static NSString kCFProxyTypeSOCKS;
		static NSString CFProxyTypeSOCKS {
			get {
				if (kCFProxyTypeSOCKS == null)
					kCFProxyTypeSOCKS = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFProxyTypeSOCKS");
				
				return kCFProxyTypeSOCKS;
			}
		}
		#endregion Proxy Types
		
		static CFProxyType CFProxyTypeToEnum (NSString type)
		{
			if (type.Handle == CFProxyTypeAutoConfigurationJavaScript.Handle)
				return CFProxyType.AutoConfigurationJavaScript;
			
			if (type.Handle == CFProxyTypeAutoConfigurationURL.Handle)
				return CFProxyType.AutoConfigurationUrl;
			
			if (type.Handle == CFProxyTypeFTP.Handle)
				return CFProxyType.FTP;
			
			if (type.Handle == CFProxyTypeHTTP.Handle)
				return CFProxyType.HTTP;
			
			if (type.Handle == CFProxyTypeHTTPS.Handle)
				return CFProxyType.HTTPS;
			
			if (type.Handle == CFProxyTypeSOCKS.Handle)
				return CFProxyType.SOCKS;
			
			return CFProxyType.None;
		}
		
#if notyet
		// AFAICT these get used with CFNetworkExecuteProxyAutoConfiguration*()
		
		// TODO: bind CFHTTPMessage so we can return the proper type here.
		public NSObject AutoConfigurationHTTPResponse {
			get { return settings[AutoConfigurationHTTPResponseKey]; }
		}
		
		public NSString AutoConfigurationJavaScript {
			get {
				return (NSString) settings[AutoConfigurationJavaScriptKey];
			}
		}
		
		public NSUrl AutoConfigurationUrl {
			get {
				return (NSUrl) settings[AutoConfigurationURLKey];
			}
		}
#endif
		
		public string HostName {
			get {
				NSString v = (NSString) settings[HostNameKey];
				
				return v != null ? v.ToString () : null;
			}
		}
		
		public string Password {
			get {
				NSString v = (NSString) settings[PasswordKey];
				
				return v != null ? v.ToString () : null;
			}
		}
		
		public int Port {
			get {
				NSNumber v = (NSNumber) settings[PortNumberKey];
				
				return v != null ? v.Int32Value : 0;
			}
		}
		
		public CFProxyType ProxyType {
			get {
				return CFProxyTypeToEnum ((NSString) settings[ProxyTypeKey]);
			}
		}
		
		public string Username {
			get {
				NSString v = (NSString) settings[UsernameKey];
				
				return v != null ? v.ToString () : null;
			}
		}
		
		public bool CanGetWebProxy {
			get {
				switch (ProxyType) {
				case CFProxyType.AutoConfigurationJavaScript: return false; // not doable via WebProxy
				case CFProxyType.AutoConfigurationUrl: return false; // not doable via WebProxy
				case CFProxyType.FTP: return true;
				case CFProxyType.HTTP: return true;
				case CFProxyType.HTTPS: return true;
				case CFProxyType.SOCKS: return false; // not doable via WebProxy
				default: return false;
				}
			}
		}
		
		internal Uri GetProxyUri ()
		{
			string protocol;
			
			switch (ProxyType) {
			case CFProxyType.FTP:
				protocol = "ftp://";
				break;
			case CFProxyType.HTTP:
				protocol = "http://";
				break;
			case CFProxyType.HTTPS:
				protocol = "https://";
				break;
			default:
				return null;
			}
			
			string username = Username;
			string password = Password;
			string hostname = HostName;
			int port = Port;
			string userinfo;
			string uri;
			
			if (username != null) {
				if (password != null)
					userinfo = Uri.EscapeDataString (username) + ':' + Uri.EscapeDataString (password) + '@';
				else
					userinfo = Uri.EscapeDataString (username) + '@';
			} else {
				userinfo = string.Empty;
			}
			
			uri = protocol + userinfo + hostname + (port != 0 ? ':' + port.ToString () : string.Empty);
			
			return new Uri (uri, UriKind.Absolute);
		}
		
		public WebProxy GetWebProxy ()
		{
			Uri uri = GetProxyUri ();
			
			if (uri == null)
				return null;
			
			return new WebProxy (uri);
		}
	}
	
	public class CFProxySettings {
		NSDictionary settings;
		
		internal CFProxySettings (NSDictionary settings)
		{
			this.settings = settings;
		}
		
		internal NSDictionary Dictionary {
			get { return settings; }
		}
		
		#region Global Proxy Setting Constants
		static NSString kCFNetworkProxiesHTTPEnable;
		static NSString CFNetworkProxiesHTTPEnable {
			get {
				if (kCFNetworkProxiesHTTPEnable == null)
					kCFNetworkProxiesHTTPEnable = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFNetworkProxiesHTTPEnable");
				
				return kCFNetworkProxiesHTTPEnable;
			}
		}
		
		static NSString kCFNetworkProxiesHTTPPort;
		static NSString CFNetworkProxiesHTTPPort {
			get {
				if (kCFNetworkProxiesHTTPPort == null)
					kCFNetworkProxiesHTTPPort = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFNetworkProxiesHTTPPort");
				
				return kCFNetworkProxiesHTTPPort;
			}
		}
		
		static NSString kCFNetworkProxiesHTTPProxy;
		static NSString CFNetworkProxiesHTTPProxy {
			get {
				if (kCFNetworkProxiesHTTPProxy == null)
					kCFNetworkProxiesHTTPProxy = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFNetworkProxiesHTTPProxy");
				
				return kCFNetworkProxiesHTTPProxy;
			}
		}
		
		static NSString kCFNetworkProxiesProxyAutoConfigEnable;
		static NSString CFNetworkProxiesProxyAutoConfigEnable {
			get {
				if (kCFNetworkProxiesProxyAutoConfigEnable == null)
					kCFNetworkProxiesProxyAutoConfigEnable = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFNetworkProxiesProxyAutoConfigEnable");
				
				return kCFNetworkProxiesProxyAutoConfigEnable;
			}
		}
		
		static NSString kCFNetworkProxiesProxyAutoConfigJavaScript;
		static NSString CFNetworkProxiesProxyAutoConfigJavaScript {
			get {
				if (kCFNetworkProxiesProxyAutoConfigJavaScript == null)
					kCFNetworkProxiesProxyAutoConfigJavaScript = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFNetworkProxiesProxyAutoConfigJavaScript");
				
				return kCFNetworkProxiesProxyAutoConfigJavaScript;
			}
		}
		
		static NSString kCFNetworkProxiesProxyAutoConfigURLString;
		static NSString CFNetworkProxiesProxyAutoConfigURLString {
			get {
				if (kCFNetworkProxiesProxyAutoConfigURLString == null)
					kCFNetworkProxiesProxyAutoConfigURLString = Dlfcn.GetStringConstant (CFNetwork.CoreFoundationLibraryHandle, "kCFNetworkProxiesProxyAutoConfigURLString");
				
				return kCFNetworkProxiesProxyAutoConfigURLString;
			}
		}
		#endregion Global Proxy Setting Constants
		
		public bool HTTPEnable {
			get {
				NSNumber v = (NSNumber) settings[CFNetworkProxiesHTTPEnable];
				
				return v != null ? v.BoolValue : false;
			}
		}
		
		public int HTTPPort {
			get {
				NSNumber v = (NSNumber) settings[CFNetworkProxiesHTTPPort];
				
				return v != null ? v.Int32Value : 0;
			}
		}
		
		public string HTTPProxy {
			get {
				NSString v = (NSString) settings[CFNetworkProxiesHTTPProxy];
				
				return v != null ? v.ToString () : null;
			}
		}
		
		public bool ProxyAutoConfigEnable {
			get {
				NSNumber v = (NSNumber) settings[CFNetworkProxiesProxyAutoConfigEnable];
				
				return v != null ? v.BoolValue : false;
			}
		}
		
		public string ProxyAutoConfigJavaScript {
			get {
				NSString v = (NSString) settings[CFNetworkProxiesProxyAutoConfigJavaScript];
				
				return v != null ? v.ToString () : null;
			}
		}
		
		public string ProxyAutoConfigURLString {
			get {
				NSString v = (NSString) settings[CFNetworkProxiesProxyAutoConfigURLString];
				
				return v != null ? v.ToString () : null;
			}
		}
	}
	
	public static class CFNetwork {
		internal static IntPtr CoreFoundationLibraryHandle = Dlfcn.dlopen (Constants.CoreFoundationLibrary, 0);
		
		[DllImport (Constants.CoreFoundationLibrary)]
		// CFArrayRef CFNetworkCopyProxiesForAutoConfigurationScript (CFStringRef proxyAutoConfigurationScript, CFURLRef targetURL);
		extern static IntPtr CFNetworkCopyProxiesForAutoConfigurationScript (IntPtr proxyAutoConfigurationScript, IntPtr targetURL);
		
		static NSArray CopyProxiesForAutoConfigurationScript (NSString proxyAutoConfigurationScript, NSUrl targetURL)
		{
			IntPtr native = CFNetworkCopyProxiesForAutoConfigurationScript (proxyAutoConfigurationScript.Handle, targetURL.Handle);
			
			if (native == IntPtr.Zero)
				return null;
			
			return new NSArray (native);
		}
		
		public static CFProxySettings[] GetProxiesForAutoConfigurationScript (NSString proxyAutoConfigurationScript, NSUrl targetURL)
		{
			if (proxyAutoConfigurationScript == null)
				throw new ArgumentNullException ("proxyAutoConfigurationScript");
			
			if (targetURL == null)
				throw new ArgumentNullException ("targetURL");
			
			NSArray array = CopyProxiesForAutoConfigurationScript (proxyAutoConfigurationScript, targetURL);
			
			if (array == null)
				return null;
			
			NSDictionary[] dictionaries = NSArray.ArrayFromHandle<NSDictionary> (array.Handle);
			array.Dispose ();
			
			if (dictionaries == null)
				return null;
			
			CFProxySettings[] proxies = new CFProxySettings [dictionaries.Length];
			for (int i = 0; i < dictionaries.Length; i++)
				proxies[i] = new CFProxySettings (dictionaries[i]);
			
			return proxies;
		}
		
		[DllImport (Constants.CoreFoundationLibrary)]
		// CFArrayRef CFNetworkCopyProxiesForURL (CFURLRef url, CFDictionaryRef proxySettings);
		extern static IntPtr CFNetworkCopyProxiesForURL (IntPtr url, IntPtr proxySettings);
		
		static NSArray CopyProxiesForURL (NSUrl url, NSDictionary proxySettings)
		{
			IntPtr native = CFNetworkCopyProxiesForURL (url.Handle, proxySettings != null ? proxySettings.Handle : IntPtr.Zero);
			
			if (native == IntPtr.Zero)
				return null;
			
			return new NSArray (native);
		}
		
		public static CFProxy[] GetProxiesForURL (NSUrl url, CFProxySettings proxySettings)
		{
			if (url == null)
				throw new ArgumentNullException ("url");
			
			if (proxySettings == null)
				proxySettings = GetSystemProxySettings ();
			
			NSArray array = CopyProxiesForURL (url, proxySettings.Dictionary);
			
			if (array == null)
				return null;
			
			NSDictionary[] dictionaries = NSArray.ArrayFromHandle<NSDictionary> (array.Handle);
			array.Dispose ();
			
			if (dictionaries == null)
				return null;
			
			CFProxy[] proxies = new CFProxy [dictionaries.Length];
			for (int i = 0; i < dictionaries.Length; i++)
				proxies[i] = new CFProxy (dictionaries[i]);
			
			return proxies;
		}
		
		public static CFProxy[] GetProxiesForUri (Uri uri, CFProxySettings proxySettings)
		{
			if (uri == null)
				throw new ArgumentNullException ("uri");
			
			NSUrl url = new NSUrl (uri.AbsoluteUri, null);
			CFProxy[] proxies = GetProxiesForURL (url, proxySettings);
			url.Dispose ();
			
			return proxies;
		}
		
		[DllImport (Constants.CoreFoundationLibrary)]
		// CFDictionaryRef CFNetworkCopySystemProxySettings (void);
		extern static IntPtr CFNetworkCopySystemProxySettings ();
		
		public static CFProxySettings GetSystemProxySettings ()
		{
			IntPtr native = CFNetworkCopySystemProxySettings ();
			
			if (native == IntPtr.Zero)
				return null;
			
			return new CFProxySettings (new NSDictionary (native));
		}
		
#if notyet
		// FIXME: These require bindings for CFRunLoopSource and CFStreamClientContext
		
		public delegate void CFProxyAutoConfigurationResultCallback (IntPtr client, NSArray proxyList, NSError error);
		
		[DllImport (Constants.CoreFoundationLibrary)]
		// CFRunLoopSourceRef CFNetworkExecuteProxyAutoConfigurationScript (CFStringRef proxyAutoConfigurationScript, CFURLRef targetURL, CFProxyAutoConfigurationResultCallback cb, CFStreamClientContext *clientContext);
		extern static IntPtr CFNetworkExecuteProxyAutoConfigurationScript (IntPtr proxyAutoConfigurationScript, IntPtr targetURL, IntPtr cb, IntPtr clientContext);
		
		public static CFRunLoopSource ExecuteProxyAutoConfigurationScript (NSString proxyAutoConfigurationScript, NSUrl targetURL, CFProxyAutoConfigurationResultCallback resultCallback, CFStreamClientContext clientContext)
		{
			if (proxyAutoConfigurationScript == null)
				throw new ArgumentNullException ("proxyAutoConfigurationScript");

			if (targetURL == null)
				throw new ArgumentNullException ("targetURL");

			if (resultCallback == null)
				throw new ArgumentNullException ("resultCallback");

			if (clientContext == null)
				throw new ArgumentNullException ("clientContext");

			IntPtr source = CFNetworkExecuteProxyAutoConfigurationScript (proxyAutoConfigurationScript.Handle, targetURL.Handle, resultCallback, clientContext);

			if (source != IntPtr.Zero)
				return new CFRunLoopSource (source);

			return null;
		}
		
		[DllImport (Constants.CoreFoundationLibrary)]
		// CFRunLoopSourceRef CFNetworkExecuteProxyAutoConfigurationURL (CFURLRef proxyAutoConfigurationURL, CFURLRef targetURL, CFProxyAutoConfigurationResultCallback cb, CFStreamClientContext *clientContext);
		extern static IntPtr CFNetworkExecuteProxyAutoConfigurationURL (IntPtr proxyAutoConfigurationURL, IntPtr targetURL, IntPtr cb, IntPtr clientContext);
		
		public static CFRunLoopSource ExecuteProxyAutoConfigurationURL (NSUrl proxyAutoConfigurationURL, NSUrl targetURL, CFProxyAutoConfigurationResultCallback resultCallback, CFStreamClientContext clientContext)
		{
			if (proxyAutoConfigurationURL == null)
				throw new ArgumentNullException ("proxyAutoConfigurationURL");

			if (targetURL == null)
				throw new ArgumentNullException ("targetURL");

			if (resultCallback == null)
				throw new ArgumentNullException ("resultCallback");

			if (clientContext == null)
				throw new ArgumentNullException ("clientContext");

			IntPtr source = CFNetworkExecuteProxyAutoConfigurationURL (proxyAutoConfigurationURL.Handle, targetURL.Handle, resultCallback, clientContext);

			if (source != IntPtr.Zero)
				return new CFRunLoopSource (source);

			return null;
		}
#endif
		
		class CFWebProxy : IWebProxy {
			public CFWebProxy ()
			{
				
			}
			
			public ICredentials Credentials {
				get {
					throw new NotSupportedException ();
				}
				set {
					throw new NotSupportedException ();
				}
			}
			
			public Uri GetProxy (Uri targetUri)
			{
				if (targetUri == null)
					throw new ArgumentNullException ("targetUri");
				
				CFProxySettings settings = CFNetwork.GetSystemProxySettings ();
				CFProxy[] proxies = CFNetwork.GetProxiesForUri (targetUri, settings);
				
				if (proxies == null)
					return targetUri;
				
				for (int i = 0; i < proxies.Length; i++) {
					Uri uri = proxies[i].GetProxyUri ();
					
					if (uri != null)
						return uri;
				}
				
				// no supported proxies for this Uri, fall back to trying to connect to targetUri directly.
				return targetUri;
			}
			
			public bool IsBypassed (Uri targetUri)
			{
				if (targetUri == null)
					throw new ArgumentNullException ("targetUri");
				
				CFProxySettings settings = CFNetwork.GetSystemProxySettings ();
				CFProxy[] proxies = CFNetwork.GetProxiesForUri (targetUri, settings);
				
				if (proxies == null) {
					// if there are no proxies for target Uri, then we can assume it is bypassed
					return true;
				}
				
				for (int i = 0; i < proxies.Length; i++) {
					if (proxies[i].CanGetWebProxy) {
						// there is a supported proxy we can use for the targetUri, so not bypassed
						return false;
					}
				}
				
				// no supported proxies for the target Uri, will be bypassed
				return true;
			}
		}
		
		static CFWebProxy defaultWebProxy;
		public static IWebProxy GetDefaultProxy ()
		{
			if (defaultWebProxy == null)
				defaultWebProxy = new CFWebProxy ();
			
			return defaultWebProxy;
		}
	}
}
