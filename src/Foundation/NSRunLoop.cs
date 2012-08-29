using System;
using System.Runtime.InteropServices;

namespace MonoMac.Foundation {
	public enum NSRunLoopMode {
		Default,
		Common,
#if MONOMAC
		ConnectionReply = 2,
		ModalPanel,
		EventTracking,
#else
		// iOS-specific Enums start in 100 to avoid conflicting with future extensions to MonoMac
		UITracking = 100,
#endif

		// If it is not part of these enumerations
		Other = 1000
	}
	
	public partial class NSRunLoop {
		static NSString GetRealMode (string mode)
		{
			if (mode == NSDefaultRunLoopMode)
				return NSDefaultRunLoopMode;
			else if (mode == NSRunLoopCommonModes)
				return NSRunLoopCommonModes;
			else if (mode == UITrackingRunLoopMode)
				return UITrackingRunLoopMode;
			else
				return new NSString (mode);
		}

		static NSString FromEnum (NSRunLoopMode mode)
		{
			switch (mode){
			case NSRunLoopMode.Common:
				return NSRunLoopCommonModes;
			case NSRunLoopMode.UITracking:
				return UITrackingRunLoopMode;
#if MONOMAC
			case NSRunLoopMode.ConnectionReply:
				return NSRunLoopConnectionReplyMode;
			case NSRunLoopMode.ModalPanel:
				return NSRunLoopModalPanelMode;
			case NSRunLoopMode.EventTracking:
				return NSRunLoopEventTracking;
#endif
	
			default:
			case NSRunLoopMode.Default:
				return NSDefaultRunLoopMode;
			}
		}
		
		[Obsolete ("Use AddTimer (NSTimer, NSRunLoopMode)")]
		public void AddTimer (NSTimer timer, string forMode)
		{
			AddTimer (timer, GetRealMode (forMode));
		}

		public void AddTimer (NSTimer timer, NSRunLoopMode forMode)
		{
			AddTimer (timer, FromEnum (forMode));
		}


		[Obsolete ("Use LimitDateForMode (NSRunLoopMode) instead")]
		public NSDate LimitDateForMode (string mode)
		{
			return LimitDateForMode (GetRealMode (mode));
		}

		public NSDate LimitDateForMode (NSRunLoopMode mode)
		{
			return LimitDateForMode (FromEnum (mode));
		}
		
		[Obsolete ("Use AcceptInputForMode (NSRunLoopMode, NSDate)")]
		public void AcceptInputForMode (string mode, NSDate limitDate)
		{
			AcceptInputForMode (GetRealMode (mode), limitDate);
		}
		
		public void AcceptInputForMode (NSRunLoopMode mode, NSDate limitDate)
		{
			AcceptInputForMode (FromEnum (mode), limitDate);
		}

		public void Stop ()
		{
			GetCFRunLoop ().Stop ();
		}

		public void WakeUp ()
		{
			GetCFRunLoop ().WakeUp ();
		}

		public bool RunUntil (NSRunLoopMode mode, NSDate limitDate)
		{
			return RunUntil (FromEnum (mode), limitDate);
		}

		public NSRunLoopMode CurrentRunLoopMode {
			get {
				var mode = CurrentMode;

				if (mode == NSDefaultRunLoopMode)
					return NSRunLoopMode.Default;
				if (mode == NSRunLoopCommonModes)
					return NSRunLoopMode.Common;
#if MONOMAC
				if (mode == NSRunLoopConnectionReplyMode)
					return NSRunLoopMode.Connection;
				if (mode == NSRunLoopModalPanelMode)
					return NSRunLoopMode.ModalPanel;
				if (mode == NSRunLoopEventTracking)
					return NSRunLoopMode.EventTracking;
#else
				if (mode == UITrackingRunLoopMode)
					return NSRunLoopMode.UITracking;
#endif
				return NSRunLoopMode.Other;
			}
		}
	}
}