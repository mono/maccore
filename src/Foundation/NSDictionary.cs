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
using System;
using System.Collections;
using System.Collections.Generic;
using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {

	public partial class NSDictionary : IDictionary, IDictionary<NSObject, NSObject> {
		public static NSDictionary FromObjectsAndKeys (NSObject [] objects, NSObject [] keys)
		{
			if (objects.Length != keys.Length)
				throw new ArgumentException ("objects and keys arrays have different sizes");
			
			var no = NSArray.FromNSObjects (objects);
			var nk = NSArray.FromNSObjects (keys);
			var r = FromObjectsAndKeysInternal (no, nk);
			no.Dispose ();
			nk.Dispose ();
			return r;
		}

		public static NSDictionary FromObjectsAndKeys (object [] objects, object [] keys)
		{
			if (objects.Length != keys.Length)
				throw new ArgumentException ("objects and keys arrays have different sizes");
			
			var no = NSArray.FromObjects (objects);
			var nk = NSArray.FromObjects (keys);
			var r = FromObjectsAndKeysInternal (no, nk);
			no.Dispose ();
			nk.Dispose ();
			return r;
		}

		public static NSDictionary FromObjectsAndKeys (NSObject [] objects, NSObject [] keys, int count)
		{
			if (objects.Length != keys.Length)
				throw new ArgumentException ("objects and keys arrays have different sizes");
			if (count < 1 || objects.Length < count || keys.Length < count)
				throw new ArgumentException ("count");
			
			var no = NSArray.FromNSObjects (objects);
			var nk = NSArray.FromNSObjects (keys);
			var r = FromObjectsAndKeysInternal (no, nk);
			no.Dispose ();
			nk.Dispose ();
			return r;
		}

		public static NSDictionary FromObjectsAndKeys (object [] objects, object [] keys, int count)
		{
			if (objects.Length != keys.Length)
				throw new ArgumentException ("objects and keys arrays have different sizes");
			if (count < 1 || objects.Length < count || keys.Length < count)
				throw new ArgumentException ("count");
			
			var no = NSArray.FromObjects (objects);
			var nk = NSArray.FromObjects (keys);
			var r = FromObjectsAndKeysInternal (no, nk);
			no.Dispose ();
			nk.Dispose ();
			return r;
		}
		
		internal bool ContainsKeyValuePair (KeyValuePair<NSObject, NSObject> pair)
		{
			NSObject value;
			if (!TryGetValue (pair.Key, out value))
				return false;

			return EqualityComparer<NSObject>.Default.Equals (pair.Value, value);
		}

		#region ICollection
		void ICollection.CopyTo (Array array, int arrayIndex)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			if (arrayIndex < 0)
				throw new ArgumentOutOfRangeException ("arrayIndex");
			if (array.Rank > 1)
				throw new ArgumentException ("array is multidimensional");
			if ((array.Length > 0) && (arrayIndex >= array.Length))
				throw new ArgumentException ("arrayIndex is equal to or greater than array.Length");
			if (arrayIndex + Count > array.Length)
				throw new ArgumentException ("Not enough room from arrayIndex to end of array for this Hashtable");
			IDictionaryEnumerator e = ((IDictionary) this).GetEnumerator ();
			int i = arrayIndex;
			while (e.MoveNext ())
				array.SetValue (e.Entry, i++);
		}

		int ICollection.Count {
			get {return (int) Count;}
		}

		bool ICollection.IsSynchronized {
			get {return false;}
		}

		object ICollection.SyncRoot {
			get {return this;}
		}
		#endregion

		#region ICollection<KeyValuePair<NSObject, NSObject>>
		void ICollection<KeyValuePair<NSObject, NSObject>>.Add (KeyValuePair<NSObject, NSObject> item)
		{
			throw new NotSupportedException ();
		}

		void ICollection<KeyValuePair<NSObject, NSObject>>.Clear ()
		{
			throw new NotSupportedException ();
		}

		bool ICollection<KeyValuePair<NSObject, NSObject>>.Contains (KeyValuePair<NSObject, NSObject> keyValuePair)
		{
			return ContainsKeyValuePair (keyValuePair);
		}

		void ICollection<KeyValuePair<NSObject, NSObject>>.CopyTo (KeyValuePair<NSObject, NSObject>[] array, int index)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			if (index < 0)
				throw new ArgumentOutOfRangeException ("index");
			// we want no exception for index==array.Length && Count == 0
			if (index > array.Length)
				throw new ArgumentException ("index larger than largest valid index of array");
			if (array.Length - index < Count)
				throw new ArgumentException ("Destination array cannot hold the requested elements!");

			var e = GetEnumerator ();
			while (e.MoveNext ())
				array [index++] = e.Current;
		}

		bool ICollection<KeyValuePair<NSObject, NSObject>>.Remove (KeyValuePair<NSObject, NSObject> keyValuePair)
		{
			throw new NotSupportedException ();
		}

		int ICollection<KeyValuePair<NSObject, NSObject>>.Count {
			get {return (int) Count;}
		}

		bool ICollection<KeyValuePair<NSObject, NSObject>>.IsReadOnly {
			get {return true;}
		}
		#endregion

		#region IDictionary

		void IDictionary.Add (object key, object value)
		{
			throw new NotSupportedException ();
		}

		void IDictionary.Clear ()
		{
			throw new NotSupportedException ();
		}

		bool IDictionary.Contains (object key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			NSObject _key = key as NSObject;
			if (_key == null)
				return false;
			return ContainsKey (_key);
		}

		IDictionaryEnumerator IDictionary.GetEnumerator ()
		{
			return new ShimEnumerator (this);
		}

		[Serializable]
		class ShimEnumerator : IDictionaryEnumerator, IDisposable, IEnumerator {
			IEnumerator<KeyValuePair<NSObject, NSObject>> e;

			public ShimEnumerator (NSDictionary host)
			{
				e = host.GetEnumerator ();
			}

			public void Dispose ()
			{
				e.Dispose ();
			}

			public bool MoveNext ()
			{
				return e.MoveNext ();
			}

			public DictionaryEntry Entry {
				get { return new DictionaryEntry { Key = e.Current.Key, Value = e.Current.Value }; }
			}

			public object Key {
				get { return e.Current.Key; }
			}

			public object Value {
				get { return e.Current.Value; }
			}

			public object Current {
				get {return Entry;}
			}

			public void Reset ()
			{
				e.Reset ();
			}
		}

		void IDictionary.Remove (object key)
		{
			throw new NotSupportedException ();
		}

		bool IDictionary.IsFixedSize {
			get {return true;}
		}

		bool IDictionary.IsReadOnly {
			get {return true;}
		}

		object IDictionary.this [object key] {
			get {
				NSObject _key = key as NSObject;
				if (_key == null)
					return null;
				return ObjectForKey (_key);
			}
			set {
				throw new NotSupportedException ();
			}
		}

		ICollection IDictionary.Keys {
			get {return Keys;}
		}

		ICollection IDictionary.Values {
			get {return Values;}
		}

		#endregion

		#region IDictionary<NSObject, NSObject>

		void IDictionary<NSObject, NSObject>.Add (NSObject key, NSObject value)
		{
			throw new NotSupportedException ();
		}

		static readonly NSObject marker = new NSObject ();

		public bool ContainsKey (NSObject key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			return ObjectForKey (key) != null;
		}

		bool IDictionary<NSObject, NSObject>.Remove (NSObject key)
		{
			throw new NotSupportedException ();
		}

		public bool TryGetValue (NSObject key, out NSObject value)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			value = ObjectForKey (key);
			// NSDictionary can not contain NULLs, if you want a NULL, it exists as an NSNulln
			if (value == null)
				return false;
			return true;
		}

		public virtual NSObject this [NSObject key] {
			get {
				if (key == null)
					throw new ArgumentNullException ("key");
				return ObjectForKey (key);
			}
			set {
				throw new NotSupportedException ();
			}
		}

		ICollection<NSObject> IDictionary<NSObject, NSObject>.Keys {
			get {return Keys;}
		}

		ICollection<NSObject> IDictionary<NSObject, NSObject>.Values {
			get {return Values;}
		}

		#endregion

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		public IEnumerator<KeyValuePair<NSObject, NSObject>> GetEnumerator ()
		{
			foreach (var key in Keys) {
				yield return new KeyValuePair<NSObject, NSObject> (key, ObjectForKey (key));
			}
		}

		public IntPtr LowlevelObjectForKey (IntPtr key)
		{
			return MonoMac.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, selObjectForKey_, key);
		}
	}
}
