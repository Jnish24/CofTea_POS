using CofTeaPOSSystem.Resources.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace CofTeaPOSSystem.Windows
{

    public partial class WindowNewItem : Window
    {
        int productType = -1;

        public WindowNewItem()
        {
            InitializeComponent();
            txt_productName.textbox.Focus();
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> seriesList = new List<string>();
            seriesList = Globals.AddProduct_AddItem_GetAllSeries();
            foreach (string series in seriesList)
            {
                combo_Series.Items.Add(series);
            }
        }

        private void Btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            string productName = txt_productName.C_txt_Text?.Trim();
            string productSeries = combo_Series.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(productName) || productType == -1)
            {
                MessageBox.Show("Please fill out all fields", "Submit", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (Globals.AddProduct_AddItem_NameExists(combo_Series.SelectedItem.ToString(), productName))
            {
                MessageBox.Show("Product item name already exists", "Submit", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (var child in stack_priceList.Children)
            {
                if (child is CustomTextBox customTextBox)
                {
                    if (string.IsNullOrEmpty(customTextBox.C_txt_Text))
                    {
                        MessageBox.Show("Price cannot be empty", "Submit", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else if (!double.TryParse(customTextBox.C_txt_Text, out double result))
                    {
                        MessageBox.Show("Invalid Price", "Submit", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }

            if (productType == 1)
            {
                foreach (var child in stack_priceList.Children)
                {
                    if (child is CustomTextBox customTextBox)
                    {
                        double price = Math.Round(double.Parse(customTextBox.C_txt_Text), 2, MidpointRounding.AwayFromZero);

                        Globals.AddProduct_AddItemBeverage(productSeries, customTextBox.Tag.ToString(), productName, price);
                    }
                }
                MessageBox.Show("Product Item Added Successfully", "Submit", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else if (productType == 2)
            {
                foreach (var child in stack_priceList.Children)
                {
                    if (child is CustomTextBox customTextBox)
                    {
                        double price = Math.Round(double.Parse(customTextBox.C_txt_Text), 2, MidpointRounding.AwayFromZero);

                        Globals.AddProduct_AddItemStandard(productSeries, productName, price);
                    }
                }
                this.Close();
            }
        }

        private void combo_Series_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Globals.AddProduct_AddItem_GetProductType(combo_Series.SelectedItem.ToString()) == 1)
            {
                productType = 1;

                stack_priceList.Children.Clear();

                List<string> list = new List<string>(Globals.AddProduct_AddItem_GetAllCupSize());

                foreach (string item in list)
                {
                    TextBlock textblock = new TextBlock();
                    textblock.Name = "textblock_" + item;
                    textblock.Text = item;
                    textblock.Style = (Style)FindResource("txt_priceHeader");

                    stack_priceList.Children.Add(textblock);

                    CustomTextBox customTextBox = new CustomTextBox();
                    customTextBox.Tag = item;
                    customTextBox.Name = "textbox_" + item;
                    customTextBox.C_txt_FontSize = 14;
                    customTextBox.C_tb_Text = "Enter Price";
                    customTextBox.C_txt_BorderBrush = "#9C7C60";
                    customTextBox.Style = (Style)FindResource("txtbox_pricing");
                    customTextBox.IsPreviewTextInput += Textbox_Price_IsPreviewTextInput;

                    stack_priceList.Children.Add(customTextBox);
                }
            }
            else if (Globals.AddProduct_AddItem_GetProductType(combo_Series.SelectedItem.ToString()) == 2)
            {
                productType = 2;

                stack_priceList.Children.Clear();

                TextBlock textblock = new TextBlock();
                textblock.Name = "textblock_price";
                textblock.Text = "Product Price";
                textblock.Style = (Style)FindResource("txt_priceHeader");

                stack_priceList.Children.Add(textblock);

                CustomTextBox customTextBox = new CustomTextBox();
                customTextBox.Name = "textbox_price";
                customTextBox.C_txt_FontSize = 14;
                customTextBox.C_tb_Text = "Enter Price";
                customTextBox.C_txt_FontSize = 14;
                customTextBox.C_txt_BorderBrush = "#9C7C60";
                customTextBox.Style = (Style)FindResource("txtbox_pricing");
                customTextBox.IsPreviewTextInput += Textbox_Price_IsPreviewTextInput;

                stack_priceList.Children.Add(customTextBox);
            }
            else
            {

            }
        }

        private void Textbox_Price_IsPreviewTextInput(object sender, TextCompositionEventArgs e)
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

    }
}
