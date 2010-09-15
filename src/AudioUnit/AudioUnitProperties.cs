using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MonoMac.CoreAudio;

using OSStatus                  = System.Int32;

using UInt8                     = System.Byte;
using UInt32                    = System.UInt32;
using Float32                   = System.Single;
using Float64                   = System.Double;
using SInt16                    = System.Int16;
using SInt32                    = System.Int32;
using SInt64                    = System.Int64;
using Boolean                   = System.Boolean; // Historic MacOS type with Sizeof() == 1

using AudioUnitPropertyID       = System.UInt32;
using AudioUnitScope            = System.UInt32;
using AudioUnitElement          = System.UInt32;
using AudioUnitParameterID      = System.UInt32;
using AudioUnitParameterValue   = System.Single;

using CFStringRef               = System.IntPtr;
using AudioUnit                 = System.IntPtr;
using AudioFileID               = System.IntPtr;

// TODO: Create proper signature delegate 
using HostCallback_GetBeatAndTempo = System.Delegate;
using HostCallback_GetMusicalTimeLocation = System.Delegate;
using HostCallback_GetTransportState = System.Delegate;
using AUMIDIOutputCallback  = System.Delegate;
using AUInputSamplesInOutputCallback = System.Delegate;
using ScheduledAudioSliceCompletionProc = System.Delegate;
using ScheduledAudioFileRegionCompletionProc = System.Delegate;
namespace MonoMac.AudioUnitFramework {

	public enum AudioUnitScope {
    	Global	= 0,
    	Input	= 1,
    	Output	= 2,
    }
    
    // I added ID to avoid a namespace collision with AudioUnitProperty struct in AUComponent
    public enum AudioUnitPropertyID : uint {
        // Generic
        ClassInfo					= 0,
    	MakeConnection				= 1,
    	SampleRate					= 2,
    	ParameterList				= 3,
    	ParameterInfo				= 4,
    	StreamFormat				= 8,
    	ElementCount				= 11,
    	Latency						= 12,
    	SupportedNumChannels		= 13,
    	MaximumFramesPerSlice		= 14,
    	AudioChannelLayout			= 19,  
    	TailTime					= 20,
    	BypassEffect				= 21,
    	LastRenderError				= 22,
    	SetRenderCallback			= 23,
    	FactoryPresets				= 24,
    	RenderQuality				= 26,
    	InPlaceProcessing			= 29,
    	ElementName					= 30,
    	SupportedChannelLayoutTags	= 32,
    	PresentPreset				= 36,
    	ShouldAllocateBuffer		= 51,
		FrequencyResponse = 52,
		
		FastDispatch               = 5,
		CPULoad                    = 6,
		SetExternalBuffer          = 15,
		ParameterValueStrings      = 16,
		GetUIComponentList         = 18,
		ContextName                = 25,
		HostCallbacks              = 27,
		CocoaUI                    = 31,
		ParameterStringFromValue   = 33,
		ParameterIDName            = 34,
		ParameterClumpName         = 35,
		OfflineRender              = 37,
		ParameterValueFromString   = 38,
		IconLocation               = 39,
		PresentationLatency        = 40,
		DependentParameters        = 45,
		AUHostIdentifier           = 46,
		MIDIOutputCallbackInfo     = 47,
		MIDIOutputCallback         = 48,
		InputSamplesInOutput       = 49,
		ClassInfoFromDocument      = 50,
		
    	
    	// MusicDevice Class
    	AllParameterMIDIMappings		= 41,
    	AddParameterMIDIMapping		    = 42,
    	RemoveParameterMIDIMapping      = 43,
    	HotMapParameterMIDIMapping      = 44,
    	
    	// General mixers
    	MeteringMode					= 3007,

    	// Matrix Mixer
    	MatrixLevels					= 3006,
    	MatrixDimensions				= 3009,
    	MeterClipping				    = 3011,
    	
    	// 3D Mixer
    	// I added a _ because numerals are not allowed as 1st character
    	_3DMixerDistanceParams		    = 3010,
    	_3DMixerAttenuationCurve		= 3013,
    	SpatializationAlgorithm		    = 3000,
    	DopplerShift					= 3002,
    	_3DMixerRenderingFlags		    = 3003,
    	_3DMixerDistanceAtten			= 3004,
    	ReverbPreset					= 3012,
    	
    	// Internal Reverb
    	ReverbRoomType				    = 10,

    	// 3DMixer, DLSMusicDevice
    	UsesInternalReverb		    	= 1005,
    	
    	// AUScheduledSoundPlayer
    	ScheduleAudioSlice			    = 3300,
    	ScheduleStartTimeStamp		    = 3301,
    	CurrentPlayTime			    	= 3302,
    	
    	// AUConverter
    	SampleRateConverterComplexity	= 3014,
    	
    	// Panner Unit
	    DistanceAttenuationData         = 3600,
	    
	    // AUAudioFilePlayer
	    ScheduledFileIDs				= 3310,
    	ScheduledFileRegion			    = 3311,
    	ScheduledFilePrime		    	= 3312,
    	ScheduledFileBufferSizeFrames   = 3313,
    	ScheduledFileNumberBuffers      = 3314,
    	
    	// AUDeferredRenderer
    	DeferredRendererPullSize		= 3320,
    	DeferredRendererExtraLatency	= 3321,
    	DeferredRendererWaitFrames      = 3322,
    	    	
    }

/*    
    #define kAUPresetVersionKey 		"version"
    #define kAUPresetTypeKey 			"type"
    #define kAUPresetSubtypeKey 		"subtype"
    #define kAUPresetManufacturerKey	"manufacturer"
    #define kAUPresetDataKey			"data"
    #define kAUPresetNameKey			"name"
    #define kAUPresetRenderQualityKey	"render-quality"
    #define kAUPresetCPULoadKey			"cpu-load"
    #define kAUPresetElementNameKey		"element-name"
    #define kAUPresetExternalFileRefs	"file-references"    
    
    #define kAUPresetPartKey			"part"
*/
    public class AUPresetKey {
        public static readonly string Version           = "version";
        public static readonly string Type   			= "type";
        public static readonly string Subtype    		= "subtype";
        public static readonly string Manufacturer  	= "manufacturer";
        public static readonly string Data  			= "data";
        public static readonly string Name  			= "name";
        public static readonly string RenderQuality 	= "render-quality";
        public static readonly string CPULoad   		= "cpu-load";
        public static readonly string ElementName   	= "element-name";
        public static readonly string ExternalFileRefs	= "file-references";    
        public static readonly string Part  			= "part";
    }
    
	[StructLayout(LayoutKind.Sequential)]
   	public struct AudioUnitConnection {
   	    public AudioUnit	SourceAudioUnit;
    	public UInt32       SourceOutputNumber;
    	public UInt32   	DestInputNumber;
    	
    	public override string ToString ()
		{
			return String.Format ("AudioUnit={0} SourceOutputNumber={1} DestInputNumber={2}", SourceAudioUnit, SourceOutputNumber, DestInputNumber);
		}
   	}
   	
   	[StructLayout(LayoutKind.Sequential)]
   	public struct AUChannelInfo {
   	    public SInt16   InChannels;
   	    public SInt16   OutChannels;
   	}
   	
   	[StructLayout(LayoutKind.Sequential)]
   	public struct AudioUnitExternalBuffer {
   	    public IntPtr   Buffer;
   	    public UInt32   Size;
    }
    
    [StructLayout(LayoutKind.Sequential)]
   	public struct AURenderCallbackStruct {
   	    public AURenderCallback     InputProc;
   	    public IntPtr               InputProcRefCon;
   	}
  
  	[StructLayout(LayoutKind.Sequential)]
   	public struct AUPreset {
   	    public SInt32   PresetNumber;
   	    public IntPtr   PresetName;
   	}
   	
   	public enum RenderQuality{
   	    Max								= 0x7F,
    	High							= 0x60,
    	Medium							= 0x40,
    	Low								= 0x20,
    	Min								= 0,
   	}
   	   	
   	[StructLayout(LayoutKind.Sequential)]
   	public struct AudioUnitFrequencyResponseBin {
   	    public Float64  Frequency;
   	    public Float64  Magnitude;
   	}
	
    [StructLayout(LayoutKind.Sequential)]
    public struct HostCallbackInfo {
        public IntPtr HostUserData;
        public HostCallback_GetBeatAndTempo BeatAndTempoProc;
        public HostCallback_GetMusicalTimeLocation MusicalTimeLocationProc;
        public HostCallback_GetTransportState TransportStateProc;
    }    
   

	[StructLayout(LayoutKind.Sequential)]
   	unsafe public struct AudioUnitCocoaViewInfo {
   	    public IntPtr CocoaAUViewBundleLocation;
   	    public IntPtr CocoaAUViewClass; // This is meant to be an array of a pre-allocated size and has to be handled on an app-by-app basis
   	}
   	
   	[StructLayout(LayoutKind.Sequential)]
   	public struct AUDependentParameter {
   	    public AudioUnitScope       Scope;
   	    public AudioUnitParameterID ParameterID;
   	}
   	
   	[StructLayout(LayoutKind.Sequential)]
   	public struct AUHostVersionIdentifier {
   	    public IntPtr   HostName;
   	    public UInt32   HostVerstion;    
   	}
   	
    [StructLayout(LayoutKind.Sequential)]
    public struct AUMIDIOutputCallbackStruct {
        public AUMIDIOutputCallback MidiOutputCallback;
        public IntPtr               UserData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AUInputSamplesInOutputCallbackStruct {
        public AUInputSamplesInOutputCallback   InputToOutputCallback;
        public IntPtr                           UserData;                   
    }   


    public enum AudioUnitParameterUnit {
        Generic				= 0,
    	Indexed				= 1,
    	Boolean				= 2,
    	Percent				= 3,
    	Seconds				= 4,
    	SampleFrames		= 5,
    	Phase				= 6,
    	Rate				= 7,
    	Hertz				= 8,
    	Cents				= 9,
    	RelativeSemiTones	= 10,
    	MIDINoteNumber		= 11,
    	MIDIController		= 12,
    	Decibels			= 13,
    	LinearGain			= 14,
    	Degrees				= 15,
    	EqualPowerCrossfade = 16,
    	MixerFaderCurve1	= 17,
    	Pan					= 18,
    	Meters				= 19,
    	AbsoluteCents		= 20,
    	Octaves				= 21,
    	BPM					= 22,
        Beats               = 23,
    	Milliseconds		= 24,
    	Ratio				= 25,
    	CustomUnit			= 26,
    }
    
    [StructLayout(LayoutKind.Sequential)]
   	unsafe public struct AudioUnitParameterInfo {
   	    public fixed char     Name[52];
   	    public IntPtr   UnitName;
   	    public uint     ClumpID;
   	    public IntPtr   CFNameString;
   	    public uint     Unit;
   	    public float    MinValue;
   	    public float    MaxValue;
   	    public float    DefaultValue;
   	    public uint     Flags;
   	}
   	
   	[Flags]
   	public enum AudioUnitParameterFlag : long {
   	    CFNameRelease		= (1L << 4),

    	MeterReadOnly		= (1L << 15),

    	// bit positions 18,17,16 are set aside for display scales. bit 19 is reserved.
    	DisplayMask			= (7L << 16) | (1L << 22),
    	DisplaySquareRoot	= (1L << 16),
    	DisplaySquared		= (2L << 16),
    	DisplayCubed		= (3L << 16),
    	DisplayCubeRoot		= (4L << 16),
    	DisplayExponential	= (5L << 16),

    	HasClump	 		= (1L << 20),
    	ValuesHaveStrings	= (1L << 21),

    	DisplayLogarithmic 	= (1L << 22),		

    	IsHighResolution 	= (1L << 23),
    	NonRealTime 		= (1L << 24),
    	CanRamp 			= (1L << 25),
    	ExpertMode 			= (1L << 26),
    	HasCFNameString 	= (1L << 27),
    	IsGlobalMeta 		= (1L << 28),
    	IsElementMeta		= (1L << 29),
    	IsReadable			= (1L << 30),
    	IsWritable			= (1L << 31),
   	}
    
    public class AudioUnitProperties {
    
       	public static int kNumberOfResponseFrequencies = 1024;
        public static int kAudioUnitClumpID_System = 0;
	
    	static AudioUnitParameterFlag GetAudioUnitParameterDisplayType(AudioUnitParameterFlag flags)
    	{
    	    return (flags & AudioUnitParameterFlag.DisplayMask);
    	}
	
    	static bool AudioUnitDisplayTypeIsLogarithmic(AudioUnitParameterFlag flags)
    	{
    	    return GetAudioUnitParameterDisplayType(flags) == AudioUnitParameterFlag.DisplayLogarithmic;
    	}
	
    	static bool AudioUnitDisplayTypeIsSquareRoot(AudioUnitParameterFlag flags)
    	{
    	    return GetAudioUnitParameterDisplayType(flags) == AudioUnitParameterFlag.DisplaySquareRoot;
    	}
	
    	static bool AudioUnitDisplayTypeIsSquared(AudioUnitParameterFlag flags)
    	{
    	    return GetAudioUnitParameterDisplayType(flags) == AudioUnitParameterFlag.DisplaySquared;
    	}
	
    	static bool AudioUnitDisplayTypeIsCubed(AudioUnitParameterFlag flags)
    	{
    	    return GetAudioUnitParameterDisplayType(flags) == AudioUnitParameterFlag.DisplayCubed;
    	}
	
    	static bool AudioUnitDisplayTypeIsCubeRoot(AudioUnitParameterFlag flags)
    	{
    	    return GetAudioUnitParameterDisplayType(flags) == AudioUnitParameterFlag.DisplayCubeRoot;
    	}
	
    	static bool AudioUnitDisplayTypeIsExponential(AudioUnitParameterFlag flags)
    	{
    	    return GetAudioUnitParameterDisplayType(flags) == AudioUnitParameterFlag.DisplayExponential;
    	}
	
    	static AudioUnitParameterFlag SetAudioUnitParameterDisplayType(AudioUnitParameterFlag flags, AudioUnitParameterFlag displayType)
    	{
    	    return (((flags) & ~AudioUnitParameterFlag.DisplayMask) | (displayType));
    	}
	
    }
    
	public enum AudioUnitParameterName {
	    Full    = -1,
	}
	
	[StructLayout(LayoutKind.Sequential)]
	public struct AudioUnitParameterNameInfo {
	    public AudioUnitParameterID InID;
	    public int                  InDesiredLength;
	    public CFStringRef          OutName; 
	}
	
	[StructLayout(LayoutKind.Sequential)]
	public struct AudioUnitParameterStringFromValue {
	    public AudioUnitParameterID     InParamID;
	    public IntPtr                   InValue;
	    public CFStringRef              OutString;
	}
	
	[StructLayout(LayoutKind.Sequential)]
	public struct AudioUnitParameterValueFromString {
	    public AudioUnitParameterID     InParamID;
	    public CFStringRef              InString;
	    public AudioUnitParameterValue  OutValue;
	}
	
	public enum AudioOutputUnitProperty {
	    CurrentDevice			= 2000,
	    IsRunning               = 2001,
    	ChannelMap				= 2002,
    	EnableIO				= 2003,
    	StartTime				= 2004,
    	SetInputCallback		= 2005,
    	HasIO					= 2006,
    	StartTimestampsAtZero   = 2007,
	}
	
	[Flags]
	public enum AUParameterMIDIMappingFlags : long {
	    AnyChannelFlag		= (1L << 0),
    	AnyNoteFlag			= (1L << 1),
    	SubRange			= (1L << 2),
    	Toggle				= (1L << 3),	
    	Bipolar				= (1L << 4),
    	Bipolar_On			= (1L << 5),
	}
	
	[StructLayout(LayoutKind.Sequential)]
	public struct AUParameterMIDIMapping {
	    public AudioUnitScope				Scope;
    	public AudioUnitElement			    Element;
    	public AudioUnitParameterID		    ParameterID;
    	public UInt32						Flags;
    	public AudioUnitParameterValue		SubRangeMin;
    	public AudioUnitParameterValue		SubRangeMax;
    	public UInt8						Status;
    	public UInt8						Data1;
    	public UInt8						Reserved1; // MUST be set to zero
    	public UInt8						Reserved2; // MUST be set to zero
    	public UInt32						Reserved3; // MUST be set to zero
	}
	
	public enum MusicDeviceProperty : uint {
	    // Instrument Unit (MusicDevice)
	    InstrumentCount 			= 1000,
    	MIDIXMLNames				= 1006,
    	PartGroup					= 1010,
    	DualSchedulingMode			= 1013,
    	SupportsStartStopNote		= 1014,
    	
    	// DLSMusicDevice & Internal Reverb
    	InstrumentName				= 1001,
    	InstrumentNumber 			= 1004,
    	UsesInternalReverb			= AudioUnitPropertyID.UsesInternalReverb,
    	BankName					= 1007,
    	SoundBankData				= 1008,
    	StreamFromDisk				= 1011,
    	SoundBankFSRef				= 1012,
    	SoundBankURL				= 1100
	}
	
	public enum MusicDeviceSampleFrameMask {
	    SampleOffset = 0xFFFFFF,
    	IsScheduled = 0x01000000
	}
	
	public enum AudioUnitOfflineProperty {
	    InputSize				= 3020,
    	OutputSize			    = 3021,
    	StartOffset		    	= 3022,
    	PreflightRequirements	= 3023,
    	PreflightName			= 3024
	}
	
	public enum OfflinePreflight {
	    NotRequired     = 0,
	    Optional        = 1,
	    Required        = 2
	}
	

    [StructLayout(LayoutKind.Sequential)]
    public struct AUDistanceAttenuationData {
        UInt32   InNumberOfPairs;
        
        [StructLayout(LayoutKind.Sequential)]
    	public struct Pairs/*[1]*/{ // this is a variable length array of InNumberOfPairs elements
            public Float32 InDistance; // 0-1000
            public Float32 OutGain;    // 0-1
        } 
    }
    
    public enum AudioUnitMigrateProperty {
        FromPlugin			    = 4000,
    	OldAutomation			= 4001
    }
    
    public enum OtherPluginFormat {
        Undefined   = 0,
    	kMAS		= 1,
    	kVST		= 2,
    	AU			= 3
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct AudioUnitOtherPluginDesc {
        public UInt32                       Format;
        public AudioClassDescription        Plugin;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct AudioUnitParameterValueTranslation {
    	public AudioUnitOtherPluginDesc	    OtherDesc;
    	public UInt32						OtherParamID;
    	public Float32						OtherValue;
    	public AudioUnitParameterID		    AUParamID;
    	public AudioUnitParameterValue		AUValue;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    unsafe public struct AudioUnitPresetMAS_SettingData {
    	public UInt32 				IsStockSetting;
    	public UInt32 				SettingID;
    	public UInt32 				DataLen; 
    	public UInt8 			    Data;       // Variable size array of length DataLen
    }
        
    [StructLayout(LayoutKind.Sequential)]
    unsafe public struct AudioUnitPresetMAS_Settings {
    	public UInt32 							ManufacturerID;
    	public UInt32 							EffectID;
    	public UInt32 							VariantID;
    	public UInt32 							SettingsVersion;
    	public UInt32 							SettingsCount;
    	public AudioUnitPresetMAS_SettingData 	Settings; // This is meant to be an array of a pre-allocated size and has to be handled on an app-by-app basis
    }
    
    public enum AudioUnitSampleRateConverterComplexity {
        Linear      = 0x6c696e65, // 'line'
        Normal      = 0x6e6f726d, // 'norm'
        Mastering   = 0x62617473, // 'bats'
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AudioOutputUnitStartAtTimeParams {
        public AudioTimeStamp  TimeStamp;
        public UInt32          Flags;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct AudioUnitMeterClipping {
    	public Float32  PeakValueSinceLastCall; 
    	public Boolean  SawInfinity;
    	public Boolean  SawNotANumber;
    }
    
    public enum _3DMixerAttenuationCurve {
        Power       = 0,
        Exponential = 1,
        Inverse     = 2,
        Linear      = 3,    
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct MixerDistanceParams {
        public Float32					ReferenceDistance;
    	public Float32					MaxDistance;
    	public Float32					MaxAttenuation;
    }
    
    public enum SpatializationAlgorithm {
    	EqualPowerPanning 		= 0,
    	SphericalHead 			= 1,
    	HRTF			 		= 2,
    	SoundField		 		= 3,
    	VectorBasedPanning		= 4,
    	StereoPassThrough		= 5
    }

    [Flags]
    public enum _3DMixerRenderingFlags : long {
    	InterAuralDelay			    = (1L << 0),
    	DopplerShift				= (1L << 1),
    	DistanceAttenuation		    = (1L << 2),
    	DistanceFilter			    = (1L << 3),
    	DistanceDiffusion		    = (1L << 4),
    	LinearDistanceAttenuation	= (1L << 5),
    	ConstantReverbBlend		    = (1L << 6)
    }
    
    public enum ReverbRoomType {
        SmallRoom		= 0,
   	    MediumRoom		= 1,
   	    LargeRoom		= 2,
   	    MediumHall		= 3,
   	    LargeHall		= 4,
   	    Plate			= 5,
   	    MediumChamber	= 6,
   	    LargeChamber	= 7,
        Cathedral		= 8,
   	    LargeRoom2		= 9,
   	    MediumHall2		= 10,
   	    MediumHall3		= 11,
   	    LargeHall2		= 12
    }
    
    [Flags]
    public enum ScheduledAudioSliceFlag {
        Complete            = 1,
        BeganToRender       = 2,
        BeganToRenderLate   = 3,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ScheduledAudioSlice {
        public AudioTimeStamp                       TimeStamp;
        public ScheduledAudioSliceCompletionProc    CompletionProc;
        public IntPtr                               CompletionProcUserData;
        public UInt32                               Flags;
        public UInt32                               Reserved;      // must be 0
        public IntPtr                               Reserved2;     
        public UInt32                               NumberFrames;  
        public IntPtr                               BufferList;
    }               
    
    [StructLayout(LayoutKind.Sequential)]
    public struct ScheduledAudioFileRegion {
        public AudioTimeStamp                          TimeStamp;
        public ScheduledAudioFileRegionCompletionProc  CompletionProc;
        public IntPtr                                  CompletionProcUserData;
        public IntPtr                                  AudioFile;
        public UInt32                                  LoopCount;
        public SInt64                                  StartFrame;
        public UInt32                                  FramesToPlay;
    }

    public enum AUNetReceiveProperty {
        Hostname    = 3511,
        Password    = 3512,
    }
    
    public enum AUNetSendProperty {
        PortNum                      = 3513,
    	TransmissionFormat           = 3514,
    	TransmissionFormatIndex      = 3515,
    	ServiceName                  = 3516,
    	Disconnect                   = 3517,
    	Password                     = 3518,
    }
    
    public enum AUNetSendPresetFormat {
        PCMFloat32	    	= 0,
    	PCMInt24			= 1,
    	PCMInt16			= 2,
    	Lossless24	    	= 3,
    	Lossless16	    	= 4,
    	ULaw				= 5,
    	IMA4				= 6,
    	AAC_128kbpspc   	= 7,
    	AAC_96kbpspc		= 8,
    	AAC_80kbpspc		= 9,
    	AAC_64kbpspc		= 10,
    	AAC_48kbpspc		= 11,
    	AAC_40kbpspc		= 12,
    	AAC_32kbpspc		= 13,
    	AAC_LD_64kbpspc 	= 14,
    	AAC_LD_48kbpspc 	= 15,
    	AAC_LD_40kbpspc 	= 16,
    	AAC_LD_32kbpspc 	= 17,
    	kAUNetSendNumPresetFormats  = 18,
    }
}