using System.Linq;
using System.Windows;
using WarehouseData.Models;
using WarehouseData.Services;
using WarehouseApp.Views;

namespace WarehouseApp
{
    public partial class MainWindow : Window
    {
        private readonly DataService dataService;

        public MainWindow()
        {
            InitializeComponent();

            dataService = new DataService();
            OrganizationsList.ItemsSource = dataService.Organizations;

            if (dataService.Organizations.Count > 0)
                OrganizationsList.SelectedIndex = 0;
        }

        private void AddOrganization_Click(object sender, RoutedEventArgs e)
        {
            var window = new OrganizationWindow();
            window.Owner = this;

            if (window.ShowDialog() != true)
                return;

            int newId = dataService.Organizations.Count == 0
                ? 1
                : dataService.Organizations.Max(o => o.Id) + 1;

            var org = new Organization(newId, window.OrganizationName);
            dataService.Organizations.Add(org);

            OrganizationsList.Items.Refresh();
            OrganizationsList.SelectedItem = org;
        }

        private void EditOrganization_Click(object sender, RoutedEventArgs e)
        {
            var org = OrganizationsList.SelectedItem as Organization;
            if (org == null)
            {
                MessageBox.Show("Выберите организацию для редактирования.");
                return;
            }

            var window = new OrganizationWindow(org.Name);
            window.Owner = this;

            if (window.ShowDialog() != true)
                return;

            org.Name = window.OrganizationName;
            OrganizationsList.Items.Refresh();
        }

        private void DeleteOrganization_Click(object sender, RoutedEventArgs e)
        {
            var org = OrganizationsList.SelectedItem as Organization;
            if (org == null)
            {
                MessageBox.Show("Выберите организацию для удаления.");
                return;
            }

            var result = MessageBox.Show(
                $"Удалить организацию \"{org.Name}\"?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            var warehouses = dataService.Warehouses
                .Where(w => w.OrganizationId == org.Id)
                .ToList();

            foreach (var w in warehouses)
            {
                var products = dataService.Products
                    .Where(p => p.WarehouseId == w.Id)
                    .ToList();

                foreach (var p in products)
                    dataService.Products.Remove(p);

                dataService.Warehouses.Remove(w);
            }

            dataService.Organizations.Remove(org);
            OrganizationsList.Items.Refresh();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            var org = OrganizationsList.SelectedItem as Organization;
            if (org == null)
            {
                MessageBox.Show("Выберите организацию.");
                return;
            }

            var storageWindow = new StorageWindow(org);
            storageWindow.Owner = this;
            storageWindow.ShowDialog();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}


