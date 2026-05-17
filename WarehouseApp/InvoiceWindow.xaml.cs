using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WarehouseData.Models;
using WarehouseData.Services;

namespace WarehouseApp.Views
{
    public partial class InvoiceWindow : Window
    {
        private readonly DataService dataService;
        private readonly Warehouse warehouse;

        public List<ProductForInvoice> ProductList { get; set; }

        public InvoiceWindow(DataService service, Warehouse wh)
        {
            InitializeComponent();
            dataService = service;
            warehouse = wh;

            ProductList = dataService.Products
                .Where(p => p.WarehouseId == warehouse.Id)
                .Select(p => new ProductForInvoice
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category,
                    Manufacturer = p.Manufacturer,
                    Supplier = p.Supplier,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    QuantityToMove = 0
                }).ToList();

            ProductsGrid.ItemsSource = ProductList;
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            bool isOutgoing = OutgoingRadio.IsChecked == true;

            foreach (var item in ProductList)
            {
                if (item.QuantityToMove <= 0) continue;

                var product = dataService.Products.First(p => p.Id == item.Id);

                if (isOutgoing)
                {
                    if (item.QuantityToMove > product.StockQuantity)
                    {
                        MessageBox.Show($"Невозможно списать {item.QuantityToMove} единиц '{product.Name}' — на складе только {product.StockQuantity}.",
                                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    product.StockQuantity -= item.QuantityToMove;
                }
                else
                {
                    product.StockQuantity += item.QuantityToMove;
                }
            }

            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }

    public class ProductForInvoice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Manufacturer { get; set; }
        public string Supplier { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int QuantityToMove { get; set; }
    }
}