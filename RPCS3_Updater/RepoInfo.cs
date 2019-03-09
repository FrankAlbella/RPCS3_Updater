using Octokit;
using System.Threading.Tasks;

namespace RPCS3_Updater
{
    static class RepoInfo
    {
        private const string repoOwner = "RPCS3";
        private const string repoName = "rpcs3-binaries-win";

        private static GitHubClient gitHubClient = new GitHubClient(new ProductHeaderValue("RPCS3-Updater"));

        private static Release latestRelease;
        public static Release LatestRelease
        {
            get
            {
                if (latestRelease == null)
                    latestRelease = Task.Run(() => GetLatestReleaseAsync()).Result;

                return latestRelease;
            }
        }

        private static async Task<Release> GetLatestReleaseAsync()
        {
            Release latestRelease = await gitHubClient.Repository.Release.GetLatest(repoOwner, repoName);

            return latestRelease;
        }

        public static string GetLatestVersion()
        {
            return 'v' + LatestRelease.Name;
        }

        public static string GetDownloadUrl()
        {
            return LatestRelease.Assets[0].BrowserDownloadUrl;
        }
    }
}
