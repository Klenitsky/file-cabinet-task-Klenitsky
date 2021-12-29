using FileCabinetApp.Consts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp.Iterators
{
    /// <summary>
    /// Iterator for FileCabinetMemoryService.
    /// </summary>
    public class FilesystemIterator : IRecordIterator
    {
        private readonly FileStream fileStream;
        private readonly List<long> positions;
        private int currentPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesystemIterator"/> class.
        /// </summary>
        /// <param name="positions">"List of positions in the file."</param>
        /// <param name="reader">Reader provided.</param>
        public FilesystemIterator(List<long> positions, FileStream reader)
        {
            this.fileStream = reader;
            this.positions = positions;
            this.currentPosition = 0;
        }

        /// <summary>
        /// Returns next value of the collection.
        /// </summary>
        /// <returns>Record from collection.</returns>
        public FileCabinetRecord GetNext()
        {
            this.fileStream.Seek(this.positions[this.currentPosition], SeekOrigin.Begin);
            FileCabinetRecord record;
            byte[] buffer = new byte[FileConsts.RecordSize];
            this.fileStream.Read(buffer, 0, buffer.Length);

            byte[] recordIdBuf = buffer[FileConsts.IdBegin..FileConsts.FirstNameBegin];
            byte[] firstNameBuf = buffer[FileConsts.FirstNameBegin..FileConsts.LastNameBegin];
            byte[] lastNameBuf = buffer[FileConsts.LastNameBegin..FileConsts.YearBegin];
            byte[] yearBuf = buffer[FileConsts.YearBegin..FileConsts.MonthBegin];
            byte[] monthBuf = buffer[FileConsts.MonthBegin..FileConsts.DayBegin];
            byte[] dayBuf = buffer[FileConsts.DayBegin..FileConsts.HeightBegin];
            byte[] heightBuf = buffer[FileConsts.HeightBegin..FileConsts.WeightBegin];
            byte[] weightBuf = buffer[FileConsts.WeightBegin..FileConsts.DrivingLicenseCategoryBegin];
            byte[] drivingLicenseCategoryBuf = buffer[FileConsts.DrivingLicenseCategoryBegin..FileConsts.RecordSize];

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
            this.currentPosition++;
            return record;
        }

        /// <summary>
        /// Indicates if there are more records in the cpllection.
        /// </summary>
        /// <returns>Bool indicator.</returns>
        public bool HasMore()
        {
            return this.currentPosition < this.positions.Count;
        }
    }
}
