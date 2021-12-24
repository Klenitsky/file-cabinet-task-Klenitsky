using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Service that works with FileStream.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private readonly FileStream fileStream;
        private readonly IRecordValidator validator;
        private int id = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">Filestream provided.</param>
        /// <param name="validator">Validator provided.</param>
        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator)
        {
            this.fileStream = fileStream;
            this.validator = validator;
            this.id = (int)(this.fileStream.Length / 270) + 1;
        }

        /// <summary>
        /// Creates a new Record.
        /// </summary>
        /// <param name="arguments">Properties of the record.</param>
        /// <returns>New record's Id.</returns>
        public int CreateRecord(Arguments arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            this.validator.ValidateParameters(arguments);
            short st = 1;
            byte[] status = BitConverter.GetBytes(st);
            byte[] recordId = BitConverter.GetBytes(this.id);
            byte[] firstName = Encoding.UTF8.GetBytes(arguments.FirstName);

            byte[] firstNameResult = new byte[120];
            for (int i = 0; i < firstName.Length; i++)
            {
                firstNameResult[i] = firstName[i];
            }

            byte[] lastName = Encoding.UTF8.GetBytes(arguments.LastName);
            byte[] lastNameResult = new byte[120];
            for (int i = 0; i < lastName.Length; i++)
            {
                lastNameResult[i] = lastName[i];
            }

            byte[] year = BitConverter.GetBytes(arguments.DateOfBirth.Year);
            byte[] month = BitConverter.GetBytes(arguments.DateOfBirth.Month);
            byte[] day = BitConverter.GetBytes(arguments.DateOfBirth.Day);

            byte[] height = BitConverter.GetBytes(arguments.Height);
            byte[] weight = BitConverter.GetBytes(decimal.ToDouble(arguments.Weight));
            byte[] drivingLicenseCategory = BitConverter.GetBytes(arguments.DrivingLicenseCategory);
            this.id++;

            this.fileStream.Write(status, 0, status.Length);
            this.fileStream.Write(recordId, 0, recordId.Length);
            this.fileStream.Write(firstNameResult, 0, firstNameResult.Length);
            this.fileStream.Write(lastNameResult, 0, lastNameResult.Length);
            this.fileStream.Write(year, 0, year.Length);
            this.fileStream.Write(month, 0, month.Length);
            this.fileStream.Write(day, 0, day.Length);
            this.fileStream.Write(height, 0, height.Length);
            this.fileStream.Write(weight, 0, weight.Length);
            this.fileStream.Write(drivingLicenseCategory, 0, drivingLicenseCategory.Length);
            this.fileStream.Flush();

            return this.id;
        }

        /// <summary>
        /// Gets a copy of the list.
        /// </summary>
        /// <returns>Array of records.</returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            if (this.fileStream.Length % 270 != 0)
            {
                return result;
            }

            int index = 0;
            this.fileStream.Seek(index, SeekOrigin.Begin);
            while (index < this.fileStream.Length)
            {
                FileCabinetRecord record;
                byte[] buffer = new byte[270];
                this.fileStream.Read(buffer, 0, buffer.Length);
                byte[] recordIdBuf = buffer[2..6];
                byte[] firstNameBuf = buffer[6..126];
                byte[] lastNameBuf = buffer[126..246];
                byte[] yearBuf = buffer[246..250];
                byte[] monthBuf = buffer[250..254];
                byte[] dayBuf = buffer[254..258];
                byte[] heightBuf = buffer[258..260];
                byte[] weightBuf = buffer[260..268];
                byte[] drivingLicenseCategoryBuf = buffer[268..270];

                int recordId = BitConverter.ToInt32(recordIdBuf);
                string firstName = Encoding.UTF8.GetString(firstNameBuf);
                string lastName = Encoding.UTF8.GetString(lastNameBuf);
                DateTime dateOfBirth = new DateTime(BitConverter.ToInt32(yearBuf), BitConverter.ToInt32(monthBuf), BitConverter.ToInt32(dayBuf));
                short height = BitConverter.ToInt16(heightBuf);
                decimal weight = new decimal(BitConverter.ToDouble(weightBuf));
                char drivingLicenseCategory = Encoding.UTF8.GetString(drivingLicenseCategoryBuf)[0];

                record = new FileCabinetRecord
                {
                    Id = recordId,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    Height = height,
                    Weight = weight,
                    DrivingLicenseCategory = drivingLicenseCategory,
                };
                result.Add(record);
                index += 270;
            }

            return result;
        }

        /// <summary>
        /// Gets a number of records.
        /// </summary>
        /// <returns>Number of records.</returns>
        public int GetStat()
        {
            return (int)(this.fileStream.Length / 270);
        }

        /// <summary>
        /// Edits an existing record.
        /// </summary>
        /// <param name="id">The ID of a record.</param>
        /// <param name="arguments">Properties of the record.</param>
        /// <exception cref="ArgumentNullException">One of the parameters is null.</exception>
        /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
        public void EditRecord(int id, Arguments arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            int index = 0;
            this.fileStream.Seek(index, SeekOrigin.Begin);
            while (index < this.fileStream.Length)
            {
                byte[] buffer = new byte[270];
                this.fileStream.Read(buffer, 0, buffer.Length);
                byte[] recordIdBuf = buffer[2..6];

                int recordId = BitConverter.ToInt32(recordIdBuf);
                if (recordId == id)
                {
                    this.validator.ValidateParameters(arguments);
                    this.fileStream.Seek(index, SeekOrigin.Begin);
                    short st = 1;
                    byte[] status = BitConverter.GetBytes(st);
                    byte[] firstName = Encoding.UTF8.GetBytes(arguments.FirstName);

                    byte[] firstNameResult = new byte[120];
                    for (int i = 0; i < firstName.Length; i++)
                    {
                        firstNameResult[i] = firstName[i];
                    }

                    byte[] lastName = Encoding.UTF8.GetBytes(arguments.LastName);
                    byte[] lastNameResult = new byte[120];
                    for (int i = 0; i < lastName.Length; i++)
                    {
                        lastNameResult[i] = lastName[i];
                    }

                    byte[] year = BitConverter.GetBytes(arguments.DateOfBirth.Year);
                    byte[] month = BitConverter.GetBytes(arguments.DateOfBirth.Month);
                    byte[] day = BitConverter.GetBytes(arguments.DateOfBirth.Day);

                    byte[] height = BitConverter.GetBytes(arguments.Height);
                    byte[] weight = BitConverter.GetBytes(decimal.ToDouble(arguments.Weight));
                    byte[] drivingLicenseCategory = BitConverter.GetBytes(arguments.DrivingLicenseCategory);
                    this.fileStream.Write(status, 0, status.Length);
                    this.fileStream.Write(recordIdBuf, 0, recordIdBuf.Length);
                    this.fileStream.Write(firstNameResult, 0, firstNameResult.Length);
                    this.fileStream.Write(lastNameResult, 0, lastNameResult.Length);
                    this.fileStream.Write(year, 0, year.Length);
                    this.fileStream.Write(month, 0, month.Length);
                    this.fileStream.Write(day, 0, day.Length);
                    this.fileStream.Write(height, 0, height.Length);
                    this.fileStream.Write(weight, 0, weight.Length);
                    this.fileStream.Write(drivingLicenseCategory, 0, drivingLicenseCategory.Length);
                    this.fileStream.Flush();
                    break;
                }

                index += 270;
            }
        }

        /// <summary>
        /// Finds all records with given firstname.
        /// </summary>
        /// <param name="firstName">The first name of the person.</param>
        /// <returns>A list of records found.</returns>
        /// <exception cref="ArgumentNullException">String firstName is null.</exception>
        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (firstName == null)
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            if (this.fileStream.Length % 270 != 0)
            {
                return result;
            }

            int index = 0;
            this.fileStream.Seek(index, SeekOrigin.Begin);
            while (index < this.fileStream.Length)
            {
                byte[] buffer = new byte[270];
                this.fileStream.Read(buffer, 0, buffer.Length);
                byte[] firstNameBuf = buffer[6..126];
                string firstNameRecord = Encoding.UTF8.GetString(firstNameBuf);
                if (firstNameRecord[0..firstName.Length].ToUpperInvariant() == firstName.ToUpperInvariant())
                {
                    FileCabinetRecord record;
                    byte[] recordIdBuf = buffer[2..6];
                    byte[] lastNameBuf = buffer[126..246];
                    byte[] yearBuf = buffer[246..250];
                    byte[] monthBuf = buffer[250..254];
                    byte[] dayBuf = buffer[254..258];
                    byte[] heightBuf = buffer[258..260];
                    byte[] weightBuf = buffer[260..268];
                    byte[] drivingLicenseCategoryBuf = buffer[268..270];
                    int recordId = BitConverter.ToInt32(recordIdBuf);
                    string lastName = Encoding.UTF8.GetString(lastNameBuf);
                    DateTime dateOfBirth = new DateTime(BitConverter.ToInt32(yearBuf), BitConverter.ToInt32(monthBuf), BitConverter.ToInt32(dayBuf));
                    short height = BitConverter.ToInt16(heightBuf);
                    decimal weight = new decimal(BitConverter.ToDouble(weightBuf));
                    char drivingLicenseCategory = Encoding.UTF8.GetString(drivingLicenseCategoryBuf)[0];

                    record = new FileCabinetRecord
                    {
                        Id = recordId,
                        FirstName = firstNameRecord,
                        LastName = lastName,
                        DateOfBirth = dateOfBirth,
                        Height = height,
                        Weight = weight,
                        DrivingLicenseCategory = drivingLicenseCategory,
                    };
                    result.Add(record);
                }

                index += 270;
            }

            return result;
        }

        /// <summary>
        /// Finds all records with given lastname.
        /// </summary>
        /// <param name="lastName">The last name of the person.</param>
        /// <returns>A list of records found.</returns>
        /// <exception cref="ArgumentNullException">String firstName is null.</exception>
        public IReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (lastName == null)
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            if (this.fileStream.Length % 270 != 0)
            {
                return result;
            }

            int index = 0;
            this.fileStream.Seek(index, SeekOrigin.Begin);
            while (index < this.fileStream.Length)
            {
                byte[] buffer = new byte[270];
                this.fileStream.Read(buffer, 0, buffer.Length);

                byte[] lastNameBuf = buffer[126..246];
                string lastNameRecord = Encoding.UTF8.GetString(lastNameBuf);
                if (lastNameRecord[0..lastName.Length].ToUpperInvariant() == lastName.ToUpperInvariant())
                {
                    FileCabinetRecord record;
                    byte[] recordIdBuf = buffer[2..6];
                    byte[] firstNameBuf = buffer[6..126];
                    byte[] yearBuf = buffer[246..250];
                    byte[] monthBuf = buffer[250..254];
                    byte[] dayBuf = buffer[254..258];
                    byte[] heightBuf = buffer[258..260];
                    byte[] weightBuf = buffer[260..268];
                    byte[] drivingLicenseCategoryBuf = buffer[268..270];
                    int recordId = BitConverter.ToInt32(recordIdBuf);
                    string firstNameRecord = Encoding.UTF8.GetString(firstNameBuf);
                    DateTime dateOfBirth = new DateTime(BitConverter.ToInt32(yearBuf), BitConverter.ToInt32(monthBuf), BitConverter.ToInt32(dayBuf));
                    short height = BitConverter.ToInt16(heightBuf);
                    decimal weight = new decimal(BitConverter.ToDouble(weightBuf));
                    char drivingLicenseCategory = Encoding.UTF8.GetString(drivingLicenseCategoryBuf)[0];

                    record = new FileCabinetRecord
                    {
                        Id = recordId,
                        FirstName = firstNameRecord,
                        LastName = lastNameRecord,
                        DateOfBirth = dateOfBirth,
                        Height = height,
                        Weight = weight,
                        DrivingLicenseCategory = drivingLicenseCategory,
                    };
                    result.Add(record);
                }

                index += 270;
            }

            return result;
        }

        /// <summary>
        /// Finds all records with given date of Birth.
        /// </summary>
        /// <param name="dateTime">The date of birth of the person.</param>
        /// <returns>A list of records found.</returns>
        public IReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateTime)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            if (this.fileStream.Length % 270 != 0)
            {
                return result;
            }

            int index = 0;
            this.fileStream.Seek(index, SeekOrigin.Begin);
            while (index < this.fileStream.Length)
            {
                byte[] buffer = new byte[270];
                this.fileStream.Read(buffer, 0, buffer.Length);

                byte[] yearBuf = buffer[246..250];
                byte[] monthBuf = buffer[250..254];
                byte[] dayBuf = buffer[254..258];

                if (BitConverter.ToInt32(yearBuf) == dateTime.Year && BitConverter.ToInt32(monthBuf) == dateTime.Month && BitConverter.ToInt32(dayBuf) == dateTime.Day)
                {
                    FileCabinetRecord record;
                    byte[] recordIdBuf = buffer[2..6];
                    byte[] firstNameBuf = buffer[6..126];
                    byte[] lastNameBuf = buffer[126..246];
                    byte[] heightBuf = buffer[258..260];
                    byte[] weightBuf = buffer[260..268];
                    byte[] drivingLicenseCategoryBuf = buffer[268..270];
                    int recordId = BitConverter.ToInt32(recordIdBuf);
                    string firstNameRecord = Encoding.UTF8.GetString(firstNameBuf);
                    string lastNameRecord = Encoding.UTF8.GetString(lastNameBuf);
                    DateTime dateOfBirth = new DateTime(BitConverter.ToInt32(yearBuf), BitConverter.ToInt32(monthBuf), BitConverter.ToInt32(dayBuf));
                    short height = BitConverter.ToInt16(heightBuf);
                    decimal weight = new decimal(BitConverter.ToDouble(weightBuf));
                    char drivingLicenseCategory = Encoding.UTF8.GetString(drivingLicenseCategoryBuf)[0];

                    record = new FileCabinetRecord
                    {
                        Id = recordId,
                        FirstName = firstNameRecord,
                        LastName = lastNameRecord,
                        DateOfBirth = dateOfBirth,
                        Height = height,
                        Weight = weight,
                        DrivingLicenseCategory = drivingLicenseCategory,
                    };
                    result.Add(record);
                }

                index += 270;
            }

            return result;
        }

        /// <summary>
        /// Generates snapshot of the service.
        /// </summary>
        /// <returns>A snapshot of this service.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            if (this.fileStream.Length % 270 != 0)
            {
                return new FileCabinetServiceSnapshot(result);
            }

            int index = 0;
            this.fileStream.Seek(index, SeekOrigin.Begin);
            while (index < this.fileStream.Length)
            {
                FileCabinetRecord record;
                byte[] buffer = new byte[270];
                this.fileStream.Read(buffer, 0, buffer.Length);
                byte[] recordIdBuf = buffer[2..6];
                byte[] firstNameBuf = buffer[6..126];
                byte[] lastNameBuf = buffer[126..246];
                byte[] yearBuf = buffer[246..250];
                byte[] monthBuf = buffer[250..254];
                byte[] dayBuf = buffer[254..258];
                byte[] heightBuf = buffer[258..260];
                byte[] weightBuf = buffer[260..268];
                byte[] drivingLicenseCategoryBuf = buffer[268..270];

                int recordId = BitConverter.ToInt32(recordIdBuf);
                string firstName = Encoding.UTF8.GetString(firstNameBuf);
                string lastName = Encoding.UTF8.GetString(lastNameBuf);
                for (int i = 0; i < firstName.Length; i++)
                {
                    if (firstName[i] == '\0')
                    {
                        firstName = firstName[0..i];
                    }
                }

                for (int i = 0; i < lastName.Length; i++)
                {
                    if (lastName[i] == '\0')
                    {
                        lastName = lastName[0..i];
                    }
                }

                DateTime dateOfBirth = new DateTime(BitConverter.ToInt32(yearBuf), BitConverter.ToInt32(monthBuf), BitConverter.ToInt32(dayBuf));
                short height = BitConverter.ToInt16(heightBuf);
                decimal weight = new decimal(BitConverter.ToDouble(weightBuf));
                char drivingLicenseCategory = Encoding.UTF8.GetString(drivingLicenseCategoryBuf)[0];

                record = new FileCabinetRecord
                {
                    Id = recordId,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    Height = height,
                    Weight = weight,
                    DrivingLicenseCategory = drivingLicenseCategory,
                };
                result.Add(record);
                index += 270;
            }

            return new FileCabinetServiceSnapshot(result);
        }

        /// <summary>
        /// Adds records loaded from file.
        /// </summary>
        /// <param name="snapshot">Properties of the record.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            foreach (FileCabinetRecord record in snapshot.Records)
            {
                if (record.Id < this.id)
                {
                    this.EditRecord(record.Id, new Arguments(record.FirstName, record.LastName, record.DateOfBirth, record.Height, record.Weight, record.DrivingLicenseCategory));
                }
                else
                {
                    this.id = record.Id;
                    this.CreateRecord(new Arguments(record.FirstName, record.LastName, record.DateOfBirth, record.Height, record.Weight, record.DrivingLicenseCategory));
                }
            }
        }
    }
}
