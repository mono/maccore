//
// AudioBufferList.cs: AudioBufferList wrapper class
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
using System.Runtime.InteropServices;
using MonoMac.ObjCRuntime;

namespace MonoMac.AudioToolbox
{
	[StructLayout(LayoutKind.Sequential)]
	public class AudioBufferList : IDisposable {
		#region Variables
		int bufferCount;
		// mBuffers array size is variable. But here we uses fixed size of 2, because iPhone phone terminal two (L/R) channels.        
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		AudioBuffer [] buffers;
		#endregion
		uint allocated;
		
		public int BufferCount { get { return bufferCount; }}
		public AudioBuffer [] Buffers { get { return buffers; }}
		
		#region Constructor
		public AudioBufferList() 
		{
		}

		public AudioBufferList (int nubuffers, int bufferSize)
		{
			allocated = 0xcafebabe;
			bufferCount = nubuffers;
			buffers = new AudioBuffer[bufferCount];
			for (int i = 0; i < bufferCount; i++) {
				buffers[i].NumberChannels = 1;
				buffers[i].DataByteSize = bufferSize;
				buffers[i].Data = Marshal.AllocHGlobal((int)bufferSize);
			}
		}
		#endregion
			
		#region IDisposable メンバ (Members)
		public void Dispose()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public virtual void Dispose (bool disposing)
		{
			if (buffers != null && allocated != 0){
				foreach (var mbuf in buffers)
					Marshal.FreeHGlobal(mbuf.Data);
				buffers = null;
			}
		}

		~AudioBufferList ()
		{
			Dispose (false);
		}
		#endregion

		public override string ToString ()
		{
			if (buffers != null)
				return string.Format ("[buffers={0},bufferSize={1}]", buffers [0].DataByteSize);
			else
				return string.Format ("[empty]");
		}
	}
}
