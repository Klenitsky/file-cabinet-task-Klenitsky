using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handler of Help command.
    /// </summary>
    public class HelpCommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints the statistics", "The 'stat' command prints the statistics." },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record." },
            new string[] { "update", "edits records", "The 'edit' command edits  records with entered properties" },
            new string[] { "select", "selects  records according to the parameters.", "The 'selects' command selects records according to the parameters." },
            new string[] { "export", "exports the data into file", "The 'export' command exports the data." },
            new string[] { "import", "imports the data from file", "The 'import' command imports the data." },
            new string[] { "insert", "Inserts a new record.", "The 'insert' command inserts the data." },
            new string[] { "delete", "removes records", "The 'remove' command removes records." },
            new string[] { "purge", "purges deleted records", "The 'purge' command purges deleted records." },
        };

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

            if (request.Command == "help")
            {
                PrintHelp(request.Parameters);
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }
    }
}
