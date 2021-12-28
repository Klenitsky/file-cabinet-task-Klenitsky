using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Validators;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handler of commands that need service.
    /// </summary>
    public class ServiceCommandHandlerBase : CommandHandlerBase
    {
        /// <summary>
        /// Service to work with.
        /// </summary>
        protected IFileCabinetService fileCabinetService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCommandHandlerBase"/> class.
        /// </summary>
        /// <param name="service"> service provided.</param>
        public ServiceCommandHandlerBase(IFileCabinetService service)
        {
            this.fileCabinetService = service;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCommandHandlerBase"/> class.
        /// </summary>
        public ServiceCommandHandlerBase()
        {
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        public static void Handle()
        {
            // Method intentionally left empty.
        }
    }
}
