//
// Copyright 2009, Novell, Inc.
//
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
using System;
using System.Collections.Generic;

namespace MonoMac.Foundation {

	[Register]
	internal class InternalNSNotificationHandler : NSObject  {
		NSNotificationCenter notificationCenter;
		Action<NSNotification> notify;
		
		public InternalNSNotificationHandler (NSNotificationCenter notificationCenter, Action<NSNotification> notify)
		{
			this.notificationCenter = notificationCenter;
			this.notify = notify;
		}
		
		[Export ("post:")]
		[Preserve (Conditional = true)]
		public void Post (NSNotification s)
		{
			notify (s);
			s.Dispose ();
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing && notificationCenter != null){
				notificationCenter.RemoveObserver (this);
				notificationCenter = null;
			}
			base.Dispose (disposing);
		}
	}

	// The C# overloads
	public partial class NSNotificationCenter {
		static Selector postSelector = new Selector ("post:");
		List <NSObject> __mt_ObserverList_var = new List <NSObject> ();

		[Obsolete ("Use AddObserver(NSString, Action<NSNotification>, NSObject)")]
		public NSObject AddObserver (string aName, Action<NSNotification> notify, NSObject fromObject)
		{
			return AddObserver (new NSString (aName), notify, fromObject);
		}
		
		public NSObject AddObserver (NSString aName, Action<NSNotification> notify, NSObject fromObject)
		{
			if (notify == null)
				throw new ArgumentNullException ("notify");
			
			var proxy = new InternalNSNotificationHandler (this, notify);
			
			AddObserver (proxy, postSelector, aName, fromObject);

			return proxy;
		}

		public NSObject AddObserver (NSString aName, Action<NSNotification> notify)
		{
			return AddObserver (aName, notify, null);
		}

		[Obsolete ("Use AddObserver(NSString, Action<NSNotification>) instead")]
		public NSObject AddObserver (string aName, Action<NSNotification> notify)
		{
			return AddObserver (aName, notify, null);
		}

		[Obsolete ("Use AddObserver(NSObject, Selector, NSString, NSObject) instead")]
		public void AddObserver (NSObject observer, Selector aSelector, string aname, NSObject anObject)
		{
			AddObserver (observer, aSelector, new NSString (aname), anObject);
		}

		public void RemoveObservers (IEnumerable<NSObject> keys)
		{
			if (keys == null)
				return;
			foreach (var k in keys)
				RemoveObserver (k);
		}
	}

	public class NSNotificationEventArgs : EventArgs {
		public NSNotification Notification { get; private set; }
		public NSNotificationEventArgs (NSNotification notification)
		{
			Notification = notification;
		}
	}
}
