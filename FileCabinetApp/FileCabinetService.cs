using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short income, decimal money, char letter)
        {
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
            if (this.firstNameDictionary.TryGetValue(record.FirstName.ToLowerInvariant(), out List<FileCabinetRecord> firstNameMatchList))
            {
                firstNameMatchList.Add(record);
            }
            else
            {
                this.firstNameDictionary.Add(record.FirstName.ToLowerInvariant(), new List<FileCabinetRecord> { record });
            }

            if (this.lastNameDictionary.TryGetValue(record.LastName.ToLowerInvariant(), out List<FileCabinetRecord> lastNameMatchList))
            {
                lastNameMatchList.Add(record);
            }
            else
            {
                this.lastNameDictionary.Add(record.LastName.ToLowerInvariant(), new List<FileCabinetRecord> { record });
            }

            if (this.dateOfBirthDictionary.TryGetValue(record.DateOfBirth, out List<FileCabinetRecord> dateOfBirthMatchList))
            {
                dateOfBirthMatchList.Add(record);
            }
            else
            {
                this.dateOfBirthDictionary.Add(record.DateOfBirth, new List<FileCabinetRecord> { record });
            }

            this.list.Add(record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short income, decimal money, char letter)
        {
            if (id > this.list.Count || id <= 0)
            {
                throw new ArgumentException($"#{id} record is not found.");
            }

            if (this.firstNameDictionary.TryGetValue(this.list[id - 1].FirstName.ToLowerInvariant(), out List<FileCabinetRecord> firstNameMatchList))
            {
                firstNameMatchList.Remove(this.list[id - 1]);
                if (firstNameMatchList.Count == 0)
                {
                    this.firstNameDictionary.Remove(this.list[id - 1].FirstName.ToLowerInvariant());
                }
            }

            if (this.lastNameDictionary.TryGetValue(this.list[id - 1].LastName.ToLowerInvariant(), out List<FileCabinetRecord> lastNameMatchList))
            {
                lastNameMatchList.Remove(this.list[id - 1]);
                if (lastNameMatchList.Count == 0)
                {
                    this.lastNameDictionary.Remove(this.list[id - 1].LastName.ToLowerInvariant());
                }
            }

            if (this.dateOfBirthDictionary.TryGetValue(this.list[id - 1].DateOfBirth, out List<FileCabinetRecord> dateOfBirthMatchList))
            {
                dateOfBirthMatchList.Remove(this.list[id - 1]);
                if (dateOfBirthMatchList.Count == 0)
                {
                    this.dateOfBirthDictionary.Remove(this.list[id - 1].DateOfBirth);
                }
            }

            this.list[id - 1].FirstName = firstName;
            this.list[id - 1].LastName = lastName;
            this.list[id - 1].DateOfBirth = dateOfBirth;
            this.list[id - 1].Income = income;
            this.list[id - 1].Money = money;
            this.list[id - 1].HometownFirstLetter = letter;
            if (this.firstNameDictionary.TryGetValue(this.list[id - 1].FirstName.ToLowerInvariant(), out List<FileCabinetRecord> editedFirstNameMatchList))
            {
                editedFirstNameMatchList.Add(this.list[id - 1]);
            }
            else
            {
                this.firstNameDictionary.Add(firstName.ToLowerInvariant(), new List<FileCabinetRecord> { this.list[id - 1] });
            }

            if (this.lastNameDictionary.TryGetValue(this.list[id - 1].LastName.ToLowerInvariant(), out List<FileCabinetRecord> editedLastNameMatchList))
            {
                editedLastNameMatchList.Add(this.list[id - 1]);
            }
            else
            {
                this.lastNameDictionary.Add(lastName.ToLowerInvariant(), new List<FileCabinetRecord> { this.list[id - 1] });
            }

            if (this.dateOfBirthDictionary.TryGetValue(this.list[id - 1].DateOfBirth, out List<FileCabinetRecord> editedDateOfBirthMatchList))
            {
                editedDateOfBirthMatchList.Add(this.list[id - 1]);
            }
            else
            {
                this.dateOfBirthDictionary.Add(dateOfBirth, new List<FileCabinetRecord> { this.list[id - 1] });
            }
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            firstName = firstName.Replace("\"", string.Empty, StringComparison.OrdinalIgnoreCase);
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentNullException(nameof(firstName), "First name is null");
            }

            if (!firstName.ToString().All(char.IsLetter) || firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException("Invalid first name", nameof(firstName));
            }

            if (this.firstNameDictionary.TryGetValue(firstName.ToLowerInvariant(), out List<FileCabinetRecord> firstNameMatchList))
            {
                return firstNameMatchList.ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            lastName = lastName.Replace("\"", string.Empty, StringComparison.OrdinalIgnoreCase);
            if (string.IsNullOrEmpty(lastName) || string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException(nameof(lastName), "Last name is null");
            }

            if (!lastName.ToString().All(char.IsLetter) || lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException("Invalid Last name", nameof(lastName));
            }

            if (this.lastNameDictionary.TryGetValue(lastName.ToLowerInvariant(), out List<FileCabinetRecord> lastNameMatchList))
            {
                return lastNameMatchList.ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        public FileCabinetRecord[] FindByDateOfBirth(string date)
        {
            date = date.Replace("\"", string.Empty, StringComparison.OrdinalIgnoreCase);
            DateTime minimal_date = DateTime.Parse("01/01/1950", CultureInfo.CreateSpecificCulture("en-US"));
            if (string.IsNullOrEmpty(date) || string.IsNullOrWhiteSpace(date))
            {
                throw new ArgumentNullException(nameof(date), "Date is null");
            }

            if (!(DateTime.TryParse(date, CultureInfo.CreateSpecificCulture("en-US"), out DateTime result) && result.Date >= minimal_date
                                && result.Date <= DateTime.Today))
            {
                throw new ArgumentException("Invalid date", nameof(date));
            }
            else
            {
                if (this.dateOfBirthDictionary.TryGetValue(result.Date, out List<FileCabinetRecord> dateOfBirthMatchList))
                {
                    return dateOfBirthMatchList.ToArray();
                }
            }
            //var lastNameList = new List<FileCabinetRecord>();
            //foreach (var record in this.list.Where(x => DateTime.Equals(x.DateOfBirth, result.Date)))
            //{
            //    lastNameList.Add(record);
            //}

            return Array.Empty<FileCabinetRecord>();
        }
    }
}