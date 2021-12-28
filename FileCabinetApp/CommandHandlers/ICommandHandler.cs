using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Interface of a Handler.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Sets the next handler.
        /// </summary>
        /// <param name="handler">Next handler.</param>
        public void SetNext(ICommandHandler handler);

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="request">Request provided.</param>
        public void Handle(AppCommandRequest request);
    }
}
