using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Request of a programm command.
    /// </summary>
    public class AppCommandRequest
    {
        /// <summary>
        /// Gets or sets command name.
        /// </summary>
        /// <value>
        /// Name of a command.
        /// </value>
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets command parameters.
        /// </summary>
        /// <value>
        /// Parameters of the record.
        /// </value>
        public string Parameters { get; set; }
    }
}
