/*
 * Delete Microsoft Zone.Identifier from data
 * Handle with care!
*/
namespace UnblockZoneIdentifier
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;

    internal class Program
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteFile(string name);

        public static bool UnblockZone(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            return DeleteFile($"{fileName}:Zone.Identifier");
        }

        private static void Main(string[] args)
        {
            var self = Environment.GetCommandLineArgs().First();
            var root = Directory.GetCurrentDirectory();
            var unblocked = 0;

            foreach (var file in Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories))
            {
                if (file == self)
                {
                    continue;
                }

                try
                {
                    if (UnblockZone(file))
                    {
                        Console.WriteLine($"Unblocked {file}");
                        unblocked++;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            Console.WriteLine($"Completed. Total of {unblocked} files unblocked.");
            Console.ReadLine();
        }
    }
}