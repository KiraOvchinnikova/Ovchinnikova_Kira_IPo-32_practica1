using System.Linq;
using WarehouseData.Models;
using WarehouseData.Services;
using Xunit;

namespace WarehouseApp_Tests_xUnit

{
    public class UnitTest1

    {

        private DataService dataService;

        public UnitTest1()

        {

            dataService = new DataService();

            var org = new Organization(1, "Тестовая организация");

            dataService.Organizations.Add(org);

            var warehouse = new Warehouse(1, org.Id, "Склад 1", "Адрес склада 1");

            dataService.Warehouses.Add(warehouse);

            dataService.Products.Add(new Product

            {

                Id = 1,

                WarehouseId = warehouse.Id,

                Name = "Товар 1",

                Category = "Категория 1",

                Manufacturer = "Производитель 1",

                Supplier = "Поставщик 1",

                Price = 100,

                StockQuantity = 10,

                DiscountPercent = 0

            });

            dataService.Products.Add(new Product

            {

                Id = 2,

                WarehouseId = warehouse.Id,

                Name = "Товар 2",

                Category = "Категория 2",

                Manufacturer = "Производитель 2",

                Supplier = "Поставщик 2",

                Price = 200,

                StockQuantity = 5,

                DiscountPercent = 5

            });

        }
        [Fact]
        public void AddOrganization_ShouldIncreaseCount()

        {

            int initialCount = dataService.Organizations.Count;

            dataService.Organizations.Add(new Organization(999, "Новая организация"));

            Assert.Equal(initialCount + 1, dataService.Organizations.Count);

        }

        [Fact]
        public void AddWarehouse_ShouldAssignCorrectOrganizationId()

        {

            var org = dataService.Organizations.First();

            int initialCount = dataService.Warehouses.Count;

            var warehouse = new Warehouse(999, org.Id, "Новый склад", "Новый адрес");

            dataService.Warehouses.Add(warehouse);

            Assert.Equal(initialCount + 1, dataService.Warehouses.Count);

            Assert.Equal(org.Id, warehouse.OrganizationId);

        }

        [Fact]
        public void AddProduct_ShouldAssignCorrectWarehouseId()

        {

            var warehouse = dataService.Warehouses.First();

            int initialCount = dataService.Products.Count;

            var product = new Product

            {

                Id = 999,

                WarehouseId = warehouse.Id,

                Name = "Тестовый товар",

                Category = "Тест",

                Manufacturer = "Производитель",

                Supplier = "Поставщик",

                Price = 100,

                StockQuantity = 10,

                DiscountPercent = 0

            };

            dataService.Products.Add(product);

            Assert.Equal(initialCount + 1, dataService.Products.Count);

            Assert.Equal(warehouse.Id, product.WarehouseId);

        }

        [Fact]
        public void DeleteWarehouse_ShouldRemoveAssociatedProducts()

        {

            var warehouse = dataService.Warehouses.First();

            var productsInWarehouse = dataService.Products

                .Where(p => p.WarehouseId == warehouse.Id)

                .ToList();


            foreach (var p in productsInWarehouse)

            {

                dataService.Products.Remove(p);

            }

            dataService.Warehouses.Remove(warehouse);

            foreach (var p in productsInWarehouse)

            {

                Assert.DoesNotContain(p, dataService.Products);

            }

        }

        [Fact]
        public void IncomingInvoice_ShouldIncreaseStock()

        {

            var warehouse = dataService.Warehouses.First();

            var product = dataService.Products.First(p => p.WarehouseId == warehouse.Id);

            int oldStock = product.StockQuantity;

            int quantity = 5;

            product.StockQuantity += quantity;

            Assert.Equal(oldStock + quantity, product.StockQuantity);

        }

        [Fact]
        public void OutgoingInvoice_ShouldDecreaseStock()

        {

            var warehouse = dataService.Warehouses.First();

            var product = dataService.Products.First(p => p.WarehouseId == warehouse.Id);

            int oldStock = product.StockQuantity;

            int quantity = 3;

            if (oldStock >= quantity)

                product.StockQuantity -= quantity;

            Assert.Equal(oldStock - quantity, product.StockQuantity);

        }

    }

}