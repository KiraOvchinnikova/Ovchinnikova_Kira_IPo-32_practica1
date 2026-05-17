using System.Windows;

namespace WarehouseApp.Views
{
    public partial class ProductWindow : Window
    {
        public string ProductName => NameBox.Text.Trim();

        public string Category => CategoryBox.Text.Trim();

        public string Manufacturer => ManufacturerBox.Text.Trim();

        public string Supplier => SupplierBox.Text.Trim();

        public decimal Price { get; private set; }

        public int StockQuantity { get; private set; }

        public decimal DiscountPercent { get; private set; }

        public ProductWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Введите название товара.");
                return;
            }

            if (!decimal.TryParse(PriceBox.Text, out decimal price))
            {
                MessageBox.Show("Цена должна быть числом.");
                return;
            }

            if (!int.TryParse(StockBox.Text, out int stock))
            {
                MessageBox.Show("Остаток должен быть числом.");
                return;
            }

            if (!decimal.TryParse(DiscountBox.Text, out decimal discount))
            {
                MessageBox.Show("Скидка должна быть числом.");
                return;
            }

            Price = price;
            StockQuantity = stock;
            DiscountPercent = discount;

            DialogResult = true;
        }

        public ProductWindow(
            string name,
            string category,
            string manufacturer,
            string supplier,
            decimal price,
            int stock,
            decimal discount)
        {
            InitializeComponent();

            NameBox.Text = name;
            CategoryBox.Text = category;
            ManufacturerBox.Text = manufacturer;
            SupplierBox.Text = supplier;
            PriceBox.Text = price.ToString();
            StockBox.Text = stock.ToString();
            DiscountBox.Text = discount.ToString();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
