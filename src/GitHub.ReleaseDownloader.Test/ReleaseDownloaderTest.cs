using System;
using System.IO;
using System.Net.Http;
using GitHub.ReleaseDownloader;
using NUnit.Framework;

namespace GitHubReleaseDownloader.Test
{
    [TestFixture]
    public class ReleaseDownloaderTest
    {
        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient(new HttpMessageHandlerMock());
            _downloadDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets");
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
            _downloader.DeInit();
        }

        private const string Author = "kalilistic";
        private const string Repo = "TestLibrary";
        private HttpClient _httpClient;
        private string _downloadDirPath;
        private IReleaseDownloader _downloader;

        [Test]
        public void DeInit_RemovesAgentHeader()
        {
            var settings = new ReleaseDownloaderSettings(Author, Repo, true, _downloadDirPath);
            _downloader = new ReleaseDownloader(settings, _httpClient);
            _downloader.DeInit();
            Assert.IsEmpty(_httpClient.DefaultRequestHeaders.UserAgent);
        }

        [Test]
        [Ignore("Integration Test using real GitHub API")]
        public void DownloadLatestRelease_IT_ReturnsTrue()
        {
            var httpClient = new HttpClient();
            var settings = new ReleaseDownloaderSettings(Author, Repo, true, _downloadDirPath);
            _downloader = new ReleaseDownloader(settings, httpClient);
            Assert.IsTrue(_downloader.DownloadLatestRelease());
            httpClient.Dispose();
        }

        [Test]
        public void DownloadLatestRelease_ReturnsTrue()
        {
            var settings = new ReleaseDownloaderSettings(Author, Repo, true, _downloadDirPath);
            _downloader = new ReleaseDownloader(settings, _httpClient);
            Directory.CreateDirectory(_downloadDirPath);
            _downloader.DownloadLatestRelease();
        }

        [Test]
        public void IsLatestRelease_BadRepo_ThrowsException()
        {
            var settings = new ReleaseDownloaderSettings(Author, "BAD-REPO", true, _downloadDirPath);
            _downloader = new ReleaseDownloader(settings, _httpClient);
            Assert.Throws<AggregateException>(() => _downloader.IsLatestRelease("5.0.0"));
        }

        [Test]
        public void IsLatestRelease_BadVersion_ThrowsException()
        {
            var settings = new ReleaseDownloaderSettings(Author, Repo, true, _downloadDirPath);
            _downloader = new ReleaseDownloader(settings, _httpClient);
            Assert.Throws<Exception>(() => _downloader.IsLatestRelease("bad-version"));
        }

        [Test]
        public void IsLatestRelease_IsLatestNoPR_ReturnsTrue()
        {
            var settings = new ReleaseDownloaderSettings(Author, Repo, false, _downloadDirPath);
            _downloader = new ReleaseDownloader(settings, _httpClient);
            Assert.IsTrue(_downloader.IsLatestRelease("6.0.30"));
        }

        [Test]
        public void IsLatestRelease_IsLatestPR_ReturnsTrue()
        {
            var settings = new ReleaseDownloaderSettings(Author, Repo, true, _downloadDirPath);
            _downloader = new ReleaseDownloader(settings, _httpClient);
            Assert.IsTrue(_downloader.IsLatestRelease("6.0.53"));
        }

        [Test]
        public void IsLatestRelease_IsNotLatestNoPR_ReturnsFalse()
        {
            var settings = new ReleaseDownloaderSettings(Author, Repo, false, _downloadDirPath);
            _downloader = new ReleaseDownloader(settings, _httpClient);
            Assert.IsFalse(_downloader.IsLatestRelease("6.0.29"));
        }

        [Test]
        public void IsLatestRelease_IsNotLatestPR_ReturnsFalse()
        {
            var settings = new ReleaseDownloaderSettings(Author, Repo, true, _downloadDirPath);
            _downloader = new ReleaseDownloader(settings, _httpClient);
            Assert.IsFalse(_downloader.IsLatestRelease("5.0.0"));
        }

        [Test]
        [Ignore("Integration Test using real GitHub API")]
        public void IsLatestRelease_IT_ReturnsTrue()
        {
            var httpClient = new HttpClient();
            var settings = new ReleaseDownloaderSettings(Author, Repo, false, _downloadDirPath);
            _downloader = new ReleaseDownloader(settings, httpClient);
            Assert.IsTrue(_downloader.IsLatestRelease("6.0.30"));
            httpClient.Dispose();
        }

        [Test]
        public void IsLatestRelease_OtherAPIError_ThrowsException()
        {
            var settings = new ReleaseDownloaderSettings(Author, "OTHER-ERROR", true, _downloadDirPath);
            _downloader = new ReleaseDownloader(settings, _httpClient);
            Assert.Throws<AggregateException>(() => _downloader.IsLatestRelease("5.0.0"));
        }

        [Test]
        public void IsLatestRelease_RateLimit_ThrowsException()
        {
            var settings = new ReleaseDownloaderSettings(Author, "RATE-LIMITED", true, _downloadDirPath);
            _downloader = new ReleaseDownloader(settings, _httpClient);
            Assert.Throws<AggregateException>(() => _downloader.IsLatestRelease("5.0.0"));
        }
    }
}