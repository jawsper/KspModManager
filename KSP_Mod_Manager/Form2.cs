using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KSP_Mod_Manager
{
    public partial class Form2 : Form
    {
        private bool _vetoClosing = false;

        public Form2()
        {
            InitializeComponent();
            txtKspPath.Text = Properties.Settings.Default.KSP_Path;
            txtModPath.Text = Properties.Settings.Default.Mods_Path;
            txtShipsPath.Text = Properties.Settings.Default.ShipsPath;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtKspPath.Text.Length == 0)
            {
                MessageBox.Show("No path to KSP folder entered!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _vetoClosing = true;
                return;
            }
            var kspPath = new DirectoryInfo(txtKspPath.Text);
            if (!kspPath.Exists)
            {
                MessageBox.Show("Invalid path to KSP folder entered!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _vetoClosing = true;
                return;
            }
            /*if (!File.Exists(Path.Combine(kspPath.FullName, "KSP.exe")))
            {
                MessageBox.Show("KSP.exe not found in path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _vetoClosing = true;
                return;
            }*/

            if (!Directory.Exists(txtModPath.Text))
            {
                try
                {
                    Directory.CreateDirectory(txtModPath.Text);
                }
                catch (IOException io_ex)
                {
                    MessageBox.Show(string.Format("Cannot create mod directory: {0}", io_ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _vetoClosing = true;
                    return;
                }
            }

            Properties.Settings.Default.KSP_Path = txtKspPath.Text;
            Properties.Settings.Default.Mods_Path = txtModPath.Text;
            Properties.Settings.Default.ShipsPath = txtShipsPath.Text;
            Properties.Settings.Default.Save();

            this.DialogResult = DialogResult.OK;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            if (_vetoClosing)
            {
                _vetoClosing = false;
                e.Cancel = true;
                return;
            }
            base.OnClosing(e);
        }

        private void btnSelectKspPath_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = txtKspPath.Text;
            folderBrowserDialog1.ShowNewFolderButton = false;
            if (DialogResult.OK == folderBrowserDialog1.ShowDialog())
            {
                txtKspPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnSelectModPath_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = txtModPath.Text;
            folderBrowserDialog1.ShowNewFolderButton = true;
            if (DialogResult.OK == folderBrowserDialog1.ShowDialog())
            {
                txtModPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnSelectShipsPath_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = txtShipsPath.Text;
            folderBrowserDialog1.ShowNewFolderButton = true;
            if (DialogResult.OK == folderBrowserDialog1.ShowDialog())
            {
                txtShipsPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
