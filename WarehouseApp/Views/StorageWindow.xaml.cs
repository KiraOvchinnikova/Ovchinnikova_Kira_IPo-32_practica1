using System;

using System.Linq;

using System.Windows;

using WarehouseData.Models;

using WarehouseData.Services;

using Microsoft.Win32;

namespace WarehouseApp.Views

{

    public partial class StorageWindow : Window

    {

        private readonly DataService dataService;

        private readonly Organization organization;

        public StorageWindow(Organization org)

        {

            InitializeComponent();

            organization = org;

            dataService = new DataService();

            OrganizationText.Text = organization.Name;

            LoadWarehouses();

        }

        private void LoadWarehouses()

        {

            var warehouses = dataService.Warehouses.Where(w => w.OrganizationId == organization.Id).ToList();

            WarehousesList.ItemsSource = warehouses;

            if (warehouses.Count > 0)

                WarehousesList.SelectedIndex = 0;

        }

        private void WarehousesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)

        {

            LoadProducts();

            UpdateCurrentWarehouseText();

        }

        private void LoadProducts()

        {

            var warehouse = WarehousesList.SelectedItem as Warehouse;

            ProductsGrid.ItemsSource = warehouse == null ? null : dataService.Products.Where(p => p.WarehouseId == warehouse.Id).ToList();

        }

        private void UpdateCurrentWarehouseText()

        {

            var warehouse = WarehousesList.SelectedItem as Warehouse;

            string warehouseName = warehouse?.Name ?? "склад не выбран";

            CurrentWarehouseText.Text = warehouseName;

            StatusText.Text = $"Организация: {organization.Name} | Склад: {warehouseName}";

        }
        private void AddWarehouse_Click(object sender, RoutedEventArgs e)

        {

            var window = new WarehouseWindow();

            window.Owner = this;

            if (window.ShowDialog() != true) return;

            int newId = dataService.Warehouses.Count == 0

                ? 1

                : dataService.Warehouses.Max(w => w.Id) + 1;

            var warehouse = new Warehouse(newId, organization.Id, window.WarehouseName, window.WarehouseAddress);

            dataService.Warehouses.Add(warehouse);

            LoadWarehouses();

            WarehousesList.SelectedItem = warehouse;

        }

        private void EditWarehouse_Click(object sender, RoutedEventArgs e)

        {

            var warehouse = WarehousesList.SelectedItem as Warehouse;

            if (warehouse == null) { MessageBox.Show("Выберите склад."); return; }

            var window = new WarehouseWindow(warehouse.Name, warehouse.Address);

            window.Owner = this;

            if (window.ShowDialog() != true) return;

            warehouse.Name = window.WarehouseName;

            warehouse.Address = window.WarehouseAddress;

            WarehousesList.Items.Refresh();

            UpdateCurrentWarehouseText();

        }

        private void DeleteWarehouse_Click(object sender, RoutedEventArgs e)

        {

            var warehouse = WarehousesList.SelectedItem as Warehouse;

            if (warehouse == null) { MessageBox.Show("Выберите склад."); return; }

            var result = MessageBox.Show($"Удалить склад \"{warehouse.Name}\"?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;


            var products = dataService.Products.Where(p => p.WarehouseId == warehouse.Id).ToList();

            foreach (var p in products)

                dataService.Products.Remove(p);

            dataService.Warehouses.Remove(warehouse);

            LoadWarehouses();

        }
        private void AddProduct_Click(object sender, RoutedEventArgs e)

        {

            var warehouse = WarehousesList.SelectedItem as Warehouse;

            if (warehouse == null) { MessageBox.Show("Выберите склад."); return; }

            var window = new ProductWindow();

            window.Owner = this;

            if (window.ShowDialog() != true) return;

            int newId = dataService.Products.Count == 0 ? 1 : dataService.Products.Max(p => p.Id) + 1;

            var product = new Product

            {

                Id = newId,

                WarehouseId = warehouse.Id,

                Name = window.ProductName,

                Category = window.Category,

                Manufacturer = window.Manufacturer,

                Supplier = window.Supplier,

                Price = window.Price,

                StockQuantity = window.StockQuantity,

                DiscountPercent = window.DiscountPercent

            };

            dataService.Products.Add(product);

            LoadProducts();

        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)

        {

            var product = ProductsGrid.SelectedItem as Product;

            if (product == null) { MessageBox.Show("Выберите товар."); return; }

            var window = new ProductWindow(product.Name, product.Category, product.Manufacturer, product.Supplier, product.Price, product.StockQuantity, product.DiscountPercent);

            window.Owner = this;

            if (window.ShowDialog() != true) return;

            product.Name = window.ProductName;

            product.Category = window.Category;

            product.Manufacturer = window.Manufacturer;

            product.Supplier = window.Supplier;

            product.Price = window.Price;

            product.StockQuantity = window.StockQuantity;

            product.DiscountPercent = window.DiscountPercent;

            ProductsGrid.Items.Refresh();

        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)

        {

            var product = ProductsGrid.SelectedItem as Product;

            if (product == null) { MessageBox.Show("Выберите товар для удаления."); return; }

            var result = MessageBox.Show($"Удалить товар \"{product.Name}\"?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            dataService.Products.Remove(product);

            LoadProducts();

        }
        private void ImportProducts_Click(object sender, RoutedEventArgs e)

        {

            var warehouse = WarehousesList.SelectedItem as Warehouse;

            if (warehouse == null) { MessageBox.Show("Сначала выберите склад.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            var dlg = new OpenFileDialog { Filter = "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*" };

            if (dlg.ShowDialog() != true) return;

            try

            {

                var lines = System.IO.File.ReadAllLines(dlg.FileName);

                foreach (var line in lines.Skip(1)) 

                {

                    var fields = line.Split(',');

                    if (fields.Length < 7) continue;

                    var product = new Product

                    {

                        Id = dataService.Products.Count == 0 ? 1 : dataService.Products.Max(p => p.Id) + 1,

                        WarehouseId = warehouse.Id,

                        Name = fields[0].Trim(),

                        Category = fields[1].Trim(),

                        Manufacturer = fields[2].Trim(),

                        Supplier = fields[3].Trim(),

                        Price = decimal.Parse(fields[4], System.Globalization.CultureInfo.InvariantCulture),

                        StockQuantity = int.Parse(fields[5]),

                        DiscountPercent = decimal.Parse(fields[6], System.Globalization.CultureInfo.InvariantCulture)

                    };

                    dataService.Products.Add(product);

                }

                LoadProducts();

                UpdateCurrentWarehouseText();

                StatusText.Text = "Импорт товаров завершён!";

            }

            catch (Exception ex)

            {

                MessageBox.Show("Ошибка при импорте: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            }



        }

        private void OpenInvoiceWindow_Click(object sender, RoutedEventArgs e)
        {
            var warehouse = WarehousesList.SelectedItem as Warehouse;
            if (warehouse == null)
            {
                MessageBox.Show("Сначала выберите склад.", "Ошибка");
                return;
            }

            var window = new InvoiceWindow(dataService, warehouse) { Owner = this };
            if (window.ShowDialog() == true)
            {
                LoadProducts();
                UpdateCurrentWarehouseText();
                StatusText.Text = "Накладная применена, остатки обновлены.";
            }
        }


    }

}