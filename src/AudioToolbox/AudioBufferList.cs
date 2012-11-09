//
// AudioBufferList.cs: AudioBufferList wrapper class
//
// Authors:
//   AKIHIRO Uehara (u-akihiro@reinforce-lab.com)
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2010 Reinforce Lab.
// Copyright 2011, 2012 Xamarin Inc.
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
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.AudioToolbox
{
	public class AudioBufferList
	{
		internal unsafe AudioBufferList (IntPtr _ptr)
		{
			byte *ptr = (byte *) _ptr;
			//
			// Decodes
			//
			// struct AudioBufferList
			// {
			//    UInt32      mNumberBuffers;
			//    AudioBuffer mBuffers[1]; // this is a variable length array of mNumberBuffers elements
			// }

			int count = Marshal.ReadInt32 ((IntPtr) ptr, 0);
			ptr += sizeof (int);

			Buffers = new AudioBuffer [count];

			for (int i = 0; i < count; ++i) {
				Buffers [i] = (AudioBuffer) Marshal.PtrToStructure ((IntPtr) ptr, typeof (AudioBuffer));
				ptr += Marshal.SizeOf (typeof (AudioBuffer));
			}
		}

		public AudioBufferList (int bufferSize)
		{
			Buffers = new AudioBuffer [bufferSize];
		}

		public AudioBuffer[] Buffers { get; set; }

		// Caller is resposible for releasing the structure
		public  IntPtr ToPointer ()
		{
			var size = sizeof (int);
			if (Buffers.Length != 0)
				size += Buffers.Length * Marshal.SizeOf (Buffers [0]);

			IntPtr buffer = Marshal.AllocHGlobal (size);
			Marshal.WriteInt32 (buffer, 0, Buffers.Length);

			unsafe {
				var ptr = (byte *) buffer + sizeof (int);
				foreach (var b in Buffers) {
					Marshal.StructureToPtr (b, (IntPtr) ptr, false);
					ptr += Marshal.SizeOf (typeof (AudioBuffer));
				}
			}
			
			return buffer;
		}
	}

	[Obsolete ("Use AudioBufferList")]
	public class MutableAudioBufferList : AudioBufferList, IDisposable {
		public MutableAudioBufferList (int nubuffers, int bufferSize)
			: base (nubuffers)
		{
			for (int i = 0; i < nubuffers; i++) {
				Buffers[i].NumberChannels = 1;
				Buffers[i].DataByteSize = bufferSize;
				Buffers[i].Data = Marshal.AllocHGlobal((int)bufferSize);
			}
		}
			
		public void Dispose()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public virtual void Dispose (bool disposing)
		{
			if (Buffers != null){
				foreach (var mbuf in Buffers)
					Marshal.FreeHGlobal(mbuf.Data);
				Buffers = null;
			}
		}

		~MutableAudioBufferList ()
		{
			Dispose (false);
		}
	}

}
