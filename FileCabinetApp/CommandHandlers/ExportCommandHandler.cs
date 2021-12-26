using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handler of export command.
    /// </summary>
    public class ExportCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service provided.</param>
        public ExportCommandHandler(IFileCabinetService fileCabinetService)
        {
            ExportCommandHandler.fileCabinetService = fileCabinetService;
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="request">Request provided.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Command == "export")
            {
                Export(request.Parameters);
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }

        private static void Export(string parameters)
        {
            if (parameters[0..3] == "csv")
            {
                string filename = parameters[4..];
                try
                {
                    if (File.Exists(filename))
                    {
                        Console.WriteLine("File is exist - rewrite " + filename + "?[Y/n]");
                        if (Console.ReadLine().ToUpperInvariant() == "y".ToUpperInvariant())
                        {
                            StreamWriter writer = new StreamWriter(filename);
                            fileCabinetService.MakeSnapshot().SaveToCSV(writer);
                            writer.Close();
                            Console.WriteLine("All records are exported to file " + filename + ".");
                        }
                    }
                    else
                    {
                        StreamWriter writer = new StreamWriter(filename);
                        fileCabinetService.MakeSnapshot().SaveToCSV(writer);
                        writer.Close();
                        Console.WriteLine("All records are exported to file " + filename + ".");
                    }
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("Export failed: can't open file " + filename + ".");
                }
            }

            if (parameters[0..3] == "xml")
            {
                string filename = parameters[4..];
                try
                {
                    if (File.Exists(filename))
                    {
                        Console.WriteLine("File is exist - rewrite " + filename + "?[Y/n]");
                        if (Console.ReadLine().ToUpperInvariant() == "y".ToUpperInvariant())
                        {
                            StreamWriter writer = new StreamWriter(filename);
                            fileCabinetService.MakeSnapshot().SaveToXml(writer);
                            writer.Close();
                            Console.WriteLine("All records are exported to file " + filename + ".");
                        }
                    }
                    else
                    {
                        StreamWriter writer = new StreamWriter(filename);
                        fileCabinetService.MakeSnapshot().SaveToXml(writer);
                        writer.Close();
                        Console.WriteLine("All records are exported to file " + filename + ".");
                    }
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("Export failed: can't open file " + filename + ".");
                }
            }
        }
    }
}
