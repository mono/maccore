//
// Copyright 2009, Novell, Inc.
//
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
using System;
using System.Collections.Generic;

namespace MonoMac.Foundation {

	[Register]
	internal class NSNotificationHandler : NSObject  {
		Action<NSNotification> notify;
		
		public NSNotificationHandler (Action<NSNotification> notify)
		{
				this.notify = notify;
		}
		
		[Export ("post:")]
		[Preserve (Conditional = true)]
		public void Post (NSNotification s)
		{
			notify (s);
			s.Dispose ();
		}
	}

	// The C# overloads
	public partial class NSNotificationCenter {
		static Selector postSelector = new Selector ("post:");
		List <NSObject> __mt_ObserverList_var = new List <NSObject> ();

		public NSObject AddObserver (string aName, Action<NSNotification> notify, NSObject fromObject)
		{
			if (notify == null)
				throw new ArgumentNullException ("notify");
			
			var proxy = new NSNotificationHandler (notify);
			
			AddObserver (proxy, postSelector, aName, fromObject);

			return proxy;
		}

		public NSObject AddObserver (string aName, Action<NSNotification> notify)
		{
			return AddObserver (aName, notify, null);
		}
	}
		
}
