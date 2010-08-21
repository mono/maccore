//
// This file describes the API that the generator will produce
//
// Authors:
//   Geoff Norton
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
//
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
using MonoMac.CoreGraphics;
using MonoMac.CoreLocation;
using MonoMac.UIKit;
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
