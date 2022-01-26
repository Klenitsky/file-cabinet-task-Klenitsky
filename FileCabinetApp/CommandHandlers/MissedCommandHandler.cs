using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handler of the missing command.
    /// </summary>
    public class MissedCommandHandler : CommandHandlerBase
    {
        private static string[][] helpMessages = new string[][]
       {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints the statistics", "The 'stat' command prints the statistics." },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record." },
            new string[] { "update", "edits records", "The 'edit' command edits  records with entered properties" },
            new string[] { "select", "selects records according to the parameters.", "The 'select' command selects records according to the parameters." },
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

            PrintMissedCommandInfo(request.Command);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine($"There most similar commands are:");
            for (int i = 0; i < 11; i++)
            {
                if (helpMessages[i][0].StartsWith(command, StringComparison.InvariantCulture))
                {
                    Console.WriteLine("     " + helpMessages[i][0]);
                }
            }

            Console.WriteLine();
        }
    }
}
