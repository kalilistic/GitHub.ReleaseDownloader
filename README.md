
<h1 align="center">
  <br><a href="https://github.com/kalilistic/github.releasedownloader"><img src="img/bannerIcon.png" alt="GitHubReleaseDownloader"></a>
  <br>GitHub Release Downloader<br>
</h1>
<h4 align="center">Compare GitHub versions and download latest release assets.</h4>

<p align="center">
  <a href="https://github.com/kalilistic/github.releasedownloader/releases/latest"><img src="https://img.shields.io/github/v/release/kalilistic/github.releasedownloader"></a>
  <a href="https://ci.appveyor.com/project/kalilistic/github-releasedownloader/branch/master"><img src="https://img.shields.io/appveyor/ci/kalilistic/github-releasedownloader"></a>
  <a href="https://ci.appveyor.com/project/kalilistic/github-releasedownloader/branch/master/tests"><img src="https://img.shields.io/appveyor/tests/kalilistic/github-releasedownloader"></a>
  <a href="https://codecov.io/gh/kalilistic/github.releasedownloader/branch/master"><img src="https://img.shields.io/codecov/c/gh/kalilistic/github.releasedownloader"></a>
  <a href="https://github.com/kalilistic/github.releasedownloader/blob/master/LICENSE"><img src="https://img.shields.io/github/license/kalilistic/github.releasedownloader?color=lightgrey"></a>
</p>

## Background

Small .NET framework library to compare versions and download the latest GitHub release.

## Key Features

* Check if current version is most recent using AssemblyVersion.
* Download latest release artifacts from GitHub releases.
* Include / exclude pre-releases.
  
## How To Use

```csharp
// create settings object
HttpClient httpClient = new HttpClient();
string author = "kalilistic";
string repo = "github.releasedownloader";
bool includePreRelease = true;
string downloadDirPath = "C:\assets";
IReleaseDownloaderSettings settings = new ReleaseDownloaderSettings(httpClient, author, repo, includePreRelease, downloadDirPath);

// create downloader
IReleaseDownloader downloader = new ReleaseDownloader(settings);

// check version
string currentVersion = "5.0.0";
bool isMostRecentVersion = downloader.IsLatestRelease(currentVersion);

// download latest github release
if (!isMostRecentVersion) {
  downloader.DownloadLatestRelease();
}

// clean up
downloader.DeInit();
httpClient.Dispose();
```

## Considerations
* Versions must be SemVer-compliant or exception will be thrown.
* Will not compare and silently skip over GitHub releases that aren't AssemblyVersion-compliant.
* GitHub API calls are made anonymously and subject to rate limits.

## How To Contribute

Feel free to open an issue or submit a PR.
