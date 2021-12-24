using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetGenerator
{
    static class Program
    {
        private static string filename = String.Empty;
        private static int amount = 0;
        private static int startId = 1;
        private static bool isCsv = false;
        private static bool isXml = false;

        static void Main(string[] args)
        {
            int argsId = 0;
            Console.WriteLine($"File Cabinet Generator, developed by Konstantin Klenitsky");
            if (args != null && args.Length > 0)
            {
                if (args[argsId] == "-t")
                {
                    argsId++;
                    if (args[argsId].ToLower(CultureInfo.CurrentCulture) == "csv")
                    {
                        isCsv = true;
                    }

                    if (args[argsId].ToLower(CultureInfo.CurrentCulture) == "xml")
                    {
                        isXml = true;
                    }
                    argsId++;
                }
                else
                {
                    if (args[argsId].Contains("--output-type=", StringComparison.InvariantCulture))
                    {
                        if (args[0][14..].ToLower(CultureInfo.CurrentCulture) == "csv")
                        {
                            isCsv = true;
                        }

                        if (args[0][14..].ToLower(CultureInfo.CurrentCulture) == "xml")
                        {
                            isCsv = true;
                        }
                    }
                    argsId++;
                }

                if (args[argsId] == "-o")
                {
                    argsId++;
                    filename = args[argsId];
                    argsId++;
                }
                else
                {
                    if (args[argsId].Contains("--output=", StringComparison.InvariantCulture))
                    {

                        filename = args[argsId][9..];
                        argsId++;
                    }
                }

                if (args[argsId] == "-a")
                {
                    argsId++;
                    string number = args[argsId];
                    Int32.TryParse(number, out amount);
                    argsId++;
                }
                else
                {
                    if (args[argsId].Contains("--records-amount=", StringComparison.InvariantCulture))
                    {

                        string number = args[argsId][17..];
                        Int32.TryParse(number, out amount);
                        argsId++;
                    }
                }

                if (args[argsId] == "-i")
                {
                    argsId++;
                    string start = args[argsId];
                    Int32.TryParse(start, out startId);
                }
                else
                {
                    if (args[argsId].Contains("--start-id=", StringComparison.InvariantCulture))
                    {

                        string start = args[argsId][11..];
                        Int32.TryParse(start, out startId);
                    }
                }
            }
            Console.WriteLine("Generation of " + amount + " records, starting from id: " + startId);
            if (isCsv)
            {
                Console.WriteLine("Filetype csv");
            }

            if (isXml)
            {
                Console.WriteLine("Filetype xml");
            }

            Console.WriteLine("Filename" + filename);

            List<FileCabinetRecord> list O
            for(int i=0; i < amount; i++)
            {

            }
        }
    }
}
