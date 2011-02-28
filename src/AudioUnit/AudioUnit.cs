//
// AudioUnit.cs: AudioUnit wrapper class
//
// Author:
//   AKIHIRO Uehara (u-akihiro@reinforce-lab.com)
//
// Copyright 2010 Reinforce Lab.
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
using System.Text;
using System.Runtime.InteropServices;
using MonoMac.AudioToolbox;

namespace MonoMac.AudioUnit
{
	public enum AudioUnitStatus {
		NoError = 0,
		ParameterError = -50,
		InvalidProperty = -10879,
		InvalidParameter = -10878,
		InvalidElement = -10877,
		NoConnection = -10876,
		FailedInitialization = -10875,
		TooManyFramesToProcess = -10874,
		InvalidFile = -10871,
		FormatNotSupported = -10868,
		Uninitialized = -10867,
		InvalidScope = -10866,
		PropertyNotWritable = -10865,
		CannotDoInCurrentContext = -10863,
		InvalidPropertyValue = -10851,
		PropertyNotInUse = -10850,
		Initialized = -10849,
		InvalidOfflineRender = -10848,
		Unauthorized = -10847,
	}
	
	public class AudioUnitException : Exception {
		static string Lookup (int k)
		{
			switch ((AudioUnitStatus)k)
			{
			case AudioUnitStatus.InvalidProperty:
				return "Invalid Property";
				
			case AudioUnitStatus.InvalidParameter :
				return "Invalid Parameter";
				
			case AudioUnitStatus.InvalidElement :
				return "Invalid Element";
				
			case AudioUnitStatus.NoConnection :
				return "No Connection";
				
			case AudioUnitStatus.FailedInitialization :
				return "Failed Initialization";
				
			case AudioUnitStatus.TooManyFramesToProcess :
				return "Too Many Frames To Process";
				
			case AudioUnitStatus.InvalidFile :
				return "Invalid File";
				
			case AudioUnitStatus.FormatNotSupported :
				return "Format Not Supported";
				
			case AudioUnitStatus.Uninitialized :
				return "Uninitialized";
				
			case AudioUnitStatus.InvalidScope :
				return "Invalid Scope";
				
			case AudioUnitStatus.PropertyNotWritable :
				return "Property Not Writable";
				
			case AudioUnitStatus.CannotDoInCurrentContext :
				return "Cannot Do In Current Context";
				
			case AudioUnitStatus.InvalidPropertyValue :
				return "Invalid Property Value";
				
			case AudioUnitStatus.PropertyNotInUse :
				return "Property Not In Use";
				
			case AudioUnitStatus.Initialized :
				return "Initialized";
				
			case AudioUnitStatus.InvalidOfflineRender :
				return "Invalid Offline Render";
				
			case AudioUnitStatus.Unauthorized :
				return "Unauthorized";
				
			}
			return String.Format ("Unknown error code: 0x{0:x}", k);
		}
		
		internal AudioUnitException (int k) : base (Lookup (k))
		{
		}
	}
	
	public class AudioUnit : IDisposable, MonoMac.ObjCRuntime.INativeObject {
		GCHandle gcHandle;
		IntPtr handle;
		bool _isPlaying;

		public IntPtr Handle { get { return handle; }}

		public event EventHandler<AudioUnitEventArgs> RenderCallback;
		//public event EventHandler<AudioUnitEventArgs> InputCallback;
		public bool IsPlaying { get { return _isPlaying; } }

		internal AudioUnit (IntPtr ptr)
		{
			handle = ptr;
		}
		
		public AudioUnit (AudioComponent component)
		{
			if (component == null)
				throw new ArgumentNullException ("component");
			if (component.Handle == IntPtr.Zero)
				throw new ObjectDisposedException ("component");
			
			int err = AudioComponentInstanceNew (component.handle, out handle);
			if (err != 0)
				throw new AudioUnitException (err);
			
			_isPlaying = false;
			
			gcHandle = GCHandle.Alloc(this);
			var callbackStruct = new AURenderCallbackStrct();
			callbackStruct.inputProc = renderCallback; // setting callback function            
			callbackStruct.inputProcRefCon = GCHandle.ToIntPtr(gcHandle); // a pointer that passed to the renderCallback (IntPtr inRefCon) 
			err = AudioUnitSetProperty(handle,
						   AudioUnitPropertyIDType.SetRenderCallback,
						   AudioUnitScopeType.Input,
						   0, // 0 == speaker                
						   callbackStruct,
						   (uint)Marshal.SizeOf(callbackStruct));
			if (err != 0)
				throw new AudioUnitException (err);
		}
		
		// callback funtion should be static method and be attatched a MonoPInvokeCallback attribute.        
		[MonoMac.MonoPInvokeCallback(typeof(AURenderCallback))]
		static int renderCallback(IntPtr inRefCon, ref AudioUnitRenderActionFlags _ioActionFlags,
					  ref AudioTimeStamp _inTimeStamp,
					  int _inBusNumber,
					  int _inNumberFrames,
					  AudioBufferList _ioData)
		{
			// getting audiounit instance
			var handler = GCHandle.FromIntPtr(inRefCon);
			var inst = (AudioUnit)handler.Target;
			
			// evoke event handler with an argument
			if (inst.RenderCallback != null)  { 
				var args = new AudioUnitEventArgs(
					_ioActionFlags,
					_inTimeStamp,
					_inBusNumber,
					_inNumberFrames,
					_ioData);
				inst.RenderCallback(inst, args);
			}
			
			return 0; // noerror
		}

		public void SetAudioFormat(MonoMac.AudioToolbox.AudioStreamBasicDescription audioFormat, AudioUnitScopeType scope, uint audioUnitElement)
		{
			int err = AudioUnitSetProperty(handle,
						       AudioUnitPropertyIDType.StreamFormat,
						       scope,
						       audioUnitElement, 
						       ref audioFormat,
						       (uint)Marshal.SizeOf(audioFormat));
			if (err != 0)
				throw new AudioUnitException (err);
		}
		public MonoMac.AudioToolbox.AudioStreamBasicDescription GetAudioFormat(AudioUnitScopeType scope, uint audioUnitElement)
		{
			MonoMac.AudioToolbox.AudioStreamBasicDescription audioFormat = new AudioStreamBasicDescription();
			uint size = (uint)Marshal.SizeOf(audioFormat);
			
			int err = AudioUnitGetProperty(handle,
						       AudioUnitPropertyIDType.StreamFormat,
						       scope,
						       audioUnitElement,
						       ref audioFormat,
						       ref size);
			if (err != 0)
				throw new AudioUnitException (err);
			
			return audioFormat;
		}
		
		public void SetEnableIO(bool enableIO, AudioUnitScopeType scope, uint audioUnitElement)
		{                                   
			uint flag = (uint)(enableIO ? 1 : 0);
			int err = AudioUnitSetProperty(handle,
						       AudioUnitPropertyIDType.EnableIO,
						       scope,
						       audioUnitElement,
						       ref flag,
						       (uint)Marshal.SizeOf(typeof(uint)));
			if (err != 0)
				throw new AudioUnitException (err);
		}
			
		public int Initialize ()
		{
			return AudioUnitInitialize(handle);
		}

		public int Uninitialize ()
		{
			return AudioUnitUnInitialize (handle);
		}

		public void Start()
		{
			if (! _isPlaying) {
				AudioOutputUnitStart(handle);
				_isPlaying = true;
			}
		}
		
		public void Stop()
		{
			if (_isPlaying) {
				AudioOutputUnitStop(handle);
				_isPlaying = false;
			}
		}

		public void Render(AudioUnitRenderActionFlags flags,
				   AudioTimeStamp timeStamp,
				   int outputBusnumber,
				   int numberFrames, AudioBufferList data)
		{
			int err = AudioUnitRender(handle,
						  ref flags,
						  ref timeStamp,
						  outputBusnumber,
						  numberFrames,
						  data);
			if (err != 0)
				throw new AudioUnitException (err);
		}

		public AudioUnitStatus TryRender(AudioUnitRenderActionFlags flags,
						AudioTimeStamp timeStamp,
						int outputBusnumber,
						int numberFrames, AudioBufferList data)
		{
			return (AudioUnitStatus) AudioUnitRender(handle,
								ref flags,
								ref timeStamp,
								outputBusnumber,
								numberFrames,
								data);
		}
		
		#region IDisposable メンバ
		public void Dispose()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}
		
		[DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "AudioComponentInstanceDispose")]
		static extern int AudioComponentInstanceDispose(IntPtr inInstance);

		public void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				Stop ();
				AudioUnitUnInitialize (handle);
				AudioComponentInstanceDispose (handle);
				gcHandle.Free();
				handle = IntPtr.Zero;
			}
		}

		/// <summary>
		/// AudioUnit call back method declaration
		/// </summary>
		internal delegate int AURenderCallback(IntPtr inRefCon,
					      ref AudioUnitRenderActionFlags ioActionFlags,
					      ref AudioTimeStamp inTimeStamp,
					      int inBusNumber,
					      int inNumberFrames,
					      AudioBufferList ioData);
		
		[StructLayout(LayoutKind.Sequential)]
		class AURenderCallbackStrct {
			public AURenderCallback inputProc;
			public IntPtr inputProcRefCon;
			
			public AURenderCallbackStrct() { }
		}    

		[DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "AudioComponentInstanceNew")]
		static extern int AudioComponentInstanceNew(IntPtr inComponent, out IntPtr inDesc);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "AudioComponentInstanceNew")]
		static extern IntPtr AudioComponentInstanceGetComponent (IntPtr inComponent);
		public AudioComponent Component {
			get {
				return new AudioComponent (AudioComponentInstanceGetComponent (handle));
			}
		}
		
		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern int AudioUnitInitialize(IntPtr inUnit);
		
		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern int AudioUnitUnInitialize(IntPtr inUnit);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern int AudioOutputUnitStart(IntPtr ci);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern int AudioOutputUnitStop(IntPtr ci);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern int AudioUnitRender(IntPtr inUnit,
						  ref AudioUnitRenderActionFlags ioActionFlags,
						  ref AudioTimeStamp inTimeStamp,
						  int inOutputBusNumber,
						  int inNumberFrames,
						  AudioBufferList ioData);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "AudioUnitSetProperty")]
		static extern int AudioUnitSetProperty(IntPtr inUnit,
						       [MarshalAs(UnmanagedType.U4)] AudioUnitPropertyIDType inID,
						       [MarshalAs(UnmanagedType.U4)] AudioUnitScopeType inScope,
						       [MarshalAs(UnmanagedType.U4)] uint inElement,
						       AURenderCallbackStrct inData,
						       uint inDataSize);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "AudioUnitSetProperty")]
		static extern int AudioUnitSetProperty(IntPtr inUnit,
						       [MarshalAs(UnmanagedType.U4)] AudioUnitPropertyIDType inID,
						       [MarshalAs(UnmanagedType.U4)] AudioUnitScopeType inScope,
						       [MarshalAs(UnmanagedType.U4)] uint inElement,
						       ref MonoMac.AudioToolbox.AudioStreamBasicDescription inData,
						       uint inDataSize);
        
		[DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "AudioUnitSetProperty")]
		static extern int AudioUnitSetProperty(IntPtr inUnit,
						       [MarshalAs(UnmanagedType.U4)] AudioUnitPropertyIDType inID,
						       [MarshalAs(UnmanagedType.U4)] AudioUnitScopeType inScope,
						       [MarshalAs(UnmanagedType.U4)] uint inElement,
						       ref uint flag,
						       uint inDataSize);
        
		[DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "AudioUnitGetProperty")]
		static extern int AudioUnitGetProperty(IntPtr inUnit,
						       [MarshalAs(UnmanagedType.U4)] AudioUnitPropertyIDType inID,
						       [MarshalAs(UnmanagedType.U4)] AudioUnitScopeType inScope,
						       [MarshalAs(UnmanagedType.U4)] uint inElement,
						       ref MonoMac.AudioToolbox.AudioStreamBasicDescription outData,
						       ref uint ioDataSize);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "AudioUnitGetProperty")]
		static extern int AudioUnitGetProperty(IntPtr inUnit,
						       [MarshalAs(UnmanagedType.U4)] AudioUnitPropertyIDType inID,
						       [MarshalAs(UnmanagedType.U4)] AudioUnitScopeType inScope,
						       [MarshalAs(UnmanagedType.U4)] uint inElement,
						       ref uint flag,
						       ref uint ioDataSize
			);
		
	}

	public enum AudioUnitPropertyIDType {
		ClassInfo = 0,
		MakeConnection = 1,
		SampleRate = 2,
		ParameterList = 3,
		ParameterInfo = 4,
		StreamFormat = 8,
		ElementCount = 11,
		Latency = 12,
		SupportedNumChannels = 13,
		MaximumFramesPerSlice = 14,
		AudioChannelLayout = 19,
		TailTime = 20,
		BypassEffect = 21,
		LastRenderError = 22,
		SetRenderCallback = 23,
		FactoryPresets = 24,
		RenderQuality = 26,
		InPlaceProcessing = 29,
		ElementName = 30,
		SupportedChannelLayoutTags = 32,
		PresentPreset = 36,
		ShouldAllocateBuffer = 51,

		//Output property
	        CurrentDevice			= 2000,
	        ChannelMap				= 2002, // this will also work with AUConverter
	        EnableIO				= 2003,
	        StartTime				= 2004,
	        SetInputCallback		= 2005,
	        HasIO					= 2006,
	        StartTimestampsAtZero  = 2007	// this will also work with AUConverter
        };
        
        public enum AudioUnitScopeType {
		Global = 0,
		Input = 1,
		Output = 2
        }

        [Flags]
        public enum AudioUnitRenderActionFlags {
		PreRender = (1 << 2),
		PostRender = (1 << 3),
		OutputIsSilence = (1 << 4),
		OfflinePreflight = (1 << 5),
		OfflineRender = (1 << 6),
		OfflineComplete = (1 << 7),
		PostRenderError = (1 << 8)
        };
        #endregion    
	
}
