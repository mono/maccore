//
// AVFoundation.cs: This file describes the API that the generator will produce for AVFoundation
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2010, Novell, Inc.
// Copyright 2011-2012, Xamarin, INc.
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
using MonoMac.CoreVideo;
using System;
using System.Drawing;

namespace MonoMac.AVFoundation {

	delegate void AVAssetImageGeneratorCompletionHandler (CMTime requestedTime, IntPtr imageRef, CMTime actualTime, AVAssetImageGeneratorResult result, NSError error);
	delegate void AVCompletion (bool finished);
	
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

		[Field ("AVMediaCharacteristicIsMainProgramContent")]
		NSString IsMainProgramContent { get; }

		[Field ("AVMediaCharacteristicIsAuxiliaryContent")]
		NSString IsAuxiliaryContent { get; }

		[Field ("AVMediaCharacteristicContainsOnlyForcedSubtitles")]
		NSString ContainsOnlyForcedSubtitles { get; }

		[Field ("AVMediaCharacteristicTranscribesSpokenDialogForAccessibility")]
		NSString TranscribesSpokenDialogForAccessibility { get; }

		[Field ("AVMediaCharacteristicDescribesMusicAndSoundForAccessibility")]
		NSString DescribesMusicAndSoundForAccessibility { get; }

		[Field ("AVMediaCharacteristicDescribesVideoForAccessibility")]
		NSString DescribesVideoForAccessibility { get;  }
	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))][Static]
	interface AVFileType {
		[Field ("AVFileTypeQuickTimeMovie")]
		NSString QuickTimeMovie { get; }
		
		[Field ("AVFileTypeMPEG4")]
		NSString Mpeg4 { get; }
		
		[Field ("AVFileTypeAppleM4V")]
		NSString AppleM4V { get; }
		
		[Field ("AVFileType3GPP")]
		NSString ThreeGpp { get; }
		
		[Field ("AVFileTypeAppleM4A")]
		NSString AppleM4A { get; }
		
		[Field ("AVFileTypeCoreAudioFormat")]
		NSString CoreAudioFormat { get; }
		
		[Field ("AVFileTypeWAVE")]
		NSString Wave { get; }
		
		[Field ("AVFileTypeAIFF")]
		NSString Aiff { get; }
		
		[Field ("AVFileTypeAIFC")]
		NSString Aifc { get; }
		
		[Field ("AVFileTypeAMR")]
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

		[Field ("AVVideoQualityKey")]
		NSString QualityKey { get; }
		
		[Field ("AVVideoProfileLevelH264Baseline30")]
		NSString ProfileLevelH264Baseline30 { get; }
		
		[Field ("AVVideoProfileLevelH264Baseline31")]
		NSString ProfileLevelH264Baseline31 { get; }
		
		[Field ("AVVideoProfileLevelH264Main30")]
		NSString ProfileLevelH264Main30 { get; }
		
		[Field ("AVVideoProfileLevelH264Main31")]
		NSString ProfileLevelH264Main31 { get; }

		[Field ("AVVideoProfileLevelH264Baseline41")]
		NSString ProfileLevelH264Baseline41 { get; }

		[Field ("AVVideoProfileLevelH264Main32")]
		NSString ProfileLevelH264Main32 { get; }

		[Field ("AVVideoProfileLevelH264Main41")]
		NSString ProfileLevelH264Main41 { get; }
		
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
	[DisableDefaultCtor]
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
		bool PlayAtTime (double time);

		[Since (4,0)]
		[Export ("settings"), Internal]
		NSDictionary _Settings { get;  }

		[Field ("AVChannelLayoutKey"), Internal]
		NSString AVChannelLayoutKey { get; }

		[Field ("AVEncoderBitRateKey"), Internal]
		NSString AVEncoderBitRateKey { get; }

		[Field ("AVFormatIDKey"), Internal]
		NSString AVFormatIDKey { get; }

		[Field ("AVNumberOfChannelsKey"), Internal]
		NSString AVNumberOfChannelsKey { get; }

		[Field ("AVSampleRateKey"), Internal]
		NSString AVSampleRateKey { get; }
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	interface AVAudioPlayerDelegate {
		[Export ("audioPlayerDidFinishPlaying:successfully:"), CheckDisposed]
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

		[Field ("AVLinearPCMBitDepthKey"), Internal]
		NSString AVLinearPCMBitDepthKey { get; }

		[Field ("AVLinearPCMIsBigEndianKey"), Internal]
		NSString AVLinearPCMIsBigEndianKey { get; }

		[Field ("AVLinearPCMIsFloatKey"), Internal]
		NSString AVLinearPCMIsFloatKey { get; }

		[Field ("AVLinearPCMIsNonInterleaved"), Internal]
		NSString AVLinearPCMIsNonInterleaved { get; }

		[Field ("AVEncoderAudioQualityKey"), Internal]
		NSString AVEncoderAudioQualityKey { get; }
		
		[Field ("AVEncoderBitRateKey"), Internal]
		NSString AVEncoderBitRateKey { get; }
		
		[Field ("AVEncoderBitRatePerChannelKey"), Internal]
		NSString AVEncoderBitRatePerChannelKey { get; }
		
		[Field ("AVEncoderBitDepthHintKey"), Internal]
		NSString AVEncoderBitDepthHintKey { get; }

		[Field ("AVSampleRateConverterAudioQualityKey"), Internal]
		NSString AVSampleRateConverterAudioQualityKey { get; }
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	interface AVAudioRecorderDelegate {
		[Export ("audioRecorderDidFinishRecording:successfully:"), CheckDisposed]
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
		bool SetCategory (NSString theCategory, IntPtr outError);
	
		[Export ("setPreferredHardwareSampleRate:error:"), Internal]
		bool SetPreferredHardwareSampleRate (double sampleRate, IntPtr outError);
	
		[Export ("setPreferredIOBufferDuration:error:"), Internal]
		bool SetPreferredIOBufferDuration (double duration, IntPtr outError);
	
		[Export ("category")]
		NSString Category { get;  }

		[Export ("mode")]
		NSString Mode { get; }

		[Export ("setMode:error:")]
		bool SetMode (NSString mode, out NSError error);
	
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

		[Field ("AVAudioSessionCategoryAmbient")]
		NSString CategoryAmbient { get; }

		[Field ("AVAudioSessionCategorySoloAmbient")]
		NSString CategorySoloAmbient { get; }

		[Field ("AVAudioSessionCategoryPlayback")]
		NSString CategoryPlayback { get; }

		[Field ("AVAudioSessionCategoryRecord")]
		NSString CategoryRecord { get; }

		[Field ("AVAudioSessionCategoryPlayAndRecord")]
		NSString CategoryPlayAndRecord { get; }

		[Field ("AVAudioSessionCategoryAudioProcessing")]
		NSString CategoryAudioProcessing { get; }

		[Field ("AVAudioSessionModeDefault")]
		NSString ModeDefault { get; }

		[Field ("AVAudioSessionModeVoiceChat")]
		NSString ModeVoiceChat { get; }

		[Field ("AVAudioSessionModeVideoRecording")]
		NSString ModeVideoRecording { get; }

		[Field ("AVAudioSessionModeMeasurement")]
		NSString ModeMeasurement { get; }

		[Field ("AVAudioSessionModeGameChat")]
		NSString ModeGameChat { get; }
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

		[Export ("naturalSize"), Obsolete ("Instead use NaturalSize/PreferredTransform as appropriate on the video track")]
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

		[Since (4,2)]
		[Export ("hasProtectedContent")]
		bool ProtectedContent { get; }

		[Since (4,3)]
		[Export ("availableChapterLocales")]
		NSLocale [] AvailableChapterLocales { get; }

		[Since (4,3)]
		[Export ("chapterMetadataGroupsWithTitleLocale:containingItemsWithCommonKeys:")]
		AVMetadataItem [] ChapterMetadataGroups (NSLocale forLocale, [NullAllowed] AVMetadataItem [] commonKeys);

		[Since (4,3)]
		[Export ("isPlayable")]
		bool Playable { get; }

		[Since (4,3)]
		[Export ("isExportable")]
		bool Exportable { get; }

		[Since (4,3)]
		[Export ("isReadable")]
		bool Readable { get; }

		[Since (4,3)]
		[Export ("isComposable")]
		bool Composable { get; }

		// 5.0 APIs:
		[Since (5,0)]
		[Static, Export ("assetWithURL:")]
		AVAsset FromUrl (NSUrl url);

		[Since (5,0)]
		[Export ("availableMediaCharacteristicsWithMediaSelectionOptions")]
		string [] AvailableMediaCharacteristicsWithMediaSelectionOptions { get; }

		[Since (5,0)]
		[Export ("compatibleWithSavedPhotosAlbum")]
		bool CompatibleWithSavedPhotosAlbum  { get; }

		[Since (5,0)]
		[Export ("creationDate")]
		AVMetadataItem CreationDate { get; }

		[Since (5,0)]
		[Export ("mediaSelectionGroupForMediaCharacteristic:")]
		AVMediaSelectionGroup MediaSelectionGroupForMediaCharacteristic (string avMediaCharacteristic);

		[Export ("statusOfValueForKey:error:")]
		AVKeyValueStatus StatusOfValue (string key, out NSError error);

		[Export ("loadValuesAsynchronouslyForKeys:completionHandler:")]
		void LoadValuesAsynchronously (string [] keys, NSAction handler);
	}

	[BaseType (typeof (NSObject))]
	interface AVAssetImageGenerator {
		[Export ("maximumSize")]
		SizeF MaximumSize { get; set;  }

		[Export ("apertureMode")]
		NSString ApertureMode { get; set;  }

		[Export ("videoComposition", ArgumentSemantic.Copy)]
		AVVideoComposition VideoComposition { get; set;  }

		[Export ("appliesPreferredTrackTransform")]
		bool AppliesPreferredTrackTransform { get; set; }

		[Static]
		[Export ("assetImageGeneratorWithAsset:")]
		AVAssetImageGenerator FromAsset (AVAsset asset);

		[Export ("initWithAsset:")]
		IntPtr Constructor (AVAsset asset);

		[Export ("copyCGImageAtTime:actualTime:error:")]
		CGImage CopyCGImageAtTime (CMTime requestedTime, out CMTime actualTime, out NSError outError);

		[Export ("generateCGImagesAsynchronouslyForTimes:completionHandler:")]
		void GenerateCGImagesAsynchronously (NSValue cmTimesRequestedTimes, AVAssetImageGeneratorCompletionHandler handler);

		[Export ("cancelAllCGImageGeneration")]
		void CancelAllCGImageGeneration ();

		[Field ("AVAssetImageGeneratorApertureModeCleanAperture")]
		NSString ApertureModeCleanAperture { get; }

		[Field ("AVAssetImageGeneratorApertureModeProductionAperture")]
		NSString ApertureModeProductionAperture { get; }

		[Field ("AVAssetImageGeneratorApertureModeEncodedPixels")]
		NSString ApertureModeEncodedPixels { get; }

		// 5.0 APIs
		[Since (5,0)]
		[Export ("requestedTimeToleranceBefore")]
		CMTime RequestedTimeToleranceBefore { get; set;  }

		[Since (5,0)]
		[Export ("requestedTimeToleranceAfter")]
		CMTime RequestedTimeToleranceAfter { get; set;  }
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
		AVAssetReader FromAsset (AVAsset asset, out NSError error);

		[Export ("initWithAsset:error:")]
		IntPtr Constructor (AVAsset asset, out NSError error);

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

		[Export ("alwaysCopiesSampleData")]
		bool AlwaysCopiesSampleData { get; set; }
	}

	[Since (4,1)]
	[BaseType (typeof (AVAssetReaderOutput))]
	interface AVAssetReaderTrackOutput {
		[Export ("track")]
		AVAssetTrack Track { get;  }

		[Static, Export ("assetReaderTrackOutputWithTrack:outputSettings:")]
		AVAssetReaderTrackOutput FromTrack (AVAssetTrack track, [NullAllowed] NSDictionary outputSettings);

		[Export ("initWithTrack:outputSettings:")]
		IntPtr Constructor (AVAssetTrack track, [NullAllowed] NSDictionary outputSettings);

		[Export ("outputSettings")]
		NSDictionary OutputSettings { get; }

	}

	[Since (4,1)]
	[BaseType (typeof (AVAssetReaderOutput))]
	interface AVAssetReaderAudioMixOutput {
		[Export ("audioTracks")]
		AVAssetTrack [] AudioTracks { get;  }

		[Export ("audioMix", ArgumentSemantic.Copy)]
		AVAudioMix AudioMix { get; set;  }

		[Static, Export ("assetReaderAudioMixOutputWithAudioTracks:audioSettings:")]
		AVAssetReaderAudioMixOutput FromTracks (AVAssetTrack [] audioTracks, [NullAllowed] NSDictionary audioSettings);

		[Export ("initWithAudioTracks:audioSettings:")]
		IntPtr Constructor (AVAssetTrack [] audioTracks, [NullAllowed] NSDictionary audioSettings);

		[Export ("audioSettings")]
		NSDictionary AudioSettings { get; }
	}

	[Since (4,1)]
	[BaseType (typeof (AVAssetReaderOutput))]
	interface AVAssetReaderVideoCompositionOutput {
		[Export ("videoTracks")]
		AVAssetTrack [] VideoTracks { get;  }

		[Export ("videoComposition", ArgumentSemantic.Copy)]
		AVVideoComposition VideoComposition { get; set;  }

		[Export ("assetReaderVideoCompositionOutputWithVideoTracks:videoSettings:")]
		AVAssetReaderVideoCompositionOutput WeakFromTracks (AVAssetTrack [] videoTracks, [NullAllowed] NSDictionary videoSettings);

		[Export ("initWithVideoTracks:videoSettings:")]
		IntPtr Constructor (AVAssetTrack [] videoTracks, NSDictionary videoSettings);

		[Export ("videoSettings")]
		NSDictionary WeakVideoSettings { get; }
	}

	[Since (4,1)]
	[BaseType (typeof (NSObject))]
	interface AVAssetWriter {
		[Export ("outputURL", ArgumentSemantic.Copy)]
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
		AVAssetWriter FromUrl (NSUrl outputUrl, string outputFileType, out NSError error);

		[Export ("initWithURL:fileType:error:")]
		IntPtr Constructor (NSUrl outputUrl, string outputFileType, out NSError error);

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

		[Export ("movieTimeScale")]
		int MovieTimeScale { get; set; }
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

		[Export ("mediaTimeScale")]
		int MediaTimeScale { get; set; }
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

		[Static, Export ("assetWriterInputPixelBufferAdaptorWithAssetWriterInput:sourcePixelBufferAttributes:")]
		AVAssetWriterInputPixelBufferAdaptor FromInput (AVAssetWriterInput input, NSDictionary sourcePixelBufferAttributes);

		[Export ("initWithAssetWriterInput:sourcePixelBufferAttributes:")]
		IntPtr Constructor (AVAssetWriterInput input, NSDictionary sourcePixelBufferAttributes);

		[Export ("appendPixelBuffer:withPresentationTime:")]
		bool AppendPixelBufferWithPresentationTime (CVPixelBuffer pixelBuffer, CMTime presentationTime);
	}

	[Since (4,0)]
	[BaseType (typeof (AVAsset), Name="AVURLAsset")]
	interface AVUrlAsset {
		[Export ("URL", ArgumentSemantic.Copy)]
		NSUrl Url { get;  }

		[Static, Export ("URLAssetWithURL:options:")]
		AVUrlAsset FromUrl (NSUrl URL, [NullAllowed] NSDictionary options);

		[Export ("initWithURL:options:")]
		IntPtr Constructor (NSUrl URL, [NullAllowed] NSDictionary options);

		[Export ("compatibleTrackForCompositionTrack:")]
		AVAssetTrack CompatibleTrack (AVCompositionTrack forCompositionTrack);

		[Field ("AVURLAssetPreferPreciseDurationAndTimingKey")]
		NSString PreferPreciseDurationAndTimingKey { get; }

		[Since (5,0)]
		[Static, Export ("audiovisualMIMETypes")]
		string [] AudiovisualMimeTypes { get; }

		[Since (5,0)]
		[Static, Export ("audiovisualTypes")]
		string [] AudiovisualTypes { get; }

		[Since (5,0)]
		[Static, Export ("isPlayableExtendedMIMEType")]
		bool IsPlayable (string extendedMimeType);
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

		[Export ("isPlayable")]
		bool Playable { get; }
	}

	[BaseType (typeof (NSObject))]
	interface AVMediaSelectionGroup {
		[Export ("options")]
		AVMediaSelectionOption [] Options { get;  }
		
		[Export ("allowsEmptySelection")]
		bool AllowsEmptySelection { get;  }

		[Export ("mediaSelectionOptionWithPropertyList:")]
		AVMediaSelectionOption GetMediaSelectionOptionForPropertyList (NSObject propertyList);

		[Static]
		[Export ("playableMediaSelectionOptionsFromArray:")]
		AVMediaSelectionOption [] PlayableMediaSelectionOptions (AVMediaSelectionOption [] source);

		[Static]
		[Export ("mediaSelectionOptionsFromArray:withLocale:")]
		AVMediaSelectionOption [] MediaSelectionOptions (AVMediaSelectionOption [] source, NSLocale locale);

		[Static]
		[Export ("mediaSelectionOptionsFromArray:withMediaCharacteristics:")]
		AVMediaSelectionOption [] MediaSelectionOptions (AVMediaSelectionOption [] source, NSString [] avmediaCharacteristics);

		[Static]
		[Export ("mediaSelectionOptionsFromArray:withoutMediaCharacteristics:")]
		AVMediaSelectionOption [] MediaSelectionOptionsExcludingCharacteristics (NSArray array, NSString [] avmediaCharacteristics);
	}

	[BaseType (typeof (NSObject))]
	interface AVMediaSelectionOption {
		[Export ("mediaType")]
		string MediaType { get;  }

		[Export ("mediaSubTypes")]
		NSNumber []  MediaSubTypes { get;  }

		[Export ("playable")]
		bool Playable { [Bind ("isPlayable")] get;  }

		[Export ("locale")]
		NSLocale Locale { get;  }

		[Export ("commonMetadata")]
		AVMetadataItem [] CommonMetadata { get;  }

		[Export ("availableMetadataFormats")]
		string [] AvailableMetadataFormats { get;  }

		[Export ("hasMediaCharacteristic:")]
		bool HasMediaCharacteristic (string mediaCharacteristic);

		[Export ("metadataForFormat:")]
		AVMetadataItem [] GetMetadataForFormat (string format);

		[Export ("associatedMediaSelectionOptionInMediaSelectionGroup:")]
		AVMediaSelectionOption AssociatedMediaSelectionOptionInMediaSelectionGroup (AVMediaSelectionGroup mediaSelectionGroup);

		[Export ("propertyList")]
		NSObject PropertyList { get; }
	}

	[Static]
	interface AVMetadata {
		[Field ("AVMetadataKeySpaceCommon")]
		NSString KeySpaceCommon { get; }
		
		[Field ("AVMetadataCommonKeyTitle")]
		NSString CommonKeyTitle { get; }
		
		[Field ("AVMetadataCommonKeyCreator")]
		NSString CommonKeyCreator { get; }
		
		[Field ("AVMetadataCommonKeySubject")]
		NSString CommonKeySubject { get; }
		
		[Field ("AVMetadataCommonKeyDescription")]
		NSString CommonKeyDescription { get; }
		
		[Field ("AVMetadataCommonKeyPublisher")]
		NSString CommonKeyPublisher { get; }
		
		[Field ("AVMetadataCommonKeyContributor")]
		NSString CommonKeyContributor { get; }
		
		[Field ("AVMetadataCommonKeyCreationDate")]
		NSString CommonKeyCreationDate { get; }
		
		[Field ("AVMetadataCommonKeyLastModifiedDate")]
		NSString CommonKeyLastModifiedDate { get; }
		
		[Field ("AVMetadataCommonKeyType")]
		NSString CommonKeyType { get; }
		
		[Field ("AVMetadataCommonKeyFormat")]
		NSString CommonKeyFormat { get; }
		
		[Field ("AVMetadataCommonKeyIdentifier")]
		NSString CommonKeyIdentifier { get; }
		
		[Field ("AVMetadataCommonKeySource")]
		NSString CommonKeySource { get; }
		
		[Field ("AVMetadataCommonKeyLanguage")]
		NSString CommonKeyLanguage { get; }
		
		[Field ("AVMetadataCommonKeyRelation")]
		NSString CommonKeyRelation { get; }
		
		[Field ("AVMetadataCommonKeyLocation")]
		NSString CommonKeyLocation { get; }
		
		[Field ("AVMetadataCommonKeyCopyrights")]
		NSString CommonKeyCopyrights { get; }
		
		[Field ("AVMetadataCommonKeyAlbumName")]
		NSString CommonKeyAlbumName { get; }
		
		[Field ("AVMetadataCommonKeyAuthor")]
		NSString CommonKeyAuthor { get; }
		
		[Field ("AVMetadataCommonKeyArtist")]
		NSString CommonKeyArtist { get; }
		
		[Field ("AVMetadataCommonKeyArtwork")]
		NSString CommonKeyArtwork { get; }
		
		[Field ("AVMetadataCommonKeyMake")]
		NSString CommonKeyMake { get; }
		
		[Field ("AVMetadataCommonKeyModel")]
		NSString CommonKeyModel { get; }
		
		[Field ("AVMetadataCommonKeySoftware")]
		NSString CommonKeySoftware { get; }

		[Field ("AVMetadataFormatQuickTimeUserData")]
		NSString FormatQuickTimeUserData { get; }
		
		[Field ("AVMetadataKeySpaceQuickTimeUserData")]
		NSString KeySpaceQuickTimeUserData { get; }
	
		[Field ("AVMetadataQuickTimeUserDataKeyAlbum")]
		NSString QuickTimeUserDataKeyAlbum { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyArranger")]
		NSString QuickTimeUserDataKeyArranger { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyArtist")]
		NSString QuickTimeUserDataKeyArtist { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyAuthor")]
		NSString QuickTimeUserDataKeyAuthor { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyChapter")]
		NSString QuickTimeUserDataKeyChapter { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyComment")]
		NSString QuickTimeUserDataKeyComment { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyComposer")]
		NSString QuickTimeUserDataKeyComposer { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyCopyright")]
		NSString QuickTimeUserDataKeyCopyright { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyCreationDate")]
		NSString QuickTimeUserDataKeyCreationDate { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyDescription")]
		NSString QuickTimeUserDataKeyDescription { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyDirector")]
		NSString QuickTimeUserDataKeyDirector { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyDisclaimer")]
		NSString QuickTimeUserDataKeyDisclaimer { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyEncodedBy")]
		NSString QuickTimeUserDataKeyEncodedBy { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyFullName")]
		NSString QuickTimeUserDataKeyFullName { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyGenre")]
		NSString QuickTimeUserDataKeyGenre { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyHostComputer")]
		NSString QuickTimeUserDataKeyHostComputer { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyInformation")]
		NSString QuickTimeUserDataKeyInformation { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyKeywords")]
		NSString QuickTimeUserDataKeyKeywords { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyMake")]
		NSString QuickTimeUserDataKeyMake { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyModel")]
		NSString QuickTimeUserDataKeyModel { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyOriginalArtist")]
		NSString QuickTimeUserDataKeyOriginalArtist { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyOriginalFormat")]
		NSString QuickTimeUserDataKeyOriginalFormat { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyOriginalSource")]
		NSString QuickTimeUserDataKeyOriginalSource { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyPerformers")]
		NSString QuickTimeUserDataKeyPerformers { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyProducer")]
		NSString QuickTimeUserDataKeyProducer { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyPublisher")]
		NSString QuickTimeUserDataKeyPublisher { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyProduct")]
		NSString QuickTimeUserDataKeyProduct { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeySoftware")]
		NSString QuickTimeUserDataKeySoftware { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeySpecialPlaybackRequirements")]
		NSString QuickTimeUserDataKeySpecialPlaybackRequirements { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyTrack")]
		NSString QuickTimeUserDataKeyTrack { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyWarning")]
		NSString QuickTimeUserDataKeyWarning { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyWriter")]
		NSString QuickTimeUserDataKeyWriter { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyURLLink")]
		NSString QuickTimeUserDataKeyURLLink { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyLocationISO6709")]
		NSString QuickTimeUserDataKeyLocationISO6709 { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyTrackName")]
		NSString QuickTimeUserDataKeyTrackName { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyCredits")]
		NSString QuickTimeUserDataKeyCredits { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyPhonogramRights")]
		NSString QuickTimeUserDataKeyPhonogramRights { get; }
		
		[Field ("AVMetadataQuickTimeUserDataKeyTaggedCharacteristic")]
		NSString QuickTimeUserDataKeyTaggedCharacteristic { get; }
		
		[Field ("AVMetadataISOUserDataKeyCopyright")]
		NSString ISOUserDataKeyCopyright { get; }
		
		[Field ("AVMetadata3GPUserDataKeyCopyright")]
		NSString K3GPUserDataKeyCopyright { get; }
		
		[Field ("AVMetadata3GPUserDataKeyAuthor")]
		NSString K3GPUserDataKeyAuthor { get; }
		
		[Field ("AVMetadata3GPUserDataKeyPerformer")]
		NSString K3GPUserDataKeyPerformer { get; }
		
		[Field ("AVMetadata3GPUserDataKeyGenre")]
		NSString K3GPUserDataKeyGenre { get; }
		
		[Field ("AVMetadata3GPUserDataKeyRecordingYear")]
		NSString K3GPUserDataKeyRecordingYear { get; }
		
		[Field ("AVMetadata3GPUserDataKeyLocation")]
		NSString K3GPUserDataKeyLocation { get; }
		
		[Field ("AVMetadata3GPUserDataKeyTitle")]
		NSString K3GPUserDataKeyTitle { get; }
		
		[Field ("AVMetadata3GPUserDataKeyDescription")]
		NSString K3GPUserDataKeyDescription { get; }
		

		[Field ("AVMetadataFormatQuickTimeMetadata")]
		NSString FormatQuickTimeMetadata { get; }
		
		[Field ("AVMetadataKeySpaceQuickTimeMetadata")]
		NSString KeySpaceQuickTimeMetadata { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyAuthor")]
		NSString QuickTimeMetadataKeyAuthor { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyComment")]
		NSString QuickTimeMetadataKeyComment { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyCopyright")]
		NSString QuickTimeMetadataKeyCopyright { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyCreationDate")]
		NSString QuickTimeMetadataKeyCreationDate { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyDirector")]
		NSString QuickTimeMetadataKeyDirector { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyDisplayName")]
		NSString QuickTimeMetadataKeyDisplayName { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyInformation")]
		NSString QuickTimeMetadataKeyInformation { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyKeywords")]
		NSString QuickTimeMetadataKeyKeywords { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyProducer")]
		NSString QuickTimeMetadataKeyProducer { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyPublisher")]
		NSString QuickTimeMetadataKeyPublisher { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyAlbum")]
		NSString QuickTimeMetadataKeyAlbum { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyArtist")]
		NSString QuickTimeMetadataKeyArtist { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyArtwork")]
		NSString QuickTimeMetadataKeyArtwork { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyDescription")]
		NSString QuickTimeMetadataKeyDescription { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeySoftware")]
		NSString QuickTimeMetadataKeySoftware { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyYear")]
		NSString QuickTimeMetadataKeyYear { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyGenre")]
		NSString QuickTimeMetadataKeyGenre { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyiXML")]
		NSString QuickTimeMetadataKeyiXML { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyLocationISO6709")]
		NSString QuickTimeMetadataKeyLocationISO6709 { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyMake")]
		NSString QuickTimeMetadataKeyMake { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyModel")]
		NSString QuickTimeMetadataKeyModel { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyArranger")]
		NSString QuickTimeMetadataKeyArranger { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyEncodedBy")]
		NSString QuickTimeMetadataKeyEncodedBy { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyOriginalArtist")]
		NSString QuickTimeMetadataKeyOriginalArtist { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyPerformer")]
		NSString QuickTimeMetadataKeyPerformer { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyComposer")]
		NSString QuickTimeMetadataKeyComposer { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyCredits")]
		NSString QuickTimeMetadataKeyCredits { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyPhonogramRights")]
		NSString QuickTimeMetadataKeyPhonogramRights { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyCameraIdentifier")]
		NSString QuickTimeMetadataKeyCameraIdentifier { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyCameraFrameReadoutTime")]
		NSString QuickTimeMetadataKeyCameraFrameReadoutTime { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyTitle")]
		NSString QuickTimeMetadataKeyTitle { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyCollectionUser")]
		NSString QuickTimeMetadataKeyCollectionUser { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyRatingUser")]
		NSString QuickTimeMetadataKeyRatingUser { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyLocationName")]
		NSString QuickTimeMetadataKeyLocationName { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyLocationBody")]
		NSString QuickTimeMetadataKeyLocationBody { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyLocationNote")]
		NSString QuickTimeMetadataKeyLocationNote { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyLocationRole")]
		NSString QuickTimeMetadataKeyLocationRole { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyLocationDate")]
		NSString QuickTimeMetadataKeyLocationDate { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyDirectionFacing")]
		NSString QuickTimeMetadataKeyDirectionFacing { get; }
		
		[Field ("AVMetadataQuickTimeMetadataKeyDirectionMotion")]
		NSString QuickTimeMetadataKeyDirectionMotion { get; }
		
		[Field ("AVMetadataFormatiTunesMetadata")]
		NSString FormatiTunesMetadata { get; }
		
		[Field ("AVMetadataKeySpaceiTunes")]
		NSString KeySpaceiTunes { get; }
		

		[Field ("AVMetadataiTunesMetadataKeyAlbum")]
		NSString iTunesMetadataKeyAlbum { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyArtist")]
		NSString iTunesMetadataKeyArtist { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyUserComment")]
		NSString iTunesMetadataKeyUserComment { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyCoverArt")]
		NSString iTunesMetadataKeyCoverArt { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyCopyright")]
		NSString iTunesMetadataKeyCopyright { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyReleaseDate")]
		NSString iTunesMetadataKeyReleaseDate { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyEncodedBy")]
		NSString iTunesMetadataKeyEncodedBy { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyPredefinedGenre")]
		NSString iTunesMetadataKeyPredefinedGenre { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyUserGenre")]
		NSString iTunesMetadataKeyUserGenre { get; }
		
		[Field ("AVMetadataiTunesMetadataKeySongName")]
		NSString iTunesMetadataKeySongName { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyTrackSubTitle")]
		NSString iTunesMetadataKeyTrackSubTitle { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyEncodingTool")]
		NSString iTunesMetadataKeyEncodingTool { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyComposer")]
		NSString iTunesMetadataKeyComposer { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyAlbumArtist")]
		NSString iTunesMetadataKeyAlbumArtist { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyAccountKind")]
		NSString iTunesMetadataKeyAccountKind { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyAppleID")]
		NSString iTunesMetadataKeyAppleID { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyArtistID")]
		NSString iTunesMetadataKeyArtistID { get; }
		
		[Field ("AVMetadataiTunesMetadataKeySongID")]
		NSString iTunesMetadataKeySongID { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyDiscCompilation")]
		NSString iTunesMetadataKeyDiscCompilation { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyDiscNumber")]
		NSString iTunesMetadataKeyDiscNumber { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyGenreID")]
		NSString iTunesMetadataKeyGenreID { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyGrouping")]
		NSString iTunesMetadataKeyGrouping { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyPlaylistID")]
		NSString iTunesMetadataKeyPlaylistID { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyContentRating")]
		NSString iTunesMetadataKeyContentRating { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyBeatsPerMin")]
		NSString iTunesMetadataKeyBeatsPerMin { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyTrackNumber")]
		NSString iTunesMetadataKeyTrackNumber { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyArtDirector")]
		NSString iTunesMetadataKeyArtDirector { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyArranger")]
		NSString iTunesMetadataKeyArranger { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyAuthor")]
		NSString iTunesMetadataKeyAuthor { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyLyrics")]
		NSString iTunesMetadataKeyLyrics { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyAcknowledgement")]
		NSString iTunesMetadataKeyAcknowledgement { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyConductor")]
		NSString iTunesMetadataKeyConductor { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyDescription")]
		NSString iTunesMetadataKeyDescription { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyDirector")]
		NSString iTunesMetadataKeyDirector { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyEQ")]
		NSString iTunesMetadataKeyEQ { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyLinerNotes")]
		NSString iTunesMetadataKeyLinerNotes { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyRecordCompany")]
		NSString iTunesMetadataKeyRecordCompany { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyOriginalArtist")]
		NSString iTunesMetadataKeyOriginalArtist { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyPhonogramRights")]
		NSString iTunesMetadataKeyPhonogramRights { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyProducer")]
		NSString iTunesMetadataKeyProducer { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyPerformer")]
		NSString iTunesMetadataKeyPerformer { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyPublisher")]
		NSString iTunesMetadataKeyPublisher { get; }
		
		[Field ("AVMetadataiTunesMetadataKeySoundEngineer")]
		NSString iTunesMetadataKeySoundEngineer { get; }
		
		[Field ("AVMetadataiTunesMetadataKeySoloist")]
		NSString iTunesMetadataKeySoloist { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyCredits")]
		NSString iTunesMetadataKeyCredits { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyThanks")]
		NSString iTunesMetadataKeyThanks { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyOnlineExtras")]
		NSString iTunesMetadataKeyOnlineExtras { get; }
		
		[Field ("AVMetadataiTunesMetadataKeyExecProducer")]
		NSString iTunesMetadataKeyExecProducer { get; }
		
		[Field ("AVMetadataFormatID3Metadata")]
		NSString FormatID3Metadata { get; }
		
		[Field ("AVMetadataKeySpaceID3")]
		NSString KeySpaceID3 { get; }
		

		[Field ("AVMetadataID3MetadataKeyAudioEncryption")]
		NSString ID3MetadataKeyAudioEncryption { get; }
		
		[Field ("AVMetadataID3MetadataKeyAttachedPicture")]
		NSString ID3MetadataKeyAttachedPicture { get; }
		
		[Field ("AVMetadataID3MetadataKeyAudioSeekPointIndex")]
		NSString ID3MetadataKeyAudioSeekPointIndex { get; }
		
		[Field ("AVMetadataID3MetadataKeyComments")]
		NSString ID3MetadataKeyComments { get; }
		
		[Field ("AVMetadataID3MetadataKeyCommerical")]
		NSString ID3MetadataKeyCommerical { get; }
		
		[Field ("AVMetadataID3MetadataKeyEncryption")]
		NSString ID3MetadataKeyEncryption { get; }
		
		[Field ("AVMetadataID3MetadataKeyEqualization")]
		NSString ID3MetadataKeyEqualization { get; }
		
		[Field ("AVMetadataID3MetadataKeyEqualization2")]
		NSString ID3MetadataKeyEqualization2 { get; }
		
		[Field ("AVMetadataID3MetadataKeyEventTimingCodes")]
		NSString ID3MetadataKeyEventTimingCodes { get; }
		
		[Field ("AVMetadataID3MetadataKeyGeneralEncapsulatedObject")]
		NSString ID3MetadataKeyGeneralEncapsulatedObject { get; }
		
		[Field ("AVMetadataID3MetadataKeyGroupIdentifier")]
		NSString ID3MetadataKeyGroupIdentifier { get; }
		
		[Field ("AVMetadataID3MetadataKeyInvolvedPeopleList")]
		NSString ID3MetadataKeyInvolvedPeopleList { get; }
		
		[Field ("AVMetadataID3MetadataKeyLink")]
		NSString ID3MetadataKeyLink { get; }
		
		[Field ("AVMetadataID3MetadataKeyMusicCDIdentifier")]
		NSString ID3MetadataKeyMusicCDIdentifier { get; }
		
		[Field ("AVMetadataID3MetadataKeyMPEGLocationLookupTable")]
		NSString ID3MetadataKeyMPEGLocationLookupTable { get; }
		
		[Field ("AVMetadataID3MetadataKeyOwnership")]
		NSString ID3MetadataKeyOwnership { get; }
		
		[Field ("AVMetadataID3MetadataKeyPrivate")]
		NSString ID3MetadataKeyPrivate { get; }
		
		[Field ("AVMetadataID3MetadataKeyPlayCounter")]
		NSString ID3MetadataKeyPlayCounter { get; }
		
		[Field ("AVMetadataID3MetadataKeyPopularimeter")]
		NSString ID3MetadataKeyPopularimeter { get; }
		
		[Field ("AVMetadataID3MetadataKeyPositionSynchronization")]
		NSString ID3MetadataKeyPositionSynchronization { get; }
		
		[Field ("AVMetadataID3MetadataKeyRecommendedBufferSize")]
		NSString ID3MetadataKeyRecommendedBufferSize { get; }
		
		[Field ("AVMetadataID3MetadataKeyRelativeVolumeAdjustment")]
		NSString ID3MetadataKeyRelativeVolumeAdjustment { get; }
		
		[Field ("AVMetadataID3MetadataKeyRelativeVolumeAdjustment2")]
		NSString ID3MetadataKeyRelativeVolumeAdjustment2 { get; }
		
		[Field ("AVMetadataID3MetadataKeyReverb")]
		NSString ID3MetadataKeyReverb { get; }
		
		[Field ("AVMetadataID3MetadataKeySeek")]
		NSString ID3MetadataKeySeek { get; }
		
		[Field ("AVMetadataID3MetadataKeySignature")]
		NSString ID3MetadataKeySignature { get; }
		
		[Field ("AVMetadataID3MetadataKeySynchronizedLyric")]
		NSString ID3MetadataKeySynchronizedLyric { get; }
		
		[Field ("AVMetadataID3MetadataKeySynchronizedTempoCodes")]
		NSString ID3MetadataKeySynchronizedTempoCodes { get; }
		
		[Field ("AVMetadataID3MetadataKeyAlbumTitle")]
		NSString ID3MetadataKeyAlbumTitle { get; }
		
		[Field ("AVMetadataID3MetadataKeyBeatsPerMinute")]
		NSString ID3MetadataKeyBeatsPerMinute { get; }
		
		[Field ("AVMetadataID3MetadataKeyComposer")]
		NSString ID3MetadataKeyComposer { get; }
		
		[Field ("AVMetadataID3MetadataKeyContentType")]
		NSString ID3MetadataKeyContentType { get; }
		
		[Field ("AVMetadataID3MetadataKeyCopyright")]
		NSString ID3MetadataKeyCopyright { get; }
		
		[Field ("AVMetadataID3MetadataKeyDate")]
		NSString ID3MetadataKeyDate { get; }
		
		[Field ("AVMetadataID3MetadataKeyEncodingTime")]
		NSString ID3MetadataKeyEncodingTime { get; }
		
		[Field ("AVMetadataID3MetadataKeyPlaylistDelay")]
		NSString ID3MetadataKeyPlaylistDelay { get; }
		
		[Field ("AVMetadataID3MetadataKeyOriginalReleaseTime")]
		NSString ID3MetadataKeyOriginalReleaseTime { get; }
		
		[Field ("AVMetadataID3MetadataKeyRecordingTime")]
		NSString ID3MetadataKeyRecordingTime { get; }
		
		[Field ("AVMetadataID3MetadataKeyReleaseTime")]
		NSString ID3MetadataKeyReleaseTime { get; }
		
		[Field ("AVMetadataID3MetadataKeyTaggingTime")]
		NSString ID3MetadataKeyTaggingTime { get; }
		
		[Field ("AVMetadataID3MetadataKeyEncodedBy")]
		NSString ID3MetadataKeyEncodedBy { get; }
		
		[Field ("AVMetadataID3MetadataKeyLyricist")]
		NSString ID3MetadataKeyLyricist { get; }
		
		[Field ("AVMetadataID3MetadataKeyFileType")]
		NSString ID3MetadataKeyFileType { get; }
		
		[Field ("AVMetadataID3MetadataKeyTime")]
		NSString ID3MetadataKeyTime { get; }
		
		[Field ("AVMetadataID3MetadataKeyContentGroupDescription")]
		NSString ID3MetadataKeyContentGroupDescription { get; }
		
		[Field ("AVMetadataID3MetadataKeyTitleDescription")]
		NSString ID3MetadataKeyTitleDescription { get; }
		
		[Field ("AVMetadataID3MetadataKeySubTitle")]
		NSString ID3MetadataKeySubTitle { get; }
		
		[Field ("AVMetadataID3MetadataKeyInitialKey")]
		NSString ID3MetadataKeyInitialKey { get; }
		
		[Field ("AVMetadataID3MetadataKeyLanguage")]
		NSString ID3MetadataKeyLanguage { get; }
		
		[Field ("AVMetadataID3MetadataKeyLength")]
		NSString ID3MetadataKeyLength { get; }
		
		[Field ("AVMetadataID3MetadataKeyMusicianCreditsList")]
		NSString ID3MetadataKeyMusicianCreditsList { get; }
		
		[Field ("AVMetadataID3MetadataKeyMediaType")]
		NSString ID3MetadataKeyMediaType { get; }
		
		[Field ("AVMetadataID3MetadataKeyMood")]
		NSString ID3MetadataKeyMood { get; }
		
		[Field ("AVMetadataID3MetadataKeyOriginalAlbumTitle")]
		NSString ID3MetadataKeyOriginalAlbumTitle { get; }
		
		[Field ("AVMetadataID3MetadataKeyOriginalFilename")]
		NSString ID3MetadataKeyOriginalFilename { get; }
		
		[Field ("AVMetadataID3MetadataKeyOriginalLyricist")]
		NSString ID3MetadataKeyOriginalLyricist { get; }
		
		[Field ("AVMetadataID3MetadataKeyOriginalArtist")]
		NSString ID3MetadataKeyOriginalArtist { get; }
		
		[Field ("AVMetadataID3MetadataKeyOriginalReleaseYear")]
		NSString ID3MetadataKeyOriginalReleaseYear { get; }
		
		[Field ("AVMetadataID3MetadataKeyFileOwner")]
		NSString ID3MetadataKeyFileOwner { get; }
		
		[Field ("AVMetadataID3MetadataKeyLeadPerformer")]
		NSString ID3MetadataKeyLeadPerformer { get; }
		
		[Field ("AVMetadataID3MetadataKeyBand")]
		NSString ID3MetadataKeyBand { get; }
		
		[Field ("AVMetadataID3MetadataKeyConductor")]
		NSString ID3MetadataKeyConductor { get; }
		
		[Field ("AVMetadataID3MetadataKeyModifiedBy")]
		NSString ID3MetadataKeyModifiedBy { get; }
		
		[Field ("AVMetadataID3MetadataKeyPartOfASet")]
		NSString ID3MetadataKeyPartOfASet { get; }
		
		[Field ("AVMetadataID3MetadataKeyProducedNotice")]
		NSString ID3MetadataKeyProducedNotice { get; }
		
		[Field ("AVMetadataID3MetadataKeyPublisher")]
		NSString ID3MetadataKeyPublisher { get; }
		
		[Field ("AVMetadataID3MetadataKeyTrackNumber")]
		NSString ID3MetadataKeyTrackNumber { get; }
		
		[Field ("AVMetadataID3MetadataKeyRecordingDates")]
		NSString ID3MetadataKeyRecordingDates { get; }
		
		[Field ("AVMetadataID3MetadataKeyInternetRadioStationName")]
		NSString ID3MetadataKeyInternetRadioStationName { get; }
		
		[Field ("AVMetadataID3MetadataKeyInternetRadioStationOwner")]
		NSString ID3MetadataKeyInternetRadioStationOwner { get; }
		
		[Field ("AVMetadataID3MetadataKeySize")]
		NSString ID3MetadataKeySize { get; }
		
		[Field ("AVMetadataID3MetadataKeyAlbumSortOrder")]
		NSString ID3MetadataKeyAlbumSortOrder { get; }
		
		[Field ("AVMetadataID3MetadataKeyPerformerSortOrder")]
		NSString ID3MetadataKeyPerformerSortOrder { get; }
		
		[Field ("AVMetadataID3MetadataKeyTitleSortOrder")]
		NSString ID3MetadataKeyTitleSortOrder { get; }
		
		[Field ("AVMetadataID3MetadataKeyInternationalStandardRecordingCode")]
		NSString ID3MetadataKeyInternationalStandardRecordingCode { get; }
		
		[Field ("AVMetadataID3MetadataKeyEncodedWith")]
		NSString ID3MetadataKeyEncodedWith { get; }
		
		[Field ("AVMetadataID3MetadataKeySetSubtitle")]
		NSString ID3MetadataKeySetSubtitle { get; }
		
		[Field ("AVMetadataID3MetadataKeyYear")]
		NSString ID3MetadataKeyYear { get; }
		
		[Field ("AVMetadataID3MetadataKeyUserText")]
		NSString ID3MetadataKeyUserText { get; }
		
		[Field ("AVMetadataID3MetadataKeyUniqueFileIdentifier")]
		NSString ID3MetadataKeyUniqueFileIdentifier { get; }
		
		[Field ("AVMetadataID3MetadataKeyTermsOfUse")]
		NSString ID3MetadataKeyTermsOfUse { get; }
		
		[Field ("AVMetadataID3MetadataKeyUnsynchronizedLyric")]
		NSString ID3MetadataKeyUnsynchronizedLyric { get; }
		
		[Field ("AVMetadataID3MetadataKeyCommercialInformation")]
		NSString ID3MetadataKeyCommercialInformation { get; }
		
		[Field ("AVMetadataID3MetadataKeyCopyrightInformation")]
		NSString ID3MetadataKeyCopyrightInformation { get; }
		
		[Field ("AVMetadataID3MetadataKeyOfficialAudioFileWebpage")]
		NSString ID3MetadataKeyOfficialAudioFileWebpage { get; }
		
		[Field ("AVMetadataID3MetadataKeyOfficialArtistWebpage")]
		NSString ID3MetadataKeyOfficialArtistWebpage { get; }
		
		[Field ("AVMetadataID3MetadataKeyOfficialAudioSourceWebpage")]
		NSString ID3MetadataKeyOfficialAudioSourceWebpage { get; }
		
		[Field ("AVMetadataID3MetadataKeyOfficialInternetRadioStationHomepage")]
		NSString ID3MetadataKeyOfficialInternetRadioStationHomepage { get; }
		
		[Field ("AVMetadataID3MetadataKeyPayment")]
		NSString ID3MetadataKeyPayment { get; }
		
		[Field ("AVMetadataID3MetadataKeyOfficialPublisherWebpage")]
		NSString ID3MetadataKeyOfficialPublisherWebpage { get; }
		
		[Field ("AVMetadataID3MetadataKeyUserURL")]
		NSString ID3MetadataKeyUserURL { get; }
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

		[Since (4,2)]
		[Export ("duration")]
		CMTime Duration { get; }

		[Export ("statusOfValueForKey:error:")]
		AVKeyValueStatus StatusOfValueForKeyerror (string key, out NSError error);

		[Export ("loadValuesAsynchronouslyForKeys:completionHandler:")]
		void LoadValuesAsynchronously (string [] keys, NSAction handler);
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
		
		[Since (4,2)]
		[Export ("duration")]
		CMTime Duration { get; }
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

		[Export ("insertTimeRange:ofTrack:atTime:error:")]
		bool InsertTimeRange (CMTimeRange timeRange, AVAssetTrack ofTrack, CMTime atTime, out NSError error);

		[Export ("insertEmptyTimeRange:")]
		void InsertEmptyTimeRange (CMTimeRange timeRange);

		[Export ("removeTimeRange:")]
		void RemoveTimeRange (CMTimeRange timeRange);

		[Export ("scaleTimeRange:toDuration:")]
		void ScaleTimeRange (CMTimeRange timeRange, CMTime duration);

		[Export ("validateTrackSegments:error:")]
		bool ValidateTrackSegments (AVCompositionTrackSegment [] trackSegments, out NSError error);

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

		// 5.0
		[Since (5,0)]
		[Export ("insertTimeRanges:ofTracks:atTime:error:")]
		bool InsertTimeRanges (NSValue cmTimeRanges, AVAssetTrack [] tracks, CMTime startTime, out NSError error);
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
	[BaseType (typeof (AVAsset))]
	interface AVComposition {
		[Export ("tracks")]
		AVCompositionTrack [] Tracks { get; }

		[Export ("naturalSize")]
		SizeF NaturalSize { get; set; }

	}

	[Since (4,0)]
	[BaseType (typeof (AVComposition))]
	interface AVMutableComposition {
		[Export ("composition"), Static]
		AVMutableComposition Create ();

		[Export ("insertTimeRange:ofAsset:atTime:error:")]
		bool Insert (CMTimeRange insertTimeRange, AVAsset sourceAsset, CMTime atTime, out NSError error);

		[Export ("insertEmptyTimeRange:")]
		void InserEmptyTimeRange (CMTimeRange timeRange);

		[Export ("removeTimeRange:")]
		void RemoveTimeRange (CMTimeRange timeRange);

		[Export ("scaleTimeRange:toDuration:")]
		void ScaleTimeRange (CMTimeRange timeRange, CMTime duration);

		[Export ("addMutableTrackWithMediaType:preferredTrackID:")]
		AVMutableCompositionTrack AddMutableTrack (string mediaType, int preferredTrackId);

		[Export ("removeTrack:")]
		void RemoveTrack (AVCompositionTrack track);

		[Export ("mutableTrackCompatibleWithTrack:")]
		AVMutableCompositionTrack CreateMutableTrack (AVAssetTrack referenceTrack);
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

		[Static, Export ("allExportPresets")]
		string [] AllExportPresets { get; }

		[Static]
		[Export ("exportPresetsCompatibleWithAsset:")]
		string [] ExportPresetsCompatibleWithAsset (AVAsset asset);

		[Export ("initWithAsset:presetName:")]
		IntPtr Constructor (AVAsset asset, string presetName);

		[Export ("exportAsynchronouslyWithCompletionHandler:")]
		void ExportAsynchronously (AVCompletionHandler handler);

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

		// 5.0 APIs
		[Export ("asset")]
		AVAsset Asset { get; }

		[Export ("estimatedOutputFileLength")]
		long EstimatedOutputFileLength { get; }
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

		[Since (5,0)]
		[Export ("isValidForAsset:timeRange:validationDelegate:")]
                bool IsValidForAsset (AVAsset asset, CMTimeRange timeRange, AVVideoCompositionValidationHandling validationDelegate);
	}

	[Since (5,0)]
	[BaseType (typeof (NSObject))]
        [Model]
        interface AVVideoCompositionValidationHandling {
                [Export ("videoComposition:shouldContinueValidatingAfterFindingInvalidValueForKey:")]
                bool ShouldContinueValidatingAfterFindingInvalidValueForKey (AVVideoComposition videoComposition, string key);

                [Export ("videoComposition:shouldContinueValidatingAfterFindingEmptyTimeRange:")]
                bool ShouldContinueValidatingAfterFindingEmptyTimeRange (AVVideoComposition videoComposition, CMTimeRange timeRange);

                [Export ("videoComposition:shouldContinueValidatingAfterFindingInvalidTimeRangeInInstruction:")]
                bool ShouldContinueValidatingAfterFindingInvalidTimeRangeInInstruction (AVVideoComposition videoComposition, AVVideoCompositionInstruction videoCompositionInstruction);

                [Export ("videoComposition:shouldContinueValidatingAfterFindingInvalidTrackIDInInstruction:layerInstruction:asset:")]
                bool ShouldContinueValidatingAfterFindingInvalidTrackIDInInstruction (AVVideoComposition videoComposition, AVVideoCompositionInstruction videoCompositionInstruction, AVVideoCompositionLayerInstruction layerInstruction, AVAsset asset);
        }

	[Since (4,0)]
	[BaseType (typeof (AVVideoComposition))]
	interface AVMutableVideoComposition {
		[Export ("frameDuration")]
		CMTime FrameDuration { get; set;  }

		[Export ("renderSize")]
		SizeF RenderSize { get; set;  }

		[Export ("instructions", ArgumentSemantic.Copy)]
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

		[Export ("layerInstructions", ArgumentSemantic.Copy)]
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

		[Export ("layerInstructions", ArgumentSemantic.Copy)]
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

	interface AVCaptureSessionRuntimeErrorEventArgs {
		[Export ("AVCaptureSessionErrorKey")]
		NSError Error { get; }
	}
	
	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVCaptureSession {
		[Export ("sessionPreset")]
		NSString SessionPreset { get; set;  }

		[Export ("inputs")]
		AVCaptureInput [] Inputs { get;  }

		[Export ("outputs")]
		AVCaptureOutput [] Outputs { get;  }

		[Export ("running")]
		bool Running { [Bind ("isRunning")] get;  }

		[Export ("interrupted")]
		bool Interrupted { [Bind ("isInterrupted")] get;  }

		[Export ("canSetSessionPreset:")]
		bool CanSetSessionPreset (NSString preset);

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

		[Field ("AVCaptureSessionPresetPhoto")]
		NSString PresetPhoto { get; }
		
		[Field ("AVCaptureSessionPresetHigh")]
		NSString PresetHigh { get; }
		
		[Field ("AVCaptureSessionPresetMedium")]
		NSString PresetMedium { get; }
		
		[Field ("AVCaptureSessionPresetLow")]
		NSString PresetLow { get; }
		
		[Field ("AVCaptureSessionPreset640x480")]
		NSString Preset640x480 { get; }
		
		[Field ("AVCaptureSessionPreset1280x720")]
		NSString Preset1280x720 { get; }

		[Field ("AVCaptureSessionPresetiFrame960x540")]
		NSString PresetiFrame960x540 { get; }

		[Field ("AVCaptureSessionPresetiFrame1280x720")]
		NSString PresetiFrame1280x720 { get; }

		[Field ("AVCaptureSessionPreset352x288")]
		NSString Preset352x288 { get; }

		[Field ("AVCaptureSessionRuntimeErrorNotification")]
		[Notification (typeof (AVCaptureSessionRuntimeErrorEventArgs))]
		NSString RuntimeErrorNotification { get; }
		
		[Field ("AVCaptureSessionErrorKey")]
		NSString ErrorKey { get; }
		
		[Field ("AVCaptureSessionDidStartRunningNotification")]
		[Notification]
		NSString DidStartRunningNotification { get; }
		
		[Field ("AVCaptureSessionDidStopRunningNotification")]
		[Notification]
		NSString DidStopRunningNotification { get; }
		
		[Field ("AVCaptureSessionWasInterruptedNotification")]
		[Notification]
		NSString WasInterruptedNotification { get; }
		
		[Field ("AVCaptureSessionInterruptionEndedNotification")]
		[Notification]
		NSString InterruptionEndedNotification { get; }
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

		[Export ("supportsVideoMinFrameDuration"), Internal]
		bool _SupportsVideoMinFrameDuration { [Bind ("isVideoMinFrameDurationSupported")] get;  }

		[Export ("videoMinFrameDuration")]
		CMTime VideoMinFrameDuration { get; set;  }

		[Export ("supportsVideoMaxFrameDuration"), Internal]
		bool _SupportsVideoMaxFrameDuration { [Bind ("isVideoMaxFrameDurationSupported")] get;  }

		[Export ("videoMaxFrameDuration")]
		CMTime VideoMaxFrameDuration { get; set;  }

		[Export ("videoMaxScaleAndCropFactor")]
		float VideoMaxScaleAndCropFactor { get;  }

		[Export ("videoScaleAndCropFactor")]
		float VideoScaleAndCropFactor { get; set;  }
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

		[Field ("AVCaptureInputPortFormatDescriptionDidChangeNotification")]
		[Notification]
		NSString PortFormatDescriptionDidChangeNotification { get; }
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

		[Static, Export ("deviceInputWithDevice:error:")]
		AVCaptureDeviceInput FromDevice (AVCaptureDevice device, out NSError error);

		[Export ("initWithDevice:error:")]
		IntPtr Constructor (AVCaptureDevice device, out NSError error);

	}

	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	interface AVCaptureOutput {
		[Export ("connections")]
		NSObject [] Connections { get; }

		[Export ("connectionWithMediaType:")]
                AVCaptureConnection ConnectionFromMediaType (NSString avMediaType);
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
		DispatchQueue SampleBufferCallbackQueue { get;  }

		[Export ("videoSettings", ArgumentSemantic.Copy), NullAllowed]
		NSDictionary WeakVideoSettings { get; set;  }

		[Export ("minFrameDuration")]
		[Obsolete ("On iOS 5.0 and later, you can use AVCaptureConnection's MinVideoFrameDuration")]
		CMTime MinFrameDuration { get; set;  }

		[Export ("alwaysDiscardsLateVideoFrames")]
		bool AlwaysDiscardsLateVideoFrames { get; set;  }

		[Export ("setSampleBufferDelegate:queue:")]
		[PostGet ("SampleBufferDelegate")]
		[PostGet ("SampleBufferCallbackQueue")]
		void SetSampleBufferDelegate ([NullAllowed] AVCaptureVideoDataOutputSampleBufferDelegate sampleBufferDelegate, IntPtr sampleBufferCallbackQueue);

		// 5.0 APIs
		[Export ("availableVideoCVPixelFormatTypes")]
                NSNumber [] AvailableVideoCVPixelFormatTypes { get;  }

		// This is an NSString, because these are are codec types that can be used as keys in
		// the WeakVideoSettings properties.
                [Export ("availableVideoCodecTypes")]
                NSString [] AvailableVideoCodecTypes { get;  }
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

		[Export ("captureOutput:didFinishRecordingToOutputFileAtURL:fromConnections:error:"), CheckDisposed]
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
		
		[Export ("outputSettings", ArgumentSemantic.Copy)]
		NSDictionary OutputSettings { get; set; }

		[Export ("captureStillImageAsynchronouslyFromConnection:completionHandler:")]
		void CaptureStillImageAsynchronously (AVCaptureConnection connection, AVCaptureCompletionHandler completionHandler);

		[Static, Export ("jpegStillImageNSDataRepresentation:")]
		NSData JpegStillToNSData (MonoMac.CoreMedia.CMSampleBuffer buffer);

		// 5.0
		[Export ("capturingStillImage")]
		bool CapturingStillImage { [Bind ("isCapturingStillImage")] get;  }
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

		[Export ("lockForConfiguration:")]
		bool LockForConfiguration (out NSError error);

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

		[Field ("AVCaptureDeviceWasConnectedNotification")]
		[Notification]
		NSString WasConnectedNotification { get; }

		[Field ("AVCaptureDeviceWasDisconnectedNotification")]
		[Notification]
		NSString WasDisconnectedNotification { get; }

		[Field ("AVCaptureDeviceSubjectAreaDidChangeNotification")]
		[Notification]
		NSString SubjectAreaDidChangeNotification { get; }

		// 5.0

		[Export ("isFlashAvailable")]
		bool FlashAvailable { get;  }

		[Export ("isFlashActive")]
		bool FlashActive { get; }

		[Export ("isTorchAvailable")]
		bool TorchAvailable { get; }

		[Export ("torchLevel")]
		float TorchLevel { get; }
	}

	public delegate void AVCompletionHandler ();
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

		// 5.0
		[Since (5,0)]
		[Export ("allowsAirPlayVideo")]
		bool AllowsAirPlayVideo { get; set;  }

		[Since (5,0)]
		[Export ("airPlayVideoActive")]
		bool AirPlayVideoActive { [Bind ("isAirPlayVideoActive")] get;  }

		[Since (5,0)]
		[Export ("usesAirPlayVideoWhileAirPlayScreenIsActive")]
		bool UsesAirPlayVideoWhileAirPlayScreenIsActive { get; set;  }

		[Since (5,0)]
		[Export ("seekToTime:completionHandler:")]
		void Seek (CMTime time, AVCompletionHandler completion);

		[Since (5,0)]
		[Export ("seekToTime:toleranceBefore:toleranceAfter:completionHandler:")]
		void Seek (CMTime time, CMTime toleranceBefore, CMTime toleranceAfter, AVCompletionHandler completion);
	}

	[BaseType (typeof (NSObject))]
	[Since (4,3)]
	interface AVTimedMetadataGroup {
		[Export ("timeRange")]
		CMTimeRange TimeRange { get;  }

		[Export ("items")]
		AVMetadataItem [] Items { get;  }

		[Export ("initWithItems:timeRange:")]
		IntPtr Constructor (AVMetadataItem [] items, CMTimeRange timeRange);
	}

	[BaseType (typeof (AVTimedMetadataGroup))]
	interface AVMutableTimedMetadataGroup {
		[Export ("items")]
		AVMetadataItem [] Items { get; set;  }

		[Export ("timeRange")]
		CMTimeRange Timerange { get; set; }
	}

	delegate void AVTimeHandler (CMTime time);

	interface AVPlayerItemErrorEventArgs {
		[Export ("AVPlayerItemFailedToPlayToEndTimeErrorKey")]
		NSError Error { get; }
	}
		
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

		[Export ("audioMix", ArgumentSemantic.Copy)]
		AVAudioMix AudioMix { get; set;  }

		[Export ("videoComposition", ArgumentSemantic.Copy)]
		AVVideoComposition VideoComposition { get; set;  }

		[Export ("currentTime")]
		CMTime CurrentTime { get; set;  }

		[Export ("playbackLikelyToKeepUp")]
		bool PlaybackLikelyToKeepUp { [Bind ("isPlaybackLikelyToKeepUp")] get;  }

		[Export ("playbackBufferFull")]
		bool PlaybackBufferFull { [Bind ("isPlaybackBufferFull")] get;  }

		[Export ("playbackBufferEmpty")]
		bool PlaybackBufferEmpty { [Bind ("isPlaybackBufferEmpty")] get;  }

		[Export ("seekableTimeRanges")]
		NSValue [] SeekableTimeRanges { get;  }

		[Export ("loadedTimeRanges")]
		NSValue [] LoadedTimeRanges { get;  }

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

		[Field ("AVPlayerItemDidPlayToEndTimeNotification")]
		[Notification]
		NSString DidPlayToEndTimeNotification { get; }

		[Since (4,3)]
		[Field ("AVPlayerItemFailedToPlayToEndTimeNotification")]
		[Notification (typeof (AVPlayerItemErrorEventArgs))]
		NSString ItemFailedToPlayToEndTimeNotification { get; }

		[Since (4,3)]
		[Field ("AVPlayerItemFailedToPlayToEndTimeErrorKey")]
		NSString ItemFailedToPlayToEndTimeErrorKey { get; }

		[Since (4,3)]
		[Export ("accessLog")]
		AVPlayerItemAccessLog AccessLog { get; }

		[Since (4,3)]
		[Export ("errorLog")]
		AVPlayerItemErrorLog ErrorLog { get; }

		[Since (4,3)]
		[Export ("currentDate")]
		NSDate CurrentDate { get; }

		[Since (5,0)]
		[Export ("canPlayFastReverse")]
		bool CanPlayFastReverse { get;  }

		[Since (5,0)]
		[Export ("canPlayFastForward")]
		bool CanPlayFastForward { get; }

		[Since (5,0)]
		[Field ("AVPlayerItemTimeJumpedNotification")]
		[Notification]
		NSString TimeJumpedNotification { get; }

		[Since (5,0)]
		[Export ("seekToTime:completionHandler:")]
		void Seek (CMTime time, AVCompletion completion);

		[Since (5,0)]
		[Export ("cancelPendingSeeks")]
		void CancelPendingSeeks ();

		[Since (5,0)]
		[Export ("seekToTime:toleranceBefore:toleranceAfter:completionHandler:")]
		void Seek (CMTime time, CMTime toleranceBefore, CMTime toleranceAfter, AVCompletionHandler completion);

		[Since (5,0)]
		[Export ("selectMediaOption:inMediaSelectionGroup:")]
		void SelectMediaOption (AVMediaSelectionOption mediaSelectionOption, AVMediaSelectionGroup mediaSelectionGroup);

		[Export ("selectedMediaOptionInMediaSelectionGroup:")]
		AVMediaSelectionOption SelectedMediaOption (AVMediaSelectionGroup inMediaSelectionGroup);
	}

	[BaseType (typeof (NSObject))]
	[Since (4,3)]
	public interface AVPlayerItemAccessLog {
		[Export ("events")]
		AVPlayerItemAccessLogEvent [] Events { get; }

		[Export ("extendedLogDataStringEncoding")]
		NSStringEncoding ExtendedLogDataStringEncoding { get; }

		[Export ("extendedLogData")]
		NSData ExtendedLogData { get; }
	}

	[BaseType (typeof (NSObject))]
	[Since (4,3)]
	public interface AVPlayerItemErrorLog {
		[Export ("events")]
		AVPlayerItemErrorLogEvent [] Events { get; }

		[Export ("extendedLogDataStringEncoding")]
		NSStringEncoding ExtendedLogDataStringEncoding { get; }

		[Export ("extendedLogData")]
		NSData ExtendedLogData { get; }
	}
	
	[BaseType (typeof (NSObject))]
	[Since (4,3)]
	public interface AVPlayerItemAccessLogEvent {
		[Export ("numberOfSegmentsDownloaded")]
		int SegmentedDownloadedCount { get; }

		[Export ("playbackStartDate")]
		NSData PlaybackStartDate { get; }

		[Export ("URI")]
		string Uri { get; }

		[Export ("serverAddress")]
		string ServerAddress { get; }

		[Export ("numberOfServerAddressChanges")]
		int ServerAddressChangeCount { get; }

		[Export ("playbackSessionID")]
		string PlaybackSessionID { get; }

		[Export ("playbackStartOffset")]
		double PlaybackStartOffset { get; }

		[Export ("segmentsDownloadedDuration")]
		double SegmentsDownloadedDuration { get; }

		[Export ("durationWatched")]
		double DurationWatched { get; }

		[Export ("numberOfStalls")]
		int StallCount { get; }

		[Export ("numberOfBytesTransferred")]
		long BytesTransferred { get; }

		[Export ("observedBitrate")]
		double ObservedBitrate { get; }

		[Export ("indicatedBitrate")]
		double IndicatedBitrate { get; }

		[Export ("numberOfDroppedVideoFrames")]
		int DroppedVideoFrameCount { get; }
	}

	[BaseType (typeof (NSObject))]
	[Since (4,3)]
	public interface AVPlayerItemErrorLogEvent {
		[Export ("date")]
		NSDate Date { get; }

		[Export ("URI")]
		string Uri { get; }

		[Export ("serverAddress")]
		string ServerAddress { get; }

		[Export ("playbackSessionID")]
		string PlaybackSessionID { get; }

		[Export ("errorStatusCode")]
		int ErrorStatusCode { get; }

		[Export ("errorDomain")]
		string ErrorDomain { get; }

		[Export ("errorComment")]
		string ErrorComment { get; }
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

	[Static]
	public interface AVAudioSettings {
		[Field ("AVFormatIDKey")]
		NSString AVFormatIDKey { get; }
		
		[Field ("AVSampleRateKey")]
		NSString AVSampleRateKey { get; }
		
		[Field ("AVNumberOfChannelsKey")]
		NSString AVNumberOfChannelsKey { get; }
		
		[Field ("AVLinearPCMBitDepthKey")]
		NSString AVLinearPCMBitDepthKey { get; }
		
		[Field ("AVLinearPCMIsBigEndianKey")]
		NSString AVLinearPCMIsBigEndianKey { get; }
		
		[Field ("AVLinearPCMIsFloatKey")]
		NSString AVLinearPCMIsFloatKey { get; }
		
		[Field ("AVLinearPCMIsNonInterleaved")]
		NSString AVLinearPCMIsNonInterleaved { get; }
		
		[Field ("AVEncoderAudioQualityKey")]
		NSString AVEncoderAudioQualityKey { get; }
		
		[Field ("AVEncoderBitRateKey")]
		NSString AVEncoderBitRateKey { get; }
		
		[Field ("AVEncoderBitRatePerChannelKey")]
		NSString AVEncoderBitRatePerChannelKey { get; }
		
		[Field ("AVEncoderBitDepthHintKey")]
		NSString AVEncoderBitDepthHintKey { get; }
		
		[Field ("AVSampleRateConverterAudioQualityKey")]
		NSString AVSampleRateConverterAudioQualityKey { get; }
		
		[Field ("AVChannelLayoutKey")]
		NSString AVChannelLayoutKey { get; }
	}
	
}
