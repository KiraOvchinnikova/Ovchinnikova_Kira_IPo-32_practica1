namespace WarehouseData.Models
{
    public class Manufacturer
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Manufacturer()
        {
        }

        public Manufacturer(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}


