namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public FileCabinetRecord()
        {
            this.FirstName = "default";
            this.LastName = "default";
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}