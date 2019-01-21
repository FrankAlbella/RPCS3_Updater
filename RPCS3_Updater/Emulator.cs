using System;
using System.IO;
using System.Linq;

namespace RPCS3_Updater
{
    class Emulator
    {
        private const string verUrl = "https://rpcs3.net/download";
        private readonly Uri downloadUrl;
        private readonly string latestVer;
        private readonly string currentVer;

        public Emulator()
        {
            this.downloadUrl = new Uri(RepoInfo.GetDownloadUrl());
            this.latestVer = RepoInfo.GetLatestVersion();
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
