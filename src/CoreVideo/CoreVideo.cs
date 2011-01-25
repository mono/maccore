// 
// CoreVideo.cs: 
//
// Authors: Mono Team
//     
// Copyright 2010 Novell, Inc
//
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreVideo {
	public enum CVAttachmentMode : uint {
		ShouldNotPropagate    = 0,
		ShouldPropagate       = 1,
	}

	[Flags]
	public enum CVPixelBufferLock {
		ReadOnly = 0x00000001,
	}	

	public struct CVPlanarComponentInfo {
		public int Offset;
		public uint RowBytes;
	}

	public struct CVPlanarPixelBufferInfo {
		public CVPlanarComponentInfo[] ComponentInfo;
	}

	public struct CVPlanarPixelBufferInfo_YCbCrPlanar {
		public CVPlanarComponentInfo ComponentInfoY;
		public CVPlanarComponentInfo ComponentInfoCb;
		public CVPlanarComponentInfo ComponentInfoCr;
	}

	public enum CVReturn {
		Success = 0,
		First = -6660,
		Error = -6660,
		InvalidArgument = -6661,
		AllocationFailed = -6662,
		InvalidDisplay = -6670,
		DisplayLinkAlreadyRunning = -6671,
		DisplayLinkNotRunning = -6672,
		DisplayLinkCallbacksNotSet = -6673,
		InvalidPixelFormat = -6680,
		InvalidSize = -6681,
		InvalidPixelBufferAttributes = -6682,
		PixelBufferNotOpenGLCompatible = -6683,
		PoolAllocationFailed = -6690,
		InvalidPoolAttributes = -6691,
		Last = -6699,
	}

	public enum CVPixelFormatType : uint {
		// FIXME: These all start with integers; what should we do here?
		CV1Monochrome    = 0x00000001,
		CV2Indexed       = 0x00000002,
		CV4Indexed       = 0x00000004,
		CV8Indexed       = 0x00000008,
		CV1IndexedGray_WhiteIsZero = 0x00000021,
		CV2IndexedGray_WhiteIsZero = 0x00000022,
		CV4IndexedGray_WhiteIsZero = 0x00000024,
		CV8IndexedGray_WhiteIsZero = 0x00000028,
		CV16BE555        = 0x00000010,
		CV24RGB          = 0x00000018,
		CV32ARGB         = 0x00000020,
		CV16LE555        = 0x4c353535,
		CV16LE5551       = 0x35353531,
		CV16BE565        = 0x42353635,
		CV16LE565        = 0x4c353635,
		CV24BGR          = 0x32344247,
		CV32BGRA         = 0x42475241,
		CV32ABGR         = 0x41424752,
		CV32RGBA         = 0x52474241,
		CV64ARGB         = 0x62363461,
		CV48RGB          = 0x62343872,
		CV32AlphaGray    = 0x62333261,
		CV16Gray         = 0x62313667,
		CV422YpCbCr8     = 0x32767579,
		CV4444YpCbCrA8   = 0x76343038,
		CV4444YpCbCrA8R  = 0x72343038,
		CV444YpCbCr8     = 0x76333038,
		CV422YpCbCr16    = 0x76323136,
		CV422YpCbCr10    = 0x76323130,
		CV444YpCbCr10    = 0x76343130,
		CV420YpCbCr8Planar = 0x79343230,
		CV420YpCbCr8PlanarFullRange    = 0x66343230,
		CV422YpCbCr_4A_8BiPlanar = 0x61327679,
		CV420YpCbCr8BiPlanarVideoRange = 0x34323076,
		CV420YpCbCr8BiPlanarFullRange  = 0x34323066,
		CV422YpCbCr8_yuvs = 0x79757673,
		CV422YpCbCr8FullRange = 0x79757666,
		CV30RGB = 0x5231306b,
		CV4444AYpCbCr8   = 0x79343038,
		CV4444AYpCbCr16  = 0x79343136,
	}

	public enum CVOptionFlags {
		None = 0,
	}

	public struct CVTimeStamp {
		public UInt32	Version;
		public Int32 	VideoTimeScale;
		public Int64 	VideoTime;
		public UInt64 	HostTime;
		public double 	RateScalar;
		public Int64 	VideoRefreshPeriod;
		public double 	SMPTETime;
		public UInt64 	Flags;
		public UInt64 	Reserved;
	}
	
}
