//
// This is the binding generator for the MonoTouch API, it uses the
// contract in API.cs to generate the binding.
//
// Authors:
//   Geoff Norton
//   Miguel de Icaza
//
// Copyright 2009-2010, Novell, Inc.
//
//
// This generator produces various */*.g.cs files based on the
// interface-based type description on this file, see the 
// embedded `MonoTouch.UIKit' namespace here for an example
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
// TODO:
//   * Add support for wrapping "ref" and "out" NSObjects (WrappedTypes)
//     Typically this is necessary for things like NSError.
//
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices;

#if MONOMAC
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
using MonoMac.CoreGraphics;
#else
using MonoTouch.ObjCRuntime;
using MonoTouch.Foundation;
using MonoTouch.CoreFoundation;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreMedia;
#endif

public class RetainListAttribute : Attribute {
	public RetainListAttribute (bool doadd, string name)
	{
		Add = doadd;
		WrapName = name;
	}

	public string WrapName { get; set; }
	public bool Add { get; set; }
}

public class RetainAttribute : Attribute {
	public RetainAttribute ()
	{
	}

	public RetainAttribute (string wrap)
	{
		WrapName = wrap;
	}
	public string WrapName { get; set; }
}

public class PostGetAttribute : Attribute {
	public PostGetAttribute (string name)
	{
		MethodName = name;
	}

	public string MethodName { get; set; }
}

public class FieldAttribute : Attribute {
	public FieldAttribute (string symbolName) {
		SymbolName = symbolName;
	}
	public FieldAttribute (string symbolName, string libraryName) {
		SymbolName = symbolName;
		LibraryName = libraryName;
	}
	public string SymbolName { get; set; }
	public string LibraryName { get; set; }
}

public class BaseTypeAttribute : Attribute {
	public BaseTypeAttribute (Type t)
	{
		BaseType = t;
	}
	public Type BaseType { get; set; }
	public string Name { get; set; }
	public Type [] Events { get; set; }
	public string [] Delegates { get; set; }
	public bool Singleton { get; set; }

	// If set, the code will keep a reference in the EnsureXXX method for
	// delegates and will clear the reference to the object in the method
	// referenced by KeepUntilRef.   Currently uses an ArrayList, so this
	// is not really designed as a workaround for systems that create
	// too many objects, but two cases in particular that users keep
	// trampling on: UIAlertView and UIActionSheet
	public string KeepRefUntil { get; set; }
}

//
// Used for methods that invoke other targets, not this.Handle
//
public class BindAttribute : Attribute {
	public BindAttribute (string sel)
	{
		Selector = sel;
	}
	public string Selector { get; set; }

	// By default [Bind] makes non-virtual methods
	public bool Virtual { get; set; }
}

public class WrapAttribute : Attribute {
	public WrapAttribute (string methodname)
	{
		MethodName = methodname;
	}
	public string MethodName { get; set; }
}

// When applied instructs the generator to call Release on the returned objects
// this happens when factory methods in Objetive-C return objects with refcount=1
public class FactoryAttribute : Attribute {
	public FactoryAttribute () {}
}

// When applied, it instructs the generator to not use NSStrings for marshalling.
public class PlainStringAttribute : Attribute {
	public PlainStringAttribute () {}
}

//
// When applied, instructs the generator to use this object as the
// target, instead of the implicit Handle Can only be used in methods
// that are [Bind] instead of [Export]
//
public class TargetAttribute : Attribute {
	public TargetAttribute () {}
}

public class StaticAttribute : Attribute {
	public StaticAttribute () {}
}

public class NullAllowedAttribute : Attribute {
	public NullAllowedAttribute () {}
}

public class InternalAttribute : Attribute {
	public InternalAttribute () {}
}

// Used for mandatory methods that must be implemented in a [Model].
public class AbstractAttribute : Attribute {
	public AbstractAttribute () {} 
}

// Used for mandatory methods that must be implemented in a [Model].
public class OverrideAttribute : Attribute {
	public OverrideAttribute () {} 
}

// Makes the result use the `new' attribtue
public class NewAttribute : Attribute {
	public NewAttribute () {} 
}

// Makes the result sealed
public class SealedAttribute : Attribute {
	public SealedAttribute () {} 
}

public class EventArgsAttribute : Attribute {
	public EventArgsAttribute (string s)
	{
		ArgName = s;
	}
	public EventArgsAttribute (string s, bool skip)
	{
		ArgName = s;
		SkipGeneration = skip;
	}
	public EventArgsAttribute (string s, bool skip, bool fullname)
	{
		ArgName = s;
		SkipGeneration = skip;
		FullName = fullname;
	}

	public string ArgName { get; set; }
	public bool SkipGeneration { get; set; }
	public bool FullName { get; set; }
}

public class EventNameAttribute : Attribute {
	public EventNameAttribute (string s)
	{
		EvtName = s;
	}
	public string EvtName { get; set; }
}

public class DefaultValueAttribute : Attribute {
	public DefaultValueAttribute (object o){
		Default = o;
	}
	public object Default { get; set; }
}

public class DefaultValueFromArgumentAttribute : Attribute {
	public DefaultValueFromArgumentAttribute (string s){
		Argument = s;
	}
	public string Argument { get; set; }
}
//
// Used to encapsulate flags about types in either the parameter or the return value
// For now, it only supports the [PlainString] attribute on strings.
//
public class MarshalInfo {
	public bool PlainString;
	public Type Type;

	// Used for parameters
	public MarshalInfo (ParameterInfo pi)
	{
		PlainString = pi.GetCustomAttributes (typeof (PlainStringAttribute), true).Length > 0;
		Type = pi.ParameterType;
	}

	// Used to return values
	public MarshalInfo (MethodInfo mi)
	{
		PlainString = mi.ReturnTypeCustomAttributes.GetCustomAttributes (typeof (PlainStringAttribute), true).Length > 0;
		Type = mi.ReturnType;
	}

	public static bool UseString (ParameterInfo pi)
	{
		return new MarshalInfo (pi).PlainString;
	}

	public static implicit operator MarshalInfo (ParameterInfo pi)
	{
		return new MarshalInfo (pi);
	}

	public static implicit operator MarshalInfo (MethodInfo mi)
	{
		return new MarshalInfo (mi);
	}
}

public class Tuple<A,B> {
	public Tuple (A a, B b)
	{
		Item1 = a;
		Item2 = b;
	}
	public A Item1;
	public B Item2;
}
//
// Encapsulates the information necessary to create a block delegate
//
// The Name is the internal generated name we use for the delegate
// The Parameters is used for the internal delegate signature
// The Invoke contains the invocation steps necessary to invoke the method
//
public class TrampolineInfo {
	public string UserDelegate, DelegateName, TrampolineName, Parameters, Invoke;

	public TrampolineInfo (string userDelegate, string delegateName, string trampolineName, string pars, string invoke)
	{
		UserDelegate = userDelegate;
		DelegateName = delegateName;
		Parameters = pars;
		TrampolineName = trampolineName;
		Invoke = invoke;
	}

	public string StaticName {
		get {
			return "static_" + DelegateName;
		}
	}
}

public class Generator {
	Dictionary<Type,IEnumerable<string>> selectors = new Dictionary<Type,IEnumerable<string>> ();
	Dictionary<Type,bool> need_static = new Dictionary<Type,bool> ();
	Dictionary<Type,bool> need_abstract = new Dictionary<Type,bool> ();
	Dictionary<string,int> selector_use = new Dictionary<string, int> ();
	Dictionary<string,string> selector_names = new Dictionary<string,string> ();
	Dictionary<string,string> send_methods = new Dictionary<string,string> ();
	Dictionary<string,string> original_methods = new Dictionary<string,string> ();
	List<MarshalType> marshal_types = new List<MarshalType> ();
	Dictionary<Type,TrampolineInfo> trampolines = new Dictionary<Type,TrampolineInfo> ();
	Dictionary<Type,Type> delegates_emitted = new Dictionary<Type, Type> ();
	
	public bool Alpha;
	public bool OnlyX86;
	
	Type [] types;
	bool debug;
	bool external;
	bool btouch;
	StreamWriter sw, m;
	int indent;

	public class MarshalType {
		public Type Type;
		public string Encoding;
		public string ParameterMarshal;
		public string CreateFromRet;
		
		public MarshalType (Type t, string encode, string fetch, string create)
		{
			Type = t;
			Encoding = encode;
			ParameterMarshal = fetch;
			CreateFromRet = create;
		}
	}

	public bool LookupMarshal (Type t, out MarshalType res)
	{
		res = null;
		foreach (var mt in marshal_types){
			if (mt.Type == t){
				res = mt;
				return true;
			}
		}
		return false;
	}

	//
	// Properties and definitions to support binding third-party Objective-C libraries
	//
	string init_binding_type;

	// Where the assembly messaging is located (core)
	public string CoreMessagingNS = "MonoTouch.ObjCRuntime";

	// This can be plugged by the user when using btouch/bmac for their own bindings
	public string MessagingNS = "MonoTouch.ObjCRuntime";
	
	public bool BindThirdPartyLibrary = false;
	public string BaseDir { get { return basedir; } set { basedir = value; }}
	string basedir;
	public List<string> GeneratedFiles = new List<string> ();
	public Type CoreNSObject = typeof (NSObject);
#if MONOMAC
	public Type MessagingType = typeof (MonoMac.ObjCRuntime.Messaging);
	public Type SampleBufferType = typeof (MonoMac.CoreMedia.CMSampleBuffer);
	string [] standard_namespaces = new string [] { "MonoMac.Foundation", "MonoMac.ObjCRuntime", "MonoMac.CoreGraphics" };
	const string MainPrefix = "MonoMac";
#else
	public Type MessagingType = typeof (MonoTouch.ObjCRuntime.Messaging);
	public Type SampleBufferType = typeof (MonoTouch.CoreMedia.CMSampleBuffer);
	string [] standard_namespaces = new string [] { "MonoTouch.Foundation", "MonoTouch.ObjCRuntime", "MonoTouch.CoreGraphics" };
	const string MainPrefix = "MonoTouch";
#endif

	//
	// Used by the public binding generator to populate the
	// class with types that do not exist
	//
	public void RegisterMethodName (string method_name)
	{
		send_methods [method_name] = method_name;
		original_methods [method_name] = method_name;
	}

	//
	// Helpers
	//
	string MakeSig (MethodInfo mi, bool stret) { return MakeSig ("objc_msgSend", stret, mi); }
	string MakeSuperSig (MethodInfo mi, bool stret) { return MakeSig ("objc_msgSendSuper", stret, mi); }

	bool IsNativeType (Type pt)
	{
		return (pt == typeof (int) || pt == typeof (long) || pt == typeof (byte) || pt == typeof (short));
	}

	public string PrimitiveType (Type t)
	{
		if (t == typeof (void))
			return "void";

		if (t.IsEnum)
			t = Enum.GetUnderlyingType (t);
		
		if (t == typeof (int))
			return "int";
		if (t == typeof (short))
			return "short";
		if (t == typeof (byte))
			return "byte";
		if (t == typeof (float))
			return "float";

		return t.Name;
	}

	// Is this a wrapped type of NSObject from the MonoTouch/MonoMac binding world?
	public bool IsWrappedType (Type t)
	{
		if (t.IsInterface) 
			return true;
		if (CoreNSObject != null)
			return t.IsSubclassOf (CoreNSObject) || t == CoreNSObject; 
		return false;
	}

	//
	// Returns the type that we use to marshal the given type as a string
	// for example "UIView" -> "IntPtr"
	string ParameterGetMarshalType (MarshalInfo mai)
	{
		if (mai.Type.IsEnum)
			return PrimitiveType (mai.Type);

		if (IsWrappedType (mai.Type))
			return "IntPtr";

		if (IsNativeType (mai.Type))
			return PrimitiveType (mai.Type);

		if (mai.Type == typeof (string)){
			if (mai.PlainString)
				return "string";

			// We will do NSString
			return "IntPtr";
		} 

		MarshalType mt;
		if (LookupMarshal (mai.Type, out mt))
			return mt.Encoding;
		
		if (mai.Type.IsValueType)
			return PrimitiveType (mai.Type);

		// Arrays are returned as NSArrays
		if (mai.Type.IsArray)
			return "IntPtr";

		//
		// Pass "out ValueType" directly
		//
		if (mai.Type.IsByRef && mai.Type.GetElementType ().IsValueType)
			return "out " + mai.Type.GetElementType ().Name;

		if (mai.Type.IsSubclassOf (typeof (Delegate))){
			return "IntPtr";
		}
		
		//
		// Edit the table in the "void Go ()" routine
		//
		
		if (mai.Type.IsByRef && mai.Type.GetElementType ().IsValueType == false)
			return "IntPtr";
		
		Console.WriteLine ("Do not know how to make a signature for {0}", mai.Type);
		throw new Exception ();
		//return null;
	}

	//
	// This probably should use MarshalInfo to find the correct way of turning
	// the native types into managed types instead of hardcoding the limited
	// values we know about here
	//
	public string MakeTrampoline (Type t)
	{
		if (trampolines.ContainsKey (t))
			return trampolines [t].StaticName;

		var mi = t.GetMethod ("Invoke");
		var pars = new StringBuilder ();
		var invoke = new StringBuilder ();
		pars.Append ("IntPtr block");
		var parameters = mi.GetParameters ();
		foreach (var pi in parameters){
			pars.Append (", ");
			if (pi != parameters [0])
				invoke.Append (", ");
			
			if (IsWrappedType (pi.ParameterType)){
				pars.AppendFormat ("IntPtr {0}", pi.Name);
				invoke.AppendFormat ("({1}) Runtime.GetNSObject ({0})", pi.Name, pi.ParameterType);
				continue;
			}
			if (pi.ParameterType.IsByRef){
				var nt = pi.ParameterType.GetElementType ();
				if (nt.IsValueType){
					pars.AppendFormat ("{0} {1} {2}", pi.IsOut ? "out" : "ref", FormatType (null, nt), pi.Name);
					invoke.AppendFormat ("{0} {1}", pi.IsOut ? "out" : "ref", pi.Name);
					continue;
				} 
			} else if (pi.ParameterType.IsValueType){
				pars.AppendFormat ("{0} {1}", pi.ParameterType.Name, pi.Name);
				invoke.AppendFormat ("{0}", pi.Name);
				continue;
			}
		
			if (pi.ParameterType == typeof (string [])){
				pars.AppendFormat ("IntPtr {0}", pi.Name);
				invoke.AppendFormat ("NSArray.StringArrayFromHandle ({0})", pi.Name);
				continue;
			}
			if (pi.ParameterType == typeof (string)){
				pars.AppendFormat ("IntPtr {0}", pi.Name);
				invoke.AppendFormat ("NSString.FromHandle ({0})", pi.Name);
				continue;
			}
			if (pi.ParameterType == SampleBufferType){
				pars.AppendFormat ("IntPtr {0}", pi.Name);
				invoke.AppendFormat ("new CMSampleBuffer ({0})", pi.Name);
				continue;
			}

			if (pi.ParameterType == typeof (string [])){
				pars.AppendFormat ("string [] {0}", pi.Name);
				invoke.AppendFormat ("{0}", pi.Name);
				continue;
			}
			
			if (pi.ParameterType.IsArray){
				Type et = pi.ParameterType.GetElementType ();
				if (IsWrappedType (et)){
					pars.AppendFormat ("IntPtr {0}", pi.Name);
					invoke.AppendFormat ("NSArray.ArrayFromHandle<{0}> ({1})", FormatType (null, et), pi.Name);
					continue;
				}
			}
			
			Console.Error.WriteLine ("MakeTrampoline: do not know how to make a trampoline for {0}", pi);
			throw new Exception ();
		}

		var ti = new TrampolineInfo (t.FullName,
					     "Inner" + t.Name,
					     "Trampoline" + t.Name,
					     pars.ToString (), invoke.ToString ());

		trampolines [t] = ti;
		return ti.StaticName;
	}
	
	//
	// Returns the actual way in which the type t must be marshalled
	// for example "UIView foo" is generated as  "foo.Handle"
	//
	public string MarshalParameter (ParameterInfo pi, bool null_allowed_override)
	{
		if (pi.ParameterType.IsByRef && pi.ParameterType.GetElementType ().IsValueType == false){
			return pi.Name + "Ptr";
		}
		
		if (IsWrappedType (pi.ParameterType)){
			if (null_allowed_override || HasAttribute (pi, typeof (NullAllowedAttribute)))
				return String.Format ("{0} == null ? IntPtr.Zero : {0}.Handle", pi.Name);
			return pi.Name + ".Handle";
		}
		
		if (pi.ParameterType.IsEnum){
			return "(" + PrimitiveType (pi.ParameterType) + ")" + pi.Name;
		}
		
		if (IsNativeType (pi.ParameterType))
			return pi.Name;

		if (pi.ParameterType == typeof (string)){
			if (MarshalInfo.UseString (pi))
				return pi.Name;
			else {
				if (null_allowed_override || HasAttribute (pi, typeof (NullAllowedAttribute)))
					return String.Format ("ns{0} == null ? IntPtr.Zero : ns{0}.Handle", pi.Name);
				else 
					return "ns" + pi.Name + ".Handle";
			}
		}

		if (pi.ParameterType.IsValueType)
			return pi.Name;

		MarshalType mt;
		if (LookupMarshal (pi.ParameterType, out mt)){
			string access = String.Format (mt.ParameterMarshal, pi.Name);
			if (null_allowed_override || HasAttribute (pi, typeof (NullAllowedAttribute)))
				return String.Format ("{0} == null ? IntPtr.Zero : {1}", pi.Name, access);
			return access;
		}

		if (pi.ParameterType.IsArray){
			//Type etype = pi.ParameterType.GetElementType ();

			if (null_allowed_override || HasAttribute (pi, typeof (NullAllowedAttribute)))
				return String.Format ("nsa_{0} == null ? IntPtr.Zero : nsa_{0}.Handle", pi.Name);
			return "nsa_" + pi.Name + ".Handle";
		}

		//
		// Handle (out ValeuType foo)
		//
		if (pi.ParameterType.IsByRef && pi.ParameterType.GetElementType ().IsValueType){
			return "out " + pi.Name;
		}

		if (pi.ParameterType.IsSubclassOf (typeof (Delegate))){
			return String.Format ("(IntPtr) block_ptr_{0}", pi.Name);
		}
		
		Console.WriteLine ("Unknown kind {0}", pi);
		throw new Exception ();
	}

	public bool ParameterNeedsNullCheck (ParameterInfo pi, MethodInfo mi)
	{
		if (HasAttribute (pi, typeof (NullAllowedAttribute)))
			return false;

		if (mi.IsSpecialName && mi.Name.StartsWith ("set_")){
			if (HasAttribute (mi, typeof (NullAllowedAttribute))){
				return false;
			}
		}
		if (IsWrappedType (pi.ParameterType))
			return true;

		if (pi.ParameterType.IsArray)
			return true;
		
		if (pi.ParameterType == typeof (Selector))
			return true;
		
		if (pi.ParameterType == typeof (Class))
			return true;
		
		if (pi.ParameterType == typeof (NSObject))
			return true;
		
		if (IsNativeType (pi.ParameterType))
			return false;

		if (pi.ParameterType == typeof (string))
			return true;

		if (pi.ParameterType.IsSubclassOf (typeof (Delegate)))
			return true;
		return false;
	}

	public object GetAttribute (MethodInfo mi, Type t)
	{
		object [] a = mi.GetCustomAttributes (t, true);
		if (a.Length > 0)
			return a [0];
		return null;
	}

	public BindAttribute GetBindAttribute (MethodInfo mi)
	{
		return GetAttribute (mi, typeof (BindAttribute)) as BindAttribute;
	}
	
	public bool HasAttribute (PropertyInfo pi, Type t)
	{
		return pi.GetCustomAttributes (t, true).Length > 0;
	}

	public bool HasAttribute (Type queryType, Type t)
	{
		return queryType.GetCustomAttributes (t, true).Length > 0;
	}
	
	public bool HasAttribute (MethodInfo mi, Type t)
	{
		return mi.GetCustomAttributes (t, true).Length > 0;
	}

	public bool HasAttribute (ParameterInfo pi, Type t)
	{
		return pi.GetCustomAttributes (t, true).Length > 0;
	}
	
	public bool IsTarget (ParameterInfo pi)
	{
		return HasAttribute (pi, typeof (TargetAttribute)); 
	}
	
	//
	// Makes the method name for a objcSend call
	//
	string MakeSig (string send, bool stret, MethodInfo mi)
	{
		var sb = new StringBuilder ();

		try {
			sb.Append (ParameterGetMarshalType (mi));
		} catch {
			Console.WriteLine ("   in Method `{0}'", mi.Name);
		}

		sb.Append ("_");
		sb.Append (send);
		if (stret)
			sb.Append ("_stret");
		
		foreach (var pi in mi.GetParameters ()){
			if (IsTarget (pi))
				continue;
			sb.Append ("_");
			try {
				sb.Append (ParameterGetMarshalType (new MarshalInfo (pi)).Replace (' ', '_'));
			} catch {
				Console.WriteLine ("  in parameter `{0}' from {1}.{2}", pi.Name, mi.DeclaringType, mi.Name);
				throw;
			}
		}

		return sb.ToString ();
	}

	void RegisterMethod (bool need_stret, MethodInfo mi, string method_name)
	{
		if (send_methods.ContainsKey (method_name))
			return;
		send_methods [method_name] = method_name;

		var b = new StringBuilder ();
		int n = 0;
		
		foreach (var pi in mi.GetParameters ()){
			if (IsTarget (pi))
				continue;

			b.Append (", ");

			try {
				b.Append (ParameterGetMarshalType (pi));
			} catch {
				Console.WriteLine ("  in parameter {0} of {1}.{2}", pi.Name, mi.DeclaringType, mi.Name);
			}
			b.Append (" ");
			b.Append ("arg" + (++n));
		}

		string entry_point;
		if (method_name.IndexOf ("objc_msgSendSuper") != -1){
			entry_point = need_stret ? "objc_msgSendSuper_stret" : "objc_msgSendSuper";
		} else
			entry_point = need_stret ? "objc_msgSend_stret" : "objc_msgSend";

		//
		// These exist in Messaging.cs and we can not remove them from there
		//
		switch (method_name){
		case "void_objc_msgSend":
		case "void_objc_msgSendSuper":
			return;
		}

		print (m, "\t\t[DllImport (LIBOBJC_DYLIB, EntryPoint=\"{0}\")]", entry_point);
		print (m, "\t\tpublic extern static {0} {1} ({3}IntPtr receiver, IntPtr selector{2});",
		       need_stret ? "void" : ParameterGetMarshalType (mi), method_name, b.ToString (),
		       need_stret ? "out " + FormatType (MessagingType, mi.ReturnType) + " retval, " : "");
		       
	}

	bool ArmNeedStret (MethodInfo mi)
	{
		Type t = mi.ReturnType;

		if (!t.IsValueType || t.IsEnum || t.Assembly == typeof (object).Assembly)
			return false;

		return true;
	}

	bool X86NeedStret (MethodInfo mi)
	{
		Type t = mi.ReturnType;
		
		if (!t.IsValueType || t.IsEnum || t.Assembly == typeof (object).Assembly)
			return false;

		return Marshal.SizeOf (t) > 8;
	}

	bool NeedStret (MethodInfo mi)
	{
		return ArmNeedStret (mi) || X86NeedStret (mi); 
	}
		       
	
	void DeclareInvoker (MethodInfo mi)
	{
		bool arm_stret = ArmNeedStret (mi);
		try {
			RegisterMethod (arm_stret, mi, MakeSig (mi, arm_stret));
			RegisterMethod (arm_stret, mi, MakeSuperSig (mi, arm_stret));
			
			bool x86_stret = X86NeedStret (mi);
			if (x86_stret != arm_stret){
				RegisterMethod (x86_stret, mi, MakeSig (mi, x86_stret));
				RegisterMethod (x86_stret, mi, MakeSuperSig (mi, x86_stret));
			}
		} catch {
			Console.WriteLine ("   in Method: {0}", mi);
		}
	}

	//
	// Either we have an [Export] attribute, or we have a [Wrap] one
	//
	ExportAttribute GetExportAttribute (PropertyInfo pi, out string wrap)
	{
		wrap = null;
		object [] attrs = pi.GetCustomAttributes (typeof (ExportAttribute), true);
		if (attrs.Length == 0){
			attrs = pi.GetCustomAttributes (typeof (WrapAttribute), true);
			if (attrs.Length != 0){
				wrap = ((WrapAttribute) attrs [0]).MethodName;
				return null;
			}
			return null;
		}

		return (ExportAttribute) attrs [0];
	}

	public ExportAttribute MakeSetAttribute (ExportAttribute source)
	{
		return new ExportAttribute ("set" + Char.ToUpper (source.Selector [0]) + source.Selector.Substring (1) + ":");
	}

	public Generator (bool btouch, bool external, bool debug, Type [] types)
	{
		this.btouch = btouch;
		this.external = external;
		this.debug = debug;
		this.types = types;
		basedir = ".";
	}

	public void Go ()
	{
		marshal_types.Add (new MarshalType (typeof (CGColor), "IntPtr", "{0}.handle", "new CGColor ("));
		marshal_types.Add (new MarshalType (typeof (CGContext), "IntPtr", "{0}.Handle", "new CGContext ("));
		marshal_types.Add (new MarshalType (typeof (CGImage), "IntPtr", "{0}.Handle", "new CGImage ("));
		marshal_types.Add (new MarshalType (typeof (CGPath), "IntPtr", "{0}.handle", "new CGPath ("));
		marshal_types.Add (new MarshalType (typeof (NSObject), "IntPtr", "{0}.Handle", "Runtime.GetNSObject ("));
		marshal_types.Add (new MarshalType (typeof (Selector), "IntPtr", "{0}.Handle", "new Selector ("));
		marshal_types.Add (new MarshalType (typeof (Class), "IntPtr", "{0}.Handle", "new Class ("));
		marshal_types.Add (new MarshalType (typeof (NSString), "IntPtr", "{0}.Handle", "new NSString ("));
		marshal_types.Add (new MarshalType (typeof (CFRunLoop), "IntPtr", "{0}.Handle", "new CFRunLoop ("));
		marshal_types.Add (new MarshalType (typeof (CGColorSpace), "IntPtr", "{0}.Handle", "new CGColorSpace ("));
		marshal_types.Add (new MarshalType (typeof (DispatchQueue), "IntPtr", "{0}.Handle", "new DispatchQueue ("));
#if !MONOMAC
		marshal_types.Add (new MarshalType (typeof (MonoTouch.CoreMedia.CMSampleBuffer), "IntPtr", "{0}.Handle", "new MonoTouch.CoreMedia.CMSampleBuffer ("));
#endif

		marshal_types.Add (new MarshalType (typeof (BlockLiteral), "BlockLiteral", "{0}", "THIS_IS_BROKEN"));

		init_binding_type = String.Format ("IsDirectBinding = GetType ().Assembly == global::{0}.Messaging.this_assembly;", MessagingNS);

		Directory.CreateDirectory (Path.Combine (basedir, "ObjCRuntime"));

		var messaging_cs = Path.Combine (basedir, "ObjCRuntime/Messaging.g.cs");
		GeneratedFiles.Add (messaging_cs);
		m = new StreamWriter (messaging_cs);
		Header (m);
		print (m, "namespace {0} {{", MessagingNS);
		print (m, "\tpublic partial class Messaging {");

		if (BindThirdPartyLibrary){
			print (m, "\t\tstatic internal System.Reflection.Assembly this_assembly = typeof (Messaging).Assembly;\n");
			print (m, "\t\t\tconst string LIBOBJC_DYLIB = \"/usr/lib/libobjc.dylib\";\n");

		}
		
		foreach (Type t in types){
			if (HasAttribute (t, typeof (AlphaAttribute)) && Alpha == false)
				continue;

			var tselectors = new List<string> ();
			
			foreach (var pi in t.GetProperties ()){
				if (HasAttribute (pi, typeof (AlphaAttribute)) && Alpha == false)
					continue;

				string wrapname;
				var export = GetExportAttribute (pi, out wrapname);
				if (export == null){
					if (wrapname != null)
						continue;

					// Let properties with the [Field] attribute through as well.
					var attrs = pi.GetCustomAttributes (typeof (FieldAttribute), true);
					if (attrs.Length != 0)
						continue;
					
					Console.WriteLine ("Error: no [Export] attribute on property {0}.{1}", pi.DeclaringType, pi);
					Environment.Exit (1);
				}
				if (HasAttribute (pi, typeof (StaticAttribute)))
					need_static [t] = true;

				bool is_abstract = HasAttribute (pi, typeof (AbstractAttribute));
				
				if (pi.CanRead){
					MethodInfo getter = pi.GetGetMethod ();
					BindAttribute ba = GetBindAttribute (getter);

					if (!is_abstract)
						tselectors.Add (ba != null ? ba.Selector : export.Selector);
					DeclareInvoker (getter);
				}
				
				if (pi.CanWrite){
					MethodInfo setter = pi.GetSetMethod ();
					BindAttribute ba = GetBindAttribute (setter);
					
					if (!is_abstract)
						tselectors.Add (ba != null ? ba.Selector : MakeSetAttribute (export).Selector);
					DeclareInvoker (setter);
				}
			}
			
			foreach (var mi in t.GetMethods (BindingFlags.Instance | BindingFlags.Public)){
				// Skip properties
				if (mi.IsSpecialName)
					continue;

				if (HasAttribute (mi, typeof (AlphaAttribute)) && Alpha == false)
					continue;
				
				foreach (Attribute attr in mi.GetCustomAttributes (typeof (Attribute), true)){
					string selector = null;
					ExportAttribute ea = attr as ExportAttribute;
					BindAttribute ba = attr as BindAttribute;
					if (ea != null){
						selector = ea.Selector;
					} else if (ba != null){
						selector = ba.Selector;
					} else if (attr is StaticAttribute){
						need_static [t] = true;
						continue;
					} else if (attr is InternalAttribute){
						continue;
					} else if (attr is FactoryAttribute){
						continue;
					} else  if (attr is AbstractAttribute){
						need_abstract [t] = true;
						continue;
					} else if (attr is SealedAttribute || attr is EventArgsAttribute || attr is EventNameAttribute || attr is DefaultValueAttribute || attr is ObsoleteAttribute || attr is AlphaAttribute || attr is DefaultValueFromArgumentAttribute || attr is NewAttribute || attr is SinceAttribute || attr is PostGetAttribute)
						continue;
					else 
						Console.WriteLine ("Error: Unknown attribute {0} on {1}", attr.GetType (), t);

					if (selector == null){
						Console.WriteLine ("Error: No selector specified for method `{0}.{1}'", mi.DeclaringType, mi.Name);
						Environment.Exit (1);
					}
					
					tselectors.Add (selector);
					if (selector_use.ContainsKey (selector)){
						selector_use [selector]++;
					} else
						selector_use [selector] = 1;
				}

				DeclareInvoker (mi);
			}

			foreach (var pi in t.GetProperties (BindingFlags.Instance | BindingFlags.Public)){
				if (HasAttribute (pi, typeof (AlphaAttribute)) && Alpha == false)
					continue;

				if (HasAttribute (pi, typeof (AbstractAttribute)))
					need_abstract [t] = true;
			}
			
			selectors [t] = tselectors.Distinct ();
		}

		foreach (Type t in types){
			if (HasAttribute (t, typeof (AlphaAttribute)) && Alpha == false)
				continue;
			
			Generate (t);
		}
		
		print (m, "\t}\n}");
		m.Close ();
	}

	public void print (string format)
	{
		if (indent != 0)
			for (int i = 0; i < indent; i++)
				sw.Write ("\t");
		print (sw, format);
	}

	public void print (string format, params object [] args)
	{
		if (indent != 0)
			for (int i = 0; i < indent; i++)
				sw.Write ("\t");
		print (sw, format, args);
	}

	public void print (StreamWriter w, string format)
	{
		w.WriteLine (format);
	}

	public void print (StreamWriter w, string format, params object [] args)
	{
		w.WriteLine (format, args);
	}

	public void print (StreamWriter w, IEnumerable e)
	{
		foreach (var a in e)
			w.WriteLine (a);
	}
	
	public string SelectorField (string s)
	{
		string name;
		
		if (selector_names.TryGetValue (s, out name))
			return name;
		
		StringBuilder sb = new StringBuilder ();
		bool up = true;
		sb.Append ("sel");
		
		foreach (char c in s){
			if (up && c != ':'){
				sb.Append (Char.ToUpper (c));
				up = false;
			} else if (c == ':') {
				up = true;
			} else
				sb.Append (c);
		}
		name = sb.ToString ();
		selector_names [s] = name;
		return name;
	}

	public string FormatType (Type usedIn, Type type)
	{
		if (type == typeof (void))
			return "void";
		if (type == typeof (int))
			return "int";
		if (type == typeof (short))
			return "short";
		if (type == typeof (byte))
			return "byte";
		if (type == typeof (float))
			return "float";
		if (type == typeof (bool))
			return "bool";

		if (usedIn != null && type.Namespace == usedIn.Namespace)
			return type.Name;

		if (standard_namespaces.Contains (type.Namespace))
			return type.Name;

		if (type == typeof (string))
			return "string";

		// Use fully qualified name
		return type.ToString ();
	}

	//
	// Makes the public signature for an exposed method
	//
	public string MakeSignature (MethodInfo mi)
	{
		StringBuilder sb = new StringBuilder ();
		bool ctor = mi.Name == "Constructor";
		string name =  ctor ? mi.DeclaringType.Name : mi.Name;

		if (mi.Name == "AutocapitalizationType"){
		}
		if (!ctor){
			sb.Append (FormatType (mi.DeclaringType, mi.ReturnType));
			sb.Append (" ");
		}
		sb.Append (name);
		sb.Append (" (");

		bool comma = false;
		foreach (var pi in mi.GetParameters ()){
			if (comma)
				sb.Append (", ");
			comma = true;

			// Format nicely the type, as succinctly as possible
			Type parType = pi.ParameterType;
			if (parType.IsByRef && parType.GetElementType ().IsValueType){
				sb.Append ("out ");
				parType = parType.GetElementType ();
			} 
			else if (parType.IsByRef && parType.GetElementType ().IsValueType == false){
				sb.Append ("out ");
				parType = parType.GetElementType ();
			}
		
			sb.Append (FormatType (mi.DeclaringType, parType));
			sb.Append (" ");
			sb.Append (pi.Name);
		}
		sb.Append (")");
		if (ctor)
			sb.Append (" : base (NSObjectFlag.Empty)");
		return sb.ToString ();
	}

	string [] implicit_ns = new string [] {
		"System", 
		"System.Drawing", 
		"System.Runtime.InteropServices",
#if MONOMAC
		"MonoMac.CoreFoundation",
		"MonoMac.Foundation",
		"MonoMac.ObjCRuntime",
		"MonoMac.CoreGraphics",
		"MonoMac.CoreAnimation",
		"MonoMac.CoreLocation", 
		"MonoMac.QTKit",
#else
		"MonoTouch",
		"MonoTouch.CoreFoundation",
		"MonoTouch.CoreMedia",
		"MonoTouch.Foundation", 
		"MonoTouch.ObjCRuntime", 
		"MonoTouch.CoreAnimation", 
		"MonoTouch.CoreLocation", 
		"MonoTouch.MapKit", 
		"MonoTouch.UIKit",
		"MonoTouch.CoreGraphics"
#endif
	};
		
	void Header (StreamWriter w)
	{
		print (w, "//\n// Auto-generated from generator.cs, do not edit\n//");
		print (w, "// We keep references to objects, so warning 414 is expected\n");
		print (w, "#pragma warning disable 414\n");
		print (w, from ns in implicit_ns select "using " + ns + ";\n");
	}

	void GenerateInvoke (bool stret, bool supercall, MethodInfo mi, string selector, string args, bool assign_to_temp, bool is_static)
	{
		string target_name = "this";

		// If we have supercall == false, we can be a Bind methdo that has a [Target]
		if (supercall == false && !is_static){
			foreach (var pi in mi.GetParameters ()){
				if (IsTarget (pi)){
					if (pi.ParameterType == typeof (string)){
						if (new MarshalInfo (pi).PlainString){
							Console.WriteLine ("Trying to use a string as a [Target]");
						}
						target_name = "ns" + pi.Name;
					} else
						target_name = pi.Name;
					break;
				}
			}
		}
		
		string sig = supercall ? MakeSuperSig (mi, stret) : MakeSig (mi, stret);

		// If we had this registered, it means that it existed in monotouch.dll, so fully namespace it
		if (original_methods.ContainsKey (sig))
			sig = CoreMessagingNS + ".Messaging." + sig;
		else
			sig = MessagingNS + ".Messaging." + sig;
		
		if (stret){
			if (is_static)
				print ("{0} (out ret, class_ptr, {3}{4});", sig, target_name, supercall ? "Super" : "", selector, args);
			else
				print ("{0} (out ret, {1}.{2}Handle, {3}{4});", sig, target_name, supercall ? "Super" : "", selector, args);
		} else {
			bool returns = mi.ReturnType != typeof (void) && mi.Name != "Constructor";

			string cast_a = "", cast_b = "";
			if (returns){
				MarshalInfo mai = new MarshalInfo (mi);
				MarshalType mt;
				
				if (mi.ReturnType.IsEnum){
					cast_a = "(" + FormatType (mi.DeclaringType, mi.ReturnType) + ") ";
					cast_b = "";
				} else if (IsWrappedType (mi.ReturnType)){
					cast_a = "(" + FormatType (mi.DeclaringType, mi.ReturnType) + ") Runtime.GetNSObject (";
					cast_b = ")";
				} else if (mai.Type == typeof (string) && !mai.PlainString){
					cast_a = "NSString.FromHandle (";
					cast_b = ")";
				} else if (LookupMarshal (mai.Type, out mt)){
					cast_a = mt.CreateFromRet;
					cast_b = ")";
				} else if (mai.Type.IsArray){
					Type etype = mai.Type.GetElementType ();
					if (etype == typeof (string)){
						cast_a = "NSArray.StringArrayFromHandle (";
						cast_b = ")";
					} else {
						cast_a = "NSArray.ArrayFromHandle<" + etype + ">(";
						cast_b = ")";
					}
				}
			} else if (mi.Name == "Constructor") {
				cast_a = "Handle = ";
			}

			if (is_static)
				print ("{0}{1}{2} (class_ptr, {5}{6}){7};",
				       returns ? (assign_to_temp ? "ret = " : "return ") : "",
				       cast_a, sig, target_name, 
				       supercall ? "Super" : "",
				       selector, args, cast_b);
			else
				print ("{0}{1}{2} ({3}.{4}Handle, {5}{6}){7};",
				       returns ? (assign_to_temp ? "ret = " : "return ") : "",
				       cast_a, sig, target_name, 
				       supercall ? "Super" : "",
				       selector, args, cast_b);
		}
	}
	
	void GenerateInvoke (bool supercall, MethodInfo mi, string selector, string args, bool assign_to_temp, bool is_static)
	{
		bool arm_stret = ArmNeedStret (mi);
		bool x86_stret = X86NeedStret (mi);

		if (OnlyX86){
			GenerateInvoke (x86_stret, supercall, mi, selector, args, assign_to_temp, is_static);
			return;
		}
		
		bool need_two_paths = arm_stret != x86_stret;
		if (need_two_paths){
			print ("if (Runtime.Arch == Arch.DEVICE){");
			indent++;
			GenerateInvoke (arm_stret, supercall, mi, selector, args, assign_to_temp, is_static);
			indent--;
			print ("} else {");
			indent++;
			GenerateInvoke (x86_stret, supercall, mi, selector, args, assign_to_temp, is_static);
			indent--;
			print ("}");
		} else {
			GenerateInvoke (arm_stret, supercall, mi, selector, args, assign_to_temp, is_static);
		}
	}

	//
	// The NullAllowed can be applied on a property, to avoid the ugly syntax, we allow it on the property
	// So we need to pass this as `null_allowed_override',   This should only be used by setters.
	//
	public void GenerateMethodBody (Type type, MethodInfo mi, bool virtual_method, bool is_static, string sel, bool null_allowed_override, string assign = null)
	{
		string selector = SelectorField (sel);
		var args = new StringBuilder ();
		var convs = new StringBuilder ();
		var disposes = new StringBuilder ();
		var byRefPostProcessing = new StringBuilder();
		
		indent++;
		foreach (var pi in mi.GetParameters ()){
			MarshalInfo mai = new MarshalInfo (pi);

			if (!IsTarget (pi)){
				// Construct invocation
				args.Append (", ");
				args.Append (MarshalParameter (pi, null_allowed_override));
			}

			// Construct conversions
			if (mai.Type == typeof (string) && !mai.PlainString){
				if (null_allowed_override || HasAttribute (pi, typeof (NullAllowedAttribute))){
					convs.AppendFormat ("\t\t\tvar ns{0} = {0} == null ? null : new NSString ({0});\n", pi.Name);
					disposes.AppendFormat ("\t\t\tif (ns{0} != null)\n\t\t\t\tns{0}.Dispose ();", pi.Name);
				} else {
					convs.AppendFormat ("\t\t\tvar ns{0} = new NSString ({0});\n", pi.Name);
					disposes.AppendFormat ("\t\t\tns{0}.Dispose ();\n", pi.Name);
				}
			}

			if (mai.Type.IsArray){
				Type etype = mai.Type.GetElementType ();
				if (etype == typeof (string)){
					if (null_allowed_override || HasAttribute (pi, typeof (NullAllowedAttribute))){
						convs.AppendFormat ("\t\t\tvar nsa_{0} = {0} == null ? null : NSArray.FromStrings ({0});\n", pi.Name);
						disposes.AppendFormat ("\t\t\tif (nsa_{0} != null)\n\t\t\t\tnsa_{0}.Dispose ();\n", pi.Name);
					} else {
						convs.AppendFormat ("\t\t\tvar nsa_{0} = NSArray.FromStrings ({0});\n", pi.Name);
						disposes.AppendFormat ("\t\t\tnsa_{0}.Dispose ();\n", pi.Name);
					}
				} else {
					if (null_allowed_override || HasAttribute (pi, typeof (NullAllowedAttribute))){
						convs.AppendFormat ("\t\t\tvar nsa_{0} = {0} == null ? null : NSArray.FromNSObjects ({0});\n", pi.Name);
						disposes.AppendFormat ("\t\t\tif (nsa_{0} != null)\n\t\t\t\tnsa_{0}.Dispose ();\n", pi.Name);
					} else {
						convs.AppendFormat ("\t\t\tvar nsa_{0} = NSArray.FromNSObjects ({0});\n", pi.Name);
						disposes.AppendFormat ("\t\t\tnsa_{0}.Dispose ();\n", pi.Name);
					}
				}
			}

			
			if (mai.Type.IsSubclassOf (typeof (Delegate))){
				string trampoline_name = MakeTrampoline (pi.ParameterType);
				string extra = "";
				bool null_allowed = HasAttribute (pi, typeof (NullAllowedAttribute));
				
				convs.AppendFormat ("\t\t\tBlockLiteral *block_ptr_{0};\n", pi.Name);
				convs.AppendFormat ("\t\t\tBlockLiteral block_{0};\n", pi.Name);
				if (null_allowed){
					convs.AppendFormat ("\t\t\tif ({0} == null){{\n", pi.Name);
					convs.AppendFormat ("\t\t\t\tblock_ptr_{0} = (BlockLiteral *) 0;\n", pi.Name);
					convs.AppendFormat ("\t\t\t}} else {{\n");
					extra = "\t";
				}
				convs.AppendFormat (extra + "\t\t\tblock_{0} = new BlockLiteral ();\n", pi.Name);
				convs.AppendFormat (extra + "\t\t\tblock_ptr_{0} = &block_{0};\n", pi.Name);
				convs.AppendFormat (extra + "\t\t\tblock_{0}.SetupBlock ({1}, {0});\n", pi.Name, trampoline_name);
				if (null_allowed)
					convs.AppendFormat ("\t\t\t}}");

				if (null_allowed){
					disposes.AppendFormat ("if (block_ptr_{0} != (BlockLiteral *) 0)\n", pi.Name);
				}
				disposes.AppendFormat (extra + "\t\t\tblock_ptr_{0}->CleanupBlock ();\n", pi.Name);
			}

			// Handle ByRef
			if (mai.Type.IsByRef && mai.Type.GetElementType ().IsValueType == false){
				print ("IntPtr {0}Ptr = Marshal.AllocHGlobal(4);", pi.Name);
				print ("Marshal.WriteInt32({0}Ptr, 0);", pi.Name);
				print ("");
				
				byRefPostProcessing.AppendLine();
				byRefPostProcessing.AppendFormat("\t\t\tIntPtr {0}Value = Marshal.ReadIntPtr({0}Ptr);", pi.Name);
				byRefPostProcessing.AppendLine();
				byRefPostProcessing.AppendFormat("\t\t\t{0} = {0}Value != IntPtr.Zero ? ({1})Runtime.GetNSObject({0}Value) : null;", pi.Name, mai.Type.Name.Replace("&", ""));
				byRefPostProcessing.AppendLine();
				byRefPostProcessing.AppendFormat("\t\t\tMarshal.FreeHGlobal({0}Ptr);", pi.Name);
				byRefPostProcessing.AppendLine();
			}
			// Insert parameter checking
			else if (!null_allowed_override && ParameterNeedsNullCheck (pi, mi)){
				print ("if ({0} == null)", pi.Name);
				print ("\tthrow new ArgumentNullException (\"{0}\");", pi.Name);
			}
		}

		foreach (var pi in mi.GetParameters ()) {
			RetainAttribute [] attr = (RetainAttribute []) pi.GetCustomAttributes (typeof (RetainAttribute), true);
			var ra = attr.Length > 0 ? attr[0] : null;
			if (ra != null) {
				if (!string.IsNullOrEmpty (ra.WrapName))
					print ("__mt_{0}_var = {1};", ra.WrapName, pi.Name);
				else
					print ("__mt_{1}_{2} = {2};", pi.ParameterType, mi.Name, pi.Name);
			}
			RetainListAttribute [] lattr = (RetainListAttribute []) pi.GetCustomAttributes (typeof (RetainListAttribute), true);
			var rla = lattr.Length > 0 ? lattr[0] : null;
			if (rla != null) {
				if (rla.Add)
					print ("__mt_{0}_var.Add ({1});", rla.WrapName, pi.Name);
				else
					print ("__mt_{0}_var.Remove ({1});", rla.WrapName, pi.Name);
			}
		}

		if (convs.Length > 0)
			print (sw, convs.ToString ());


		bool has_postget = HasAttribute (mi, typeof (PostGetAttribute));
		bool use_temp_return =
			(mi.Name != "Constructor" && (NeedStret (mi) || disposes.Length > 0 || has_postget) && mi.ReturnType != typeof (void)) ||
			(HasAttribute (mi, typeof (FactoryAttribute))) ||
			(assign != null && (IsWrappedType (mi.ReturnType) || (mi.ReturnType.IsArray && IsWrappedType (mi.ReturnType.GetElementType ())))) ||
			(mi.Name != "Constructor" && byRefPostProcessing.Length > 0);
		

		if (use_temp_return)
			print ("{0} ret;", FormatType (mi.DeclaringType, mi.ReturnType)); //  = new {0} ();"
		
		bool needs_temp = use_temp_return || disposes.Length > 0;
		if (virtual_method || mi.Name == "Constructor"){
			//print ("if (this.GetType () == typeof ({0})) {{", type.Name);
			if (external) {
				GenerateInvoke (false, mi, selector, args.ToString (), needs_temp, is_static);
			} else {
				if (BindThirdPartyLibrary && mi.Name == "Constructor"){
					print (init_binding_type);
				}
				
				print ("if (IsDirectBinding) {{", type.Name);
				indent++;
				GenerateInvoke (false, mi, selector, args.ToString (), needs_temp, is_static);
				indent--;
				print ("} else {");
				indent++;
				GenerateInvoke (true, mi, selector, args.ToString (), needs_temp, is_static);
				indent--;
				print ("}");
			}
		} else {
			GenerateInvoke (false, mi, selector, args.ToString (), needs_temp, is_static);
		}
		
		if (disposes.Length > 0)
			print (disposes.ToString ());
		if (assign != null && (IsWrappedType (mi.ReturnType) || (mi.ReturnType.IsArray && IsWrappedType (mi.ReturnType.GetElementType ()))))
			print ("{0} = ret;", assign);
		if (has_postget) {
			PostGetAttribute [] attr = (PostGetAttribute []) mi.GetCustomAttributes (typeof (PostGetAttribute), true);
			print ("#pragma warning disable 168");
			print ("var postget = {0};", attr [0].MethodName);
			print ("#pragma warning restore 168");
		}
		if (HasAttribute (mi, typeof (FactoryAttribute)))
			print ("ret.Release (); // Release implicit ref taken by GetNSObject");
		if (byRefPostProcessing.Length > 0)
			print (byRefPostProcessing.ToString ());
		if (use_temp_return)
			print ("return ret;");

		indent--;
	}

	Dictionary<string,object> generatedEvents = new Dictionary<string,object> ();
	Dictionary<string,object> generatedDelegates = new Dictionary<string,object> ();
	
	public void Generate (Type type)
	{
		string TypeName;
		object [] bindOnType = type.GetCustomAttributes (typeof (BindAttribute), true);
		if (bindOnType.Length > 0)
			TypeName = ((BindAttribute) bindOnType [0]).Selector;
		else
			TypeName = type.Name;

		string dir = Path.Combine (basedir, type.Namespace.Replace (MainPrefix + ".", ""));
		string file = TypeName + ".g.cs";

		if (!Directory.Exists (dir))
			Directory.CreateDirectory (dir);

		indent = 0;
		string output_file = Path.Combine (dir, file);
		GeneratedFiles.Add (output_file);
		using (var sw = new StreamWriter (output_file)){
			this.sw = sw;
			bool is_static_class = type.GetCustomAttributes (typeof (StaticAttribute), true).Length > 0;
			bool is_model = type.GetCustomAttributes (typeof (ModelAttribute), true).Length > 0;
			object [] btype = type.GetCustomAttributes (typeof (BaseTypeAttribute), true);
			BaseTypeAttribute bta = btype.Length > 0 ? ((BaseTypeAttribute) btype [0]) : null;
			Type base_type = bta != null ?  bta.BaseType : typeof (object);
			string objc_type_name = bta != null ? (bta.Name != null ? bta.Name : TypeName) : TypeName;
			Header (sw);

			print ("namespace {0} {{", type.Namespace);

			if (is_static_class){
				base_type = typeof (object);
			} else {
				print ("\t[Register(\"{0}\")]", objc_type_name);
			} 
			
			if (is_model)
				print ("\t[Model]");

			print ("\tpublic {0}partial class {1} {2} {{",
			       need_abstract.ContainsKey (type) ? "abstract " : "",
			       TypeName,
			       base_type != typeof (object) && TypeName != "NSObject" ? ": " + FormatType (type, base_type) : "");

			if (!is_model){
				foreach (var ea in selectors [type]){
					if (external || btouch)
						print ("\t\tstatic IntPtr {0} = Selector.GetHandle (\"{1}\");", SelectorField (ea), ea);
					else
						print ("\t\tstatic IntPtr {0} = Selector.sel_registerName (\"{1}\");", SelectorField (ea), ea);
				}
			}
			print ("");

			if (!is_static_class){
				print ("\t\tstatic IntPtr class_ptr = Class.GetHandle (\"{0}\");\n", objc_type_name);
				if (!is_model && !external) {
					print ("\t\tpublic {1} IntPtr ClassHandle {{ get {{ return class_ptr; }} }}\n", objc_type_name, TypeName == "NSObject" ? "virtual" : "override");
				}

				if (TypeName != "NSObject"){
					if (external) {
						sw.WriteLine ("\t\t[Export (\"init\")]\n\t\tpublic {0} () : base (NSObjectFlag.Empty)\n\t\t{{\n\t\t\t{1}Handle = {2}.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);\n\t\t\t\n\t\t}}\n",
							      TypeName, debug ? String.Format ("Console.WriteLine (\"{0}.ctor ()\");", TypeName) : "", MainPrefix);
					} else {
						sw.WriteLine ("\t\t[Export (\"init\")]\n\t\tpublic {0} () : base (NSObjectFlag.Empty)\n\t\t{{\n\t\t\t{1}{2}if (IsDirectBinding) {{\n\t\t\t\tHandle = {3}.ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.Init);\n\t\t\t}} else {{\n\t\t\t\tHandle = {3}.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.Init);\n\t\t\t}}\n\t\t}}\n",
							      TypeName,
							      BindThirdPartyLibrary ? init_binding_type + "\n\t\t\t" : "",
							      debug ? String.Format ("Console.WriteLine (\"{0}.ctor ()\");", TypeName) : "",
							      MainPrefix);
						sw.WriteLine ("\t\t[Export (\"initWithCoder:\")]\n\t\tpublic {0} (NSCoder coder) : base (NSObjectFlag.Empty)\n\t\t{{\n\t\t\t{1}{2}if (IsDirectBinding) {{\n\t\t\t\tHandle = {3}.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, Selector.InitWithCoder, coder.Handle);\n\t\t\t}} else {{\n\t\t\t\tHandle = {3}.ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.InitWithCoder, coder.Handle);\n\t\t\t}}\n\t\t}}\n",
							      TypeName,
							      BindThirdPartyLibrary ? init_binding_type + "\n\t\t\t" : "",
							      debug ? String.Format ("Console.WriteLine (\"{0}.ctor (NSCoder)\");", TypeName) : "",
							      MainPrefix);
					}
					sw.WriteLine ("\t\tpublic {0} (NSObjectFlag t) : base (t) {{}}\n", TypeName);
					sw.WriteLine ("\t\tpublic {0} (IntPtr handle) : base (handle) {{}}\n", TypeName);
				}
			}
			
			indent = 2;
			foreach (var mi in type.GetMethods (BindingFlags.Public | BindingFlags.Instance)){
				if (mi.IsSpecialName)
					continue;

#if RETAIN_AUDITING
				if (mi.Name.StartsWith ("Set"))
					foreach (ParameterInfo pi in mi.GetParameters ()) {
						if (IsWrappedType (pi.ParameterType) || pi.ParameterType.IsArray) {
							Console.WriteLine ("AUDIT: {0}", mi);
						}
					}
#endif

				if (HasAttribute (mi, typeof (AlphaAttribute)) && Alpha == false)
					continue;
				
				foreach (ParameterInfo pi in mi.GetParameters ())
					if (HasAttribute (pi, typeof (RetainAttribute))){
						print ("#pragma warning disable 168");
						print ("{0} __mt_{1}_{2};", pi.ParameterType, mi.Name, pi.Name);
						print ("#pragma warning enable 168");
					}

				string selector = null;
				bool virtual_method = false;
				object [] attr = mi.GetCustomAttributes (typeof (ExportAttribute), true);
				if (attr.Length != 1){
					attr = mi.GetCustomAttributes (typeof (BindAttribute), true);
					if (attr.Length != 1){
						Console.WriteLine ("No Export or Bind attribute defined on {0}.{1}", type, mi.Name);
						Environment.Exit (1);
					}
					BindAttribute ba = (BindAttribute) attr [0];
					selector = ba.Selector;
					virtual_method = ba.Virtual;
				} else {
					ExportAttribute ea = (ExportAttribute) attr [0];
					selector = ea.Selector;
					
					print ("[Export (\"{0}\")]", ea.Selector);
					virtual_method = mi.Name != "Constructor";
				}

				foreach (ObsoleteAttribute oa in mi.GetCustomAttributes (typeof (ObsoleteAttribute), false)) {
					print ("[Obsolete (\"{0}\", {1})]",
							oa.Message, oa.IsError ? "true" : "false");
				}

				bool is_static = HasAttribute (mi, typeof (StaticAttribute));
				if (is_static)
					virtual_method = false;

				bool is_abstract = HasAttribute (mi, typeof (AbstractAttribute));
				bool is_public = !HasAttribute (mi, typeof (InternalAttribute));
				bool is_override = HasAttribute (mi, typeof (OverrideAttribute));
				bool is_new = HasAttribute (mi, typeof (NewAttribute));
				bool is_sealed = HasAttribute (mi, typeof (SealedAttribute));
				bool is_unsafe = false;

				foreach (ParameterInfo pi in mi.GetParameters ())
					if (pi.ParameterType.IsSubclassOf (typeof (Delegate)))
						is_unsafe = true;

				print ("{0} {1}{2}{3}{4}{5}",
				       is_public ? "public" : "internal",
				       is_unsafe ? "unsafe " : "",
				       is_new ? "new " : "",
				       is_sealed ? "" : (is_abstract ? "abstract " : (virtual_method ? (is_override ? "override " : "virtual ") : (is_static ? "static " : ""))),
				       MakeSignature (mi),
				       is_abstract ? ";" : "");

				if (!is_abstract){
					print ("{");
					if (debug)
						print ("Console.WriteLine (\"In {0}\");", mi);
					
					if (is_model)
						print ("\tthrow new You_Should_Not_Call_base_In_This_Method ();");
					else
						GenerateMethodBody (type, mi, virtual_method, is_static, selector, false);
					print ("}\n");
				}
			}

			var field_exports = new List<PropertyInfo> ();
			foreach (var pi in type.GetProperties ()){
				if (HasAttribute (pi, typeof (AlphaAttribute)) && Alpha == false)
					continue;

				if (HasAttribute (pi, typeof (FieldAttribute))){
					field_exports.Add (pi);
					continue;
				}

				string wrap;
				var export = GetExportAttribute (pi, out wrap);
				bool is_static = HasAttribute (pi, typeof (StaticAttribute));
				bool is_abstract = HasAttribute (pi, typeof (AbstractAttribute));
				bool is_public = !HasAttribute (pi, typeof (InternalAttribute));
				bool is_override = HasAttribute (pi, typeof (OverrideAttribute));
				bool is_new = HasAttribute (pi, typeof (NewAttribute));
				bool is_sealed = HasAttribute (pi, typeof (SealedAttribute));
				bool is_unsafe = false;

				foreach (ObsoleteAttribute oa in pi.GetCustomAttributes (typeof (ObsoleteAttribute), false)) {
					print ("[Obsolete (\"{0}\", {1})]",
							oa.Message, oa.IsError ? "true" : "false");
				}

				if (pi.PropertyType.IsSubclassOf (typeof (Delegate)))
					is_unsafe = true;

				if (wrap != null){
					print ("{0} {1}{2}{3}{4} {5} {{",
					       is_public ? "public" : "internal",
					       is_unsafe ? "unsafe " : "",
					       is_new ? "new " : "",
					       (is_static ? "static " : ""),
					       FormatType (pi.DeclaringType,  pi.PropertyType),
					       pi.Name);
					indent++;
					if (pi.CanRead)
						print ("get {{ return {0} as {1}; }}", wrap, FormatType (pi.DeclaringType, pi.PropertyType));
					if (pi.CanWrite)
						print ("set {{ {0} = value; }}", wrap);
					indent--;
					print ("}\n");
					continue;
				}
				
				string var_name = string.Format ("__mt_{0}_var{1}", pi.Name, is_static ? "_static" : "");

				if ((IsWrappedType (pi.PropertyType) || (pi.PropertyType.IsArray && IsWrappedType (pi.PropertyType.GetElementType ()))))
					print ("{2}{0} {1};", pi.PropertyType, var_name, is_static ? "static " : "");
				print ("{0} {1}{2}{3}{4} {5} {{",
				       is_public ? "public" : "internal",
				       is_unsafe ? "unsafe " : "",
				       is_new ? "new " : "",
				       is_sealed ? "" : (is_static ? "static " : (is_abstract ? "abstract " : (is_override ? "override " : "virtual "))),
				       FormatType (pi.DeclaringType,  pi.PropertyType),
				       pi.Name);
				indent++;

				if (pi.CanRead){
					var getter = pi.GetGetMethod ();
					var ba = GetBindAttribute (getter);
					string sel = ba != null ? ba.Selector : export.Selector;
					
					if (export.ArgumentSemantic != ArgumentSemantic.None)
						print ("[Export (\"{0}\", ArgumentSemantic.{1})]", sel, export.ArgumentSemantic);
					else
						print ("[Export (\"{0}\")]", sel);
					if (is_abstract){
						print ("get; ");
					} else {
						print ("get {");
						if (debug)
							print ("Console.WriteLine (\"In {0}\");", pi.GetGetMethod ());
						if (is_model)
							print ("\tthrow new ModelNotImplementedException ();");
						else
							GenerateMethodBody (type, getter, !is_static, is_static, sel, false, var_name);
						print ("}\n");
					}
				}
				if (pi.CanWrite){
					var setter = pi.GetSetMethod ();
					var ba = GetBindAttribute (setter);
					bool null_allowed = HasAttribute (pi, typeof (NullAllowedAttribute)) || HasAttribute (setter, typeof (NullAllowedAttribute));
					string sel;

					if (ba == null){
						ExportAttribute setexport = MakeSetAttribute (export);
						sel = setexport.Selector;
					} else {
						sel = ba.Selector;
					}

					if (export.ArgumentSemantic != ArgumentSemantic.None)
						print ("[Export (\"{0}\", ArgumentSemantic.{1})]", sel, export.ArgumentSemantic);
					else
						print ("[Export (\"{0}\")]", sel);
					if (is_abstract){
						print ("set; ");
					} else {
						print ("set {");
						if (debug)
							print ("Console.WriteLine (\"In {0}\");", pi.GetSetMethod ());
						if (is_model)
							print ("\tthrow new ModelNotImplementedException ();");
						else {
							GenerateMethodBody (type, setter, !is_static, is_static, sel, null_allowed);
							if (!is_static && (IsWrappedType (pi.PropertyType) || (pi.PropertyType.IsArray && IsWrappedType (pi.PropertyType.GetElementType ()))))
								print ("\t{0} = value;", var_name);
						}
						print ("}");
					}
				}
				indent--;
				print ("}}\n", pi.Name);
			}

			if (field_exports.Count != 0){
				List <string> libraries = new List <string> ();

				foreach (var field_pi in field_exports){
					var fieldAttr = (FieldAttribute) field_pi.GetCustomAttributes (typeof (FieldAttribute), true) [0];
					string library_name; 

					if (fieldAttr.LibraryName != null)
						library_name = fieldAttr.LibraryName;
					else
						library_name = type.Namespace.Substring (MainPrefix.Length+1);

					if (!libraries.Contains (library_name)) {
						print ("static IntPtr {0}_libraryHandle = Dlfcn.dlopen (Constants.{0}Library, 0);", library_name);
						libraries.Add (library_name);
					}

					string fieldTypeName = FormatType (field_pi.DeclaringType, field_pi.PropertyType);
					// Value types we dont cache for now, to avoid Nullabel<T>
					if (!field_pi.PropertyType.IsValueType)
						print ("static {0} _{1};", fieldTypeName, field_pi.Name);

					print ("public static {0} {1} {{", fieldTypeName, field_pi.Name);
					indent++;
					print ("get {");
					indent++;
					if (field_pi.PropertyType == typeof (NSString)){
						print ("if (_{0} == null)", field_pi.Name);
						indent++;
						print ("_{0} = Dlfcn.GetStringConstant ({2}_libraryHandle, \"{1}\");", field_pi.Name, fieldAttr.SymbolName, library_name);
						indent--;
						print ("return _{0};", field_pi.Name);
					} else if (field_pi.PropertyType.Name == "NSArray"){
						print ("if (_{0} == null)", field_pi.Name);
						indent++;
						print ("_{0} = new NSArray (Dlfcn.GetIndirect ({2}_libraryHandle, \"{1}\"));", field_pi.Name, fieldAttr.SymbolName, library_name);
						indent--;
						print ("return _{0};", field_pi.Name);
					} else {
						if (field_pi.PropertyType == typeof (string))
							Console.WriteLine ("Unsupported type for Fields (string), you probably meant NSString");
						else
							Console.WriteLine ("Unsupported type for Fields: {0}", fieldTypeName);
						Environment.Exit (1);
					}
					
					indent--;
					print ("}");
					indent--;
					print ("}");
				}
			}
			
			var eventArgTypes = new Dictionary<string,ParameterInfo[]> ();
			var delegateTypes = new Dictionary<string,MethodInfo> ();
			
			if (bta.Events != null){
				if (bta.Delegates == null){
					Console.WriteLine ("In class {0} You specified the Events property, but did not bind those to names with Delegates", type.FullName);
					Environment.Exit (1);
				}
				
				print ("//");
				print ("// Events and properties from the delegate");
				print ("//\n");


				int delidx = 0;
				foreach (var dtype in bta.Events){
					string delName = bta.Delegates [delidx++];

					// The ensure method
					if (bta.KeepRefUntil == null)
						print ("_{0} Ensure{0} ()", dtype.Name);
					else {
						print ("static System.Collections.ArrayList instances;");
						print ("_{0} Ensure{0} (object oref)", dtype.Name);
					}
					
					print ("{"); indent++;
					print ("var del = {0};", delName);
					print ("if (del == null || (!(del is _{0}))){{", dtype.Name);
					print ("\tdel = new _{0} ({1});", dtype.Name, bta.KeepRefUntil == null ? "" : "oref");
					if (bta.KeepRefUntil != null){
						print ("\tif (instances == null) instances = new System.Collections.ArrayList ();");
						print ("\tif (!instances.Contains (this)) instances.Add (this);");
					}
					print ("\t{0} = del;", delName);
					print ("}");
					print ("return (_{0}) del;", dtype.Name);
					indent--; print ("}\n");
					
					print ("[Register]");
					print ("class _{0} : {1} {{ ", dtype.Name, RenderType (dtype));
					indent++;
					if (bta.KeepRefUntil != null){
						print ("object reference;");
						print ("public _{0} (object reference) {{ this.reference = reference; }}\n", dtype.Name);
					} else 
						print ("public _{0} () {{}}\n", dtype.Name);
						

					foreach (var mi in dtype.GetMethods ()){
						if (HasAttribute (mi, typeof (AlphaAttribute)) && Alpha == false)
							continue;

						// Skip property getter/setters
						if (mi.IsSpecialName && (mi.Name.StartsWith ("get_") || mi.Name.StartsWith ("set_")))
							continue;
						
						var pars = mi.GetParameters ();
						int minPars = bta.Singleton ? 0 : 1;

						if (pars.Length < minPars){
							Console.WriteLine ("Error, the delegate method {0}.{1} needs to take at least one parameter", dtype.FullName, mi.Name);
							Environment.Exit (1);
						}
						
						var sender = pars.Length == 0 ? "this" : pars [0].Name;

						if (mi.ReturnType == typeof (void)){
							if (bta.Singleton || mi.GetParameters ().Length == 1)
								print ("internal EventHandler {0};", PascalCase (mi.Name));
							else
								print ("internal EventHandler<{0}> {1};", GetEventArgName (mi), PascalCase (mi.Name));
						} else
							print ("internal {0} {1};", GetDelegateName (mi), PascalCase (mi.Name));
						
						print ("[Preserve (Conditional = true)]");
						print ("public override {0} {1} ({2})", RenderType (mi.ReturnType), mi.Name, RenderParameterDecl (pars));
						print ("{"); indent++;

						if (mi.Name == bta.KeepRefUntil)
							print ("instances.Remove (reference);");
						
						if (mi.ReturnType == typeof (void)){
							string eaname;
								
							if (pars.Length != minPars){
								eaname = GetEventArgName (mi);
								if (!generatedEvents.ContainsKey (eaname) && !eventArgTypes.ContainsKey (eaname)){
									eventArgTypes.Add (eaname, pars);
									generatedEvents.Add (eaname, pars);
								}
							} else
								eaname = "<NOTREACHED>";
							
							print ("if ({0} != null)", PascalCase (mi.Name));
							print ("	{1} ({0}, {2});",
							       sender, PascalCase (mi.Name),
							       pars.Length == minPars ? "EventArgs.Empty" : String.Format ("new {0} ({1})", eaname, RenderArgs (pars.Skip (minPars))));
						} else {
							var delname = GetDelegateName (mi);

							if (!generatedDelegates.ContainsKey (delname) && !delegateTypes.ContainsKey (delname)){
								generatedDelegates.Add (delname, null);
								delegateTypes.Add (delname, mi);
							}
							
							print ("if ({0} != null)", PascalCase (mi.Name));
							print ("	return {0} ({1}{2});",
							       PascalCase (mi.Name), sender,
							       pars.Length == minPars ? "" : String.Format (", {0}", RenderArgs (pars.Skip (1))));

							var def = GetDefaultValue (mi);
							if ((def is string) && ((def as string) == "null") && mi.ReturnType.IsValueType)
								print ("throw new Exception ();");
							else {
								foreach (var j in pars){
									if (j.ParameterType.IsByRef && j.IsOut){
										print ("{0} = null;", j.Name);
									}
								}
										
								print ("return {0};", def);
							}
						}
						
						indent--;
						print ("}\n");
					}
					indent--;
					print ("}");
				}
				print ("");

				
				// Now add the instance vars and event handlers
				foreach (var dtype in bta.Events){
					foreach (var mi in dtype.GetMethods ()){
						if (HasAttribute (mi, typeof (AlphaAttribute)) && Alpha == false)
							continue;

						// Skip property getter/setters
						if (mi.IsSpecialName && (mi.Name.StartsWith ("get_") || mi.Name.StartsWith ("set_")))
							continue;

						string ensureArg = bta.KeepRefUntil == null ? "" : "this";
						
						if (mi.ReturnType == typeof (void)){
							if (bta.Singleton && mi.GetParameters ().Length == 0 || mi.GetParameters ().Length == 1)
								print ("public event EventHandler {0} {{", CamelCase (GetEventName (mi)));
							else 
								print ("public event EventHandler<{0}> {1} {{", GetEventArgName (mi), CamelCase (GetEventName (mi)));
							print ("\tadd {{ Ensure{0} ({1}).{2} += value; }}", dtype.Name, ensureArg, PascalCase (mi.Name));
							print ("\tremove {{ Ensure{0} ({1}).{2} -= value; }}", dtype.Name, ensureArg, PascalCase (mi.Name));
							print ("}\n");
						} else {
							print ("public {0} {1} {{", GetDelegateName (mi), CamelCase (mi.Name));
							print ("\tget {{ return Ensure{0} ({1}).{2}; }}", dtype.Name, ensureArg, PascalCase (mi.Name));
							print ("\tset {{ Ensure{0} ({1}).{2} = value; }}", dtype.Name, ensureArg, PascalCase (mi.Name));
							print ("}\n");
						}
					}
				}
			}
			
			indent--;
			print ("");

			//
			// Now the trampolines
			//
			foreach (var ti in trampolines.Values){
				print ("internal delegate void {0} ({1});", ti.DelegateName, ti.Parameters);
				print ("static {0} {1} = new {0} ({2});", ti.DelegateName, ti.StaticName, ti.TrampolineName);
				print ("[MonoPInvokeCallback (typeof ({0}))]", ti.DelegateName);
				print ("static unsafe void {0} ({1}) {{", ti.TrampolineName, ti.Parameters);
				indent++;
				print ("var descriptor = (BlockLiteral *) block;");
				print ("var del = ({0}) (descriptor->global_handle != IntPtr.Zero ? GCHandle.FromIntPtr (descriptor->global_handle).Target : GCHandle.FromIntPtr (descriptor->local_handle).Target);", ti.UserDelegate);
				print ("del ({0});", ti.Invoke);
				indent--;
				print ("}");
				print ("");
			}
			
			print ("}} /* class {0} */", TypeName);

			//
			// Copy delegates from the API files into the output if they were delcared there
			//
			var rootAssembly = types [0].Assembly;
			foreach (var deltype in trampolines.Keys){
				if (deltype.Assembly != rootAssembly)
					continue;

				if (delegates_emitted.ContainsKey (deltype))
					continue;

				delegates_emitted [deltype] = deltype;

				// This formats the delegate 
				var delmethod = deltype.GetMethod ("Invoke");
				var del = new StringBuilder ("public delegate ");
				del.Append (FormatType (type, delmethod.ReturnType));
				del.Append (" ");
				del.Append (deltype.Name);
				del.Append (" (");
				var delpars = delmethod.GetParameters ();
				for (int dmi = 0; dmi < delpars.Length; dmi++){
					Type ptype = delpars [dmi].ParameterType;
					string modifier = "";
					if (ptype.IsByRef){
						if (delpars [dmi].IsOut)
							modifier = "out ";
						else
							modifier = "ref ";
						ptype = ptype.GetElementType ();
					}
					
					del.AppendFormat ("{0}{1} {2}{3}", modifier, FormatType (type, ptype), delpars [dmi].Name, dmi+1 == delpars.Length ? "" : ", ");
				}
				del.Append (");");
				print (del.ToString ());
			}
			trampolines.Clear ();

			
			if (eventArgTypes.Count > 0){
				print ("\n");
				print ("//");
				print ("// EventArgs classes");
				print ("//");
			}
			// Now add the EventArgs classes
			foreach (var eaclass in eventArgTypes.Keys){
				if (skipGeneration.ContainsKey (eaclass)){
					continue;
				}
				int minPars = bta.Singleton ? 0 : 1;
				
				var pars = eventArgTypes [eaclass];

				print ("public partial class {0} : EventArgs {{", eaclass); indent++;
				print ("public {0} ({1})", eaclass, RenderParameterDecl (pars.Skip (1)));
				print ("{");
				indent++;
				foreach (var p in pars.Skip (minPars)){
					print ("this.{0} = {1};", GetPublicParameterName (p), p.Name);
				}
				indent--;
				print ("}");
				
				// Now print the properties
				foreach (var p in pars.Skip (minPars)){
					print ("public {0} {1} {{ get; set; }}", RenderType (p.ParameterType), GetPublicParameterName (p));
				}
				indent--; print ("}\n");
			}

			if (delegateTypes.Count > 0){
				print ("\n");
				print ("//");
				print ("// Delegate classes");
				print ("//");
			}
			// Now add the delegate declarations
			foreach (var delname in delegateTypes.Keys){
				var mi = delegateTypes [delname];
				
				print ("public delegate {0} {1} ({2});",
				       RenderType (mi.ReturnType),
				       delname,
				       RenderParameterDecl (mi.GetParameters ()));
			}
			
			indent--;
			print ("}");
		}

	}

	//
	// Support for the automatic delegate/event generation
	//
	string RenderParameterDecl (IEnumerable<ParameterInfo> pi)
	{
		return String.Join (", ", pi.Select (p => 
					(p.ParameterType.IsByRef ? (p.IsOut ? "out " : "ref ") + RenderType (p.ParameterType.GetElementType ())
					: RenderType (p.ParameterType)) + " " + p.Name).ToArray ());
	}

	string GetPublicParameterName (ParameterInfo pi)
	{
		object [] attrs = pi.GetCustomAttributes (typeof (EventNameAttribute), true);
		if (attrs.Length == 0)
			return CamelCase (pi.Name);

		var a = (EventNameAttribute) attrs [0];
		return CamelCase (a.EvtName);
	}
	
	string RenderArgs (IEnumerable<ParameterInfo> pi)
	{
		return String.Join (", ", pi.Select (p => (p.ParameterType.IsByRef ? (p.IsOut ? "out " : "ref ") : "")+ p.Name).ToArray ());
	}

	string CamelCase (string ins)
	{
		return Char.ToUpper (ins [0]) + ins.Substring (1);
	}

	string PascalCase (string ins)
	{
		return Char.ToLower (ins [0]) + ins.Substring (1);
	}

	Dictionary<string,bool> skipGeneration = new Dictionary<string,bool> ();
	string GetEventName (MethodInfo mi)
	{
		var a = GetAttribute (mi, typeof (EventNameAttribute));
		if (a == null)
			return mi.Name;
		var ea = (EventNameAttribute) a;
		
		return ea.EvtName;
	}
	string GetEventArgName (MethodInfo mi)
	{
		if (mi.GetParameters ().Length == 1)
			return "EventArgs";
		
		var a = GetAttribute (mi, typeof (EventArgsAttribute));
		if (a == null){
			Console.WriteLine ("The delegate method {0}.{1} is missing the [EventArgs] attribute (has {2} parameters)", mi.DeclaringType.FullName, mi.Name, mi.GetParameters ().Length);
			throw new Exception ();
		}
		var ea = (EventArgsAttribute) a;
		if (ea.SkipGeneration){
			skipGeneration [ea.FullName ? ea.ArgName : ea.ArgName + "EventArgs"] = true;
		}
		
		if (ea.FullName)
			return ea.ArgName;

		return ea.ArgName + "EventArgs";
	}

	string GetDelegateName (MethodInfo mi)
	{
		var a = GetAttribute (mi, typeof (EventArgsAttribute));
		if (a == null){
			Console.WriteLine ("The delegate method {0}.{1} is missing the [EventArgs] attribute", mi.DeclaringType.FullName, mi.Name);
			Environment.Exit (1);
		}
		return ((EventArgsAttribute) a).ArgName;
	}
	
	object GetDefaultValue (MethodInfo mi)
	{
		var a = GetAttribute (mi, typeof (DefaultValueAttribute));
		if (a == null){
			a = GetAttribute (mi, typeof (DefaultValueFromArgumentAttribute));
			if (a != null){
				var fvfa = (DefaultValueFromArgumentAttribute) a;
				return fvfa.Argument;
			}
			
			Console.WriteLine ("The delegate method {0}.{1} is missing the [DefaultValue] attribute", mi.DeclaringType.FullName, mi.Name);
			Environment.Exit (1);
		}
		var def = ((DefaultValueAttribute) a).Default;
		if (def == null)
			return "null";

		if (def.GetType ().FullName == "System.Drawing.RectangleF")
			return "System.Drawing.RectangleF.Empty";
		
		if (def is bool)
			return (bool) def ? "true" : "false";

		if (def is Enum)
			return def.GetType ().FullName + "." + def;

		return def;
	}
	
	string RenderType (Type t)
	{
		if (!t.IsEnum){
			switch (Type.GetTypeCode (t)){
			case TypeCode.Char:
				return "char";
			case TypeCode.String:
				return "string";
			case TypeCode.Int32:
				return "int";
			case TypeCode.UInt32:
				return "uint";
			case TypeCode.Int64:
				return "long";
			case TypeCode.UInt64:
				return "ulong";
			case TypeCode.Single:
				return "float";
			case TypeCode.Double:
				return "double";
			case TypeCode.Decimal:
				return "decimal";
			case TypeCode.SByte:
				return "sbyte";
			case TypeCode.Byte:
				return "byte";
			case TypeCode.Boolean:
				return "bool";
			}
		}
		string ns = t.Namespace;
		if (implicit_ns.Contains (ns))
			return t.Name;
		else
			return t.FullName;
		
	}
	
}
