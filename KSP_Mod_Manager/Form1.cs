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
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace KSP_Mod_Manager
{
    public partial class Form1 : Form
    {
        private KspMods mods;
        public Form1()
        {
            InitializeComponent();

            mods = new KspMods(Properties.Settings.Default.KSP_Path, Properties.Settings.Default.Mods_Path);

            UpdateMods();
        }

        private void UpdateMods()
        {
            lstAvailableMods.Items.Clear();
            lstAvailableMods.Items.AddRange(mods.GetAvailableMods());
            lstInstalledMods.Items.Clear();
            lstInstalledMods.Items.AddRange(mods.GetInstalledMods());
        }

        void SetStatus(string text, bool enabled)
        {
            btnInstallMod.Enabled = enabled;
            btnUninstallMod.Enabled = enabled;
            lstAvailableMods.Enabled = enabled;
            lstInstalledMods.Enabled = enabled;
            lblStatus.Text = text;
        }

        private void btnInstallMod_Click(object sender, EventArgs e)
        {
            if (lstAvailableMods.SelectedItem != null)
            {
                SetStatus("Installing mod...", false);
                var pkg = (KspPackage)lstAvailableMods.SelectedItem;
                new Thread(delegate()
                {
                    var success = mods.InstallMod(pkg);
                    this.Invoke((Action)delegate
                    {
                        if (success)
                        {
                            lstAvailableMods.Items.Remove(pkg);
                            lstInstalledMods.Items.Add(pkg);
                        }
                        SetStatus( success ? "Done!" : "Error!", true);
                    });
                }).Start();
            }
        }

        private void btnUninstallMod_Click(object sender, EventArgs e)
        {
            if (lstInstalledMods.SelectedItem != null)
            {
                SetStatus("Uninstalling mod...", false);
                var pkg = (KspPackage)lstInstalledMods.SelectedItem;
                new Thread(delegate()
                {
                    mods.UninstallMod(pkg);
                    this.Invoke((Action)delegate
                    {
                        lstInstalledMods.Items.Remove(pkg);
                        lstAvailableMods.Items.Add(pkg);
                        SetStatus("Done!", true);
                    });
                }).Start();
            }
        }

        private void refreshFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateMods();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new Form2();
            if (DialogResult.OK == dialog.ShowDialog())
            {
                mods.InstallationDirectory = Properties.Settings.Default.KSP_Path;
                mods.ModDirectory = Properties.Settings.Default.Mods_Path;
                UpdateMods();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lstAvailableMods_DoubleClick(object sender, EventArgs e)
        {
            btnInstallMod_Click(sender, e);
        }

        private void lstInstalledMods_DoubleClick(object sender, EventArgs e)
        {
            btnUninstallMod_Click(sender, e);
        }

        private void lstAvailableMods_DragEnter(object sender, DragEventArgs e)
        {
            Console.WriteLine(string.Join(", ", e.Data.GetFormats()));

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void lstAvailableMods_DragDrop(object sender, DragEventArgs e)
        {
            var filename = new FileInfo(((string[])e.Data.GetData(DataFormats.FileDrop))[0]);
            Console.WriteLine("'{0}'", filename);
            filename.CopyTo(Path.Combine(mods.ModDirectory, filename.Name));
            UpdateMods();
        }
    }
}
