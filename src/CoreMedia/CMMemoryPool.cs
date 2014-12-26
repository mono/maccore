// 
// CMMemoryPool.cs: Implements the managed CMMemoryPool
//
// Authors: Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2012 Xamarin Inc
//

using System;
using System.Runtime.InteropServices;

using MonoMac;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreMedia {

	[Since (6,0)]
	public class CMMemoryPool : CFType
	{
		static readonly IntPtr AgeOutPeriodSelector;
		
		static CMMemoryPool ()
		{
			var handle = Dlfcn.dlopen (Constants.CoreMediaLibrary, 0);
			try {
				AgeOutPeriodSelector = Dlfcn.GetIntPtr (handle, "kCMMemoryPoolOption_AgeOutPeriod");
			} finally {
				Dlfcn.dlclose (handle);
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMMemoryPoolCreate (IntPtr options);

		public CMMemoryPool ()
		{
			Handle = CMMemoryPoolCreate (IntPtr.Zero);
		}

#if !COREBUILD
		public CMMemoryPool (TimeSpan ageOutPeriod)
		{
			using (var dict = new NSMutableDictionary ()) {
				dict.LowlevelSetObject (AgeOutPeriodSelector, new NSNumber (ageOutPeriod.TotalSeconds).Handle);
				Handle = CMMemoryPoolCreate (dict.Handle);
			}
		}
#endif

		~CMMemoryPool ()
		{
			Dispose (false);
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMMemoryPoolGetAllocator (IntPtr pool);

		public CFAllocator GetAllocator ()
		{
			return new CFAllocator (CMMemoryPoolGetAllocator (Handle), false);
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static void CMMemoryPoolFlush (IntPtr pool);

		public void Flush ()
		{
			CMMemoryPoolFlush (Handle);
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static void CMMemoryPoolInvalidate (IntPtr pool);

		public void Invalidate ()
		{
			CMMemoryPoolInvalidate (Handle);
		}
	}
}
