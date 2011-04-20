using System;
using System.Runtime.InteropServices;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreMedia {

	[StructLayout(LayoutKind.Sequential)]
	public struct CMTime {
		[FlagsAttribute]
		public enum Flags {
			Valid = 1,
			HasBeenRounded = 2,
			PositiveInfinity = 4,
			NegativeInfinity = 8,
			Indefinite = 16,
			ImpliedValueFlagsMask = 28
		}

		public static CMTime Invalid = new CMTime (0);
		const Flags kIndefinite = Flags.Valid | Flags.Indefinite;
		public static CMTime Indefinite = new CMTime (kIndefinite);
		const Flags kPositive = Flags.Valid | Flags.PositiveInfinity;
		public static CMTime PositiveInfinity = new CMTime (kPositive);
		const Flags kNegative = Flags.Valid | Flags.NegativeInfinity;
		public static CMTime NegativeInfinity = new CMTime (kNegative);
		public static CMTime Zero = new CMTime (Flags.Valid, 1);
		
		public const int MaxTimeScale = 0x7fffffff;
		public long Value;
		public int TimeScale;
		public Flags TimeFlags;
		public long TimeEpoch;

		CMTime (Flags f)
		{
			Value = 0;
			TimeScale = 0;
			TimeEpoch = 0;
			TimeFlags = f;
		}

		CMTime (Flags f, int timescale)
		{
			Value = 0;
			TimeScale = timescale;
			TimeEpoch = 0;
			TimeFlags = f;
		}
		       
		public CMTime (long value, int timescale)
		{
			Value = value;
			TimeScale = timescale;
			TimeFlags = Flags.Valid;
			TimeEpoch = 0;
		}

		public bool IsInvalid {
			get {
				return (TimeFlags & Flags.Valid) == 0;
			}
		}

		public bool IsIndefinite {
			get {
				return (TimeFlags & kIndefinite) == kIndefinite;
			}
		}

		public bool IsPositiveInfinity {
			get {
				return (TimeFlags & kPositive) == kPositive;
			}
		}

		public bool IsNegativeInfinity {
			get {
				return (TimeFlags & kNegative) == kNegative;
			}
		}

		// TODO: kCMTimeValueKey, TimeScaleKey, TimeEpockKey, TimeFlagsKey
	}
	
	public enum CMMediaType : uint
	{
		Video         = 1986618469, // 'vide'
		Audio         = 1936684398, // 'soun'
		Muxed         = 1836415096, // 'muxx'
		Text          = 1952807028, // 'text'
		ClosedCaption = 1668047728, // 'clcp'
		Subtitle      = 1935832172, // 'sbtl'
		TimeCode      = 1953325924, // 'tmcd'
		TimedMetadata = 1953326452, // 'tmet'
	}
	
	[StructLayout(LayoutKind.Sequential)]
	struct CMSampleTimingInfo
	{
		public CMTime Duration;
		public CMTime PresentationTimeStamp;
		public CMTime DecodeTimeStamp;
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