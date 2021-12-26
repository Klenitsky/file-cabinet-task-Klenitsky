using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handler of import command.
    /// </summary>
    public class ImportCommandHandler : CommandHandlerBase
    {
        private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service provided.</param>
        public ImportCommandHandler(IFileCabinetService fileCabinetService)
        {
            ImportCommandHandler.fileCabinetService = fileCabinetService;
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

            if (request.Command == "import")
            {
                Import(request.Parameters);
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }

        private static void Import(string parameters)
        {
            string[] parametersArray = parameters.Split(' ');
            if (parametersArray[0] == "csv")
            {
                StreamReader file = null;
                try
                {
                    file = new StreamReader(parametersArray[1]);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Import error: file  " + parametersArray[1] + " is not exist.");
                }

                FileCabinetServiceSnapshot snapshot = new FileCabinetServiceSnapshot(new List<FileCabinetRecord>());
                snapshot.LoadFromCsv(file);
                fileCabinetService.Restore(snapshot);
                file.Close();
                Console.WriteLine(snapshot.Records.Count + " records were imported from " + parametersArray[1]);
            }

            if (parametersArray[0] == "xml")
            {
                StreamReader file = null;
                try
                {
                    file = new StreamReader(parametersArray[1]);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Import error: file  " + parametersArray[1] + " is not exist.");
                }

                FileCabinetServiceSnapshot snapshot = new FileCabinetServiceSnapshot(new List<FileCabinetRecord>());
                snapshot.LoadFromXml(file);
                fileCabinetService.Restore(snapshot);
                file.Close();
                Console.WriteLine(snapshot.Records.Count + " records were imported from " + parametersArray[1]);
            }
        }
    }
}
