using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Iterators;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// The class of the programm.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Konstantin Klenitsky";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private static FileStream fileStream = File.Open("cabinet-records.db", FileMode.OpenOrCreate);

        private static bool isRunning = true;
        private static bool isCustom;

        private static IFileCabinetService fileCabinetService = new ServiceLogger(new ServiceMeter(new FileCabinetFilesystemService(fileStream, new ValidatorBuilder().CreateDefault())), "applicationLog.log");

        /// <summary>
        /// The main function of the application.
        /// </summary>
        /// <param name="args">Array of arguments.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            if (args != null && args.Length > 0)
            {
                for (int i = 0; i < args.Length;)
                {
                    if (args[i] == "-v" && args[i + 1].ToLower(CultureInfo.CurrentCulture) == "custom")
                    {
                        fileCabinetService = new FileCabinetMemoryService(new ValidatorBuilder().CreateCustom());
                        isCustom = true;
                        i += 2;
                        continue;
                    }

                    if (args[i].Contains("--validation-rules=", StringComparison.InvariantCulture) && args[0][19..].ToLower(CultureInfo.CurrentCulture) == "custom")
                    {
                        fileCabinetService = new FileCabinetMemoryService(new ValidatorBuilder().CreateCustom());
                        isCustom = true;
                        i++;
                        continue;
                    }

                    if (args[i] == "--storage" || args[i] == "-s")
                    {
                        if (args[i + 1].ToLower(CultureInfo.CurrentCulture) == "memory")
                        {
                            if (isCustom)
                            {
                                fileCabinetService = new FileCabinetMemoryService(new ValidatorBuilder().CreateCustom());
                                Console.WriteLine($"Using memory.");
                            }
                            else
                            {
                                fileCabinetService = new FileCabinetMemoryService(new ValidatorBuilder().CreateDefault());
                                Console.WriteLine($"Using memory.");
                            }

                            i += 2;
                        }
                        else
                        {
                            if (args[i + 1].ToLower(CultureInfo.CurrentCulture) == "file")
                            {
                                if (isCustom)
                                {
                                    fileCabinetService = new FileCabinetFilesystemService(fileStream, new ValidatorBuilder().CreateCustom());
                                    Console.WriteLine($"Using filesystem.");
                                }
                                else
                                {
                                    fileCabinetService = new FileCabinetFilesystemService(fileStream, new ValidatorBuilder().CreateDefault());
                                    Console.WriteLine($"Using filesystem.");
                                }
                            }

                            i += 2;
                        }

                        continue;
                    }

                    if (args[i] == "--use-stopwatch")
                    {
                        if (isCustom)
                        {
                            fileCabinetService = new ServiceMeter(new FileCabinetMemoryService(new ValidatorBuilder().CreateCustom()));
                            Console.WriteLine($"Using stopwatch");
                        }
                        else
                        {
                            fileCabinetService = new ServiceMeter(new FileCabinetMemoryService(new ValidatorBuilder().CreateDefault()));
                            Console.WriteLine($"Using stopwatch");
                        }

                        i++;
                        continue;
                    }

                    if (args[i] == "--use-logger")
                    {
                        if (isCustom)
                        {
                            fileCabinetService = new ServiceLogger(new ServiceMeter(new FileCabinetMemoryService(new ValidatorBuilder().CreateCustom())), "applicationLog.log");
                            Console.WriteLine($"Using logger");
                        }
                        else
                        {
                            fileCabinetService = new ServiceLogger(new ServiceMeter(new FileCabinetMemoryService(new ValidatorBuilder().CreateDefault())), "applicationLog.log");
                            Console.WriteLine($"Using logger");
                        }

                        i++;
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
                commandHandler.Handle(new CommandHandlers.AppCommandRequest { Command = command, Parameters = parameters, });
            }
            while (isRunning);
        }

        private static FileCabinetApp.CommandHandlers.ICommandHandler CreateCommandHandlers()
        {
            var helpCommandHandler = new HelpCommandHandler();
            var exitCommandHandler = new ExitCommandHandler(ExitOperation);
            var statCommandHandler = new StatCommandHandler(fileCabinetService);
            var createCommandHandler = new CreateCommandHandler(fileCabinetService, isCustom);
            var insertCommandHandler = new InsertCommandHandler(fileCabinetService, isCustom);
            var exportCommandHandler = new ExportCommandHandler(fileCabinetService);
            var importCommandHandler = new ImportCommandHandler(fileCabinetService);
            var purgeCommandHandler = new PurgeCommandHandler(fileCabinetService);
            var deleteCommandHandler = new DeleteCommandHandler(fileCabinetService);
            var updateCommandHandler = new UpdateCommandHandler(fileCabinetService);
            var selectCommandHandler = new SelectCommandHandler(fileCabinetService);
            var commandMissHandler = new MissedCommandHandler();

            helpCommandHandler.SetNext(exitCommandHandler);
            exitCommandHandler.SetNext(statCommandHandler);
            statCommandHandler.SetNext(createCommandHandler);
            createCommandHandler.SetNext(insertCommandHandler);
            insertCommandHandler.SetNext(exportCommandHandler);
            exportCommandHandler.SetNext(importCommandHandler);
            importCommandHandler.SetNext(purgeCommandHandler);
            purgeCommandHandler.SetNext(deleteCommandHandler);
            deleteCommandHandler.SetNext(updateCommandHandler);
            updateCommandHandler.SetNext(selectCommandHandler);
            selectCommandHandler.SetNext(commandMissHandler);
            return helpCommandHandler;
        }

        private static void ExitOperation(bool status)
        {
            isRunning = status;
        }
    }
}
