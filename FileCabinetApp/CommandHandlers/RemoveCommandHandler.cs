using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handler of remove command.
    /// </summary>
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service provided.</param>
        public RemoveCommandHandler(IFileCabinetService fileCabinetService)
        {
            RemoveCommandHandler.fileCabinetService = fileCabinetService;
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

            if (request.Command == "remove")
            {
                Remove(request.Parameters);
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }

        private static void Remove(string parameters)
        {
            int removeId;
            bool success = int.TryParse(parameters, out removeId);
            if (!success)
            {
                Console.WriteLine("Error occured: Invalid Id");
            }

            success = fileCabinetService.Remove(removeId);
            if (success)
            {
                Console.WriteLine("Record #" + removeId + " is removed.");
            }
            else
            {
                Console.WriteLine("Record #" + removeId + " doesn't exist.");
            }
        }
    }
}
