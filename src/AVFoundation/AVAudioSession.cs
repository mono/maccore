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
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using System;

namespace MonoMac.AVFoundation {

	public partial class AVAudioSession {

		[Obsolete ("Use SetActive(bool, out NSError) instead")]
		public bool SetActive (bool beActive, NSError outError)
		{
			// Effectively discarded due to original binding error
			return SetActive (beActive, out outError);
		}

		public bool SetActive (bool beActive, out NSError outError)
		{
			unsafe {
				IntPtr errhandle;
				IntPtr ptrtohandle = (IntPtr) (&errhandle);

				if (SetActive (beActive, ptrtohandle)){
					outError = null;
					return true;
				} else {
					outError = (NSError) Runtime.GetNSObject (errhandle);
					return false;
				}
			}			
		}

		[Since (4,0)]
		public bool SetActive (bool beActive, AVAudioSessionFlags flags, out NSError outError)
		{
			unsafe {
				IntPtr errhandle;
				IntPtr ptrtohandle = (IntPtr) (&errhandle);

				if (_SetActive (beActive, (int) flags, ptrtohandle)){
					outError = null;
					return true;
				} else {
					outError = (NSError) Runtime.GetNSObject (errhandle);
					return false;
				}
			}			
		}

		[Obsolete ("Use SetCategory(bool, out NSError) instead")]
		public bool SetCategory (NSString theCategory, NSError outError)
		{
			// Effectively discarded due to original binding error
			return SetCategory (theCategory, out outError);
		}

		public bool SetCategory (NSString theCategory, out NSError outError)
		{
			unsafe {
				IntPtr errhandle;
				IntPtr ptrtohandle = (IntPtr) (&errhandle);

				if (SetCategory (theCategory, ptrtohandle)){
					outError = null;
					return true;
				} else {
					outError = (NSError) Runtime.GetNSObject (errhandle);
					return false;
				}
			}			
		}

		[Obsolete ("Use SetPreferredHardwareSampleRate(bool, out NSError) instead")]
		public bool SetPreferredHardwareSampleRate (double sampleRate, NSError outError)
		{
			// Effectively discarded due to original binding error
			return SetPreferredHardwareSampleRate (sampleRate, out outError);
		}

		public bool SetPreferredHardwareSampleRate (double sampleRate, out NSError outError)
		{
			unsafe {
				IntPtr errhandle;
				IntPtr ptrtohandle = (IntPtr) (&errhandle);

				if (SetPreferredHardwareSampleRate (sampleRate, ptrtohandle)){
					outError = null;
					return true;
				} else {
					outError = (NSError) Runtime.GetNSObject (errhandle);
					return false;
				}
			}			
		}

		[Obsolete ("Use SetPreferredIOBufferDuration(bool, out NSError) instead")]
		public bool SetPreferredIOBufferDuration (double duration, NSError outError)
		{
			// Effectively discarded due to original binding error
			return SetPreferredIOBufferDuration (duration, out outError);
		}

		public bool SetPreferredIOBufferDuration (double duration, out NSError outError)
		{
			unsafe {
				IntPtr errhandle;
				IntPtr ptrtohandle = (IntPtr) (&errhandle);

				if (SetPreferredIOBufferDuration (duration, ptrtohandle)){
					outError = null;
					return true;
				} else {
					outError = (NSError) Runtime.GetNSObject (errhandle);
					return false;
				}
			}			
		}
		
	}
}
