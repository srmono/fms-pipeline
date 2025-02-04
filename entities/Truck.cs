namespace fleetmanagement.entities
{
    public class Truck
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }

        public Truck(string name, string model, int year)
        {
            Name = name;
            Model = model;
            Year = year;
        }
    }
}
