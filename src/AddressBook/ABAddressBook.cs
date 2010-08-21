// 
// ABAddressBook.cs: Implements the managed ABAddressBook
//
// Authors: Mono Team
//     
// Copyright (C) 2009 Novell, Inc
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using MonoMac;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
using MonoMac.ObjCRuntime;
using MonoMac.OpenGLES;

namespace MonoMac.AddressBook {

	public enum ABAddressBookError {
		OperationNotPermittedByStore = 0
	}

	public class ExternalChangeEventArgs : EventArgs {
		public ExternalChangeEventArgs (ABAddressBook addressBook, NSDictionary info)
		{
			AddressBook = addressBook;
			Info        = info;
		}

		public ABAddressBook AddressBook {get; private set;}
		public NSDictionary Info {get; private set;}
	}

	// Quoth the docs: 
	//    http://developer.apple.com/iphone/library/documentation/AddressBook/Reference/ABPersonRef_iPhoneOS/Reference/reference.html#//apple_ref/c/func/ABPersonGetSortOrdering
	//
	//   "The value of these constants is undefined until one of the following has
	//    been called: ABAddressBookCreate, ABPersonCreate, ABGroupCreate."
	//
	// Meaning we can't rely on static constructors, as they could be invoked
	// before those functions have been invoked. :-/
	class InitConstants {
		public static void Init () {}

		static InitConstants ()
		{
			// ensure we can init.
			CFObject.CFRelease (ABAddressBook.ABAddressBookCreate ());

			ABGroupProperty.Init ();
			ABLabel.Init ();
			ABPersonAddressKey.Init ();
			ABPersonDateLabel.Init ();
			ABPersonInstantMessageKey.Init ();
			ABPersonInstantMessageService.Init ();
			ABPersonKindId.Init ();
			ABPersonPhoneLabel.Init ();
			ABPersonPropertyId.Init ();
			ABPersonRelatedNamesLabel.Init ();
			ABPersonUrlLabel.Init ();
		}
	}

	public class ABAddressBook : INativeObject, IDisposable, IEnumerable<ABRecord> {

		public static readonly NSString ErrorDomain;

		IntPtr handle;
		GCHandle sender;

		[DllImport (Constants.AddressBookLibrary)]
		internal extern static IntPtr ABAddressBookCreate ();
		public ABAddressBook ()
		{
			this.handle = ABAddressBookCreate ();

			InitConstants.Init ();
		}

		internal ABAddressBook (IntPtr handle)
		{
			this.handle = CFObject.CFRetain (handle);
		}

		static ABAddressBook ()
		{
			var handle = Dlfcn.dlopen (Constants.AddressBookLibrary, 0);
			if (handle == IntPtr.Zero)
				return;
			try {
				ErrorDomain = Dlfcn.GetStringConstant (handle, "ABAddressBookErrorDomain");
			}
			finally {
				Dlfcn.dlclose (handle);
			}
		}

		~ABAddressBook ()
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
			if (handle != IntPtr.Zero)
				CFObject.CFRelease (handle);
			if (sender.IsAllocated)
				sender.Free ();
			handle = IntPtr.Zero;
		}

		void AssertValid ()
		{
			if (handle == IntPtr.Zero)
				throw new ObjectDisposedException ("");
		}

		public IntPtr Handle {
			get {
				AssertValid ();
				return handle;
			}
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static bool ABAddressBookHasUnsavedChanges (IntPtr addressBook);
		public bool HasUnsavedChanges {
			get {
				AssertValid ();
				return ABAddressBookHasUnsavedChanges (Handle);
			}
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static bool ABAddressBookSave (IntPtr addressBook, out IntPtr error);
		public void Save ()
		{
			AssertValid ();
			IntPtr error;
			if (!ABAddressBookSave (Handle, out error))
				throw CFException.FromCFError (error);
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static void ABAddressBookRevert (IntPtr addressBook);
		public void Revert ()
		{
			AssertValid ();
			ABAddressBookRevert (Handle);
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static bool ABAddressBookAddRecord (IntPtr addressBook, IntPtr record, out IntPtr error);
		public void Add (ABRecord record)
		{
			AssertValid ();
			IntPtr error;
			if (!ABAddressBookAddRecord (Handle, record.Handle, out error))
				throw CFException.FromCFError (error);
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static bool ABAddressBookRemoveRecord (IntPtr addressBook, IntPtr record, out IntPtr error);
		public void Remove (ABRecord record)
		{
			AssertValid ();
			IntPtr error;
			if (!ABAddressBookRemoveRecord (Handle, record.Handle, out error))
				throw CFException.FromCFError (error);
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static int ABAddressBookGetPersonCount (IntPtr addressBook);
		public int PeopleCount {
			get {
				AssertValid ();
				return ABAddressBookGetPersonCount (Handle);
			}
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABAddressBookCopyArrayOfAllPeople (IntPtr addressBook);
		public ABPerson [] GetPeople ()
		{
			AssertValid ();
			IntPtr cfArrayRef = ABAddressBookCopyArrayOfAllPeople (Handle);
			return NSArray.ArrayFromHandle (cfArrayRef, h => new ABPerson (h));
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static int ABAddressBookGetGroupCount (IntPtr addressBook);
		public int GroupCount {
			get {
				AssertValid ();
				return ABAddressBookGetGroupCount (Handle);
			}
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABAddressBookCopyArrayOfAllGroups (IntPtr addressBook);
		public ABGroup [] GetGroups ()
		{
			AssertValid ();
			IntPtr cfArrayRef = ABAddressBookCopyArrayOfAllGroups (Handle);
			return NSArray.ArrayFromHandle (cfArrayRef, h => new ABGroup (h));
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABAddressBookCopyLocalizedLabel (IntPtr label);
		public static string LocalizedLabel (NSString label)
		{
			using (var s = new NSString (ABAddressBookCopyLocalizedLabel (label.Handle)))
				return s.ToString ();
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static void ABAddressBookRegisterExternalChangeCallback (IntPtr addressBook, ABExternalChangeCallback callback, IntPtr context);

		[DllImport (Constants.AddressBookLibrary)]
		extern static void ABAddressBookUnregisterExternalChangeCallback (IntPtr addressBook, ABExternalChangeCallback callback, IntPtr context);

		delegate void ABExternalChangeCallback (IntPtr addressBook, IntPtr info, IntPtr context);

		[MonoPInvokeCallback (typeof (ABExternalChangeCallback))]
		static void ExternalChangeCallback (IntPtr addressBook, IntPtr info, IntPtr context)
		{
			GCHandle s = GCHandle.FromIntPtr (context);
			var self = s.Target as ABAddressBook;
			if (self == null)
				return;
			self.OnExternalChange (new ExternalChangeEventArgs (
						new ABAddressBook (addressBook),
						(NSDictionary) Runtime.GetNSObject (info)));
		}

		object eventLock = new object ();

		EventHandler<ExternalChangeEventArgs> externalChange;

		protected virtual void OnExternalChange (ExternalChangeEventArgs e)
		{
			AssertValid ();
			EventHandler<ExternalChangeEventArgs> h = externalChange;
			if (h != null)
				h (this, e);
		}

		public event EventHandler<ExternalChangeEventArgs> ExternalChange {
			add {
				lock (eventLock) {
					if (externalChange == null) {
						sender = GCHandle.Alloc (this);
						ABAddressBookRegisterExternalChangeCallback (Handle, ExternalChangeCallback, GCHandle.ToIntPtr (sender));
					}
					externalChange += value;
				}
			}
			remove {
				lock (eventLock) {
					externalChange -= value;
					if (externalChange == null) {
						ABAddressBookUnregisterExternalChangeCallback (Handle, ExternalChangeCallback, GCHandle.ToIntPtr (sender));
						sender.Free ();
					}
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		public IEnumerator<ABRecord> GetEnumerator ()
		{
			AssertValid ();
			foreach (var p in GetPeople ())
				yield return p;
			foreach (var g in GetGroups ())
				yield return g;
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABAddressBookGetGroupWithRecordID (IntPtr addressBook, int recordId);
		public ABGroup GetGroup (int recordId)
		{
			var h = ABAddressBookGetGroupWithRecordID (Handle, recordId);
			if (h == IntPtr.Zero)
				return null;
			return new ABGroup (h);
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABAddressBookGetPersonWithRecordID (IntPtr addressBook, int recordId);
		public ABPerson GetPerson (int recordId)
		{
			var h = ABAddressBookGetPersonWithRecordID (Handle, recordId);
			if (h == IntPtr.Zero)
				return null;
			return new ABPerson (h);
		}

		[DllImport (Constants.AddressBookLibrary)]
		extern static IntPtr ABAddressBookCopyPeopleWithName (IntPtr addressBook, IntPtr name);
		public ABPerson [] GetPeopleWithName (string name)
		{
			IntPtr cfArrayRef;
			using (var s = new NSString (name))
				cfArrayRef = ABAddressBookCopyPeopleWithName (Handle, s.Handle);
			return NSArray.ArrayFromHandle (cfArrayRef, h => new ABPerson (h));
		}
	}
}

