using System;
using System.Reflection;
using System.Collections;
using System.Runtime.InteropServices;

using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {

	public partial class NSTimer {
		
		public static NSTimer CreateRepeatingScheduledTimer (TimeSpan when, NSAction action)
		{
			return CreateScheduledTimer (when.TotalSeconds, new NSActionDispatcher (action), NSActionDispatcher.Selector, null, true);
		}

		public static NSTimer CreateRepeatingScheduledTimer (double seconds, NSAction action)
		{
			return CreateScheduledTimer (seconds, new NSActionDispatcher (action), NSActionDispatcher.Selector, null, true);
		}
		
		public static NSTimer CreateScheduledTimer (TimeSpan when, NSAction action)
		{
			return CreateScheduledTimer (when.TotalSeconds, new NSActionDispatcher (action), NSActionDispatcher.Selector, null, false);
		}

		public static NSTimer CreateScheduledTimer (double seconds, NSAction action)
		{
			return CreateScheduledTimer (seconds, new NSActionDispatcher (action), NSActionDispatcher.Selector, null, false);
		}

		public static NSTimer CreateRepeatingTimer (TimeSpan when, NSAction action)
		{
			return CreateTimer (when.TotalSeconds, new NSActionDispatcher (action), NSActionDispatcher.Selector, null, true);
		}

		public static NSTimer CreateRepeatingTimer (double seconds, NSAction action)
		{
			return CreateTimer (seconds, new NSActionDispatcher (action), NSActionDispatcher.Selector, null, true);
		}
		
		public static NSTimer CreateTimer (TimeSpan when, NSAction action)
		{
			return CreateTimer (when.TotalSeconds, new NSActionDispatcher (action), NSActionDispatcher.Selector, null, false);
		}

		public static NSTimer CreateTimer (double seconds, NSAction action)
		{
			return CreateTimer (seconds, new NSActionDispatcher (action), NSActionDispatcher.Selector, null, false);
		}
		
		public NSTimer (NSDate date, TimeSpan when, NSAction action, System.Boolean repeats)
			: this (date, when.TotalSeconds, new NSActionDispatcher (action), NSActionDispatcher.Selector, null, repeats)
		{
		}
	}
}
