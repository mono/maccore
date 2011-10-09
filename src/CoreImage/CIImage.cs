//
// CIImage.cs: Extensions
//
// Copyright 2011 Xamarin Inc.
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
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
#if !MONOMAC
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
#endif

namespace MonoMac.CoreImage {
	public class CIAutoAdjustmentFilterOptions {

		// The default value is true.
		public bool? Enhance;

		// The default value is true
		public bool? RedEye;

		public CIFeature [] Features;

		public CIImageOrientation? ImageOrientation;
		
		internal NSDictionary ToDictionary ()
		{
			int n = 0;
			if (Enhance.HasValue && Enhance.Value == false)
				n++;
			if (RedEye.HasValue && RedEye.Value == false)
				n++;
			if (ImageOrientation.HasValue)
				n++;
			if (Features != null && Features.Length != 0)
				n++;
			if (n == 0)
				return null;
			var keys = new NSString [n];
			var values = new NSObject [n];

			int i = 0;
			if (Enhance.HasValue && Enhance.Value == false){
				keys [i] = CIImage.AutoAdjustEnhanceKey;
				values [i] = CFBoolean.FalseObject;
				i++;
			}
			if (RedEye.HasValue && RedEye.Value == false){
				keys [i] = CIImage.AutoAdjustRedEyeKey;
				values [i] = CFBoolean.FalseObject;
				i++;
			}
			if (Features != null && Features.Length != 0){
				keys [i] = CIImage.AutoAdjustFeaturesKey;
				values [i] = NSArray.FromObjects (Features);
				i++;
			}
			if (ImageOrientation.HasValue){
				keys [i] = CIImage.ImagePropertyOrientation;
				values [i] = new NSNumber ((int)ImageOrientation.Value);
				i++;
			}
			for (i = 0; i < n; i++){
				Console.WriteLine ("{0} {1}-{2}", i, keys [i], values [i]);
			}
			
			return NSDictionary.FromObjectsAndKeys (values, keys);
		}
	}
	
	public partial class CIImage {

		static CIFilter [] WrapFilters (NSArray filters)
		{
			if (filters == null)
				return new CIFilter [0];

			uint count = filters.Count;
			if (count == 0)
				return new CIFilter [0];
			var ret = new CIFilter [count];
			for (uint i = 0; i < count; i++){
				IntPtr filterHandle = filters.ValueAt (i);
				string filterName = CIFilter.GetFilterName (filterHandle);
									 
				ret [i] = CIFilter.FromName (filterName, filterHandle);
			}
			return ret;
		}
		
		public CIFilter [] GetAutoAdjustmentFilters ()
		{
			return WrapFilters (_GetAutoAdjustmentFilters ());
		}
		
		public CIFilter [] GetAutoAdjustmentFilters (CIAutoAdjustmentFilterOptions options)
		{
			if (options == null)
				return GetAutoAdjustmentFilters ();
			var dict = options.ToDictionary ();
			if (dict == null)
				return GetAutoAdjustmentFilters ();
			return WrapFilters (_GetAutoAdjustmentFilters (dict));
		}
	}
}