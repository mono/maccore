//
// Authors: Jeffrey Stedfast
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
using System.IO;

namespace MonoTouch.ObjCRuntime {
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple=true)]
	public class LinkWithAttribute : Attribute {
		public LinkWithAttribute (string libName, string linkerFlags)
		{
			LinkerFlags = linkerFlags;
			LibraryName = libName;
		}
		
		public LinkWithAttribute (string libName)
		{
			LibraryName = libName;
		}
		
		public string LibraryName {
			get; private set;
		}
		
		public string LinkerFlags {
			get; set;
		}
		
		public bool IsArmV6 { get; set; }
		public bool IsArmV7 { get; set; }
		public bool IsArmV7Thumb { get; set; }
		public bool IsX86 { get; set; }
		
		public string ResourceName {
			get {
				string name, suffix;
				int dot;
				
				name = Path.GetFileName (LibraryName);
				
				if ((dot = name.LastIndexOf ('.')) != -1) {
					suffix = name.Substring (dot);
					name = name.Substring (0, dot);
				} else {
					suffix = String.Empty;
				}
				
				return string.Format ("{0}{1}{2}{3}{4}{5}", name, IsArmV6 ? "-armv6" : "",
						      IsArmV7 ? "-armv7" : "", IsArmV7Thumb ? "-thumb" : "",
						      IsX86 ? "-x86" : "", suffix);
			}
		}
	}
}
