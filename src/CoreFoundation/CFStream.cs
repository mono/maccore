// 
// CFStream.cs:
//
// Authors:
//		Rolf Bjarne Kvinge <rolf@xamarin.com>
//     
// Copyright (C) 2012 Xamarin, Inc.
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
using System.Net;
using System.Runtime.InteropServices;
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;

namespace MonoMac.CoreFoundation {
	[Flags]
	public enum CFStreamEventType
	{
		None = 0,
		OpenCompleted = 1,
		HasBytesAvailable = 2,
		CanAcceptBytes = 4,
		ErrorOccurred = 8,
		EndEncountered = 16
	}
	
	[StructLayout (LayoutKind.Sequential)]
	public struct CFStreamClientContext
	{
		public int Version;
		public IntPtr Info;
		IntPtr retain;
		IntPtr release;
		IntPtr copyDescription;
		
		public void Retain ()
		{
			if (retain == IntPtr.Zero || Info == IntPtr.Zero)
				return;
			
			CFReadStreamRef_InvokeRetain (retain, Info);
		}
		
		public void Release ()
		{
			if (release == IntPtr.Zero || Info == IntPtr.Zero)
				return;
			
			CFReadStreamRef_InvokeRelease (release, Info);
		}
		
		public override string ToString ()
		{
			if (copyDescription == IntPtr.Zero)
				return base.ToString ();
			
			var ptr = CFReadStreamRef_InvokeCopyDescription (copyDescription, Info);
			return ptr == IntPtr.Zero ? base.ToString () : new NSString (ptr).ToString ();
		}
		
		internal void Invoke (IntPtr callback, IntPtr stream, CFStreamEventType eventType)
		{
			if (callback == IntPtr.Zero)
				return;

			CFReadStreamRef_InvokeCallback (callback, stream, eventType, Info);
		}
		
		[MonoNativeFunctionWrapper]
		delegate IntPtr RetainDelegate (IntPtr info);
		static IntPtr CFReadStreamRef_InvokeRetain (IntPtr retain, IntPtr info)
		{
			return ((RetainDelegate) Marshal.GetDelegateForFunctionPointer (retain, typeof (RetainDelegate))) (info);
		}
		
		[MonoNativeFunctionWrapper]
		delegate void ReleaseDelegate (IntPtr info);
		static void CFReadStreamRef_InvokeRelease (IntPtr release, IntPtr info)
		{
			((ReleaseDelegate) Marshal.GetDelegateForFunctionPointer (release, typeof (ReleaseDelegate))) (info);
		}
		
		[MonoNativeFunctionWrapper]
		delegate IntPtr CopyDescriptionDelegate (IntPtr info);
		static IntPtr CFReadStreamRef_InvokeCopyDescription (IntPtr copyDescription, IntPtr info)
		{
			return ((CopyDescriptionDelegate) Marshal.GetDelegateForFunctionPointer (copyDescription, typeof (CopyDescriptionDelegate))) (info);
		}
		
		[MonoNativeFunctionWrapper]
		delegate void CallbackDelegate (IntPtr stream, CFStreamEventType eventType, IntPtr info);
		static void CFReadStreamRef_InvokeCallback (IntPtr callback, IntPtr stream, CFStreamEventType eventType, IntPtr info)
		{
			((CallbackDelegate) Marshal.GetDelegateForFunctionPointer (callback, typeof (CallbackDelegate))) (stream, eventType, info);
		}
	}
}
