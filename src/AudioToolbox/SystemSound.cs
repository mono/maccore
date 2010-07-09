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
using MonoMac.ObjCRuntime;

namespace MonoMac.AudioToolbox {

	enum AudioServiceErrors {
		None                      = 0,
		UnsupportedProperty       = 0x7074793f, // 'pty?'
		BadPropertySize           = 0x2173697a, // '!siz'
		BadSpecifierSize          = 0x21737063, // '!spc'
		SystemSoundUnspecified    = -1500,
		SystemSoundClientTimedOut = -1501,
	}

	delegate void SystemSoundCompletionCallback (uint ssID, IntPtr clientData);

	enum SystemSoundId : uint {
		Vibrate = 0x00000FFF,
	}

	public class SystemSound : INativeObject, IDisposable {

		public static readonly SystemSound Vibrate = new SystemSound ((uint) SystemSoundId.Vibrate, false);

		uint soundId;
		bool ownsHandle;

		internal SystemSound (uint soundId, bool ownsHandle)
		{
			this.soundId = soundId;
			this.ownsHandle = ownsHandle;
		}

		~SystemSound ()
		{
			Dispose (false);
		}

		public IntPtr Handle {
			get {
				AssertNotDisposed ();
				return (IntPtr) soundId;
			}
		}

		void AssertNotDisposed ()
		{
			if (soundId == 0)
				throw new ObjectDisposedException ("SystemSound");
		}

		void IDisposable.Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected virtual void Dispose (bool disposing)
		{
			Cleanup (false);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern AudioServiceErrors AudioServicesDisposeSystemSoundID (uint soundId);

		void Cleanup (bool checkForError)
		{
			if (soundId == 0 || !ownsHandle)
				return;
			var error = AudioServicesDisposeSystemSoundID (soundId);
			var oldId = soundId;
			soundId = 0;
			if (checkForError && error != AudioServiceErrors.None) {
				throw new InvalidOperationException (string.Format ("Error while disposing SystemSound with ID {0}: {1}",
							oldId, error.ToString()));
			}
		}

		public void Close ()
		{
			Cleanup (true);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern void AudioServicesPlayAlertSound (uint inSystemSoundID);
		public void PlayAlertSound ()
		{
			AssertNotDisposed ();
			AudioServicesPlayAlertSound (soundId);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern void AudioServicesPlaySystemSound(uint inSystemSoundID);
		public void PlaySystemSound ()
		{
			AssertNotDisposed ();
			AudioServicesPlaySystemSound (soundId);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern AudioServiceErrors AudioServicesCreateSystemSoundID (IntPtr fileUrl, out uint soundId);

		public SystemSound (NSUrl fileUrl)
		{
			var error = AudioServicesCreateSystemSoundID (fileUrl.Handle, out soundId);
			if (error != AudioServiceErrors.None)
				throw new InvalidOperationException (string.Format ("Could not create system sound ID for url {0}; error={1}",
							fileUrl, error));
			ownsHandle = true;
		}
			
		public static SystemSound FromFile (NSUrl fileUrl)
		{
			uint soundId;
			var error = AudioServicesCreateSystemSoundID (fileUrl.Handle, out soundId);
			if (error != AudioServiceErrors.None)
				return null;
			return new SystemSound (soundId, true);
		}

		public static SystemSound FromFile (string filename)
		{
			using (var url = new NSUrl (filename)){
				uint soundId;
				var error = AudioServicesCreateSystemSoundID (url.Handle, out soundId);
				if (error != AudioServiceErrors.None)
					return null;
				return new SystemSound (soundId, true);
			}
		}
	}
}
