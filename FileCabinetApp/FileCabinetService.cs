namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth)
        {
            // TODO: добавьте реализацию метода
            if (string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException(nameof(firstName), "something is null");
            }

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
            };

            this.list.Add(record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            // TODO: добавьте реализацию метода
            return this.list.ToArray();
        }

        public int GetStat()
        {
            // TODO: добавьте реализацию метода
            return this.list.Count;
        }
    }
}