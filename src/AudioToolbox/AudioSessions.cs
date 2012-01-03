// 
// AudioSessions.cs:
//
// Authors: Mono Team
//     
// Copyright 2009 Novell, Inc
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
using System.Runtime.InteropServices;
using MonoMac.CoreFoundation;

namespace MonoMac.AudioToolbox {

	public enum AudioSessionErrors {
		None                      = 0,
		NotInitialized            = 0x21696e69, // '!ini',
		AlreadyInitialized        = 0x696e6974, // 'init',
		InitializationError       = 0x696e693f, // 'ini?',
		UnsupportedPropertyError  = 0x7074793f, // 'pty?',
		BadPropertySizeError      = 0x2173697a, // '!siz',
		NotActiveError            = 0x21616374, // '!act',
		NoHardwareError           = 0x6e6f6877, // 'nohw'
		IncompatibleCategory      = 0x21636174, // '!cat'
		NoCategorySet             = 0x3f636174, // '?cat'
	}

	public enum AudioSessionInterruptionState {
		End   = 0,
		Begin = 1,
	}

	public enum AudioSessionCategory {
		AmbientSound      = 0x616d6269, // 'ambi'
		SoloAmbientSound  = 0x736f6c6f, // 'solo'
		MediaPlayback     = 0x6d656469, // 'medi'
		RecordAudio       = 0x72656361, // 'reca'
		PlayAndRecord     = 0x706c6172, // 'plar'
		AudioProcessing   = 0x70726f63  // 'proc'
	}

	public enum AudioSessionRoutingOverride {
		None    = 0,
		Speaker = 0x73706b72, // 'spkr'
	}

	public enum AudioSessionRouteChangeReason {
		Unknown               = 0,
		NewDeviceAvailable    = 1,
		OldDeviceUnavailable  = 2,
		CategoryChange        = 3,
		Override              = 4,
		WakeFromSleep         = 6,
	}

	public enum AudioSessionProperty {
		PreferredHardwareSampleRate = 0x68777372,
		PreferredHardwareIOBufferDuration = 0x696f6264,
		AudioCategory = 0x61636174,
		AudioRoute = 0x726f7574,
		AudioRouteDescription = 0x63726172, // 'crar' 
		AudioRouteChange = 0x726f6368,
		CurrentHardwareSampleRate = 0x63687372,
		CurrentHardwareInputNumberChannels = 0x63686963,
		CurrentHardwareOutputNumberChannels = 0x63686f63,
		CurrentHardwareOutputVolume = 0x63686f76,
		CurrentHardwareInputLatency = 0x63696c74,
		CurrentHardwareOutputLatency = 0x636f6c74,
		CurrentHardwareIOBufferDuration = 0x63686264,
		OtherAudioIsPlaying = 0x6f746872,
		OverrideAudioRoute = 0x6f767264,
		AudioInputAvailable = 0x61696176,
		ServerDied = 0x64696564,
		OtherMixableAudioShouldDuck = 0x6475636b,
		OverrideCategoryMixWithOthers = 0x636d6978,
		OverrideCategoryDefaultToSpeaker = 0x6373706b, //'cspk'
		OverrideCategoryEnableBluetoothInput = 0x63626c75, //'cblu'
		Mode = 0x6d6f6465,
		InterruptionType = 0x2172736d
	}

	public enum AudioSessionMode {
		Default = 0x64666c74,
		VoiceChat = 0x76636374,
		VideoRecording = 0x76726364,
		Measurement = 0x6d736d74
	}
	
	public enum AudioSessionInputRouteKind {
		None,
		LineIn,
		BuiltInMic,
		HeadsetMic,
		BluetoothHFP,
		USBAudio,
	}
	
	public enum AudioSessionOutputRouteKind {
		None,
		LineOut,
		Headphones,
		BluetoothHFP,
		BluetoothA2DP,
		BuiltInReceiver,
		BuiltInSpeaker,
		USBAudio,
		HDMI,
		AirPlay,
	}
}

