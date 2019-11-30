using System.Net.Http;

namespace GitHub.ReleaseDownloader
{
    public interface IReleaseDownloaderSettings
    {
        HttpClient HTTPClient { get; set; }
        string Author { get; set; }
        string Repository { get; set; }
        bool IncludePreRelease { get; set; }
        string DownloadDirPath { get; set; }
        IReleaseDownloaderSettings Copy();
    }
}