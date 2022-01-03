using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handler of select command.
    /// </summary>
    public class SelectCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=" SelectCommandHandler"/> class.
        /// </summary>
        /// <param name="service"> Service provided.</param>
        public SelectCommandHandler(IFileCabinetService service)
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

            if (request.Command == "select")
            {
                List<string> argumentsToShow;
                var result = this.Select(request.Parameters, out argumentsToShow);
                Console.Write("+");
                foreach (var argument in argumentsToShow)
                {
                    string str = new string('-', argument.Length + 2);
                    Console.Write(str + "+");
                }

                Console.WriteLine();
                Console.Write("|");
                foreach (var argument in argumentsToShow)
                {
                    Console.Write(" " + argument + " |");
                }

                Console.WriteLine();
                Console.Write("+");
                foreach (var argument in argumentsToShow)
                {
                    string str = new string('-', argument.Length + 2);
                    Console.Write(str + "+");
                }

                foreach (var record in result)
                {
                    Console.WriteLine();
                    Console.Write('|');
                    foreach (var argument in argumentsToShow)
                    {
                        switch (argument)
                        {
                            case "id":
                                Console.Write(" " + record.Id + " ");
                                break;
                            case "firstname":
                                StringBuilder str = new StringBuilder();
                                int q = 0;
                                while (q < record.FirstName.Length)
                                {
                                    if (!char.IsLetter(record.FirstName[q]))
                                    {
                                        break;
                                    }

                                    str.Append(record.FirstName[q]);
                                    q++;
                                }

                                Console.Write(" " + str.ToString());
                                for (int i = 0; i < argument.Length + 1 - str.Length; i++)
                                {
                                    Console.Write(" ");
                                }

                                break;
                            case "lastname":
                                str = new StringBuilder();
                                q = 0;
                                while (q < record.LastName.Length)
                                {
                                    if (!char.IsLetter(record.LastName[q]))
                                    {
                                        break;
                                    }

                                    str.Append(record.LastName[q]);
                                    q++;
                                }

                                Console.Write(" " + str.ToString());
                                for (int i = 0; i < argument.Length + 1 - str.Length; i++)
                                {
                                    Console.Write(" ");
                                }

                                break;
                            case "height":
                                str = new StringBuilder();
                                str.Append(record.Height + " ");
                                StringBuilder resultStr = new StringBuilder();
                                while (resultStr.Length != argument.Length + 2-str.Length)
                                {
                                    resultStr.Append(' ');
                                }

                                resultStr.Append(str);
                                Console.Write(resultStr);
                                break;
                            case "weight":
                                str = new StringBuilder();
                                str.Append(record.Weight + " ");
                                resultStr = new StringBuilder();
                                while (resultStr.Length != argument.Length + 2 - str.Length)
                                {
                                    resultStr.Append(' ');
                                }

                                resultStr.Append(str);
                                Console.Write(resultStr);
                                break;
                            case "drivinglicensecategory":
                                Console.Write(" " + record.DrivingLicenseCategory + "                      ");
                                break;
                        }

                        Console.Write('|');
                    }

                    Console.WriteLine();
                    Console.Write("+");
                    foreach (var argname in argumentsToShow)
                    {
                        string str = new string('-', argname.Length + 2);
                        Console.Write(str + "+");
                    }
                }

                Console.WriteLine();
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }

        private List<FileCabinetRecord> Select(string parameters, out List<string> attributesToShow)
        {
            string complexAttribute = string.Empty;
            List<FileCabinetRecord> result;
            attributesToShow = new List<string>();
            List<SearchingAttributes> attributesToFind = new List<SearchingAttributes>();
            parameters = parameters.Replace("=", " ", StringComparison.InvariantCulture);
            parameters = parameters.Replace(",", " ", StringComparison.InvariantCulture);
            parameters = parameters.Replace("'", string.Empty, StringComparison.InvariantCulture);
            Regex.Replace(parameters, @"\s+", " ");
            List<string> str = new List<string>(parameters.Split(' '));
            for (int j = 0; j < str.Count; j++)
            {
                if (string.IsNullOrEmpty(str[j]))
                {
                    str.RemoveAt(j);
                    j--;
                }
            }

            int i = 0;
            while (str[i] != "where" && i < str.Count)
            {
                attributesToShow.Add(str[i]);
                i += 1;
            }

            i++;
            if (i + 2 < str.Count)
            {
                complexAttribute = str[i + 2];
            }

            while (i < str.Count)
            {
                attributesToFind.Add(new SearchingAttributes(str[i], str[i + 1]));
                i += 3;
            }

            result = (List<FileCabinetRecord>)this.fileCabinetService.Select(attributesToFind, complexAttribute);
            return result;
        }
    }
}
