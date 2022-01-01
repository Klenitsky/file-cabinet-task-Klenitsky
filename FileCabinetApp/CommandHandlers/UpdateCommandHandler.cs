using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handler of update command.
    /// </summary>
    public class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="service"> Service provided.</param>
        public UpdateCommandHandler(IFileCabinetService service)
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

            if (request.Command == "update")
            {
                var result = this.Update(request.Parameters);
                StringBuilder resultId = new StringBuilder();
                foreach (var record in result)
                {
                    resultId.Append('#' + record.Id.ToString(CultureInfo.InvariantCulture) + ',');
                }

               // resultId.Remove(resultId.Length - 1, 1);
                Console.WriteLine($"\nRecords " + resultId.ToString() + "are deleted.");
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }

        private List<FileCabinetRecord> Update(string parameters)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            List<SearchingAttributes> attributesToChange = new List<SearchingAttributes>();
            List<SearchingAttributes> attributesToFind = new List<SearchingAttributes>();
            parameters = parameters.Replace("=", " ", StringComparison.InvariantCulture);
            parameters = parameters.Replace(",", " ", StringComparison.InvariantCulture);
            parameters = parameters.Replace("'", string.Empty, StringComparison.InvariantCulture);
            Regex.Replace(parameters, @"\s+", " ");
            List<string> str = new List<string>(parameters.Split(' '));
            for (int j = 0; j < str.Count;  j++)
            {
                if (string.IsNullOrEmpty(str[j]))
                {
                    str.RemoveAt(j);
                    j--;
                }
            }

            int i = 1;
            while (str[i] != "where" && i < str.Count)
            {
                attributesToChange.Add(new SearchingAttributes(str[i], str[i + 1]));
                i += 2;
            }

            i++;
            while (i < str.Count)
            {
                attributesToFind.Add(new SearchingAttributes(str[i], str[i + 1]));
                i += 3;
            }

            this.fileCabinetService.Update(attributesToChange, attributesToFind);
            return result;
        }
    }
}
