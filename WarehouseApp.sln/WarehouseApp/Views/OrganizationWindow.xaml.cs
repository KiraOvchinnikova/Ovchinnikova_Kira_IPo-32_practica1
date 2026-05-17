using System.Windows;

namespace WarehouseApp.Views
{
    public partial class OrganizationWindow : Window
    {
        public string OrganizationName { get; private set; } = "";

        public OrganizationWindow()
        {
            InitializeComponent();
        }

        public OrganizationWindow(string oldName)
        {
            InitializeComponent();

            NameBox.Text = oldName;
            OrganizationName = oldName;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Введите наименование организации.");
                return;
            }

            OrganizationName = NameBox.Text.Trim();
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}


