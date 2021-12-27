using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handler of list command.
    /// </summary>
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private readonly Action<IEnumerable<FileCabinetRecord>> printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service provided.</param>
        /// <param name="recordPrinter">Printer provided.</param>
        public ListCommandHandler(IFileCabinetService fileCabinetService, Action<IEnumerable<FileCabinetRecord>> recordPrinter)
        {
            ListCommandHandler.fileCabinetService = fileCabinetService;
            this.printer = recordPrinter;
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
                IReadOnlyCollection<FileCabinetRecord> arrayOfRecords = fileCabinetService.GetRecords();
                this.printer(arrayOfRecords);
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}
