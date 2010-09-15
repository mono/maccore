using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using OSStatus                  = System.Int32;
using OSType                    = System.UInt32;
using UInt8                     = System.Byte;
using UInt32                    = System.UInt32;
using Float32                   = System.Single;
using Float64                   = System.Double;
using SInt16                    = System.Int16;
using SInt64                    = System.Int64;
using Boolean                   = System.Boolean; // Historic MacOS type with Sizeof() == 1


using   AudioSampleType         = System.Single;
using   AudioUnitSampleType     = System.Single;
using   AudioChannelLabel       = System.UInt32;
using   AudioChannelLayoutTag   = System.UInt32;
namespace MonoMac.CoreAudio {

    public class CoreAudioTypes {
    
        /*        
        TODO: Convert these two macros and several inline functions
        
        // #define AudioChannelLayoutTag_GetNumberOfChannels(layoutTag) ((public UInt32)((layoutTag) & 0x0000FFFF))
        // #define TestAudioFormatNativeEndian(f)  ((f.mFormatID == kAudioFormatLinearPCM) && ((f.mFormatFlags & kAudioFormatFlagsIsBigEndian) == kAudioFormatFlagssNativeEndian))
        #if defined(__cplusplus)
            inline bool IsAudioFormatNativeEndian(const AudioStreamBasicDescription& f) { return (f.mFormatID == kAudioFormatLinearPCM) && ((f.mFormatFlags & kAudioFormatFlagsIsBigEndian) == kAudioFormatFlagssNativeEndian); }
            inline public UInt32    CalculateLPCMFlags(public UInt32 inValidBitsPerChannel, public UInt32 inTotalBitsPerChannel, bool inIsFloat, bool inIsBigEndian, bool inIsNonInterleaved = false) { return (inIsFloat ? kAudioFormatFlagsIsFloat : kAudioFormatFlagsIsSignedInteger) | (inIsBigEndian ? ((public UInt32)kAudioFormatFlagsIsBigEndian) : 0) | ((!inIsFloat && (inValidBitsPerChannel == inTotalBitsPerChannel)) ? kAudioFormatFlagsIsPacked : kAudioFormatFlagsIsAlignedHigh) | (inIsNonInterleaved ? ((public UInt32)kAudioFormatFlagsIsNonInterleaved) : 0); }
            inline void    FillOutASBDForLPCM(AudioStreamBasicDescription& outASBD, public Float64 inSampleRate, public UInt32 inChannelsPerFrame, public UInt32 inValidBitsPerChannel, public UInt32 inTotalBitsPerChannel, bool inIsFloat, bool inIsBigEndian, bool inIsNonInterleaved = false)    { outASBD.mSampleRate = inSampleRate; outASBD.mFormatID = kAudioFormatLinearPCM; outASBD.mFormatFlags = CalculateLPCMFlags(inValidBitsPerChannel, inTotalBitsPerChannel, inIsFloat, inIsBigEndian, inIsNonInterleaved); outASBD.mBytesPerPacket = (inIsNonInterleaved ? 1 : inChannelsPerFrame) * (inTotalBitsPerChannel / 8); outASBD.mFramesPerPacket = 1; outASBD.mBytesPerFrame = (inIsNonInterleaved ? 1 : inChannelsPerFrame) * (inTotalBitsPerChannel / 8); outASBD.mChannelsPerFrame = inChannelsPerFrame; outASBD.mBitsPerChannel = inValidBitsPerChannel; }
            inline void    FillOutAudioTimeStampWithSampleTime(AudioTimeStamp& outATS, public Float64 inSampleTime)    { outATS.mSampleTime = inSampleTime; outATS.mHostTime = 0; outATS.mRateScalar = 0; outATS.mWordClockTime = 0; memset(&outATS.mSMPTETime, 0, sizeof(SMPTETime)); outATS.mFlags = kAudioTimeStampSampleTimeValid; }
            inline void    FillOutAudioTimeStampWithHostTime(AudioTimeStamp& outATS, public UInt64 inHostTime) { outATS.mSampleTime = 0; outATS.mHostTime = inHostTime; outATS.mRateScalar = 0; outATS.mWordClockTime = 0; memset(&outATS.mSMPTETime, 0, sizeof(SMPTETime)); outATS.mFlags = kAudioTimeStampHostTimeValid; }
            inline void    FillOutAudioTimeStampWithSampleAndHostTime(AudioTimeStamp& outATS, public Float64 inSampleTime, public UInt64 inHostTime) { outATS.mSampleTime = inSampleTime; outATS.mHostTime = inHostTime; outATS.mRateScalar = 0; outATS.mWordClockTime = 0; memset(&outATS.mSMPTETime, 0, sizeof(SMPTETime)); outATS.mFlags = kAudioTimeStampSampleTimeValid | kAudioTimeStampHostTimeValid; }
        #endif
*/        
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AudioValueRange {
        public Float64  Minimum;
        public Float64  Maximum;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct AudioValueTranslation {
        public IntPtr      InputData;
        public UInt32      InputDataSize;
        public IntPtr      OutputData;
        public UInt32      OutputDataSize;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct AudioBuffer
    {
        public UInt32       NumberChannels;
        public UInt32       DataByteSize;
        public IntPtr       Data;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public class AudioBufferList
    {
        public UInt32       NumberBuffers;
		// HACK only works with interleaved audio, noninterleaved will have a buffer per channel
		// possible fix would be to go back to using ref's and unsafe contexts
		[MarshalAs(UnmanagedType.ByValArray, SizeConst=1)]
        public AudioBuffer[]  Buffers; // this is a variable length array of NumberBuffers elements
		
        public AudioBufferList() { }
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct AudioStreamBasicDescription
    {
        public Float64      SampleRate;
        public AudioFormat  FormatID;
        public UInt32       FormatFlags;
        public UInt32       BytesPerPacket;
        public UInt32       FramesPerPacket;
        public UInt32       BytesPerFrame;
        public UInt32       ChannelsPerFrame;
        public UInt32       BitsPerChannel;
        public UInt32       Reserved;
    }
    
    public enum AudioFormat : uint {
        LinearPCM               = 0x6c70636d, // 'lpcm'
        AC3	                    = 0x61632d33, // 'ac-3'
        _60958AC3	            = 0x63616333, // 'cac3'
        AppleIMA4	            = 0x696d6134, // 'ima4'
        MPEG4AAC            	= 0x61616320, // 'aac '
        MPEG4CELP           	= 0x63656c70, // 'celp'
        MPEG4HVXC           	= 0x68767863, // 'hvxc'
        MPEG4TwinVQ	            = 0x74777671, // 'twvq'
        MACE3               	= 0x4d414333, // 'MAC3'
        MACE6               	= 0x4d414336, // 'MAC6'
        ULaw                	= 0x756c6177, // 'ulaw'
        ALaw                	= 0x616c6177, // 'alaw'
        QDesign             	= 0x51444d43, // 'QDMC'
        QDesign2            	= 0x51444d32, // 'QDM2'
        QUALCOMM            	= 0x51636c70, // 'Qclp'
        MPEGLayer1          	= 0x2e6d7031, // '.mp1'
        MPEGLayer2	            = 0x2e6d7032, // '.mp2'
        MPEGLayer3          	= 0x2e6d7033, // '.mp3'
        TimeCode            	= 0x74696d65, // 'time'
        MIDIStream          	= 0x6d696469, // 'midi'
        ParameterValueStream	= 0x61707673, // 'apvs'
        AppleLossless       	= 0x616c6163, // 'alac'
        MPEG4AAC_HE	            = 0x61616368, // 'aach'
        MPEG4AAC_LD	            = 0x6161636c, // 'aacl'
        MPEG4AAC_HE_V2	        = 0x61616370, // 'aacp'
        MPEG4AAC_Spatial    	= 0x61616373, // 'aacs'
        AMR	                    = 0x73616d72, // 'samr'
        Audible	                = 0x41554442, // 'AUDB'
        iLBC                	= 0x696c6263, // 'ilbc'        
        DVIIntelIMA             = 0x6D730011,
        MicrosoftGSM            = 0x6D730031,
        AES3                    = 0x61657333, // 'aes3',
    }
    
    [Flags]
    public enum AudioFormatFlags {
        IsFloat                     = (1 << 0),     // 0x1
        IsBigEndian                 = (1 << 1),     // 0x2
        IsSignedInteger             = (1 << 2),     // 0x4
        IsPacked                    = (1 << 3),     // 0x8
        IsAlignedHigh               = (1 << 4),     // 0x10
        IsNonInterleaved            = (1 << 5),     // 0x20
        IsNonMixable                = (1 << 6),     // 0x40
        AreAllClear                 = (1 << 31),
        
        AudioStreamAnyRate          = 0,
        NativeEndian                = 0,
        Canonical                   = IsFloat | NativeEndian | IsPacked,
        AudioUnitCanonical          = IsFloat | NativeEndian | IsPacked | IsNonInterleaved,
        NativeFloatPacked           = IsFloat | NativeEndian | IsPacked,
        
    }
    
    [Flags]
    public enum LinearPCMFormatFlag {
        IsFloat                 = AudioFormatFlags.IsFloat,
        IsBigEndian             = AudioFormatFlags.IsBigEndian,
        IsSignedInteger         = AudioFormatFlags.IsSignedInteger,
        IsPacked                = AudioFormatFlags.IsPacked,
        IsAlignedHigh           = AudioFormatFlags.IsAlignedHigh,
        IsNonInterleaved        = AudioFormatFlags.IsNonInterleaved,
        IsNonMixable            = AudioFormatFlags.IsNonMixable,
        SampleFractionShift     = 7,
        SampleFractionMask      = (0x3F << LinearPCMFormatFlag.SampleFractionShift),
        AreAllClear             = AudioFormatFlags.AreAllClear,
    }
    
    [Flags]
    public enum AppleLosslessFormatFlag {
        _16BitSourceData    = 1,
        _20BitSourceData    = 2,
        _24BitSourceData    = 3,
        _32BitSourceData    = 4,
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct AudioStreamPacketDescription
    {
        public SInt64       StartOffset;
        public UInt32       VariableFramesInPacket;
        public UInt32       DataByteSize;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct  SmpteTime
    {
        [Flags]
        public enum SMPTETimeFlags : uint {
            Valid     = (1 << 0),
            Running   = (1 << 1),
        }
        
        public SInt16       Subframes;
        public SInt16       SubframeDivisor;
        public UInt32       Counter;
        public UInt32       Type;
        public SMPTETimeFlags       Flags;
        public SInt16       Hours;
        public SInt16       Minutes;
        public SInt16       Seconds;
        public SInt16       Frames;
        
        public override string ToString ()
		{
			return String.Format ("[Subframes={0},Divisor={1},Counter={2},Type={3},Flags={4},Hours={5},Minutes={6},Seconds={7},Frames={8}]",
					      Subframes, SubframeDivisor, Counter, Type, Flags, Hours, Minutes, Seconds, Frames);
		}
    }
    
    public enum SMPTETimeType {
        _24        = 0,
        _25        = 1,
        _30Drop    = 2,
        _30        = 3,
        _2997      = 4,
        _2997Drop  = 5,
        _60        = 6,
        _5994      = 7,
        _60Drop    = 8,
        _5994Drop  = 9,
        _50        = 10,
        _2398      = 11,
    }

	[StructLayout(LayoutKind.Sequential)]
	public struct AudioTimeStamp {

		[Flags]
		public enum AtsFlags { 
			SampleTimeValid      = (1 << 0),
			HostTimeValid        = (1 << 1),
			RateScalarValid      = (1 << 2),
			WordClockTimeValid   = (1 << 3),
			SmpteTimeValid       = (1 << 4),
			SampleHostTimeValid  = SampleTimeValid | HostTimeValid
		}
			
		public double    SampleTime;
		public ulong     HostTime;
		public double    RateScalar;
		public ulong     WordClockTime;
		public SmpteTime SMPTETime;
		public AtsFlags  Flags;
		public uint      Reserved;
		
        public override string ToString ()
    	{
            var sb = new StringBuilder ("{");
            if ((Flags & AtsFlags.SampleTimeValid) != 0)
                sb.Append ("SampleTime=" + SampleTime.ToString ());

            if ((Flags & AtsFlags.HostTimeValid) != 0){
                if (sb.Length > 0)
                    sb.Append (',');
                sb.Append ("HostTime="   + HostTime.ToString ());
    		}

            if ((Flags & AtsFlags.RateScalarValid) != 0){
                if (sb.Length > 0)
                    sb.Append (',');
                sb.Append ("RateScalar=" + RateScalar.ToString ());
    		}

            if ((Flags & AtsFlags.WordClockTimeValid) != 0){
                if (sb.Length > 0)
                    sb.Append (',');
                sb.Append ("WordClock="  + HostTime.ToString () + ",");
    		}

            if ((Flags & AtsFlags.SmpteTimeValid) != 0){
                if (sb.Length > 0)
                    sb.Append (',');
                sb.Append ("SmpteTime="   + SMPTETime.ToString ());
    		}
    		sb.Append ("}");

    		return sb.ToString ();
    	}
	}

    [StructLayout(LayoutKind.Sequential)]
    public struct AudioClassDescription {
        public OSType  Type;
        public OSType  SubType;
        public OSType  Manufacturer;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    unsafe public struct AudioChannelDescription
    {
        public AudioChannelLabel    ChannelLabel;
        public UInt32               ChannelFlags;
        public fixed Float32        Coordinates[3];
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct AudioChannelLayout
    {
        public AudioChannelLayoutTag       ChannelLayoutTag;
        public UInt32                      ChannelBitmap;
        public UInt32                      NumberChannelDescriptions;
        public AudioChannelDescription     ChannelDescriptions; // this is a variable length array of NumberChannelDescriptions elements
    }
    
    public enum AudioChannelLabel : uint {
        Unknown                  = 0xFFFFFFFF,   // unknown or unspecified other use
        Unused                   = 0,            // channel is present, but has no intended use or destination
        UseCoordinates           = 100,          // channel is described by the mCoordinates fields.

        Left                     = 1,
        Right                    = 2,
        Center                   = 3,
        LFEScreen                = 4,
        LeftSurround             = 5,            // WAVE: "Back Left"
        RightSurround            = 6,            // WAVE: "Back Right"
        LeftCenter               = 7,
        RightCenter              = 8,
        CenterSurround           = 9,            // WAVE: "Back Center" or plain "Rear Surround"
        LeftSurroundDirect       = 10,           // WAVE: "Side Left"
        RightSurroundDirect      = 11,           // WAVE: "Side Right"
        TopCenterSurround        = 12,
        VerticalHeightLeft       = 13,           // WAVE: "Top Front Left"
        VerticalHeightCenter     = 14,           // WAVE: "Top Front Center"
        VerticalHeightRight      = 15,           // WAVE: "Top Front Right"

        TopBackLeft              = 16,
        TopBackCenter            = 17,
        TopBackRight             = 18,

        RearSurroundLeft         = 33,
        RearSurroundRight        = 34,
        LeftWide                 = 35,
        RightWide                = 36,
        LFE2                     = 37,
        LeftTotal                = 38,           // matrix encoded 4 channels
        RightTotal               = 39,           // matrix encoded 4 channels
        HearingImpaired          = 40,
        Narration                = 41,
        Mono                     = 42,
        DialogCentricMix         = 43,

        CenterSurroundDirect     = 44,           // back center, non diffuse

        Haptic                   = 45,

        // first order ambisonic channels
        Ambisonic_W              = 200,
        Ambisonic_X              = 201,
        Ambisonic_Y              = 202,
        Ambisonic_Z              = 203,

        // Mid/Side Recording
        MS_Mid                   = 204,
        MS_Side                  = 205,

        // X-Y Recording
        XY_X                     = 206,
        XY_Y                     = 207,

        // other
        HeadphonesLeft           = 301,
        HeadphonesRight          = 302,
        ClickTrack               = 304,
        ForeignLanguage          = 305,

        // generic discrete channel
        Discrete                 = 400,

        // numbered discrete channel
        Discrete_0               = (1<<16) | 0,
        Discrete_1               = (1<<16) | 1,
        Discrete_2               = (1<<16) | 2,
        Discrete_3               = (1<<16) | 3,
        Discrete_4               = (1<<16) | 4,
        Discrete_5               = (1<<16) | 5,
        Discrete_6               = (1<<16) | 6,
        Discrete_7               = (1<<16) | 7,
        Discrete_8               = (1<<16) | 8,
        Discrete_9               = (1<<16) | 9,
        Discrete_10              = (1<<16) | 10,
        Discrete_11              = (1<<16) | 11,
        Discrete_12              = (1<<16) | 12,
        Discrete_13              = (1<<16) | 13,
        Discrete_14              = (1<<16) | 14,
        Discrete_15              = (1<<16) | 15,
        Discrete_65535           = (1<<16) | 65535,
    }
    
    public enum AudioChannelBit {
        Left                       = (1<<0),
        Right                      = (1<<1),
        Center                     = (1<<2),
        LFEScreen                  = (1<<3),
        LeftSurround               = (1<<4),      // WAVE: "Back Left"
        RightSurround              = (1<<5),      // WAVE: "Back Right"
        LeftCenter                 = (1<<6),
        RightCenter                = (1<<7),
        CenterSurround             = (1<<8),      // WAVE: "Back Center"
        LeftSurroundDirect         = (1<<9),      // WAVE: "Side Left"
        RightSurroundDirect        = (1<<10),     // WAVE: "Side Right"
        TopCenterSurround          = (1<<11),
        VerticalHeightLeft         = (1<<12),     // WAVE: "Top Front Left"
        VerticalHeightCenter       = (1<<13),     // WAVE: "Top Front Center"
        VerticalHeightRight        = (1<<14),     // WAVE: "Top Front Right"
        TopBackLeft                = (1<<15),
        TopBackCenter              = (1<<16),
        TopBackRight               = (1<<17),
    }
    
    [Flags]
    public enum AudioChannelFlags {
        AllOff                   = 0,
        RectangularCoordinates   = (1<<0),
        SphericalCoordinates     = (1<<1),
        Meters                   = (1<<2),
    }
    
    public enum AudioChannelCoordinates {
        LeftRight  = 0,
        BackFront  = 1,
        DownUp     = 2,
        Azimuth    = 0,
        Elevation  = 1,
        Distance   = 2,
    }
    
    public enum AudioChannelLayoutTag : uint{
        // Some channel abbreviations used below:
        // L - left
        // R - right
        // C - center
        // Ls - left surround
        // Rs - right surround
        // Cs - center surround
        // Rls - rear left surround
        // Rrs - rear right surround
        // Lw - left wide
        // Rw - right wide
        // Lsd - left surround direct
        // Rsd - right surround direct
        // Lc - left center
        // Rc - right center
        // Ts - top surround
        // Vhl - vertical height left
        // Vhc - vertical height center
        // Vhr - vertical height right
        // Lt - left matrix total. for matrix encoded stereo.
        // Rt - right matrix total. for matrix encoded stereo.

        //  General layouts
        UseChannelDescriptions   = (0<<16) | 0,     // use the array of AudioChannelDescriptions to define the mapping.
        UseChannelBitmap         = (1<<16) | 0,     // use the bitmap to define the mapping.

        Mono                     = (100<<16) | 1,   // a standard mono stream
        Stereo                   = (101<<16) | 2,   // a standard stereo stream (L R) - implied playback
        StereoHeadphones         = (102<<16) | 2,   // a standard stereo stream (L R) - implied headphone playbac
        MatrixStereo             = (103<<16) | 2,   // a matrix encoded stereo stream (Lt, Rt)
        MidSide                  = (104<<16) | 2,   // mid/side recording
        XY                       = (105<<16) | 2,   // coincident mic pair (often 2 figure 8's)
        Binaural                 = (106<<16) | 2,   // binaural stereo (left, right)
        Ambisonic_B_Format       = (107<<16) | 4,   // W, X, Y, Z

        Quadraphonic             = (108<<16) | 4,   // L R Ls Rs  -- 90 degree speaker separation
        Pentagonal               = (109<<16) | 5,   // L R Ls Rs C  -- 72 degree speaker separation
        Hexagonal                = (110<<16) | 6,   // L R Ls Rs C Cs  -- 60 degree speaker separation
        Octagonal                = (111<<16) | 8,   // L R Ls Rs C Cs Lw Rw  -- 45 degree speaker separation
        Cube                     = (112<<16) | 8,   // left, right, rear left, rear right
                                                                           // top left, top right, top rear left, top rear right

        //  MPEG defined layouts
        MPEG_1_0                 = AudioChannelLayoutTag.Mono,         //  C
        MPEG_2_0                 = AudioChannelLayoutTag.Stereo,       //  L R
        MPEG_3_0_A               = (113<<16) | 3,                       //  L R C
        MPEG_3_0_B               = (114<<16) | 3,                       //  C L R
        MPEG_4_0_A               = (115<<16) | 4,                       //  L R C Cs
        MPEG_4_0_B               = (116<<16) | 4,                       //  C L R Cs
        MPEG_5_0_A               = (117<<16) | 5,                       //  L R C Ls Rs
        MPEG_5_0_B               = (118<<16) | 5,                       //  L R Ls Rs C
        MPEG_5_0_C               = (119<<16) | 5,                       //  L C R Ls Rs
        MPEG_5_0_D               = (120<<16) | 5,                       //  C L R Ls Rs
        MPEG_5_1_A               = (121<<16) | 6,                       //  L R C LFE Ls Rs
        MPEG_5_1_B               = (122<<16) | 6,                       //  L R Ls Rs C LFE
        MPEG_5_1_C               = (123<<16) | 6,                       //  L C R Ls Rs LFE
        MPEG_5_1_D               = (124<<16) | 6,                       //  C L R Ls Rs LFE
        MPEG_6_1_A               = (125<<16) | 7,                       //  L R C LFE Ls Rs Cs
        MPEG_7_1_A               = (126<<16) | 8,                       //  L R C LFE Ls Rs Lc Rc
        MPEG_7_1_B               = (127<<16) | 8,                       //  C Lc Rc L R Ls Rs LFE    (doc: IS-13818-7 MPEG2-AAC Table 3.1)
        MPEG_7_1_C               = (128<<16) | 8,                       //  L R C LFE Ls Rs Rls Rrs
        Emagic_Default_7_1       = (129<<16) | 8,                       //  L R Ls Rs C LFE Lc Rc
        SMPTE_DTV                = (130<<16) | 8,                       //  L R C LFE Ls Rs Lt Rt
    																						   //      (ITU_5_1 plus a matrix encoded stereo mix)

        //  ITU defined layouts
        ITU_1_0                  = AudioChannelLayoutTag.Mono,         //  C
        ITU_2_0                  = AudioChannelLayoutTag.Stereo,       //  L R

        ITU_2_1                  = (131<<16) | 3,                       //  L R Cs
        ITU_2_2                  = (132<<16) | 4,                       //  L R Ls Rs
        ITU_3_0                  = AudioChannelLayoutTag.MPEG_3_0_A,   //  L R C
        ITU_3_1                  = AudioChannelLayoutTag.MPEG_4_0_A,   //  L R C Cs

        ITU_3_2                  = AudioChannelLayoutTag.MPEG_5_0_A,   //  L R C Ls Rs
        ITU_3_2_1                = AudioChannelLayoutTag.MPEG_5_1_A,   //  L R C LFE Ls Rs
        ITU_3_4_1                = AudioChannelLayoutTag.MPEG_7_1_C,   //  L R C LFE Ls Rs Rls Rrs

        // DVD defined layouts
        DVD_0                    = AudioChannelLayoutTag.Mono,         // C (mono)
        DVD_1                    = AudioChannelLayoutTag.Stereo,       // L R
        DVD_2                    = AudioChannelLayoutTag.ITU_2_1,      // L R Cs
        DVD_3                    = AudioChannelLayoutTag.ITU_2_2,      // L R Ls Rs
        DVD_4                    = (133<<16) | 3,                       // L R LFE
        DVD_5                    = (134<<16) | 4,                       // L R LFE Cs
        DVD_6                    = (135<<16) | 5,                       // L R LFE Ls Rs
        DVD_7                    = AudioChannelLayoutTag.MPEG_3_0_A,   // L R C
        DVD_8                    = AudioChannelLayoutTag.MPEG_4_0_A,   // L R C Cs
        DVD_9                    = AudioChannelLayoutTag.MPEG_5_0_A,   // L R C Ls Rs
        DVD_10                   = (136<<16) | 4,                       // L R C LFE
        DVD_11                   = (137<<16) | 5,                       // L R C LFE Cs
        DVD_12                   = AudioChannelLayoutTag.MPEG_5_1_A,   // L R C LFE Ls Rs
        // 13 through 17 are duplicates of 8 through 12.
        DVD_13                   = AudioChannelLayoutTag.DVD_8,        // L R C Cs
        DVD_14                   = AudioChannelLayoutTag.DVD_9,        // L R C Ls Rs
        DVD_15                   = AudioChannelLayoutTag.DVD_10,       // L R C LFE
        DVD_16                   = AudioChannelLayoutTag.DVD_11,       // L R C LFE Cs
        DVD_17                   = AudioChannelLayoutTag.DVD_12,       // L R C LFE Ls Rs
        DVD_18                   = (138<<16) | 5,                       // L R Ls Rs LFE
        DVD_19                   = AudioChannelLayoutTag.MPEG_5_0_B,   // L R Ls Rs C
        DVD_20                   = AudioChannelLayoutTag.MPEG_5_1_B,   // L R Ls Rs C LFE

        // These layouts are recommended for AudioUnit usage
            // These are the symmetrical layouts
        AudioUnit_4              = AudioChannelLayoutTag.Quadraphonic,
        AudioUnit_5              = AudioChannelLayoutTag.Pentagonal,
        AudioUnit_6              = AudioChannelLayoutTag.Hexagonal,
        AudioUnit_8              = AudioChannelLayoutTag.Octagonal,
            // These are the surround-based layouts
        AudioUnit_5_0            = AudioChannelLayoutTag.MPEG_5_0_B,   // L R Ls Rs C
        AudioUnit_6_0            = (139<<16) | 6,                       // L R Ls Rs C Cs
        AudioUnit_7_0            = (140<<16) | 7,                       // L R Ls Rs C Rls Rrs
        AudioUnit_7_0_Front      = (148<<16) | 7,                       // L R Ls Rs C Lc Rc
        AudioUnit_5_1            = AudioChannelLayoutTag.MPEG_5_1_A,   // L R C LFE Ls Rs
        AudioUnit_6_1            = AudioChannelLayoutTag.MPEG_6_1_A,   // L R C LFE Ls Rs Cs
        AudioUnit_7_1            = AudioChannelLayoutTag.MPEG_7_1_C,   // L R C LFE Ls Rs Rls Rrs
        AudioUnit_7_1_Front      = AudioChannelLayoutTag.MPEG_7_1_A,   // L R C LFE Ls Rs Lc Rc

        AAC_3_0                  = AudioChannelLayoutTag.MPEG_3_0_B,   // C L R
        AAC_Quadraphonic         = AudioChannelLayoutTag.Quadraphonic, // L R Ls Rs
        AAC_4_0                  = AudioChannelLayoutTag.MPEG_4_0_B,   // C L R Cs
        AAC_5_0                  = AudioChannelLayoutTag.MPEG_5_0_D,   // C L R Ls Rs
        AAC_5_1                  = AudioChannelLayoutTag.MPEG_5_1_D,   // C L R Ls Rs Lfe
        AAC_6_0                  = (141<<16) | 6,                       // C L R Ls Rs Cs
        AAC_6_1                  = (142<<16) | 7,                       // C L R Ls Rs Cs Lfe
        AAC_7_0                  = (143<<16) | 7,                       // C L R Ls Rs Rls Rrs
        AAC_7_1                  = AudioChannelLayoutTag.MPEG_7_1_B,   // C Lc Rc L R Ls Rs Lfe
        AAC_Octagonal            = (144<<16) | 8,                       // C L R Ls Rs Rls Rrs Cs

        TMH_10_2_std             = (145<<16) | 16,                      // L R C Vhc Lsd Rsd Ls Rs Vhl Vhr Lw Rw Csd Cs LFE1 LFE2
        TMH_10_2_full            = (146<<16) | 21,                      // TMH_10_2_std plus: Lc Rc HI VI Haptic

        AC3_1_0_1                = (149<<16) | 2,                       // C LFE
        AC3_3_0                  = (150<<16) | 3,                       // L C R
        AC3_3_1                  = (151<<16) | 4,                       // L C R Cs
        AC3_3_0_1                = (152<<16) | 4,                       // L C R LFE
        AC3_2_1_1                = (153<<16) | 4,                       // L R Cs LFE
        AC3_3_1_1                = (154<<16) | 5,                       // L C R Cs LFE

        DiscreteInOrder          = (147<<16) | 0,                       // needs to be ORed with the actual number of channels  
        Unknown                  = 0xFFFF0000,                          // needs to be ORed with the actual number of channels  
    }
 }
 