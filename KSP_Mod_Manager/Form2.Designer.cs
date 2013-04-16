namespace KSP_Mod_Manager
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtKspPath = new System.Windows.Forms.TextBox();
            this.txtModPath = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSelectKspPath = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSelectModPath = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSave.Location = new System.Drawing.Point(281, 161);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(200, 161);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtKspPath
            // 
            this.txtKspPath.Location = new System.Drawing.Point(6, 19);
            this.txtKspPath.Name = "txtKspPath";
            this.txtKspPath.Size = new System.Drawing.Size(294, 20);
            this.txtKspPath.TabIndex = 2;
            // 
            // txtModPath
            // 
            this.txtModPath.Location = new System.Drawing.Point(6, 19);
            this.txtModPath.Name = "txtModPath";
            this.txtModPath.Size = new System.Drawing.Size(294, 20);
            this.txtModPath.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSelectKspPath);
            this.groupBox1.Controls.Add(this.txtKspPath);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 45);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "KSP main directory";
            // 
            // btnSelectKspPath
            // 
            this.btnSelectKspPath.Location = new System.Drawing.Point(306, 19);
            this.btnSelectKspPath.Name = "btnSelectKspPath";
            this.btnSelectKspPath.Size = new System.Drawing.Size(31, 20);
            this.btnSelectKspPath.TabIndex = 3;
            this.btnSelectKspPath.Text = "...";
            this.btnSelectKspPath.UseVisualStyleBackColor = true;
            this.btnSelectKspPath.Click += new System.EventHandler(this.btnSelectKspPath_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSelectModPath);
            this.groupBox2.Controls.Add(this.txtModPath);
            this.groupBox2.Location = new System.Drawing.Point(12, 63);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(344, 45);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mod storage directory";
            // 
            // btnSelectModPath
            // 
            this.btnSelectModPath.Location = new System.Drawing.Point(307, 19);
            this.btnSelectModPath.Name = "btnSelectModPath";
            this.btnSelectModPath.Size = new System.Drawing.Size(30, 20);
            this.btnSelectModPath.TabIndex = 4;
            this.btnSelectModPath.Text = "...";
            this.btnSelectModPath.UseVisualStyleBackColor = true;
            this.btnSelectModPath.Click += new System.EventHandler(this.btnSelectModPath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(346, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Note: it is not recommended to change these paths after installing mods.";
            // 
            // Form2
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(368, 196);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.Text = "Options | KSP mod manager";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtKspPath;
        private System.Windows.Forms.TextBox txtModPath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSelectKspPath;
        private System.Windows.Forms.Button btnSelectModPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label1;
    }
}