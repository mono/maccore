//
// CLLocationDistance.cs: Type wrapper for CLLocationDistance
//
// Authors:
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012, Xamarin, Inc
//
// The class can be either constructed from a string (from user code)
// or from a handle (from iphone-sharp.dll internal calls).  This
// delays the creation of the actual managed string until actually
// required
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
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreLocation {

	public struct CLLocationDistance
	{
		public double Distance;

		public CLLocationDistance (double distance)
		{
			this.Distance = distance;
		}

		static double? max_distance;
		[Since (6, 0)]
		public static CLLocationDistance MaxDistance {
			get {
				if (max_distance == null)
					max_distance = GetConstant ("CLLocationDistanceMax");
				return new CLLocationDistance (max_distance.Value); 
			}
		}

		static double? filter_none;
		public static CLLocationDistance FilterNone {
			get {
				if (filter_none == null)
					filter_none = GetConstant ("kCLDistanceFilterNone");
				return new CLLocationDistance (filter_none.Value);
			}
		}

		public static implicit operator CLLocationDistance (double distance)
		{
			return new CLLocationDistance (distance);
		}

		public static implicit operator double (CLLocationDistance distance)
		{
			return distance.Distance;
		}

		static double GetConstant (string constantName)
		{ 
			var handle = Dlfcn.dlopen (Constants.CoreLocationLibrary, 0);

			try {
				return Dlfcn.GetDouble (handle, constantName);
			} finally {
				Dlfcn.dlclose (handle);
			}
		}
	}
}