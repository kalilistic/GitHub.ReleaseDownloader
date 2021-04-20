using System;
using System.Collections.Generic;
using System.Net.Http;

namespace GitHub.ReleaseDownloader
{
    public interface IReleaseDownloader
    {
        HttpClient HttpClient { get; }
        bool IsLatestRelease(string version);
        KeyValuePair<string, Version> GetRelease(string version);
        bool DownloadRelease(string releaseId);
        bool DownloadLatestRelease();
        bool DownloadLatestReleaseAsset(string assetIdName);
        bool IsExistUser(string userName);
        bool IsExistRepository(string userName, string repositoryName);
        void DeInit();
        void SetPAT(string pat);

        /// <summary>
        /// For DeadLock in UI Thread(for winform)
        /// </summary>
        bool IsConfigureAwait { get; set; }
    }
}