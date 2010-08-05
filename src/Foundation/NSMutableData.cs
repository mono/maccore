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
