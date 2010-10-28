//
// AVFoundation.cs: This file describes the API that the generator will produce for AVFoundation
//
// Authors:
//   Miguel de Icaza
//
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
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
using MonoMac.CoreMedia;
using MonoMac.CoreGraphics;
using MonoMac.CoreAnimation;
using System;
using System.Drawing;

namespace MonoMac.AVFoundation {
	[Since (4,0)]
	[BaseType (typeof (NSObject))][Static]
	interface AVMediaType {
		[Field ("AVMediaTypeVideo")]
		NSString Video { get; }
		
		[Field ("AVMediaTypeAudio")]
		NSString Audio { get; }

		[Field ("AVMediaTypeText")]
		NSString Text { get; }

		[Field ("AVMediaTypeClosedCaption")]
		NSString ClosedCaption { get; }

		[Field ("AVMediaTypeSubtitle")]
		NSString Subtitle { get; }

		[Field ("AVMediaTypeTimecode")]
		NSString Timecode { get; }

		[Field ("AVMediaTypeTimedMetadata")]
		NSString TimedMetadata { get; }

		[Field ("AVMediaTypeMuxed")]
		NSString Muxed { get; }
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))][Static]
	interface AVMediaCharacteristic {
		[Field ("AVMediaCharacteristicVisual")]
		NSString Visual { get; }

		[Field ("AVMediaCharacteristicAudible")]
		NSString Audible { get; }

		[Field ("AVMediaCharacteristicLegible")]
		NSString Legible { get; }

		[Field ("AVMediaCharacteristicFrameBased")]
		NSString FrameBased { get; }
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))][Static]
	interface AVFileType {
		[Field ("AVFileTypeQuickTimeMovie")]
		NSString QuickTimeMovie { get; }
		
		[Field ("AVFileTypeMpeg4")]
		NSString Mpeg4 { get; }
		
		[Field ("AVFileTypeAppleM4V")]
		NSString AppleM4V { get; }
		
		[Field ("AVFileType3Gpp")]
		NSString ThreeGpp { get; }
		
		[Field ("AVFileTypeAppleM4A")]
		NSString AppleM4A { get; }
		
		[Field ("AVFileTypeCoreAudioFormat")]
		NSString CoreAudioFormat { get; }
		
		[Field ("AVFileTypeWave")]
		NSString Wave { get; }
		
		[Field ("AVFileTypeAiff")]
		NSString Aiff { get; }
		
		[Field ("AVFileTypeAifc")]
		NSString Aifc { get; }
		
		[Field ("AVFileTypeAmr")]
		NSString Amr { get; }
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))][Static]
	interface AVVideo {
		[Field ("AVVideoCodecKey")]
		NSString CodecKey { get; }
		
		[Field ("AVVideoCodecH264")]
		NSString CodecH264 { get; }
		
		[Field ("AVVideoCodecJPEG")]
		NSString CodecJPEG { get; }
		
		[Field ("AVVideoWidthKey")]
		NSString WidthKey { get; }
		
		[Field ("AVVideoHeightKey")]
		NSString HeightKey { get; }
		
		[Field ("AVVideoCompressionPropertiesKey")]
		NSString CompressionPropertiesKey { get; }
		
		[Field ("AVVideoAverageBitRateKey")]
		NSString AverageBitRateKey { get; }
		
		[Field ("AVVideoMaxKeyFrameIntervalKey")]
		NSString MaxKeyFrameIntervalKey { get; }
		
		[Field ("AVVideoProfileLevelKey")]
		NSString ProfileLevelKey { get; }
		
		[Field ("AVVideoProfileLevelH264Baseline30")]
		NSString ProfileLevelH264Baseline30 { get; }
		
		[Field ("AVVideoProfileLevelH264Baseline31")]
		NSString ProfileLevelH264Baseline31 { get; }
		
		[Field ("AVVideoProfileLevelH264Main30")]
		NSString ProfileLevelH264Main30 { get; }
		
		[Field ("AVVideoProfileLevelH264Main31")]
		NSString ProfileLevelH264Main31 { get; }
		
		[Field ("AVVideoPixelAspectRatioKey")]
		NSString PixelAspectRatioKey { get; }
		
		[Field ("AVVideoPixelAspectRatioHorizontalSpacingKey")]
		NSString PixelAspectRatioHorizontalSpacingKey { get; }
		
		[Field ("AVVideoPixelAspectRatioVerticalSpacingKey")]
		NSString PixelAspectRatioVerticalSpacingKey { get; }
		
		[Field ("AVVideoCleanApertureKey")]
		NSString CleanApertureKey { get; }
		
		[Field ("AVVideoCleanApertureWidthKey")]
		NSString CleanApertureWidthKey { get; }
		
		[Field ("AVVideoCleanApertureHeightKey")]
		NSString CleanApertureHeightKey { get; }
		
		[Field ("AVVideoCleanApertureHorizontalOffsetKey")]
		NSString CleanApertureHorizontalOffsetKey { get; }
		
		[Field ("AVVideoCleanApertureVerticalOffsetKey")]
		NSString CleanApertureVerticalOffsetKey { get; }
	}	
	
	[BaseType (typeof (NSObject))]
	interface AVAudioPlayer {
		[Export ("initWithContentsOfURL:error:")][Internal]
		IntPtr Constructor (NSUrl url, IntPtr outError);
	
		[Export ("initWithData:error:")][Internal]
		IntPtr Constructor (NSData  data, IntPtr outError);
	
		[Export ("prepareToPlay")]
		bool PrepareToPlay ();
	
		[Export ("play")]
		bool Play ();
	
		[Export ("pause")]
		void Pause ();
	
		[Export ("stop")]
		void Stop ();
	
		[Export ("playing")]
		bool Playing { [Bind ("isPlaying")] get;  }
	
		[Export ("numberOfChannels")]
		uint NumberOfChannels { get;  }
	
		[Export ("duration")]
		double Duration { get;  }
	
		[Export ("delegate", ArgumentSemantic.Assign)]
		NSObject WeakDelegate { get; set;  }

		[Wrap ("WeakDelegate")]
		AVAudioPlayerDelegate Delegate { get; set; }
	
		[Export ("url")]
		NSUrl Url { get;  }
	
		[Export ("data")]
		NSData Data { get;  }
	
		[Export ("volume")]
		float Volume { get; set;  }
	
		[Export ("currentTime")]
		double CurrentTime { get; set;  }
	
		[Export ("numberOfLoops")]
		int NumberOfLoops { get; set;  }
	
		[Export ("meteringEnabled")]
		bool MeteringEnabled { [Bind ("isMeteringEnabled")] get; set;  }
	
		[Export ("updateMeters")]
		void UpdateMeters ();
	
		[Export ("peakPowerForChannel:")]
		float PeakPower (uint channelNumber);
	
		[Export ("averagePowerForChannel:")]
		float AveragePower (uint channelNumber);

		[Since (4,0)]
		[Export ("deviceCurrentTime")]
		double DeviceCurrentTime { get;  }

		[Export ("pan")]
		float Pan { get; set; }

		[Since (4,0)]
		[Export ("playAtTime:")]
		bool PlayAtTimetime (double time);

		[Since (4,0)]
		[Export ("settings")]
		NSDictionary Settings { get;  }
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	interface AVAudioPlayerDelegate {
		[Export ("audioPlayerDidFinishPlaying:successfully:")]
		void FinishedPlaying (AVAudioPlayer player, bool flag);
	
		[Export ("audioPlayerDecodeErrorDidOccur:error:")]
		void DecoderError (AVAudioPlayer player, NSError  error);
	
		[Export ("audioPlayerBeginInterruption:")]
		void BeginInterruption (AVAudioPlayer  player);
	
		[Export ("audioPlayerEndInterruption:")]
		void EndInterruption (AVAudioPlayer player);

		[Since (4,0)]
		[Export ("audioPlayerEndInterruption:withFlags:")]
		void EndInterruption (AVAudioPlayer player, AVAudioSessionInterruptionFlags flags);
	}

	[BaseType (typeof (NSObject))]
	interface AVAudioRecorder {
		[Export ("initWithURL:settings:error:")][Internal]
		IntPtr Constructor (NSUrl url, NSDictionary settings, IntPtr outError);
	
		[Export ("prepareToRecord")]
		bool PrepareToRecord ();
	
		[Export ("record")]
		bool Record ();
	
		[Export ("recordForDuration:")]
		bool RecordFor (double duration);
	
		[Export ("pause")]
		void Pause ();
	
		[Export ("stop")]
		void Stop ();
	
		[Export ("deleteRecording")]
		bool DeleteRecording ();
	
		[Export ("recording")]
		bool Recording { [Bind ("isRecording")] get;  }
	
		[Export ("url")]
		NSUrl Url { get;  }
	
		[Export ("settings")]
		NSDictionary Settings { get;  }
	
		[Export ("delegate")]
		NSObject WeakDelegate { get; set;  }

		[Wrap ("WeakDelegate")]
		AVAudioRecorderDelegate Delegate { get; set;  }
	
		[Export ("currentTime")]
		double currentTime { get; }
	
		[Export ("meteringEnabled")]
		bool MeteringEnabled { [Bind ("isMeteringEnabled")] get; set;  }
	
		[Export ("updateMeters")]
		void UpdateMeters ();
	
		[Export ("peakPowerForChannel:")]
		float PeakPower (uint channelNumber);
	
		[Export ("averagePowerForChannel:")]
		float AveragePower (uint channelNumber);
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	interface AVAudioRecorderDelegate {
		[Export ("audioRecorderDidFinishRecording:successfully:")]
		void FinishedRecording (AVAudioRecorder recorder, bool flag);
	
		[Export ("audioRecorderEncodeErrorDidOccur:error:")]
		void EncoderError (AVAudioRecorder recorder, NSError  error);
	
		[Export ("audioRecorderBeginInterruption:")]
		void BeginInterruption (AVAudioRecorder  recorder);
	
		[Export ("audioRecorderEndInterruption:")]
		void EndInterruption (AVAudioRecorder  recorder);

		[Since (4,0)]
		[Export ("audioRecorderEndInterruption:withFlags:")]
		void EndInterruption (AVAudioRecorder recorder, AVAudioSessionInterruptionFlags flags);
	}
	
	[BaseType (typeof (NSObject))]
	interface AVAudioSession {
		[Export ("sharedInstance"), Static]
		AVAudioSession SharedInstance ();
	
		[Export ("delegate")]
		NSObject WeakDelegate { get; set;  }

		[Wrap ("WeakDelegate")]
		AVAudioSessionDelegate Delegate { get; set;  }
	
		[Export ("setActive:error:"), Internal]
		bool SetActive (bool beActive, IntPtr outError);

		[Export ("setActive:withFlags:error:"), Internal]
		[Since (4,0)]
		bool _SetActive (bool beActive, int flags, IntPtr outError);

		[Export ("setCategory:error:"), Internal]
		bool SetCategory (string theCategory, IntPtr outError);
	
		[Export ("setPreferredHardwareSampleRate:error:"), Internal]
		bool SetPreferredHardwareSampleRate (double sampleRate, IntPtr outError);
	
		[Export ("setPreferredIOBufferDuration:error:"), Internal]
		bool SetPreferredIOBufferDuration (double duration, IntPtr outError);
	
		[Export ("category")]
		string Category { get;  }
	
		[Export ("preferredHardwareSampleRate")]
		double PreferredHardwareSampleRate { get;  }
	
		[Export ("preferredIOBufferDuration")]
		double PreferredIOBufferDuration { get;  }
	
		[Export ("inputIsAvailable")]
		bool InputIsAvailable { get;  }
	
		[Export ("currentHardwareSampleRate")]
		double CurrentHardwareSampleRate { get;  }
	
		[Export ("currentHardwareInputNumberOfChannels")]
		int currentHardwareInputNumberOfChannels { get;  }
	
		[Export ("currentHardwareOutputNumberOfChannels")]
		int CurrentHardwareOutputNumberOfChannels { get;  }

	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	interface AVAudioSessionDelegate {
		[Export ("beginInterruption")]
		void BeginInterruption ();
	
		[Export ("endInterruption")]
		void EndInterruption ();

		[Export ("inputIsAvailableChanged:")]
		void InputIsAvailableChanged (bool isInputAvailable);
	
		[Since (4,0)]
		[Export ("endInterruptionWithFlags:")]
		void EndInterruption (AVAudioSessionInterruptionFlags flags);
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVAsset {
		[Export ("duration")]
		CMTime Duration { get;  }

		[Export ("preferredRate")]
		float PreferredRate { get;  }

		[Export ("preferredVolume")]
		float PreferredVolume { get;  }

		[Export ("preferredTransform")]
		CGAffineTransform PreferredTransform { get;  }

		[Export ("naturalSize")]
		SizeF NaturalSize { get;  }

		[Export ("providesPreciseDurationAndTiming")]
		bool ProvidesPreciseDurationAndTiming { get;  }

		[Export ("cancelLoading")]
		void CancelLoading ();

		[Export ("tracks")]
		AVAssetTrack [] Tracks { get;  }

		[Export ("trackWithTrackID:")]
		AVAssetTrack TrackWithTrackID (int trackID);

		[Export ("tracksWithMediaType:")]
		AVAssetTrack [] TracksWithMediaType (string mediaType);

		[Export ("tracksWithMediaCharacteristic:")]
		AVAssetTrack [] TracksWithMediaCharacteristic (string mediaCharacteristic);

		[Export ("lyrics")]
		string Lyrics { get;  }

		[Export ("commonMetadata")]
		AVMetadataItem [] CommonMetadata { get;  }

		[Export ("availableMetadataFormats")]
		string [] AvailableMetadataFormats { get;  }

		[Export ("metadataForFormat:")]
		AVMetadataItem [] MetadataForFormat (string format);
	}

	[Since (4,1)]
	[BaseType (typeof (NSObject))]
	interface AVAssetReader {
		[Export ("asset")]
		AVAsset Asset { get;  }

		[Export ("status")]
		AVAssetReaderStatus Status { get;  }

		[Export ("error")]
		NSError Error { get;  }

		[Export ("timeRange")]
		CMTimeRange TimeRange { get; set;  }

		[Export ("outputs")]
		AVAssetReaderOutput [] Outputs { get;  }

		[Static, Export ("assetReaderWithAsset:error:")]
		AVAssetReader _FromAsset (AVAsset asset, IntPtr ptrToNsError);

		[Export ("initWithAsset:error:")]
		IntPtr Constructor (AVAsset asset, IntPtr ptrToNsError);

		[Export ("canAddOutput:")]
		bool CanAddOutput (AVAssetReaderOutput output);

		[Export ("addOutput:")]
		void AddOutput (AVAssetReaderOutput output);

		[Export ("startReading")]
		bool StartReading ();

		[Export ("cancelReading")]
		void CancelReading ();
	}

	[Since (4,1)]
	[BaseType (typeof (NSObject))]
	interface AVAssetReaderOutput {
		[Export ("mediaType")]
		string MediaType { get; }

		[Export ("copyNextSampleBuffer")]
		CMSampleBuffer CopyNextSampleBuffer ();
	}

	[Since (4,1)]
	[BaseType (typeof (AVAssetReaderOutput))]
	interface AVAssetReaderTrackOutput {
		[Export ("track")]
		AVAssetTrack Track { get;  }

		[Static, Export ("assetReaderTrackOutputWithTrack:outputSettings:")]
		AVAssetReaderTrackOutput FromTrack (AVAssetTrack track, NSDictionary outputSettings);

		[Export ("initWithTrack:outputSettings:")]
		IntPtr Constructor (AVAssetTrack track, NSDictionary outputSettings);

		[Export ("outputSettings")]
		NSDictionary OutputSettings { get; }

	}

	[Since (4,1)]
	[BaseType (typeof (AVAssetReaderOutput))]
	interface AVAssetReaderAudioMixOutput {
		[Export ("audioTracks")]
		AVAssetTrack [] AudioTracks { get;  }

		[Export ("audioMix")]
		AVAudioMix AudioMix { get; set;  }

		[Export ("assetReaderAudioMixOutputWithAudioTracks:audioSettings:")]
		AVAssetReaderAudioMixOutput FromTracks (AVAssetTrack [] audioTracks, NSDictionary audioSettings);

		[Export ("initWithAudioTracks:audioSettings:")]
		IntPtr Constructor (AVAssetTrack [] audioTracks, NSDictionary audioSettings);

		[Export ("audioSettings")]
		NSDictionary AudioSettings { get; }
	}

	[Since (4,1)]
	[BaseType (typeof (AVAssetReaderOutput))]
	interface AVAssetReaderVideoCompositionOutput {
		[Export ("videoTracks")]
		AVAssetTrack [] VideoTracks { get;  }

		[Export ("videoComposition")]
		AVVideoComposition VideoComposition { get; set;  }

		[Export ("assetReaderVideoCompositionOutputWithVideoTracks:videoSettings:")]
		AVAssetReaderVideoCompositionOutput FromTracks (AVAssetTrack [] videoTracks, NSDictionary videoSettings);

		[Export ("initWithVideoTracks:videoSettings:")]
		IntPtr Constructor (AVAssetTrack [] videoTracks, NSDictionary videoSettings);

		[Export ("videoSettings")]
		NSDictionary VideoSettings { get; }
	}

	[Since (4,1)]
	[BaseType (typeof (NSObject))]
	interface AVAssetWriter {
		[Export ("outputURL")]
		NSUrl OutputURL { get;  }

		[Export ("outputFileType")]
		string OutputFileType { get;  }

		[Export ("status")]
		AVAssetWriterStatus Status { get;  }

		[Export ("error")]
		NSError Error { get;  }

		[Export ("movieFragmentInterval")]
		CMTime MovieFragmentInterval { get; set;  }

		[Export ("shouldOptimizeForNetworkUse")]
		bool ShouldOptimizeForNetworkUse { get; set;  }

		[Export ("inputs")]
		AVAssetWriterInput [] inputs { get;  }

		[Export ("metadata")]
		AVMetadataItem [] Metadata { get; set;  }

		[Static, Export ("assetWriterWithURL:fileType:error:")]
		AVAssetWriter FromUrl (NSUrl outputUrl, string outputFileType, IntPtr ptrToNSError);

		[Export ("initWithURL:fileType:error:")]
		IntPtr Constructor (NSUrl outputUrl, string outputFileType, IntPtr ptrToNSError);

		[Export ("canApplyOutputSettings:forMediaType:")]
		bool CanApplyOutputSettings (NSDictionary outputSettings, string toMediaType);

		[Export ("canAddInput:")]
		bool CanAddInput (AVAssetWriterInput input);

		[Export ("addInput:")]
		void AddInput (AVAssetWriterInput input);

		[Export ("startWriting")]
		bool StartWriting ();

		[Export ("startSessionAtSourceTime:")]
		void StartSessionAtSourceTime (CMTime startTime);

		[Export ("endSessionAtSourceTime:")]
		void EndSessionAtSourceTime (CMTime endTime);

		[Export ("cancelWriting")]
		void CancelWriting ();

		[Export ("finishWriting")]
		bool FinishWriting ();
	}

	[Since (4,1)]
	[BaseType (typeof (NSObject))]
	interface AVAssetWriterInput {
		[Export ("mediaType")]
		string MediaType { get;  }

		[Export ("outputSettings")]
		NSDictionary OutputSettings { get;  }

		[Export ("transform")]
		CGAffineTransform Transform { get; set;  }

		[Export ("metadata")]
		AVMetadataItem [] Metadata { get; set;  }

		[Export ("readyForMoreMediaData")]
		bool ReadyForMoreMediaData { [Bind ("isReadyForMoreMediaData")] get;  }

		[Export ("expectsMediaDataInRealTime")]
		bool ExpectsMediaDataInRealTime { get; set;  }

		[Static, Export ("assetWriterInputWithMediaType:outputSettings:")]
		AVAssetWriterInput FromType (string mediaType, NSDictionary outputSettings);

		[Export ("initWithMediaType:outputSettings:")]
		IntPtr Constructor (string mediaType, NSDictionary outputSettings);

		[Export ("requestMediaDataWhenReadyOnQueue:usingBlock:")]
		void RequestMediaData (DispatchQueue queue, NSAction action);

		[Export ("appendSampleBuffer:")]
		bool AppendSampleBuffer (CMSampleBuffer sampleBuffer);

		[Export ("markAsFinished")]
		void MarkAsFinished ();
	}

	[Since (4,1)]
	[BaseType (typeof (NSObject))]
	interface AVAssetWriterInputPixelBufferAdaptor {
		[Export ("assetWriterInput")]
		AVAssetWriterInput AssetWriterInput { get;  }

		[Export ("sourcePixelBufferAttributes")]
		NSDictionary SourcePixelBufferAttributes { get;  }

		//[Export ("pixelBufferPool")]
		//CVPixelBufferPoolRef pixelBufferPool { get;  }

		[Export ("assetWriterInputPixelBufferAdaptorWithAssetWriterInput:sourcePixelBufferAttributes:")]
		AVAssetWriterInputPixelBufferAdaptor FromInput (AVAssetWriterInput input, NSDictionary sourcePixelBufferAttributes);

		[Export ("initWithAssetWriterInput:sourcePixelBufferAttributes:")]
		IntPtr Constructor (AVAssetWriterInput input, NSDictionary sourcePixelBufferAttributes);

		//[Export ("appendPixelBuffer:withPresentationTime:")]
		//bool AppendPixelBufferwithPresentationTime (CVPixelBufferRef pixelBuffer, CMTime presentationTime);
	}

	[Since (4,0)]
	[BaseType (typeof (AVAsset), Name="AVURLAsset")]
	interface AVUrlAsset {
		[Export ("URL", ArgumentSemantic.Copy)]
		NSUrl Url { get;  }

		[Static, Export ("URLAssetWithURL:options:")]
		AVUrlAsset FromUrl (NSUrl URL, NSDictionary options);

		[Export ("initWithURL:options:")]
		IntPtr Constructor (NSUrl URL, NSDictionary options);

		[Export ("compatibleTrackForCompositionTrack:")]
		AVAssetTrack CompatibleTrack (AVCompositionTrack forCompositionTrack);
	}

	[BaseType (typeof (NSObject))]
	interface AVAssetTrack {
		[Export ("trackID")]
		int TrackID { get;  }

		[Export ("asset")]
		AVAsset Asset { get; }

		[Export ("mediaType")]
		string MediaType { get;  }

		// TODO: CMFormatDescriptions
		[Export ("formatDescriptions")]
		NSObject [] FormatDescriptionsAsObjects { get;  }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get;  }

		[Export ("selfContained")]
		bool SelfContained { [Bind ("isSelfContained")] get;  }

		[Export ("totalSampleDataLength")]
		long TotalSampleDataLength { get;  }

		[Export ("hasMediaCharacteristic:")]
		bool HasMediaCharacteristic (string mediaCharacteristic);

		[Export ("timeRange")]
		CMTimeRange TimeRange { get;  }

		[Export ("naturalTimeScale")]
		int NaturalTimeScale { get;  }

		[Export ("estimatedDataRate")]
		float EstimatedDataRate { get;  }

		[Export ("languageCode")]
		string LanguageCode { get;  }

		[Export ("extendedLanguageTag")]
		string ExtendedLanguageTag { get;  }

		[Export ("naturalSize")]
		SizeF NaturalSize { get;  }

		[Export ("preferredVolume")]
		float PreferredVolume { get;  }

		[Export ("preferredTransform")]
		CGAffineTransform PreferredTransform { get; }

		[Export ("nominalFrameRate")]
		float NominalFrameRate { get;  }

		[Export ("segments", ArgumentSemantic.Copy)]
		AVAssetTrackSegment [] Segments { get;  }

		[Export ("segmentForTrackTime:")]
		AVAssetTrackSegment SegmentForTrackTime (CMTime trackTime);

		[Export ("samplePresentationTimeForTrackTime:")]
		CMTime SamplePresentationTimeForTrackTime (CMTime trackTime);

		[Export ("availableMetadataFormats")]
		string [] AvailableMetadataFormats { get;  }

		[Export ("commonMetadata")]
		AVMetadataItem [] CommonMetadata { get; }

		[Export ("metadataForFormat:")]
		AVMetadataItem [] MetadataForFormat (string format);

	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVMetadataItem {
		[Export ("commonKey", ArgumentSemantic.Copy)]
		string CommonKey { get;  }

		[Export ("keySpace", ArgumentSemantic.Copy)]
		string KeySpace { get;  }

		[Export ("locale", ArgumentSemantic.Copy)]
		NSLocale Locale { get;  }

		[Export ("time")]
		CMTime Time { get;  }

		[Export ("value", ArgumentSemantic.Copy)]
		NSObject Value { get;  }

		[Export ("extraAttributes", ArgumentSemantic.Copy)]
		NSDictionary ExtraAttributes { get;  }

		[Export ("key", ArgumentSemantic.Copy)]
		NSObject Key { get; }

		[Export ("stringValue")]
		string StringValue { get;  }

		[Export ("numberValue")]
		NSNumber NumberValue { get;  }

		[Export ("dateValue")]
		NSDate DateValue { get;  }

		[Export ("dataValue")]
		NSData DataValue { get;  }

		[Static]
		[Export ("metadataItemsFromArray:withLocale:")]
		AVMetadataItem [] FilterWithLocale (AVMetadataItem [] arrayToFilter, NSLocale locale);

		[Static]
		[Export ("metadataItemsFromArray:withKey:keySpace:")]
		AVMetadataItem [] FilterWithKey (AVMetadataItem [] array, NSObject key, string keySpace);
	}

	[Since (4,0)]
	[BaseType (typeof (AVMetadataItem))]
	interface AVMutableMetadataItem {
		[Export ("keySpace", ArgumentSemantic.Copy)]
		string KeySpace { get; set;  }

		[Export ("metadataItem"), Static]
		AVMutableMetadataItem Create ();
		
		[Export ("locale", ArgumentSemantic.Copy)]
		NSLocale Locale { get; set;  }

		[Export ("time")]
		CMTime Time { get; set;  }

		[Export ("value", ArgumentSemantic.Copy)]
		NSObject Value { get; set;  }

		[Export ("extraAttributes", ArgumentSemantic.Copy)]
		NSDictionary ExtraAttributes { get; set;  }

		[Export ("key", ArgumentSemantic.Copy)]
		NSObject Key { get; }
	}

	[Since (4,0)]
	[BaseType (typeof (AVAssetTrack))]
	interface AVCompositionTrack {
		[Export ("segments", ArgumentSemantic.Copy)]
		AVCompositionTrackSegment [] Segments { get; }
	}

	[Since (4,0)]
	[BaseType (typeof (AVCompositionTrack))]
	interface AVMutableCompositionTrack {
		[Export ("segments", ArgumentSemantic.Copy)]
		AVCompositionTrackSegment [] Segments { get; set; }

		// TODO: binding for out NSError
		//[Export ("insertTimeRange:ofTrack:atTime:error:")]
		//bool InsertTimeRange (CMTimeRange timeRange, AVAssetTrack ofTrack, CMTime atTime, out NSError error);

		[Export ("insertEmptyTimeRange:")]
		void InsertEmptyTimeRange (CMTimeRange timeRange);

		[Export ("removeTimeRange:")]
		void RemoveTimeRange (CMTimeRange timeRange);

		[Export ("scaleTimeRange:toDuration:")]
		void ScaleTimeRange (CMTimeRange timeRange, CMTime duration);

		// TODO binding for out NSError
		//[Export ("validateTrackSegments:error:")]
		//bool ValidateTrackSegments (AVCompositionTrackSegment [] trackSegments, out NSError error);

		[Export ("extendedLanguageTag")]
		string ExtendedLanguageTag { get; set; }

		[Export ("languageCode")]
		string LanguageCode { get; set; }

		[Export ("naturalTimeScale")]
		int NaturalTimeScale { get; set; }

		[Export ("preferredTransform")]
		CGAffineTransform PreferredTransform { get; set; }

		[Export ("preferredVolume")]
		float PreferredVolume { get; set; }
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVAssetTrackSegment {
		[Export ("empty")]
		bool Empty { [Bind ("isEmpty")] get;  }

		[Export ("timeMapping")]
		CMTimeMapping TimeMapping { get; }

	}

	[Since (4,0)]
	[BaseType (typeof (AVAssetTrackSegment))]
	interface AVCompositionTrackSegment {
		[Export ("sourceURL")]
		NSUrl SourceUrl { get;  }

		[Export ("sourceTrackID")]
		int SourceTrackID { get;  }

		[Static]
		[Export ("compositionTrackSegmentWithURL:trackID:sourceTimeRange:targetTimeRange:")]
		IntPtr FromUrl (NSUrl url, int trackID, CMTimeRange sourceTimeRange, CMTimeRange targetTimeRange);

		[Static]
		[Export ("compositionTrackSegmentWithTimeRange:")]
		IntPtr FromTimeRange (CMTimeRange timeRange);

		[Export ("initWithURL:trackID:sourceTimeRange:targetTimeRange:")]
		IntPtr Constructor (NSUrl URL, int trackID, CMTimeRange sourceTimeRange, CMTimeRange targetTimeRange);

		[Export ("initWithTimeRange:")]
		IntPtr Constructor (CMTimeRange timeRange);
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVAssetExportSession {
		[Export ("presetName")]
		string PresetName { get;  }

		[Export ("supportedFileTypes")]
		NSObject [] SupportedFileTypes { get;  }

		[Export ("outputFileType", ArgumentSemantic.Copy)]
		string OutputFileType { get; set;  }

		[Export ("outputURL", ArgumentSemantic.Copy)]
		NSUrl OutputUrl { get; set;  }

		[Export ("status")]
		AVAssetExportSessionStatus Status { get;  }

		[Export ("progress")]
		float Progress { get;  }

		[Export ("maxDuration")]
		CMTime MaxDuration { get;  }

		[Export ("timeRange")]
		CMTimeRange TimeRange { get; set;  }

		[Export ("metadata", ArgumentSemantic.Copy)]
		AVMetadataItem [] Metadata { get; set;  }

		[Export ("fileLengthLimit")]
		long FileLengthLimit { get; set;  }

		[Export ("audioMix", ArgumentSemantic.Copy)]
		AVAudioMix AudioMix { get; set;  }

		[Export ("videoComposition", ArgumentSemantic.Copy)]
		AVVideoComposition VideoComposition { get; set;  }

		[Export ("shouldOptimizeForNetworkUse")]
		bool ShouldOptimizeForNetworkUse { get; set;  }

		[Export ("allExportPresets")]
		string [] AllExportPresets { get; }

		[Static]
		[Export ("exportPresetsCompatibleWithAsset:")]
		string [] ExportPresetsCompatibleWithAsset (AVAsset asset);

		[Export ("initWithAsset:presetName:")]
		IntPtr Constructor (AVAsset asset, string presetName);

		[Export ("exportAsynchronouslyWithCompletionHandler:")]
		void ExportAsynchronously (AVErrorHandler errorHandler);

		[Export ("cancelExport")]
		void CancelExport ();

		[Export ("error")]
		NSError Error { get; }

		[Field ("AVAssetExportPresetLowQuality")]
		NSString PresetLowQuality { get; }

		[Field ("AVAssetExportPresetMediumQuality")]
		NSString PresetMediumQuality { get; }

		[Field ("AVAssetExportPresetHighestQuality")]
		NSString PresetHighestQuality { get; }

		[Field ("AVAssetExportPreset640x480")]
		NSString Preset640x480 { get; }

		[Field ("AVAssetExportPreset960x540")]
		NSString Preset960x540 { get; }

		[Field ("AVAssetExportPreset1280x720")]
		NSString Preset1280x720 { get; }

		[Field ("AVAssetExportPresetAppleM4A")]
		NSString PresetAppleM4A { get; }

		[Field ("AVAssetExportPresetPassthrough")]
		NSString PresetPassthrough { get; }

		
	}
	
	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	interface AVAudioMix {
		[Export ("inputParameters", ArgumentSemantic.Copy)]
		AVAudioMixInputParameters [] InputParameters { get;  }
	}

	[Since (4,0)]
	[BaseType (typeof (AVAudioMix))]
	interface AVMutableAudioMix {
		[Export ("inputParameters", ArgumentSemantic.Copy)]
		AVAudioMixInputParameters [] InputParameters { get; set;  }

		[Static, Export ("audioMix")]
		AVMutableAudioMix Create ();
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVAudioMixInputParameters {
		[Export ("trackID")]
		int TrackID { get;  }

		[Export ("getVolumeRampForTime:startVolume:endVolume:timeRange:")]
		bool GetVolumeRamp (CMTime forTime, float startVolume, float endVolume, CMTimeRange timeRange);
	}


	[BaseType (typeof (AVAudioMixInputParameters))]
	interface AVMutableAudioMixInputParameters {
		[Export ("trackID")]
		int TrackID { get; set;  }

		[Static]
		[Export ("audioMixInputParametersWithTrack:")]
		AVMutableAudioMixInputParameters FromTrack (AVAssetTrack track);

		[Static]
		[Export ("audioMixInputParameters")]
		AVMutableAudioMixInputParameters Create ();
		
		[Export ("setVolumeRampFromStartVolume:toEndVolume:timeRange:")]
		void SetVolumeRamp (float startVolume, float endVolume, CMTimeRange timeRange);

		[Export ("setVolume:atTime:")]
		void SetVolume (float volume, CMTime atTime);
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVVideoComposition {
		[Export ("frameDuration")]
		CMTime FrameDuration { get;  }

		[Export ("renderSize")]
		SizeF RenderSize { get;  }

		[Export ("instructions", ArgumentSemantic.Copy)]
		AVVideoCompositionInstruction [] Instructions { get;  }

		[Export ("animationTool", ArgumentSemantic.Retain)]
		AVVideoCompositionCoreAnimationTool AnimationTool { get;  }

		[Export ("renderScale")]
		float RenderScale { get; set; }
	}

	[Since (4,0)]
	[BaseType (typeof (AVVideoComposition))]
	interface AVMutableVideoComposition {
		[Export ("frameDuration")]
		CMTime FrameDuration { get; set;  }

		[Export ("renderSize")]
		SizeF RenderSize { get; set;  }

		[Export ("instructions")]
		AVVideoCompositionInstruction [] Instructions { get; set;  }

		[Export ("animationTool")]
		AVVideoCompositionCoreAnimationTool AnimationTool { get; set;  }

		[Export ("renderScale")]
		float RenderScale { get; }

		[Static, Export ("videoComposition")]
		AVMutableVideoComposition Create ();
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVVideoCompositionInstruction {
		[Export ("timeRange")]
		CMTimeRange TimeRange { get;  }

		[Export ("backgroundColor")]
		CGColor BackgroundColor { get; set;  }

		[Export ("layerInstructions")]
		AVVideoCompositionLayerInstruction [] LayerInstructions { get;  }

		[Export ("enablePostProcessing")]
		bool EnablePostProcessing { get; }
	}

	[Since (4,0)]
	[BaseType (typeof (AVVideoCompositionInstruction))]
	interface AVMutableVideoCompositionInstruction {
		[Export ("timeRange")]
		CMTimeRange TimeRange { get; set;  }

		[Export ("backgroundColor")]
		CGColor BackgroundColor { get; set;  }

		// Already on base class, why does Apple add it here as well?
		//[Export ("enablePostProcessing")]
		//bool EnablePostProcessing { get; }

		[Export ("layerInstructions")]
		AVVideoCompositionLayerInstruction [] LayerInstructions { get; set;  }

		[Static, Export ("videoComposition")]
		AVVideoCompositionInstruction Create (); 		
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVVideoCompositionLayerInstruction {
		[Export ("trackID")]
		int TrackID { get;  }

		[Export ("getTransformRampForTime:startTransform:endTransform:timeRange:")]
		bool GetTransformRamp (CMTime time, CGAffineTransform startTransform, CGAffineTransform endTransform, CMTimeRange timeRange);

		[Export ("getOpacityRampForTime:startOpacity:endOpacity:timeRange:")]
		bool GetOpacityRamp (CMTime time, float startOpacity, float endOpacity, CMTimeRange timeRange);
	}

	[Since (4,0)]
	[BaseType (typeof (AVVideoCompositionLayerInstruction))]
	interface AVMutableVideoCompositionLayerInstruction {
		[Export ("trackID")]
		int TrackID { get; set;  }

		[Static]
		[Export ("videoCompositionLayerInstructionWithAssetTrack:")]
		AVMutableVideoCompositionLayerInstruction FromAssetTrack (AVAssetTrack track);

		[Static]
		[Export ("videoCompositionLayerInstruction")]
		AVMutableVideoCompositionLayerInstruction Create ();
		
		[Export ("setTransformRampFromStartTransform:toEndTransform:timeRange:")]
		void SetTransformRamp (CGAffineTransform startTransform, CGAffineTransform endTransform, CMTimeRange timeRange);

		[Export ("setTransform:atTime:")]
		void SetTransform (CGAffineTransform transform, CMTime atTime);

		[Export ("setOpacityRampFromStartOpacity:toEndOpacity:timeRange:")]
		void SetOpacityRamp (float startOpacity, float endOpacity, CMTimeRange timeRange);

		[Export ("setOpacity:atTime:")]
		void SetOpacity (float opacity, CMTime time);
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVVideoCompositionCoreAnimationTool {
		[Static]
		[Export ("videoCompositionCoreAnimationToolWithAdditionalLayer:asTrackID:")]
		AVVideoCompositionCoreAnimationTool FromLayer (CALayer layer, int trackID);

		[Static]
		[Export ("videoCompositionCoreAnimationToolWithPostProcessingAsVideoLayer:inLayer:")]
		AVVideoCompositionCoreAnimationTool FromLayer (CALayer videoLayer, CALayer animationLayer);
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVCaptureSession {
		[Export ("sessionPreset")]
		string SessionPreset { get; set;  }

		[Export ("inputs")]
		AVCaptureInput [] Inputs { get;  }

		[Export ("outputs")]
		AVCaptureOutput [] Outputs { get;  }

		[Export ("running")]
		bool Running { [Bind ("isRunning")] get;  }

		[Export ("interrupted")]
		bool Interrupted { [Bind ("isInterrupted")] get;  }

		[Export ("canSetSessionPreset:")]
		bool CanSetSessionPreset (string preset);

		[Export ("canAddInput:")]
		bool CanAddInput (AVCaptureInput input);

		[Export ("addInput:")]
		void AddInput (AVCaptureInput input);

		[Export ("removeInput:")]
		void RemoveInput (AVCaptureInput input);

		[Export ("canAddOutput:")]
		bool CanAddOutput (AVCaptureOutput output);

		[Export ("addOutput:")]
		void AddOutput (AVCaptureOutput output);

		[Export ("removeOutput:")]
		void RemoveOutput (AVCaptureOutput output);

		[Export ("beginConfiguration")]
		void BeginConfiguration ();

		[Export ("commitConfiguration")]
		void CommitConfiguration ();

		[Export ("startRunning")]
		void StartRunning ();

		[Export ("stopRunning")]
		void StopRunning ();
	}

	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	interface AVCaptureConnection {
		[Export ("output")]
		AVCaptureOutput Output { get;  }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set;  }

		[Export ("audioChannels")]
		AVCaptureAudioChannel AudioChannels { get;  }

		[Export ("videoMirrored")]
		bool VideoMirrored { [Bind ("isVideoMirrored")] get; set;  }

		[Export ("videoOrientation")]
		AVCaptureVideoOrientation VideoOrientation { get; set;  }

		[Export ("inputPorts")]
		AVCaptureInputPort [] inputPorts { get; }

		[Export ("isActive")]
		bool Active { get; }

		[Export ("isVideoMirroringSupported")]
		bool SupportsVideoMirroring { get; }

		[Export ("isVideoOrientationSupported")]
		bool SupportsVideoOrientation { get; }
	}

	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	interface AVCaptureAudioChannel {
		[Export ("peakHoldLevel")]
		float PeakHoldLevel { get;  }

		[Export ("averagePowerLevel")]
		float AveragePowerLevel { get; }
	}
	
	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	interface AVCaptureInput {
		[Export ("ports")]
		AVCaptureInputPort [] Ports { get; }
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVCaptureInputPort {
		[Export ("mediaType")]
		string MediaType { get;  }

		// TODO: bind CMFormatDescriptionRef
		//[Export ("formatDescription")]
		//CMFormatDescriptionRef FormatDescription { get;  }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set;  }

		[Export ("input")]
		AVCaptureInput Input  { get; }
	}

	[Since (4,0)]
	[BaseType (typeof (AVCaptureInput))]
	interface AVCaptureDeviceInput {
		[Export ("device")]
		AVCaptureDevice Device { get;  }

		// TODO: binding of error
		[Static, Export ("deviceInputWithDevice:error:")]
		AVCaptureDeviceInput FromDevice (AVCaptureDevice device, IntPtr ptrToHandleToError);

		// TODO: binding of error
		[Export ("initWithDevice:error:")]
		IntPtr Constructor (AVCaptureDevice device, IntPtr ptrToHandleToError);

	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVCaptureOutput {
		[Export ("connections")]
		NSObject [] Connections { get; }
	}

	[Since (4,0)]
        [BaseType (typeof (CALayer))]
        interface AVCaptureVideoPreviewLayer {
                [Export ("session")]
                AVCaptureSession Session { get; set;  }

                [Export ("orientation")]
                AVCaptureVideoOrientation Orientation { get; set;  }

                [Export ("automaticallyAdjustsMirroring")]
                bool AutomaticallyAdjustsMirroring { get; set;  }

                [Export ("mirrored")]
                bool Mirrored { [Bind ("isMirrored")] get; set;  }

		[Export ("isMirroringSupported")]
		bool MirroringSupported { get; }

		[Export ("isOrientationSupported")]
		bool OrientationSupported { get; }

		[Export ("videoGravity")]
		string VideoGravity { get; set; }

                [Static, Export ("layerWithSession:")]
                AVCaptureVideoPreviewLayer FromSession (AVCaptureSession session);

                [Export ("initWithSession:")]
                IntPtr Constructor (AVCaptureSession session);
        }
	
	[Since (4,0)]
	[BaseType (typeof (AVCaptureOutput))]
	interface AVCaptureVideoDataOutput {
		[Export ("sampleBufferDelegate")]
		AVCaptureVideoDataOutputSampleBufferDelegate SampleBufferDelegate { get; }

		[Export ("sampleBufferCallbackQueue")]
		IntPtr SampleBufferCallbackQueue { get;  }

		[Export ("videoSettings")]
		NSDictionary VideoSettings { get; set;  }

		[Export ("minFrameDuration")]
		CMTime MinFrameDuration { get; set;  }

		[Export ("alwaysDiscardsLateVideoFrames")]
		bool AlwaysDiscardsLateVideoFrames { get; set;  }

		[Export ("setSampleBufferDelegate:queue:")]
		void SetSampleBufferDelegatequeue (AVCaptureVideoDataOutputSampleBufferDelegate sampleBufferDelegate, IntPtr sampleBufferCallbackQueue);
	}

	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	[Model]
	interface AVCaptureVideoDataOutputSampleBufferDelegate {
		[Export ("captureOutput:didOutputSampleBuffer:fromConnection:")]
		// CMSampleBufferRef		
		void DidOutputSampleBuffer (AVCaptureOutput captureOutput, CMSampleBuffer sampleBuffer, AVCaptureConnection connection);
	}

	[Since (4,0)]
	[BaseType (typeof (AVCaptureOutput))]
	interface AVCaptureAudioDataOutput {
		[Export ("sampleBufferDelegate")]
		AVCaptureAudioDataOutputSampleBufferDelegate SampleBufferDelegate { get;  }

		[Export ("sampleBufferCallbackQueue")]
		DispatchQueue SampleBufferCallbackQueue { get;  }

		[Export ("setSampleBufferDelegate:queue:")]
		void SetSampleBufferDelegatequeue (AVCaptureAudioDataOutputSampleBufferDelegate sampleBufferDelegate, DispatchQueue sampleBufferCallbackDispatchQueue);
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	[Model]
	interface AVCaptureAudioDataOutputSampleBufferDelegate {
		[Export ("captureOutput:didOutputSampleBuffer:fromConnection:")]
		void DidOutputSampleBuffer (AVCaptureOutput captureOutput, CMSampleBuffer sampleBuffer, AVCaptureConnection connection);

	}

	[BaseType (typeof (AVCaptureOutput))]
	[Since (4,0)]
	interface AVCaptureFileOutput {
		[Export ("recordedDuration")]
		CMTime RecordedDuration { get;  }

		[Export ("recordedFileSize")]
		long RecordedFileSize { get;  }

		[Export ("isRecording")]
		bool Recording { get; }

		[Export ("maxRecordedDuration")]
		CMTime MaxRecordedDuration { get; set;  }

		[Export ("maxRecordedFileSize")]
		long MaxRecordedFileSize { get; set;  }

		[Export ("minFreeDiskSpaceLimit")]
		long MinFreeDiskSpaceLimit { get; set;  }

		[Export ("outputFileURL")]
		NSUrl OutputFileURL { get; }

		[Export ("startRecordingToOutputFileURL:recordingDelegate:")]
		void StartRecordingToOutputFile (NSUrl outputFileUrl, AVCaptureFileOutputRecordingDelegate recordingDelegate);

		[Export ("stopRecording")]
		void StopRecording ();
	}

	[BaseType (typeof (NSObject))]
	[Model]
	[Since (4,0)]
	interface AVCaptureFileOutputRecordingDelegate {
		[Export ("captureOutput:didStartRecordingToOutputFileAtURL:fromConnections:")]
		void DidStartRecording (AVCaptureFileOutput captureOutput, NSUrl outputFileUrl, NSObject [] connections);

		[Export ("captureOutput:didFinishRecordingToOutputFileAtURL:fromConnections:error:")]
		void FinishedRecording (AVCaptureFileOutput captureOutput, NSUrl outputFileUrl, NSObject [] connections, NSError error);
	}

	[Since (4,0)]
	[BaseType (typeof (AVCaptureFileOutput))]
	interface AVCaptureMovieFileOutput {
		[Export ("metadata")]
		AVMetadataItem [] Metadata { get; set;  }

		[Export ("movieFragmentInterval")]
		CMTime MovieFragmentInterval { get; }
	}

	[Since (4,0)]
	[BaseType (typeof (AVCaptureOutput))]
	interface AVCaptureStillImageOutput {
		[Export ("availableImageDataCVPixelFormatTypes")]
		NSNumber [] AvailableImageDataCVPixelFormatTypes { get;  }

		[Export ("availableImageDataCodecTypes")]
		string [] AvailableImageDataCodecTypes { get; }
		
		[Export ("outputSettings")]
		NSDictionary OutputSettings { get; }

		[Export ("captureStillImageAsynchronouslyFromConnection:completionHandler:")]
		void CaptureStillImageAsynchronously (AVCaptureConnection connection, AVCaptureCompletionHandler completionHandler);

		[Static, Export ("jpegStillImageNSDataRepresentation:")]
		NSData JpegStillToNSData (MonoMac.CoreMedia.CMSampleBuffer buffer);
	}
		
	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	interface AVCaptureDevice {
		[Export ("uniqueID")]
		string UniqueID { get;  }

		[Export ("modelID")]
		string ModelID { get;  }

		[Export ("localizedName")]
		string LocalizedName { get;  }

		[Export ("connected")]
		bool Connected { [Bind ("isConnected")] get;  }

		[Static, Export ("devices")]
		AVCaptureDevice [] Devices { get;  }

		[Static]
		[Export ("devicesWithMediaType:")]
		AVCaptureDevice [] DevicesWithMediaType (string mediaType);

		[Static]
		[Export ("defaultDeviceWithMediaType:")]
		AVCaptureDevice DefaultDeviceWithMediaType (string mediaType);

		[Static]
		[Export ("deviceWithUniqueID:")]
		AVCaptureDevice DeviceWithUniqueID (string deviceUniqueID);

		[Export ("hasMediaType:")]
		bool HasMediaType (string mediaType);

		// TODO: NSError
		[Export ("lockForConfiguration:")]
		bool LockForConfiguration (IntPtr ptrToHandleToError);

		[Export ("unlockForConfiguration")]
		void UnlockForConfiguration ();

		[Export ("supportsAVCaptureSessionPreset:")]
		bool SupportsAVCaptureSessionPreset (string preset);

		[Export ("flashMode")]
		AVCaptureFlashMode FlashMode { get; set;  }

		[Export ("isFlashModeSupported:")]
		bool IsFlashModeSupported (AVCaptureFlashMode flashMode);

		[Export ("torchMode")]
		AVCaptureTorchMode TorchMode { get; set;  }

		[Export ("isTorchModeSupported:")]
		bool IsTorchModeSupported (AVCaptureTorchMode torchMode);

		[Export ("focusMode")]
		AVCaptureFocusMode FocusMode { get; set;  }

		[Export ("focusPointOfInterestSupported")]
		bool FocusPointOfInterestSupported { [Bind ("isFocusPointOfInterestSupported")] get;  }

		[Export ("focusPointOfInterest")]
		PointF FocusPointOfInterest { get; set;  }

		[Export ("adjustingFocus")]
		bool AdjustingFocus { [Bind ("isAdjustingFocus")] get;  }

		[Export ("exposureMode")]
		AVCaptureExposureMode ExposureMode { get; set;  }

		[Export ("exposurePointOfInterestSupported")]
		bool ExposurePointOfInterestSupported { [Bind ("isExposurePointOfInterestSupported")] get;  }

		[Export ("exposurePointOfInterest")]
		PointF ExposurePointOfInterest { get; set;  }

		[Export ("adjustingExposure")]
		bool AdjustingExposure { [Bind ("isAdjustingExposure")] get;  }


		[Export ("whiteBalanceMode")]
		AVCaptureWhiteBalanceMode WhiteBalanceMode { get; set;  }

		[Export ("adjustingWhiteBalance")]
		bool AdjustingWhiteBalance { [Bind ("isAdjustingWhiteBalance")] get;  }

		[Export ("position")]
		AVCaptureDevicePosition Position { get; }
	}

	public delegate void AVErrorHandler (NSError error);
	public delegate void AVCaptureCompletionHandler (MonoMac.CoreMedia.CMSampleBuffer imageDataSampleBuffer, NSError error);

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVPlayer {
		[Export ("currentItem")]
		AVPlayerItem CurrentItem { get;  }

		[Export ("rate")]
		float Rate { get; set;  }

		[Export ("currentTime")]
		CMTime CurrentTime { get; set;  }

		[Export ("actionAtItemEnd")]
		AVPlayerActionAtItemEnd ActionAtItemEnd { get; set;  }

		[Export ("closedCaptionDisplayEnabled")]
		bool ClosedCaptionDisplayEnabled { [Bind ("isClosedCaptionDisplayEnabled")] get; set;  }

		[Static, Export ("playerWithURL:")]
		AVPlayer FromUrl (NSUrl URL);

		[Static]
		[Export ("playerWithPlayerItem:")]
		AVPlayer FromPlayerItem (AVPlayerItem item);

		[Export ("initWithURL:")]
		IntPtr Constructor (NSUrl URL);

		[Export ("initWithPlayerItem:")]
		IntPtr Constructor (AVPlayerItem item);

		[Export ("play")]
		void Play ();

		[Export ("pause")]
		void Pause ();

		[Export ("replaceCurrentItemWithPlayerItem:")]
		void ReplaceCurrentItemWithPlayerItem (AVPlayerItem item);

		[Export ("addPeriodicTimeObserverForInterval:queue:usingBlock:")]
		NSObject AddPeriodicTimeObserver (CMTime interval, DispatchQueue queue, AVTimeHandler handler);

		[Export ("addBoundaryTimeObserverForTimes:queue:usingBlock:")]
		NSObject AddBoundaryTimeObserver (NSValue []times, DispatchQueue queue, NSAction handler);

		[Export ("removeTimeObserver:")]
		void RemoveTimeObserver (NSObject observer);

		[Export ("seekToTime:")]
		void Seek (CMTime toTime);

		[Export ("seekToTime:toleranceBefore:toleranceAfter:")]
		void Seek (CMTime toTime, CMTime toleranceBefore, CMTime toleranceAfter);

		[Export ("error")]
		NSError Error { get; }

		[Export ("status")]
		AVPlayerStatus Status { get; }
	}

	delegate void AVTimeHandler (CMTime time);

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVPlayerItem {
		[Export ("status")]
		AVPlayerItemStatus Status { get;  }

		[Export ("asset")]
		AVAsset Asset { get;  }

		[Export ("tracks")]
		AVPlayerItem [] Tracks { get;  }

		[Export ("presentationSize")]
		SizeF PresentationSize { get;  }

		[Export ("forwardPlaybackEndTime")]
		CMTime ForwardPlaybackEndTime { get; set;  }

		[Export ("reversePlaybackEndTime")]
		CMTime ReversePlaybackEndTime { get; set;  }

		[Export ("audioMix")]
		AVAudioMix AudioMix { get; set;  }

		[Export ("videoComposition")]
		AVVideoComposition VideoComposition { get; set;  }

		[Export ("currentTime")]
		CMTime CurrentTime { get; set;  }

		[Export ("playbackLikelyToKeepUp")]
		bool PlaybackLikelyToKeepUp { [Bind ("isPlaybackLikelyToKeepUp")] get;  }

		[Export ("playbackBufferFull")]
		bool PlaybackBufferFull { [Bind ("isPlaybackBufferFull")] get;  }

		[Export ("playbackBufferEmpty")]
		bool PlaybackBufferEmpty { [Bind ("isPlaybackBufferEmpty")] get;  }

		// TODO: binding
		//[Export ("seekableTimeRanges")]
		//CMTimeRange [] seekableTimeRanges { get;  }

		// TODO; binding
		//[Export ("loadedTimeRanges")]
		// NSArray loadedTimeRanges { get;  }

		[Export ("timedMetadata")]
		NSObject [] TimedMetadata { get;  }

		[Static, Export ("playerItemWithURL:")]
		AVPlayerItem FromUrl (NSUrl URL);

		[Static]
		[Export ("playerItemWithAsset:")]
		AVPlayerItem FromAsset (AVAsset asset);

		[Export ("initWithURL:")]
		IntPtr Constructor (NSUrl URL);

		[Export ("initWithAsset:")]
		IntPtr Constructor (AVAsset asset);

		[Export ("stepByCount:")]
		void StepByCount (int stepCount);

		[Export ("seekToDate:")]
		bool Seek (NSDate date);

		[Export ("seekToTime:")]
		bool Seek (CMTime time);
		
		[Export ("seekToDate:")]
		bool SeekToDate (CMTime time, CMTime toleranceBefore, CMTime toleranceAfter);

		[Export ("error")]
		NSError Error { get; }	
	}

	[Since (4,0)]
	[BaseType (typeof (CALayer))]
	interface AVPlayerLayer {
		[Export ("player")]
		AVPlayer Player { get; set;  }

		[Static, Export ("playerLayerWithPlayer:")]
		AVPlayerLayer FromPlayer (AVPlayer player);

		[Export ("videoGravity")]
		string VideoGravity { get; set; }

		[Field ("AVLayerVideoGravityResizeAspect")]
		NSString GravityResizeAspect { get; }

		[Field ("AVLayerVideoGravityResizeAspectFill")]
		NSString GravityResizeAspectFill { get; }

		[Field ("AVLayerVideoGravityResize")]
		NSString GravityResize { get; }

		[Export ("isReadyForDisplay")]
		bool ReadyForDisplay { get; }
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVPlayerItemTrack {
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set;  }

		[Export ("assetTrack")]
		AVAssetTrack AssetTrack { get; }

	}

        [BaseType (typeof (NSObject))]
        [Model]
	[Since (4,0)]
        interface AVAsynchronousKeyValueLoading {
                [Abstract]
                [Export ("statusOfValueForKey:error:")]
                AVKeyValueStatus StatusOfValueForKeyerror (string key, IntPtr outError);

                [Abstract]
                [Export ("loadValuesAsynchronouslyForKeys:completionHandler:")]
                void LoadValuesAsynchronously (string [] keys, NSAction handler);
        }

	[Since (4,1)]
	[BaseType (typeof (AVPlayer))]
	interface AVQueuePlayer {
		[Static, Export ("queuePlayerWithItems:")]
		AVQueuePlayer FromItems (AVPlayerItem [] items);

		[Export ("initWithItems:")]
		IntPtr Constructor (AVPlayerItem [] items);

		[Export ("items")]
		AVPlayerItem [] Items { get; }

		[Export ("advanceToNextItem")]
		void AdvanceToNextItem ();

		[Export ("canInsertItem:afterItem:")]
		bool CanInsert (AVPlayerItem item, AVPlayerItem afterItem);

		[Export ("insertItem:afterItem:")]
		void InsertItem (AVPlayerItem item, AVPlayerItem afterItem);

		[Export ("removeItem:")]
		void RemoveItem (AVPlayerItem item);

		[Export ("removeAllItems")]
		void RemoveAllItems ();
	}
	
}
