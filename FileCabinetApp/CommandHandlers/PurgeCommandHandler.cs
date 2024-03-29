﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handler of purge command.
    /// </summary>
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service provided.</param>
        public PurgeCommandHandler(IFileCabinetService fileCabinetService)
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

            if (request.Command == "purge")
            {
                this.Purge(request.Parameters);
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }

        private void Purge(string parameters)
        {
            int stat = this.fileCabinetService.GetStat();
            int result = this.fileCabinetService.Purge();
            Console.WriteLine("Data file processing is completed:" + result + " of " + stat + "  records were purged.");
        }
    }
}
