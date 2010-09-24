//
// This file describes the API that the generator will produce
//
// Authors:
//   Geoff Norton
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
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
#if MONOMAC
using MonoMac.AppKit;
#else
using MonoMac.UIKit;
#endif
using System;
using System.Drawing;

namespace MonoMac.QuickLook {
	[Since (4,0)]
	[BaseType (typeof (UIViewController), Delegates = new string [] { "WeakDelegate" }, Events=new Type [] { typeof (QLPreviewControllerDelegate)})]
	interface QLPreviewController {
		[Export ("dataSource"), NullAllowed]
		NSObject WeakDataSource { get; set; }
		
		[Wrap ("WeakDataSource")]
		QLPreviewControllerDataSource DataSource { get; set;  }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		QLPreviewControllerDelegate Delegate { get; set; }
		
		[Export ("currentPreviewItemIndex")]
		int CurrentPreviewItemIndex  { get; set;  }

		[Export ("currentPreviewItem")]
		QLPreviewItem CurrentPreviewItem { get;  }

		[Static]
		[Export ("canPreviewItem:")]
		bool CanPreviewItem (QLPreviewItem item);

		[Export ("reloadData")]
		void ReloadData ();

		[Export ("refreshCurrentPreviewItem")]
		void RefreshCurrentPreviewItem ();
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	[Model]
	interface QLPreviewControllerDataSource {
		[Abstract]
		[Export ("numberOfPreviewItemsInPreviewController:")]
		int PreviewItemCount (QLPreviewController controller);

		[Abstract]
		[Export ("previewController:previewItemAtIndex:")]
		QLPreviewItem GetPreviewItem (QLPreviewController controller, int index);
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	[Model]
	interface QLPreviewControllerDelegate {
		[Export ("previewControllerWillDismiss:")]
		void WillDismiss (QLPreviewController controller);

		[Export ("previewControllerDidDismiss:")]
		void DidDismiss (QLPreviewController controller);

		[Export ("previewController:shouldOpenURL:forPreviewItem:"), EventArgs ("QLOpenUrl"), DefaultValue (false)]
		bool ShouldOpenUrl (QLPreviewController controller, NSUrl url, QLPreviewItem item);

	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	[Model]
	interface QLPreviewItem {
		[Abstract]
		[Export ("previewItemURL")]
		NSUrl ItemUrl { get; }

		[Abstract]
		[Export ("previewItemTitle")]
		string ItemTitle { get; }
	}
}
