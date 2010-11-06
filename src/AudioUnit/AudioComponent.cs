//
// AudioComponent.cs: AudioComponent wrapper class
//
// Author:
//   AKIHIRO Uehara (u-akihiro@reinforce-lab.com)
//
// Copyright 2010 Reinforce Lab.
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
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MonoMac.ObjCRuntime;
using MonoMac.AudioToolbox;

namespace MonoMac.AudioUnit
{
	public class AudioComponent : INativeObject, IDisposable {
		#region Variables
		internal IntPtr handle;
		#endregion

		#region Properties
		public IntPtr Handle { get { return handle; } }
		#endregion

		#region Constructor
		private AudioComponent(IntPtr handle)
		{ 
			this.handle = handle;
		}
		#endregion
			
		#region public methods
		public static AudioComponent FindNextComponent(AudioComponent cmp, AudioComponentDescription cd)
		{
			// Getting component hanlder
			IntPtr handle;
			if (cmp == null)
				handle = AudioComponentFindNext(IntPtr.Zero, cd);
			else
				handle = AudioComponentFindNext(cmp.Handle, cd);
			
			// creating an instance
			if (handle != IntPtr.Zero)
				return new AudioComponent (handle);
			else
				return null;
		}
		
		public static AudioComponent FindComponent (AudioComponentDescription cd)
		{
			return FindNextComponent(null, cd);
		}
		#endregion

		#region IDisposable メンバ (Members)
		public void Dispose()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public virtual void Dispose (bool disposing)
		{			
			if (handle != IntPtr.Zero){
				AudioComponentInstanceDispose(handle);
				handle = IntPtr.Zero;
			}
		}

		~AudioComponent ()
		{
			Dispose (false);
		}
		#endregion

		#region Inteop
		[DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "AudioComponentFindNext")]
		static extern IntPtr AudioComponentFindNext(IntPtr inComponent, AudioComponentDescription inDesc);

		[DllImport(MonoMac.Constants.AudioToolboxLibrary, EntryPoint = "AudioComponentInstanceDispose")]
		static extern int AudioComponentInstanceDispose(IntPtr inInstance);
		#endregion
    }
}
