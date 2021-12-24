using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    static class Program
    {
        private static string filename = String.Empty;
        private static int amount = 0;
        private static int startId = 1;
        private static bool isCsv = false;
        private static bool isXml = false;

        private static string[] firstNames = { "Ivan", "Konstantin", "Roman", "Andrew", "John", "James", "Ethan", "Alex", "Kate", "Mary", "Phillip", "Mason", "Martin", "Hugo", "Paul" };

        private static string[] lastNames = { "Smith", "James", "Hendrix", "Eriks", "George", "Jhones", "Ivanov", "Pavlov", "Wizard", "Popov", "Rudenko", "Cook", "Martin", "Boss", "Pearl" };

        private static char[] categories = { 'A', 'B', 'C', 'D' };
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


            List<FileCabinetRecord> listOfRecords = new List<FileCabinetRecord>();
            for (int i = 0; i < amount; i++)
            {
                listOfRecords.Add(GenerateRecord(startId + i));
            }

            if (isCsv)
            {
                ExportCsv(listOfRecords, filename);
            }
            if (isXml)
            {
                ExportXml(listOfRecords, filename);
            }
        }

        public static FileCabinetRecord GenerateRecord(int id)
        {
            Random rnd = new Random();


            short height = (short)rnd.Next(0, 250);
            decimal weight = new decimal(rnd.Next(0, 180));
            int year = rnd.Next(1950, 2021);
            int month = rnd.Next(1, 12);
            int day = rnd.Next(1, 28);
            int nameNum = rnd.Next(0, 14);
            int surnameNum = rnd.Next(0, 14);
            int categoryNum = rnd.Next(0, 3);

            DateTime dateOfBirth = new DateTime(year, month, day);
            string firstName = firstNames[nameNum];
            string lastName = lastNames[surnameNum];
            char category = categories[categoryNum];

            return new FileCabinetRecord {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Height = height,
                Weight = weight,
                DrivingLicenseCategory = category,
            };
        }

        public static void ExportCsv(List<FileCabinetRecord> list, string filename)
        {
            if (File.Exists(filename))
            {
                Console.WriteLine("File is exist - rewrite " + filename + "?[Y/n]");
                if (Console.ReadLine().ToUpperInvariant() == "y".ToUpperInvariant())
                {
                    StreamWriter writer = new StreamWriter(filename);
                    writer.WriteLine("Id,First Name, Last Name,Date Of Birth,Height,Weight,Driving License Category");
                    foreach (FileCabinetRecord record in list)
                    {
                        writer.Write(record.Id + ",");
                        writer.Write(record.FirstName + ",");
                        writer.Write(record.LastName + ",");
                        writer.Write(record.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + ",");
                        writer.Write(record.Height + ",");
                        writer.Write(record.Weight + ",");
                        writer.WriteLine(record.DrivingLicenseCategory);
                    }
                    writer.Close();
                    Console.WriteLine(amount + " records were written to " + filename);
                }
            }
            else
            {
                StreamWriter writer = new StreamWriter(filename);
                writer.WriteLine("Id,First Name, Last Name,Date Of Birth,Height,Weight,Driving License Category");
                foreach (FileCabinetRecord record in list)
                {
                    writer.Write(record.Id + ",");
                    writer.Write(record.FirstName + ",");
                    writer.Write(record.LastName + ",");
                    writer.Write(record.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + ",");
                    writer.Write(record.Height + ",");
                    writer.Write(record.Weight + ",");
                    writer.WriteLine(record.DrivingLicenseCategory);
                }
                writer.Close();
                Console.WriteLine(amount + " records were written to " + filename);
            }
        }

        public static void ExportXml(List<FileCabinetRecord> list, string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RecordsXml));
            List<RecordXml> array = new List<RecordXml>();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                IndentChars = "\t",
                NewLineHandling = NewLineHandling.Replace
            };
            foreach (FileCabinetRecord item in list)
            {
                array.Add(new RecordXml
                {
                    Id = item.Id,
                    Name = new XmlName { First = item.FirstName, Last = item.LastName },
                    DateOfBirth = item.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Height = item.Height,
                    Weight = item.Weight,
                    DrivingLicenseCategory = item.DrivingLicenseCategory
                });
            }

            serializer.Serialize(XmlWriter.Create(filename,settings),new RecordsXml(array));
            Console.WriteLine(amount + " records were written to " + filename);

        }
    }

    
}


