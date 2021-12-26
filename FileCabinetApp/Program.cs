using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using FileCabinetApp.CommandHandlers;

namespace FileCabinetApp
{
    /// <summary>
    /// The class of the programm.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Konstantin Klenitsky";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        public static bool isCustom;
        private static FileStream fileStream = File.Open("cabinet-records.db", FileMode.OpenOrCreate);

        public static bool isRunning = true;

        private static IFileCabinetService fileCabinetService = new FileCabinetFilesystemService(fileStream, new DefaultValidator());

        /// <summary>
        /// The main function of the application.
        /// </summary>
        /// <param name="args">Array of arguments.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            if (args != null && args.Length > 0)
            {
                if (args[0] == "-v" && args[1].ToLower(CultureInfo.CurrentCulture) == "custom")
                {
                    fileCabinetService = new FileCabinetMemoryService(new CustomValidator());
                    isCustom = true;
                }

                if (args[0].Contains("--validation-rules=", StringComparison.InvariantCulture) && args[0][19..].ToLower(CultureInfo.CurrentCulture) == "custom")
                {
                    fileCabinetService = new FileCabinetMemoryService(new CustomValidator());
                    isCustom = true;
                }

                if (args[0] == "--storage" || args[0] == "-s")
                {
                    if (args[1].ToLower(CultureInfo.CurrentCulture) == "memory")
                    {
                        if (isCustom)
                        {
                            fileCabinetService = new FileCabinetMemoryService(new CustomValidator());
                            Console.WriteLine($"Using memory.");
                        }
                        else
                        {
                            fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());
                            Console.WriteLine($"Using memory.");
                        }
                    }
                    else
                    {
                        if (args[1].ToLower(CultureInfo.CurrentCulture) == "file")
                        {
                            if (isCustom)
                            {
                                fileCabinetService = new FileCabinetFilesystemService(fileStream, new CustomValidator());
                                Console.WriteLine($"Using filesystem.");
                            }
                            else
                            {
                                fileCabinetService = new FileCabinetFilesystemService(fileStream, new DefaultValidator());
                                Console.WriteLine($"Using filesystem.");
                            }
                        }
                    }
                }
            }

            if (isCustom)
            {
                Console.WriteLine($"Using custom validation rules.");
            }
            else
            {
                Console.WriteLine($"Using default validation rules.");
            }

            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];
                const int parametersIndex = 1;
                var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var commandHandler = CreateCommandHandlers();
                try
                {
                    commandHandler.Handle(new CommandHandlers.AppCommandRequest { Command = command, Parameters = parameters, });
                }
                catch (ArgumentException)
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static FileCabinetApp.CommandHandlers.ICommandHandler CreateCommandHandlers()
        {
            var helpCommandHandler = new HelpCommandHandler();
            var exitCommandHandler = new ExitCommandHandler();
            var statCommandHandler = new StatCommandHandler(fileCabinetService);
            var createCommandHandler = new CreateCommandHandler(fileCabinetService);
            var listCommandHandler = new ListCommandHandler(fileCabinetService);
            var editCommandHandler = new EditCommandHandler(fileCabinetService);
            var findCommandHandler = new FindCommandHandler(fileCabinetService);
            var exportCommandHandler = new ExportCommandHandler(fileCabinetService);
            var importCommandHandler = new ImportCommandHandler(fileCabinetService);
            var removeCommandHandler = new RemoveCommandHandler(fileCabinetService);
            var purgeCommandHandler = new PurgeCommandHandler(fileCabinetService);
            var commandMissHandler = new MissedCommandHandler();

            helpCommandHandler.SetNext(exitCommandHandler);
            exitCommandHandler.SetNext(statCommandHandler);
            statCommandHandler.SetNext(createCommandHandler);
            createCommandHandler.SetNext(listCommandHandler);
            listCommandHandler.SetNext(editCommandHandler);
            editCommandHandler.SetNext(findCommandHandler);
            findCommandHandler.SetNext(exportCommandHandler);
            exportCommandHandler.SetNext(importCommandHandler);
            importCommandHandler.SetNext(removeCommandHandler);
            removeCommandHandler.SetNext(purgeCommandHandler);
            purgeCommandHandler.SetNext(commandMissHandler);
            return helpCommandHandler;
        }
    }
}
