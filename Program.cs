using System;
using System.IO;
using StormLibSharp;

namespace FrozenMPQ
{
    class FMPQ
    {

        private void Run(String[] args)
        {
            if(!CheckArguments(args))
            {
                return;
            }
            Console.WriteLine("Running operation. . .");
            String operationName = args[0];
            String mpqName = args[1];
            String listfileName = "listfile.txt";
            if (!File.Exists(mpqName))
            {
                Console.WriteLine("MPQ file does not exist: " + mpqName);
                return;
            }
            if(!File.Exists(listfileName))
            {
                Console.WriteLine("Listfile does not exist: " + listfileName);
                return;
            }
            Boolean extract;
            if (operationName.Equals("ext"))
            {
                extract = true;

            }
            else if (operationName.Equals("imp"))
            {
                extract = false;
            }
            else
            {
                Console.WriteLine("Invalid operation: " + operationName);
                return;
            }
            for (int i = 2; i < args.Length; i++)
            {
                using (MpqArchive archive = new MpqArchive(mpqName, FileAccess.ReadWrite))
                {
                    int retval = archive.AddListFile(listfileName);
                    Console.WriteLine("Added listfile: " + retval + " (0 is ok)");
                    if (extract)
                    {
                        Extract(args[i], archive);
                    }
                    else
                    {
                        Import(args[i], archive);
                    }
                    Console.WriteLine("All desired operations completed. Flushing & closing.");
                    archive.Compact(listfileName);
                    archive.Flush();
                    archive.Dispose();
                }
            }
        }

        private void Extract(String filename, MpqArchive archive)
        {
            Console.WriteLine("Extracting file: " + filename);
            if(!archive.HasFile(filename))
            {
                Console.WriteLine("File did not exist in MPQ archive: " + filename + " skipping. . .");
                return;
            }
            archive.ExtractFile(filename, filename);
            if(File.Exists("out/" + filename))
            {
                File.Delete("out/" + filename);
                Console.WriteLine("Overwrote existing file...");
            }
            if(!System.IO.Directory.Exists("out"))
            {
                System.IO.Directory.CreateDirectory("out");
            }
            File.Copy(filename, "out/" + filename);
            File.Delete(filename);
            Console.WriteLine("Extracted successfully to: " + filename);
        }

        private void Import(String filename, MpqArchive archive)
        {
            Console.WriteLine("Importing file: in/" + filename);
            if(!File.Exists("in/" + filename))
            {
                Console.WriteLine("File does not exist: in/" + filename + " skipping. . .");
                return;
            }
            archive.AddFileFromDisk("in/" + filename, filename);
            Console.WriteLine("Successfully imported in/" + filename + " as " + filename);
        }

        private Boolean CheckArguments(String[] args)
        {
            Console.WriteLine("Received " + args.Length + " arguments");
            if (args.Length <= 2)
            {
                Console.WriteLine("Usage: frozenmpq.exe <actionType> <fileName> <file1> <file2> ... <etc>");
                Console.WriteLine("Action type: either \"ext\" (extract) or \"imp\" (import)");
                Console.WriteLine("Filename: relative filename to MPQ file, including extension");
                Console.WriteLine("Files: A series of files to either export or import. For importing, path should be relative");
                Console.WriteLine("Extract action will extract requested files to \"out/\" directory");
                Console.WriteLine("Import action will import the requested files to the MPQ, overwriting.");
                Console.WriteLine("The file should exist in the \"in\" directory and be the same name as desired in the MPQ");
                Console.WriteLine("This application expects listfile.txt to exist in the same directory.");
                Console.WriteLine("EXAMPLE: frozenmpq.exe myMap.w3x ext scripts/war3map.j war3map.j war3map.w3o");
                Console.WriteLine("Don't include any spaces!");
                return false;
            }
            return true;
        }

        static void Main(string[] args)
        {
            new FMPQ().Run(args);
        }
    }
}
