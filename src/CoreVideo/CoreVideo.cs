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
		_1Monochrome    = 0x00000001,
		_2Indexed       = 0x00000002,
		_4Indexed       = 0x00000004,
		_8Indexed       = 0x00000008,
		_1IndexedGray_WhiteIsZero = 0x00000021,
		_2IndexedGray_WhiteIsZero = 0x00000022,
		_4IndexedGray_WhiteIsZero = 0x00000024,
		_8IndexedGray_WhiteIsZero = 0x00000028,
		_16BE555        = 0x00000010,
		_16LE555        = 'L555',
		_16LE5551       = '5551',
		_16BE565        = 'B565',
		_16LE565        = 'L565',
		_24RGB          = 0x00000018,
		_24BGR          = '24BG',
		_32ARGB         = 0x00000020,
		_32BGRA         = 'BGRA',
		_32ABGR         = 'ABGR',
		_32RGBA         = 'RGBA',
		_64ARGB         = 'b64a',
		_48RGB          = 'b48r',
		_32AlphaGray    = 'b32a',
		_16Gray         = 'b16g',
		_422YpCbCr8     = '2vuy',
		_4444YpCbCrA8   = 'v408',
		_4444YpCbCrA8R  = 'r408',
		_444YpCbCr8     = 'v308',
		_422YpCbCr16    = 'v216',
		_422YpCbCr10    = 'v210',
		_444YpCbCr10    = 'v410',
		_420YpCbCr8Planar = 'y420',
		_420YpCbCr8PlanarFullRange    = 'f420',
		_422YpCbCr_4A_8BiPlanar = 'a2vy',
		_420YpCbCr8BiPlanarVideoRange = '420v',
		_420YpCbCr8BiPlanarFullRange  = '420f',
		_422YpCbCr8_yuvs = 'yuvs',
		_422YpCbCr8FullRange = 'yuvf',
#endif
	}

	public enum CVOptionFlags {
		None = 0,
	}
}
