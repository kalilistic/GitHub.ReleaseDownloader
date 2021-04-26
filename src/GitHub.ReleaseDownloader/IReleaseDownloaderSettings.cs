using System.Net.Http;

namespace GitHub.ReleaseDownloader
{
    public interface IReleaseDownloaderSettings
    {
        string Author { get; set; }
        string Repository { get; set; }
        bool IncludePreRelease { get; set; }
        string DownloadDirPath { get; set; }
        /// <summary>
        /// Personal Access Token
        /// </summary>
        string PAT { get; }
        IReleaseDownloaderSettings Copy();
    }
}