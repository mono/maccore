// 
// AudioSessions.cs:
//
// Authors:
//    Miguel de Icaza (miguel@novell.com)
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
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;

using OSStatus = System.Int32;
using AudioQueueParameterID = System.UInt32;
using AudioQueueParameterValue = System.Single;
using AudioQueueRef = System.IntPtr;
using AudioQueueTimelineRef = System.IntPtr;

namespace MonoMac.AudioToolbox {

	public class AudioSessionException : Exception {
		static string Lookup (int k)
		{
			switch ((AudioSessionErrors)k){
			case AudioSessionErrors.NotInitialized:
				return "AudioSession.Initialize has not been called";
					
			case AudioSessionErrors.AlreadyInitialized:
				return "You called AudioSession.Initialize more than once";
			
			case AudioSessionErrors.InitializationError:
				return "There was an error during the AudioSession.initialization";
				
			case AudioSessionErrors.UnsupportedPropertyError:
				return "The audio session property is not supported";
				
			case AudioSessionErrors.BadPropertySizeError:
				return "The size of the audio property was not correct";
				
			case AudioSessionErrors.NotActiveError:
				return "Application Audio Session is not active";
				
			case AudioSessionErrors.NoHardwareError:
				return "The device has no Audio Input capability";
				
			case AudioSessionErrors.IncompatibleCategory:
				return "The specified AudioSession.Category can not be used with this audio operation";
				
			case AudioSessionErrors.NoCategorySet:
				return "This operation requries AudioSession.Category to be explicitly set";
				
			}
			return String.Format ("Unknown error code: 0x{0:x}", k);
		}
		
		internal AudioSessionException (int k) : base (Lookup (k))
		{
		}
	}
	
	public class AudioSessionPropertyEventArgs :EventArgs {
		public AudioSessionPropertyEventArgs (AudioSessionProperty prop, int size, IntPtr data)
		{
			this.Property = prop;
			this.Size = size;
			this.Data = data;
		}
		public AudioSessionProperty Property { get; set; }
		public int Size  { get; set; }
		public IntPtr Data { get; set; }
	}
	
	public static class AudioSession {
		static bool initialized;
		public static event EventHandler Interrupted;
		public static event EventHandler Resumed;


		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioSessionInitialize(IntPtr cfRunLoop, IntPtr cfstr_runMode, InterruptionListener listener, IntPtr userData);
		
		public static void Initialize ()
		{
			Initialize (null, null);
		}

		public static void Initialize (CFRunLoop runLoop, string runMode)
		{
			if (initialized)
				return;

			CFString s = runMode == null ? null : new CFString (runMode);
			int k = AudioSessionInitialize (runLoop == null ? IntPtr.Zero : runLoop.Handle, s == null ? IntPtr.Zero : s.Handle, Interruption, IntPtr.Zero);
			if (k != 0)
				throw new AudioSessionException (k);
			
			initialized = true;
		}

		delegate void InterruptionListener (IntPtr userData, uint state);

		[MonoPInvokeCallback (typeof (InterruptionListener))]
		static void Interruption (IntPtr userData, uint state)
		{
			EventHandler h;

			h = (state == 1) ? Interrupted : Resumed;
			if (h != null)
				h (null, EventArgs.Empty);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioSessionSetActive (int active);
		public static void SetActive (bool active)
		{
			int k = AudioSessionSetActive (active ? 1 : 0);
			if (k != 0)
				throw new AudioSessionException (k);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioSessionGetProperty(AudioSessionProperty id, ref int size, IntPtr data);

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioSessionSetProperty (AudioSessionProperty id, int size, IntPtr data);

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioSessionGetPropertySize (AudioSessionProperty id, out int size);

		static double GetDouble (AudioSessionProperty property)
		{
			unsafe {
				double val = 0;
				int size = 8;
				int k = AudioSessionGetProperty (property, ref size, (IntPtr) (&val));
				if (k != 0)
					throw new AudioSessionException (k);

				return val;
			}
		}

		static float GetFloat (AudioSessionProperty property)
		{
			unsafe {
				float val = 0;
				int size = 4;
				int k = AudioSessionGetProperty (property, ref size, (IntPtr) (&val));
				if (k != 0)
					throw new AudioSessionException (k);
				return val;
			}
		}

		static int GetInt (AudioSessionProperty property)
		{
			unsafe {
				int val = 0;
				int size = 4;
				int k = AudioSessionGetProperty (property, ref size, (IntPtr) (&val));
				if (k != 0)
					throw new AudioSessionException (k);
				
				return val;
			}
		}
		
		static void SetDouble (AudioSessionProperty property, double val)
		{
			unsafe {
				int k = AudioSessionSetProperty (property, 8, (IntPtr) (&val));
				if (k != 0)
					throw new AudioSessionException (k);
			}
		}

		static void SetInt (AudioSessionProperty property, int val)
		{
			unsafe {
				int k = AudioSessionSetProperty (property, 4, (IntPtr) (&val));
				if (k != 0)
					throw new AudioSessionException (k);
			}
		}

		static void SetFloat (AudioSessionProperty property, float val)
		{
			unsafe {
				int k = AudioSessionSetProperty (property, 4, (IntPtr) (&val));
				if (k != 0)
					throw new AudioSessionException (k);
			}
		}
		
		static public double PreferredHardwareSampleRate {
			get {
				return GetDouble (AudioSessionProperty.PreferredHardwareSampleRate);
			}
			set {
				SetDouble (AudioSessionProperty.PreferredHardwareSampleRate, value);
			}
		}

		static public float PreferredHardwareIOBufferDuration {
			get {
				return GetFloat (AudioSessionProperty.PreferredHardwareIOBufferDuration);
			}
			set {
				SetFloat (AudioSessionProperty.PreferredHardwareIOBufferDuration, value);
			}
		}

		static public AudioSessionCategory Category {
			get {
				return (AudioSessionCategory) GetInt (AudioSessionProperty.AudioCategory);
			}
			set {
				SetInt (AudioSessionProperty.AudioCategory, (int) value);
			}
		}

		[Since (4,0)]
		public static AudioSessionInterruptionType InterruptionType {
			get {
				return (AudioSessionInterruptionType) GetInt (AudioSessionProperty.InterruptionType);
			}
		}

		static public string AudioRoute {
			get {
				return CFString.FetchString ((IntPtr) GetInt (AudioSessionProperty.AudioRoute));
			}
		}

		static public double CurrentHardwareSampleRate {
			get {
				return GetDouble (AudioSessionProperty.CurrentHardwareSampleRate);
			}
		}

		static public int CurrentHardwareInputNumberChannels {
			get {
				return GetInt (AudioSessionProperty.CurrentHardwareInputNumberChannels);
			}
		}

		static public int CurrentHardwareOutputNumberChannels {
			get {
				return GetInt (AudioSessionProperty.CurrentHardwareOutputNumberChannels);
			}
		}

		static public float CurrentHardwareOutputVolume {
			get {
				return GetFloat (AudioSessionProperty.CurrentHardwareOutputVolume);
			}
		}

		static public float CurrentHardwareInputLatency {
			get {
				return GetFloat (AudioSessionProperty.CurrentHardwareInputLatency);
			}
		}
		
		static public float CurrentHardwareOutputLatency {
			get {
				return GetFloat (AudioSessionProperty.CurrentHardwareOutputLatency);
			}
		}
		
		static public float CurrentHardwareIOBufferDuration {
			get {
				return GetFloat (AudioSessionProperty.CurrentHardwareIOBufferDuration);
			}
		}

		static public bool OtherAudioIsPlaying {
			get {
				return GetInt (AudioSessionProperty.OtherAudioIsPlaying) != 0;
			}
		}

		static public AudioSessionRoutingOverride RoutingOverride {
			set {
				SetInt (AudioSessionProperty.OverrideAudioRoute, (int) value);
			}
		}

		static public bool AudioInputAvailable {
			get {
				return GetInt (AudioSessionProperty.AudioInputAvailable) != 0;
			}
		}

		static public bool AudioShouldDuck {
			get {
				return GetInt (AudioSessionProperty.OtherMixableAudioShouldDuck) != 0;
			}
			set {
				SetInt (AudioSessionProperty.OtherMixableAudioShouldDuck, value ? 1 : 0);
			}
		}

		static public bool OverrideCategoryMixWithOthers {
			get {
				return GetInt (AudioSessionProperty.OverrideCategoryMixWithOthers) != 0;
			}
			set {
				SetInt (AudioSessionProperty.OverrideCategoryMixWithOthers, value ? 1 : 0);
			}
		}

		static public AudioSessionMode Mode {
			get {
				return (AudioSessionMode) GetInt (AudioSessionProperty.Mode);
			}
			set {
				SetInt (AudioSessionProperty.Mode, (int) value);
			}
		}
		
		static public bool OverrideCategoryDefaultToSpeaker {
			get {
				return GetInt (AudioSessionProperty.OverrideCategoryDefaultToSpeaker) != 0;
			}
			set {
				SetInt (AudioSessionProperty.OverrideCategoryDefaultToSpeaker, value ? 1 : 0);
			}
		}

		static public bool OverrideCategoryEnableBluetoothInput {
			get {
				return GetInt (AudioSessionProperty.OverrideCategoryEnableBluetoothInput) != 0;
			}
			set {
				SetInt (AudioSessionProperty.OverrideCategoryEnableBluetoothInput, value ? 1 : 0);
			}
		}
		

		delegate void _PropertyListener (IntPtr userData, AudioSessionProperty prop, int size, IntPtr data);
		public delegate void PropertyListener (AudioSessionProperty prop, int size, IntPtr data);
		
		[MonoPInvokeCallback (typeof (_PropertyListener))]
		static void Listener (IntPtr userData, AudioSessionProperty prop, int size, IntPtr data)
		{
			ArrayList a = (ArrayList) listeners [prop];
			if (a == null){
				// Should never happen
				return;
			}

			foreach (PropertyListener pl in a){
				pl (prop, size, data);
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioSessionAddPropertyListener(AudioSessionProperty id, _PropertyListener inProc, IntPtr userData);

		static Hashtable listeners;

		public static void AddListener (AudioSessionProperty property, PropertyListener listener)
		{
			if (listener == null)
				throw new ArgumentNullException ("listener");

			if (listeners == null)
				listeners = new Hashtable ();

			ArrayList a = (ArrayList) listeners [property];
			if (a == null)
				listeners [property] = a = new ArrayList ();

			a.Add (listener);

			if (a.Count == 1)
				AudioSessionAddPropertyListener (property, Listener, IntPtr.Zero);
		}

		public static void RemoveListener (AudioSessionProperty property, PropertyListener listener)
		{
			if (listener == null)
				throw new ArgumentNullException ("listener");

			ArrayList a = (ArrayList) listeners [property];
			if (a == null)
				return;
			a.Remove (listener);
			if (a.Count == 0)
				listeners [property] = null;
		}

	}
}
