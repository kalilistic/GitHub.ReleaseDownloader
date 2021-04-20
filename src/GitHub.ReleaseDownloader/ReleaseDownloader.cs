using GitHub.ReleaseDownloader.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace GitHub.ReleaseDownloader
{
    public class ReleaseDownloader : IReleaseDownloader
    {
        public HttpClient HttpClient { get; private set; }
        private readonly IReleaseDownloaderSettings _settings;
        public bool IsConfigureAwait { get; set; } = true;
        private readonly string baseAddress = "https://api.github.com";

        public ReleaseDownloader(IReleaseDownloaderSettings settings)
        {
            _settings = settings.Copy();

            HttpClient = new HttpClient();
            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            HttpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue(assemblyName.Name, assemblyName.Version.ToString())));
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3.raw");
            if (!string.IsNullOrEmpty(_settings.PAT))
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", _settings.PAT);
            }
        }

        /// <summary>
        /// Deprecated. Initialize using custom httpClient.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="httpClient"></param>
        [Obsolete("this constrcutor is deprecated, please use other constrcutor instead.", false)]
        public ReleaseDownloader(IReleaseDownloaderSettings settings, HttpClient httpClient) : this(settings)
        {
            // Check User Using Custom httpClient himself.
            if (httpClient.DefaultRequestHeaders.UserAgent.Any())
            {
                this.HttpClient.Dispose();
                this.HttpClient = httpClient;
            }
        }

        #region Public Methods

        /// <summary>
        /// Apply Setting into HttpClient
        /// </summary>
        /// <returns></returns>
        public void SetPAT(string pat)
        {
            (_settings as ReleaseDownloaderSettings).SetPAT(pat);
            if (!string.IsNullOrEmpty(_settings.PAT))
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", _settings.PAT);
            }
        }

        public bool IsLatestRelease(string currentVersion)
        {
            Version version;
            try
            {
                version = Version.Parse(CleanVersion(currentVersion));
            }
            catch (Exception)
            {
                throw new Exception("Invalid version.");
            }

            var latestRelease = GetLatestRelease();
            return version >= latestRelease.Value;
        }

        public bool IsExistUser(string userName)
        {
            try
            {
                var userinfo = GetUserAsync(userName).Result;

                if (userinfo != null && !userinfo.ContainsKey("message"))
                {
                    return true;
                }
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException is APINotFoundException)
                {
                    return false;
                }
                throw ex;
            }
            return false;
        }

        public bool IsExistRepository(string userName, string repositoryName)
        {
            try
            {
                var userinfo = GetRepositoryAsync(userName, repositoryName).Result;
                
                if (userinfo != null && !userinfo.ContainsKey("message"))
                {
                    return true;
                }
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException is APINotFoundException)
                {
                    return false;
                }
                throw ex;
            }
            return false;
        }

        public KeyValuePair<string, Version> GetRelease(string version)
        {
            Version ver;
            try
            {
                ver = Version.Parse(CleanVersion(version));
            }
            catch (Exception)
            {
                throw new Exception("Invalid version.");
            }

            var releases = GetReleasesAsync().Result;
            if (!releases.Any())
            {
                throw new InvalidOperationException("This Repositroy Doesn't Have Release.");
            }
            if (!releases.Any(x => x.Value.CompareTo(ver) == 0))
            {
                throw new InvalidOperationException($"This Repositroy Doesn't Have Release(Version : {ver}).");
            }

            return releases.First(x => x.Value.CompareTo(ver) == 0);

        }

        public void DeInit()
        {
            HttpClient.DefaultRequestHeaders.Remove("User-Agent");
        }

        #region Download Methods

        public bool DownloadRelease(string releaseId)
        {
            var assetInfos = GetAssetsAsync(releaseId).Result;

            while (!Directory.Exists(_settings.DownloadDirPath))
            {
                Directory.CreateDirectory(_settings.DownloadDirPath);
                Thread.Sleep(1);
            }

            foreach (var assetInfo in assetInfos)
            {
                _ = DownloadAssetAsync(assetInfo);
            }

            return true;
        }

        public bool DownloadLatestRelease()
        {
            return DownloadRelease(GetLatestRelease().Key);
        }

        public bool DownloadLatestReleaseAsset(string assetIdName)
        {
            var latestReleaseId = GetLatestRelease().Key;
            var assetInfos = GetAssetsAsync(latestReleaseId).Result;

            while (!Directory.Exists(_settings.DownloadDirPath))
            {
                Directory.CreateDirectory(_settings.DownloadDirPath);
                Thread.Sleep(1);
            }

            if (!assetInfos.Any(x => assetIdName.Equals(x["id"]) || assetIdName.Equals(x["name"])))
            {
                return false;
            }
            _ = DownloadAssetAsync(assetInfos.First(x => assetIdName.Equals(x["id"]) || assetIdName.Equals(x["name"])));
            return true;
        }
        #endregion

        #endregion


        #region Private Methods

        private KeyValuePair<string, Version> GetLatestRelease()
        {
            var releases = GetReleasesAsync().Result;
            if (!releases.Any())
            {
                throw new InvalidOperationException("This Repositroy Doesn't Have Release.");
            }
            var latestRelease = releases.First();

            foreach (var release in releases)
                if (release.Value.CompareTo(latestRelease.Value) > 0)
                    latestRelease = release;

            return latestRelease;
        }

        #region Private Static Methods

        private static string CleanVersion(string version)
        {
            var regex = new Regex(@"[^0-9.]");
            return regex.Replace(version, "");
        }

        private static string GetNextPageNumber(HttpHeaders headers)
        {
            string linkHeader;
            try
            {
                linkHeader = headers.GetValues("Link").FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(linkHeader)) return null;
            var links = linkHeader.Split(',');
            return !links.Any()
                ? null
                : (
                    from link in links
                    where link.Contains(@"rel=""next""")
                    select Regex.Match(link, "(?<=page=)(.*)(?=>;)").Value).FirstOrDefault();
        }

        private static void VerifyGitHubAPIResponse(HttpStatusCode statusCode, string content, string apiName)
        {
            switch (statusCode)
            {
                case HttpStatusCode.Forbidden when content.Contains("API rate limit exceeded"):
                    throw new APIException("GitHub API rate limit exceeded.");
                case HttpStatusCode.NotFound when content.Contains("Not Found"):
                    throw new APINotFoundException($"GitHub {apiName} not found.");
                default:
                    {
                        if (statusCode != HttpStatusCode.OK) throw new APIException("GitHub API call failed.");
                        break;
                    }
            }
        }

        #endregion

        #region Private Async Methods

        private async Task<List<Dictionary<string, object>>> GetAssetsAsync(string releaseId)
        {
            List<Dictionary<string, object>> assetinfo = null;
            if (string.IsNullOrWhiteSpace(releaseId))
            {
                return null;
            }

            using (var response = await HttpClient.GetAsync(new Uri($"{baseAddress}/repos/{_settings.Author}/{_settings.Repository}/releases/{releaseId}/assets")).ConfigureAwait(IsConfigureAwait))
            {
                var contentJson = await response.Content.ReadAsStringAsync();
                VerifyGitHubAPIResponse(response.StatusCode, contentJson, "Users");
                var jsonSerializer = new JavaScriptSerializer();
                var dynamicJson = jsonSerializer.Deserialize<dynamic>(contentJson);
                assetinfo = new List<Dictionary<string, object>>();
                foreach (var objJson in dynamicJson)
                {
                    var dict = new Dictionary<string, object>();
                    foreach (var json in objJson)
                    {
                        dict.Add(json.Key, json.Value);
                    }
                    if (dict.Any())
                    {
                        assetinfo.Add(dict);
                    }
                }
            }
            return assetinfo;
        }

        private async Task DownloadAssetAsync(Dictionary<string,object> assetInfo)
        {
            try
            {
                var path = Path.Combine(_settings.DownloadDirPath, Path.GetFileName(assetInfo["name"].ToString()));

                HttpClient.DefaultRequestHeaders.Accept.First().MediaType = assetInfo["content_type"].ToString();
                using (var response = await HttpClient.GetAsync(new Uri(assetInfo["url"].ToString())).ConfigureAwait(IsConfigureAwait))
                {
                    var jObject = await response.Content.ReadAsByteArrayAsync();
                    System.IO.MemoryStream stream = new System.IO.MemoryStream(jObject);
                    // convert stream to string
                    StreamReader reader = new StreamReader(stream);
                    string text = reader.ReadToEnd();

                    using (var fileStream = File.Create(path))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.CopyTo(fileStream);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " | " + ex.StackTrace);
                throw new Exception("Release assets download failed.");
            }
            finally
            {
                HttpClient.DefaultRequestHeaders.Accept.First().MediaType = "application/vnd.github.v3.raw";
            }
        }

        private async Task<Dictionary<string, Version>> GetReleasesAsync()
        {
            var pageNumber = "1";
            var releases = new Dictionary<string, Version>();
            while (pageNumber != null)
            {
                using (var response = await HttpClient.GetAsync(new Uri($"{baseAddress}/repos/{_settings.Author}/{_settings.Repository}/releases?page={pageNumber}")).ConfigureAwait(IsConfigureAwait))
                {
                    var contentJson = await response.Content.ReadAsStringAsync();
                    VerifyGitHubAPIResponse(response.StatusCode, contentJson, "Repo");
                    var jsonSerializer = new JavaScriptSerializer();
                    var releasesJson = jsonSerializer.Deserialize<dynamic>(contentJson);
                    foreach (var releaseJson in releasesJson)
                    {
                        bool preRelease = releaseJson["prerelease"];
                        if (!_settings.IncludePreRelease && preRelease) continue;
                        var releaseId = releaseJson["id"].ToString();
                        try
                        {
                            string tagName = releaseJson["tag_name"].ToString();
                            var version = Version.Parse(CleanVersion(tagName));
                            releases.Add(releaseId, version);
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }

                    pageNumber = GetNextPageNumber(response.Headers);
                }
            }

            return releases;
        }

        private async Task<Dictionary<string, object>> GetUserAsync(string userName)
        {
            Dictionary<string, object> userInfo = null;
            if (string.IsNullOrWhiteSpace(userName))
            {
                return null;
            }
            using (var response = await HttpClient.GetAsync(new Uri($"{baseAddress}/users/{userName}")).ConfigureAwait(IsConfigureAwait))
            {
                var contentJson = await response.Content.ReadAsStringAsync();
                VerifyGitHubAPIResponse(response.StatusCode, contentJson, "Users");
                var jsonSerializer = new JavaScriptSerializer();
                var dynamicJson = jsonSerializer.Deserialize<dynamic>(contentJson);
                userInfo = new Dictionary<string, object>();
                foreach (var json in dynamicJson)
                {
                    userInfo.Add(json.Key, json.Value);
                }
            }
            return userInfo;
        }

        private async Task<Dictionary<string,object>> GetRepositoryAsync(string userName, string repositoryName)
        {
            Dictionary<string, object> userInfo = null;
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(repositoryName))
            {
                return null;
            }
            using (var response = await HttpClient.GetAsync(new Uri($"{baseAddress}/repos/{userName}/{repositoryName}")).ConfigureAwait(IsConfigureAwait))
            {
                var contentJson = await response.Content.ReadAsStringAsync();
                VerifyGitHubAPIResponse(response.StatusCode, contentJson, "Repos");
                var jsonSerializer = new JavaScriptSerializer();
                var dynamicJson = jsonSerializer.Deserialize<dynamic>(contentJson);
                userInfo = new Dictionary<string, object>();
                foreach (var json in dynamicJson)
                {
                    userInfo.Add(json.Key, json.Value);
                }
            }
            return userInfo;
        }

        #endregion

        #endregion
    }
}