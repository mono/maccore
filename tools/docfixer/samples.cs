using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

using Ionic.Zip;

public struct SampleDesc {
	public string ID { get; set; }
	public string FullTypeName { get; set; }
	public string DocumentationFilePath { get; set; }
	public string Language { get; set; } // e.g 'obj-c' when taken from Apple otherwise a value understood by mdoc like 'C#'
}

public class SampleRepository
{
	ZipFile archive;
	HashAlgorithm hasher = MD5.Create ();
	HashSet<string> validFiles = new HashSet<string> ();
	Dictionary<string, SampleDesc> index = new Dictionary<string, SampleDesc> ();

	public SampleRepository (string name) : this (new ZipFile (name.EndsWith (".zip") ? name : name + ".zip"))
	{
		
	}

	protected SampleRepository (ZipFile archive)
	{
		this.archive = archive;
		if (archive.ContainsEntry ("index.xml"))
			index = ((ICollection<SampleDesc>)IndexSerializer.Deserialize (archive["index.xml"].OpenReader ())).ToDictionary (sd => sd.ID, sd => sd);
	}

	// Returns an id that can be used to register the sample position in the documentation flow
	public string RegisterSample (string source, SampleDesc desc)
	{
		var hash = StringHash (source);
		validFiles.Add (hash);
		desc.ID = hash;

		if (!archive.ContainsEntry (hash)) {
			archive.AddEntry (hash, source);
			index[hash] = desc;
		}

		return hash;
	}

	public string GetSampleFromID (string id, out SampleDesc desc)
	{
		desc = new SampleDesc ();
		var entry = archive[id];

		if (entry == null)
			return null;
		desc = index[id];
		using (var stream = entry.OpenReader ())
			return new StreamReader (stream).ReadToEnd ();
	}

	public string GetSampleFromContent (string content, out SampleDesc desc)
	{
		return GetSampleFromID (StringHash (content), out desc);
	}

	public static SampleRepository LoadFrom (string file)
	{
		var samples = new SampleRepository (ZipFile.Read (file));
		return samples;
	}

	public void Close (bool removeOldEntries)
	{
		// See if we have any stale file
		if (removeOldEntries) {
			var list = new List<ZipEntry> (archive.Entries);
			foreach (var entry in list) {
				if (!validFiles.Contains (entry.FileName))
					archive.RemoveEntry (entry);
			}
		}
		// Serialize index
		var writer = new StringWriter ();
		IndexSerializer.Serialize (writer, new List<SampleDesc> (index.Values));
		archive.UpdateEntry ("index.xml", writer.ToString ());

		archive.Save ();
	}

	public IEnumerable<string> AllIDs {
		get {
			return index.Keys;
		}
	}

	string StringHash (String input)
	{
		// TODO: Reuse byte array
		return hasher.ComputeHash (Encoding.UTF8.GetBytes (input)).Select (b => String.Format("{0:X2}", b)).Aggregate (string.Concat);
	}

	XmlSerializer IndexSerializer {
		get {
			return new XmlSerializer (typeof (List<SampleDesc>));
		}
	}

#if STANDALONE
	public static void Main (string[] args)
	{
		string samplesPath = args[0];
		string outDir = args[1];

		if (!File.Exists (samplesPath)) {
			Console.WriteLine ("Couldn't find samples repository at {0}");
			return;
		}

		SampleRepository samples = SampleRepository.LoadFrom (samplesPath);

		if (!Directory.Exists (outDir))
			Directory.CreateDirectory (outDir);

		foreach (var id in samples.AllIDs) {
			SampleDesc desc;
			var content = samples.GetSampleFromID (id, out desc);

			var type = desc.FullTypeName.Substring (desc.FullTypeName.LastIndexOf ('.') + 1);
			var ns = desc.FullTypeName.Substring (0, desc.FullTypeName.LastIndexOf ('.'));

			Console.WriteLine ("Processing {0}::{1}", ns, type);
			var path = Path.Combine (outDir, ns, type);
			if (!Directory.Exists (path))
				Directory.CreateDirectory (path);
			if (!Directory.Exists (path))
				Directory.CreateDirectory (path);
			path = Path.Combine (path, id + ".native");

			File.WriteAllText (path, content);
		}
	}
#endif
}