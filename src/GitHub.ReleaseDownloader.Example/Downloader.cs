using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitHub.ReleaseDownloader.Example
{
    public partial class Downloader : Form
    {
        private IReleaseDownloader downloader;
        public Downloader()
        {
            InitializeComponent();

        }

        private void btn_Download_Click(object sender, EventArgs e)
        {
            DialogResult result = fbd.ShowDialog();
            if (result != DialogResult.OK)
                return;

            try
            {
                progressBar1.Enabled = true;
                progressBar1.Visible = true;

                // Create Setting Object
                IReleaseDownloaderSettings settings = new ReleaseDownloaderSettings(this.txt_User.Text, this.txt_Repository.Text,
                this.chk_IncludePreRelease.Checked, fbd.SelectedPath, this.txt_PAT.Text);
                // Create Downloader
                downloader = new ReleaseDownloader(settings)
                {
                    // Fix DeadLock
                    IsConfigureAwait = false
                };

                // Check Exist User
                if (!downloader.IsExistUser(this.txt_User.Text))
                {
                    MessageBox.Show("User does not exist.");
                    return;
                }

                // Check Exist Repository of User
                if (!downloader.IsExistRepository(this.txt_User.Text, this.txt_Repository.Text))
                {
                    MessageBox.Show("Repository does not exist.");
                    return;
                }

                downloader.SetPAT("a");

                // Check Version
                bool isMostRecentVersion = false; downloader.IsLatestRelease("0.1.0.0");

                // Download Latest Github Release
                if (!isMostRecentVersion)
                {
                    //var version =  downloader.GetRelease("0.2.0.0");
                    //downloader.DownloadRelease(version.Key);

                    //downloader.DownloadLatestReleaseAsset("ac");

                    downloader.DownloadLatestRelease();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[Error] {ex.Message}");
            }
            finally
            {
                progressBar1.Enabled = false;
                progressBar1.Visible = false;
            }

        }

        private string GetCurrentVersion()
        {
            var assembly = Assembly.GetEntryAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fileVersionInfo.ProductVersion;
            return version;
        }

        private void Downloader_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Clean Up
            downloader?.DeInit();
        }
    }
}
