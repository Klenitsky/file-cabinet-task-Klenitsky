using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handler of list command.
    /// </summary>
    public class ListCommandHandler : CommandHandlerBase
    {
        private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service provided.</param>
        public ListCommandHandler(IFileCabinetService fileCabinetService)
        {
            ListCommandHandler.fileCabinetService = fileCabinetService;
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

            if (request.Command == "list")
            {
                List(request.Parameters);
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }

        private static void List(string parameters)
        {
            IReadOnlyCollection<FileCabinetRecord> arrayOfRecords = fileCabinetService.GetRecords();
            foreach (FileCabinetRecord record in arrayOfRecords)
            {
                Console.WriteLine(record.ToString());
            }
        }
    }
}
