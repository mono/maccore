//
// NSMutablenData.cs:
// Author:
//   Miguel de Icaza
//
using System;
using MonoMac.ObjCRuntime;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;

namespace MonoMac.Foundation {
	public partial class NSMutableData : IEnumerable, IEnumerable<byte> {

		public override byte this [int idx] {
			set {
				if (idx < 0 || idx >= Int32.MaxValue || idx > (int) Length)
					throw new ArgumentException ("idx");
				Marshal.WriteByte (Bytes, idx, value);
			}
		}

		public void AppendBytes (byte [] bytes)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");
			
			unsafe {
				fixed (byte *p = &bytes[0]){
					AppendBytes ((IntPtr) p, (uint) bytes.Length);
				}
			}
		}

		public void AppendBytes (byte [] bytes, int start, int len)
		{
			if (bytes == null)
				throw new ArgumentNullException ("bytes");

			if (start < 0 || start > bytes.Length)
				throw new ArgumentException ("start");
			if (start+len > bytes.Length)
				throw new ArgumentException ("len");
			
			unsafe {
				fixed (byte *p = &bytes[start]){
					AppendBytes ((IntPtr) p, (uint) len);
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			IntPtr source = Bytes;
			int top = (int) Length;

			for (int i = 0; i < top; i++){
				if (source == Bytes && top == Length)
					yield return Marshal.ReadByte (source, i);
				else
					throw new InvalidOperationException ("The NSMutableData has changed");
			}
		}

		IEnumerator<byte> IEnumerable<byte>.GetEnumerator ()
		{
			IntPtr source = Bytes;
			int top = (int) Length;

			for (int i = 0; i < top; i++){
				if (source == Bytes && top == Length)
					yield return Marshal.ReadByte (source, i);
				else
					throw new InvalidOperationException ("The NSMutableData has changed");
			}
		}
	}
}
