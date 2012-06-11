// 
// CoreMedia.cs: Basic definitions for CoreMedia
//
// Authors: Mono Team
//
// Copyright 2010-2011 Novell Inc
// Copyright 2012 Xamarin Inc
//
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
		
		public CMTime (long value, int timescale, long epoch)
		{
			Value = value;
			TimeScale = timescale;
			TimeFlags = Flags.Valid;
			TimeEpoch = epoch;
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
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeAbsoluteValue (CMTime time);
		
		public CMTime AbsoluteValue {
			get {
				return CMTimeAbsoluteValue (this);
			}
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static int CMTimeCompare (CMTime time1, CMTime time2);
		public static int Compare (CMTime time1, CMTime time2)
		{
			return CMTimeCompare (time1, time2);
		}
		
		public static bool operator == (CMTime time1, CMTime time2)
		{
			return Compare (time1, 	time2) == 0;
		}
		
		public static bool operator != (CMTime time1, CMTime time2)
		{
			return !(time1 == time2);
		}
		
		public override bool Equals (object obj)
		{
			if (!(obj is CMTime))
				return false;
			
			CMTime other = (CMTime) obj;
			return other == this;
		}
		
		public override int GetHashCode ()
		{
			return Value.GetHashCode () ^ TimeScale.GetHashCode () ^ TimeFlags.GetHashCode () ^ TimeEpoch.GetHashCode ();
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeAdd (CMTime addend1, CMTime addend2);
		public static CMTime Add (CMTime time1, CMTime time2)
		{
			return CMTimeAdd (time1, time2);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeSubtract (CMTime minuend, CMTime subtraend);
		public static CMTime Subtract (CMTime minuend, CMTime subtraend)
		{
			return CMTimeSubtract (minuend, subtraend);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeMultiply (CMTime time, int multiplier);
		public static CMTime Multiply (CMTime time, int multiplier)
		{
			return CMTimeMultiply (time, multiplier);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeMultiplyByFloat64 (CMTime time, double multiplier);
		public static CMTime Multiply (CMTime time, double multiplier)
		{
			return CMTimeMultiplyByFloat64 (time, multiplier);
		}
		
		public static CMTime operator + (CMTime time1, CMTime time2)
		{
			return Add (time1, time2);
		}
		
		public static CMTime operator - (CMTime minuend, CMTime subtraend)
		{
			return Subtract (minuend, subtraend);
		}
		
		public static CMTime operator * (CMTime time, int multiplier)
		{
			return Multiply (time, multiplier);
		}
		
		public static CMTime operator * (CMTime time, double multiplier)
		{
			return Multiply (time, multiplier);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeConvertScale (CMTime time, int newScale, CMTimeRoundingMethod method);
		public CMTime ConvertScale (int newScale, CMTimeRoundingMethod method)
		{
			return CMTimeConvertScale (this, newScale, method);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static double CMTimeGetSeconds (CMTime time);
		public double Seconds {
			get {
				return CMTimeGetSeconds (this);
			}
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeMakeWithSeconds (double seconds, int preferredTimeScale);
		public static CMTime FromSeconds (double seconds, int preferredTimeScale)
		{
			return CMTimeMakeWithSeconds (seconds, preferredTimeScale);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeMaximum (CMTime time1, CMTime time2);
		public static CMTime GetMaximum (CMTime time1, CMTime time2)
		{
			return CMTimeMaximum (time1, time2);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeMinimum (CMTime time1, CMTime time2);
		public static CMTime GetMinimum (CMTime time1, CMTime time2)
		{
			return CMTimeMinimum (time1, time2);
		}
		
		public readonly static NSString ValueKey;
		public readonly static NSString ScaleKey;
		public readonly static NSString EpochKey;
		public readonly static NSString FlagsKey;
		
		static CMTime ()
		{
			var lib = Dlfcn.dlopen (Constants.CoreMediaLibrary, 0);
			if (lib != IntPtr.Zero) {
				try {
					ValueKey  = Dlfcn.GetStringConstant (lib, "kCMTimeValueKey");
					ScaleKey  = Dlfcn.GetStringConstant (lib, "kCMTimeScaleKey");
					EpochKey  = Dlfcn.GetStringConstant (lib, "kCMTimeEpochKey");
					FlagsKey  = Dlfcn.GetStringConstant (lib, "kCMTimeFlagsKey");
				} finally {
					Dlfcn.dlclose (lib);
				}
			}
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMTimeCopyAsDictionary (CMTime time, IntPtr allocator);
		public IntPtr AsDictionary {
			get {
				return CMTimeCopyAsDictionary (this, IntPtr.Zero);
			}
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMTimeCopyDescription (IntPtr allocator, CMTime time);
		public string Description {
			get {
				return NSString.FromHandle (CMTimeCopyDescription (IntPtr.Zero, this)).ToString ();
			}
		}
		
		public override string ToString ()
		{
			return Description;
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimeMakeFromDictionary (IntPtr dict);
		public static CMTime FromDictionary (IntPtr dict)
		{
			return CMTimeMakeFromDictionary (dict);
		}
		
		// Should we also bind CMTimeShow?
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
	
	public enum CMTimeRoundingMethod 
	{
		RoundHalfAwayFromZero = 1,
		RoundTowardZero = 2,
		RoundAwayFromZero = 3,
		QuickTime = 4,
		RoundTowardPositiveInfinity = 5,
		RoundTowardNegativeInfinity = 6,
		Default = RoundHalfAwayFromZero,
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

	public enum CMVideoCodecType 
	{
		YUV422YpCbCr8    = 0x32767579,
		Animation        = 0x726c6520,
		Cinepak          = 0x63766964,
		JPEG             = 0x6a706567,
		JPEG_OpenDML     = 0x646d6231,
		SorensonVideo    = 0x53565131,
		SorensonVideo3   = 0x53565133,
		H263             = 0x68323633,
		H264             = 0x61766331,
		Mpeg4Video       = 0x6d703476,
		Mpeg2Video       = 0x6d703276,
		Mpeg1Video       = 0x6d703176,
		DvcNtsc          = 0x64766320,
		DvcPal           = 0x64766370,
		DvcProPal        = 0x64767070,
		DvcPro50NTSC     = 0x6476356e,
		DvcPro50PAL      = 0x64763570,
		DvcProHD720p60   = 0x64766870,
		DvcProHD720p50   = 0x64766871,
		DvcProHD1080i60  = 0x64766836,
		DvcProHD1080i50  = 0x64766835,
		DvcProHD1080p30  = 0x64766833,
		DvcProHD1080p25  = 0x64766832,
		AppleProRes4444  = 0x61703468,
		AppleProRes422HQ = 0x61706368,
		AppleProRes422   = 0x6170636e,
		AppleProRes422LT = 0x61706373,
		AppleProRes422Proxy = 0x6170636f,
	}
}