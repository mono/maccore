//
// Authors:
//      James Clancey james.clancey@gmail.com>
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
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using MonoMac.ObjCRuntime;

namespace MonoMac.Foundation {
	
	public partial class NSIndexSet : IEnumerable, IEnumerable<uint> {

		IEnumerator IEnumerable.GetEnumerator ()
		{
			for (uint i = this.FirstIndex; i <= this.LastIndex;) {
				yield return i;
				i = this.IndexGreaterThan (i);
			}
		}

		public IEnumerator<uint> GetEnumerator ()
		{
			for (uint i = this.FirstIndex; i <= this.LastIndex;) {
				yield return i;
				i = this.IndexGreaterThan (i);
			}
		}
		
		public uint[] ToArray ()
		{
			uint [] indexes = new uint [Count];
			int j = 0;
			for (uint i = this.FirstIndex; i <= this.LastIndex;) {
				indexes [j++] = i;
				i = this.IndexGreaterThan (i);
			}
			return indexes;
		}
		public static NSIndexSet FromArray (uint[] items)
		{
			if (items == null)
				return new NSIndexSet ();
			
			var indexSet = new NSMutableIndexSet();
			foreach (var index in items) 
				indexSet.Add (index);	
			return indexSet;
		}

		public static NSIndexSet FromArray (int[] items)
		{
			if (items == null)
				return new NSIndexSet ();
			
			var indexSet = new NSMutableIndexSet();
			foreach (var index in items){
				if (index < 0)
					throw new ArgumentException ("One of the items values is negative");
				indexSet.Add ((uint) index);
			}
			return indexSet;
		}
		
		public NSIndexSet (int value) : this ((uint) value)
		{
			if (value < 0)
				throw new ArgumentException ("value must be positive");
			// init done by the base ctor
		}
	}
		
}
