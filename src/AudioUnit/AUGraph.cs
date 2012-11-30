//
// AUGraph.cs: AUGraph wrapper class
//
// Authors:
//   AKIHIRO Uehara (u-akihiro@reinforce-lab.com)
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2010 Reinforce Lab.
// Copyright 2010 Novell, Inc.
// Copyright 2012 Xamarin Inc
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
using System.Runtime.InteropServices;
using MonoMac.AudioToolbox;

namespace MonoMac.AudioUnit
{
	public enum AUGraphError
	{
		OK = 0,
		NodeNotFound 				= -10860,
		InvalidConnection 			= -10861,
		OutputNodeError				= -10862,
		CannotDoInCurrentContext	= -10863,
		InvalidAudioUnit			= -10864,

		// Values returned & shared with other error enums
		FormatNotSupported			= -10868,
	}

	public class AUGraph: IDisposable 
	{
		#region Variables
		readonly GCHandle gcHandle;
		readonly IntPtr handle;
		#endregion

		#region Properties
		public event EventHandler<AudioGraphEventArgs> RenderCallback;
		public IntPtr Handler { get { return handle; } }
		#endregion

		#region Constructor
		private AUGraph(IntPtr ptr)
		{
			handle = ptr;
			
			gcHandle = GCHandle.Alloc(this);            
			int err = AUGraphAddRenderNotify(handle, renderCallback, GCHandle.ToIntPtr(gcHandle));
			if (err != 0)
				throw new ArgumentException(String.Format("Error code: {0}", err));
		}

		public AUGraph ()
		{
			int err = NewAUGraph (ref handle);
			if (err != 0)
				throw new InvalidOperationException(String.Format("Cannot create new AUGraph. Error code:", err));
		}
		
		#endregion
			
		#region Private methods
		// callback funtion should be static method and be attatched a MonoPInvokeCallback attribute.        
		[MonoMac.MonoPInvokeCallback(typeof(AudioUnit.AURenderCallback))]
		static int renderCallback(IntPtr inRefCon,
					  ref AudioUnitRenderActionFlags _ioActionFlags,
					  ref AudioTimeStamp _inTimeStamp,
					  int _inBusNumber,
					  int _inNumberFrames,
					  AudioBufferList _ioData)
		{
			// getting audiounit instance
			var handler = GCHandle.FromIntPtr(inRefCon);
			var inst = (AUGraph)handler.Target;
			
			// invoke event handler with an argument
			if (inst.RenderCallback != null){
				var args = new AudioGraphEventArgs(
					_ioActionFlags,
					_inTimeStamp,
					_inBusNumber,
					_inNumberFrames,
					_ioData);
				inst.RenderCallback(inst, args);
			}
			
			return 0; // noerror
		}
		#endregion

		#region Public methods
		public void Open ()
		{ 
			int err = AUGraphOpen (handle);
			if (err != 0)
				throw new InvalidOperationException(String.Format("Cannot open AUGraph. Error code:", err));
		}

		public int TryOpen ()
		{ 
			int err = AUGraphOpen (handle);
			return err;
		}
		
		public int AddNode (AudioComponentDescription cd)
		{
			int node;
			var err = AUGraphAddNode (handle, cd, out node);
			if (err != 0)
				throw new ArgumentException(String.Format("Error code:", err));
			
			return node;
		}
		
		public AudioUnit GetNodeInfo (int node)
		{
			IntPtr ptr;
			var err = AUGraphNodeInfo(handle, node, IntPtr.Zero, out ptr);
			if (err != 0)
				throw new ArgumentException(String.Format("Error code:{0}", err));

			if (ptr == IntPtr.Zero)
				throw new InvalidOperationException("Can not get object instance");
			
			return new AudioUnit (ptr);
		}

		public void ConnnectNodeInput(int inSourceNode, uint inSourceOutputNumber, int inDestNode, uint inDestInputNumber)
		{
			int err = AUGraphConnectNodeInput(handle,                                    
							  inSourceNode, inSourceOutputNumber,                                    
							  inDestNode, inDestInputNumber                                    
				);
			if (err != 0)
				throw new ArgumentException(String.Format("Error code:", err));            
		}

		public AUGraphError Start()
		{
			return AUGraphStart (handle);
		}

		public AUGraphError Stop()
		{
			return AUGraphStop (handle);
		}
		
		public AUGraphError Initialize()
		{
			return AUGraphInitialize (handle);
		}

		#endregion

		public void Dispose()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				AUGraphUninitialize (handle);
				AUGraphClose (handle);
				DisposeAUGraph (handle);
				
				gcHandle.Free();
			}
		}

		~AUGraph ()
		{
			Dispose (false);
		}
			
		[DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "NewAUGraph")]
		static extern int NewAUGraph(ref IntPtr outGraph);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "AUGraphOpen")]
		static extern int AUGraphOpen(IntPtr inGraph);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphAddNode(IntPtr inGraph, AudioComponentDescription inDescription, out int outNode);
	
		[DllImport(MonoMac.Constants.AudioToolboxLibrary)]
		static extern AUGraphError AUGraphNodeInfo(IntPtr inGraph, int inNode, IntPtr outDescription, out IntPtr outAudioUnit);
	
	        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "AUGraphConnectNodeInput")]
	        static extern int AUGraphConnectNodeInput(IntPtr inGraph, int inSourceNode, uint inSourceOutputNumber, int inDestNode, uint inDestInputNumber);
	
	        [DllImport(MonoMac.Constants.AudioToolboxLibrary)]
	        static extern AUGraphError AUGraphInitialize (IntPtr inGraph);
	
	        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "AUGraphAddRenderNotify")]
	        static extern int AUGraphAddRenderNotify(IntPtr inGraph, AudioUnit.AURenderCallback inCallback, IntPtr inRefCon );
	
	        [DllImport(MonoMac.Constants.AudioToolboxLibrary)]
	        static extern AUGraphError AUGraphStart (IntPtr inGraph);
	
	        [DllImport(MonoMac.Constants.AudioToolboxLibrary)]
	        static extern AUGraphError AUGraphStop (IntPtr inGraph);
	
	        [DllImport(MonoMac.Constants.AudioToolboxLibrary)]
	        static extern AUGraphError AUGraphUninitialize (IntPtr inGraph);
	
	        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "AUGraphClose")]
	        static extern int AUGraphClose(IntPtr inGraph);
	
	        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "DisposeAUGraph")]
	        static extern int DisposeAUGraph(IntPtr inGraph);
	}
}
