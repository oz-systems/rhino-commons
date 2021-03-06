﻿#region license

// Copyright (c) 2005 - 2007 Ayende Rahien (ayende@ayende.com)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Ayende Rahien nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Rhino.Commons.Binsor
{
	public static class BinsorScript
	{
		public static BinsorFileInstaller FromFile(string fileName)
		{
			return new BinsorFileInstaller(fileName);
		}

        public static BinsorResourceInstaller FromResource(string uri)
        {
            return new BinsorResourceInstaller(uri);
        }
	    
		public static BinsorStreamInstaller FromStream(Stream stream)
		{
			return new BinsorStreamInstaller(stream);
		}

		public static BinsorStreamInstaller Inline(string script)
		{
			return FromStream(new MemoryStream(ASCIIEncoding.ASCII.GetBytes(script)));
		}		
		
		public static BinsorRunnerInstaller FromCompiledAssembly(string assemblyName)
		{
			return FromCompiledAssembly(LoadAssembly(assemblyName));
		}

		public static BinsorRunnerInstaller FromCompiledAssembly(Assembly assembly)
		{
			foreach (var type in assembly.GetExportedTypes())
			{
				if (type.IsClass && type.IsAbstract == false &&
					typeof(AbstractConfigurationRunner).IsAssignableFrom(type))
				{
					var runner = (AbstractConfigurationRunner)Activator.CreateInstance(type);
					return FromRunner(runner);
				}
			}
			
			throw new ArgumentException(
				string.Format("Assembly {0} does not appear to be a Binsor assembly",
				assembly.FullName));
		}

		public static BinsorRunnerInstaller FromRunner(AbstractConfigurationRunner runner)
		{
			return new BinsorRunnerInstaller(runner);
		}

		private static Assembly LoadAssembly(string assemblyName)
		{
			var extension = Path.GetExtension(assemblyName);

			if (extension == ".dll" || extension == ".exe")
			{
				string path = Path.GetDirectoryName(assemblyName);

				return path == string.Empty ||
					   Path.IsPathRooted(path) == false || 
					   path == AppDomain.CurrentDomain.BaseDirectory
					? Assembly.Load(Path.GetFileNameWithoutExtension(assemblyName)) 
					: Assembly.LoadFile(assemblyName);
			}
			else
			{
				return Assembly.Load(assemblyName);
			}
		}
	}
}
