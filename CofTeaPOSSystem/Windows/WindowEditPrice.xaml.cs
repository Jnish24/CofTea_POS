using CofTeaPOSSystem.Resources.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class WindowEditPrice : Window
    {
        int productID;
        double productPrice;
        string productCup;

        public WindowEditPrice(int productID, double productPrice, string productCup)
        {
            InitializeComponent();
            this.productID = productID;
            this.productPrice = productPrice;
            this.productCup = productCup;

            tb_Header.Text = $"{Globals.AddProduct_UpdatePrice_GetProductFullName(productID)} {productCup}";
        }

        private void Btn_Update_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_price.C_txt_Text))
            {
                if (double.TryParse(txt_price.C_txt_Text, out double result))
                {
                    result = Math.Round(result, 2, MidpointRounding.AwayFromZero);
                    Globals.AddProduct_UpdatePrice(productID, result);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid Price", "Set Price", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please fill out all required fields", "Set Price", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txt_name_IsPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]");
            e.Handled = regex.IsMatch(e.Text);
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
