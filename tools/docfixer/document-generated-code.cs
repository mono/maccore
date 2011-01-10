//
// This program takes the API definition from the build and
// uses it to generate the documentation for the auto-generated
// code
//
//
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
#if MONOMAC
using MonoMac.Foundation;
#else
using MonoTouch.Foundation;
#endif

class DocumentGeneratedCode {
#if MONOMAC
	static string ns = "MonoMac";
	Type nso = typeof (MonoMac.Foundation.NSObject);
#else
	static string ns = "MonoTouch";
	Type nso = typeof (MonoTouch.Foundation.NSObject);
#endif

	static void Help ()
	{
		Console.WriteLine ("Usage is: document-generated-code temp.dll path-to-documentation");
	}

	static string assembly_dir;
	static Assembly assembly;

	static string GetMdocPath (Type t)
	{
		return string.Format ("{0}/{1}/{2}.xml", assembly_dir, t.Namespace, t.Name);
	}
	
	static Dictionary<Type,XDocument> docs = new Dictionary<Type,XDocument> ();
	static XDocument GetDoc (Type t)
	{
		if (docs.ContainsKey (t))
			return docs [t];
		
		string xmldocpath = GetMdocPath (t);
		if (!File.Exists (xmldocpath)) {
			Console.WriteLine ("DOC REGEN PENDING for type: {0}", t.FullName);
			return null;
		}
		
		XDocument xmldoc;
		try {
			using (var f = File.OpenText (xmldocpath))
				xmldoc = XDocument.Load (f);
			docs [t] = xmldoc;
		} catch {
			Console.WriteLine ("Failure while loading {0}", xmldocpath);
			return null;
		}

		return xmldoc;
	}
	
	static void SaveDocs ()
	{
		foreach (var t in docs.Keys){
			var xmldocpath = GetMdocPath (t);
			var xmldoc = docs [t];
				
			var xmlSettings = new XmlWriterSettings (){
				Indent = true,
				Encoding = new UTF8Encoding (false),
				OmitXmlDeclaration = true
			};
			using (var xmlw = XmlWriter.Create (xmldocpath, xmlSettings)){
				xmldoc.Save (xmlw);
			}
		}
	}

	//
	// Handles fields, but perhaps this is better done in DocFixer to pull the definitions
	// from the docs?
	//
	public static void ProcessField (Type t, XDocument xdoc, PropertyInfo pi)
	{
		var fieldAttr = pi.GetCustomAttributes (typeof (FieldAttribute), true);
		if (fieldAttr.Length == 0)
			return;
		
		var export = ((FieldAttribute) fieldAttr [0]).SymbolName;
		
		var field = xdoc.XPathSelectElement ("Type/Members/Member[@MemberName='" + pi.Name + "']");
		if (field == null){
			Console.WriteLine ("Warning: {0} document is not up-to-date with the latest assembly", t);
			return;
		}
		var returnType = field.XPathSelectElement ("ReturnValue/ReturnType");
		Console.WriteLine (" {0} {1} -> {2}", t, pi.Name, returnType.Value);
		var summary = field.XPathSelectElement ("Docs/summary");
		var remarks = field.XPathSelectElement ("Docs/remarks");

		// Notifications should be handled in docfixer
		if (returnType.Value == "MonoMac.Foundation.NSString" && pi.Name.EndsWith ("Notification")){
			var mdoc = DocGenerator.GetAppleMemberDocs (t, export);
			if (mdoc == null)
				return;
			
			var section = DocGenerator.ExtractSection (mdoc);
			summary.Value = "";
			summary.Add (section);
		}
	}
	
	public static void ProcessNSO (Type t)
	{
		var xmldoc = GetDoc (t);
		if (xmldoc == null)
			return;
		
		Console.WriteLine ("Processing: {0}", t);
		foreach (var pi in t.GatherProperties ()){
			if (pi.GetCustomAttributes (typeof (FieldAttribute), true).Length > 0)
				ProcessField (t, xmldoc, pi);
		}
	}
			
	public static int Main (string [] args)
	{
		string dir = null;
		string lib = null;
		var debug = Environment.GetEnvironmentVariable ("DOCFIXER");

		for (int i = 0; i < args.Length; i++){
			var arg = args [i];
			if (arg == "-h" || arg == "--help"){
				Help ();
				return 0;
			}
			if (lib == null)
				lib = arg;
			else
				dir = arg;
		}
		
		if (dir == null){
			Help ();
			return 1;
		}
		
		if (File.Exists (Path.Combine (dir, "en"))){
			Console.WriteLine ("The directory does not seem to be the root for documentation (missing `en' directory)");
			return 1;
		}
		assembly_dir = Path.Combine (dir, "en");
		assembly = Assembly.LoadFrom (lib);

		foreach (Type t in assembly.GetTypes ()){
			if (debug != null && t.FullName != debug)
				continue;
			
			if (t.GetCustomAttributes (typeof (BaseTypeAttribute), true).Length > 0)
				ProcessNSO (t);
		}

		SaveDocs ();
		
		return 0;
	}
}