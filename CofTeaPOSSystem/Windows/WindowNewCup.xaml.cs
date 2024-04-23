using CofTeaPOSSystem.Resources.UserControls;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CofTeaPOSSystem.Windows
{
    public partial class WindowNewCup : Window
    {
        public WindowNewCup()
        {
            InitializeComponent();
        }

        private void Btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            string cupName = txt_cupName.C_txt_Text?.Trim();

            if (string.IsNullOrEmpty(cupName))
            {
                MessageBox.Show("Cup name cannot be empty", "Submit", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else if (Globals.AddProduct_AddCup_NameExists(cupName))
            {
                MessageBox.Show("Cup name already exists", "Submit", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"Adding a new cup will set a new variant for each beverage item with this cup. These new variant prices will be set to 0 and will not be available for order unless you change its price greater than 0", "Submit", MessageBoxButton.YesNo, MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    Globals.AddProduct_AddCup(cupName);
                    this.Close();
                }
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
