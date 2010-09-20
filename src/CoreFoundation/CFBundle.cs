using System;
using System.Runtime.InteropServices;
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;

namespace MonoMac.CoreFoundation
{
	public class CFBundle : INativeObject, IDisposable
	{
		IntPtr handle;
		
		public CFBundle (IntPtr ptr)
		{
			handle = ptr;
			CFObject.CFRetain(handle);
		}
		
		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFBundleCopyResourceURL (IntPtr bundle, IntPtr resourceName, IntPtr resourceType, IntPtr subDirName);
		
		static public CFUrl CopyResourceURL (CFBundle bundle, string resourceName, string resourceType, string subDirName)
		{
			return new CFUrl (CFBundleCopyResourceURL (bundle.Handle, new NSString(resourceName).Handle, new NSString(resourceType).Handle, new NSString(subDirName).Handle));
		}
		
		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFBundleGetMainBundle();
		
		static public CFBundle MainBundle()
		{
			return new CFBundle(CFBundleGetMainBundle());
		}
		
		public IntPtr Handle {
			get {
				return handle;
			}
		}

        	public void Dispose ()
        	{
                	Dispose (true);
                	GC.SuppressFinalize (this);
        	}

		public void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero)
			{
				CFObject.CFRelease(handle);
				handle = IntPtr.Zero;
			}
		}
	}
}

