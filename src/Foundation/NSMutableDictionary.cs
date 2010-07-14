using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoMac.Foundation {

	public partial class NSMutableDictionary : IDictionary, IDictionary<NSObject, NSObject> {

		public static NSMutableDictionary FromObjectsAndKeys (NSObject [] objects, NSObject [] keys)
		{
			if (objects.Length != keys.Length)
				throw new ArgumentException ("objects and keys arrays have different sizes");
			return FromObjectsAndKeys (objects, keys, objects.Length);
		}
		
		#region ICollection<KeyValuePair<NSObject, NSObject>>
		void ICollection<KeyValuePair<NSObject, NSObject>>.Add (KeyValuePair<NSObject, NSObject> item)
		{
			SetObject (item.Value, item.Key);
		}

		public void Clear ()
		{
			RemoveAllObjects ();
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
			var count = Count;
			RemoveObjectForKey (keyValuePair.Key);
			return count != Count;
		}

		int ICollection<KeyValuePair<NSObject, NSObject>>.Count {
			get {return (int) Count;}
		}

		bool ICollection<KeyValuePair<NSObject, NSObject>>.IsReadOnly {
			get {return false;}
		}
		#endregion

		#region IDictionary

		void IDictionary.Add (object key, object value)
		{
			var nsokey = key as NSObject;
			var nsovalue = value as NSObject;
			
			if (nsokey == null || nsovalue == null)
				throw new ArgumentException ("You can only use NSObjects for keys and values in an NSMutableDictionary");

			// Inverted args
			SetObject (nsovalue, nsokey);
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
		class ShimEnumerator : IDictionaryEnumerator, IEnumerator {
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
			if (key == null)
				throw new ArgumentNullException ("key");
			var nskey = key as NSObject;
			if (nskey == null)
				throw new ArgumentException ("The key must be an NSObject");
			
			RemoveObjectForKey (nskey);
		}

		bool IDictionary.IsFixedSize {
			get {return false;}
		}

		bool IDictionary.IsReadOnly {
			get {return false;}
		}

		object IDictionary.this [object key] {
			get {
				NSObject _key = key as NSObject;
				if (_key == null)
					return null;
				return ObjectForKey (_key);
			}
			set {
				var nsokey = key as NSObject;
				var nsovalue = value as NSObject;

				if (nsokey == null || nsovalue == null)
					throw new ArgumentException ("You can only use NSObjects for keys and values in an NSMutableDictionary");
				
				SetObject (nsovalue, nsokey);
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

		public void Add (NSObject key, NSObject value)
		{
			// Inverted args.
			SetObject (value, key);
		}

		static readonly NSObject marker = new NSObject ();

		public bool ContainsKey (NSObject key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			var keys   = NSArray.FromNSObjects (new [] {key});
			var values = ObjectsForKeys (keys, marker);
			return object.ReferenceEquals (marker, values [0]);
		}

		public bool Remove (NSObject key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			var last = Count;
			RemoveObjectForKey (key);
			return last != Count;
		}

		public bool TryGetValue (NSObject key, out NSObject value)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			var keys   = NSArray.FromNSObjects (new [] {key});
			var values = ObjectsForKeys (keys, marker);
			if (object.ReferenceEquals (marker, values [0])) {
				value = null;
				return false;
			}
			value = values [0];
			return true;
		}

		public override NSObject this [NSObject key] {
			get {
				if (key == null)
					throw new ArgumentNullException ("key");
				return ObjectForKey (key);
			}
			set {
				SetObject (value, key);
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
	}
}
