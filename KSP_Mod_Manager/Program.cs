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
using System.Windows.Forms;

namespace KSP_Mod_Manager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!Properties.Settings.Default.AnnoyingFirstTimeMessageShown)
            {
                MessageBox.Show(
@"Welcome to KSP mod manager!

It seems this is the first time you run this program!

Here are some tips for proper usage:
 * Make sure your KSP directory is clean
 * Select a folder for your mods. Installed mods will be backed up in a folder named ""Installed"" in that folder.
 * Bugs reports can be sent to ksp-mod-manager@jawsper.nl

Good flying, fellow Kerbal!

 - jawsper",
                    "Welcome to KSP mod manager!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Properties.Settings.Default.AnnoyingFirstTimeMessageShown = true;
                Properties.Settings.Default.Save();
            }

            if (Properties.Settings.Default.KSP_Path.Length == 0 || !Directory.Exists(Properties.Settings.Default.KSP_Path))
            {
                var dialog = new Form2();
                var result = dialog.ShowDialog();
                if (DialogResult.OK != result)
                {
                    Application.Exit();
                    return;
                }
            }
            Application.Run(new Form1());
        }
    }
}
