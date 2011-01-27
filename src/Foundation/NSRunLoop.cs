using System;
using System.Runtime.InteropServices;

namespace MonoMac.Foundation {
	public enum NSRunLoopMode {
		Default,
		Common,

#if MONOMAC
		ConnectionReply,
		ModalPanel,
		EventTracking
#endif
	}
	
	public partial class NSRunLoop {
		static NSString GetRealMode (string mode)
		{
			if (mode == NSDefaultRunLoopMode)
				return NSDefaultRunLoopMode;
			else if (mode == NSRunLoopCommonModes)
				return NSRunLoopCommonModes;
			else
				return new NSString (mode);
		}

		static NSString FromEnum (NSRunLoopMode mode)
		{
			switch (mode){
			case NSRunLoopMode.Common:
				return NSRunLoopCommonModes;
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
		
		[Obsolete ("Use AddTimer (NSTimer, NSString)")]
		public void AddTimer (NSTimer timer, string forMode)
		{
			AddTimer (timer, GetRealMode (forMode));
		}

		public void AddTimer (NSTimer timer, NSRunLoopMode forMode)
		{
			AddTimer (timer, FromEnum (forMode));
		}


		[Obsolete ("Use LimitDateForMode (NSString) instead")]
		public NSDate LimitDateForMode (string mode)
		{
			return LimitDateForMode (GetRealMode (mode));
		}

		public NSDate LimitDateForMode (NSRunLoopMode mode)
		{
			return LimitDateForMode (FromEnum (mode));
		}
		
		[Obsolete ("Use AcceptInputForMode (NSString, NSDate)")]
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
	}
}