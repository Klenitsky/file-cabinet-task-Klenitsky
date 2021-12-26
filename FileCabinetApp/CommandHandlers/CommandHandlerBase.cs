using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Base of Handler.
    /// </summary>
    public class CommandHandlerBase : ICommandHandler
    {
        protected ICommandHandler nextHandler;

        /// <summary>
        /// Sets the next handler.
        /// </summary>
        /// <param name="handler">Next handler.</param>
        public void SetNext(ICommandHandler handler)
        {
            this.nextHandler = handler;
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="request">Request provided.</param>
        public virtual void Handle(AppCommandRequest request)
        {
        }
    }
}
