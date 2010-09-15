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


namespace MonoMac.AudioUnitFramework {
	
	#region General Declarations
	public enum AUGroupParameterID : uint {
		Volume					= 7,	// value 0 < 128
		Sustain					= 64, 	// value 0-63 (off), 64-127 (on)
		AllNotesOff				= 123,	// value ignored
		ModWheel				= 1,	// value 0 < 128
		PitchBend				= 0xE0,	// value -8192 - 8191
		AllSoundOff				= 120,	// value ignored
		ResetAllControllers		= 121,	// value ignored
		Pan						= 10,	// value 0 < 128
		Foot					= 4,	// value 0 < 128
		ChannelPressure			= 0xD0,	// value 0 < 128
		KeyPressure				= 0xA0,	// values 0 < 128
		Expression				= 11,	// value 0 < 128
		DataEntry				= 6,	// value 0 < 128
		
		Volume_LSB				= Volume + 32,		// value 0 < 128
		ModWheel_LSB			= ModWheel + 32,	// value 0 < 128
		Pan_LSB					= Pan + 32,			// value 0 < 128
		Foot_LSB				= Foot + 32,		// value 0 < 128
		Expression_LSB			= Expression + 32,	// value 0 < 128
		DataEntry_LSB			= DataEntry + 32,	// value 0 < 128
		
		KeyPressure_FirstKey	= 256,	// value 0 < 128
		KeyPressure_LastKey		= 383	// value 0 < 128
	}
	
	public enum PannerParam : uint {
		Gain = 0,			// 0 .. 1
		
		Azimuth = 1,		// -180 .. +180 degrees
		Elevation = 2,		// -90 .. +90 degrees
		Distance = 3,		// 0 .. 1
		
		CoordScale = 4,	// 0.01 .. 1000 meters
		RefDistance = 5,	// 0.01 .. 1000 meters
	}
#endregion
	
	#region Apple Specific
	public enum ThreeDMixerParam : uint {
		// Input, Degrees, -180->180, 0
		Azimuth		= 0,
		
		// Input, Degrees, -90->90, 0
		Elevation		= 1,
		
		// Input, Metres, 0->10000, 1
		Distance		= 2,
		
		// Input/Output, dB, -120->20, 0
		Gain			= 3,
		
		// Input, rate scaler	0.5 -> 2.0
		PlaybackRate	= 4,
		
		// Input, Dry/Wet equal-power blend, %	  0.0 -> 100.0
		ReverbBlend		= 5,
		
		// Global, dB,		-40.0 -> +40.0
		GlobalReverbGain	= 6,
		
		// Input, Lowpass filter attenuation at 5KHz :		decibels -100.0dB -> 0.0dB
		// smaller values make sound more muffled; a value of 0.0 indicates no filtering
		OcclusionAttenuation	= 7,
		
		// Input, Lowpass filter attenuation at 5KHz :		decibels -100.0dB -> 0.0dB
		// smaller values make sound more muffled; a value of 0.0 indicates no filtering
		ObstructionAttenuation = 8,
		
		// read-only
		//
		// For each of the following, use the parameter ID plus the channel number
		// to get the specific parameter ID for a given channel.
		// For example, PostAveragePower indicates the left channel
		// while PostAveragePower + 1 indicates the right channel.
		PreAveragePower	= 1000,
		PrePeakHoldLevel	= 2000,
		PostAveragePower	= 3000,
		PostPeakHoldLevel	= 4000	
	}
	
	// Parameters for the AUMultiChannelMixer unit
	public enum MultiChannelMixerParam : uint {
		Volume 	= 0,
		Enable 	= 1,
		
		// read-only
		// these report level in dB, as do the other mixers
		PreAveragePower		= 1000,
		PrePeakHoldLevel	= 2000,
		PostAveragePower	= 3000,
		PostPeakHoldLevel	= 4000
	}
	
	// Output Units
	// Parameters for the AudioDeviceOutput, DefaultOutputUnit, and SystemOutputUnit units
	public enum HALOutputParam : uint {
		// Global, LinearGain, 0->1, 1
		Volume 		= 14 
	}
	
	// Parameters for the AUTimePitch, AUTimePitch (offline), AUPitch units
	public enum TimePitchParam : uint {
		Rate						= 0,
		
		Pitch						= 1,
		EffectBlend					= 2		// only for the AUPitch unit
	}

	#endregion
	
	#region Apple Specific - Desktop
	public enum BandpassParam : uint {
		// Global, Hz, 20->(SampleRate/2), 5000
		CenterFrequency 			= 0,
		
		// Global, Cents, 100->12000, 600
		_Bandwidth 				= 1
	}
	
	// Some parameters for the AUGraphicEQ unit
	public enum GraphicEQParam : uint {
		// Global, Indexed, currently either 10 or 31
		NumberOfBands 			= 10000
	}
	
	// Parameters for the AUHipass unit
	public enum HipassParam : uint {
		// Global, Hz, 10->(SampleRate/2), 6900
		CutoffFrequency 			= 0,
		
		// Global, dB, -20->40, 0
		Resonance					= 1
	}

	// Parameters for the AULowpass unit
	public enum LowPassParam : uint {
		// Global, Hz, 10->(SampleRate/2), 6900
		CutoffFrequency 			= 0,
		
		// Global, dB, -20->40, 0
		Resonance 				= 1
	}
	
	// Parameters for the AUHighShelfFilter unit
	public enum HighShelfParam : uint {
		// Global, Hz, 10000->(SampleRate/2), 10000
		CutOffFrequency 		= 0,
		
		// Global, dB, -40->40, 0
		Gain 					= 1
	}
	
	// Parameters for the AULowShelfFilter unit
	public enum AULowShelfParam : uint {
		// Global, Hz, 10->200, 80
		CutoffFrequency = 0,
		
		// Global, dB, -40->40, 0
		Gain = 1
	}

	// Parameters for the AUParametricEQ unit
	public enum ParametricEQParam : uint {
		// Global, Hz, 20->(SampleRate/2), 2000
		CenterFreq = 0,
		
		// Global, Hz, 0.1->20, 1.0
		Q = 1,
		
		// Global, dB, -20->20, 0
		Gain = 2
	}
	
	// Parameters for the AUMatrixReverb unit
	public enum ReverbParam : uint {
		// Global, EqPow CrossFade, 0->100, 100
		DryWetMix 							= 0,
		
		// Global, EqPow CrossFade, 0->100, 50
		SmallLargeMix						= 1,
		
		// Global, Secs, 0.005->0.020, 0.06
		SmallSize							= 2,
		
		// Global, Secs, 0.4->10.0, 3.07
		LargeSize							= 3,
		
		// Global, Secs, 0.001->0.03, 0.025
		PreDelay							= 4,
		
		// Global, Secs, 0.001->0.1, 0.035
		LargeDelay							= 5,
		
		// Global, Genr, 0->1, 0.28
		SmallDensity						= 6,
		
		// Global, Genr, 0->1, 0.82
		LargeDensity						= 7,
		
		// Global, Genr, 0->1, 0.3
		LargeDelayRange					= 8,
		
		// Global, Genr, 0.1->1, 0.96
		SmallBrightness					= 9,
		
		// Global, Genr, 0.1->1, 0.49
		LargeBrightness					= 10,
		
		// Global, Genr, 0->1 0.5
		SmallDelayRange					= 11,
		
		// Global, Hz, 0.001->2.0, 1.0
		ModulationRate						= 12,
		
		// Global, Genr, 0.0 -> 1.0, 0.2
		ModulationDepth					= 13,
		
		// Global, Hertz, 10.0 -> 20000.0, 800.0
		FilterFrequency					= 14,
		
		// Global, Octaves, 0.05 -> 4.0, 3.0
		FilterBandwidth					= 15,
		
		// Global, Decibels, -18.0 -> +18.0, 0.0
		FilterGain							= 16
}

	// Parameters for the AUDelay unit
	public enum DelayParam : uint {
		// Global, EqPow Crossfade, 0->100, 50
		WetDryMix 				= 0,
		
		// Global, Secs, 0->2, 1
		DelayTime				= 1,
		
		// Global, Percent, -100->100, 50
		Feedback 				= 2,
		
		// Global, Hz, 10->(SampleRate/2), 15000
		LopassCutoff	 		= 3
	}
	
	// Parameters for the AUPeakLimiter unit
	public enum LimiterParam : uint {
		// Global, Secs, 0.001->0.03, 0.012
		AttackTime 			= 0,
		
		// Global, Secs, 0.001->0.06, 0.024
		DecayTime 			= 1,
		
		// Global, dB, -40->40, 0
		PreGain 				= 2
	}
	
	
	// Parameters for the AUDynamicsProcessor unit
	public enum DynamicsProcessorParam : uint {
		// Global, dB, -40->20, -20
		Threshold 			= 0,
		
		// Global, dB, 0.1->40.0, 5
		HeadRoom	 		= 1,
		
		// Global, rate, 1->50.0, 2
		ExpansionRatio		= 2,
		
		// Global, dB
		ExpansionThreshold	= 3,
		
		// Global, secs, 0.0001->0.2, 0.001
		AttackTime 			= 4,
		
		// Global, secs, 0.01->3, 0.05
		ReleaseTime 		= 5,
		
		// Global, dB, -40->40, 0
		MasterGain 			= 6,
		
		// Global, dB, read-only parameter
		CompressionAmount 	= 1000,
		InputAmplitude		= 2000,
		OutputAmplitude 	= 3000
	}
	
	
	// Parameters for the AUMultibandCompressor unit
	public enum MultibandCompressorParam : uint {
		Pregain 			= 0,
		Postgain 			= 1,
		Crossover1 		= 2,
		Crossover2 		= 3,
		Crossover3 		= 4,
		Threshold1 		= 5,
		Threshold2 		= 6,
		Threshold3 		= 7,
		Threshold4 		= 8,
		Headroom1 		= 9,
		Headroom2 		= 10,
		Headroom3 		= 11,
		Headroom4 		= 12,
		AttackTime 		= 13,
		ReleaseTime 		= 14,
		EQ1 				= 15,
		EQ2 				= 16,
		EQ3 				= 17,
		EQ4 				= 18,
		
		// read-only parameters
		CompressionAmount1 = 1000,
		CompressionAmount2 = 2000,
		CompressionAmount3 = 3000,
		CompressionAmount4 = 4000,
		
		InputAmplitude1 = 5000,
		InputAmplitude2 = 6000,
		InputAmplitude3 = 7000,
		InputAmplitude4 = 8000,
		
		OutputAmplitude1 = 9000,
		OutputAmplitude2 = 10000,
		OutputAmplitude3 = 11000,
		OutputAmplitude4 = 12000
	}
	
	// Parameters for the AUVarispeed unit
	public enum VarispeedPAram : uint {
		PlaybackRate				= 0,
		PlaybackCents				= 1
	}
	
	// Parameters for the AUFilter unit
	public enum MultiBandFilter
	{
		LowFilterType  = 0,
		LowFrequency   = 1,
		LowGain		= 2,
		
		CenterFreq1	= 3,
		CenterGain1	= 4,
		Bandwidth1		= 5,
		
		CenterFreq2	= 6,
		CenterGain2	= 7,
		Bandwidth2		= 8,
		
		CenterFreq3	= 9,
		CenterGain3	= 10,
		Bandwidth3		= 11,
		
		HighFilterType	= 12,
		HighFrequency  = 13,
		HighGain		= 14
	}
	
	// Mixer Units
	
	// Parameters for the Stereo Mixer unit
	public enum  StereoMixerParam : uint {
		// Input/Output, Mixer Fader Curve, 0->1, 1
		Volume 	= 0,
		
		// Input, Pan, 0->1, 0.5
		Pan		= 1,
		
		// read-only
		//
		// For each of the following, use the parameter ID for the left channel
		// and the parameter ID plus one for the right channel.
		// For example, PostAveragePower indicates the left channel
		// while kStereiMixerParam_PostAveragePower + 1 indicates the right channel.
		PreAveragePower	= 1000,
		PrePeakHoldLevel	= 2000,
		PostAveragePower	= 3000,
		PostPeakHoldLevel	= 4000
	}
	
	// Parameters for the AUMatrixMixer unit
	public enum MatrixMixerParam : uint {
		Volume 	= 0,
		Enable 	= 1,
		
		// read-only
		// these report level in dB, as do the other mixers
		PreAveragePower	= 1000,
		PrePeakHoldLevel	= 2000,
		PostAveragePower	= 3000,
		PostPeakHoldLevel	= 4000,
		
		// these report linear levels - for "expert" use only.
		PreAveragePowerLinear			= 5000,
		PrePeakHoldLevelLinear		= 6000,
		PostAveragePowerLinear		= 7000,
		PostPeakHoldLevelLinear		= 8000
	}

	// Parameters for the AUNetReceive unit
	public enum AUNetReceiveParam : uint {
		Status = 0,
		NumParameters = 1
	}
	
	// Parameters for the AUNetSend unit
	public enum AUNetSendParam : uint {
		Status = 0,
		NumParameters = 1
	}

	
	// Status values for the AUNetSend and AUNetReceive units
	public enum AUNetStatus : uint {
		NotConnected = 0,
		Connected = 1,
		Overflow = 2,
		Underflow = 3,
		Connecting = 4,
		Listening = 5
	}
	
	// Parameters for the Distortion unit 
	public enum DistortionParam : uint {
		Delay = 0,
		Decay = 1,
		DelayMix = 2,
		
		Decimation = 3,
		Rounding = 4,
		DecimationMix = 5,
		
		LinearTerm = 6,  
		SquaredTerm = 7,	
		CubicTerm = 8,  
		PolynomialMix = 9,
		
		RingModFreq1 = 10,
		RingModFreq2 = 11,
		RingModBalance = 12,
		RingModMix = 13,
		
		SoftClipGain = 14,
		
		FinalMix = 15
	}
	
	// Parameters for AURogerBeep
	public enum RogerBeepParam : uint {
		InGateThreshold = 0,
		InGateThresholdTime = 1,
		OutGateThreshold = 2,
		OutGateThresholdTime = 3,	
		Sensitivity = 4,	
		RogerType = 5,
		RogerGain = 6
	}
	
	// Music Device
	// Parameters for the DLSMusicDevice unit - defined and reported in the global scope
	public enum MusicDeviceParam : uint {
		// Global, Cents, -1200, 1200, 0
		Tuning 	= 0,
		
		// Global, dB, -120->40, 0
		Volume	= 1,
		
		// Global, dB, -120->40, 0
		ReverbVolume	= 2
	}
	
	#endregion
}