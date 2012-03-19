// 
// AudioSessions.cs:
//
// Authors:
//    Miguel de Icaza (miguel@novell.com)
//    AKIHIRO Uehara (u-akihiro@reinforce-lab.com)
//     
// Copyright 2009, 2010 Novell, Inc
// Copyright 2010, Reinforce Lab.
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

using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;

namespace MonoMac.AudioToolbox {
	public enum AudioFormatType {
		LinearPCM               = 0x6c70636d,
		AC3                     = 0x61632d33,
		CAC3                    = 0x63616333,
		AppleIMA4               = 0x696d6134,
		MPEG4AAC                = 0x61616320,
		MPEG4CELP               = 0x63656c70,
		MPEG4HVXC               = 0x68767863,
		MPEG4TwinVQ             = 0x74777671,
		MACE3                   = 0x4d414333,
		MACE6                   = 0x4d414336,
		ULaw                    = 0x756c6177,
		ALaw                    = 0x616c6177,
		QDesign                 = 0x51444d43,
		QDesign2                = 0x51444d32,
		QUALCOMM                = 0x51636c70,
		MPEGLayer1              = 0x2e6d7031,
		MPEGLayer2              = 0x2e6d7032,
		MPEGLayer3              = 0x2e6d7033,
		TimeCode                = 0x74696d65,
		MIDIStream              = 0x6d696469,
		ParameterValueStream    = 0x61707673,
		AppleLossless           = 0x616c6163,
		MPEG4AAC_HE             = 0x61616368,
		MPEG4AAC_LD             = 0x6161636c,
		MPEG4AAC_HE_V2          = 0x61616370,
		MPEG4AAC_Spatial        = 0x61616373,
		AMR                     = 0x73616d72,
		Audible                 = 0x41554442,
		iLBC                    = 0x696c6263,
		DVIIntelIMA             = 0x6d730011,
		MicrosoftGSM            = 0x6d730031,
		AES3                    = 0x61657333,
		MPEG4AAC_ELD            = 0x61616365,
		//[Since (5,1)]
		MPEG4AAC_ELD_V2         = 0x61616367,			// 'aacg'
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
		FlagsAreAllClear            = (1 << 31),

		LinearPCMIsFloat                     = (1 << 0),     // 0x1
		LinearPCMIsBigEndian                 = (1 << 1),     // 0x2
		LinearPCMIsSignedInteger             = (1 << 2),     // 0x4
		LinearPCMIsPacked                    = (1 << 3),     // 0x8
		LinearPCMIsAlignedHigh               = (1 << 4),     // 0x10
		LinearPCMIsNonInterleaved            = (1 << 5),     // 0x20
		LinearPCMIsNonMixable                = (1 << 6),     // 0x40

		LinearPCMSampleFractionShift    = 7,
		LinearPCMSampleFractionMask     = (0x3F << LinearPCMSampleFractionShift),
		LinearPCMFlagsAreAllClear            = FlagsAreAllClear,
    
		AppleLossless16BitSourceData    = 1,
		AppleLossless20BitSourceData    = 2,
		AppleLossless24BitSourceData    = 3,
		AppleLossless32BitSourceData    = 4
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AudioStreamBasicDescription {
		public double SampleRate;

		[Obsolete ("Use the strongly-typed Format property instead")]
		public int FormatID {
			get {
				return (int) Format;
			}
			set {
				Format = (AudioFormatType) value;
			}
		}

		public AudioFormatType Format;
		public AudioFormatFlags FormatFlags;
		public int BytesPerPacket;
		public int FramesPerPacket;
		public int BytesPerFrame;
		public int ChannelsPerFrame;
		public int BitsPerChannel;
		public int Reserved;

		public override string ToString ()
		{
			return String.Format ("[SampleRate={0} FormatID={1} FormatFlags={2} BytesPerPacket={3} FramesPerPacket={4} BytesPerFrame={5} ChannelsPerFrame={6} BitsPerChannel={7}]",
					      SampleRate, Format, FormatFlags, BytesPerPacket, FramesPerPacket, BytesPerFrame, ChannelsPerFrame, BitsPerChannel);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AudioStreamPacketDescription {
		public long  StartOffset;
		public int  VariableFramesInPacket;
		public int  DataByteSize;

		public override string ToString ()
		{
			return String.Format ("StartOffset={0} VariableFramesInPacket={1} DataByteSize={2}", StartOffset, VariableFramesInPacket, DataByteSize);
		}
	}

	[Flags]
	public enum AudioChannelFlags {
		AllOff = 0,
		RectangularCoordinates = 1 << 0,
		SphericalCoordinates = 1 << 1,
		Meters = 1 << 2
	}

	public enum AudioChannelLabel {
		Unknown               = -1,
		Unused                = 0,
		UseCoordinates        = 100,
		
		Left                  = 1,
		Right                 = 2,
		Center                = 3,
		LFEScreen             = 4,
		LeftSurround          = 5,
		RightSurround         = 6,
		LeftCenter            = 7,
		RightCenter           = 8,
		CenterSurround        = 9,
		LeftSurroundDirect    = 10,
		RightSurroundDirect   = 11,
		TopCenterSurround     = 12,
		VerticalHeightLeft    = 13,
		VerticalHeightCenter  = 14,
		VerticalHeightRight   = 15,
		TopBackLeft           = 16,
		TopBackCenter         = 17,
		TopBackRight          = 18,
		RearSurroundLeft      = 33,
		RearSurroundRight     = 34,
		LeftWide              = 35,
		RightWide             = 36,
		LFE2                  = 37,
		LeftTotal             = 38,
		RightTotal            = 39,
		HearingImpaired       = 40,
		Narration             = 41,
		Mono                  = 42,
		DialogCentricMix      = 43,
		CenterSurroundDirect  = 44,
		Haptic                = 45,
   
		// first order ambisonic channels
		Ambisonic_W           = 200,
		Ambisonic_X           = 201,
		Ambisonic_Y           = 202,
		Ambisonic_Z           = 203,
   
		// Mid/Side Recording
		MS_Mid                = 204,
		MS_Side               = 205,
   
		// X-Y Recording
		XY_X                  = 206,
		XY_Y                  = 207,
   
		// other
		HeadphonesLeft        = 301,
		HeadphonesRight       = 302,
		ClickTrack            = 304,
		ForeignLanguage       = 305,
   
		// generic discrete channel
	        Discrete              = 400,
   
	   	// numbered discrete channel
		Discrete_0            = (1<<16) | 0,
		Discrete_1            = (1<<16) | 1,
		Discrete_2            = (1<<16) | 2,
		Discrete_3            = (1<<16) | 3,
		Discrete_4            = (1<<16) | 4,
		Discrete_5            = (1<<16) | 5,
		Discrete_6            = (1<<16) | 6,
		Discrete_7            = (1<<16) | 7,
		Discrete_8            = (1<<16) | 8,
		Discrete_9            = (1<<16) | 9,
		Discrete_10           = (1<<16) | 10,
		Discrete_11           = (1<<16) | 11,
		Discrete_12           = (1<<16) | 12,
		Discrete_13           = (1<<16) | 13,
		Discrete_14           = (1<<16) | 14,
		Discrete_15           = (1<<16) | 15,
		Discrete_65535        = (1<<16) | 65535
	}
	
	public class AudioChannelDescription {
		public AudioChannelLabel  Label;
		public AudioChannelFlags  Flags;
		public float [] Coords;

		public override string ToString ()
		{
			return String.Format ("[id={0} {1} - {2},{3},{4}", Label, Flags, Coords [0], Coords[1], Coords[2]);
		}
	}

	public enum AudioChannelLayoutTag {
		UseChannelDescriptions   = (0<<16) | 0,     
		UseChannelBitmap         = (1<<16) | 0,     
		
		Mono                     = (100<<16) | 1,   
		Stereo                   = (101<<16) | 2,   
		StereoHeadphones         = (102<<16) | 2,   
		MatrixStereo             = (103<<16) | 2,   
		MidSide                  = (104<<16) | 2,   
		XY                       = (105<<16) | 2,   
		Binaural                 = (106<<16) | 2,   
		Ambisonic_B_Format       = (107<<16) | 4,   
		
		Quadraphonic             = (108<<16) | 4,   
		Pentagonal               = (109<<16) | 5,   
		Hexagonal                = (110<<16) | 6,   
		Octagonal                = (111<<16) | 8,   
		Cube                     = (112<<16) | 8,   
		                                            
		
		MPEG_1_0                 = Mono,         
		MPEG_2_0                 = Stereo,       
		MPEG_3_0_A               = (113<<16) | 3,                       
		MPEG_3_0_B               = (114<<16) | 3,                       
		MPEG_4_0_A               = (115<<16) | 4,                       
		MPEG_4_0_B               = (116<<16) | 4,                       
		MPEG_5_0_A               = (117<<16) | 5,                       
		MPEG_5_0_B               = (118<<16) | 5,                       
		MPEG_5_0_C               = (119<<16) | 5,                       
		MPEG_5_0_D               = (120<<16) | 5,                       
		MPEG_5_1_A               = (121<<16) | 6,                       
		MPEG_5_1_B               = (122<<16) | 6,                       
		MPEG_5_1_C               = (123<<16) | 6,                       
		MPEG_5_1_D               = (124<<16) | 6,                       
		MPEG_6_1_A               = (125<<16) | 7,                       
		MPEG_7_1_A               = (126<<16) | 8,                       
		MPEG_7_1_B               = (127<<16) | 8,                       
		MPEG_7_1_C               = (128<<16) | 8,                       
		Emagic_Default_7_1       = (129<<16) | 8,                       
		SMPTE_DTV                = (130<<16) | 8,                       
		
		ITU_1_0                  = Mono,         
		ITU_2_0                  = Stereo,       
		
		ITU_2_1                  = (131<<16) | 3,                       
		ITU_2_2                  = (132<<16) | 4,                       
		ITU_3_0                  = MPEG_3_0_A,   
		ITU_3_1                  = MPEG_4_0_A,   
		
		ITU_3_2                  = MPEG_5_0_A,   
		ITU_3_2_1                = MPEG_5_1_A,   
		ITU_3_4_1                = MPEG_7_1_C,   
		
		
		DVD_0                    = Mono,         
		DVD_1                    = Stereo,       
		DVD_2                    = ITU_2_1,      
		DVD_3                    = ITU_2_2,      
		DVD_4                    = (133<<16) | 3,                       
		DVD_5                    = (134<<16) | 4,                       
		DVD_6                    = (135<<16) | 5,                       
		DVD_7                    = MPEG_3_0_A,   
		DVD_8                    = MPEG_4_0_A,   
		DVD_9                    = MPEG_5_0_A,   
		DVD_10                   = (136<<16) | 4,                       
		DVD_11                   = (137<<16) | 5,                       
		DVD_12                   = MPEG_5_1_A,   

		DVD_13                   = DVD_8,        
		DVD_14                   = DVD_9,        
		DVD_15                   = DVD_10,       
		DVD_16                   = DVD_11,       
		DVD_17                   = DVD_12,       
		DVD_18                   = (138<<16) | 5,                       
		DVD_19                   = MPEG_5_0_B,   
		DVD_20                   = MPEG_5_1_B,   
		
		AudioUnit_4              = Quadraphonic,
		AudioUnit_5              = Pentagonal,
		AudioUnit_6              = Hexagonal,
		AudioUnit_8              = Octagonal,

		AudioUnit_5_0            = MPEG_5_0_B,   
		AudioUnit_6_0            = (139<<16) | 6,                       
		AudioUnit_7_0            = (140<<16) | 7,                       
		AudioUnit_7_0_Front      = (148<<16) | 7,                       
		AudioUnit_5_1            = MPEG_5_1_A,   
		AudioUnit_6_1            = MPEG_6_1_A,   
		AudioUnit_7_1            = MPEG_7_1_C,   
		AudioUnit_7_1_Front      = MPEG_7_1_A,   
		
		AAC_3_0                  = MPEG_3_0_B,   
		AAC_Quadraphonic         = Quadraphonic, 
		AAC_4_0                  = MPEG_4_0_B,   
		AAC_5_0                  = MPEG_5_0_D,   
		AAC_5_1                  = MPEG_5_1_D,   
		AAC_6_0                  = (141<<16) | 6,                       
		AAC_6_1                  = (142<<16) | 7,                       
		AAC_7_0                  = (143<<16) | 7,                       
		AAC_7_1                  = MPEG_7_1_B,   
		AAC_Octagonal            = (144<<16) | 8,                       
		
		TMH_10_2_std             = (145<<16) | 16,                      
		TMH_10_2_full            = (146<<16) | 21,                      
		
		AC3_1_0_1                = (149<<16) | 2,                       
		AC3_3_0                  = (150<<16) | 3,                       
		AC3_3_1                  = (151<<16) | 4,                       
		AC3_3_0_1                = (152<<16) | 4,                       
		AC3_2_1_1                = (153<<16) | 4,                       
		AC3_3_1_1                = (154<<16) | 5,                       
		
		EAC_6_0_A                = (155<<16) | 6,                       
		EAC_7_0_A                = (156<<16) | 7,                       
		
		EAC3_6_1_A               = (157<<16) | 7,                       
		EAC3_6_1_B               = (158<<16) | 7,                       
		EAC3_6_1_C               = (159<<16) | 7,                       
		EAC3_7_1_A               = (160<<16) | 8,                       
		EAC3_7_1_B               = (161<<16) | 8,                       
		EAC3_7_1_C               = (162<<16) | 8,                       
		EAC3_7_1_D               = (163<<16) | 8,                       
		EAC3_7_1_E               = (164<<16) | 8,                       
		
		EAC3_7_1_F               = (165<<16) | 8,                        
		EAC3_7_1_G               = (166<<16) | 8,                        
		EAC3_7_1_H               = (167<<16) | 8,                        
		
		DTS_3_1                  = (168<<16) | 4,                        
		DTS_4_1                  = (169<<16) | 5,                        
		DTS_6_0_A                = (170<<16) | 6,                        
		DTS_6_0_B                = (171<<16) | 6,                        
		DTS_6_0_C                = (172<<16) | 6,                        
		DTS_6_1_A                = (173<<16) | 7,                        
		DTS_6_1_B                = (174<<16) | 7,                        
		DTS_6_1_C                = (175<<16) | 7,                        
		DTS_7_0                  = (176<<16) | 7,                        
		DTS_7_1                  = (177<<16) | 8,                        
		DTS_8_0_A                = (178<<16) | 8,                        
		DTS_8_0_B                = (179<<16) | 8,                        
		DTS_8_1_A                = (180<<16) | 9,                        
		DTS_8_1_B                = (181<<16) | 9,                        
		DTS_6_1_D                = (182<<16) | 7,                         
		
		DiscreteInOrder          = (147<<16) | 0,                       // needs to be ORed with the actual number of channels  
		Unknown                  = unchecked ((int)(0xFFFF0000))                           // needs to be ORed with the actual number of channels  
	}
	
	public class AudioChannelLayout {
		[Obsolete ("Use the strongly typed enum AudioTag instead")]
		public int Tag { get { return (int) AudioTag; } set { AudioTag = (AudioChannelLayoutTag) value; } }
		public AudioChannelLayoutTag AudioTag;
		public int Bitmap;
		public AudioChannelDescription [] Channels ;

		static internal AudioChannelLayout FromHandle (IntPtr h)
		{
			var layout = new AudioChannelLayout ();
			layout.AudioTag  = (AudioChannelLayoutTag) Marshal.ReadInt32 (h, 0);
			layout.Bitmap = Marshal.ReadInt32 (h, 4);
			layout.Channels = new AudioChannelDescription [Marshal.ReadInt32 (h, 8)];
			int p = 12;
			for (int i = 0; i < layout.Channels.Length; i++){
				var desc = new AudioChannelDescription ();
				desc.Label = (AudioChannelLabel) Marshal.ReadInt32 (h, p);
				desc.Flags = (AudioChannelFlags) Marshal.ReadInt32 (h, p+4);
				desc.Coords = new float [3];
				desc.Coords [0] = ReadFloat (h, p+8);
				desc.Coords [1] = ReadFloat (h, p+12);
				desc.Coords [2] = ReadFloat (h, p+16);
				layout.Channels [i] = desc;
				
				p += 20;
			}

			return layout;
		}
		
		public override string ToString ()
		{
			return String.Format ("AudioChannelLayout: Tag={0} Bitmap={1} Channels={2}", AudioTag, Bitmap, Channels.Length);
		}

		unsafe static float ReadFloat (IntPtr p, int offset)
		{
			byte *src = ((byte *)p) + offset;

			return *(float *) src;
		}

		unsafe static void WriteFloat (IntPtr p, int offset, float f)
		{
			byte *dest = ((byte *)p) + offset;
			*((float *) dest) = f;
		}
		
		// The returned block must be released with FreeHGbloal
		static internal IntPtr ToBlock (AudioChannelLayout layout, out int size)
		{
			if (layout == null)
				throw new ArgumentNullException ("layout");
			if (layout.Channels == null)
				throw new ArgumentNullException ("layout.Channels");
			
			size = 12 + layout.Channels.Length * 20;
			IntPtr buffer = Marshal.AllocHGlobal (size);
			int p;
			Marshal.WriteInt32 (buffer, 0, (int) layout.AudioTag);
			Marshal.WriteInt32 (buffer, 4, layout.Bitmap);
			Marshal.WriteInt32 (buffer, 8, layout.Channels.Length);
			p = 12;
			foreach (var desc in layout.Channels){
				Marshal.WriteInt32 (buffer, p, (int) desc.Label);
				Marshal.WriteInt32 (buffer, p + 4, (int) desc.Flags);
				WriteFloat (buffer, p + 8, desc.Coords [0]);
				WriteFloat (buffer, p + 12, desc.Coords [1]);
				WriteFloat (buffer, p + 16, desc.Coords [2]);

				p += 20;
			}
			
			return buffer;
		}

		public NSData AsData ()
		{
			int size;
			
			var p = ToBlock (this, out size);
			var result = NSData.FromBytes (p, (uint) size);
			Marshal.FreeHGlobal (p);
			return result;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SmpteTime {
		public short Subframes;
		public short SubframeDivisor;
		public uint  Counter;
		public uint  Type;
		public uint  Flags;
		public short Hours;
		public short Minutes;
		public short Seconds;
		public short Frames;

		public override string ToString ()
		{
			return String.Format ("[Subframes={0},Divisor={1},Counter={2},Type={3},Flags={4},Hours={5},Minutes={6},Seconds={7},Frames={8}]",
					      Subframes, SubframeDivisor, Counter, Type, Flags, Hours, Minutes, Seconds, Frames);
		}
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
	public struct AudioBuffer {
		public int NumberChannels;
		public int DataByteSize;
		public IntPtr Data;

		public override string ToString ()
		{
			return string.Format ("[channels={0},dataByteSize={1},ptrData=0x{2:x}]", NumberChannels, DataByteSize, Data);
		}
	}
}

