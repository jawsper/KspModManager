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

using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace KSP_Mod_Manager
{
    class KspMods
    {
        private SQLiteConnection db;

        private string m_ModPath = "";
        private static readonly string s_InstalledModsCache = "Installed";

        private string ModPath { get { return m_ModPath; } }
        private string TempPath { get { return Path.Combine(m_ModPath, "temp"); } }
        private string InstalledModCache { get { return Path.Combine(m_ModPath, s_InstalledModsCache); } }

        private string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public KspMods(string a_InstallationDirectory, string a_ModPath )
        {
            InstallationDirectory = a_InstallationDirectory;
            ModDirectory = a_ModPath;

            db = new SQLiteConnection("kmm.sqlite");

            // make sure the table exists
            if (db.GetTableInfo("KMMInfo").Count == 0)
            {
                db.CreateTable<KMMInfo>();
            }

            var tables = new Type[] { typeof(InstalledMods), typeof(ModFiles), typeof(InstalledFiles) };

            foreach (var table in tables)
            {
                var info = db.GetTableInfo(table.Name);
                if (info.Count == 0)
                    db.CreateTable(table);
            }

            // oh noez it does not match
            if (db.Table<KMMInfo>().Count() == 0 || db.Table<KMMInfo>().First().Version != GetAssemblyVersion())
            {
                // salvage data
                var installed_mods = db.Table<InstalledMods>().ToList();
                db.DropTable<InstalledMods>();
                db.CreateTable<InstalledMods>();
                db.InsertAll(installed_mods);
                
                var mod_files = db.Table<ModFiles>().ToList();
                db.DropTable<ModFiles>();
                db.CreateTable<ModFiles>();
                db.InsertAll(mod_files);
                
                var installed_files = db.Table<InstalledFiles>().ToList();
                db.DropTable<InstalledFiles>();
                db.CreateTable<InstalledFiles>();
                db.InsertAll(installed_files);
            }

            // make sure the table is filled
            if (db.Table<KMMInfo>().Count() == 0)
            {
                var nfo = new KMMInfo()
                {
                    Version = GetAssemblyVersion()
                };
                db.Insert(nfo);
            }
        }

        public string InstallationDirectory
        {
            get;
            set;
        }
        public string ModDirectory
        {
            get { return m_ModPath; }
            set
            {
                if (m_ModPath != value)
                {
                    m_ModPath = value;
                    if (!Directory.Exists(InstalledModCache)) Directory.CreateDirectory(InstalledModCache);
                }
            }
        }

        ~KspMods()
        {

        }

        public KspPackage[] GetAvailableMods()
        {
            var mods = new List<KspPackage>();
            foreach (var file in Directory.GetFiles(m_ModPath, "*.zip", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    var pkg = new KspPackage(file);
                    mods.Add(pkg);
                }
                catch (Exception) { continue; }
            }

            return mods.ToArray();
        }

        public KspPackage[] GetInstalledMods()
        {
            var mods = new List<KspPackage>();
            foreach (var file in Directory.GetFiles(InstalledModCache, "*.zip", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    var pkg = new KspPackage(file);
                    mods.Add(pkg);
                }
                catch (Exception) { continue; }
            }

            return mods.ToArray();
        }

        public IEnumerable<string> GetInstalledFiles()
        {
            var files = new List<string>();
            foreach (var f in db.Table<InstalledFiles>())
            {
                files.Add(f.Filename + " - " + f.CRC);
            }
            return files;
        }

        public Dictionary<string, List<string>> GetInstalledModFiles()
        {
            var files = new Dictionary<string, List<string>>();
            foreach (var f in db.Table<ModFiles>())
            {
                if (!files.ContainsKey(f.Filename)) files.Add(f.Filename, new List<string>());
                files[f.Filename].Add(f.ModArchive);
            }
            return files;
        }

        public bool InstallMod(KspPackage pkg)
        {
            var installation_date = DateTime.Now;
            IEnumerable<string> files = null;
            if (Properties.Settings.Default.CompatibilityMode == "0.20")
            {
                var name_without_ext = pkg.Filename;
                name_without_ext = name_without_ext.Substring(0, name_without_ext.LastIndexOf('.'));
                var new_style_path = Path.Combine("GameData", name_without_ext);
                files = pkg.Install(InstallationDirectory, new_style_path);
            }
            else
            {
                files = pkg.Install(InstallationDirectory);
            }

            if (files == null)
            {
                return false;
            }

            db.RunInTransaction(() =>
            {
                db.Insert(new InstalledMods()
                {
                    ModArchive = pkg.Filename,
                    InstallationDate = installation_date
                });
                foreach (var filename in files)
                {
                    Console.WriteLine(filename);
                    if (db.Table<InstalledFiles>().Count(t => t.Filename == filename) == 0)
                    {
                        db.Insert(new InstalledFiles()
                        {
                            Filename = filename,
                            InstallationDate = installation_date
                        });
                    }
                    db.Insert(new ModFiles()
                    {
                        ModArchive = pkg.Filename,
                        Filename = filename,
                    });
                }

                pkg.MoveTo(InstalledModCache);
            });
            return true;
        }

        public void UninstallMod(KspPackage pkg)
        {
            var mod = db.Table<InstalledMods>().First(m => m.ModArchive == pkg.Filename);
            db.RunInTransaction(() =>
            {
                db.Delete(mod);
                foreach (var modfile in db.Table<ModFiles>().Where(mf => mf.ModArchive == pkg.Filename))
                {
                    db.Delete(modfile);
                    if (db.Table<ModFiles>().Count(mf => mf.Filename == modfile.Filename) == 0)
                    {
                        Console.WriteLine("Deleting file: {0}", modfile.Filename);
                        db.Delete<InstalledFiles>(modfile.Filename);
                        var full_filename = Path.Combine(InstallationDirectory, modfile.Filename);
                        File.Delete(full_filename);
                        RemoveEmptyFolders(Path.GetDirectoryName(full_filename), InstallationDirectory);
                    }
                    else
                    {
                        Console.WriteLine("NOT Deleting file: {0}", modfile.Filename);
                    }
                }
            });
            pkg.MoveTo(ModPath);
        }

        private void RemoveEmptyFolders(string path, string shield_path)
        {
            var di = new DirectoryInfo(path);
            var shield = new DirectoryInfo(shield_path).FullName;
            while (string.Compare(di.FullName, shield, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                if (di.GetFileSystemInfos().Count() != 0) break;
                if (di.GetFiles().Count() != 0 || di.GetDirectories().Count() != 0)
                {
                    break;
                }
                di.Delete();
                di = di.Parent;
            }
        }

        #region Database Tables

        class KMMInfo
        {
            public string Version { get; set; }
        }

        class InstalledMods
        {
            [PrimaryKey]
            public string ModArchive { get; set; }
            public DateTime InstallationDate { get; set; }
        }

        class ModFiles
        {
            [PrimaryKey]
            public string ModArchive { get; set; }
            [PrimaryKey]
            public string Filename { get; set; }
        }
        class InstalledFiles
        {
            [PrimaryKey]
            public string Filename { get; set; }
            public string CRC { get; set; }
            public DateTime InstallationDate { get; set; }
        }
        #endregion
    }
}
