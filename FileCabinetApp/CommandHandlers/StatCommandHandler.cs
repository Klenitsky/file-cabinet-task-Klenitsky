using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handler of Stat command.
    /// </summary>
    public class StatCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service provided.</param>
        public StatCommandHandler(IFileCabinetService fileCabinetService)
        {
            this.fileCabinetService = fileCabinetService;
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

            if (request.Command == "stat")
            {
                this.Stat(request.Parameters);
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }

        private void Stat(string parameters)
        {
            var recordsCount = this.fileCabinetService.GetStat();
            int deletedCount = this.fileCabinetService.GetDeletedStat();
            var records = this.fileCabinetService.GetRecords();
            foreach (FileCabinetRecord record in records)
            {
                Console.WriteLine(record.ToString());
            }

            Console.WriteLine($"{recordsCount} record(s).{deletedCount} records were deleted");
        }
    }
}
