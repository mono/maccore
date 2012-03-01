//
// quicklookclient.cs: Binds the quicklook thumbnail client function
//
// Author:
//   Jean-Louis Fuchs
//
// Copyright 2012
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

using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
using MonoMac.CoreGraphics;
using MonoMac.CoreFoundation;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MonoMac.QuickLook {
	
	public static class Client
	{	
		static IntPtr QuickLook_libraryHandle = Dlfcn.dlopen (Constants.QuickLookLibrary, 0);
		static NSString _ThumbnailOptionIconModeKey;
		public static NSString ThumbnailOptionIconModeKey {
			get {
				if (_ThumbnailOptionIconModeKey == null)
					_ThumbnailOptionIconModeKey = Dlfcn.GetStringConstant (QuickLook_libraryHandle, "kQLThumbnailOptionIconModeKey");
				return _ThumbnailOptionIconModeKey;
			}
		}
		
		static NSString _ThumbnailOptionScaleFactorKey;
		public static NSString ThumbnailOptionScaleFactorKey {
			get {
				if (_ThumbnailOptionScaleFactorKey == null)
					_ThumbnailOptionScaleFactorKey = Dlfcn.GetStringConstant (QuickLook_libraryHandle, "kQLThumbnailOptionScaleFactorKey");
				return _ThumbnailOptionScaleFactorKey;
			}
		}
		
		[DllImport(Constants.QuickLookLibrary)]
		private extern static IntPtr QLThumbnailImageCreate (IntPtr allocator, IntPtr url, System.Drawing.SizeF maxThumbnailSize, IntPtr options);
		
		public static CGImage ThumbnailImageCreate (CFUrl url, System.Drawing.SizeF size, NSDictionary options)
		{
			//You can cast NSDictionary to CFDictionary
			IntPtr img = QLThumbnailImageCreate (IntPtr.Zero, url.Handle, size, options.Handle);
			return new CGImage (img);
		}
		
	}
}
