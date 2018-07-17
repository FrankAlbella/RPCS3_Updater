using System;
using System.IO;
using System.Diagnostics;

namespace RPCS3_Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            CheckPrerequisites();
            Directory.CreateDirectory(DirectoryInfo.tempDirPath);
            Console.WriteLine("Checking for updates...");

            Emulator emu = new Emulator();
            Updater updater = new Updater(emu);

            if (!emu.IsOutdated())
            {
                Console.WriteLine("RPCS3 is up to date. Exiting...");
                ExitAndStartEmu();
            }
            else
            {
                Console.WriteLine("Downloading new update...");
                Console.WriteLine("Updating from {0} to {1}", emu.GetCurrentVersion(), emu.GetLatestVersion());

                updater.Init();
            }

            // Prevent program from closing
            // until updater finishes and calls exit
            while(true)
            {
                Console.ReadKey(true);
            }
        }

        public static void ExitAndStartEmu()
        {
#if !DEBUG
            Process.Start(DirectoryInfo.emuDirPath);
            Environment.Exit(0);
#endif
        }

        private static void CheckPrerequisites()
        {
            // TODO: use registry key info to detect 7zip install.
            // Registry key path: Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\7zFM.exe
            if (!File.Exists(DirectoryInfo.zFilePath))
                ExitWithError("7zip installation not detected");

            if (Process.GetProcessesByName("rpcs3").Length != 0)
                ExitWithError("RPCS3 instance is running");

        }

        private static void ExitWithError(string error)
        {
            Console.WriteLine("ERROR: {0}. Restart the updater after fixing the issue.", error);
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}
