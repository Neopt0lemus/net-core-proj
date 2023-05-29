namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short income, decimal money, char letter)
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
                Income = income,
                Money = money,
                HometownFirstLetter = letter,
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

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short income, decimal money, char letter)
        {
            if (id > this.list.Count || id <= 0)
            {
                throw new ArgumentException($"#{id} record is not found.");
            }

            this.list[id - 1].FirstName = firstName;
            this.list[id - 1].LastName = lastName;
            this.list[id - 1].DateOfBirth = dateOfBirth;
            this.list[id - 1].Income = income;
            this.list[id - 1].Money = money;
            this.list[id - 1].HometownFirstLetter = letter;
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            firstName = firstName.Replace("\"", string.Empty);
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentNullException(nameof(firstName), "First name is null");
            }

            if (!firstName.ToString().All(char.IsLetter) || firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException("Invalid first name", nameof(firstName));
            }

            var firstNameList = new List<FileCabinetRecord>();
            foreach (var record in this.list.Where(x => string.Equals(x.FirstName, firstName, StringComparison.OrdinalIgnoreCase)))
            {
                firstNameList.Add(record);
            }

            return firstNameList.ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            lastName = lastName.Replace("\"", string.Empty);
            if (string.IsNullOrEmpty(lastName) || string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException(nameof(lastName), "Last name is null");
            }

            if (!lastName.ToString().All(char.IsLetter) || lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException("Invalid Last name", nameof(lastName));
            }

            var lastNameList = new List<FileCabinetRecord>();
            foreach (var record in this.list.Where(x => string.Equals(x.LastName, lastName, StringComparison.OrdinalIgnoreCase)))
            {
                lastNameList.Add(record);
            }

            return lastNameList.ToArray();
        }
    }
}