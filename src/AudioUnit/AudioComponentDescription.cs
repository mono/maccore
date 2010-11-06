//
// AudioComponentDescription.cs: AudioComponentDescription wrapper class
//
// Author:
//   AKIHIRO Uehara (u-akihiro@reinforce-lab.com)
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2010 Reinforce Lab.
// Copyright 2010 Novell, Inc
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using MonoMac.AudioToolbox;

 namespace MonoMac.AudioUnit
{
        public enum AudioComponentType {
		Output = 0x61756f75, //'auou',
		MusicDevice=0x61756d75, // 'aumu'
		MusicEffect=0x61756d66, // 'aumf'
		FormatConverter=0x61756663, // 'aufc'
		Effect=0x61756678, // 'aufx'
		Mixer=0x61756d78, // 'aumx'
		Panner=0x6175706e, // 'aupn'
		OfflineEffect=0x61756f6c, // 'auol'
		Generator=0x6175676e, // 'augn'
	}

	public enum AudioComponentSubType {
		OutputGeneric=0x67656e72, // 'genr'
#if MONOMAC
		OutputHAL=0x6168616c, // 'ahal'
		OutputDefault=0x64656620, // 'def'
		OutputSystem=0x73797320, // 'sys'
#else
		OutputRemote=0x72696f63, // 'rioc'
#endif

#if MONOMAC
		MusicDeviceDLSSynth=0x646c7320, // 'dls'
#endif
			
		ConverterAU=0x636f6e76, // 'conv'
#if MONOMAC
		ConverterVarispeed=0x76617269, // 'vari'
		ConverterDeferredRenderer=0x64656672, // 'defr'
		ConverterTimePitch=0x746d7074, // 'tmpt'
		ConverterSplitter=0x73706c74, // 'splt'
		ConverterMerger=0x6d657267, // 'merg'
#else
		ConverterAUiPodTime=0x6970746d, // 'iptm'
#endif

#if MONOMAC
		EffectDelay=0x64656c79, // 'dely'
		EffectLowPassFilter=0x6c706173, // 'lpas'
		EffectHighPassFilter=0x68706173, // 'hpas'
		EffectBandPassFilter=0x62706173, // 'bpas'
		EffectHighShelfFilter=0x68736866, // 'hshf'
		EffectLowShelfFilter=0x6c736866, // 'lshf'
		EffectParametricEQ=0x706d6571, // 'pmeq'
		EffectGraphicEQ=0x67726571, // 'greq'
		EffectPeakLimiter=0x6c6d7472, // 'lmtr'
		EffectDynamicsProcessor=0x64636d70, // 'dcmp'
		EffectMultiBandCompressor=0x6d636d70, // 'mcmp'
		EffectMatrixReverb=0x6d726576, // 'mrev'
		EffectSampleDelay=0x73646c79, // 'sdly'
		EffectPitch=0x70697463, // 'pitc'
		EffectAUFilter=0x66696c74, // 'filt'
		EffectNetSend=0x6e736e64, // 'nsnd'
		EffectDistortion=0x64697374, // 'dist'
		EffectRogerBeep=0x726f6772, // 'rogr'
#else
		EffectAUiPodEQ=0x69706571, // 'ipeq'
#endif
			
		MixerMultiChannel=0x6d636d78, // 'mcmx'
#if MONOMAC
		MixerStereo=0x736d7872, // 'smxr'
		Mixer3D=0x33646d78, // '3dmx'
		MixerMatrix=0x6d786d78, // 'mxmx'
#else
		Mixer3DEmbedded=0x3364656d, // '3dem'
#endif

#if MONOMAC
		PannerSphericalHead=0x73706872, // 'sphr'
		PannerVector=0x76626173, // 'vbas'
		PannerSoundField=0x616d6269, // 'ambi'
		PannerHRTF=0x68727466, // 'hrtf'
			
		GeneratorScheduledSoundPlayer=0x7373706c, // 'sspl'
		GeneratorAudioFilePlayer=0x6166706c, // 'afpl'
		GeneratorNetReceive=0x6e726376, // 'nrcv'
#endif
        }
        
        public enum AudioComponentManufacturerType {
		Apple = 0x6170706c // little endian 0x6c707061 //'appl'
        }
	
	[StructLayout(LayoutKind.Sequential)]
	public class AudioComponentDescription {
		[MarshalAs(UnmanagedType.U4)] 
		public AudioComponentType ComponentType;
		
		[MarshalAs(UnmanagedType.U4)]
		public AudioComponentSubType ComponentSubType;
        
		[MarshalAs(UnmanagedType.U4)] 
		public AudioComponentManufacturerType ComponentManufacturer;

		public int ComponentFlags;
		public int ComponentFlagsMask;

		public AudioComponentDescription () {}

		public AudioComponentDescription (AudioComponentType type, AudioComponentSubType subType)
		{
			ComponentType = type;
			ComponentSubType = subType;
			ComponentManufacturer = AudioComponentManufacturerType.Apple;
			//ComponentFlags = 0;
			//ComponentFlagsMask = 0;
		}

		public override string ToString ()
		{
			return string.Format ("[componetType={0}, subType={2}]", ComponentType, ComponentSubType);
		}
	}
}
