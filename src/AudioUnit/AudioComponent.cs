using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MonoMac.CoreAudio;
using MonoMac.CoreFoundation;

using OSStatus                  = System.Int32;
using OSType                    = System.UInt32;

using UInt8                     = System.Byte;
using UInt32                    = System.UInt32;
using Float32                   = System.Single;
using Float64                   = System.Double;
using SInt16                    = System.Int16;
using SInt32                    = System.Int32;
using SInt64                    = System.Int64;
using Boolean                   = System.Boolean; // Historic MacOS type with Sizeof() == 1
using CFStringRef               = System.IntPtr;
// These are all 10.6+

namespace MonoMac.AudioUnitFramework {
    // AudioComponentDescription should be binary compatible with ComponentDescription from CarbonCore.framework
    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct AudioComponentDescription {
    	public OSType              Type;
    	public OSType              SubType;
    	public OSType              Manufacturer;
    	public UInt32              Flags;
    	public UInt32              FlagsMask;
    }
    
    public class AudioComponentInstance : IDisposable {
        internal protected IntPtr handle;
		internal protected GCHandle gch;
		
        private bool disposed = false;
        
		~AudioComponentInstance ()
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
			Console.WriteLine ("Start disposing AudioComponentInstanece");
			
			if (!disposed) {
				if (disposing)
				{
					if (gch.IsAllocated)
						gch.Free ();				
				}
				
				if (handle != IntPtr.Zero) {
					AudioComponentInstanceDispose (handle);
					handle = IntPtr.Zero;
				}
				
				disposed = true;
			}
			Console.WriteLine("End disposing AudioComponentInstanece");
		}
		
		public IntPtr Handle {
			get { return handle; }
			protected set { handle = value; }
		}
		
        protected AudioComponentInstance ()
        {
			gch = GCHandle.Alloc(this);  
		}
        
        [DllImport (Constants.AudioUnitLibrary)]
        protected extern static OSStatus AudioComponentInstanceNew (IntPtr inComponent, out IntPtr outInstance);   
        public AudioComponentInstance (AudioComponent component){
            IntPtr newInstance;
            int k = AudioComponentInstanceNew(component.Handle, out newInstance);
            if (k != 0)
                /// AudioComponent.h does not define error codes
                throw new Exception(String.Format("Failed to create new AudioComponentInstance, code: 0x{0:x}", k));
            else
                this.Handle = newInstance;
        }
    							
		[DllImport (Constants.AudioUnitLibrary)]
        extern static OSStatus AudioComponentInstanceDispose (IntPtr inInstance);
    					
    	[DllImport (Constants.AudioUnitLibrary)]
		extern static IntPtr AudioComponentInstanceGetComponent (IntPtr inInstance);
		IntPtr Component {
		    get { return AudioComponentInstanceGetComponent(handle); }
		}

        [DllImport (Constants.AudioUnitLibrary)]
		extern static Boolean AudioComponentInstanceCanDo (IntPtr inInstance, SInt16 inSelectorID);
		public bool CanDo(SInt16 selector) {
		    return AudioComponentInstanceCanDo(handle, selector);
		}
    }
    
    public class AudioComponent : IDisposable {
        internal IntPtr handle;
		internal protected GCHandle gch;

		private bool disposed = false;
		
		internal AudioComponent (IntPtr handle)
		{
			this.handle = handle;
			gch = GCHandle.Alloc(this);
		}

		~AudioComponent ()
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

			if (!disposed)
			{
				if (disposing)
				{
					if (gch.IsAllocated)
						gch.Free ();
				}
				
				if (handle != IntPtr.Zero) {
					handle = IntPtr.Zero;
				}
				
				disposed = true;
			}
		}
		
		public IntPtr Handle {
			get { return handle; }
		}
        
        [DllImport (Constants.AudioUnitLibrary)]        				
        extern static UInt32 AudioComponentCount (ref AudioComponentDescription inDesc);	
        public static UInt32 Count(AudioComponentDescription description){
            return AudioComponentCount(ref description);
        }
        
		[DllImport (Constants.AudioUnitLibrary)]
        extern static IntPtr AudioComponentFindNext (IntPtr inComponent, ref AudioComponentDescription inDesc);
        public static AudioComponent Find (AudioComponentDescription inDesc)
        {
        	IntPtr _audioComponent = AudioComponentFindNext (IntPtr.Zero, ref inDesc);
        	
            if (_audioComponent != IntPtr.Zero)
        		return new AudioComponent (_audioComponent);
        	else
        		return null;
        }
		
        public AudioComponent FindNext(AudioComponentDescription inDesc){
            IntPtr _audioComponent = AudioComponentFindNext(handle, ref inDesc);
            
            if(_audioComponent != IntPtr.Zero)
                return new AudioComponent(_audioComponent);
            else
                return null;
        }
    	
	
        [DllImport (Constants.AudioUnitLibrary)]
		extern static OSStatus AudioComponentCopyName (	IntPtr inComponent, out CFStringRef outName);
        public string Name {
            get { 
                CFStringRef name;
                int k = AudioComponentCopyName(handle, out name);
                if(k!=0)
                   throw new Exception("Could not copy component name.");
                
                 return CFString.FetchString(name);  
            }
        }
	
		[DllImport (Constants.AudioUnitLibrary)]
        extern static OSStatus 
        AudioComponentGetDescription( IntPtr inComponent, out AudioComponentDescription   outDesc);
		public AudioComponentDescription Description {
			get {
				AudioComponentDescription desc;
				int k = AudioComponentGetDescription (handle, out desc);
				if (k != 0)
					throw new Exception (String.Format ("Could not get component description error: {0}", k));
				
				return desc;
			}
		}
    							
		[DllImport (Constants.AudioUnitLibrary)]
        extern static OSStatus AudioComponentGetVersion( IntPtr inComponent, out UInt32 outVersion);
		public uint Version {
			// in hexadecimal form as 0xMMMMmmDD (major, minor, dot).
			get {
				UInt32 version;
				int k = AudioComponentGetVersion (handle, out version);
				if (k != 0)
					throw new Exception (String.Format ("Could not get component version error: {0}", k));
				
				return version;
			}
		}
    }
}