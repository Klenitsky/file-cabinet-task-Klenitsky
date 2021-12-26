using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handler of commands that need service.
    /// </summary>
    public class ServiceCommandHandlerBase : CommandHandlerBase
    {
        protected static IFileCabinetService fileCabinetService;

        public ServiceCommandHandlerBase(IFileCabinetService service)
        {
            fileCabinetService = service;
        }

        public ServiceCommandHandlerBase()
        {
        }

        public void Handle()
        {
        }
    }
}
