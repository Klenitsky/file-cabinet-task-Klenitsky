using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FileCabinetApp.Iterators;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handler of find command.
    /// </summary>
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private readonly Action<IEnumerable<FileCabinetRecord>> printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service provided.</param>
        /// <param name="recordPrinter">Printer provided.</param>
        public FindCommandHandler(IFileCabinetService fileCabinetService, Action<IEnumerable<FileCabinetRecord>> recordPrinter)
        {
            this.fileCabinetService = fileCabinetService;
            this.printer = recordPrinter;
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

            if (request.Command == "find")
            {
                this.printer(this.Find(request.Parameters));
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }

        private IEnumerable<FileCabinetRecord> Find(string parameters)
        {
            int index = parameters.IndexOf(' ', StringComparison.InvariantCulture);
            StringBuilder property = new StringBuilder(parameters, 0, index, char.MaxValue);
            IEnumerable<FileCabinetRecord> result = new MemoryIterator(new List<FileCabinetRecord>());

            if (property.ToString().ToLower(CultureInfo.CurrentCulture) == "firstname".ToLower(CultureInfo.CurrentCulture))
            {
                StringBuilder name = new StringBuilder(parameters, index + 1, parameters.Length - (index + 1), 255);
                while ((name.Length < 2) || (name.Length > 60))
                {
                    Console.WriteLine("First name is invalid, try again: ");
                    Console.Write("First Name: ");
                    name = new StringBuilder(Console.ReadLine());
                }

                result = this.fileCabinetService.FindByFirstName(name.ToString());
            }

            if (property.ToString().ToLower(CultureInfo.CurrentCulture) == "lastname".ToLower(CultureInfo.CurrentCulture))
            {
                StringBuilder name = new StringBuilder(parameters, index + 1, parameters.Length - index - 1, 255);
                while ((name.Length < 2) || (name.Length > 60))
                {
                    Console.WriteLine("First name is invalid, try again: ");
                    Console.Write("Last Name: ");
                    name = new StringBuilder(Console.ReadLine());
                }

                result = this.fileCabinetService.FindByLastName(name.ToString());
            }

            if (property.ToString().ToLower(CultureInfo.CurrentCulture) == "dateofbirth".ToLower(CultureInfo.CurrentCulture))
            {
                StringBuilder date = new StringBuilder(parameters, index + 1, parameters.Length - index - 1, 255);
                DateTime dateTime;
                bool success = DateTime.TryParse(date.ToString(), out dateTime);
                while (!success)
                {
                    Console.WriteLine("Date  is invalid, try again: ");
                    Console.Write("Date: ");
                    date = new StringBuilder(Console.ReadLine());
                    success = DateTime.TryParse(date.ToString(), out dateTime);
                }

                result = this.fileCabinetService.FindByDateOfBirth(dateTime);
            }

            return result;
        }
    }
}
