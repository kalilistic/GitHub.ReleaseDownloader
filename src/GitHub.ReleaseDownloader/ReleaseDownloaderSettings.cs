using System;
using System.Net.Http;

namespace GitHub.ReleaseDownloader
{
    public class ReleaseDownloaderSettings : IReleaseDownloaderSettings
    {
        public ReleaseDownloaderSettings(string author, string repository,
            bool includePreRelease, string downloadDirPath, string PAT = "")
        {
            this.Author = author ?? throw new ArgumentNullException(nameof(author));
            this.Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.IncludePreRelease = includePreRelease;
            this.DownloadDirPath = downloadDirPath;
            this.PAT = PAT;
        }

        public string Author { get; set; }
        public string Repository { get; set; }
        public bool IncludePreRelease { get; set; }
        public string DownloadDirPath { get; set; }
        public string PAT { get; private set; }

        public IReleaseDownloaderSettings Copy()
        {
            return new ReleaseDownloaderSettings(string.Copy(this.Author),
                string.Copy(this.Repository),
                this.IncludePreRelease, string.Copy(this.DownloadDirPath),string.Copy(this.PAT));
        }

        internal void SetPAT(string pat)
        {
            this.PAT = pat;
        }
    }
}