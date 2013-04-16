//
// Copyright (c) 2013 Jasper Seidel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace KSP_Mod_Manager
{
    class KspPackage
    {
        private FileInfo m_Path = null;
        private string m_RootDir = "";

        private static readonly char s_ZipDirectorySeparator = '/';
        private static readonly string[] s_ValidDestinationFolders = { "Internals", "Parts", "PluginData", "Plugins", "Resources", "Ships", "sounds" };
        private static readonly Regex s_ValidDestinationRegex;

        static KspPackage()
        {
            s_ValidDestinationRegex = new Regex(string.Format("(^|{0})({1}){0}", s_ZipDirectorySeparator, string.Join("|", s_ValidDestinationFolders)));
        }
        
        public KspPackage(string a_Filename)
        {
            m_Path = new FileInfo(a_Filename);
            m_RootDir = GetArchiveRootDir();
        }

        public override string ToString()
        {
            return m_Path.Name;
        }

        public string Filename
        {
            get { return m_Path.Name; }
        }
        
        public void MoveTo(string a_DestinationDirectory)
        {
            var filename = Path.GetFileName(m_Path.FullName);
            var destinationFile = Path.Combine(a_DestinationDirectory, filename);
            m_Path.MoveTo(destinationFile);
        }

        private string GetArchiveRootDir()
        {
            using (var za = ZipFile.OpenRead(m_Path.FullName))
            {
                var first_dir = za.Entries.First(entry => entry.Length == 0 && s_ValidDestinationRegex.IsMatch(entry.FullName));
                int i = 0;
                var parts = first_dir.FullName.TrimEnd(s_ZipDirectorySeparator).Split(s_ZipDirectorySeparator);
                for (; i < parts.Length; i++)
                {
                    var part = parts[i];
                    if (s_ValidDestinationFolders.Contains(part, new Comparer<string>((a, b) => a.Equals(b, StringComparison.InvariantCultureIgnoreCase))))
                    {
                        break;
                    }
                }
                if (i == 0) // we have no root dir in the zip
                {
                    return "";
                }
                return string.Join(s_ZipDirectorySeparator.ToString(), parts.Take(i)) + s_ZipDirectorySeparator;
            }
        }

        public IEnumerable<string> Install(string destination)
        {
            /*try
            {*/
                var installed_files = new List<string>();
                using (var za = ZipFile.OpenRead(m_Path.FullName))
                {
                    foreach (var file in za.Entries.Where(entry => entry.Length > 0))
                    {
                        if (!file.FullName.StartsWith(m_RootDir)) continue;
                        var relative_path = file.FullName.Substring(m_RootDir.Length);
                        if (relative_path.Count(c => c == s_ZipDirectorySeparator) == 0) continue; // don't place files in ksp root dir
                        if (s_ValidDestinationFolders.Contains(relative_path.Split(s_ZipDirectorySeparator)[0], new Comparer<string>((a, b) => a.Equals(b, StringComparison.InvariantCultureIgnoreCase))))
                        {
                            relative_path = relative_path.Replace(s_ZipDirectorySeparator, Path.DirectorySeparatorChar);
                            var directory = Path.GetDirectoryName(relative_path);

                            if (!Directory.Exists(Path.Combine(destination, directory)))
                            {
                                Directory.CreateDirectory(Path.Combine(destination, directory));
                            }
                            if (!File.Exists(Path.Combine(destination, relative_path)))
                            {
                                file.ExtractToFile(Path.Combine(destination, relative_path));
                            }
                            installed_files.Add(relative_path);
                        }
                    }
                }
                return installed_files;
            /*}
            catch (Exception e)
            {
                MessageBox.Show(string.Format("Error installing mod: {0}", e.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }*/
        }

        class Comparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _comparer;

            public Comparer(Func<T, T, bool> comparer)
            {
                if (comparer == null)
                    throw new ArgumentNullException("comparer");

                _comparer = comparer;
            }

            public bool Equals(T x, T y)
            {
                return _comparer(x, y);
            }

            public int GetHashCode(T obj)
            {
                return obj.ToString().ToLower().GetHashCode();
            }
        }
    }
}
