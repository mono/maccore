// 
// CMSync.cs: Implements the managed CMSync infrastructure
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

	public enum CMClockError
	{
		None = 0,
		MissingRequiredParameter	= -12745,
		InvalidParameter			= -12746,
		AllocationFailed			= -12747,
		UnsupportedOperation		= -12756,
	}

	[Since (6,0)]
	public class CMClock : CMClockOrTimebase
	{
		internal CMClock (IntPtr handle)
			: base (handle)
		{
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMClockGetHostTimeClock ();

		public static CMClock HostTimeClock {
			get {
				return new CMClock (CMClockGetHostTimeClock ());
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMClockGetTime (IntPtr clock);

		public CMTime CurrentTime {
			get {
				return CMClockGetTime (Handle);
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMClockError CMClockGetAnchorTime (IntPtr clock, out CMTime outClockTime, out CMTime outReferenceClockTime);

		public CMClockError GetAnchorTime (out CMTime clockTime, out CMTime referenceClockTime)
		{
			return CMClockGetAnchorTime (Handle, out clockTime, out referenceClockTime);
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static bool CMClockMightDrift (IntPtr clock, IntPtr otherClock);

		public bool MightDrift (CMClock otherClock)
		{
			if (otherClock == null)
				throw new ArgumentNullException ("otherClock");

			return CMClockMightDrift (Handle, otherClock.Handle);
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static void CMClockInvalidate (IntPtr clock);

		public void Invalidate ()
		{
			CMClockInvalidate (Handle);
		}

		[DllImport(Constants.CoreMediaLibrary, EntryPoint="CMClockConvertHostTimeToSystemUnits")]
		public extern static ulong ConvertHostTimeToSystemUnits (CMTime hostTime);

		[DllImport(Constants.CoreMediaLibrary, EntryPoint="CMClockMakeHostTimeFromSystemUnits")]
		public extern static CMTime CreateHostTimeFromSystemUnits (ulong hostTime);
	}

	public enum CMTimebaseError
	{
		None = 0,
		MissingRequiredParameter	= -12748,
		InvalidParameter			= -12749,
		AllocationFailed			= -12750,
		TimerIntervalTooShort		= -12751,
		ReadOnly					= -12757,
	}

	[Since (6,0)]
	public class CMTimebase : CMClockOrTimebase
	{
		private CMTimebase (IntPtr handle)
			: base (handle)
		{
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTimebaseError CMTimebaseCreateWithMasterClock (IntPtr allocator, IntPtr masterClock, out IntPtr timebaseOut);

		public CMTimebase (CMClock masterClock)
		{
			if (masterClock == null)
				throw new ArgumentNullException ("masterClock");

			var error = CMTimebaseCreateWithMasterClock (IntPtr.Zero, masterClock.Handle, out handle);
			if (error != CMTimebaseError.None)
				throw new ArgumentException (error.ToString ());

			CFObject.CFRetain (Handle);
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTimebaseError CMTimebaseCreateWithMasterTimebase (IntPtr allocator, IntPtr masterTimebase, out IntPtr timebaseOut);

		public CMTimebase (CMTimebase masterTimebase)
		{
			if (masterTimebase == null)
				throw new ArgumentNullException ("masterTimebase");

			var error = CMTimebaseCreateWithMasterTimebase (IntPtr.Zero, masterTimebase.Handle, out handle);
			if (error != CMTimebaseError.None)
				throw new ArgumentException (error.ToString ());

			CFObject.CFRetain (Handle);
		}


		[DllImport(Constants.CoreMediaLibrary)]
		extern static double CMTimebaseGetEffectiveRate (IntPtr timebase);

		public double EffectiveRate {
			get {
				return CMTimebaseGetEffectiveRate (Handle);
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static double CMTimebaseGetRate (IntPtr timebase);
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTimebaseError CMTimebaseSetRate (IntPtr timebase, double rate);

		public double Rate {
			get {
				return CMTimebaseGetRate (Handle);
			}
			set {
				var error = CMTimebaseSetRate (Handle, value);
				if (error != CMTimebaseError.None)
					throw new ArgumentException (error.ToString ());				
			}
		} 

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimebaseGetTime (IntPtr timebase);
		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTimebaseError CMTimebaseSetTime (IntPtr timebase, CMTime time);

		public new CMTime Time {
			get {
				return CMTimebaseGetTime (Handle);
			}
			set {
				var error = CMTimebaseSetTime (Handle, value);
				if (error != CMTimebaseError.None)
					throw new ArgumentException (error.ToString ());
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMTimebaseGetMasterTimebase (IntPtr timebase);

		public CMTimebase GetMasterTimebase ()
		{
			var ptr = CMTimebaseGetMasterTimebase (Handle);
			if (ptr == IntPtr.Zero)
				return null;

			return new CMTimebase (ptr);			
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMTimebaseGetMasterClock (IntPtr timebase);

		public CMClock GetMasterClock ()
		{
			var ptr = CMTimebaseGetMasterClock (Handle);
			if (ptr == IntPtr.Zero)
				return null;

			return new CMClock (ptr);
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMTimebaseGetMaster (IntPtr timebase);

		public CMClockOrTimebase GetMaster ()
		{
			var ptr = CMTimebaseGetMaster (Handle);
			if (ptr == IntPtr.Zero)
				return null;

			return new CMClockOrTimebase (ptr);
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static IntPtr CMTimebaseGetUltimateMasterClock (IntPtr timebase);

		public CMClock GetUltimateMasterClock ()
		{
			var ptr  = CMTimebaseGetUltimateMasterClock (Handle);
			if (ptr == IntPtr.Zero)
				return null;

			return new CMClock (ptr);
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMTimebaseGetTimeWithTimeScale (IntPtr timebase, CMTimeScale timescale, CMTimeRoundingMethod method);

		public CMTime GetTime (CMTimeScale timeScale, CMTimeRoundingMethod roundingMethod)
		{
			return CMTimebaseGetTimeWithTimeScale (Handle, timeScale, roundingMethod);
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTimebaseError CMTimebaseSetAnchorTime (IntPtr timebase, CMTime timebaseTime, CMTime immediateMasterTime);

		public CMTimebaseError SetAnchorTime (CMTime timebaseTime, CMTime immediateMasterTime)
		{
			return CMTimebaseSetAnchorTime (Handle, timebaseTime, immediateMasterTime);
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTimebaseError CMTimebaseGetTimeAndRate (IntPtr timebase, out CMTime time, out double rate);

		public CMTimebaseError GetTimeAndRate (out CMTime time, out double rate)
		{
			return CMTimebaseGetTimeAndRate (Handle, out time, out rate);
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTimebaseError CMTimebaseSetRateAndAnchorTime (IntPtr timebase, double rate, CMTime timebaseTime, CMTime immediateMasterTime);

		public CMTimebaseError SetRateAndAnchorTime (double rate, CMTime timebaseTime, CMTime immediateMasterTime)
		{
			return CMTimebaseSetRateAndAnchorTime (Handle, rate, timebaseTime, immediateMasterTime);
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTimebaseError CMTimebaseNotificationBarrier (IntPtr timebase);

		public CMTimebaseError NotificationBarrier ()
		{
			return CMTimebaseNotificationBarrier (handle);
		}
	}

	public enum CMSyncError {
		None = 0,
		MissingRequiredParameter	= -12752,
		InvalidParameter			= -12753,
		AllocationFailed			= -12754,
		RateMustBeNonZero			= -12755,
	}

	public class CMClockOrTimebase : IDisposable, INativeObject
	{
		internal IntPtr handle;

		internal CMClockOrTimebase ()
		{
		}

		internal CMClockOrTimebase (IntPtr handle)
		{
			this.handle = handle;
			CFObject.CFRetain (Handle);
		}

		~CMClockOrTimebase ()
		{
			Dispose (false);
		}
		
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected virtual void Dispose (bool disposing)
		{
			if (Handle != IntPtr.Zero){
				CFObject.CFRelease (Handle);
				handle = IntPtr.Zero;
			}
		}

		public IntPtr Handle { 
			get {
				return handle;
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMSyncGetTime (IntPtr clockOrTimebase);

		public CMTime Time { 
			get {
				return CMSyncGetTime (handle);
			}
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static double CMSyncGetRelativeRate (IntPtr ofClockOrTimebase, IntPtr relativeToClockOrTimebase);

		public static double GetRelativeRate (CMClockOrTimebase clockOrTimebaseA, CMClockOrTimebase clockOrTimebaseB)
		{
			if (clockOrTimebaseA == null)
				throw new ArgumentNullException ("clockOrTimebaseA");

			if (clockOrTimebaseB == null)
				throw new ArgumentNullException ("clockOrTimebaseB");

			return CMSyncGetRelativeRate (clockOrTimebaseA.Handle, clockOrTimebaseB.Handle);
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMSyncError CMSyncGetRelativeRateAndAnchorTime (IntPtr ofClockOrTimebase, IntPtr relativeToClockOrTimebase,
				out double outRelativeRate, out CMTime outOfClockOrTimebaseAnchorTime, out CMTime outRelativeToClockOrTimebaseAnchorTime);

		public static CMSyncError GetRelativeRateAndAnchorTime (CMClockOrTimebase clockOrTimebaseA, CMClockOrTimebase clockOrTimebaseB, out double relativeRate, out CMTime timeA, out CMTime timeB)
		{
			if (clockOrTimebaseA == null)
				throw new ArgumentNullException ("clockOrTimebaseA");

			if (clockOrTimebaseB == null)
				throw new ArgumentNullException ("clockOrTimebaseB");

			return CMSyncGetRelativeRateAndAnchorTime (clockOrTimebaseA.Handle, clockOrTimebaseB.handle, out relativeRate, out timeA, out timeB);
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static CMTime CMSyncConvertTime (CMTime time, IntPtr fromClockOrTimebase, IntPtr toClockOrTimebase);

		public static CMTime ConvertTime (CMTime time, CMClockOrTimebase from, CMClockOrTimebase to)
		{
			if (from == null)
				throw new ArgumentNullException ("from");
			if (to == null)
				throw new ArgumentNullException ("to");

			return CMSyncConvertTime (time, from.Handle, to.Handle);
		}

		[DllImport(Constants.CoreMediaLibrary)]
		extern static bool CMSyncMightDrift (IntPtr clockOrTimebase1, IntPtr clockOrTimebase2);

		public static bool MightDrift (CMClockOrTimebase clockOrTimebaseA, CMClockOrTimebase clockOrTimebaseB)
		{
			if (clockOrTimebaseA == null)
				throw new ArgumentNullException ("clockOrTimebaseA");

			if (clockOrTimebaseB == null)
				throw new ArgumentNullException ("clockOrTimebaseB");

			return CMSyncMightDrift (clockOrTimebaseA.Handle, clockOrTimebaseB.Handle);
		}
	}
}
