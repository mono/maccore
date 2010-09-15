using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using MonoMac.AudioUnitFramework;
using MonoMac.CoreAudio;

using OSStatus                  = System.Int32;
using OSType                    = System.UInt32;
using UInt8                     = System.Byte;
using UInt32                    = System.UInt32;
using Float32                   = System.Single;
using Float64                   = System.Double;
using SInt16                    = System.Int16;
using SInt64                    = System.Int64;
using Boolean                   = System.Boolean; // Historic MacOS type with Sizeof() == 1

using AUNode                    = System.Int32;
using AudioUnitElement          = System.UInt32;

namespace MonoMac.AudioToolbox {
    public enum AUGraphErrors {
        NodeNotFound 				= -10860,
    	InvalidConnection 			= -10861,
    	OutputNodeErr				= -10862,
    	CannotDoInCurrentContext	= -10863,
    	InvalidAudioUnit			= -10864,
    }
    
    public class AUGraphException : Exception {
        static string Lookup (int k)
        {        
            switch ((AUGraphErrors)k)
            {
                case AUGraphErrors.NodeNotFound:
                return "The specified node cannot be found.";
                
                case AUGraphErrors.InvalidConnection:
                return "The attempted connection between two nodes cannot be made.";
                
                case AUGraphErrors.OutputNodeErr:
                return "Audio processing graphs can only contain one output unit. This error is returned if trying to add a second output unit or if the graph’s output unit is removed while the graph is running.";
                
                case AUGraphErrors.CannotDoInCurrentContext:
                return "To avoid spinning or waiting in the render thread (a bad idea!), many of the calls to AUGraph can return: kAUGraphErr_CannotDoInCurrentContext. This result is only generated when you call an AUGraph API from its render callback. It means that the lock that it required was held at that time, by another thread. If you see this result code, you can generally attempt the action again - typically the NEXT render cycle (so in the mean time the lock can be cleared), or you can delegate that call to another thread in your app. You should not spin or put-to-sleep the render thread.";
                
                case AUGraphErrors.InvalidAudioUnit:
                return "Invalid AudioUnit.";
            }
  			
  			return String.Format ("Unknown error code: 0x{0:x}", k);
  		}

  		internal AUGraphException (int k) : base (Lookup (k))
  		{
  		}
  	}
    	
    public class AUGraph : IDisposable{
        internal protected IntPtr handle;
		internal protected GCHandle gch;
		
		public IntPtr Handle { get { return handle; } private set { this.handle = value; } }
		
	~AUGraph ()
		{
			Dispose (false);
		}
		
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero) {
				AUGraphUninitialize (handle);
				AUGraphClose (handle);
				DisposeAUGraph (handle);
				handle = IntPtr.Zero;
				
				gch.Free ();
			}
		}
		
		public EventHandler<AudioRenderEventArgs> Render;
		protected virtual void OnRender (ref AudioUnitRenderActionFlags ioActionFlags, ref MonoMac.CoreAudio.AudioTimeStamp inTimeStamp, UInt32 inBusNumber, UInt32 inNumberOfFrames, ref AudioBufferList ioData)
		{
			var r = Render;
			if (r != null)
				r (this, new AudioRenderEventArgs (ioActionFlags, inTimeStamp, inBusNumber, inNumberOfFrames, ioData));
		}
		
		
		[MonoPInvokeCallback(typeof(AURenderCallback))]
        static int RenderCallback (IntPtr inRefCon,
            ref AudioUnitRenderActionFlags ioActionFlags,
            AudioTimeStamp inTimeStamp,
            UInt32 inBusNumber,
            UInt32 inNumberFrames,
            AudioBufferList ioData)
        {
			GCHandle handle = GCHandle.FromIntPtr (inRefCon);
			var augraph = handle.Target as AUGraph;

			augraph.OnRender(ref ioActionFlags, ref inTimeStamp, inBusNumber, inNumberFrames, ref ioData);

            return 0;
        }
			
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus NewAUGraph(out IntPtr outGraph);
		public AUGraph ()
		{
			IntPtr newGraph;
			int k = NewAUGraph (out newGraph);
			if (k != 0)
				throw new AUGraphException (k);
			else
				this.Handle = newGraph;
			
			k = AUGraphOpen (handle);
			if (k != 0)
				throw new AUGraphException (k);
			
			gch = GCHandle.Alloc(this);            
            k = AUGraphAddRenderNotify(handle, RenderCallback, GCHandle.ToIntPtr(gch));
            if (k != 0)
                throw new AUGraphException(k);
		}
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus DisposeAUGraph(IntPtr inGraph);
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphAddNode (IntPtr inGraph, AudioComponentDescription inDescription, out AUNode outNode);
		AUNode AddNode (AudioComponentDescription description)
		{
			AUNode node;
			int k = AUGraphAddNode (handle, description, out node);
			if (k != 0)
				throw new AUGraphException(k);
			
			return node;
		}
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphRemoveNode (IntPtr inGraph, AUNode inNode);
		void RemoveNode (AUNode node)
		{
			int k = AUGraphRemoveNode (handle, node);
			if(k != 0)
				throw new AUGraphException(k);
		}
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphGetNodeCount (IntPtr inGraph, out UInt32 outNumberOfNodes);
		uint Count ()
		{
			uint count;
			int k = AUGraphGetNodeCount (handle, out count);
			if (k != 0)
				throw new AUGraphException (k);
			
			return count;
		}
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphGetIndNode (IntPtr inGraph, UInt32 inIndex, out AUNode outNode);
		AUNode GetNodeAtIndex (UInt32 index)
		{
			AUNode node;
			int k = AUGraphGetIndNode (handle, index, out node);
			if (k != 0)
				throw new AUGraphException (k);
			
			return node;
		}
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphNodeInfo (IntPtr inGraph, AUNode inNode, out AudioComponentDescription outDescription, out AudioUnit outAudioUnit);
		// TODO
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphNewNodeSubGraph (IntPtr inGraph, out AUNode outNode);
		AUNode NewNodeSubGraph ()
		{
			AUNode node;
			int k = AUGraphNewNodeSubGraph (handle, out node);
			if (k != 0)
				throw new AUGraphException (k);
			return node;
		}
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphGetNodeInfoSubGraph (IntPtr inGraph, AUNode inNode, out AUGraph outSubGraph);
		// TODO
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphIsNodeSubGraph (IntPtr inGraph, AUNode inNode, out Boolean outFlag);
		bool IsNodeInSubGraph (AUNode node)
		{
			Boolean flag;
			int k = AUGraphIsNodeSubGraph (handle, node, out flag);
			if (k != 0)
				throw new AUGraphException (k); 
			
			return flag;
		}
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphConnectNodeInput (IntPtr inGraph, AUNode inSourceNode, UInt32 inSourceOutputNumber, AUNode inDestNode, UInt32 inDestInputNumber);
		// TODO
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus  AUGraphSetNodeInputCallback (IntPtr inGraph, AUNode inDestNode, UInt32 inDestInputNumber, ref AURenderCallbackStruct inInputCallback);
		// TODO
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphDisconnectNodeInput (IntPtr inGraph, AUNode inDestNode, UInt32 inDestInputNumber);
		// TODO
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphClearConnections (IntPtr inGraph);
		void ClearConnection ()
		{
			int k = AUGraphClearConnections (handle);
			if(k != 0)
				throw new AUGraphException(k);
		}
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphGetNumberOfInteractions (IntPtr inGraph, out UInt32 outNumInteractions);
		public uint NumberOfInteractions {
			get {
				uint numberOfInteractions;
				int k = AUGraphGetNumberOfInteractions (handle, out numberOfInteractions);
				if (k != 0)
					throw new AUGraphException (k);
				return numberOfInteractions;
			}
		}
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphGetInteractionInfo (IntPtr inGraph, UInt32 inInteractionIndex, out AUNodeInteraction outInteraction);
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphCountNodeInteractions (IntPtr inGraph, AUNode inNode, out UInt32 outNumInteractions);
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphGetNodeInteractions (IntPtr inGraph, AUNode inNode, ref UInt32 ioNumInteractions, out AUNodeInteraction outInteractions);
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphUpdate (IntPtr inGraph, out Boolean outIsUpdated);
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphOpen (IntPtr inGraph);
        
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphClose (IntPtr inGraph);
        
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphInitialize (IntPtr inGraph);
		void Initialize ()
		{
			int k = AUGraphInitialize (handle);
			if(k != 0)
				throw new AUGraphException(k);
		}
        
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphUninitialize (IntPtr inGraph);
		private void Uninitialize ()
		{
			int k = AUGraphUninitialize (handle);
			if(k != 0)
				throw new AUGraphException(k);
		}
        
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphStart (IntPtr inGraph);
        void Start ()
        {
        	int k = AUGraphStart (handle);
        	if (k != 0)
        		throw new AUGraphException (k);
        }
		
        [DllImport (Constants.AudioToolboxLibrary)]
        extern static OSStatus AUGraphStop (IntPtr inGraph);
		void Stop ()
		{
			int k = AUGraphStop (handle);
			if(k != 0)
				throw new AUGraphException(k);
		}
        
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphIsOpen (IntPtr inGraph, out Boolean outIsOpen);
		public bool IsOpen {
			get {
				Boolean isOpen;
				int k = AUGraphIsOpen (handle, out isOpen);
				if (k != 0)
					throw new AUGraphException (k);
				return isOpen;
			}
		}
        						
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphIsInitialized (IntPtr inGraph, out Boolean outIsInitialized);
		public bool IsInitialized {
			get {
				Boolean isInitialized;
				int k = AUGraphIsInitialized (handle, out isInitialized);
				if (k != 0)
					throw new AUGraphException (k);
				
				return isInitialized;
			}
		}
        						
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphIsRunning (IntPtr inGraph, out Boolean outIsRunning);
        public bool IsRunning {
        	get {
        		Boolean isRunning;
        		int k = AUGraphIsRunning (handle, out isRunning);
        		if (k != 0)
        			throw new AUGraphException (k);
    
				return isRunning;
        	}
        }
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphGetCPULoad (IntPtr inGraph, out Float32 outAverageCPULoad);
		public float AverageCPULoad {
			get {
				float cpuLoad;
				int k = AUGraphGetCPULoad (handle, out cpuLoad);
				if (k != 0)
					throw new AUGraphException (k);
				
				return cpuLoad;
			}
		}	
        						
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphGetMaxCPULoad (IntPtr inGraph, out Float32 outMaxLoad);
        public float MaxCPULoad {
        	get {
        		float cpuLoad;
        		int k = AUGraphGetMaxCPULoad (handle, out cpuLoad);
        		if (k != 0)
        			throw new AUGraphException (k);
    
				return cpuLoad;
        	}
        }						
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphAddRenderNotify (IntPtr inGraph, AURenderCallback inCallback, IntPtr inRefCon);

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphRemoveRenderNotify (IntPtr inGraph, AURenderCallback inCallback, IntPtr inRefCon);
     	
        /// Depreacted								
		// [DllImport (Constants.AudioToolboxLibrary)]
		// extern static OSStatus AUGraphNewNode (IntPtr inGraph, ref ComponentDescription inDescription, UInt32 inClassDataSize/* reserved: must be zero*/, IntPtr inClassData, out AUNode outNode);
        				
		// [DllImport (Constants.AudioToolboxLibrary)]
		// extern static OSStatus AUGraphGetNodeInfo (IntPtr inGraph, AUNode inNode, out ComponentDescription outDescription, out UInt32 outClassDataSize, out IntPtr outClassData, out AudioUnit outAudioUnit);
        					
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphGetNumberOfConnections (IntPtr inGraph, out UInt32 outNumConnections);
         
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphGetConnectionInfo (IntPtr inGraph, UInt32 inConnectionIndex, out AUNode outSourceNode, out UInt32 outSourceOutputNumber, out AUNode outDestNode, out UInt32 outDestInputNumber);
         	
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphCountNodeConnections (IntPtr 	inGraph, AUNode inNode, out UInt32 outNumConnections);
         
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AUGraphGetNodeConnections (IntPtr inGraph, AUNode inNode, out AudioUnitNodeConnection outConnections, ref UInt32 ioNumConnections);
		/// End Deprecated
    }
    
    public enum AUNodeInteraction {
        Connection = 1,
        InputCallback = 2,
    }
    
    [StructLayout(LayoutKind.Sequential)]
	public struct AudioUnitNodeConnection
    {
    	public AUNode		SourceNode;
    	public UInt32		SourceOutputNumber;
    	public AUNode		DestNode;
    	public UInt32		DestInputNumber;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct AUNodeRenderCallback 
    {
    	public AUNode					DestNode;
    	public AudioUnitElement		    DestInputNumber;
    	public AURenderCallbackStruct	Cback;
    }
    
/*   TODO : Port the union 
    [StructLayout(LayoutKind.Sequential)]
    struct AUNodeInteraction
    {
        public UInt32       nodeInteractionType;

        union
        {
            public AUNodeConnection     connection;     
            public AUNodeRenderCallback inputCallback;

        }           nodeInteraction;
    }*/
    
    
}