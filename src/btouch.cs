//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009-2010 Novell, Inc.
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
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using Mono.Options;

class BindingTouch {
#if MONOMAC
	static string baselibdll = "MonoMac.dll";
	static string RootNS = "MonoMac";
	static Type CoreObject = typeof (MonoMac.Foundation.NSObject);
	static string tool_name = "bmac";
	static string compiler = "gmcs";
#else
	static string baselibdll = "/Developer/MonoTouch/usr/lib/mono/2.1/monotouch.dll";
	static string RootNS = "MonoTouch";
	static Type CoreObject = typeof (MonoTouch.Foundation.NSObject);
	static string tool_name = "btouch";
	static string compiler = "/Developer/MonoTouch/usr/bin/smcs";
#endif

	static void ShowHelp (OptionSet os)
	{
		Console.WriteLine ("{0} - Mono Objective-C API binder", tool_name);
		Console.WriteLine ("Usage is: {0} [options] API file [extra source files]", tool_name);
		
		os.WriteOptionDescriptions (Console.Out);
		
	}
	
	static int Main (string [] args)
	{
		bool show_help = false;
		bool alpha = false;
		string basedir = null;
		string tmpdir = null;
		string ns = null;
		string outfile = null;
		bool delete_temp = true, debug = false;
		bool verbose = false;
		bool unsafef = false;
		bool external = false;
		bool pmode = true;
		List<string> sources;
		var references = new List<string> ();
		var libs = new List<string> ();
		var core_sources = new List<string> ();
		var defines = new List<string> ();
		bool binding_third_party = true;
		string generate_file_list = null;
		
		var os = new OptionSet () {
			{ "h|?|help", "Displays the help", v => show_help = true },
			{ "a", "Include alpha bindings", v => alpha = true },
			{ "outdir=", "Sets the output directory for the temporary binding files", v => { basedir = v; }},
			{ "o|out=", "Sets the name of the output library", v => outfile = v },
			{ "tmpdir=", "Sets the working directory for temp files", v => { tmpdir = v; delete_temp = false; }},
			{ "debug", "Generates a debugging build of the binding", v => debug = true },
			{ "sourceonly=", "Only generates the source", v => generate_file_list = v },
			{ "ns=", "Sets the namespace for storing helper classes", v => ns = v },
			{ "unsafe", "Sets the unsafe flag for the build", v=> unsafef = true },
			{ "core", "Use this to build monomac.dll", v => binding_third_party = false },
			{ "r=", "Adds a reference", v => references.Add (v) },
			{ "lib=", "Adds the directory to the search path for the compiler", v => libs.Add (v) },
			{ "d=", "Defines a symbol", v => defines.Add (v) },
			{ "s=", "Adds a source file required to build the API", v => core_sources.Add (v) },
			{ "v", "Sets verbose mode", v => verbose = true },
			{ "e", "Sets external mode", v => external = true },
			{ "p", "Sets private mode", v => pmode = false },
			{ "baselib=", "Sets the base library", v => baselibdll = v },
		};

		try {
			sources = os.Parse (args);
		} catch (Exception e){
			Console.Error.WriteLine ("{0}: {1}", tool_name, e.Message);
			Console.Error.WriteLine ("see {0} --help for more information", tool_name);
			return 1;
		}
		if (show_help || sources.Count == 0){
			Console.WriteLine ("Error: no api file provided");
			ShowHelp (os);
			return 0;
		}

		if (alpha)
			defines.Add ("ALPHA");
		
		if (tmpdir == null)
			tmpdir = GetWorkDir ();

		if (outfile == null)
			outfile = Path.GetFileNameWithoutExtension (sources [0]) + ".dll";

		string refs = (references.Count > 0 ? "-r:" + String.Join (" -r:", references.ToArray ()) : "");
		string paths = (libs.Count > 0 ? "-lib:" + String.Join (" -lib:", libs.ToArray ()) : "");
			
		try {
			var api_file = sources [0];
			var tmpass = Path.Combine (tmpdir, "temp.dll");

			// -nowarn:436 is to avoid conflicts in definitions between core.dll and the sources
			var cargs = String.Format ("-unsafe -target:library {0} -nowarn:436 -out:{1} -r:{2} {3} {4} {5} -r:{6} {7} {8}",
						   string.Join (" ", sources.ToArray ()),
						   tmpass, Environment.GetCommandLineArgs ()[0],
						   string.Join (" ", core_sources.ToArray ()), refs, unsafef ? "-unsafe" : "",
						   baselibdll, string.Join (" ", defines.Select (x=> "-define:" + x).ToArray ()), paths);

			var si = new ProcessStartInfo (compiler, cargs) {
				UseShellExecute = false,
			};
			if (verbose)
				Console.WriteLine ("{0} {1}", si.FileName, si.Arguments);
			
			var p = Process.Start (si);
			p.WaitForExit ();
			if (p.ExitCode != 0){
				Console.WriteLine ("{0}: API binding contains errors.", tool_name);
				return 1;
			}

			Assembly a = null, baselib;
			try {
				a = Assembly.LoadFrom (tmpass);
			} catch (Exception e) {
				if (verbose)
					Console.WriteLine (e);
				
				Console.Error.WriteLine ("Error loading API definition from {0}", tmpass);
				return 1;
			}
			try {
				baselib = Assembly.LoadFrom (baselibdll);
			} catch (Exception e){
				if (verbose)
					Console.WriteLine (e);

				Console.Error.WriteLine ("Error loading base library {0}", baselibdll);
				return 1;
			}

			var types = new List<Type> ();
			foreach (var t in a.GetTypes ()){
				if (t.GetCustomAttributes (typeof (BaseTypeAttribute), true).Length > 0)
					types.Add (t);
			}
					
			var g = new Generator (pmode, external, debug, types.ToArray ()){
				MessagingNS = ns == null ? Path.GetFileNameWithoutExtension (api_file) : ns,
				CoreMessagingNS = RootNS + ".ObjCRuntime",
				BindThirdPartyLibrary = binding_third_party,
				CoreNSObject = CoreObject,
				BaseDir = basedir != null ? basedir : tmpdir,
#if MONOMAC
				OnlyX86 = true,
#endif
				Alpha = alpha
			};
					
			foreach (var mi in baselib.GetType (RootNS + ".ObjCRuntime.Messaging").GetMethods ()){
				if (mi.Name.IndexOf ("_objc_msgSend") != -1)
					g.RegisterMethodName (mi.Name);
			}
			
			g.Go ();

			if (generate_file_list != null){
				using (var f = File.CreateText (generate_file_list)){
					g.GeneratedFiles.ForEach (x => f.WriteLine (x));
				}
				return 0;
			}
			
			cargs = String.Format ("-unsafe -target:library -out:{0} -r:{6} {1} {2} {3} {4} {5} -r:{6}",
					       outfile,
					       String.Join (" ", g.GeneratedFiles.ToArray ()),
					       String.Join (" ", core_sources.ToArray ()),
					       String.Join (" ", sources.Skip (1).ToArray ()),
					       refs, unsafef ? "-unsafe" : "", baselibdll);

			si = new ProcessStartInfo (compiler, cargs) {
				UseShellExecute = false,
			};
			if (verbose)
				Console.WriteLine ("{0} {1}", si.FileName, si.Arguments);
			p = Process.Start (si);
			p.WaitForExit ();
			if (p.ExitCode != 0){
				Console.WriteLine ("{0}: API binding contains errors.", tool_name);
				return 1;
			}
			
		} finally {
			if (delete_temp)
				Directory.Delete (tmpdir, true);
		}
		return 0;
	}

	static string GetWorkDir ()
	{
		while (true){
			string p = Path.Combine (Path.GetTempPath(), Path.GetRandomFileName());
			if (Directory.Exists (p))
				continue;
			
			var di = Directory.CreateDirectory (p);
			return di.FullName;
		}
	}
}

