// 
// AudioFile.cs:
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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MonoMac.CoreFoundation;

using OSStatus = System.Int32;
using AudioFileID = System.IntPtr;

namespace MonoMac.AudioToolbox {

	public enum AudioFileType {
		AIFF = 0x41494646, // AIFF
		AIFC = 0x41494643, // AIFC
		WAVE = 0x57415645, // WAVE
		SoundDesigner2 = 0x53643266, // Sd2f
		Next = 0x4e655854, // NeXT
		MP3 = 0x4d504733, // MPG3
		MP2 = 0x4d504732, // MPG2
		MP1 = 0x4d504731, // MPG1
		AC3 = 0x61632d33, // ac-3
		AAC_ADTS = 0x61647473, // adts
		MPEG4 = 0x6d703466, // mp4f
		M4A = 0x6d346166, // m4af
		CAF = 0x63616666, // caff
		ThreeGP = 0x33677070, // 3gpp
		ThreeGP2 = 0x33677032, // 3gp2
		AMR = 0x616d7266, // amrf
	}

	enum AudioFileError {
		Unspecified = 0x7768743f, // wht?
		UnsupportedFileType = 0x7479703f, // typ?
		UnsupportedDataFormat = 0x666d743f, // fmt?
		UnsupportedProperty = 0x7074793f, // pty?
		BadPropertySize = 0x2173697a, // !siz
		Permissions = 0x70726d3f, // prm?
		NotOptimized = 0x6f70746d, // optm
		InvalidChunk = 0x63686b3f, // chk?
		DoesNotAllow64BitDataSize = 0x6f66663f, // off?
		InvalidPacketOffset = 0x70636b3f, // pck?
		InvalidFile = 0x6474613f, // dta?
		FileNotOpen = -38,
		EndOfFile = -39,
		FileNotFound = -43,
		FilePosition = -40,
	}

	[Flags]
	public enum AudioFilePermission
	{
		Read = 0x01,
		Write = 0x02,
		ReadWrite = 0x03
	}

	[Flags]
	public enum AudioFileFlags {
		EraseFlags = 1,
		DontPageAlignAudioData = 2
	}
	
	public enum AudioFileProperty {
		FileFormat = 0x66666d74,
		DataFormat = 0x64666d74,
		IsOptimized = 0x6f70746d,
		MagicCookieData = 0x6d676963,
		AudioDataByteCount = 0x62636e74,
		AudioDataPacketCount = 0x70636e74,
		MaximumPacketSize = 0x70737a65,
		DataOffset = 0x646f6666,
		ChannelLayout = 0x636d6170,
		DeferSizeUpdates = 0x64737a75,
		DataFormatName = 0x666e6d65,
		MarkerList = 0x6d6b6c73,
		RegionList = 0x72676c73,
		PacketToFrame = 0x706b6672,
		FrameToPacket = 0x6672706b,
		PacketToByte = 0x706b6279,
		ByteToPacket = 0x6279706b,
		ChunkIDs = 0x63686964,
		InfoDictionary = 0x696e666f,
		PacketTableInfo = 0x706e666f,
		FormatList = 0x666c7374,
		PacketSizeUpperBound = 0x706b7562,
		ReserveDuration = 0x72737276,
		EstimatedDuration = 0x65647572,
		BitRate = 0x62726174,
		ID3Tag = 0x69643374,
	}

	public enum AudioFileLoopDirection {
		NoLooping = 0,
		Forward = 1,
		ForwardAndBackward = 2,
		Backward = 3
	}

	[StructLayout(LayoutKind.Sequential)]
	struct AudioFramePacketTranslation {
		public long Frame;
		public long Packet;
		public int FrameOffsetInPacket;
	}

	[StructLayout(LayoutKind.Sequential)]
	struct AudioBytePacketTranslation {
		public long Byte;
		public long Packet;
		public int ByteOffsetInPacket;
		public uint Flags;
	};
	
	[StructLayout(LayoutKind.Sequential)]
	struct AudioFileSmpteTime {
		public sbyte Hours;
		public byte  Minutes;
		public byte  Seconds;
		public byte  Frames;
		public uint  SubFrameSampleOffset;
	}

	[StructLayout(LayoutKind.Sequential)]
	struct AudioFileMarker
	{
		public double FramePosition;
		public IntPtr Name_cfstringref;
		public int    MarkerID;
		public AudioFileSmpteTime SmpteTime;
		public uint Type;
		public ushort Reserved;
		public ushort Channel;

		public string Name {
			get {
				return CFString.FetchString (Name_cfstringref);
			}
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	struct AudioFileMarkerList {
		public uint SmpteTimeType;
		public uint NumberMarkers;
		public AudioFileMarker Markers; // this is a variable length array of mNumberMarkers elements

		public static int RecordsThatFitIn (int n)
		{
			unsafe {
				if (n <= sizeof (AudioFileMarker))
					return 0;
				n -= 8;
				return n / sizeof (AudioFileMarker);
			}
		}

		public static int SizeForMarkers (int markers)
		{
			unsafe {
				return 8 + markers * sizeof (AudioFileMarker);
			}
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	struct AudioFileRegion {
		public uint RegionID;
		public IntPtr Name_cfstringref;
		public uint Flags;
		public int Count;
		public AudioFileMarker Markers; // this is a variable length array of mNumberMarkers elements
	}

	[StructLayout(LayoutKind.Sequential)]
	struct AudioFileRegionList {
		public uint SmpteTimeType;
		public int Count;
		public AudioFileRegion Regions; // this is a variable length array of mNumberRegions elements
	}

	public class AudioFile : IDisposable {
		internal protected IntPtr handle;
		
		protected internal AudioFile (bool x)
		{
			// This ctor is used by AudioSource that will set the handle later.
		}
		
		internal AudioFile (IntPtr handle)
		{
			this.handle = handle;
		}

		~AudioFile ()
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

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileClose (AudioFileID handle);

		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				AudioFileClose (handle);
				handle = IntPtr.Zero;
			}
		}

		public long Length {
			get {
				return GetLong (AudioFileProperty.AudioDataByteCount);
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileCreateWithURL (IntPtr cfurlref_infile, AudioFileType inFileType, ref AudioStreamBasicDescription inFormat, AudioFileFlags inFlags, out AudioFileID file_id);

		public static AudioFile Create (string url, AudioFileType fileType, AudioStreamBasicDescription format, AudioFileFlags inFlags)
		{
			if (url == null)
				throw new ArgumentNullException ("url");

			using (CFUrl cfurl = CFUrl.FromUrlString (url, null))
				return Create (cfurl, fileType, format, inFlags);
		}

		public static AudioFile Create (CFUrl url, AudioFileType fileType, AudioStreamBasicDescription format, AudioFileFlags inFlags)
		{
			if (url == null)
				throw new ArgumentNullException ("url");

			IntPtr h;

			if (AudioFileCreateWithURL (url.Handle, fileType, ref format, inFlags, out h) == 0)
				return new AudioFile (h);
			return null;
		}


		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileOpenURL  (IntPtr cfurlref_infile, byte permissions, AudioFileType fileTypeHint, out IntPtr file_id);

		public static AudioFile OpenRead (string url, AudioFileType fileTypeHint)
		{
			return Open (url, AudioFilePermission.Read, fileTypeHint);
		}
		
		public static AudioFile Open (string url, AudioFilePermission permissions, AudioFileType fileTypeHint)
		{
			if (url == null)
				throw new ArgumentNullException ("url");
			
			using (CFUrl cfurl = CFUrl.FromUrlString (url, null))
				return Open (cfurl, permissions, fileTypeHint);
		}

		public static AudioFile Open (CFUrl url, AudioFilePermission permissions, AudioFileType fileTypeHint)
		{
			if (url == null)
				throw new ArgumentNullException ("url");
			
			IntPtr h;
			if (AudioFileOpenURL (url.Handle, (byte) permissions, fileTypeHint, out h) == 0)
				return new AudioFile (h);
			return null;
		}
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileOptimize (AudioFileID handle);

		public bool Optimize ()
		{
			return AudioFileOptimize (handle) == 0;
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileReadBytes (AudioFileID inAudioFile, bool useCache, long startingByte, ref int numBytes, IntPtr outBuffer);

		public int Read (long startingByte, byte [] buffer, int offset, int count, bool useCache)
		{
			if (offset < 0)
				throw new ArgumentException ("offset", "<0");
			if (count < 0)
				throw new ArgumentException ("count", "<0");
			if (startingByte < 0)
				throw new ArgumentException ("startingByte", "<0");
			int len = buffer.Length;
			if (offset > len)
                                throw new ArgumentException ("destination offset is beyond array size");
                        // reordered to avoid possible integer overflow
                        if (offset > len - count)
                                throw new ArgumentException ("Reading would overrun buffer");

			unsafe {
				fixed (byte *p = &buffer [offset]){
					if (AudioFileReadBytes (handle, useCache, startingByte, ref count, (IntPtr) p) == 0)
						return count;
					else
						return -1;
				}
			}
		}
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileWriteBytes (AudioFileID audioFile, bool useCache, long startingByte, ref int numBytes, IntPtr buffer);

		public int Write (long startingByte, byte [] buffer, int offset, int count, bool useCache)
		{
                        if (offset < 0)
                                throw new ArgumentOutOfRangeException ("offset", "< 0");
                        if (count < 0)
                                throw new ArgumentOutOfRangeException ("count", "< 0");
                        if (offset > buffer.Length - count)
                                throw new ArgumentException ("Reading would overrun buffer");

			unsafe {
				fixed (byte *p = &buffer [offset]){
					if (AudioFileWriteBytes (handle, useCache, startingByte, ref count, (IntPtr) p) == 0)
						return count;
					else
						return -1;
				}
			}
		}

		public int Write (long startingByte, byte [] buffer, int offset, int count, bool useCache, out int errorCode)
		{
                        if (offset < 0)
                                throw new ArgumentOutOfRangeException ("offset", "< 0");
                        if (count < 0)
                                throw new ArgumentOutOfRangeException ("count", "< 0");
                        if (offset > buffer.Length - count)
                                throw new ArgumentException ("Reading would overrun buffer");

			unsafe {
				fixed (byte *p = &buffer [offset]){
					errorCode = AudioFileWriteBytes (handle, useCache, startingByte, ref count, (IntPtr) p);
					if (errorCode == 0)
						return count;
					else
						return -1;
				}
			}
		}
		
		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static OSStatus AudioFileReadPacketData (
			AudioFileID audioFile, bool useCache, ref int numBytes, 
			IntPtr *ptr_outPacketDescriptions, long inStartingPacket, ref int numPackets, IntPtr outBuffer);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static OSStatus AudioFileReadPackets (
			AudioFileID audioFile, bool useCache, ref int numBytes, 
			IntPtr *ptr_outPacketDescriptions, long inStartingPacket, ref int numPackets, IntPtr outBuffer);
		
		public AudioStreamPacketDescription [] ReadPacketData (long inStartingPacket, int nPackets, byte [] buffer)
		{
			if (buffer == null)
				throw new ArgumentNullException ("buffer");
			return RealReadPacketData (false, inStartingPacket, nPackets, buffer, 0, buffer.Length);
		}
		
		public AudioStreamPacketDescription [] ReadPacketData (bool useCache, long inStartingPacket, int nPackets, byte [] buffer, int offset, int count)
		{
			if (buffer == null)
				throw new ArgumentNullException ("buffer");
			if (offset < 0)
				throw new ArgumentException ("offset", "<0");
			if (count < 0)
				throw new ArgumentException ("count", "<0");
			int len = buffer.Length;
			if (offset > len)
                                throw new ArgumentException ("destination offset is beyond array size");
                        // reordered to avoid possible integer overflow
                        if (offset > len - count)
                                throw new ArgumentException ("Reading would overrun buffer");
			return RealReadPacketData (useCache, inStartingPacket, nPackets, buffer, offset, count);
		}

		static internal AudioStreamPacketDescription [] PacketDescriptionFrom (int nPackets, IntPtr b)
		{
			if (b == IntPtr.Zero)
				return new AudioStreamPacketDescription [0];

			var ret = new AudioStreamPacketDescription [nPackets];
			int p = 0;
			for (int i = 0; i < nPackets; i++){
				ret [i].StartOffset = Marshal.ReadInt64 (b, p);
				ret [i].VariableFramesInPacket = Marshal.ReadInt32 (b, p+8);
				ret [i].DataByteSize = Marshal.ReadInt32 (b, p+12);
				p += 16;
			}

			return ret;
		}
		
		unsafe AudioStreamPacketDescription [] RealReadPacketData (bool useCache, long inStartingPacket, int nPackets, byte [] buffer, int offset, int count)
		{
			// sizeof (AudioStreamPacketDescription)  == 16
			var b = Marshal.AllocHGlobal (16 * nPackets);
			try {
				fixed (byte *bop = &buffer [offset]){
					var r = AudioFileReadPacketData (handle, useCache, ref count, &b, inStartingPacket, ref nPackets, (IntPtr) bop);
					if (r != 0)
						return null;
				}

				var ret = PacketDescriptionFrom (nPackets, b);
				return ret;
			} finally {
				Marshal.FreeHGlobal (b);
			}
		}

		public AudioStreamPacketDescription [] ReadFixedPackets (long inStartingPacket, int nPackets, byte [] buffer)
		{
			if (buffer == null)
				throw new ArgumentNullException ("buffer");
			return RealReadFixedPackets (false, inStartingPacket, nPackets, buffer, 0, buffer.Length);
		}
		
		public AudioStreamPacketDescription [] ReadFixedPackets (bool useCache, long inStartingPacket, int nPackets, byte [] buffer, int offset, int count)
		{
			if (buffer == null)
				throw new ArgumentNullException ("buffer");
			if (offset < 0)
				throw new ArgumentException ("offset", "<0");
			if (count < 0)
				throw new ArgumentException ("count", "<0");
			int len = buffer.Length;
			if (offset > len)
                                throw new ArgumentException ("destination offset is beyond array size");
                        // reordered to avoid possible integer overflow
                        if (offset > len - count)
                                throw new ArgumentException ("Reading would overrun buffer");
			return RealReadFixedPackets (useCache, inStartingPacket, nPackets, buffer, offset, count);
		}

		unsafe AudioStreamPacketDescription [] RealReadFixedPackets (bool useCache, long inStartingPacket, int nPackets, byte [] buffer, int offset, int count)
		{
			// 16 == sizeof (AudioStreamPacketDescription) 
			var b = Marshal.AllocHGlobal (16* nPackets);
			try {
				fixed (byte *bop = &buffer [offset]){
					var r = AudioFileReadPacketData (handle, useCache, ref count, &b, inStartingPacket, ref nPackets, (IntPtr) bop);
					if (r != 0)
						return null;
				}
				var ret = new AudioStreamPacketDescription [nPackets];
				int p = 0;
				for (int i = 0; i < nPackets; i++){
					ret [i].StartOffset = Marshal.ReadInt64 (b, p);
					ret [i].VariableFramesInPacket = Marshal.ReadInt32 (b, p+8);
					ret [i].DataByteSize = Marshal.ReadInt32 (b, p+12);
					p += 16;
				}
				return ret;
			} finally {
				Marshal.FreeHGlobal (b);
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileWritePackets (
			AudioFileID audioFile, bool useCache, int inNumBytes, AudioStreamPacketDescription [] inPacketDescriptions,
                        long inStartingPacket, ref int numPackets, IntPtr buffer);

		unsafe public int WritePackets (bool useCache, long inStartingPacket, AudioStreamPacketDescription [] inPacketDescriptions, IntPtr buffer, int count)
		{
			if (inPacketDescriptions == null)
				throw new ArgumentNullException ("inPacketDescriptions");
			if (buffer == IntPtr.Zero)
				throw new ArgumentNullException ("buffer");
			int nPackets = inPacketDescriptions.Length;
			if (AudioFileWritePackets (handle, useCache, count, inPacketDescriptions, inStartingPacket, ref nPackets, buffer) == 0)
				return nPackets;
			return -1;
		}
		
		unsafe public int WritePackets (bool useCache, long startingPacket, AudioStreamPacketDescription [] packetDescriptions, byte [] buffer, int offset, int count)
		{
			if (packetDescriptions == null)
				throw new ArgumentNullException ("inPacketDescriptions");
			if (buffer == null)
				throw new ArgumentNullException ("buffer");
                        if (offset < 0)
                                throw new ArgumentOutOfRangeException ("offset", "< 0");
                        if (count < 0)
                                throw new ArgumentOutOfRangeException ("count", "< 0");
                        if (offset > buffer.Length - count)
                                throw new ArgumentException ("Reading would overrun buffer");

			int nPackets = packetDescriptions.Length;
			fixed (byte *bop = &buffer [offset]){
				if (AudioFileWritePackets (handle, useCache, count, packetDescriptions, startingPacket, ref nPackets, (IntPtr) bop) == 0)
					return nPackets;
				return -1;
			}
		}

		unsafe public int WritePackets (bool useCache, long inStartingPacket, AudioStreamPacketDescription [] inPacketDescriptions, IntPtr buffer, int count, out int errorCode)
		{
			if (inPacketDescriptions == null)
				throw new ArgumentNullException ("inPacketDescriptions");
			if (buffer == IntPtr.Zero)
				throw new ArgumentNullException ("buffer");
			int nPackets = inPacketDescriptions.Length;
			
			errorCode = AudioFileWritePackets (handle, useCache, count, inPacketDescriptions, inStartingPacket, ref nPackets, buffer);
			if (errorCode == 0)
				return nPackets;
			return -1;
		}
		
		unsafe public int WritePackets (bool useCache, long startingPacket, AudioStreamPacketDescription [] packetDescriptions, byte [] buffer, int offset, int count, out int errorCode)
		{
			if (packetDescriptions == null)
				throw new ArgumentNullException ("inPacketDescriptions");
			if (buffer == null)
				throw new ArgumentNullException ("buffer");
                        if (offset < 0)
                                throw new ArgumentOutOfRangeException ("offset", "< 0");
                        if (count < 0)
                                throw new ArgumentOutOfRangeException ("count", "< 0");
                        if (offset > buffer.Length - count)
                                throw new ArgumentException ("Reading would overrun buffer");

			int nPackets = packetDescriptions.Length;
			fixed (byte *bop = &buffer [offset]){
				errorCode = AudioFileWritePackets (handle, useCache, count, packetDescriptions, startingPacket, ref nPackets, (IntPtr) bop);
				if (errorCode == 0)
					return nPackets;
				return -1;
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileCountUserData (AudioFileID handle, uint userData, out int count);

		public int CountUserData (uint userData)
		{
			int count;
			if (AudioFileCountUserData (handle, userData, out count) == 0)
				return count;
			return -1;
		}
			
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileGetUserDataSize (AudioFileID audioFile, uint userDataID, int index, out int userDataSize);
		public int GetUserDataSize (uint userDataId, int index)
		{
			int ds;
			
			if (AudioFileGetUserDataSize (handle, userDataId, index, out ds) == 0)
				return -1;
			return ds;
		}
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileGetUserData (AudioFileID audioFile, int userDataID, int index, ref int userDataSize, IntPtr userData);

		public int GetUserData (int userDataID, int index, ref int size, IntPtr userData)
		{
			return AudioFileGetUserData (handle, userDataID, index, ref size, userData);
		}
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileSetUserData (AudioFileID inAudioFile, int userDataID, int index, int userDataSize, IntPtr userData);

		public int SetUserData (int userDataId, int index, int userDataSize, IntPtr userData)
		{
			return AudioFileSetUserData (handle, userDataId, index, userDataSize, userData);
		}
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileRemoveUserData (AudioFileID audioFile, int userDataID, int index);

		public int RemoveUserData (int userDataId, int index)
		{
			return AudioFileRemoveUserData (handle, userDataId, index);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileGetPropertyInfo (AudioFileID audioFile, AudioFileProperty propertyID, out int outDataSize, out int isWritable);

		public bool GetPropertyInfo (AudioFileProperty property, out int size, out int writable)
		{
			return AudioFileGetPropertyInfo (handle, property, out size, out writable) == 0;
		}
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileGetProperty (AudioFileID audioFile, AudioFileProperty property, ref int dataSize, IntPtr outdata);

		public bool GetProperty (AudioFileProperty property, ref int dataSize, IntPtr outdata)
		{
			return AudioFileGetProperty (handle, property, ref dataSize, outdata) == 0;
		}

		public IntPtr GetProperty (AudioFileProperty property, out int size)
		{
			int writable;

			var r = AudioFileGetPropertyInfo (handle, property, out size, out writable);
			if (r != 0)
				return IntPtr.Zero;

			var buffer = Marshal.AllocHGlobal (size);
			if (buffer == IntPtr.Zero)
				return IntPtr.Zero;

			r = AudioFileGetProperty (handle, property, ref size, buffer);
			if (r == 0)
				return buffer;
			Marshal.FreeHGlobal (buffer);
			return IntPtr.Zero;
		}

		T GetProperty<T> (AudioFileProperty property)
		{
			int size, writable;

			if (AudioFileGetPropertyInfo (handle, property, out size, out writable) != 0)
				return default (T);
			var buffer = Marshal.AllocHGlobal (size);
			if (buffer == IntPtr.Zero)
				return default(T);
			try {
				var r = AudioFileGetProperty (handle, property, ref size, buffer);
				if (r == 0)
					return (T) Marshal.PtrToStructure (buffer, typeof (T));

				return default(T);
			} finally {
				Marshal.FreeHGlobal (buffer);
			}
		}
		
		int GetInt (AudioFileProperty property)
		{
			unsafe {
				int val = 0;
				int size = 4;
				if (AudioFileGetProperty (handle, property, ref size, (IntPtr) (&val)) == 0)
					return val;
				return 0;
			}
		}

		double GetDouble (AudioFileProperty property)
		{
			unsafe {
				double val = 0;
				int size = 8;
				if (AudioFileGetProperty (handle, property, ref size, (IntPtr) (&val)) == 0)
					return val;
				return 0;
			}
		}
		
		long GetLong (AudioFileProperty property)
		{
			unsafe {
				long val = 0;
				int size = 8;
				if (AudioFileGetProperty (handle, property, ref size, (IntPtr) (&val)) == 0)
					return val;
				return 0;
			}
		}
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileSetProperty (AudioFileID audioFile, AudioFileProperty property, int dataSize, IntPtr propertyData);

		public bool SetProperty (AudioFileProperty property, int dataSize, IntPtr propertyData)
		{
			return AudioFileSetProperty (handle, property, dataSize, propertyData) == 0;
		}      

		void SetInt (AudioFileProperty property, int value)
		{
			unsafe {
				AudioFileSetProperty (handle, property, 4, (IntPtr) (&value));
			}
		}
		
		public AudioFileType FileType {
			get {
				return (AudioFileType) GetInt (AudioFileProperty.FileFormat);
			}
		}

		public AudioStreamBasicDescription StreamBasicDescription {
			get {
				return GetProperty<AudioStreamBasicDescription> (AudioFileProperty.DataFormat);
			}
		}

		// TODO: kAudioFilePropertyFormatList
		public bool IsOptimized {
			get {
				return GetInt (AudioFileProperty.IsOptimized) == 1;
			}
		}

		public byte [] MagicCookie {
			get {
				int size;
				var h = GetProperty (AudioFileProperty.MagicCookieData, out size);
				if (h == IntPtr.Zero)
					return new byte [0];
				
				byte [] cookie = new byte [size];
				for (int i = 0; i < cookie.Length; i++)
					cookie [i] = Marshal.ReadByte (h, i);
				Marshal.FreeHGlobal (h);

				return cookie;
			}

			set {
				if (value == null)
					throw new ArgumentNullException ("value");

				unsafe {
					fixed (byte *bp = &value [0]){
						SetProperty (AudioFileProperty.MagicCookieData, value.Length, (IntPtr) bp);
					}
				}
			}
		}

		public long DataPacketCount {
			get {
				return GetLong (AudioFileProperty.AudioDataPacketCount);
			}
		}

		public int MaximumPacketSize {
			get {
				return GetInt (AudioFileProperty.MaximumPacketSize);
			}
		}

		public long DataOffset {
			get {
				return GetLong (AudioFileProperty.DataOffset);
			}
		}

		unsafe static float ReadFloat (IntPtr p, int offset)
		{
			float f;
			var pf = &f;
			byte *pb = (byte *) pf;
			byte *src = ((byte *)p) + offset;
			
			pb [0] = src [0];
			pb [1] = src [1];
			pb [2] = src [2];
			pb [3] = src [3];

			return f;
		}

		unsafe static void WriteFloat (IntPtr p, int offset, float f)
		{
			var pf = &f;
			byte *pb = (byte *) pf;
			byte *dest = ((byte *)p) + offset;
			
			dest [0] = pb [0];
			dest [1] = pb [1];
			dest [2] = pb [2];
			dest [3] = pb [3];
		}
		
		static internal AudioChannelLayout AudioChannelLayoutFromHandle (IntPtr h)
		{
			var layout = new AudioChannelLayout ();
			layout.AudioTag  = (AudioChannelLayoutTag) Marshal.ReadInt32 (h, 0);
			layout.Bitmap = Marshal.ReadInt32 (h, 4);
			layout.Channels = new AudioChannelDescription [Marshal.ReadInt32 (h, 8)];
			int p = 12;
			for (int i = 0; i < layout.Channels.Length; i++){
				var desc = new AudioChannelDescription ();
				desc.Label = (AudioChannelLabel) Marshal.ReadInt32 (h, p);
				desc.Flags = (AudioChannelFlags) Marshal.ReadInt32 (h, p+4);
				desc.Coords = new float [3];
				desc.Coords [0] = ReadFloat (h, p+8);
				desc.Coords [1] = ReadFloat (h, p+12);
				desc.Coords [2] = ReadFloat (h, p+16);
				layout.Channels [i] = desc;
				
				p += 20;
			}

			return layout;
		}

		static internal IntPtr AudioChannelLayoutToBlock (AudioChannelLayout layout, out int size)
		{
			if (layout == null)
				throw new ArgumentNullException ("layout");
			if (layout.Channels == null)
				throw new ArgumentNullException ("layout.Channels");
			
			size = 12 + layout.Channels.Length * 20;
			IntPtr buffer = Marshal.AllocHGlobal (size);
			int p;
			Marshal.WriteInt32 (buffer, 0, (int) layout.AudioTag);
			Marshal.WriteInt32 (buffer, 4, layout.Bitmap);
			Marshal.WriteInt32 (buffer, 8, layout.Channels.Length);
			p = 12;
			foreach (var desc in layout.Channels){
				Marshal.WriteInt32 (buffer, p, (int) desc.Label);
				Marshal.WriteInt32 (buffer, p + 4, (int) desc.Flags);
				WriteFloat (buffer, p + 8, desc.Coords [0]);
				WriteFloat (buffer, p + 12, desc.Coords [1]);
				WriteFloat (buffer, p + 16, desc.Coords [2]);

				p += 20;
			}
			
			return buffer;
		}
		
		public AudioChannelLayout ChannelLayout {
			get {
				int size;
				var h = GetProperty (AudioFileProperty.ChannelLayout, out size);
				if (h == IntPtr.Zero)
					return null;
				
				var layout = AudioChannelLayoutFromHandle (h);
				Marshal.FreeHGlobal (h);

				return layout;
			}
		}

		public bool DeferSizeUpdates {
			get {
				return GetInt (AudioFileProperty.DeferSizeUpdates) == 1;
			}
			set {
				SetInt (AudioFileProperty.DeferSizeUpdates, value ? 1 : 0);
			}
		}

		public int BitRate {
			get {
				return GetInt (AudioFileProperty.BitRate);
			}
		}

		public double EstimatedDuration {
			get {
				return GetDouble (AudioFileProperty.EstimatedDuration);
			}
		}

		public int PacketSizeUpperBound {
			get {
				return GetInt (AudioFileProperty.PacketSizeUpperBound);
			}
		}

		public long PacketToFrame (long packet)
		{
			AudioFramePacketTranslation buffer;
			buffer.Packet = packet;

			unsafe {
				AudioFramePacketTranslation *p = &buffer;
				int size = Marshal.SizeOf (buffer);
				if (AudioFileGetProperty (handle, AudioFileProperty.PacketToFrame, ref size, (IntPtr) p) == 0)
					return buffer.Frame;
				return -1;
			}
		}

		public long FrameToPacket (long frame, out int frameOffsetInPacket)
		{
			AudioFramePacketTranslation buffer;
			buffer.Frame = frame;

			unsafe {
				AudioFramePacketTranslation *p = &buffer;
				int size = Marshal.SizeOf (buffer);
				if (AudioFileGetProperty (handle, AudioFileProperty.FrameToPacket, ref size, (IntPtr) p) == 0){
					frameOffsetInPacket = buffer.FrameOffsetInPacket;
					return buffer.Packet;
				}
				frameOffsetInPacket = 0;
				return -1;
			}
		}

		public long PacketToByte (long packet, out bool isEstimate)
		{
			AudioBytePacketTranslation buffer;
			buffer.Packet = packet;

			unsafe {
				AudioBytePacketTranslation *p = &buffer;
				int size = Marshal.SizeOf (buffer);
				if (AudioFileGetProperty (handle, AudioFileProperty.PacketToByte, ref size, (IntPtr) p) == 0){
					isEstimate = (buffer.Flags & 1) == 1;
					return buffer.Byte;
				}
				isEstimate = false;
				return -1;
			}
		}

		public long ByteToPacket (long byteval, out int byteOffsetInPacket, out bool isEstimate)
		{
			AudioBytePacketTranslation buffer;
			buffer.Byte = byteval;

			unsafe {
				AudioBytePacketTranslation *p = &buffer;
				int size = Marshal.SizeOf (buffer);
				if (AudioFileGetProperty (handle, AudioFileProperty.ByteToPacket, ref size, (IntPtr) p) == 0){
					isEstimate = (buffer.Flags & 1) == 1;
					byteOffsetInPacket = buffer.ByteOffsetInPacket;
					return buffer.Packet;
				}
				byteOffsetInPacket = 0;
				isEstimate = false;
				return -1;
			}
		}
		
//		MarkerList = 0x6d6b6c73,
//		RegionList = 0x72676c73,
//		ChunkIDs = 0x63686964,
//		InfoDictionary = 0x696e666f,
//		PacketTableInfo = 0x706e666f,
//		FormatList = 0x666c7374,
//		ReserveDuration = 0x72737276,
//		EstimatedDuration = 0x65647572,
//		ID3Tag = 0x69643374,
//		
	}

	delegate int ReadProc (IntPtr clientData, long position, int requestCount, IntPtr buffer, out int actualCount);
	delegate int WriteProc (IntPtr clientData, long position, int requestCount, IntPtr buffer, out int actualCount);
	delegate long GetSizeProc (IntPtr clientData);
	delegate int SetSizeProc (IntPtr clientData, long size);
	
	public abstract class AudioSource : AudioFile {
		static ReadProc dRead;
		static WriteProc dWrite;
		static GetSizeProc dGetSize;
		static SetSizeProc dSetSize;

		GCHandle gch;
		
		static AudioSource ()
		{
			dRead = SourceRead;
			dWrite = SourceWrite;
			dGetSize = SourceGetSize;
			dSetSize = SourceSetSize;
		}

		[MonoPInvokeCallback(typeof(ReadProc))]
		static int SourceRead (IntPtr clientData, long inPosition, int requestCount, IntPtr buffer, out int actualCount)
		{
			GCHandle handle = GCHandle.FromIntPtr (clientData);
			var audioSource = handle.Target as AudioSource;
			return audioSource.Read (inPosition, requestCount, buffer, out actualCount) ? 0 : 1;
		}

		public abstract bool Read (long position, int requestCount, IntPtr buffer, out int actualCount);

		[MonoPInvokeCallback(typeof(WriteProc))]
		static int SourceWrite (IntPtr clientData, long position, int requestCount, IntPtr buffer, out int actualCount)
		{
			GCHandle handle = GCHandle.FromIntPtr (clientData);
			var audioSource = handle.Target as AudioSource;
			return audioSource.Write (position, requestCount, buffer, out actualCount) ? 0 : 1;
		}
		public abstract bool Write (long position, int requestCount, IntPtr buffer, out int actualCount);

		[MonoPInvokeCallback(typeof(GetSizeProc))]
		static long SourceGetSize (IntPtr clientData)
		{
			GCHandle handle = GCHandle.FromIntPtr (clientData);
			var audioSource = handle.Target as AudioSource;
			return audioSource.Size;
		}

		[MonoPInvokeCallback(typeof(SetSizeProc))]
		static int SourceSetSize (IntPtr clientData, long size)
		{
			GCHandle handle = GCHandle.FromIntPtr (clientData);
			var audioSource = handle.Target as AudioSource;
			
			audioSource.Size = size;
			return 0;
		}
		public abstract long Size { get; set; }

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			if (gch.IsAllocated)
				gch.Free ();
		}
		
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioFileInitializeWithCallbacks (	
			IntPtr inClientData, ReadProc inReadFunc, WriteProc inWriteFunc, GetSizeProc inGetSizeFunc, SetSizeProc inSetSizeFunc,
			AudioFileType inFileType, ref AudioStreamBasicDescription format, uint flags, out IntPtr id);

		public AudioSource (AudioFileType inFileType, AudioStreamBasicDescription format) : base (true)
		{
			IntPtr h;

			gch = GCHandle.Alloc (this);
			var code = AudioFileInitializeWithCallbacks (GCHandle.ToIntPtr (gch), dRead, dWrite, dGetSize, dSetSize, inFileType, ref format, 0, out h);
			if (code == 0){
				handle = h;
				return;
			}
			throw new Exception (String.Format ("Unable to create AudioSource, code: 0x{0:x}", code));
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static int AudioFileOpenWithCallbacks (
			IntPtr inClientData, ReadProc inReadFunc, WriteProc inWriteFunc,
			GetSizeProc inGetSizeFunc, SetSizeProc	inSetSizeFunc, AudioFileType inFileTypeHint, out IntPtr outAudioFile);
		
		public AudioSource (AudioFileType fileTypeHint) : base (true)
		{
			IntPtr h;

			gch = GCHandle.Alloc (this);
			var code = AudioFileOpenWithCallbacks (GCHandle.ToIntPtr (gch), dRead, dWrite, dGetSize, dSetSize, fileTypeHint, out h);
			if (code == 0){
				handle = h;
				return;
			}
			throw new Exception (String.Format ("Unable to create AudioSource, code: 0x{0:x}", code));
		}
	}		
}

