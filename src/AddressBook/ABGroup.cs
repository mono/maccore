// 
// ABGroup.cs: Implements the managed ABGroup
//
// Authors: Mono Team
//     
// Copyright (C) 2009 Novell, Inc
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using MonoMac.CoreFoundation;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using MonoMac.OpenGLES;

namespace MonoMac.AddressBook {

	static class ABGroupProperty {

		public static int Name {get; private set;}

		static ABGroupProperty ()
		{
			InitConstants.Init ();
		}

		internal static void Init ()
		{
			var handle = Dlfcn.dlopen (Constants.AddressBookLibrary, 0);
			if (handle == IntPtr.Zero)
				return;
			try {
				Name = Dlfcn.GetInt32 (handle, "kABGroupNameProperty");
			}
			finally {
				Dlfcn.dlclose (handle);
			}
		}
	}

	public class ABGroup : ABRecord, IEnumerable<ABRecord> {

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABGroupCreate ();

		public ABGroup ()
			: base (ABGroupCreate ())
		{
			InitConstants.Init ();
		}

		internal ABGroup (IntPtr handle)
			: base (CFObject.CFRetain (handle))
		{
		}

		public string Name {
			get {return PropertyToString (ABGroupProperty.Name);}
			set {SetValue (ABGroupProperty.Name, value);}
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static bool ABGroupAddMember (IntPtr group, IntPtr person, out IntPtr error);
		public void Add (ABRecord person)
		{
			if (person == null)
				throw new ArgumentNullException ("person");
			IntPtr error;
			if (!ABGroupAddMember (Handle, person.Handle, out error))
				throw CFException.FromCFError (error);
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABGroupCopyArrayOfAllMembers (IntPtr group);

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		public IEnumerator<ABRecord> GetEnumerator ()
		{
			var cfArrayRef = ABGroupCopyArrayOfAllMembers (Handle);
			IEnumerable<ABRecord> e = null;
			if (cfArrayRef == IntPtr.Zero)
				e = new ABRecord [0];
			else
				e = NSArray.ArrayFromHandle (cfArrayRef, h => ABRecord.FromHandle (h));
			return e.GetEnumerator ();
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABGroupCopyArrayOfAllMembersWithSortOrdering (IntPtr group, ABPersonSortBy sortOrdering);

		public ABRecord [] GetMembers (ABPersonSortBy sortOrdering)
		{
			var cfArrayRef = ABGroupCopyArrayOfAllMembersWithSortOrdering (Handle, sortOrdering);
			if (cfArrayRef == IntPtr.Zero)
				return new ABRecord [0];
			return NSArray.ArrayFromHandle (cfArrayRef, h => ABRecord.FromHandle (h));
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static bool ABGroupRemoveMember (IntPtr group, IntPtr member, out IntPtr error);
		public void Remove (ABRecord member)
		{
			if (member == null)
				throw new ArgumentNullException ("member");
			IntPtr error;
			if (!ABGroupRemoveMember (Handle, member.Handle, out error))
				throw CFException.FromCFError (error);
		}
	}
}

