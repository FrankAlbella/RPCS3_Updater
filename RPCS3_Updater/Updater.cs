using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace RPCS3_Updater
{
    class Updater
    {
        private WebClient client;
        private Emulator emu;

        private byte progressRow = 0;
        private byte percentage = 0;

        private const byte progressBarMaxWidth = 20;
        private const byte progressBarInterval = 100 / progressBarMaxWidth; // ex. 100/20 = 5. Every 5 percent, increment the progress bar.

        public Updater(Emulator emu)
        {
            this.client = new WebClient();
            this.emu = emu;
        }

        public void Init()
        {
            progressRow = (byte)Console.CursorTop;

            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            client.DownloadFileCompleted += Client_DownloadFileCompleted;

            client.DownloadFileAsync(emu.GetDownloadUrl(), DirectoryInfo.tempFilePath);
        }

        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            client.Dispose();

            Console.WriteLine();
            Console.WriteLine("Download done! Extracting...");

            ExtractFiles();

            Console.WriteLine("Update complete!");
            Directory.Delete(DirectoryInfo.tempDirPath, true);

            Program.ExitAndStartEmu();
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Prevent progress bar from being written multiple times
            if (e.ProgressPercentage <= percentage) 
                return;
            percentage = (byte)e.ProgressPercentage;

            Console.SetCursorPosition(0, progressRow);

            Console.Write("[");
            Console.Write(new string('#', e.ProgressPercentage / progressBarInterval));
            Console.Write(new string(' ', progressBarMaxWidth - (e.ProgressPercentage / progressBarInterval)));
            Console.Write("] {0}%", e.ProgressPercentage);
        }

        private void ExtractFiles()
        {
            string zFile = DirectoryInfo.zFilePath;
            string argument = string.Format("x \"{0}\" -o\"{1}\" -aoa", DirectoryInfo.tempFilePath, DirectoryInfo.currentDirPath);

            Console.WriteLine("Extracting files to: " + DirectoryInfo.currentDirPath);
            //Console.WriteLine("Running command: {0} {1}", zFile, argument);

#if DEBUG
            Console.WriteLine("Debug build detected. Canceling extraction...");
            return;
#endif

            Process proc = new Process();
            proc.StartInfo.FileName = zFile;
            proc.StartInfo.WorkingDirectory = DirectoryInfo.currentDirPath;
            proc.StartInfo.Arguments = argument;
            //proc.StartInfo.CreateNoWindow = true;
            //proc.StartInfo.UseShellExecute = false;
            proc.Start();
            proc.WaitForExit();
        }


    }
}
