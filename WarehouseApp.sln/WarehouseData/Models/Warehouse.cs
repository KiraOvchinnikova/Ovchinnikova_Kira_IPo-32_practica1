namespace WarehouseData.Models
{
    public class Warehouse
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public Warehouse(int id, int orgId, string name, string address, int organizationId)
        {
            Id = id;
            OrganizationId = orgId;
            Name = name;
            Address = address;
            OrganizationId = organizationId;
        }

        public Warehouse(int id, int organizationId, string name, string address)
        {
            Id = id;
            OrganizationId = organizationId;
            Name = name;
            Address = address;
        }
    }
}