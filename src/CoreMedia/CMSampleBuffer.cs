// 
// CMSampelBuffer.cs: Implements the managed CMSampleBuffer
//
// Authors: Mono Team
//     
// Copyright 2010 Novell, Inc
//
using System;
using System.Drawing;
using System.Runtime.InteropServices;

using MonoMac;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;

#if !COREBUILD
using MonoMac.CoreVideo;
#endif

namespace MonoMac.CoreMedia {

	[Since (4,0)]
	public class CMSampleBuffer : INativeObject, IDisposable {
		internal IntPtr handle;

		internal CMSampleBuffer (IntPtr handle)
		{
			this.handle = handle;
		}

		[Preserve (Conditional=true)]
		internal CMSampleBuffer (IntPtr handle, bool owns)
		{
			if (!owns)
				CFObject.CFRetain (handle);

			this.handle = handle;
		}
		
		~CMSampleBuffer ()
		{
			Dispose (false);
		}
		
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public IntPtr Handle {
			get { return handle; }
		}
	
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CFObject.CFRelease (handle);
				handle = IntPtr.Zero;
			}
		}
		
/*		[DllImport(Constants.CoreMediaLibrary)]
		int CMAudioSampleBufferCreateWithPacketDescriptions (
		   CFAllocatorRef allocator,
		   CMBlockBufferRef dataBuffer,
		   Boolean dataReady,
		   CMSampleBufferMakeDataReadyCallback makeDataReadyCallback,
		   void *makeDataReadyRefcon,
		   CMFormatDescriptionRef formatDescription,
		   int numSamples,
		   CMTime sbufPTS,
		   const AudioStreamPacketDescription *packetDescriptions,
		   CMSampleBufferRef *sBufOut
		);

		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferCallForEachSample (
		   CMSampleBufferRef sbuf,
		   int (*callback)(CMSampleBufferRef sampleBuffer, int index, void *refcon),
		   void *refcon
		);

		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferCopySampleBufferForRange (
		   CFAllocatorRef allocator,
		   CMSampleBufferRef sbuf,
		   CFRange sampleRange,
		   CMSampleBufferRef *sBufOut
		);

		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferCreate (
		   CFAllocatorRef allocator,
		   CMBlockBufferRef dataBuffer,
		   Boolean dataReady,
		   CMSampleBufferMakeDataReadyCallback makeDataReadyCallback,
		   void *makeDataReadyRefcon,
		   CMFormatDescriptionRef formatDescription,
		   int numSamples,
		   int numSampleTimingEntries,
		   const CMSampleTimingInfo *sampleTimingArray,
		   int numSampleSizeEntries,
		   const uint *sampleSizeArray,
		   CMSampleBufferRef *sBufOut
		);

		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferCreateCopy (
		   CFAllocatorRef allocator,
		   CMSampleBufferRef sbuf,
		   CMSampleBufferRef *sbufCopyOut
		);

		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferCreateCopyWithNewTiming (
		   CFAllocatorRef allocator,
		   CMSampleBufferRef originalSBuf,
		   int numSampleTimingEntries,
		   const CMSampleTimingInfo *sampleTimingArray,
		   CMSampleBufferRef *sBufCopyOut
		);

		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferCreateForImageBuffer (
		   CFAllocatorRef allocator,
		   CVImageBufferRef imageBuffer,
		   Boolean dataReady,
		   CMSampleBufferMakeDataReadyCallback makeDataReadyCallback,
		   void *makeDataReadyRefcon,
		   CMVideoFormatDescriptionRef formatDescription,
		   const CMSampleTimingInfo *sampleTiming,
		   CMSampleBufferRef *sBufOut
		);*/

		[DllImport(Constants.CoreMediaLibrary)]
		extern static bool CMSampleBufferDataIsReady (IntPtr handle);
		
		public bool DataIsReady
		{
			get
			{
				return CMSampleBufferDataIsReady (handle);
			}
		}

		/*[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferGetAudioBufferListWithRetainedBlockBuffer (
		   CMSampleBufferRef sbuf,
		   uint *bufferListSizeNeededOut,
		   AudioBufferList *bufferListOut,
		   uint bufferListSize,
		   CFAllocatorRef bbufStructAllocator,
		   CFAllocatorRef bbufMemoryAllocator,
		   uint32_t flags,
		   CMBlockBufferRef *blockBufferOut
		);

		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferGetAudioStreamPacketDescriptions (
		   CMSampleBufferRef sbuf,
		   uint packetDescriptionsSize,
		   AudioStreamPacketDescription *packetDescriptionsOut,
		   uint *packetDescriptionsSizeNeededOut
		);

		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferGetAudioStreamPacketDescriptionsPtr (
		   CMSampleBufferRef sbuf,
		   const AudioStreamPacketDescription **packetDescriptionsPtrOut,
		   uint *packetDescriptionsSizeOut
		);*/

		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMSampleBufferGetDataBuffer (IntPtr handle);
		
		public CMBlockBuffer GetDataBuffer ()
		{
			var blockHandle = CMSampleBufferGetDataBuffer (handle);			
			if (blockHandle == IntPtr.Zero)
			{
				return null;
			}
			else
			{
				return new CMBlockBuffer (blockHandle, false);
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMSampleBufferGetDecodeTimeStamp (IntPtr handle);
		
		public CMTime DecodeTimeStamp
		{
			get
			{
				return CMSampleBufferGetDecodeTimeStamp (handle);
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMSampleBufferGetDuration (IntPtr handle);
		
		public CMTime Duration
		{
			get
			{
				return CMSampleBufferGetDuration (handle);
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMSampleBufferGetFormatDescription (IntPtr handle);
		
		public CMFormatDescription GetFormatDescription ()
		{
			var desc = default(CMFormatDescription);
			var descHandle = CMSampleBufferGetFormatDescription (handle);
			if (descHandle != IntPtr.Zero)
			{
				desc = new CMFormatDescription (descHandle, false);
			}
			return desc;					
		}

#if !COREBUILD

		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMSampleBufferGetImageBuffer (IntPtr handle);

		public CVImageBuffer GetImageBuffer ()
		{
			IntPtr ib = CMSampleBufferGetImageBuffer (handle);
			if (ib == IntPtr.Zero)
				return null;

			var ibt = CFType.GetTypeID (ib);
			if (ibt == CVPixelBuffer.CVImageBufferType)
				return new CVPixelBuffer (ib, false);
			return new CVImageBuffer (ib, false);
		}
		
#endif
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static int CMSampleBufferGetNumSamples (IntPtr handle);
		
		public int NumSamples
		{
			get
			{
				return CMSampleBufferGetNumSamples (handle);
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMSampleBufferGetOutputDecodeTimeStamp (IntPtr handle);
		
		public CMTime OutputDecodeTimeStamp
		{
			get
			{
				return CMSampleBufferGetOutputDecodeTimeStamp (handle);
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMSampleBufferGetOutputDuration (IntPtr handle);
		
		public CMTime OutputDuration
		{
			get
			{
				return CMSampleBufferGetOutputDuration (handle);
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMSampleBufferGetOutputPresentationTimeStamp (IntPtr handle);
		
		public CMTime OutputPresentationTimeStamp
		{
			get
			{
				return CMSampleBufferGetOutputPresentationTimeStamp (handle);
			}
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static int CMSampleBufferSetOutputPresentationTimeStamp (IntPtr handle, CMTime outputPresentationTimeStamp);
		
		public int SetOutputPresentationTimeStamp (CMTime outputPresentationTimeStamp)
		{
			return CMSampleBufferSetOutputPresentationTimeStamp (handle, outputPresentationTimeStamp);
		}

		/*[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferGetOutputSampleTimingInfoArray (
		   CMSampleBufferRef sbuf,
		   int timingArrayEntries,
		   CMSampleTimingInfo *timingArrayOut,
		   int *timingArrayEntriesNeededOut
		);*/

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMSampleBufferGetPresentationTimeStamp (IntPtr handle);
		
		public CMTime PresentationTimeStamp
		{
			get
			{
				return CMSampleBufferGetPresentationTimeStamp (handle);
			}
		}
		
#if !COREBUILD

		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMSampleBufferGetSampleAttachmentsArray (IntPtr handle, bool createIfNecessary);
		
		public NSMutableDictionary [] GetSampleAttachments (bool createIfNecessary)
		{
			var cfArrayRef = CMSampleBufferGetSampleAttachmentsArray (handle, createIfNecessary);
			if (cfArrayRef == IntPtr.Zero)
			{
				return new NSMutableDictionary [0];
			}
			else
			{
				return NSArray.ArrayFromHandle (cfArrayRef, h => (NSMutableDictionary) Runtime.GetNSObject (h));
			}
		}
		
#endif

		[DllImport(Constants.CoreMediaLibrary)]
		extern static uint CMSampleBufferGetSampleSize (IntPtr handle, int sampleIndex);
		
		public uint GetSampleSize (int sampleIndex)
		{
			return CMSampleBufferGetSampleSize (handle, sampleIndex);
		}
		
		/*[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferGetSampleSizeArray (
		   CMSampleBufferRef sbuf,
		   int sizeArrayEntries,
		   uint *sizeArrayOut,
		   int *sizeArrayEntriesNeededOut
		);
		
		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferGetSampleTimingInfo (
		   CMSampleBufferRef sbuf,
		   int sampleIndex,
		   CMSampleTimingInfo *timingInfoOut
		);
		
		[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferGetSampleTimingInfoArray (
		   CMSampleBufferRef sbuf,
		   int timingArrayEntries,
		   CMSampleTimingInfo *timingArrayOut,
		   int *timingArrayEntriesNeededOut
		);*/
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static uint CMSampleBufferGetTotalSampleSize (IntPtr handle);
		
		public uint TotalSampleSize
		{
			get
			{
				return CMSampleBufferGetTotalSampleSize (handle);
			}
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static int CMSampleBufferGetTypeID ();
		
		public static int GetTypeID ()
		{
			return CMSampleBufferGetTypeID ();
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static int CMSampleBufferInvalidate (IntPtr handle);
		
		public int Invalidate()
		{
			return CMSampleBufferInvalidate (handle);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static bool CMSampleBufferIsValid (IntPtr handle);
		
		public bool IsValid
		{
			get
			{
				return CMSampleBufferIsValid (handle);
			}
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static int CMSampleBufferMakeDataReady (IntPtr handle);
		
		public int MakeDataReady ()
		{
			return CMSampleBufferMakeDataReady (handle);
		}
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static int CMSampleBufferSetDataBuffer (IntPtr handle, IntPtr dataBufferHandle);
		
		public int SetDataBuffer (CMBlockBuffer dataBuffer)
		{
			var dataBufferHandle = IntPtr.Zero;
			if (dataBuffer != null)
			{
				dataBufferHandle = dataBuffer.handle;
			}
			return CMSampleBufferSetDataBuffer (handle, dataBufferHandle);
		}
		
		/*[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferSetDataBufferFromAudioBufferList (
		   CMSampleBufferRef sbuf,
		   CFAllocatorRef bbufStructAllocator,
		   CFAllocatorRef bbufMemoryAllocator,
		   uint32_t flags,
		   const AudioBufferList *bufferList
		);*/
		
		[DllImport(Constants.CoreMediaLibrary)]
		extern static int CMSampleBufferSetDataReady (IntPtr handle);
		
		public int SetDataReady ()
		{
			return CMSampleBufferSetDataReady (handle);
		}
		
		/*[DllImport(Constants.CoreMediaLibrary)]
		int CMSampleBufferSetInvalidateCallback (
		   CMSampleBufferRef sbuf,
		   CMSampleBufferInvalidateCallback invalidateCallback,
		   uint64_t invalidateRefCon
		);*/
				
		[DllImport(Constants.CoreMediaLibrary)]
		extern static int CMSampleBufferTrackDataReadiness (IntPtr handle, IntPtr handleToTrack);
		
		public int TrackDataReadiness (CMSampleBuffer bufferToTrack)
		{
			var handleToTrack = IntPtr.Zero;
			if (bufferToTrack != null) {
				handleToTrack = bufferToTrack.handle;
			}
			return CMSampleBufferTrackDataReadiness (handle, handleToTrack);
		}

	}
}
