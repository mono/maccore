//
// ExtAudioFile.cs: ExtAudioFile wrapper class
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
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using MonoMac.AudioToolbox;

namespace MonoMac.AudioUnit
{
    public class ExtAudioFile : IDisposable
    {
        #region Variables
        const int kAudioUnitSampleFractionBits = 24;
        readonly IntPtr _extAudioFile;
        #endregion

        #region Property        
        public long FileLengthFrames
        {
            get {
                long length = 0;
                uint size   = (uint)Marshal.SizeOf(typeof(long));
                
                int err = ExtAudioFileGetProperty(_extAudioFile,
                    ExtAudioFilePropertyIDType.kExtAudioFileProperty_FileLengthFrames,
                    ref size, ref length);
                if (err != 0)
                {
                    throw new InvalidOperationException(String.Format("Error code:{0}", err));
                }

                return length;
            }
        }

        public AudioStreamBasicDescription FileDataFormat
        {
            get
            {
                AudioStreamBasicDescription dc = new AudioStreamBasicDescription();
                uint size = (uint)Marshal.SizeOf(typeof(AudioStreamBasicDescription));
                int err = ExtAudioFileGetProperty(_extAudioFile,
                    ExtAudioFilePropertyIDType.kExtAudioFileProperty_FileDataFormat,
                    ref size, ref dc);
                if (err != 0)
                {
                    throw new InvalidOperationException(String.Format("Error code:{0}", err));
                }

                return dc;
            }
        }

        public AudioStreamBasicDescription ClientDataFormat
        {
            set
            {                
                int err = ExtAudioFileSetProperty(_extAudioFile,
                    ExtAudioFilePropertyIDType.kExtAudioFileProperty_ClientDataFormat,
                    (uint)Marshal.SizeOf(value), ref value);
                if (err != 0)
                {
                    throw new InvalidOperationException(String.Format("Error code:{0}", err));
                }
            }
        }           
        #endregion

        #region Constructor
        private ExtAudioFile(IntPtr ptr)
        {
            _extAudioFile = ptr;
        }
        #endregion

        #region Private methods
        #endregion

        #region Public methods        
        public static ExtAudioFile OpenUrl(MonoMac.CoreFoundation.CFUrl url)
        { 
            int err;
            IntPtr ptr = new IntPtr();
            unsafe {                
                err = ExtAudioFileOpenUrl(url.Handle, (IntPtr)(&ptr));
            }            
            if (err != 0)
            {
                throw new ArgumentException(String.Format("Error code:{0}", err));
            }
            if (ptr == IntPtr.Zero)
            {
                throw new InvalidOperationException("Can not get object instance");
            }
            
            return new ExtAudioFile(ptr);
        }
        public static ExtAudioFile CreateWithUrl(MonoMac.CoreFoundation.CFUrl url,
            AudioFileType fileType, 
            AudioStreamBasicDescription inStreamDesc, 
            //AudioChannelLayout channelLayout, 
            AudioFileFlags flag)
        {             
            int err;
            IntPtr ptr = new IntPtr();
            unsafe {                
                err = ExtAudioFileCreateWithUrl(url.Handle, fileType, ref inStreamDesc, IntPtr.Zero, (uint)flag,
                    (IntPtr)(&ptr));
            }            
            if (err != 0)
            {
                throw new ArgumentException(String.Format("Error code:{0}", err));
            }
            if (ptr == IntPtr.Zero)
            {
                throw new InvalidOperationException("Can not get object instance");
            }
            
            return new ExtAudioFile(ptr);         
        }
        public void Seek(long frameOffset)
        {
            int err = ExtAudioFileSeek(_extAudioFile, frameOffset);
            if (err != 0)
            {
                throw new ArgumentException(String.Format("Error code:{0}", err));
            }
        }
        public long FileTell()
        {
            long frame = 0;
            
            int err = ExtAudioFileTell(_extAudioFile, ref frame);
            if (err != 0)
            {
                throw new ArgumentException(String.Format("Error code:{0}", err));
            }
            
            return frame;
        }
        public int Read(int numberFrames, AudioBufferList data)
        {            
            int err = ExtAudioFileRead(_extAudioFile, ref numberFrames, data);
            if (err != 0)
            {
                throw new ArgumentException(String.Format("Error code:{0}", err));
            }

            return numberFrames;
        }
        public void WriteAsync(int numberFrames, AudioBufferList data)
        {
            int err = ExtAudioFileWriteAsync(_extAudioFile, numberFrames, data);
            if (err != 0)
                throw new ArgumentException(String.Format("Error code:{0}", err));            
        }         
        #endregion

        #region IDisposable メンバ
        public void Dispose()
        {
            ExtAudioFileDispose(_extAudioFile);            
        }
        #endregion


        #region Interop
        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileOpenURL")]
        static extern int ExtAudioFileOpenUrl(IntPtr inUrl, IntPtr outExtAudioFile); // caution

        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileRead")]
        static extern int ExtAudioFileRead(IntPtr  inExtAudioFile, ref int ioNumberFrames, AudioBufferList ioData);

        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileWriteAsync")]
        static extern int ExtAudioFileWriteAsync(IntPtr inExtAudioFile, int inNumberFrames, AudioBufferList ioData);

        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileDispose")]
        static extern int ExtAudioFileDispose(IntPtr inExtAudioFile);

        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileSeek")]
        static extern int ExtAudioFileSeek(IntPtr inExtAudioFile, long inFrameOffset);
        
        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileTell")]
        static extern int ExtAudioFileTell(IntPtr inExtAudioFile, ref long outFrameOffset);
        
        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileCreateWithURL")]
        static extern int ExtAudioFileCreateWithUrl(IntPtr inURL,
            [MarshalAs(UnmanagedType.U4)] AudioFileType inFileType,
            ref AudioStreamBasicDescription inStreamDesc,
            IntPtr inChannelLayout, //AudioChannelLayout inChannelLayout, AudioChannelLayout results in compilation error (error code 134.)
            UInt32 flags,
            IntPtr outExtAudioFile);            
        
        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileGetProperty")]
        static extern int ExtAudioFileGetProperty(
            IntPtr inExtAudioFile, 
            ExtAudioFilePropertyIDType inPropertyID,
            ref uint ioPropertyDataSize,
            IntPtr outPropertyData);
        
        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileGetProperty")]
        static extern int ExtAudioFileGetProperty(
            IntPtr inExtAudioFile,
            ExtAudioFilePropertyIDType inPropertyID,
            ref uint ioPropertyDataSize,
            ref AudioStreamBasicDescription outPropertyData);
        

        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileGetProperty")]
        static extern int ExtAudioFileGetProperty(
            IntPtr inExtAudioFile,
            ExtAudioFilePropertyIDType inPropertyID,
            ref uint ioPropertyDataSize,
            ref long outPropertyData);

        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileSetProperty")]
        static extern int ExtAudioFileSetProperty(
            IntPtr inExtAudioFile,
            ExtAudioFilePropertyIDType inPropertyID,
            uint ioPropertyDataSize,
            IntPtr outPropertyData);

        [DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "ExtAudioFileSetProperty")]
        static extern int ExtAudioFileSetProperty(
            IntPtr inExtAudioFile,
            ExtAudioFilePropertyIDType inPropertyID,
            uint ioPropertyDataSize,
            ref AudioStreamBasicDescription outPropertyData);
        
        enum ExtAudioFilePropertyIDType {                 
	        kExtAudioFileProperty_FileDataFormat		= 0x66666d74, //'ffmt',   // AudioStreamBasicDescription
	        //kExtAudioFileProperty_FileChannelLayout		= 'fclo',   // AudioChannelLayout

            kExtAudioFileProperty_ClientDataFormat = 0x63666d74, //'cfmt',   // AudioStreamBasicDescription
	        //kExtAudioFileProperty_ClientChannelLayout	= 'cclo',   // AudioChannelLayout
	        //kExtAudioFileProperty_CodecManufacturer		= 'cman',	// UInt32
	
	        // read-only:
	        //kExtAudioFileProperty_AudioConverter		= 'acnv',	// AudioConverterRef
	        //kExtAudioFileProperty_AudioFile				= 'afil',	// AudioFileID
	        //kExtAudioFileProperty_FileMaxPacketSize		= 'fmps',	// UInt32
	        //kExtAudioFileProperty_ClientMaxPacketSize	= 'cmps',	// UInt32
	        kExtAudioFileProperty_FileLengthFrames		= 0x2366726d,//'#frm',	// SInt64
	
	        // writable:
	        //kExtAudioFileProperty_ConverterConfig		= 'accf',   // CFPropertyListRef
	        //kExtAudioFileProperty_IOBufferSizeBytes		= 'iobs',	// UInt32
	        //kExtAudioFileProperty_IOBuffer				= 'iobf',	// void *
	        //kExtAudioFileProperty_PacketTable			= 'xpti'	// AudioFilePacketTableInfo             
        };
        #endregion
    }
}
