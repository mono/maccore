//
// docfixer
//
// TODO:
//   Remove <h2...> Overview</h2> from merged docs
//   
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Xml.XPath;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;

using HtmlAgilityPack;

public partial class DocGenerator {
	static string DocBase = GetMostRecentDocBase ();

	//
	// {0} is the DocBase
	// {1} is the framework name (Cocoa, GraphicsImaging, etc) without the MonoTouch/MonoMac prefix.
	// {2} is the TypeName
	// {3} is the first alternate name (for example fx=CoreAnimation sets alt=QuartzCore)
	// {4} is the second alternate name
	// {5} is the typename with 2 letters removed from the front (UIView -> View)
	static string [] patterns = {
		"{0}/{1}/Reference/{2}_Class/{2}/{2}.html",
		"{0}/{1}/Reference/{2}Request_Class/Reference/Reference.html",
		"{0}/{1}/Reference/{2}_Class/{2}ClassReference/{2}ClassReference.html",
		"{0}/{1}/Reference/{2}_Class/Reference/Reference.html",
		"{0}/{1}/Reference/{2}_Class/Reference/{5}.html",
		"{0}/{1}/Reference/{2}_Class/Reference/{2}.html",                           // UIKit/Reference/UIDevice_Class/Reference/UIDevice.html
		"{0}/{1}/Reference/{2}_Class/Reference.html",
		"{0}/{1}/Reference/{2}_ClassRef/Reference/{2}.html",
		"{0}/{1}/Reference/{2}_ClassRef/Reference/Reference.html",
		"{0}/{3}/Reference/{2}_ClassRef/Reference/Reference.html",
		"{0}/{1}/Reference/{2}ClassRef/Reference/Reference.html",
		"{0}/{3}/Reference/{2}ClassRef/Reference/Reference.html",
		"{0}/{1}/Reference/{2}_Protocol/Introduction/Introduction.html",
		"{0}/{1}/Reference/{2}_Protocol/Reference/Reference.html",
		"{0}/{1}/Reference/{2}_Protocol/Reference/{2}.html",
		"{0}/{1}/Reference/{2}_Protocol/{2}/{2}.html",
		"{0}/{1}/Reference/{4}_Protocol/{2}/{2}.html",                              // UIModalViewDelegate_Protocol/UIActionSheetDelegate/UIActionSheetDelegate.html
		"{0}/{1}/Reference/Foundation/Classes/{2}_Class/{2}.html",
		"{0}/{1}/Reference/Foundation/Classes/{2}_Class/Reference/{2}.html",
		"{0}/{1}/Reference/Foundation/Classes/{2}_Class/Reference/Reference.html",
		"{0}/{1}/Reference/{2}_Class/Introduction/Introduction.html",
		"{0}/{4}/Reference/{2}_Class/Introduction/Introduction.html",
		"{0}/{3}/Reference/{2}_Class/{2}/{2}.html",
		"{0}/{3}/Reference/{2}_Class/Reference/Reference.html",
		"{0}/{1}/Reference/{2}_Class/Reference/Reference.html",
		"{0}/{3}/Reference/{2}_Class/{4}/{4}.html",
		"{0}/{3}/Reference/{2}_ClassRef/Reference/Reference.html",
		"{0}/{4}/Reference/{2}ClassRef/Reference/Reference.html",
		"{0}/{1}/Reference/{2}Ref/Reference/Reference.html",
		"{0}/{4}/Reference/{2}Ref/Reference/Reference.html",
		"{0}/{1}/Reference/{2}ClassRef/{2}ClassReference/{2}ClassReference.html",
		"{0}/{1}/Reference/{2}_ClassReference/Reference/Reference.html",
		"{0}/{1}/Reference/{2}_Reference/Reference/Reference.html",                 // SKProduct_Reference/Reference/Reference.html
		"{0}/{1}/Reference/{2}/Reference/Reference.html",                           // SKProductsRequest/Reference/Reference.html
		"{0}/{3}/Reference/{2}/Reference/Reference.html",                           // NetworkingInternet/Reference/CTCall/Reference/Reference.html
		"{0}/{4}/Reference/{2}/Reference/Reference.html",

		"{0}/{1}/Reference/{2}_Ref/Reference/Reference.html",                       // GameKit/Reference/GKLeaderboard_Ref/Reference/Reference.html
		

		"{0}/{1}/Reference/{2}ClassReference/Reference/Reference.html",
		"{0}/{1}/Reference/{2}ProtocolReference/Reference/Reference.html",
		"{0}/{1}/Reference/{2}_ProtocolReference/Reference/Reference.html",
		"{0}/{1}/Reference/{2}_Protocol/Reference/Reference.html",
		"{0}/{3}/Reference/{2}_Protocol_iPhoneOS/Reference/Reference.html",
		"{0}/{3}/Reference/{2}_Protocol/Reference/Reference.html",
		"{0}/{1}/Reference/{4}_Protocol/{1}/{1}.html",
		"{0}/iPhone/Reference/{2}_Class/{2}.html",
		"{0}/iPhone/Reference/{2}_Class/Reference/Reference.html", 		// UILocalReference

		// AppKit-style
		"{0}/{1}/Reference/{3}/Classes/{2}_Class/Reference/Reference.html",
		"{0}/{1}/Reference/{3}/Classes/{2}_Class/Reference/{2}.html",
		// Audit these two: is it index.html?
		//"{0}/{1}/Reference/{2}ProtocolReference/index.html"
	};

	static Type export_attribute_type;
	
	public static string GetAppleDocFor (Type t)
	{
		string fx = t.Namespace;
		string alt = "", alt2 ="";

		if (fx.StartsWith (BaseNamespace))
			fx = fx.Substring (BaseNamespace.Length+1);

		if (fx == "Foundation")
			fx = "Cocoa";
		if (fx == "CoreAnimation"){
			fx = "GraphicsImaging";
			alt = "QuartzCore";
			alt2 = "Cocoa";
		}
		if (fx == "QuickLook"){
			alt = "NetworkingInternet";
		}
		if (fx == "EventKit"){
			alt = "DataManagement";
			alt2 = "EventKitUI";
		}
		if (fx == "CoreTelephony"){
			alt = "NetworkingInternet";
		}
		if (fx == "iAd")
			fx = "UserExperience";
		if (t.Name == "CAEAGLLayer")
			alt2 = "CAEGLLayer";
		if (t.Name == "UIActionSheetDelegate")
			alt2 = "UIModalViewDelegate";
		if (fx == "AppKit"){
			fx = "Cocoa";
			alt = "ApplicationKit";
		}
		if (fx == "CoreImage"){
			fx = "GraphicsImaging";
			alt = "QuartzCoreFramework";
		}
		if (fx == "QTKit"){
			fx = "QuickTime";
			alt = "QTKitFramework";
		}
		if (fx == "WebKit"){
			fx = "Cocoa";
			alt = "WebKit";
		}
		
		foreach (string pattern in patterns){
			string path = String.Format (pattern, DocBase, fx, t.Name, alt, alt2, t.Name.Substring (2));
			if (File.Exists (path))
				return path;
		}

		// Ignore known missing documents
		switch (t.Name){
		case "CAAnimationDelegate":
		case "UITableViewSource":
			// This one was invented by us.
			return null;
		case "CLHeading":
			// This seems to be an internal Apple API, not documeted, but in the header files.
			return null;
		// public "delegate"/protocol types w/o explicit documentation
		case "CALayerDelegate":
		case "NSKeyedArchiverDelegate":
		case "NSKeyedUnarchiverDelegate":
		case "NSUrlConnectionDelegate":
			return null;
		// renames of ObjC types
		case "NSNetServiceDelegate":        // ObjC NSNetServiceDelegateMethods [no docs]
		case "NSNetServiceBrowserDelegate": // ObjC NSNetServiceBrowserDelegateMethods [no docs]
			return null;
		case "UIPickerViewModel":
			// This was invented by us.
			return null;
		}

		Console.WriteLine ("NOT FOUND: {0}", t);
#if true
	    	Console.WriteLine ("DocBase: {0}", DocBase);
		foreach (string pattern in patterns){
			string path = String.Format (pattern, "", fx, t.Name, alt, alt2, t.Name.Substring (2));
			Console.WriteLine ("    Tried: {0}", path);
		}
#endif
		return null;
	}

	public static bool KnownIssues (string type, string selector)
	{
		// generator always generates this constructor from the NSCoding protocol
		if (selector == "initWithCoder:")
			return true;
		switch (type){
		case "ABUnknownPersonViewControllerDelegate":
			switch (selector) {
				// Documented online, but not in the SDK docs.
				case "unknownPersonViewController:shouldPerformDefaultActionForPerson:property:identifier:":
					return true;
			}
			break;
		case "AVAudioSession":
			switch (selector) {
				// Documented online, but not in the SDK docs.
				case "delegate":
					return true;
			}
			break;
		case "CALayer":
			if (CAMediaTiming_Selector (selector))
				return true;
			switch (selector) {
			// renamed property getter; TODO: make it grab the 'doubleSided' property docs
			case "isDoubleSided":
			// renamed property getter; TODO: make it grab the 'geometryFlipped' property docs
			case "isGeometryFlipped":
			// renamed property getter; TODO: make it grab the 'hidden' property docs
			case "isHidden":
			// renamed property getter; TODO: make it grab the 'opaque' property docs
			case "isOpaque":
				return true;
			}
			break;
		case "CAPropertyAnimation":
			switch (selector) {
			case "isAdditive":    // TODO: additive property
			case "isCumulative":  // TODO: cumulative property
				return true;
			}
			break;
		case "MPMoviePlayerController":
			switch (selector) {
				// Documentation online, but not locally
				case "play":
				case "stop":
					return true;
			}
			break;
		case "NSBundle":
			switch (selector) {
			// Extension method from UIKit
			case "loadNibNamed:owner:options:":

			// Extension methods from AppKit 
			case "pathForImageResource:":
			case "pathForSoundResource:":
				return true;
			}
			break;
		case "NSIndexPath":
			// Documented in a separate file, .../NSIndexPath_UIKitAdditions/Reference/Reference.html
			switch (selector) {
			case "indexPathForRow:inSection:":
			case "row":
			case "section":
				return true;
			}
			break;
		case "UIDevice":
			// see TODO wrt Property handling.
			if (selector == "isGeneratingDeviceOrientationNotifications")
				return true;
			break;
		case "NSMutableUrlRequest":
			switch (selector) {
			// NSMutableUrlRequest provides setURL, but URL is provided from the
			// base NSUrlRequest method.  This is a "wart" in our binding to make
			// for nicer code.
			case "URL":
			case "cachePolicy":
			case "timeoutInterval":
			case "mainDocumentURL":
			case "HTTPMethod":
			case "allHTTPHeaderFields":
			case "HTTPBody":
			case "HTTPBodyStream":
			case "HTTPShouldHandleCookies":
				return true;
			}
			break;
		case "NSUrlConnection":
			switch (selector) {
			// NSURLConnection adopts the NSUrlAuthenticationChallengeSender, but
			// docs don't mention it
			case "useCredential:forAuthenticationChallenge:":
			case "continueWithoutCredentialForAuthenticationChallenge:":
			case "cancelAuthenticationChallenge:":
				return true;
			}
			break;
		case "NSUserDefaults":
			switch (selector) {
			// Documentation online, but not locally
			case "doubleForKey:":
			case "setDouble:forKey:":
				return true;
			}
			break;
		case "NSValue":
			switch (selector) {
			// extension methods from various places...
			case "valueWithCGPoint:":
			case "valueWithCGRect:":
			case "valueWithCGSize:":
			case "valueWithCGAffineTransform:":
			case "valueWithUIEdgeInsets:":
			case "CGPointValue":
			case "CGRectValue":
			case "CGSizeValue":
			case "CGAffineTransformValue":
			case "UIEdgeInsetsValue":
				return true;
			}
			break;
		case "SKPaymentQueue":
			switch (selector) {
			// Documented online, but not locally
			case "restoreCompletedTransactions":
				return true;
			}
			break;
		case "SKPaymentTransaction":
			switch (selector) {
			// Documented online, but not locally
			case "originalTransaction":
			case "transactionDate":
				return true;
			}
			break;
		case "SKPaymentTransactionObserver":
			switch (selector) {
			// Documented online, but not locally
			case "paymentQueue:restoreCompletedTransactionsFailedWithError:":
			case "paymentQueueRestoreCompletedTransactionsFinished:":
				return true;
			}
			break;
		case "UIApplication":
			switch (selector) {
			// deprecated in iPhoneOS 3.2
			case "setStatusBarHidden:animated:":
				return true;
			}
			break;
		case "UITableViewDelegate":
			// This is documented on a separate HTML file, deprecated file
			if (selector == "tableView:accessoryTypeForRowWithIndexPath:")
				return true;
			break;

		case "UISearchBar":
			// Online, but not local
			if (selector == "isTranslucent")
				return true;
			break;
		case "UISearchBarDelegate":
			// This was added to the 3.0 API, but was not documented.
			if (selector == "searchBar:shouldChangeTextInRange:replacementText:")
				return true;
			break;
		case "UITextField":
		case "UITextView":
			// These are from the UITextInputTraits protocol
			switch (selector) {
			case "autocapitalizationType":
			case "autocorrectionType":
			case "keyboardType":
			case "keyboardAppearance":
			case "returnKeyType":
			case "enablesReturnKeyAutomatically":
			case "isSecureTextEntry":
				return true;
			}
			break;
		case "UIImagePickerController":
			switch (selector) {
			// present online, but not locally
			case "allowsEditing":
			case "cameraOverlayView":
			case "cameraViewTransform":
			case "showsCameraControls":
			case "takePicture":
			case "videoMaximumDuration":
			case "videoQuality":
				return true;
			}
			break;
		case "UIImagePickerControllerDelegate":
			// Deprecated, but available
			if (selector == "imagePickerController:didFinishPickingImage:editingInfo:")
				return true;
			// Deprecated in iPhone 3.0
			if (selector == "imagePickerController:didFinishPickingMediaWithInfo:")
				return true;
			break;
		case "UIViewController":
			switch (selector) {
			// present online, but not locally
			case "searchDisplayController":
				return true;
			}
			break;

		case "CLLocation":
			switch (selector) {
			// deprecated and moved into CLLocation_Class/DeprecationAppendix/AppendixADeprecatedAPI.html
			case "getDistanceFrom:":
				return true;
			}
			break;
		case "CLLocationManagerDelegate":
			// Added in 3.0, Not yet documented by apple
			if (selector == "locationManager:didUpdateHeading:")
				return true;

			// Added in 3.0, Not yet documented by apple
			if (selector == "locationManagerShouldDisplayHeadingCalibration:")
				return true;
			break;
			
		case "CLLocationManager":
			// Added in 3.0, but not documented 
			if (selector == "startUpdatingHeading" || selector == "stopUpdatingHeading" || selector == "dismissHeadingCalibrationDisplay")
				return true;
			// present online, but not on local disk?
			if (selector == "headingFilter" || selector == "headingAvailable")
				return true;
			break;
			
		case "CAAnimation":
			// Not in the docs.
			if (selector == "willChangeValueForKey:" || selector == "didChangeValueForKey:")
				return true;
			if (CAMediaTiming_Selector (selector))
				return true;
			break;

		case "NSObject":
			// Defined in the NSObject_UIKitAdditions/Introduction/Introduction.html instead of NSObject.html
			if (selector == "awakeFromNib")
				return true;
			// Kind of a hack; NSObject doesn't officially implement NSCoding, but
			// most types do, and it's a NOP on NSObject, so it's easier for
			// MonoTouch users if MonoTouch.Foundation.NSObject provides the method.
			if (selector == "encodeWithCoder:")
				return true;
			break;
		}
		return false;
	}

	static bool CAMediaTiming_Selector (string selector)
	{
		switch (selector) {
			case "autoreverses":
			case "beginTime":
			case "duration":
			case "fillMode":
			case "repeatCount":
			case "repeatDuration":
			case "speed":
			case "timeOffset":
				return true;
		}
		return false;
	}

	static bool KnownMissingReturnValue (Type type, string selector)
	{
		switch (type.Name) {
		case "AVAudioSession":
			switch (selector) {
			case "sharedInstance":
				return true;
			}
			break;
		case "GKPeerPickerControllerDelegate":
			switch (selector) {
			case "peerPickerController:sessionForConnectionType:":
				return true;
			}
			break;
		case "MPMediaItemArtwork":
			switch (selector) {
			case "imageWithSize:":
				return true;
			}
			break;
		case "NSCoder":
			switch (selector) {
			case "containsValueForKey:":
			case "decodeBoolForKey:":
			case "decodeDoubleForKey:":
			case "decodeFloatForKey:":
			case "decodeInt32ForKey:":
			case "decodeInt64ForKey:":
			case "decodeObject":
			case "decodeObjectForKey:":
				return true;
			}
			break;
		case "NSDecimalNumber":
			switch (selector) {
			case "decimalNumberByAdding:withBehavior:":
			case "decimalNumberBySubtracting:withBehavior:":
			case "decimalNumberByMultiplyingBy:withBehavior:":
			case "decimalNumberByDividingBy:withBehavior:":
			case "decimalNumberByRaisingToPower:withBehavior:":
			case "decimalNumberByMultiplyingByPowerOf10:":
			case "decimalNumberByMultiplyingByPowerOf10:withBehavior:":
			case "decimalNumberByRoundingAccordingToBehavior:":
				return true;
			}
			break;
		case "NSDictionary":
			switch (selector) {
			case "objectsForKeys:notFoundMarker:":
				return true;
			}
			break;
		case "NSNotification":
			switch (selector) {
			case "notificationWithName:object:":
			case "notificationWithName:object:userInfo:":
				return true;
			}
			break;
		case "NSUrlCredential":
			switch (selector) {
			case "credentialForTrust:":
				return true;
			}
			break;
			
		case "UIResponder":
			switch (selector) {
			case "resignFirstResponder":
				return true;
			}
			break;
			
		case "UIFont":
			switch (selector){
			case "leading":
				// This one was deprecated
				return true;
			}
			break;
			
		case "UIImagePickerController":
			switch (selector){
			case "allowsImageEditing":
				// This one was deprecated.
				return true;
			}
			break;
			
		}
		return false;
	}

	public static void ReportProblem (Type t, string docpath, string selector, string key)
	{
		Console.WriteLine (t);
		Console.WriteLine ("    Error: did not find selector \"{0}\"", selector);
		Console.WriteLine ("     File: {0}", docpath);
		Console.WriteLine ("      key: {0}", key);
		Console.WriteLine ();
	}

	static Dictionary<string, Func<XElement, bool, object>> HtmlToMdocElementMapping = new Dictionary<string, Func<XElement, bool, object>> {
		{ "section",(e, i) => new [] {new XElement ("para", HtmlToMdoc ((XElement)e.FirstNode))}.Concat (HtmlToMdoc (e.Nodes ().Skip (1), i)) },
		{ "a",      (e, i) => ConvertLink (e, i) },
		{ "code",   (e, i) => ConvertCode (e, i) },
		{ "div",    (e, i) => HtmlToMdoc (e.Nodes (), i) },
		{ "em",     (e, i) => new XElement ("i", HtmlToMdoc (e.Nodes (), i)) },
		{ "li",     (e, i) => new XElement ("item", new XElement ("term", HtmlToMdoc (e.Nodes (), i))) },
		{ "ol",     (e, i) => new XElement ("list", new XAttribute ("type", "number"), HtmlToMdoc (e.Nodes ())) },
		{ "p",      (e, i) => new XElement ("para", HtmlToMdoc (e.Nodes (), i)) },
		{ "span",   (e, i) => HtmlToMdoc (e.Nodes (), i) },
		{ "strong", (e, i) => new XElement ("i", HtmlToMdoc (e.Nodes (), i)) },
		{ "tt",     (e, i) => new XElement ("c", HtmlToMdoc (e.Nodes (), i)) },
		{ "ul",     (e, i) => new XElement ("list", new XAttribute ("type", "bullet"), HtmlToMdoc (e.Nodes ())) },
	};

	static Regex selectorInHref = new Regex ("(?<type>[^/]+)/(?<selector>[^/]+)$");

	static object ConvertLink (XElement e, bool insideFormat)
	{
		var href = e.Attribute ("href");
		if (href == null){
			return "";
		}
		var m = selectorInHref.Match (href.Value);
		if (!m.Success)
			return "";

		var selType   = m.Groups ["type"].Value;
		var selector  = m.Groups ["selector"].Value;

		var type = assembly.GetTypes ().Where (t => t.Name == selType).FirstOrDefault ();
		if (type != null) {
			var typedocpath = GetMdocPath (type);
			if (File.Exists (typedocpath)) {
				XDocument typedocs;
				using (var f = File.OpenText (typedocpath))
					typedocs = XDocument.Load (f);
				var member = GetMdocMember (typedocs, selector);
				if (member != null)
					return new XElement ("see",
							new XAttribute ("cref", CreateCref (typedocs, member)));
			}
		}
		if (!href.Value.StartsWith ("#")){
			var r = Path.GetFullPath (Path.Combine (Path.GetDirectoryName (appledocpath), href.Value));

			href.Value = r.Replace (DocBase, "http://developer.apple.com/iphone/library/documentation");
		}
		
		return insideFormat
			? e
			: new XElement ("format",
					new XAttribute ("type", "text/html"), e);
	}

	static string CreateCref (XDocument typedocs, XElement member)
	{
		var cref = new StringBuilder ();
		var memberType = member.Element ("MemberType").Value;
		switch (memberType) {
			case "Constructor": cref.Append ("C"); break;
			case "Event":       cref.Append ("E"); break;
			case "Field":       cref.Append ("F"); break;
			case "Method":      cref.Append ("M"); break;
			case "Property":    cref.Append ("P"); break;
			default:
				throw new InvalidOperationException (string.Format ("Unsupported member type '{0}' for member {1}.{2}.",
							memberType,
							typedocs.Root.Attribute ("FullName").Value, 
							member.Attribute("MemberName").Value));
		}
		cref.Append (":");
		cref.Append (typedocs.Root.Attribute ("FullName").Value);
		if (memberType != "Constructor") {
			cref.Append (".");
			cref.Append (member.Attribute ("MemberName").Value.Replace (".", "#"));
		}

		var p = member.Element ("Parameters");
		if (p != null && p.Descendants ().Any ()) {
			cref.Append ("(");
			bool first = true;
			var ps = p.Descendants ();
			foreach (var pi in ps) {
				cref.AppendFormat ("{0}{1}", first ? "" : ",", pi.Attribute ("Type").Value);
				first = false;
			}
			cref.Append (")");
		}

		return cref.ToString ();
	}

	static XElement ConvertCode (XElement e, bool insideFormat)
	{
		if (e.Value == "YES")
			return new XElement ("see", new XAttribute ("langword", "true"));
		if (e.Value == "NO")
			return new XElement ("see", new XAttribute ("langword", "false"));
		if (e.Value == "nil")
			return new XElement ("see", new XAttribute ("langword", "null"));
		return new XElement ("c", HtmlToMdoc (e.Nodes (), insideFormat));
	}

	static IEnumerable<object> HtmlToMdoc (IEnumerable<XNode> rest)
	{
		return HtmlToMdoc (rest, false);
	}

	static IEnumerable<object> HtmlToMdoc (IEnumerable<XNode> rest, bool insideFormat)
	{
		foreach (var e in rest)
			yield return HtmlToMdoc (e, insideFormat);
	}

	static object HtmlToMdoc (XElement e)
	{
		return HtmlToMdoc (e, false);
	}

	static object HtmlToMdoc (XNode n, bool insideFormat)
	{
#if false
		// the "cheap" "just wrap everything in <format/> w/o attempting to
		// translate" approach.  It works...but e.g. embedded <a/> links to
		// members won't be converted into <see cref="..."/> links.
		if (!insideFormat)
			return new XElement ("format",
					new XAttribute ("type", "text/html"),
					n);
		return n;
#else
		// Try to intelligently convert HTML into mdoc(5).
		object r = null;
		var e = n as XElement;
		if (e != null && HtmlToMdocElementMapping.ContainsKey (e.Name.LocalName))
			r = HtmlToMdocElementMapping [e.Name.LocalName] (e, insideFormat);
		else if (e != null && !insideFormat)
			r = new XElement ("format",
					new XAttribute ("type", "text/html"),
					HtmlToMdoc (e, true));
		else if (e != null)
			r = new XElement (e.Name,
					e.Attributes (),
					HtmlToMdoc (e.Nodes (), insideFormat));
		else
			r = n;
		return r;
#endif
	}

	static object HtmlToMdoc (XElement e, IEnumerable<XElement> rest)
	{
		return HtmlToMdoc (new[]{e}.Concat (rest).Cast<XNode> (), false);
	}

	class XElementDocumentOrderComparer : IComparer<XElement>
	{
		public static readonly IComparer<XElement> Default = new XElementDocumentOrderComparer ();

		public int Compare (XElement a, XElement b)
		{
			if (object.ReferenceEquals (a, b))
				return 0;
			if (a.IsBefore (b))
				return -1;
			return 1;
		}
	}

	static XElement FirstInDocument (params XElement[] elements)
	{
		IEnumerable<XElement> e = elements;
		return FirstInDocument (e);
	}

	static XElement FirstInDocument (IEnumerable<XElement> elements)
	{
		return elements
			.Where (e => e != null)
			.OrderBy (e => e, XElementDocumentOrderComparer.Default)
			.FirstOrDefault ();
	}

	public static object ExtractTypeOverview (XElement appledocs)
	{
		var overview  = appledocs.Descendants("h2").Where(e => e.Value == "Overview").FirstOrDefault();
		if (overview == null)
			return null;

		var end = FirstInDocument (
				GetDocSections (appledocs)
				.Concat (new[]{
					overview.ElementsAfterSelf ().Descendants ("hr").FirstOrDefault ()
				}));
		if (end == null)
			return null;

		return HtmlToMdoc (
				overview,
				overview.ElementsAfterSelf().Where(e => e.IsBefore(end))
		);
	}

	static IEnumerable<XElement> GetDocSections (XElement appledocs)
	{
		foreach (var e in appledocs.Descendants ("h2")) {
			if (e.Value == "Class Methods" ||
					e.Value == "Instance Methods" ||
					e.Value == "Properties")
				yield return e;
		}
	}
	
	public static object ExtractSummary (XElement member)
	{
		try {
			return HtmlToMdoc (member.ElementsAfterSelf ("p").First ());
		} catch {
			return null;
		}
	}

	public static object ExtractSection (XElement member)
	{
		try {
			return HtmlToMdoc (member.ElementsAfterSelf ("section").First ());
		} catch {
			return null;
		}
	}
	
	static XElement GetMemberDocEnd (XElement member)
	{
		return FirstInDocument (
				member.ElementsAfterSelf ("h3").FirstOrDefault (),
				member.ElementsAfterSelf ("hr").FirstOrDefault ());
	}

	static IEnumerable<XElement> ExtractSection (XElement member, string section)
	{
		return from x in member.ElementsAfterSelf ("div")
			let h5 = x.Descendants ("h5").FirstOrDefault (e => e.Value == section)
			where h5 != null
			from j in h5.ElementsAfterSelf () select j;

#if false
		var docEnd = GetMemberDocEnd (member);
		var start = member.ElementsAfterSelf ("h5").FirstOrDefault (e => e.Value == section) ??
			member.ElementsAfterSelf ("div")
				.SelectMany (e => e.Descendants ("h5"))
				.FirstOrDefault (e => e.Value == section);
		if (start == null || (docEnd != null && start.IsAfter (docEnd)))
			return null;
		var end = start.ElementsAfterSelf ("h5").FirstOrDefault () ??
			start.ElementsAfterSelf ("div").SelectMany (e => e.Descendants ("h5")).FirstOrDefault () ??
			docEnd;
		return end != null 
			? start.ElementsAfterSelf ().Where (e => e.IsBefore (end))
			: start.ElementsAfterSelf ();
#endif
	}

	public static IEnumerable<object> ExtractParams (XElement member)
	{
		var param = ExtractSection (member, "Parameters");
		if (param == null || !param.Any ()){
			return new object[0];
		}

		return param.Elements ("dd").Select (d => HtmlToMdoc (d));
	}

	public static object ExtractReturn (XElement member)
	{
		var e = ExtractSection (member, "Return Value");
		if (e == null)
			return null;
		return HtmlToMdoc (e.Cast<XNode> ());
	}

	public static object ExtractDiscussion (XElement member)
	{
		var discussion = from x in member.ElementsAfterSelf ("div")
			let h5 = x.Descendants ("h5").FirstOrDefault (e => e.Value == "Discussion")
			where h5 != null
			from j in h5.ElementsAfterSelf () select j;

		return HtmlToMdoc (discussion.Cast<XNode> ());
	}

	static string GetMdocPath (Type t)
	{
		return string.Format ("{0}/{1}/{2}.xml", assembly_dir, t.Namespace, t.Name);
	}

	static XElement GetMdocMember (XDocument mdoc, string selector)
	{
		var exportAttr = BaseNamespace + ".Foundation.Export(\"" + selector + "\"";
		return
			(from m in mdoc.XPathSelectElements ("Type/Members/Member")
				where m.Descendants ("Attributes").Descendants ("Attribute").Descendants ("AttributeName")
					.Where (n => n.Value.StartsWith (exportAttr) || 
						n.Value.StartsWith ("get: " + exportAttr) || 
						n.Value.StartsWith ("set: " + exportAttr)).Any ()
				select m
			).FirstOrDefault ();
	}

	static string appledocpath;
	
	public static void ProcessNSO (Type t)
	{
		appledocpath = GetAppleDocFor (t);
		if (appledocpath == null)
			return;

		string xmldocpath = GetMdocPath (t);
		if (!File.Exists (xmldocpath)) {
			Console.WriteLine ("DOC REGEN PENDING for type: {0}", t.FullName);
			return;
		}

		XDocument xmldoc;
		using (var f = File.OpenText (xmldocpath))
			xmldoc = XDocument.Load (f);

		//Console.WriteLine ("Opened {0}", appledocpath);
		var appledocs = LoadAppleDocumentation (appledocpath);

		
		var typeRemarks = xmldoc.Element ("Type").Element ("Docs").Element ("remarks");
		var typeSummary = xmldoc.Element ("Type").Element ("Docs").Element ("summary");
		
		if (typeRemarks != null || (quick_summaries && typeSummary != null)) {
			if (typeRemarks.Value == "To be added.")
				typeRemarks.Value = "";
			var overview = ExtractTypeOverview (appledocs);
			typeRemarks.Add (overview);
			if (overview != null && quick_summaries){ // && typeSummary.Value == "To be added."){
				foreach (var x in (System.Collections.IEnumerable) overview){
					var xe = x as XElement;
					if (xe == null)
						continue;

					if (xe.Name == "para"){
						var value = xe.Value;
						var dot = value.IndexOf ('.');
						if (dot == -1)
							typeSummary.Value = value;
						else
							typeSummary.Value = value.Substring (0, dot+1);
						break;
					}
				}
			}
		}

		var flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
		foreach (var method in 
				t.GetMethods (flags).Cast<MethodBase> ()
				.Concat(t.GetConstructors (flags).Cast<MethodBase> ())) {
			bool prop = false;
			if (method.IsSpecialName)
				prop = true;

			// Skip methods from the base class
			if (method.DeclaringType != t)
				continue;

			var attrs = method.GetCustomAttributes (export_attribute_type, true);
			if (attrs.Length == 0)
				continue;
			var selector = GetSelector (attrs [0]);

			bool overrides = 
				(method.Attributes & MethodAttributes.Virtual) != 0 &&
				(method.Attributes & MethodAttributes.NewSlot) == 0;

			string keyFormat = "<h3 class=\"verytight\">{0}</h3>";
			string key = string.Format (keyFormat, selector);

			var mDoc = GetAppleMemberDocs (t, selector);
			//Console.WriteLine ("{0}", selector);
			if (mDoc == null){
				// Don't report known issues
				if (!KnownIssues (t.Name, selector) && 
						// don't report property setters
						!(prop && method.Name.StartsWith ("set_")) &&
						// don't report overriding methods
						!overrides)
					ReportProblem (t, appledocpath, selector, key);
				continue;
			}
			//Console.WriteLine ("Contents at {0}", p);
			var summary = ExtractSummary (mDoc);
			if (summary == null)
				ReportProblem (t, appledocpath, selector, key);

			//
			// Now, plug the docs
			//
			var member = GetMdocMember (xmldoc, selector);
			if (member == null){
				Console.WriteLine ("DOC REGEN PENDING for {0}.{1}", method.DeclaringType.Name, selector);
				continue;
			}

			//
			// Summary
			//
			var summaryNode = member.XPathSelectElement ("Docs/summary");
			summaryNode.Value = "";
			summaryNode.Add (summary);

			VerifyArgumentSemantic (mDoc, member, t, selector);

			//
			// Wipe out the value if it says "to be added"
			//
			var valueNode = member.XPathSelectElement ("Docs/value");
			if (valueNode != null){
				if (valueNode.Value == "To be added.")
					valueNode.Value = "";
			}
			
			//
			// Merge parameters
			//
			var eParamNodes = member.XPathSelectElements ("Docs/param").GetEnumerator ();
			//Console.WriteLine ("{0}", selector);
			var eAppleParams= ExtractParams (mDoc).GetEnumerator ();
			for ( ; eParamNodes.MoveNext () && eAppleParams.MoveNext (); ) {
				eParamNodes.Current.Value = "";
				eParamNodes.Current.Add (eAppleParams.Current);
			}

			//
			// Only extract the return value if there is a return in the type
			//
			var return_type = member.XPathSelectElement ("ReturnValue/ReturnType");
			if (return_type != null && return_type.Value != "System.Void" && member.XPathSelectElement ("MemberType").Value == "Method") {
				//Console.WriteLine ("Scanning for return {0} {1}", t.FullName, selector);
				var ret = ExtractReturn (mDoc);
				if (ret == null && !KnownMissingReturnValue (t, selector))
					Console.WriteLine ("Problem extracting a return value for type=\"{0}\" selector=\"{1}\"", t.FullName, selector);
				else {
					var retNode = prop
						? member.XPathSelectElement ("Docs/value")
						: member.XPathSelectElement ("Docs/returns");
					if (retNode != null && ret != null){
						retNode.Value = "";
						retNode.Add (ret);
					}
				}
			}

			var remarks = ExtractDiscussion (mDoc);
			
			if (remarks != null){
				var remarksNode = member.XPathSelectElement ("Docs/remarks");
				if (remarksNode.Value == "To be added.")
					remarksNode.Value = "";
				remarksNode.Add (remarks);
			}
		}

		var s = new XmlWriterSettings ();
		s.Indent = true;
		s.Encoding = new UTF8Encoding (false);
		s.OmitXmlDeclaration = true;
		using (var output = File.CreateText (xmldocpath)){
			var xmlw = XmlWriter.Create (output, s);
			xmldoc.Save (xmlw);
			output.WriteLine ();
		}
	}

	static Regex propertyAttrs = new Regex (@"^@property\((?<attrs>[^)]*)\)");

	static void VerifyArgumentSemantic (XElement mDoc, XElement member, Type t, string selector)
	{
		// ArgumentSemantic validation
		XElement code;
		var codeDeclaration = mDoc.ElementsAfterSelf ("pre").FirstOrDefault ();
		if (codeDeclaration == null || codeDeclaration.Attribute ("class") == null ||
				codeDeclaration.Attribute ("class").Value != "declaration" ||
				(code = codeDeclaration.Elements ("code").FirstOrDefault ()) == null)
			return;
		var decl = code.Value;

		var m = propertyAttrs.Match (decl);
		string attrs;
		if (!m.Success || string.IsNullOrEmpty (attrs = m.Groups ["attrs"].Value))
			return;

		string semantic = null;

		if (attrs.Contains ("assign"))
			semantic = "ArgumentSemantic.Assign";
		else if (attrs.Contains ("copy"))
			semantic = "ArgumentSemantic.Copy";
		else if (attrs.Contains ("retain"))
			semantic = "ArgumentSemantic.Retain";

		if (semantic != null &&
				!member.XPathSelectElements ("Attributes/Attribute/AttributeName").Any (a => a.Value.Contains (semantic))) {
			Console.WriteLine ("Missing [Export (\"{0}\", {1})] on Type={2} Member='{3}'", selector, semantic, t.FullName, 
					member.XPathSelectElement ("MemberSignature[@Language='C#']").Attribute ("Value").Value);
		}
	}

	public static XElement GetAppleMemberDocs(Type t, string selector)
	{
		foreach (var appledocs in GetAppleDocumentationSources (t)) {
			var mDoc = appledocs.Descendants ("h3").Where (e => e.Value == selector).FirstOrDefault ();
			if (mDoc == null) {
				// Many read-only properties have an 'is' prefix on the selector
				// (which is removed on the docs), so try w/o the prefix, e.g. 
				//   @property(getter=isDoubleSided) BOOL doubleSided;
				var newSelector = char.ToLower (selector [2]) + selector.Substring (3);
				mDoc = appledocs.Descendants ("h3").Where (e => e.Value == newSelector).FirstOrDefault ();
			}

			if (mDoc != null)
				return mDoc;
		}
		return null;
	}

	public static IEnumerable<XElement> GetAppleDocumentationSources (Type t)
	{
		var path = GetAppleDocFor (t);
		if (path != null)
			yield return LoadAppleDocumentation (path);
		while ((t = t.BaseType) != typeof (object) && t != null) {
			path = GetAppleDocFor (t);
			if (path != null)
				yield return LoadAppleDocumentation (path);
		}
	}

	static Dictionary<string, XElement> loadedAppleDocs = new Dictionary<string, XElement> ();

	public static XElement LoadAppleDocumentation (string path)
	{
		XElement appledocs;
		if (loadedAppleDocs.TryGetValue (path, out appledocs))
			return appledocs;

		var doc = new HtmlDocument();
		doc.Load (path, Encoding.UTF8);
		doc.OptionOutputAsXml = true;
		var sw = new StringWriter ();
		doc.Save (sw);

		//doc.Save ("/tmp/foo-" + Path.GetFileName (path));
		
		// minor global fixups
		var contents = sw.ToString ()
			.Replace ("&amp;#160;",   " ")
			.Replace ("&amp;#8211;",  "-")
			.Replace ("&amp;#xA0;",   " ")
			.Replace ("&amp;nbsp;",   " ");

		// HtmlDocument wraps the <html/> with a <span/>; skip the <span/>.
		appledocs = XElement.Parse (contents).Elements().First();

		// remove the xmlns element from everything...
		foreach (var e in appledocs.DescendantsAndSelf ()) {
			e.Name = XName.Get (e.Name.LocalName);
		}
		loadedAppleDocs [path] = appledocs;
		return appledocs;
	}

	static string assembly_dir;

	// If true, it extracts the first sentence from the remarks and sticks it in the summary.
	static bool quick_summaries;

	static void Help ()
	{
		Console.WriteLine ("Usage is: docfixer [--summary] path-to-files");
	}
	
	public static int Main (string [] args)
	{
		string dir = null;
		
		for (int i = 0; i < args.Length; i++){
			var arg = args [i];
			if (arg == "-h" || arg == "--help"){
				Help ();
				return 0;
			}
			if (arg == "--summary"){
				quick_summaries = true;
				continue;
			}
			dir = arg;
		}
		
		if (dir == null){
			Help ();
			return 1;
		}

		var debug = Environment.GetEnvironmentVariable ("DOCFIXER");

		if (File.Exists (Path.Combine (dir, "en"))){
			Console.WriteLine ("The directory does not seem to be the root for documentation (missing `en' directory)");
			return 1;
		}
		assembly_dir = Path.Combine (dir, "en");

		Type nso = assembly.GetType (BaseNamespace + ".Foundation.NSObject");
		export_attribute_type = assembly.GetType (BaseNamespace + ".Foundation.ExportAttribute");

		if (nso == null || export_attribute_type == null){
			Console.WriteLine ("Incomplete {0} assembly", BaseNamespace);
			return 1;
		}

		foreach (Type t in assembly.GetTypes ()){
			if (t.IsNotPublic || t.IsNested)
				continue;

			if (debug != null && t.FullName != debug)
				continue;
			
			if (t == nso || t.IsSubclassOf (nso)){
				// Useful to debug, uncomment, and examine one class:
				//if (t.Name != "CALayer")
				//continue;
				try {
					ProcessNSO (t);
				} catch {
					Console.WriteLine ("Problem with {0}", t.FullName);
				}
			}
		}

		return 0;
	}
}
