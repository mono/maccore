using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MonoMac.AudioToolbox;
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

using AudioUnitScope            = System.UInt32;
using AudioUnitElement          = System.UInt32;
using AudioUnitParameterID      = System.UInt32;
using AudioUnitParameterValue   = System.Single;
using AUParameterEventType      = System.UInt32;

// TODO: Preferred namespace AudioUnit conflicts with class `AudioUnit'
namespace MonoMac.AudioUnitFramework {
	
	public class AudioRenderEventArgs : EventArgs {
		public AudioRenderEventArgs (
			AudioUnitRenderActionFlags ioActionFlags,
            MonoMac.CoreAudio.AudioTimeStamp inTimeStamp,
            UInt32 inBusNumber,
            UInt32 inNumberFrames,
            AudioBufferList ioData)
		{
			this.ActionFlags = ioActionFlags;
			this.TimeStamp = inTimeStamp;
			this.BusNumber = inBusNumber;
			this.NumberFrames = inNumberFrames;
			this.Data = ioData;
		}
		
		public readonly AudioUnitRenderActionFlags ActionFlags;
        public readonly MonoMac.CoreAudio.AudioTimeStamp TimeStamp;
        public readonly UInt32 BusNumber;
        public readonly UInt32 NumberFrames;
        public readonly AudioBufferList Data;
		
		public override string ToString ()
		{
			return String.Format ("Packet (ActionFlags={0} TimeStamp={1} BusNumber={2} NumberFrames={3} Data={4}", ActionFlags, TimeStamp, BusNumber, NumberFrames, Data);
		}
	}

    public delegate OSStatus AURenderCallback (IntPtr inRefCon, ref AudioUnitRenderActionFlags ioActionFlags,  MonoMac.CoreAudio.AudioTimeStamp inTimeStap, UInt32 inBusNumber, UInt32 inNumberOfFrames,  AudioBufferList ioData);
	
	// TODO: These 5 delegates need implimenting
    public delegate void     AudioUnitPropertyListenerProc (IntPtr inRefCon, IntPtr inUnit, AudioUnitPropertyID inID, AudioUnitScope inScope, AudioUnitElement inElement);
	public delegate void     AUInputSamplesInOutputCallback (IntPtr inRefCon, ref MonoMac.CoreAudio.AudioTimeStamp inOutputTimeStamp, Float64 inInputSample, Float64 inNumberInputSamples);
	public delegate OSStatus AudioUnitGetParameterProc (IntPtr inComponentStorage, AudioUnitParameterID inID, AudioUnitScope inScope, AudioUnitElement inElement, out AudioUnitParameterValue outValue);
	public delegate OSStatus AudioUnitSetParameterProc (IntPtr inComponentStorage, AudioUnitParameterID inID, AudioUnitScope inScope, AudioUnitElement inElement, AudioUnitParameterValue inValue, UInt32 inBufferOffsetInFrames);
	public delegate OSStatus AudioUnitRenderProc (IntPtr inComponentStorage, ref AudioUnitRenderActionFlags ioActionFlags, ref MonoMac.CoreAudio.AudioTimeStamp inTimeStamp, UInt32 inOutputBusNumber, ref AudioBufferList ioData);
	
    public class AudioUnitException : Exception {
		static string Lookup (int k)
		{
            switch ((AudioUnitErrors)k)
            {
                case AudioUnitErrors.InvalidProperty:
                return "Invalid Property";
                
                case AudioUnitErrors.InvalidParameter :
                return "Invalid Parameter";
                
                case AudioUnitErrors.InvalidElement :
                return "Invalid Element";
                
                case AudioUnitErrors.NoConnection :
                return "No Connection";
                
                case AudioUnitErrors.FailedInitialization :
                return "Failed Initialization";
                
                case AudioUnitErrors.TooManyFramesToProcess :
                return "Too Many Frames To Process";
                
                case AudioUnitErrors.InvalidFile :
                return "Invalid File";
                
                case AudioUnitErrors.FormatNotSupported :
                return "Format Not Supported";
                
                case AudioUnitErrors.Uninitialized :
                return "Uninitialized";
                
                case AudioUnitErrors.InvalidScope :
                return "Invalid Scope";
                
                case AudioUnitErrors.PropertyNotWritable :
                return "Property Not Writable";
                
                case AudioUnitErrors.CannotDoInCurrentContext :
                return "Cannot Do In Current Context";
                
                case AudioUnitErrors.InvalidPropertyValue :
                return "Invalid Property Value";
                
                case AudioUnitErrors.PropertyNotInUse :
                return "Property Not In Use";
                
                case AudioUnitErrors.Initialized :
                return "Initialized";
                
                case AudioUnitErrors.InvalidOfflineRender :
                return "Invalid Offline Render";
                
                case AudioUnitErrors.Unauthorized :
                return "Unauthorized";
                        
            }
			return String.Format ("Unknown error code: 0x{0:x}", k);
		}
		
		internal AudioUnitException (int k) : base (Lookup (k))
		{
		}
	}
	
    // On Mac, AudioUnit is a typedef of  AudioComponentInstance
    //  These methods specifically call for a typedef AudioUnit
    public class AudioUnit : AudioComponentInstance{

 		public virtual void Dispose (bool disposing)
 		{ 		
			if (disposing) {
				/*  TODO: Dispose of listeners
 * 
				if (listeners != null){
					foreach (AudioQueueProperty prop in listeners.Keys){
					//	AudioUnitRemovePropertyListener (handle, prop, property_changed, GCHandle.ToIntPtr (gch));
					}
				}
				*/
			}
		    base.Dispose(disposing);
		}

		public EventHandler<AudioRenderEventArgs> Render;
		protected virtual void OnRender (ref AudioUnitRenderActionFlags ioActionFlags, ref MonoMac.CoreAudio.AudioTimeStamp inTimeStamp, UInt32 inBusNumber, UInt32 inNumberOfFrames,  ref AudioBufferList ioData)
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
			var au = handle.Target as AudioUnit;

			au.OnRender(ref ioActionFlags, ref inTimeStamp, inBusNumber, inNumberFrames, ref ioData);

            return 0;
        }
		
        public AudioUnit (AudioComponent component)
        {
        	IntPtr newInstance;
        	int k = AudioComponentInstance.AudioComponentInstanceNew (component.Handle, out newInstance);
        	if (k != 0)
        		throw new AudioUnitException (k);
        	else
        		this.Handle = newInstance;
   
			
			AURenderCallbackStruct input = new AURenderCallbackStruct ();
        	input.InputProc = RenderCallback;
        	input.InputProcRefCon = GCHandle.ToIntPtr (gch);
			
            this.SetProperty(AudioUnitPropertyID.SetRenderCallback,
				AudioUnitScope.Input,
                0,             
                ref input);

         }
		
		[DllImport (Constants.AudioUnitLibrary)]
        extern static OSStatus AudioUnitInitialize (IntPtr inUnit);
        public void Initialize(){
            int k = AudioUnitInitialize(handle);
            if (k != 0)
				throw new AudioUnitException (k);
        }
        
        [DllImport (Constants.AudioUnitLibrary)]
        extern static OSStatus AudioUnitUninitialize (IntPtr inUnit);
        public void Uninitialize(){
            int k = AudioUnitUninitialize(handle);
        	if (k != 0)
    			throw new AudioUnitException (k);
        }
        
        [DllImport (Constants.AudioUnitLibrary)]
        extern static OSStatus AudioUnitGetPropertyInfo (IntPtr inUnit, AudioUnitPropertyID inID, AudioUnitScope inScope, AudioUnitElement inElement, out UInt32 outDataSize, out Boolean outWritable);
        public void GetPropertyInfo(AudioUnitPropertyID id, AudioUnitScope scope, AudioUnitElement element, out UInt32 dataSize, out Boolean isWritable){

            int k = AudioUnitGetPropertyInfo(handle, id, scope, element, out dataSize, out isWritable);
            if (k != 0)
                throw new AudioUnitException(k);
        }

                                            
        [DllImport (Constants.AudioUnitLibrary)]
        extern static OSStatus AudioUnitGetProperty (   IntPtr			    	inUnit,
        									            AudioUnitPropertyID		inID,
        									            AudioUnitScope			inScope,
        									            AudioUnitElement		inElement,
        									            IntPtr					outData,
        									            ref uint				ioDataSize);
        public void GetProperty (AudioUnitPropertyID property, AudioUnitScope scope, AudioUnitElement element, IntPtr propertyData, ref UInt32 dataSize) {
			int k = AudioUnitGetProperty (handle, property, scope, element, propertyData, ref dataSize);
			if (k != 0)
				throw new AudioUnitException (k);
        }
        
        [DllImport (Constants.AudioUnitLibrary)]
        extern static OSStatus AudioUnitSetProperty( IntPtr                 inUnit,
        									         AudioUnitPropertyID	inID,
        									         AudioUnitScope			inScope,
        									         AudioUnitElement		inElement,
        									         IntPtr     			inData,
        									         UInt32					inDataSize);
        									         
         [DllImport (Constants.AudioUnitLibrary)]
         extern static OSStatus AudioUnitSetProperty( IntPtr				inUnit,
         									         AudioUnitPropertyID	inID,
         									         AudioUnitScope			inScope,
         									         AudioUnitElement		inElement,
         									         ref AURenderCallbackStruct     			inData,
         									         UInt32					inDataSize);
        									         
 		public void SetProperty (AudioUnitPropertyID property, AudioUnitScope scope, AudioUnitElement element,  IntPtr propertyData, UInt32 dataSize)
 		{
		    int k = AudioUnitSetProperty (handle, property, scope, element,  propertyData, dataSize);
			if (k != 0)
				throw new AudioUnitException (k);
 		}
 		
 		public void SetProperty (AudioUnitPropertyID property, AudioUnitScope scope, AudioUnitElement element, ref AURenderCallbackStruct propertyData)
 		{
		    int k = AudioUnitSetProperty (handle, property, scope, element, ref propertyData, (uint) Marshal.SizeOf(propertyData));
			if (k != 0)
				throw new AudioUnitException (k);
 		}
        		
		public void SetUInt (AudioUnitPropertyID property, AudioUnitScope scope, AudioUnitElement element, uint value)
		{
			int k;
			unsafe {
				k = AudioUnitSetProperty (handle, property, scope, element, (IntPtr)(&value), 4);
			}
			if (k != 0)
				throw new AudioUnitException (k);
		}
		
		public void SetDouble (AudioUnitPropertyID property, AudioUnitScope scope, AudioUnitElement element, double value)
		{
			int k;
			unsafe {
				k = AudioUnitSetProperty (handle, property, scope, element, (IntPtr)(&value), 8);
			}
			if (k != 0)
				throw new AudioUnitException (k);
		}
		public delegate void AUComponentPropertyChanged (AudioUnitProperty id);
/*    
 * TODO: 
 * 
     [MonoPInvokeCallback(typeof(AudioUnitPropertyListenerProc))]		 
      static void property_changed (IntPtr userData, IntPtr AQ, AudioQueueProperty id)
        {
            GCHandle gch = GCHandle.FromIntPtr (userData);
            var aq = gch.Target as AudioQueue;
            lock (aq.listeners){
                ArrayList a = (ArrayList)aq.listeners [id];
                if (a == null)
                    return;
                foreach (AudioQueuePropertyChanged cback in a){
                    cback (id);
                }
            }
        }
*/
        
        [DllImport (Constants.AudioUnitLibrary)]
        extern static OSStatus AudioUnitAddPropertyListener (   IntPtr						    inUnit,
        									                    AudioUnitPropertyID				inID,
        									                    AudioUnitPropertyListenerProc	inProc,
        									                    IntPtr							inProcUserData);
        
        Hashtable listeners;
        									                
		public void AddListener (AudioUnitProperty id, AUComponentPropertyChanged callback)
		{
			if (callback == null)
				throw new ArgumentNullException ("callback");
			if (listeners == null)
				listeners = new Hashtable ();

			lock (listeners){
				var a = (ArrayList) listeners [id];
				if (a == null){
				//TODO:	AudioUnitAddPropertyListener (handle, id, property_changed, GCHandle.ToIntPtr (gch));
					listeners [id] = a = new ArrayList ();
				}
				a.Add (callback);
			}
		}
        
        [DllImport (Constants.AudioUnitLibrary)]
        extern static OSStatus AudioUnitRemovePropertyListenerWithUserData( IntPtr						inUnit,
        									                                AudioUnitPropertyID				inID,
        									                                AudioUnitPropertyListenerProc	inProc,
        									                                IntPtr							inProcUserData);
        
        [DllImport (Constants.AudioUnitLibrary)]
        extern static OSStatus AudioUnitAddRenderNotify (   IntPtr				inUnit,
        									                AURenderCallback		inProc,
        									                IntPtr					inProcUserData);
		// TODO : Event hanlders for RenderNotify
		
        [DllImport (Constants.AudioUnitLibrary)]
        extern static OSStatus AudioUnitRemoveRenderNotify( IntPtr				inUnit,
        									                AURenderCallback		inProc,
        									                IntPtr					inProcUserData);
        									
        [DllImport (Constants.AudioUnitLibrary)]
		extern static OSStatus AudioUnitGetParameter( IntPtr	inUnit,
			AudioUnitParameterID		inID,
			AudioUnitScope				inScope,
			AudioUnitElement			inElement,
			out IntPtr	outValue);
		
		public IntPtr GetParameter (AudioUnitParameterID id, AudioUnitScope scope, AudioUnitElement element)
		{
			IntPtr _value;
			int k = AudioUnitGetParameter (handle, id, scope, element, out _value);
			if (k != 0)
				throw new AudioUnitException (k);
			return _value;
		}

		
		[DllImport (Constants.AudioUnitLibrary)]
		extern static OSStatus AudioUnitSetParameter(   IntPtr					inUnit,
        									            AudioUnitParameterID		inID,
        									            AudioUnitScope				inScope,
        									            AudioUnitElement			inElement,
        									            AudioUnitParameterValue		inValue,
        									            UInt32						inBufferOffsetInFrames);
		public void SetParameter (AudioUnitParameterID id, AudioUnitScope scope, AudioUnitElement element, AudioUnitParameterValue aValue)
		{
			int k = AudioUnitSetParameter (handle, id, scope, element, aValue, 0);
			if(k != 0)
				throw new AudioUnitException(k);
		}
		
        									
        [DllImport (Constants.AudioUnitLibrary)]
        extern static OSStatus AudioUnitScheduleParameters( IntPtr						inUnit,
        									                IntPtr                      	inParameterEvent,
        									                UInt32							inNumParamEvents);
		// TODO : ScheduleParameters
        									
        [DllImport (Constants.AudioUnitLibrary)]
        extern static OSStatus AudioUnitRender( IntPtr						    inUnit,
        									    ref AudioUnitRenderActionFlags  ioActionFlags,
        									    ref AudioTimeStamp              inTimeStamp,
        									    UInt32							inOutputBusNumber,
        									    UInt32							inNumberFrames,
        									    ref AudioBufferList          	ioData);
//        public void Render( ref AudioUnitRenderActionFlags actionFlags, ref AudioTimeStamp timeStamp, uint outputBusNumber, uint numberFrames, ref AudioBufferList data){
//            int k = AudioUnitRender(handle, ref actionFlags, ref timeStamp, outputBusNumber, numberFrames, ref data);
//            if( k != 0 )
//                throw new AudioUnitException(k);
//        }
        									
        [DllImport (Constants.AudioUnitLibrary)]
        extern static OSStatus AudioUnitReset(  IntPtr			inUnit,
        									    AudioUnitScope		inScope,
        									    AudioUnitElement	inElement);
        									    
        public void Reset(AudioUnitScope inScope, AudioUnitElement inElement){
            int k =  AudioUnitReset(handle, inScope, inElement);
        	if (k != 0)
  				throw new AudioUnitException (k);    
        }
        
        // TODO: These are specific to only Output type AudioUnits, move to subclass?
        [DllImport (Constants.AudioUnitLibrary)]
        extern static OSStatus AudioOutputUnitStart(IntPtr ci);
        public void Start(){
            int k = AudioOutputUnitStart(handle);
        	if (k != 0)
				throw new AudioUnitException (k);
        }
        
        [DllImport (Constants.AudioUnitLibrary)]
        extern static OSStatus AudioOutputUnitStop(IntPtr ci);
        public void Stop(){
            int k = AudioOutputUnitStop(handle);
        	if (k != 0)
				throw new AudioUnitException (k);
        }
    }
    
    public enum AudioUnitType : uint {
        Output                  = 0x61756f75, // 'auou'
        MusicDevice             = 0x61756d75, // 'aumu'
        MusicEffect             = 0x61756d66, // 'aumf'
        FormatConverter         = 0x61756663, // 'aufc'
        Effect                  = 0x61756678, // 'aufx'
        Mixer                   = 0x61756d78, // 'aumx'
        Panner                  = 0x6175706e, // 'aupn'
        Generator               = 0x6175676e, // 'augn'
        OfflineEffect           = 0x61756f6c, // 'auol'
    }
    
    public enum AudioUnitManufacturer : uint  {
        Apple   = 0x6170706c, // 'appl'
    }
    
    public enum AudioUnitSubType : uint {
        // Apple output audio unit sub types
        GenericOutput           = 0x67656e72, // 'genr'
        HALOutput               = 0x6168616c, // 'ahal'
        DefaultOutput           = 0x64656620, // 'def '
        SystemOutput            = 0x73797320, // 'sys '      
       
        // Apple music instrument sub types
        DLSSynth	            =	0x646c7320, // 'dls '
        
        // Apple converter sub types
        AUConverter	            =	0x636f6e76, // 'conv'
        TimePitch	            =	0x746d7074, // 'tmpt'
        Varispeed	            =	0x76617269, // 'vari'
        DeferredRenderer	    =	0x64656672, // 'defr'
        Splitter	            =	0x73706c74, // 'splt'
        Merger	                =	0x6d657267, // 'merg'
        
        // Apple effect unit sub types
        Delay	                =	0x64656c79, // 'dely'
        LowPassFilter	        =	0x6c706173, // 'lpas'
        HighPassFilter	        =	0x68706173, // 'hpas'
        BandPassFilter	        =	0x62706173, // 'bpas'
        HighShelfFilter	        =	0x68736866, // 'hshf'
        LowShelfFilter      	=	0x6c736866, // 'lshf'
        ParametricEQ        	=	0x706d6571, // 'pmeq'
        GraphicEQ           	=	0x67726571, // 'greq'
        PeakLimiter	            =	0x6c6d7472, // 'lmtr'
        DynamicsProcessor   	=	0x64636d70, // 'dcmp'
        MultiBandCompressor	    =	0x6d636d70, // 'mcmp'
        MatrixReverb        	=	0x6d726576, // 'mrev'
        SampleDelay         	=	0x73646c79, // 'sdly'
        Pitch               	=   0x746d7074, // 'tmpt'
        AUFilter	            =	0x66696c74, // 'filt'
        NetSend	                =	0x6e736e64, // 'nsnd'
        Distortion	            =	0x64697374, // 'dist'
        RogerBeep	            =	0x726f6772, // 'rogr'

        // Mixer sub types
        MultiChannelMixer	    =	0x6d636d78, // 'mcmx'
        StereoMixer	            =	0x736d7872, // 'smxr'
        _3DMixer	                =	0x33646d78, // '3dmx'
        MatrixMixer	            =	0x6d786d78, // 'mxmx'
        
        // Apple Panner unit sub types
        SphericalHeadPanner	    =	0x73706872, // 'sphr'
        VectorPanner        	=	0x76626173, // 'vbas'
        SoundFieldPanner	    =	0x616d6269, // 'ambi'
        HRTFPanner	            =	0x68727466, // 'hrtf'

        // Apple generator unit sub types
        ScheduledSoundPlayer	=	0x7373706c, // 'sspl'
        AudioFilePlayer	        =	0x6166706c, // 'afpl'
        NetReceive	            =	0x6e726376, // 'nrcv'
    }
    
    [Flags]
    public enum AudioUnitRenderActionFlags {
		PreRender		= (1 << 2),
    	PostRender		= (1 << 3),
    	OutputIsSilence	= (1 << 4),
    	OfflineUnitPreflight	= (1 << 5),
    	OfflineUnitRender	= (1 << 6),
    	OfflineUnitRenderComplete	= (1 << 7),
		PostRenderError	= (1 << 8),
    }
    
    public enum AudioUnitErrors {
        InvalidProperty		    	= -10879,
    	InvalidParameter			= -10878,
    	InvalidElement		    	= -10877,
    	NoConnection				= -10876,
    	FailedInitialization		= -10875,
    	TooManyFramesToProcess  	= -10874,
    	InvalidFile			    	= -10871,
    	FormatNotSupported	    	= -10868,
    	Uninitialized				= -10867,
    	InvalidScope				= -10866,
    	PropertyNotWritable	    	= -10865,
    	CannotDoInCurrentContext	= -10863,
    	InvalidPropertyValue		= -10851,
    	PropertyNotInUse			= -10850,
    	Initialized			    	= -10849,
    	InvalidOfflineRender		= -10848,
    	Unauthorized				= -10847,
    }
    
    public enum ParameterEvent {
        Immediate	= 1,
    	Ramped		= 2,
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct AudioUnitParameterEvent
    {
    	public AudioUnitScope			scope;
    	public AudioUnitElement		element;
    	public AudioUnitParameterID	parameter;

    	public AUParameterEventType	eventType;
    
        [StructLayout(LayoutKind.Explicit)]
        public struct eventValues
        {
            /// TODO : fix this union port
    	//union
    	//{
    	    /// [FieldOffset(0)]
    		public struct ramp
    		{
    			public SInt32						StartBufferOffset;
    			public UInt32						DurationInFrames;
    			public AudioUnitParameterValue		StartValue;
    			public AudioUnitParameterValue		EndValue;
    		}
    		
            /// [FieldOffset(0)]
        	public struct immediate
    		{
    			public UInt32						BufferOffset;
    			public AudioUnitParameterValue		Value;
    		}
        }
    	//}						eventValues;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct AudioUnitParameter
    {
    	public IntPtr				    AudioUnit;
    	public AudioUnitParameterID     ParameterID;
    	public AudioUnitScope			Scope;
    	public AudioUnitElement		    Element;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct AudioUnitProperty
    {
    	public IntPtr		    		AudioUnit;
    	public AudioUnitPropertyID		PropertyID;
    	public AudioUnitScope			Scope;
    	public AudioUnitElement	    	Element;
    }
        
    public enum AudioUnitRange {
    	Range							= 0x0000,	// range of selectors for audio units
    	InitializeSelect				= 0x0001,
    	UninitializeSelect		    	= 0x0002,
    	GetPropertyInfoSelect			= 0x0003,
    	GetPropertySelect				= 0x0004,
    	SetPropertySelect				= 0x0005,
    	AddPropertyListenerSelect		= 0x000A,
    	RemovePropertyListenerSelect	= 0x000B,
    	RemovePropertyListenerWithUserDataSelect = 0x0012,
    	AddRenderNotifySelect			= 0x000F,
    	RemoveRenderNotifySelect		= 0x0010,
    	GetParameterSelect		    	= 0x0006,
    	SetParameterSelect		    	= 0x0007,
    	ScheduleParametersSelect		= 0x0011,
    	RenderSelect					= 0x000E,
    	ResetSelect				    	= 0x0009,
    }			
}