namespace RPCS3_Updater
{
    static class DirectoryInfo
    {
        public static readonly string currentDirPath = System.IO.Directory.GetCurrentDirectory();
        public static readonly string tempDirPath = currentDirPath + @"\temp\";
        public static readonly string tempFilePath = tempDirPath + @"emu.7z";
        public static readonly string verFilePath = currentDirPath + @"\RPCS3.log";
        public static readonly string emuDirPath = currentDirPath + @"\rpcs3.exe";
        public static readonly string zFilePath = @"C:\Program Files\7-Zip\7z.exe";
    }
}
