namespace AppForDevices.Shared.RepairDTOs
{
    public class RepairDTO
    {
        public RepairDTO(string name, double cost, string scale, string description)
        {
            Name = name;
            Cost = cost;
            Scale = scale;
            Description = description;
        }
        public RepairDTO(int id, string name, double cost, string scale, string description)
        {
            Id = id;
            Name = name;
            Cost = cost;
            Scale = scale;
            Description = description;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public string Scale { get; set; }
        public string Description { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is RepairDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Cost == dTO.Cost &&
                   Scale == dTO.Scale &&
                   Description == dTO.Description;
        }
    }
}
