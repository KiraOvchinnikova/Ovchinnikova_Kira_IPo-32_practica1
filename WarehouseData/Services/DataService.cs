using System.Collections.Generic;
using System.Linq;
using WarehouseData.Models;

namespace WarehouseData.Services
{
    public class DataService
    {
        public List<Organization> Organizations { get; set; } = new List<Organization>();
        public List<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Invoice> Invoices { get; set; } = new List<Invoice>();

        public DataService()
        {
            Organizations.Add(new Organization(1, "Детский Мир"));
            Warehouses.Add(new Warehouse(1, 1, "Склад игрушек", "ул. Игрушечная, 5"));
            Products.Add(new Product { Id = 1, WarehouseId = 1, Name = "Лего", Category = "Игрушки", Manufacturer = "LEGO", Supplier = "LEGO", Price = 500, StockQuantity = 10, DiscountPercent = 0 });
            Products.Add(new Product { Id = 2, WarehouseId = 1, Name = "Кукла", Category = "Игрушки", Manufacturer = "Barbie", Supplier = "Barbie", Price = 1200, StockQuantity = 5, DiscountPercent = 0 });
        }
    }
}



