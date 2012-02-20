//
// MonoTouch defines for docfixer
//
using System;
using System.IO;
using System.Reflection;
using MonoTouch.Foundation;

public partial class DocGenerator {
	static Assembly assembly = typeof (MonoTouch.Foundation.NSObject).Assembly;
	const string BaseNamespace = "MonoTouch";

	static string GetMostRecentDocBase ()
	{
		//var versions = new[]{"4_0", "3_2", "3_1"};
		//string format = "/Developer/Platforms/iPhoneOS.platform/Developer/Documentation/DocSets/com.apple.adc.documentation.AppleiPhone{0}.iPhoneLibrary.docset/Contents/Resources/Documents/documentation";
		var versions = new [] { "5_0" };
		var base_paths = new [] {
			"/Library/Developer/Shared/Documentation/DocSets/com.apple.adc.documentation.AppleiOS{0}.iOSLibrary.docset/Contents/Resources/Documents/documentation",
			"/Developer/Platforms/iPhoneOS.platform/Developer/Documentation/DocSets/com.apple.adc.documentation.AppleiOS{0}.iOSLibrary.docset/Contents/Resources/Documents/documentation",
		};

		foreach (var p in base_paths){
			foreach (var v in versions) {
				var d = string.Format (p, v);
				if (Directory.Exists (d)) {
					return d;
					break;
				}
			}
		}
		return null;
	}

	public static string GetSelector (object attr)
	{
		return ((ExportAttribute) attr).Selector;
	}
}