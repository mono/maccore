//
// NSData.cs:
// Author:
//   Miguel de Icaza
//
// Copyright 2010, Novell, Inc.
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
using MonoMac.ObjCRuntime;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;

namespace MonoMac.Foundation {
	public partial class NSData : IEnumerable, IEnumerable<byte> {
		
		// some API, like SecItemCopyMatching, returns a retained NSData
		internal NSData (IntPtr handle, bool owns)
			: base (handle)
		{
			if (!owns)
				Release ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			IntPtr source = Bytes;
			int top = (int) Length;

			for (int i = 0; i < top; i++){
				yield return Marshal.ReadByte (source, i);
			}
		}

		IEnumerator<byte> IEnumerable<byte>.GetEnumerator ()
		{
			IntPtr source = Bytes;
			int top = (int) Length;

			for (int i = 0; i < top; i++)
				yield return Marshal.ReadByte (source, i);
		}
		
		public static NSData FromString (string s)
		{
			if (s == null)
				throw new ArgumentNullException ("s");
			return new NSString (s).Encode (NSStringEncoding.UTF8);
		}

		public static NSData FromArray (byte [] buffer)
		{
			if (buffer == null)
				throw new ArgumentNullException ("buffer");
			
			if (buffer.Length == 0)
				return FromBytes (IntPtr.Zero, 0);
			
			unsafe {
				fixed (byte *ptr = &buffer [0]){
					return FromBytes ((IntPtr) ptr, (uint) buffer.Length);
				}
			}
		}

		public static NSData FromStream (Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException ("stream");
			
			if (!stream.CanRead)
				return null;

			NSMutableData ret = null;
			long len;
			try {
				len = stream.Length;
			} catch {
				len = 8192;
			}
			ret = NSMutableData.FromCapacity ((int)len);
			byte [] buffer = new byte [32*1024];
			int n;
			try {
				unsafe {
					while ((n = stream.Read (buffer, 0, buffer.Length)) != 0){
						fixed (byte *ptr = &buffer [0])
							ret.AppendBytes ((IntPtr) ptr, (uint) n);
					}
				}
			} catch {
				return null;
			}
			return ret;
		}

		//
		// Keeps a ref to the source NSData
		//
		unsafe class UnmanagedMemoryStreamWithRef : UnmanagedMemoryStream {
			NSData source;
			
			public UnmanagedMemoryStreamWithRef (NSData source, byte *pointer, long length) : base (pointer, length)
			{
				this.source = source;
			}

			protected override void Dispose (bool disposing)
			{
				source = null;
				base.Dispose (disposing);
			}
		}

		public virtual Stream AsStream ()
		{
			if (this is NSMutableData)
				throw new Exception ("Wrapper for NSMutableData is not supported, call new UnmanagedMemoryStream ((Byte*) mutableData.Bytes, mutableData.Length) instead");

			unsafe {
				return new UnmanagedMemoryStreamWithRef (this, (byte *) Bytes, Length);
			}
		}
		
		public static NSData FromString (string s, NSStringEncoding encoding)
		{
			return new NSString (s).Encode (encoding);
		}

		public static implicit operator NSData (string s)
		{
			return new NSString (s).Encode (NSStringEncoding.UTF8);
		}

		public NSString ToString (NSStringEncoding encoding)
		{
			return NSString.FromData (this, encoding);
		}
		
		public override string ToString ()
		{
			return ToString (NSStringEncoding.UTF8);
		}

		public bool Save (string file, bool auxiliaryFile, out NSError error)
		{
			unsafe {
				IntPtr val;
				IntPtr val_addr = (IntPtr) ((IntPtr *) &val);

				bool ret = _Save (file, auxiliaryFile ? 1 : 0, val_addr);
				error = (NSError) Runtime.GetNSObject (val);
				
				return ret;
			}
		}

		public bool Save (string file, NSDataWritingOptions options, out NSError error)
		{
			unsafe {
				IntPtr val;
				IntPtr val_addr = (IntPtr) ((IntPtr *) &val);

				bool ret = _Save (file, (int) options, val_addr);
				error = (NSError) Runtime.GetNSObject (val);
				
				return ret;
			}
		}

		public bool Save (NSUrl url, bool auxiliaryFile, out NSError error)
		{
			unsafe {
				IntPtr val;
				IntPtr val_addr = (IntPtr) ((IntPtr *) &val);

				bool ret = _Save (url, auxiliaryFile ? 1 : 0, val_addr);
				error = (NSError) Runtime.GetNSObject (val);
				
				return ret;
			}
		}

		public virtual byte this [int idx] {
			get {
				if (idx < 0 || idx >= Int32.MaxValue || idx > (int) Length)
					throw new ArgumentException ("idx");
				return Marshal.ReadByte (Bytes, idx);
			}

			set {
				throw new NotImplementedException ("NSData arrays can not be modified, use an NSMUtableData instead");
			}
		}
	}
}
