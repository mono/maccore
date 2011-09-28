// 
// AudioServices.cs:
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
using System.Runtime.InteropServices;

using MonoMac.Foundation;

namespace MonoMac.AudioToolbox {

	enum AudioServicesError {
		None = 0,
		UnsupportedProperty = 0x7074793f, // 'pty?'
		BadPropertySize = 0x2173697a, // '!siz'
		BadSpecifierSizeError = 0x21737063, // '!spc'
		SystemSoundUnspecifiedError = -1500,
		SystemSoundClientTimedOutError = -1501
	}
		
	enum AudioServiceProperty {
		IsUISound                 = 0x69737569, // 'isui'
		CompletePlaybackIfAppDies = 0x69666469, // 'ifdi'
	}

	delegate void SoundCompletionProc (int systemSoundId, IntPtr clientData);

	enum AudioServicesProperty {
		IsUISound = 0x69737569, // 'isui'
		CompletePlaybackIfAppDies = 0x69666469 // 'ifdi'
	}
	
	public enum AudioSessionInterruptionType {
		ShouldResume = 1769108333, // 'irsm'
		ShouldNotResume = 561148781, // '!rsm'
	}

#if false
	public static class AudioServices {

		

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern int AudioServicesAddSystemSoundCompletion (uint soundId, CFRunLoopRef runLoop, NSString runLoopMode, SystemSoundCompletionCallback completeionRoutine, IntPtr clientData);

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern void AudioServicesRemoveSystemSoundCompletion (uint soundId);

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern int AudioServicesGetPropertyInfo (uint propertyId, uint specifierSize, IntPtr specifier, out uint propertyDataSize, out bool writable);

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern int AudioServicesGetProperty (uint propertyId, uint specifierSize, IntPtr specifier, out uint propertyDataSize, IntPtr propertyData);

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern int AudioServicesSetProperty (uint propertyId, uint specifierSize, IntPtr specifier, uint propertyDataSize, IntPtr propertyData);
	}
#endif
}

