using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using OSStatus                  = System.Int32;
using OSType                    = System.UInt32;
using UInt8                     = System.Byte;
using UInt32                    = System.UInt32;
using Float32                   = System.Single;
using Float64                   = System.Double;
using SInt16                    = System.Int16;
using SInt64                    = System.Int64;
using Boolean                   = System.Boolean; // Historic MacOS type with Sizeof() == 1

using AudioObjectID             = System.UInt32;
using AudioDeviceID             = System.UInt32;
using AudioStreamID             = System.UInt32;
    
using AudioObjectPropertySelector   = System.UInt32;
using AudioObjectPropertyScope      = System.UInt32;
using AudioObjectPropertyElement    = System.UInt32;

using CFRunLoopSourceRef            = System.IntPtr;
namespace MonoMac.CoreAudio {
    /// #defines
    
/*   
    define kAudioAggregateDeviceUIDKey             "uid"
    define kAudioAggregateDeviceNameKey            "name"
    define kAudioAggregateDeviceSubDeviceListKey   "subdevices"
    define kAudioAggregateDeviceMasterSubDeviceKey "master"
    define kAudioAggregateDeviceIsPrivateKey       "private"
    
    define kAudioSubDeviceUIDKey   "uid"
    define kAudioSubDeviceNameKey                      "name"
    define kAudioSubDeviceInputChannelsKey             "channels-in"
    define kAudioSubDeviceOutputChannelsKey            "channels-out"
    define kAudioSubDeviceExtraInputLatencyKey         "latency-in"
    define kAudioSubDeviceExtraOutputLatencyKey        "latency-out"
    define kAudioSubDeviceDriftCompensationKey         "drift"
    define kAudioSubDeviceDriftCompensationQualityKey  "drift quality"
    define kAudioHardwareRunLoopMode   "com.apple.audio.CoreAudio"
    */
    
    /// Callbacks
    
    public delegate OSStatus AudioObjectPropertyListenerProc (AudioObjectID inObjectID, UInt32 inNumberAddresses, AudioObjectPropertyAddress [] inAddresses, IntPtr inClientData);
    public delegate OSStatus AudioHardwarePropertyListenerProc (AudioHardwareProperty propertyID, IntPtr clientData);            
    public delegate OSStatus AudioDeviceIOProc (AudioDeviceID inDevice, ref AudioTimeStamp inNow, ref AudioBufferList inInputData, ref AudioTimeStamp inInputTime,out AudioBufferList outOutputData, ref AudioTimeStamp inOutputTime, IntPtr inClientData);  
    public delegate OSStatus AudioDevicePropertyListenerProc (AudioDeviceID inDevice, UInt32 inChannel, Boolean isInput, AudioDeviceProperty inPropertyID, IntPtr inClientData);   
    public delegate OSStatus AudioStreamPropertyListenerProc (AudioStreamID inStream, UInt32 inChannel, AudioDeviceProperty inPropertyID, IntPtr inClientData);
 
	public class AudioHardwareException : Exception {
		static string Lookup (int k)
		{
			//TODO switch AudioHardwareErrors 
			//switch ((AudioHardwareError)k)
			//{
			//            
			//}
			return String.Format ("Unknown error code: 0x{0:x}", k);
		}
		
		internal AudioHardwareException (int k) : base (Lookup (k))
		{
		}
	}
	
	 
	public class AudioDeviceException : Exception {
		static string Lookup (int k)
		{
			//TODO switch AudioDeviceError 
			//switch ((AudioDeviceError)k)
			//{
			//            
			//}
			return String.Format ("Unknown error code: 0x{0:x}", k);
		}
		
		internal AudioDeviceException (int k) : base (Lookup (k))
		{
		}
	}
	
	
	public class AudioObject {
		protected UInt32 audioObjectID;
		
		public AudioObject (UInt32 anID)
		{
			this.audioObjectID = anID;
		}

		~AudioObject ()
		{		    
			Dispose (false);
		}
		
		public void Dispose ()
		{
		    Console.WriteLine("Start disposing AudioObject");
		    
			Dispose (true);
			GC.SuppressFinalize (this);
			Console.WriteLine("End disposing AudioObject");
		}
		
		protected virtual void Dispose (bool disposing)
		{
			// ? Remove any property listeners here?
		}
		
		public UInt32 AudioObjectID {
			get { return audioObjectID; }
		}
		
		[DllImport (Constants.CoreAudioLibrary)]
        extern static void AudioObjectShow(AudioObjectID   inObjectID);
        public void Show() {
            AudioObjectShow(audioObjectID);
        }

        [DllImport (Constants.CoreAudioLibrary)]
		extern static Boolean AudioObjectHasProperty(AudioObjectID inObjectID, ref AudioObjectPropertyAddress inAddress);
        public bool HasProperty (AudioObjectPropertyAddress inAddress){
            return ! AudioObjectHasProperty(audioObjectID, ref inAddress);
        }

        [DllImport (Constants.CoreAudioLibrary)]
		extern static OSStatus AudioObjectIsPropertySettable (AudioObjectID inObjectID, ref AudioObjectPropertyAddress inAddress, out Boolean outIsSettable);             

        [DllImport (Constants.CoreAudioLibrary)]
		extern static OSStatus AudioObjectGetPropertyDataSize (AudioObjectID inObjectID, ref AudioObjectPropertyAddress inAddress, UInt32 inQualifierDataSize, IntPtr inQualifierData, ref UInt32 outDataSize);

        [DllImport (Constants.CoreAudioLibrary)]
		extern static OSStatus AudioObjectGetPropertyData (AudioObjectID inObjectID, ref AudioObjectPropertyAddress inAddress, UInt32 inQualifierDataSize, IntPtr inQualifierData, ref UInt32 ioDataSize, IntPtr outData);  

        [DllImport (Constants.CoreAudioLibrary)]
		extern static OSStatus AudioObjectSetPropertyData (AudioObjectID inObjectID, ref AudioObjectPropertyAddress inAddress, UInt32 inQualifierDataSize, IntPtr inQualifierData, UInt32 inDataSize, IntPtr inData);

        [DllImport (Constants.CoreAudioLibrary)]
		extern static OSStatus AudioObjectAddPropertyListener (AudioObjectID inObjectID, ref AudioObjectPropertyAddress inAddress, AudioObjectPropertyListenerProc inListener, IntPtr inClientData);

        [DllImport (Constants.CoreAudioLibrary)]
		extern static OSStatus AudioObjectRemovePropertyListener (AudioObjectID inObjectID, ref AudioObjectPropertyAddress inAddress, AudioObjectPropertyListenerProc inListener, IntPtr inClientData);
  }
    
    public class AudioHardware : AudioObject {
        public AudioHardware (UInt32 anID) : base(anID)
		{
		}

		~AudioHardware ()
		{
			Dispose (false);		    
		}
        
        [DllImport (Constants.CoreAudioLibrary)]
		extern static OSStatus AudioHardwareAddRunLoopSource(CFRunLoopSourceRef inRunLoopSource);
            
        [DllImport (Constants.CoreAudioLibrary)]
		extern static OSStatus AudioHardwareRemoveRunLoopSource(CFRunLoopSourceRef inRunLoopSource);
            
        [DllImport (Constants.CoreAudioLibrary)]
		extern static OSStatus AudioHardwareUnload();
        static int Unload(){
            return AudioHardwareUnload();
        }        
            
      /// Deprecated but available
		[DllImport (Constants.CoreAudioLibrary)]
        extern static OSStatus
        AudioHardwareGetPropertyInfo(   AudioHardwareProperty inPropertyID,
                                        out UInt32                 outSize,
                                        out Boolean                outWritable);
		//TODO: GetPropertyInfo(uint propertyID, out UInt32 size, out Boolean isWritable){}
		
		[DllImport (Constants.CoreAudioLibrary)]
		extern static OSStatus
        AudioHardwareGetProperty(   AudioHardwareProperty inPropertyID,
                                    ref UInt32              ioPropertyDataSize,
                                    IntPtr                  outPropertyData);
		public static IntPtr GetProperty (AudioHardwareProperty property, ref UInt32 propertyDataSize)
        {
			IntPtr data = new IntPtr();
        	int k = AudioHardwareGetProperty (property, ref propertyDataSize, data);
        	if (k != 0)
        		throw new AudioHardwareException (k);
			
			return data;
		}
				
		public static int GetInt (AudioHardwareProperty property)
		{
			unsafe {
				int val = 0;
				uint size = 4;
				int k = AudioHardwareGetProperty (property, ref size, (IntPtr)(&val));
				if (k != 0)
					throw new AudioHardwareException (k);
				
				return val;
			}
		}

		public static double GetDouble (AudioHardwareProperty property)
		{
			unsafe {
				double val = 0;
				uint size = 8;
				int k = AudioHardwareGetProperty (property, ref size, (IntPtr)(&val));
				if (k != 0)
					throw new AudioHardwareException (k);
				
				return val;
			}
		}
		
		public static long GetLong (AudioHardwareProperty property)
		{
			unsafe {
				long val = 0;
				uint size = 8;
				int k = AudioHardwareGetProperty (property, ref size, (IntPtr)(&val));
				if (k != 0)
					throw new AudioHardwareException (k);
				
				return val;
			}
		}
		
		
		[DllImport (Constants.CoreAudioLibrary)]
	    extern static OSStatus
        AudioHardwareSetProperty(   AudioHardwareProperty inPropertyID,
                                    UInt32                  inPropertyDataSize,
				                    IntPtr             inPropertyData);
		static void SetProperty (AudioHardwareProperty property, UInt32 propertyDataSize, IntPtr propertyData)
		{
			int k = AudioHardwareSetProperty (property, propertyDataSize, propertyData);
			if(k != 0)
				throw new AudioHardwareException(k);
		}
              
		// TODO::AudoHardware property listeners
        [DllImport (Constants.CoreAudioLibrary)]
		extern static OSStatus
        AudioHardwareAddPropertyListener(   uint             inPropertyID,
                                            AudioHardwarePropertyListenerProc   inProc,
                                            IntPtr                               inClientData);
                                        
        [DllImport (Constants.CoreAudioLibrary)]
		extern static OSStatus
        AudioHardwareRemovePropertyListener(    uint             inPropertyID,
                                                AudioHardwarePropertyListenerProc   inProc);
        /// END Deprecated  

    }
    
    public class AudioDevice : AudioObject {
        public AudioDevice (UInt32 anID) : base(anID)
		{
		}

		~AudioDevice ()
		{
			Dispose (false);		    
		}
		
        [DllImport (Constants.CoreAudioLibrary)]
		extern static OSStatus AudioDeviceCreateIOProcID (AudioDeviceID inDevice, AudioDeviceIOProc inProc, IntPtr inClientData, out AudioDeviceIOProc outIOProcID);
        
        [DllImport (Constants.CoreAudioLibrary)]
		extern static OSStatus AudioDeviceDestroyIOProcID (AudioDeviceID inDevice, AudioDeviceIOProc inIOProcID);
        
        /// Deprecated
        /*        
        extern OSStatus
        AudioDeviceAddIOProc(   AudioDeviceID       inDevice,
                                AudioDeviceIOProc   inProc,
                                void*               inClientData) 
        
        extern OSStatus
        AudioDeviceRemoveIOProc(    AudioDeviceID       inDevice,
                                    AudioDeviceIOProc   inProc)
        */
        /// End Deprecated

        [DllImport (Constants.CoreAudioLibrary)]
		extern static OSStatus AudioDeviceStart (AudioDeviceID inDevice, AudioDeviceIOProc inProcID);
        
        [DllImport (Constants.CoreAudioLibrary)]
		extern static OSStatus AudioDeviceStartAtTime (AudioDeviceID inDevice, AudioDeviceIOProc inProcID, ref AudioTimeStamp ioRequestedStartTime, AudioDeviceStartTime inFlags);
                                
        [DllImport (Constants.CoreAudioLibrary)]
		extern static OSStatus AudioDeviceStop (AudioDeviceID inDevice, AudioDeviceIOProc inProcID);
        /// Deprecated
        /*        
        extern OSStatus
        AudioDeviceRead(    AudioDeviceID           inDevice,
                            const AudioTimeStamp*   inStartTime,
                            AudioBufferList*        outData) 
        */
                        
        [DllImport (Constants.CoreAudioLibrary)]
        extern static OSStatus AudioDeviceGetCurrentTime (AudioDeviceID inDevice, out AudioTimeStamp outTime);
    
        [DllImport (Constants.CoreAudioLibrary)]
		extern static OSStatus AudioDeviceTranslateTime (AudioDeviceID inDevice, ref AudioTimeStamp inTime, out AudioTimeStamp outTime);
    
        [DllImport (Constants.CoreAudioLibrary)]
		extern static OSStatus AudioDeviceGetNearestStartTime (AudioDeviceID inDevice, ref AudioTimeStamp ioRequestedStartTime, AudioDeviceStartTime inFlags);
                                                   
        /// Start Deprecated by AudioObject* methods
        [DllImport (Constants.CoreAudioLibrary)]              
		extern static OSStatus
        AudioDeviceGetProperty( AudioDeviceID           inDevice,
                                UInt32                  inChannel,
                                Boolean                 isInput,
                                AudioDeviceProperty   inPropertyID,
                                ref UInt32              ioPropertyDataSize,
                                IntPtr                  outPropertyData);
		
        public int GetInt (AudioDeviceProperty property, uint channel, Boolean isInput)
		{
			unsafe {
				int val = 0;
				uint size = 4;
				int k = AudioDeviceGetProperty (this.audioObjectID, channel, isInput, property, ref size, (IntPtr)(&val));
				if (k != 0)
					throw new AudioDeviceException (k);
				
				return val;
			}
		}

		public double GetDouble (AudioDeviceProperty property, uint channel, Boolean isInput)
		{
			unsafe {
				double val = 0;
				uint size = 8;
				int k = AudioDeviceGetProperty (this.audioObjectID, channel, isInput, property, ref size, (IntPtr)(&val));
				if (k != 0)
					throw new AudioDeviceException (k);
				
				return val;
			}
		}
		
		public long GetLong (AudioDeviceProperty property, uint channel, Boolean isInput)
		{
			unsafe {
				long val = 0;
				uint size = 8;
				int k = AudioDeviceGetProperty (this.audioObjectID, channel, isInput, property, ref size, (IntPtr)(&val));
				if (k != 0)
					throw new AudioDeviceException (k);
				
				return val;
			}
		} 
        [DllImport (Constants.CoreAudioLibrary)]              
        extern static OSStatus AudioDeviceSetProperty( AudioDeviceID inDevice, ref AudioTimeStamp inWhen, UInt32 inChannel, Boolean isInput, AudioDeviceProperty   inPropertyID, UInt32 inPropertyDataSize, IntPtr inPropertyData);
		
		// Nullable version for AudioTimeStamp == null
		[DllImport (Constants.CoreAudioLibrary)]              
        extern static OSStatus AudioDeviceSetProperty( AudioDeviceID inDevice, IntPtr inWhen, UInt32 inChannel, Boolean isInput, AudioDeviceProperty   inPropertyID, UInt32 inPropertyDataSize, IntPtr inPropertyData);
		
		public void SetInt (AudioDeviceProperty property, int value)
		{
			// Most common case
			this.SetInt(property, value, 0, false);
		}
		
		public void SetInt (AudioDeviceProperty property, int value, uint channel, Boolean isInput)
		{
			unsafe {
				AudioDeviceSetProperty (this.audioObjectID,  IntPtr.Zero, channel, isInput, property, 4, (IntPtr)(&value));
			}
		}

		public void SetInt (AudioDeviceProperty property, int value, ref AudioTimeStamp when, uint channel, Boolean isInput)
		{
			unsafe {
				AudioDeviceSetProperty (this.audioObjectID, ref when, channel, isInput, property, 4, (IntPtr)(&value));
			}
		}
		
		public void SetDouble (AudioDeviceProperty property, double value)
		{
			// Most common case
			this.SetDouble(property, value, 0, false);
		}
		
		public void SetDouble (AudioDeviceProperty property, double value, uint channel, Boolean isInput)
		{
			unsafe {
				AudioDeviceSetProperty (this.audioObjectID,  IntPtr.Zero, channel, isInput, property, 8, (IntPtr)(&value));
			}
		}

		public void SetDouble (AudioDeviceProperty property, double value, ref AudioTimeStamp when, uint channel, Boolean isInput)
		{
			unsafe {
				AudioDeviceSetProperty (this.audioObjectID, ref when, channel, isInput, property, 8, (IntPtr)(&value));
			}
		}
		
		public void SetLong (AudioDeviceProperty property, long value)
		{
			// Most common case
			this.SetLong(property, value, 0, false);
		}
		
		public void SetLong (AudioDeviceProperty property, long value, uint channel, Boolean isInput)
		{
			unsafe {
				AudioDeviceSetProperty (this.audioObjectID,  IntPtr.Zero, channel, isInput, property, 8, (IntPtr)(&value));
			}
		}

		public void SetLong (AudioDeviceProperty property, long value, ref AudioTimeStamp when, uint channel, Boolean isInput)
		{
			unsafe {
				AudioDeviceSetProperty (this.audioObjectID, ref when, channel, isInput, property, 8, (IntPtr) (&value));
			}
		}
				
        /* TODO: These methods, though deprecated are the APIs many are used to
        extern OSStatus
        AudioDeviceGetPropertyInfo( AudioDeviceID           inDevice,
                                    UInt32                  inChannel,
                                    Boolean                 isInput,
                                    AudioDeviceProperty   inPropertyID,
                                    UInt32*                 outSize,
                                    Boolean*                outWritable)
                                    


        extern OSStatus
        AudioDeviceAddPropertyListener( AudioDeviceID                   inDevice,
                                        UInt32                          inChannel,
                                        Boolean                         isInput,
                                        AudioDeviceProperty           inPropertyID,
                                        AudioDevicePropertyListenerProc inProc,
                                        void*                           inClientData)  

        extern OSStatus
        AudioDeviceRemovePropertyListener(  AudioDeviceID                   inDevice,
                                            UInt32                          inChannel,
                                            Boolean                         isInput,
                                            AudioDeviceProperty           inPropertyID,
                                            AudioDevicePropertyListenerProc inProc)    
        */
        /// END Deprecated
    }

    /*
    public class AudioStream : AudioObject {
        /// AudioStream 
       
        /// Deprecated
        extern OSStatus
        AudioStreamGetPropertyInfo( AudioStreamID           inStream,
                                    UInt32                  inChannel,
                                    AudioDeviceProperty   inPropertyID,
                                    UInt32*                 outSize,
                                    Boolean*                outWritable)  

        extern OSStatus
        AudioStreamGetProperty( AudioStreamID           inStream,
                                UInt32                  inChannel,
                                AudioDeviceProperty   inPropertyID,
                                UInt32*                 ioPropertyDataSize,
                                void*                   outPropertyData) 

        extern OSStatus
        AudioStreamSetProperty( AudioStreamID           inStream,
                                const AudioTimeStamp*   inWhen,
                                UInt32                  inChannel,
                                AudioDeviceProperty   inPropertyID,
                                UInt32                  inPropertyDataSize,
                                const void*             inPropertyData) 

        extern OSStatus
        AudioStreamAddPropertyListener( AudioStreamID                   inStream,
                                        UInt32                          inChannel,
                                        AudioDeviceProperty           inPropertyID,
                                        AudioStreamPropertyListenerProc inProc,
                                        void*                           inClientData)   

        extern OSStatus
        AudioStreamRemovePropertyListener(  AudioStreamID                   inStream,
                                            UInt32                          inChannel,
                                            AudioDeviceProperty           inPropertyID,
                                            AudioStreamPropertyListenerProc inProc)
    }
    */
                       
    [StructLayout(LayoutKind.Sequential)]
    public struct  AudioObjectPropertyAddress
    {
        public AudioObjectPropertySelector Selector;
        public AudioObjectPropertyScope    Scope;
        public AudioObjectPropertyElement  Element;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct  AudioHardwareIOProcStreamUsage
    {
        public IntPtr   IOProc;
        public UInt32   NumberStreams;
        public UInt32   StreamIsOn; /// Variable length array of count NumberStreams
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct  AudioStreamRangedDescription
    {
        public AudioStreamBasicDescription      Format;
        public AudioValueRange                  SampleRateRange;
    }
    
    public enum AudioHardwareError {
        NoError                 =   0,
        NotRunning	        =	0x73746f70, // 'stop'
        Unspecified    	=	0x77686174, // 'what'
        UnknownProperty	=	0x77686f3f, // 'who?'
        BadPropertySize	=	0x2173697a, // '!siz'
        IllegalOperation	=	0x6e6f7065, // 'nope'
        BadObject      	=	0x216f626a, // '!obj'
        BadDevice      	=	0x21646576, // '!dev'
        BadStream      	=	0x21737472, // '!str'
        UnsupportedOperation	=	0x756e6f70, // 'unop'
    }
    
    public enum AudioDeviceError {
        UnsupportedFormat	=	0x21646174, // '!dat'
        Permissions	     =   	0x21686f67, // '!hog'
    }
    
    public enum AudioObjectProperty : uint {
        /// AudioObject Wildcard Constants
        SelectorWildcard	=	0x2a2a2a2a, // '****'
        ScopeWildcard	=	0x2a2a2a2a, // '****'
        ElementWildcard     = 0xFFFFFFFF,
        
        /// AudioObject Class Constants
        ScopeGlobal	=	0x676c6f62, // 'glob'
        ElementMaster   = 0,
        
        /// AudioObject Property Selectors
        Class	=	0x636c6173, // 'clas'
        Owner	=	0x73746476, // 'stdv'
        Creator	=	0x6f706c67, // 'oplg'
        Name	=	0x6c6e616d, // 'lnam'
        Manufacturer	=	0x6c6d616b, // 'lmak'
        ElementName	=	0x6c63686e, // 'lchn'
        ElementCategoryName	=	0x6c63636e, // 'lccn'
        ElementNumberName	=	0x6c636e6e, // 'lcnn'
        OwnedObjects	=	0x6f776e64, // 'ownd'
        ListenerAdded	=	0x6c697361, // 'lisa'
        ListenerRemoved	=	0x6c697372, // 'lisr'
    }
    
    public enum AudioPropertyWildcard : uint {
        PropertyID        = AudioObjectProperty.SelectorWildcard,
        Section           = 0xFF,
        Channel           = AudioObjectProperty.ElementWildcard
    }
    
    public enum AudioControlClassID {
        /// AudioControl Base Class IDs
        Audio	=	0x6163746c, // 'actl'
        Level	=	0x6c65766c, // 'levl'
        Boolean	=	0x746f676c, // 'togl'
        Selector	=	0x736c6374, // 'slct'
        StereoPan	=	0x7370616e, // 'span'
        
        /// AudioLevelControl Subclass IDs
        Volume	=	0x766c6d65, // 'vlme'
        LFEVolume	=	0x73756276, // 'subv'
        BootChimeVolume	=	0x7072616d, // 'pram'
        
        /// AudioBooleanControl Subclass IDs
        Mute	=	0x6d757465, // 'mute'
        Solo	=	0x736f6c6f, // 'solo'
        Jack	=	0x6a61636b, // 'jack'
        LFEMute	=	0x7375626d, // 'subm'
        ISubOwner	=	0x61746368, // 'atch'
        
        /// AudioSelectorControl Subclass IDs
        DataSource	=	0x64737263, // 'dsrc'
        DataDestination	=	0x64657374, // 'dest'
        ClockSource	=	0x636c636b, // 'clck'
        LineLevel	=	0x6e6c766c, // 'nlvl'
    }
    
    // TODO : Possibly re-organize these enums to other groups
    // or better handle the name collision with the AudioObject class
    public enum AudioObjectEnum {
        ClassID	            =	0x616f626a, // 'aobj'
        ClassIDWildcard	    =	0x2a2a2a2a, // '****'
        Unknown             = 0,
        SystemObject        = 1  
    }
    
    public enum AudioControlProperty {
        Scope	=	0x63736370, // 'cscp'
        Element	=	0x63656c6d, // 'celm'
        Variant	=	0x63766172, // 'cvar'
    }
  
    public enum AudioLevelControlProperty {
        ScalarValue	=	0x6c637376, // 'lcsv'
          DecibelValue	=	0x6c636476, // 'lcdv'
          DecibelRange	=	0x6c636472, // 'lcdr'
          ConvertScalarToDecibels	=	0x6c637364, // 'lcsd'
          ConvertDecibelsToScalar	=	0x6c636473, // 'lcds'
          DecibelsToScalarTransferFunction	=	0x6c637466, // 'lctf'
    }
    
    public enum AudioLevelControlTranferFunction {
        Linear      = 0,
        _1Over3     = 1,
        _1Over2     = 2,
        _3Over4     = 3,
        _3Over2     = 4,
        _2Over1     = 5,
        _3Over1     = 6,
        _4Over1     = 7,
        _5Over1     = 8,
        _6Over1     = 9,
        _7Over1     = 10,
        _8Over1     = 11,
        _9Over1     = 12,
        _10Over1    = 13,
        _11Over1    = 14,
        _12Over1    = 15,
    }

    public enum AudioBooleanControlProperty {
        Value	=	0x6263766c, // 'bcvl'
    }
  
    public enum AudioSelectorControlProperty{
        CurrentItem	=	0x73636369, // 'scci'
        AvailableItems	=	0x73636169, // 'scai'
        ItemName	=	0x7363696e, // 'scin'
    }

    public enum AudioClockSourceControlProperty{
        ItemKind	=	0x636c6b6b, // 'clkk'
    }
    
    public enum AudioClockSourceItemKind {
        Internal	=	0x696e7420, // 'int '
    }
    
    public enum AudioStereoPanControlProperty {
        Value	=	0x73706376, // 'spcv'
        PanningChannels	=	0x73706363, // 'spcc'
    }

    public enum AudioSystemObject {
        ClassID	=	0x61737973, // 'asys'
    }
    
    public enum AudioHardwareProperty : uint {
        ProcessIsMaster	=	0x6d617374, // 'mast'
          IsInitingOrExiting	=	0x696e6f74, // 'inot'
          UserIDChanged	=	0x65756964, // 'euid'
          Devices	=	0x64657623, // 'dev#'
          DefaultInputDevice	=	0x64496e20, // 'dIn '
          DefaultOutputDevice	=	0x644f7574, // 'dOut'
          DefaultSystemOutputDevice	=	0x734f7574, // 'sOut'
          DeviceForUID	=	0x64756964, // 'duid'
          ProcessIsAudible	=	0x706d7574, // 'pmut'
          SleepingIsAllowed	=	0x736c6570, // 'slep'
          UnloadingIsAllowed	=	0x756e6c64, // 'unld'
          HogModeIsAllowed	=	0x686f6772, // 'hogr'
          RunLoop	=	0x726e6c70, // 'rnlp'
          PlugInForBundleID	=	0x70696269, // 'pibi'
          UserSessionIsActiveOrHeadless	=	0x75736572, // 'user'
          MixStereoToMono	=	0x73746d6f, // 'stmo'
    }
  
    public enum AudioHardwarePropertyBootChimeVolume {
        Scalar	=	0x62627673, // 'bbvs'
        Decibels	=	0x62627664, // 'bbvd'
        RangeDecibels	=	0x62626423, // 'bbd#'
        ScalarToDecibels	=	0x62763264, // 'bv2d'
        DecibelsToScalar	=	0x62643276, // 'bd2v'
        DecibelsToScalarTransferFunction	=	0x62767466, // 'bvtf'  
    }

    public enum AudioPlugIn {
        ClassID	=	0x61706c67, // 'aplg'
        PropertyBundleID	=	0x70696964, // 'piid'
        CreateAggregateDevice	=	0x63616767, // 'cagg'
        DestroyAggregateDevice	=	0x64616767, // 'dagg'
    }
    
    // TODO : Make this enum cleaner
    public enum AudioDeviceEnum{
        PropertyScopeInput	=	0x696e7074, // 'inpt'
        PropertyScopeOutput	=	0x6f757470, // 'outp'
        PropertyScopePlayThrough	=	0x70747275, // 'ptru'
        ClassID	=	0x61646576, // 'adev'
        Unknown = AudioObjectEnum.Unknown,
    }
  
    [Flags]
    public enum AudioDeviceStartTime {
        IsInputFlag            = (1 << 0),
        DontConsultDeviceFlag  = (1 << 1),
        DontConsultHALFlag     = (1 << 2),
    }
    
    public enum AudioDeviceTransportType {
        Unknown = 0,
        BuiltIn	=	0x626c746e, // 'bltn'
        Aggregate	=	0x67727570, // 'grup'
        AutoAggregate	=	0x66677270, // 'fgrp'
        Virtual	=	0x76697274, // 'virt'
        PCI	=	0x70636920, // 'pci '
        USB	=	0x75736220, // 'usb '
        FireWire	=	0x31333934, // '1394'
        Bluetooth	=	0x626c7565, // 'blue' 
    }

    public enum AudioDeviceProperty : uint {
        /// AudioDevice Properties
        PlugIn	=	0x706c7567, // 'plug'
           ConfigurationApplication	=	0x63617070, // 'capp'
           DeviceUID	=	0x75696420, // 'uid '
           ModelUID	=	0x6d756964, // 'muid'
           TransportType	=	0x7472616e, // 'tran'
           RelatedDevices	=	0x616b696e, // 'akin'
           ClockDomain	=	0x636c6b64, // 'clkd'
           DeviceIsAlive	=	0x6c69766e, // 'livn'
           DeviceHasChanged	=	0x64696666, // 'diff'
           DeviceIsRunning	=	0x676f696e, // 'goin'
           DeviceIsRunningSomewhere	=	0x676f6e65, // 'gone'
           DeviceCanBeDefaultDevice	=	0x64666c74, // 'dflt'
           DeviceCanBeDefaultSystemDevice	=	0x73666c74, // 'sflt'
           ProcessorOverload	=	0x6f766572, // 'over'
           HogMode	=	0x6f696e6b, // 'oink'
           Latency	=	0x6c746e63, // 'ltnc'
           BufferFrameSize	=	0x6673697a, // 'fsiz'
           BufferFrameSizeRange	=	0x66737a23, // 'fsz#'
           UsesVariableBufferFrameSizes	=	0x7666737a, // 'vfsz'
           Streams	=	0x73746d23, // 'stm#'
           SafetyOffset	=	0x73616674, // 'saft'
           IOCycleUsage	=	0x6e637963, // 'ncyc'
           StreamConfiguration	=	0x736c6179, // 'slay'
           IOProcStreamUsage	=	0x73757365, // 'suse'
           PreferredChannelsForStereo	=	0x64636832, // 'dch2'
           PreferredChannelLayout	=	0x73726e64, // 'srnd'
           NominalSampleRate	=	0x6e737274, // 'nsrt'
           AvailableNominalSampleRates	=	0x6e737223, // 'nsr#'
           ActualSampleRate	=	0x61737274, // 'asrt'
           Icon	=	0x69636f6e, // 'icon'
           IsHidden	=	0x6869646e, // 'hidn'
           
           /// AudioDevice Properties Implemented via AudioControl objects
           JackIsConnected	=	0x6a61636b, // 'jack'
           VolumeScalar	=	0x766f6c6d, // 'volm'
           VolumeDecibels	=	0x766f6c64, // 'vold'
           VolumeRangeDecibels	=	0x76646223, // 'vdb#'
           VolumeScalarToDecibels	=	0x76326462, // 'v2db'
           VolumeDecibelsToScalar	=	0x64623276, // 'db2v'
           VolumeDecibelsToScalarTransferFunction	=	0x76637466, // 'vctf'
           StereoPan	=	0x7370616e, // 'span'
           StereoPanChannels	=	0x73706e23, // 'spn#'
           Mute	=	0x6d757465, // 'mute'
           Solo	=	0x736f6c6f, // 'solo'
           DataSource	=	0x73737263, // 'ssrc'
           DataSources	=	0x73736323, // 'ssc#'
           DataSourceNameForIDCFString	=	0x6c73636e, // 'lscn'
           ClockSource	=	0x63737263, // 'csrc'
           ClockSources	=	0x63736323, // 'csc#'
           ClockSourceNameForIDCFString	=	0x6c63736e, // 'lcsn'
           ClockSourceKindForID	=	0x6373636b, // 'csck'
           PlayThru	=	0x74687275, // 'thru'
           PlayThruSolo	=	0x74687273, // 'thrs'
           PlayThruVolumeScalar	=	0x6d767363, // 'mvsc'
           PlayThruVolumeDecibels	=	0x6d766462, // 'mvdb'
           PlayThruVolumeRangeDecibels	=	0x6d766423, // 'mvd#'
           PlayThruVolumeScalarToDecibels	=	0x6d763264, // 'mv2d'
           PlayThruVolumeDecibelsToScalar	=	0x6d763273, // 'mv2s'
           PlayThruVolumeDecibelsToScalarTransferFunction	=	0x6d767466, // 'mvtf'
           PlayThruStereoPan	=	0x6d73706e, // 'mspn'
           PlayThruStereoPanChannels	=	0x6d737023, // 'msp#'
           PlayThruDestination	=	0x6d646473, // 'mdds'
           PlayThruDestinations	=	0x6d646423, // 'mdd#'
           PlayThruDestinationNameForIDCFString	=	0x6d646463, // 'mddc'
           ChannelNominalLineLevel	=	0x6e6c766c, // 'nlvl'
           ChannelNominalLineLevels	=	0x6e6c7623, // 'nlv#'
           ChannelNominalLineLevelNameForIDCFString	=	0x6c636e6c, // 'lcnl'
           DriverShouldOwniSub	=	0x69737562, // 'isub'
           SubVolumeScalar	=	0x73766c6d, // 'svlm'
           SubVolumeDecibels	=	0x73766c64, // 'svld'
           SubVolumeRangeDecibels	=	0x73766423, // 'svd#'
           SubVolumeScalarToDecibels	=	0x73763264, // 'sv2d'
           SubVolumeDecibelsToScalar	=	0x73643276, // 'sd2v'
           SubVolumeDecibelsToScalarTransferFunction	=	0x73767466, // 'svtf'
           SubMute	=	0x736d7574, // 'smut'
           
           ///  AudioDevice Properties That Ought To Some Day Be Deprecated
           DeviceName	=	0x6e616d65, // 'name'
           DeviceNameCFString = AudioObjectProperty.Name,
              DeviceManufacturer	=	0x6d616b72, // 'makr'
              DeviceManufacturerCFString          = AudioObjectProperty.Manufacturer,
              
              RegisterBufferList	=	0x72627566, // 'rbuf'
              BufferSize	=	0x6273697a, // 'bsiz'
              BufferSizeRange	=	0x62737a23, // 'bsz#'
              ChannelName	=	0x63686e6d, // 'chnm'
              ChannelNameCFString                 = AudioObjectProperty.ElementName,
              ChannelCategoryName	=	0x63636e6d, // 'ccnm'
              ChannelCategoryNameCFString         = AudioObjectProperty.ElementCategoryName,
              ChannelNumberName	=	0x636e6e6d, // 'cnnm'
              ChannelNumberNameCFString           = AudioObjectProperty.ElementNumberName,
              SupportsMixing	=	0x6d69783f, // 'mix?'
              StreamFormat	=	0x73666d74, // 'sfmt'
              StreamFormats	=	0x73666d23, // 'sfm#'
              StreamFormatSupported	=	0x73666d3f, // 'sfm?'
              StreamFormatMatch	=	0x73666d6d, // 'sfmm'
              DataSourceNameForID	=	0x7373636e, // 'sscn'
              ClockSourceNameForID	=	0x6373636e, // 'cscn'
              PlayThruDestinationNameForID	=	0x6d64646e, // 'mddn'
              ChannelNominalLineLevelNameForID	=	0x636e6c76, // 'cnlv'
    }
   
    public enum AudioStream {
        ClassID	=	0x61737472, // 'astr'
        Unknown = AudioObjectEnum.Unknown,
    }
   
   public enum AudioStreamTerminalType : uint {
       Unknown  = 0,
       Line	=	0x6c696e65, // 'line'
       DigitalAudioInterface	=	0x73706466, // 'spdf'
       Speaker	=	0x73706b72, // 'spkr'
       Headphones	=	0x68647068, // 'hdph'
       LFESpeaker	=	0x6c666573, // 'lfes'
       ReceiverSpeaker	=	0x7273706b, // 'rspk'
       Microphone	=	0x6d696372, // 'micr'
       HeadsetMicrophone	=	0x686d6963, // 'hmic'
       ReceiverMicrophone	=	0x726d6963, // 'rmic'
       TTY	=	0x7474795f, // 'tty_'
   }

   public enum AudioStreamProperty : uint {
       Direction	=	0x73646972, // 'sdir'
         TerminalType	=	0x7465726d, // 'term'
         StartingChannel	=	0x7363686e, // 'schn'
         Latency                     = AudioDeviceProperty.Latency,
         VirtualFormat	=	0x73666d74, // 'sfmt'
         AvailableVirtualFormats	=	0x73666d61, // 'sfma'
         PhysicalFormat	=	0x70667420, // 'pft '
         AvailablePhysicalFormats	=	0x70667461, // 'pfta'
         
         /// AudioStream Properties That Ought To Some Day Be Deprecated
         OwningDevice                = AudioObjectProperty.Owner,
         PhysicalFormats	=	0x70667423, // 'pft#'
         PhysicalFormatSupported	=	0x7066743f, // 'pft?'
         PhysicalFormatMatch	=	0x7066746d, // 'pftm'
   }
      
    public enum AudioAggregateDeviceProperty {
        FullSubDeviceList	=	0x67727570, // 'grup'
        ActiveSubDeviceList	=	0x61677270, // 'agrp'
        Composition	=	0x61636f6d, // 'acom'
        
        /// AudioAggregateDevice Properties Implemented via AudioControl objects
        MasterSubDevice	=	0x616d7374, // 'amst'
        
    }

    public enum AudioClassID {
        SubDevice	=	0x61737562, // 'asub'
        AggregateDevice	=	0x61616767, // 'aagg'
        
    }
    
    public enum AudioSubDeviceDriftCompensation {
        MinQuality      = 0,
        LowQuality      = 0x20,
        MediumQuality   = 0x40,
        HighQuality     = 0x60,
        MaxQuality      = 0x7F
    }
    
    public enum AudioSubDeviceProperty {
        ExtraLatency	=	0x786c7463, // 'xltc'
        DriftCompensation	=	0x64726674, // 'drft'
        DriftCompensationQuality	=	0x64726671, // 'drfq'
    }   
}