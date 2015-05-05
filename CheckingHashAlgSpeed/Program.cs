using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CheckingHashAlgSpeed
{
    class Program
    {
        const string LargeFileFullPath = @"Z:\Apps\Visual Studio Family\2015\RC\en_visual_studio_enterprise_2015_rc_x86_dvd_6649702.iso";

        static string FileFullPathToUse = LargeFileFullPath;


        static void Main(string[] args)
        {
            Console.WindowHeight = 60;
            Console.WindowWidth = 200;
            Console.BufferHeight = 60;
            Console.BufferWidth = 200;
            Console.Clear();

            if (args.Length > 0)
            {
                FileFullPathToUse = args[0].Trim('\"');
            }

            Console.WriteLine("Starting");
            Console.WriteLine();

            Console.WriteLine($"File: {FileFullPathToUse}");
            Console.WriteLine($"File Size: {GetStringOfSize(new FileInfo(FileFullPathToUse).Length)}");

            Console.WriteLine();

            TimeHash(HashHelper.Algorithms.MD5);
            TimeHash(HashHelper.Algorithms.RIPEMD160);
            TimeHash(HashHelper.Algorithms.SHA1);
            TimeHash(HashHelper.Algorithms.SHA256);
            TimeHash(HashHelper.Algorithms.SHA384);
            TimeHash(HashHelper.Algorithms.SHA512);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        static string GetStringOfSize(long value)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = value;
            var order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }
            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }

        static void TimeHash(HashAlgorithm algorithm)
        {
            var hash = TimeIt(algorithm.ToString(), () =>
            {
                return HashHelper.GetHashFromFile(FileFullPathToUse, algorithm);
            });
            Console.WriteLine(hash);
            Console.WriteLine();
        }

        static string TimeIt(string desc, Func<string> work)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var result = work();
            sw.Stop();
            Console.WriteLine($"{desc}: {sw.ElapsedMilliseconds / 1000} seconds");
            return result;
        }
    }
}
