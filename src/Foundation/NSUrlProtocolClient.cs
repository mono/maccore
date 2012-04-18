//
// NSUrlProtocol support
//
// Author:
//   Rolf Bjarne Kvinge
//
// Copyright 2012, Xamarin Inc.
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

using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {
	public sealed class NSUrlProtocolClient : NSObject
	{
		public NSUrlProtocolClient (IntPtr handle)
			: base (handle)
		{
		}
		
		static IntPtr selUrlProtocolWasRedirectedToRequestRedirectResponse_ = Selector.GetHandle ("URLProtocol:wasRedirectedToRequest:redirectResponse:");
		static IntPtr selUrlProtocolCachedResponseIsValid_ = Selector.GetHandle ("URLProtocol:cachedResponseIsValid:");
		static IntPtr selUrlProtocolDidReceiveResponseCacheStoragePolicy_ = Selector.GetHandle ("URLProtocol:didReceiveResponse:cacheStoragePolicy:");
		static IntPtr selUrlProtocolDidLoadData_ = Selector.GetHandle ("URLProtocol:didLoadData:");
		static IntPtr selUrlProtocolDidFinishLoading_ = Selector.GetHandle ("URLProtocolDidFinishLoading:");
		static IntPtr selUrlProtocolDidFailWithError_ = Selector.GetHandle ("URLProtocol:didFailWithError:");
		static IntPtr selUrlProtocolDidReceiveAuthenticationChallenge_ = Selector.GetHandle ("URLProtocol:didReceiveAuthenticationChallenge:");
		static IntPtr selUrlProtocolDidCancelAuthenticationChallenge_ = Selector.GetHandle ("URLProtocol:didCancelAuthenticationChallenge:");

		public void Redirected (NSUrlProtocol protocol, NSUrlRequest redirectedToEequest, NSUrlResponse redirectResponse)
		{
			Messaging.void_objc_msgSend_IntPtr_IntPtr_IntPtr (this.Handle, selUrlProtocolWasRedirectedToRequestRedirectResponse_, protocol.Handle, redirectedToEequest.Handle, redirectResponse.Handle);
		}

		public void CachedResponseIsValid (NSUrlProtocol protocol, NSCachedUrlResponse cachedResponse)
		{
			Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selUrlProtocolCachedResponseIsValid_, protocol.Handle, cachedResponse.Handle);
		}

		public void ReceivedResponse (NSUrlProtocol protocol, NSUrlResponse response, NSUrlCacheStoragePolicy policy)
		{
			Messaging.void_objc_msgSend_IntPtr_IntPtr_int (this.Handle, selUrlProtocolDidReceiveResponseCacheStoragePolicy_, protocol.Handle, response.Handle, (int)policy);
		}

		public void DataLoaded (NSUrlProtocol protocol, NSData data)
		{
			Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selUrlProtocolDidLoadData_, protocol.Handle, data.Handle);
		}

		public void FinishedLoading (NSUrlProtocol protocol)
		{
			Messaging.void_objc_msgSend_IntPtr (this.Handle, selUrlProtocolDidFinishLoading_, protocol.Handle);
		}

		public void FailedWithError (NSUrlProtocol protocol, NSError error)
		{
			Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selUrlProtocolDidFailWithError_, protocol.Handle, error.Handle);
		}

		public void ReceivedAuthenticationChallenge (NSUrlProtocol protocol, NSUrlAuthenticationChallenge challenge)
		{
			Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selUrlProtocolDidReceiveAuthenticationChallenge_, protocol.Handle, challenge.Handle);
		}

		public void CancelledAuthenticationChallenge (NSUrlProtocol protocol, NSUrlAuthenticationChallenge challenge)
		{
			Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selUrlProtocolDidCancelAuthenticationChallenge_, protocol.Handle, challenge.Handle);
		}
	}
}
