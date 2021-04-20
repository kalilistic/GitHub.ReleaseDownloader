
namespace GitHub.ReleaseDownloader.Example
{
    partial class Downloader
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
            this.btn_Download = new System.Windows.Forms.Button();
            this.txt_User = new System.Windows.Forms.TextBox();
            this.txt_Repository = new System.Windows.Forms.TextBox();
            this.chk_IncludePreRelease = new System.Windows.Forms.CheckBox();
            this.txt_PAT = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbl_User = new System.Windows.Forms.Label();
            this.lbl_Repository = new System.Windows.Forms.Label();
            this.lbl_PAT = new System.Windows.Forms.Label();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Download
            // 
            this.btn_Download.Location = new System.Drawing.Point(509, 18);
            this.btn_Download.Name = "btn_Download";
            this.btn_Download.Size = new System.Drawing.Size(96, 94);
            this.btn_Download.TabIndex = 0;
            this.btn_Download.Text = "Download";
            this.btn_Download.UseVisualStyleBackColor = true;
            this.btn_Download.Click += new System.EventHandler(this.btn_Download_Click);
            // 
            // txt_User
            // 
            this.txt_User.Location = new System.Drawing.Point(128, 14);
            this.txt_User.Name = "txt_User";
            this.txt_User.Size = new System.Drawing.Size(150, 21);
            this.txt_User.TabIndex = 1;
            // 
            // txt_Repository
            // 
            this.txt_Repository.Location = new System.Drawing.Point(128, 39);
            this.txt_Repository.Name = "txt_Repository";
            this.txt_Repository.Size = new System.Drawing.Size(150, 21);
            this.txt_Repository.TabIndex = 2;
            // 
            // chk_IncludePreRelease
            // 
            this.chk_IncludePreRelease.AutoSize = true;
            this.chk_IncludePreRelease.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chk_IncludePreRelease.Location = new System.Drawing.Point(6, 64);
            this.chk_IncludePreRelease.Name = "chk_IncludePreRelease";
            this.chk_IncludePreRelease.Size = new System.Drawing.Size(136, 16);
            this.chk_IncludePreRelease.TabIndex = 3;
            this.chk_IncludePreRelease.Text = "InClude PreRelease";
            this.chk_IncludePreRelease.UseVisualStyleBackColor = true;
            // 
            // txt_PAT
            // 
            this.txt_PAT.Location = new System.Drawing.Point(41, 14);
            this.txt_PAT.Name = "txt_PAT";
            this.txt_PAT.Size = new System.Drawing.Size(153, 21);
            this.txt_PAT.TabIndex = 4;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(509, 18);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(96, 94);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 5;
            this.progressBar1.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbl_Repository);
            this.groupBox1.Controls.Add(this.lbl_User);
            this.groupBox1.Controls.Add(this.txt_User);
            this.groupBox1.Controls.Add(this.txt_Repository);
            this.groupBox1.Controls.Add(this.chk_IncludePreRelease);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(285, 100);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbl_PAT);
            this.groupBox2.Controls.Add(this.txt_PAT);
            this.groupBox2.Location = new System.Drawing.Point(303, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Private";
            // 
            // lbl_User
            // 
            this.lbl_User.AutoSize = true;
            this.lbl_User.Location = new System.Drawing.Point(6, 17);
            this.lbl_User.Name = "lbl_User";
            this.lbl_User.Size = new System.Drawing.Size(31, 12);
            this.lbl_User.TabIndex = 4;
            this.lbl_User.Text = "User";
            // 
            // lbl_Repository
            // 
            this.lbl_Repository.AutoSize = true;
            this.lbl_Repository.Location = new System.Drawing.Point(6, 42);
            this.lbl_Repository.Name = "lbl_Repository";
            this.lbl_Repository.Size = new System.Drawing.Size(65, 12);
            this.lbl_Repository.TabIndex = 5;
            this.lbl_Repository.Text = "Repository";
            // 
            // lbl_PAT
            // 
            this.lbl_PAT.AutoSize = true;
            this.lbl_PAT.Location = new System.Drawing.Point(6, 21);
            this.lbl_PAT.Name = "lbl_PAT";
            this.lbl_PAT.Size = new System.Drawing.Size(29, 12);
            this.lbl_PAT.TabIndex = 5;
            this.lbl_PAT.Text = "PAT";
            // 
            // Downloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 125);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btn_Download);
            this.Name = "Downloader";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Downloader_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Download;
        private System.Windows.Forms.TextBox txt_User;
        private System.Windows.Forms.TextBox txt_Repository;
        private System.Windows.Forms.CheckBox chk_IncludePreRelease;
        private System.Windows.Forms.TextBox txt_PAT;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbl_Repository;
        private System.Windows.Forms.Label lbl_User;
        private System.Windows.Forms.Label lbl_PAT;
        private System.Windows.Forms.FolderBrowserDialog fbd;
    }
}

