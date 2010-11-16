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
#if false
		CV1Monochrome    = 0x00000001,
		CV2Indexed       = 0x00000002,
		CV4Indexed       = 0x00000004,
		CV8Indexed       = 0x00000008,
		CV1IndexedGray_WhiteIsZero = 0x00000021,
		CV2IndexedGray_WhiteIsZero = 0x00000022,
		CV4IndexedGray_WhiteIsZero = 0x00000024,
		CV8IndexedGray_WhiteIsZero = 0x00000028,
		CV16BE555        = 0x00000010,
		CV16LE555        = 'L555',
		CV16LE5551       = '5551',
		CV16BE565        = 'B565',
		CV16LE565        = 'L565',
		CV24RGB          = 0x00000018,
		CV24BGR          = '24BG',
		CV32ARGB         = 0x00000020,
		CV32BGRA         = 'BGRA',
		CV32ABGR         = 'ABGR',
		CV32RGBA         = 'RGBA',
		CV64ARGB         = 'b64a',
		CV48RGB          = 'b48r',
		CV32AlphaGray    = 'b32a',
		CV16Gray         = 'b16g',
		CV422YpCbCr8     = '2vuy',
		CV4444YpCbCrA8   = 'v408',
		CV4444YpCbCrA8R  = 'r408',
		CV444YpCbCr8     = 'v308',
		CV422YpCbCr16    = 'v216',
		CV422YpCbCr10    = 'v210',
		CV444YpCbCr10    = 'v410',
		CV420YpCbCr8Planar = 'y420',
		CV420YpCbCr8PlanarFullRange    = 'f420',
		CV422YpCbCr_4A_8BiPlanar = 'a2vy',
		CV420YpCbCr8BiPlanarVideoRange = '420v',
		CV420YpCbCr8BiPlanarFullRange  = '420f',
		CV422YpCbCr8_yuvs = 'yuvs',
		CV422YpCbCr8FullRange = 'yuvf',
#endif
	}

	public enum CVOptionFlags {
		None = 0,
	}
}
