using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;

namespace RPCS3_Updater
{
    class Emulator
    {
        private readonly string verUrl = "https://rpcs3.net/download";
        private readonly Uri downloadUrl;
        private readonly string latestVer;
        private readonly string currentVer;

        public Emulator()
        {
            this.downloadUrl = FindDownloadUrl();
            this.latestVer = FindLatestEmulatorVersion();
            this.currentVer = FindCurrentEmulatorVersion();
        }

        public bool IsOutdated()
        {
            return this.latestVer != this.currentVer; 
        }

        public string GetLatestVersion()
        {
            return this.latestVer;
        }

        public string GetCurrentVersion()
        {
            return this.currentVer;
        }

        public Uri GetDownloadUrl()
        {
            return this.downloadUrl;
        }

        private Uri FindDownloadUrl()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument page = web.Load(verUrl);

            HtmlNodeCollection aNodes = page.DocumentNode.SelectNodes("//a[@target='_blank']");

            foreach (var node in aNodes)
            {
                if (node.Attributes["href"].Value.Contains(@"win64"))
                {
                    return new Uri(node.Attributes["href"].Value);
                }
            }

            return new Uri("");
        }

        private string FindLatestEmulatorVersion()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument page = web.Load(verUrl);

            HtmlNodeCollection downloadButton = page.DocumentNode.SelectNodes("//span[@class='download-define-build darkmode-txt']");

            return downloadButton[0].InnerHtml.ToString().Replace("\n", "").Substring(0, 11);
        }

        private string FindCurrentEmulatorVersion()
        {/*
#if DEBUG
            return "DEBUG";
#endif*/
            if (File.Exists(DirectoryInfo.verFilePath))
            {
                return File.ReadLines(DirectoryInfo.verFilePath).First().Substring(6, 11);
            }

            return "UNKNOWN";
        }
    }
}
