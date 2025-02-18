namespace HouseKeeper;

using System;
using System.IO;
using System.Linq;

public static class Program
{
    static void Main(string[] args)
    {
	    // Set the path or pass it as an argument in format: 'HouseKeeper.exe "C:\Path\To\Your\SymbolStorage"' 
	    string HardCodedPath = null; // @"C:\Path\To\Your\SymbolStorage";
        
        if (args.Length < 1 && String.IsNullOrEmpty(HardCodedPath))
        {
            Console.WriteLine("Usage: SymbolCleanup <path-to-symbol-storage>. Path to symbol storage is required.");
            return;
        }
        
        string symbolStoragePath = (String.IsNullOrEmpty(HardCodedPath))? args[0] : HardCodedPath;
        
        Console.WriteLine($@"The specified path is {symbolStoragePath}. Proceeding will partially delete files and folders inside of that directory. Are you sure you want to proceed? (y/n)");
        var key = Console.ReadKey();

        if (key.Key != ConsoleKey.Y)
        {
	        Console.WriteLine("\n Disagree received. Press Enter to exit. Restart and press 'Y' to proceed with cleanup.");
        }

        if (!Directory.Exists(symbolStoragePath))
        {
            Console.WriteLine($"Error: Directory does not exist: {symbolStoragePath}");
            return;
        }

        try
        {
            var moduleDirectories = Directory.GetDirectories(symbolStoragePath);
            foreach (var moduleDirectory in moduleDirectories)
            {
                Console.WriteLine($"Processing module: {Path.GetFileName(moduleDirectory)}");
                
                var folderGuids = Directory.GetDirectories(moduleDirectory);

                if (folderGuids.Length <= 1)
                {
                    Console.WriteLine(" - Skipping (no extra folders to clean).");
                    continue;
                }

                // Find the most recently created folder
                var latestFolder = folderGuids
                    .Select(folder => new DirectoryInfo(folder))
                    .OrderByDescending(folderInfo => folderInfo.CreationTimeUtc)
                    .First();

                Console.WriteLine($" - Keeping latest folder: {latestFolder.Name}");

                // Delete all other folders
                foreach (var folder in folderGuids)
                {
                    if (folder != latestFolder.FullName)
                    {
                        Console.WriteLine($" - Deleting old folder: {Path.GetFileName(folder)}");
                        Directory.Delete(folder, true);
                    }
                }
            }

            Console.WriteLine("Symbol cleanup completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during cleanup: {ex.Message}");
        }
    }
}
