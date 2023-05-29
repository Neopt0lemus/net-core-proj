using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Saveliy Maksimau";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static readonly FileCabinetService Service = new FileCabinetService();
        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("exit", Exit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "stat", "prints the number of records", "The 'stat' command prints the number of records." },
            new string[] { "create", "creates a new record in the program serivce." },
            new string[] { "edit", "edits a record from the service by id" },
            new string[] { "find", "finds all records according to property and parameter" },
            new string[] { "list", "returns all records from the service." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                var inputs = line != null ? line.Split(' ', 2) : new string[] { string.Empty, string.Empty };
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
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

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
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
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Service.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            string[] cr_str = new string[] { "First name: ", "Last name: ", "Date of birth: ", "Income per month: ", "Money: ", "Hometown first letter: " };
            StringBuilder firstname = new StringBuilder();
            StringBuilder lastname = new StringBuilder();
            DateTime date = DateTime.MinValue;
            short income = 0;
            decimal money = 0;
            char letter = ' ';
            for (int i = 0; i < cr_str.Length; i++)
            {
                bool flag = false;
                do
                {
                    Console.Write(cr_str[i]);
                    switch (i)
                    {
                        case 0:
                            firstname = new StringBuilder();
                            var c = Console.ReadLine();
                            firstname.Append(c);

                            if (!firstname.ToString().All(char.IsLetter) || firstname.Length < 2 || firstname.Length > 60)
                            {
                                Console.WriteLine("Invalid first name.");
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                            }

                            break;
                        case 1:
                            lastname = new StringBuilder();
                            c = Console.ReadLine();
                            lastname.Append(c);

                            if (!lastname.ToString().All(char.IsLetter) || lastname.Length < 2 || lastname.Length > 60)
                            {
                                Console.WriteLine("Invalid last name.");
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                            }

                            break;
                        case 2:
                            DateTime minimal_date = DateTime.Parse("01/01/1950", CultureInfo.CreateSpecificCulture("en-US"));
                            if (DateTime.TryParse(Console.ReadLine(), CultureInfo.CreateSpecificCulture("en-US"), out DateTime result) && result.Date >= minimal_date
                                && result.Date <= DateTime.Today)
                            {
                                date = result;
                                flag = false;
                            }
                            else
                            {
                                Console.WriteLine("Invalid date.\nCorrect format is: MM/DD/YYYY.\nMinimal date is 1950-Jan-01");
                                flag = true;
                            }

                            break;
                        case 3:
                            if (short.TryParse(Console.ReadLine(), out short res))
                            {
                                income = res;
                                flag = false;
                            }
                            else
                            {
                                Console.WriteLine("Invalid data.");
                                flag = true;
                            }

                            break;
                        case 4:
                            if (decimal.TryParse(Console.ReadLine(), CultureInfo.CreateSpecificCulture("en-US"), out decimal r))
                            {
                                money = r;
                                flag = false;
                            }
                            else
                            {
                                Console.WriteLine("Invalid data.");
                                flag = true;
                            }

                            break;
                        case 5:
                            if (char.TryParse(Console.ReadLine(), out char character) && char.IsLetter(character))
                            {
                                letter = character;
                                flag = false;
                            }
                            else
                            {
                                Console.WriteLine("Invalid character.");
                                flag = true;
                            }

                            break;
                    }
                }
                while (flag);
            }

            var recordId = Service.CreateRecord(firstname.ToString(), lastname.ToString(), date, income, money, letter);
            Console.WriteLine($"Record {recordId} is created.");
        }

        private static void Edit(string parameters)
        {
            int id = 0;
            if (int.TryParse(parameters, out int r))
            {
                id = r;
            }
            else
            {
                Console.WriteLine("Invalid id.");
                return;
            }

            if (id <= 0 || id > Service.GetStat())
            {
                Console.WriteLine($"Record #{id} is not found. Enter a number within the range of the list.");
                return;
            }

            string[] cr_str = new string[] { "First name: ", "Last name: ", "Date of birth: ", "Income per month: ", "Money: ", "Hometown first letter: " };
            StringBuilder firstname = new StringBuilder();
            StringBuilder lastname = new StringBuilder();
            DateTime date = DateTime.MinValue;
            short income = 0;
            decimal money = 0;
            char letter = ' ';
            for (int i = 0; i < cr_str.Length; i++)
            {
                bool flag = false;
                do
                {
                    Console.Write(cr_str[i]);
                    switch (i)
                    {
                        case 0:
                            firstname = new StringBuilder();
                            var c = Console.ReadLine();
                            firstname.Append(c);

                            if (!firstname.ToString().All(char.IsLetter) || firstname.Length < 2 || firstname.Length > 60)
                            {
                                Console.WriteLine("Invalid first name.");
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                            }

                            break;
                        case 1:
                            lastname = new StringBuilder();
                            c = Console.ReadLine();
                            lastname.Append(c);

                            if (!lastname.ToString().All(char.IsLetter) || lastname.Length < 2 || lastname.Length > 60)
                            {
                                Console.WriteLine("Invalid last name.");
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                            }

                            break;
                        case 2:
                            DateTime minimal_date = DateTime.Parse("01/01/1950", CultureInfo.CreateSpecificCulture("en-US"));
                            if (DateTime.TryParse(Console.ReadLine(), CultureInfo.CreateSpecificCulture("en-US"), out DateTime result) && result.Date >= minimal_date
                                && result.Date <= DateTime.Today)
                            {
                                date = result;
                                flag = false;
                            }
                            else
                            {
                                Console.WriteLine("Invalid date.\nCorrect format is: MM/DD/YYYY.\nMinimal date is 1950-Jan-01");
                                flag = true;
                            }

                            break;
                        case 3:
                            if (short.TryParse(Console.ReadLine(), out short res))
                            {
                                income = res;
                                flag = false;
                            }
                            else
                            {
                                Console.WriteLine("Invalid data.");
                                flag = true;
                            }

                            break;
                        case 4:
                            if (decimal.TryParse(Console.ReadLine(), CultureInfo.CreateSpecificCulture("en-US"), out decimal re))
                            {
                                money = re;
                                flag = false;
                            }
                            else
                            {
                                Console.WriteLine("Invalid data.");
                                flag = true;
                            }

                            break;
                        case 5:
                            if (char.TryParse(Console.ReadLine(), out char character) && char.IsLetter(character))
                            {
                                letter = character;
                                flag = false;
                            }
                            else
                            {
                                Console.WriteLine("Invalid character.");
                                flag = true;
                            }

                            break;
                    }
                }
                while (flag);
            }

            Service.EditRecord(id, firstname.ToString(), lastname.ToString(), date, income, money, letter);
            Console.WriteLine($"Record #{id} is updated.");
        }

        private static void Find(string parameters)
        {
            if (string.IsNullOrEmpty(parameters) || string.IsNullOrWhiteSpace(parameters))
            {
                Console.WriteLine("Invalid paramters. Correct format is: find <propertyName> \"name\" ");
                return;
            }

            var inputs = parameters.Split(' ');
            const int propertyIndex = 0;
            const int parameterIndex = 1;
            var list = new List<FileCabinetRecord>();
            if (inputs.Length < 2)
            {
                Console.WriteLine("Invalid paramters. Correct format is: find <propertyName> \"name\" ");
                return;
            }

            if (string.Equals(inputs[propertyIndex], "firstname", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    list = Service.FindByFirstName(inputs[parameterIndex]).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                    return;
                }
            }
            else if (string.Equals(inputs[propertyIndex], "lastname", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    list = Service.FindByLastName(inputs[parameterIndex]).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                    return;
                }
            }
            else if (string.Equals(inputs[propertyIndex], "dateofbirth", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    list = Service.FindByDateOfBirth(inputs[parameterIndex]).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                    return;
                }
            }
            else
            {
                Console.WriteLine($"There is no property '{inputs[propertyIndex]}'.");
                return;
            }

            if (!(inputs[parameterIndex].StartsWith('\"') && inputs[parameterIndex].EndsWith('\"')))
            {
                Console.WriteLine("Invalid paramters. Correct format is: find <propertyName> \"parameter\" ");
                return;
            }

            if (list.Count == 0)
            {
                Console.WriteLine($"No records with {inputs[propertyIndex]} {inputs[parameterIndex]} were found.");
            }
            else
            {
                foreach (var item in list)
                {
                    Console.WriteLine($"#{item.Id}, {item.FirstName}, {item.LastName}," +
                    $" {item.DateOfBirth.Year}-{item.DateOfBirth.ToString("MMM", CultureInfo.CreateSpecificCulture("en-US"))}" +
                    $"-{item.DateOfBirth.Day}, ${item.Income}, ${item.Money.ToString(CultureInfo.CreateSpecificCulture("en-US"))}," +
                    $" From {item.HometownFirstLetter}-Town");
                }
            }
        }

        private static void List(string parameters)
        {
            for (int i = 0; i < Service.GetStat(); i++)
            {
                Console.WriteLine($"#{Service.GetRecords()[i].Id}, {Service.GetRecords()[i].FirstName}, {Service.GetRecords()[i].LastName}," +
                    $" {Service.GetRecords()[i].DateOfBirth.Year}-{Service.GetRecords()[i].DateOfBirth.ToString("MMM", CultureInfo.CreateSpecificCulture("en-US"))}" +
                    $"-{Service.GetRecords()[i].DateOfBirth.Day}, ${Service.GetRecords()[i].Income}, ${Service.GetRecords()[i].Money.ToString(CultureInfo.CreateSpecificCulture("en-US"))}," +
                    $" From {Service.GetRecords()[i].HometownFirstLetter}-Town");
            }
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }
    }
}