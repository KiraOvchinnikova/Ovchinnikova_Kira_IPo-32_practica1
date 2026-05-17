using System.Windows;

namespace WarehouseApp.Views
{
    public partial class WarehouseWindow : Window
    {
        public string WarehouseName { get; private set; } = "";

        public string WarehouseAddress { get; private set; } = "";

        public WarehouseWindow()
        {
            InitializeComponent();
        }

        public WarehouseWindow(string name, string address)
        {
            InitializeComponent();

            NameBox.Text = name;
            AddressBox.Text = address;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Введите название склада.");
                return;
            }

            if (string.IsNullOrWhiteSpace(AddressBox.Text))
            {
                MessageBox.Show("Введите адрес склада.");
                return;
            }

            WarehouseName = NameBox.Text.Trim();
            WarehouseAddress = AddressBox.Text.Trim();

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}


