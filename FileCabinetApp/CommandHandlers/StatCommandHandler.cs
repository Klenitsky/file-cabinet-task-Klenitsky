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
            StatCommandHandler.fileCabinetService = fileCabinetService;
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
                Stat(request.Parameters);
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }

        private static void Stat(string parameters)
        {
            var recordsCount = fileCabinetService.GetStat();
            int deletedCount = fileCabinetService.GetDeletedStat();
            Console.WriteLine($"{recordsCount} record(s).{deletedCount} records were deleted");
        }
    }
}
