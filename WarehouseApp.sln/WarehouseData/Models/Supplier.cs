namespace WarehouseData.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Supplier()
        {
        }

        public Supplier(int id, string name)
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


