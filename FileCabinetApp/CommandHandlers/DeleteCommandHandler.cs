using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using FileCabinetApp.ValidationRules;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handler of delete command.
    /// </summary>
    public class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service provided.</param>
        public DeleteCommandHandler(IFileCabinetService service)
        {
            this.fileCabinetService = service;
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

            if (request.Command == "delete")
            {
                this.Delete(request.Parameters);
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }

        private static SearchingAttributes EnterParameters(string parameters)
        {
            parameters = parameters.Replace("=", " ", StringComparison.InvariantCulture);
            parameters = parameters.Replace("'", string.Empty, StringComparison.InvariantCulture);
            parameters = Regex.Replace(parameters, @"\s+", " ");
            string[] str = parameters.Split(' ');
            if (str[0] != "where")
            {
                throw new ArgumentException("Invalid parameters of the function", nameof(parameters));
            }

            return new SearchingAttributes(str[1], str[2]);
        }

        private void Delete(string parameters)
        {
            SearchingAttributes attributes = EnterParameters(parameters);
            IEnumerable<FileCabinetRecord> result;
            try
            {
                result = this.fileCabinetService.Delete(attributes);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Data is invalid");
                return;
            }

            StringBuilder resultId = new StringBuilder();
            foreach (var record in result)
            {
                resultId.Append('#' + record.Id.ToString(CultureInfo.InvariantCulture) + ',');
            }

            if (resultId.Length > 0)
            {
                resultId.Remove(resultId.Length - 1, 1);
                Console.WriteLine($"\nRecords " + resultId.ToString() + "are deleted.");
            }
            else
            {
                Console.WriteLine($"No such records.");
            }
        }
    }
}
