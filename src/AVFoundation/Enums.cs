// Copyright 2009, Novell, Inc.
// Copyright 2010, Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//
using System;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.AVFoundation {

	public enum AVAudioQuality {
		Min = 0,
		Low = 0x20,
		Medium = 0x40,
		High = 0x60,
		Max = 0x7F
	}

	public static class AVAudioSettings {

		public static readonly NSString AVFormatIDKey = new NSString ("AVFormatIDKey");
		public static readonly NSString AVSampleRateKey = new NSString ("AVSampleRateKey");
		public static readonly NSString AVNumberOfChannelsKey = new NSString ("AVNumberOfChannelsKey");

		public static readonly NSString AVLinearPCMBitDepthKey = new NSString ("AVLinearPCMBitDepthKey");
		public static readonly NSString AVLinearPCMIsBigEndianKey = new NSString ("AVLinearPCMIsBigEndianKey");
		public static readonly NSString AVLinearPCMIsFloatKey = new NSString ("AVLinearPCMIsFloatKey");

		public static readonly NSString AVEncoderAudioQualityKey = new NSString ("AVEncoderAudioQualityKey");
		public static readonly NSString AVEncoderBitRateKey = new NSString ("AVEncoderBitRateKey");
		public static readonly NSString AVEncoderBitDepthHintKey = new NSString ("AVEncoderBitDepthHintKey");

		public static readonly NSString AVSampleRateConverterAudioQualityKey = new NSString ("AVSampleRateConverterAudioQualityKey");
	}

	[Since (4,0)]
	public enum AVAssetExportSessionStatus {
		Unknown,
		Waiting,
		Exporting,
		Completed,
		Failed,
		Cancelled
	}

	[Since (4,0)]
	public enum AVAssetReaderStatus {
		Unknown = 0,
		Reading,
		Completed,
		Failed,
		Cancelled,
	}

	[Since (4,1)]
	public enum AVAssetWriterStatus {
		Unknown = 0,
		Writing,
		Completed,
		Failed,
		Cancelled,
	}
	
	[Since (4,0)]
	public enum AVCaptureVideoOrientation {
		Portrait = 1,
		PortraitUpsideDown,
		LandscapeLeft,
		LandscapeRight,
	}

	[Since (4,0)]
	public enum AVCaptureFlashMode {
		Off, On, Auto
	}

	[Since (4,0)]
	public enum AVCaptureTorchMode {
		Off, On, Auto
	}

	[Since (4,0)]
	public enum AVCaptureFocusMode {
		ModeLocked,
		ModeAutoFocus,
		ModeContinuousAutoFocus
	}

	[Since (4,0)]
	public enum AVCaptureDevicePosition {
		Back = 1,
		Front = 2
	}
	
	[Since (4,0)]
	public enum AVCaptureExposureMode {
		Locked, AutoExpose, ContinuousAutoExposure
	}

	[Since (4,0)]
	public enum AVCaptureWhiteBalanceMode {
		Locked, AutoWhiteBalance, ContinuousAutoWhiteBalance
	}

	[Flags]
	[Since (4,0)]
	public enum AVAudioSessionInterruptionFlags {
		ShouldResume = 1
	}

	public enum AVError {
		Unknown = -11800,
		OutOfMemory = -11801,
		SessionNotRunning = -11803,
		DeviceAlreadyUsedByAnotherSession = -11804,
		NoDataCaptured = -11805,
		SessionConfigurationChanged = -11806,
		DiskFull = -11807,
		DeviceWasDisconnected = -11808,
		MediaChanged = -11809,
		MaximumDurationReached = -11810,
		MaximumFileSizeReached = -11811,
		MediaDiscontinuity = -11812,
		MaximumNumberOfSamplesForFileFormatReached = -11813,
		DeviceNotConnected = -11814,
		DeviceInUseByAnotherApplication = -11815,
		DeviceLockedForConfigurationByAnotherProcess = -11817,
		SessionWasInterrupted = -11818,
		DecodeFailed = -11821,
		ExportFailed = -11820,
		FileAlreadyExists = -11823,
		InvalidSourceMedia = - 11822,
		CompositionTrackSegmentsNotContiguous = -11824,
		ContentIsProtected = -11831,
		FailedToParse = -11829,
		FormatNotRecognized = -11828,
		InvalidCompositionTrackSegmentDuration = -11825,
		InvalidCompositionTrackSegmentSourceStartTime= -11826,
		InvalidCompositionTrackSegmentSourceDuration = -11827,
		MaximumStillImageCaptureRequestsExceeded = 11830,
		NoImageAtTime = -11832,
		DecoderNotFound = -11833,
		EncoderNotFound = -11834,
		ContentIsNotAuthorized = -11836,
#if !MONOMAC
		DeviceIsNotAvailableInBackground = -11837,
		MediaServicesWereReset = -11820,
#endif
	}

	[Since (4,0)]
	public enum AVPlayerActionAtItemEnd {
		Pause = 1, None
	}

	[Since (4,0)]
	public enum AVPlayerItemStatus {
		Unknown, ReadyToPlay, Failed
	}

	[Flags]
	[Since (4,0)]
	public enum AVAudioSessionFlags {
		NotifyOthersOnDeactivation = 1
	}

	[Since (4,0)]
	public enum AVKeyValueStatus {
		Unknown, Loading, Loaded, Failed, Cancelled
	}

	[Since (4,0)]
	public enum AVPlayerStatus {
		Unknown,
		ReadyToPlay,
		Failed
	}
}
