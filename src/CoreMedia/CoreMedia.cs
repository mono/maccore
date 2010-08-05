using System;
using System.Runtime.InteropServices;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreMedia {

	[StructLayout(LayoutKind.Sequential)]
	public struct CMTime {
		public long Value;
		public int TimeScale;
		public int TimeFlags;
		public long TimeEpoch;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CMTimeRange {
		public CMTime Start;
		public CMTime Duration;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CMTimeMapping {
		public CMTime Source;
		public CMTime Target;
	}
	
}