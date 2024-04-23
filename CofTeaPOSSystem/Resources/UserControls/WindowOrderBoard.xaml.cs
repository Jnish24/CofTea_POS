using CofTeaPOSSystem.Windows;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.DirectoryServices;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace CofTeaPOSSystem.Resources.UserControls
{
    public partial class WindowOrderBoard : UserControl
    {
        CustomButton activeButton = new CustomButton();
        Window ParentWindow;
        int userID;

        public WindowOrderBoard(Window ParentWindow, int userID)
        {
            InitializeComponent();
            this.ParentWindow = ParentWindow;
            this.userID = userID;

            txt_discount.Text = "0.00";
        }

        private void UpdateSubtotal()
        {
            double subtotal = 0;

            foreach (var child in panel_cart.Children)
            {
                if (child is StackPanel stackPanel)
                {
                    TextBlock textBlock = (TextBlock)stackPanel.Children[5];
                    double orderPrice = double.Parse(textBlock.Text.Substring(2));

                    subtotal += orderPrice;
                }
            }

            tb_subtotal.Text = $"₱ {Math.Round(subtotal, 2, MidpointRounding.AwayFromZero)}";

            UpdateTotal(subtotal);
        }

        private void UpdateTotal(double subtotal = 0)
        {
            double discountValue = double.Parse(txt_discount.Text);
            double formula = subtotal - (subtotal * (discountValue / 100));

            tb_total.Text = $"₱ {Math.Round(formula, 2, MidpointRounding.AwayFromZero)}";
        }

        private void panel_Series_Loaded(object sender, RoutedEventArgs e)
        {
            panel_Series.Children.Clear();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(Globals.connectionString))
                {
                    connection.Open();
                    string query = "SELECT id, seriesName, seriesIndex FROM tbl_ProductSeries";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CustomButton seriesButton = new CustomButton();
                                seriesButton.Tag = reader.GetInt32(0);
                                seriesButton.Height = 125;
                                seriesButton.Width = 125;
                                seriesButton.C_btn_Content = reader.GetString(1);
                                seriesButton.C_btn_Background = "#D1A782";
                                seriesButton.C_btn_Foreground = "#5F432C";
                                seriesButton.C_btn_HoverColor = "#FFEDDB";
                                seriesButton.Margin = new Thickness(20);

                                seriesButton.IsClicked += SeriesButton_IsClicked;

                                panel_Series.Children.Add(seriesButton);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SeriesButton_IsClicked(object sender, RoutedEventArgs e)
        {
            CustomButton customButton = sender as CustomButton;
            int seriesID = (int)customButton.Tag;
            string seriesName = (string)customButton.C_btn_Content;

            panel_Items.Children.Clear();
            tb_products.Text = seriesName;

            activeButton.C_btn_Background = "#D1A782";
            activeButton = customButton;
            customButton.C_btn_Background = "#FFEDDB";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(Globals.connectionString))
                {
                    connection.Open();
                    string query = @"SELECT DISTINCT seriesId, productName FROM tbl_Product 
                                     INNER JOIN tbl_ProductSeries ON tbl_Product.seriesId = tbl_ProductSeries.id 
                                     WHERE seriesId = @seriesId";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@seriesId", seriesID);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CustomButton productButton = new CustomButton();
                                productButton.Tag = reader.GetInt32(0);
                                productButton.Height = 125;
                                productButton.Width = 125;
                                productButton.C_btn_Content = reader.GetString(1);
                                productButton.C_btn_Background = "#D1A782";
                                productButton.C_btn_Foreground = "#5F432C";
                                productButton.C_btn_HoverColor = "#FFEDDB";
                                productButton.Margin = new Thickness(20);

                                productButton.IsClicked += ProductButton_IsClicked; ;

                                panel_Items.Children.Add(productButton);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ProductButton_IsClicked(object sender, RoutedEventArgs e)
        {
            CustomButton customButton = (CustomButton)sender;
            int seriesID = (int)customButton.Tag;
            string productName = (string)customButton.C_btn_Content;

            string productDescription = "";

            WindowAddToCart obj = new WindowAddToCart(seriesID, productName);

            ParentWindow.Opacity = 0.4;
            obj.ShowDialog();
            ParentWindow.Opacity = 1;

            int amount = obj.ReturnAmount;
            int productID = obj.ReturnProductID;

            foreach (var child in panel_cart.Children)
            {
                if (child is StackPanel stackPanel && (int)stackPanel.Tag == productID)
                {
                    TextBlock tb_amount = (TextBlock)stackPanel.Children[3];
                    TextBlock tb_price = (TextBlock)stackPanel.Children[5];

                    tb_amount.Text = amount.ToString();
                    tb_price.Text = $"₱ {Math.Round(((double)tb_price.Tag * amount), 2, MidpointRounding.AwayFromZero)}";

                    UpdateSubtotal();
                    return;
                }
            }

            if (obj.ReturnProductID != 0)
            {
                bool isBeverage = false;
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(Globals.connectionString))
                    {
                        connection.Open();
                        string query = @"SELECT seriesIndex || productName || "" "" || cupName AS ""Product Description""
                                         FROM tbl_Product 
                                         INNER JOIN tbl_ProductSeries ON tbl_ProductSeries.id = seriesId
                                         INNER JOIN tbl_Cups ON tbl_Cups.id = cupId
                                         WHERE tbl_Product.id = @id";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", obj.ReturnProductID);

                            using (SQLiteDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    productDescription = reader.GetString(0);
                                    isBeverage = true;
                                }
                            }
                        }

                        if (!isBeverage)
                        {
                            query = @"SELECT seriesIndex || productName || "" "" AS ""Product Description""
                                              FROM tbl_Product 
                                              INNER JOIN tbl_ProductSeries ON tbl_ProductSeries.id = seriesId
                                              WHERE tbl_Product.id = @id";

                            using (SQLiteCommand command = new SQLiteCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@id", obj.ReturnProductID);

                                using (SQLiteDataReader reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        productDescription = reader.GetString(0);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                StackPanel stackPanel = new StackPanel();
                stackPanel.Tag = productID;
                stackPanel.Orientation = Orientation.Horizontal;

                CustomButton trashButton = new CustomButton();
                trashButton.C_btn_Content = "x";
                trashButton.C_btn_Background = "#F4382D";
                trashButton.C_btn_HoverColor = "#C82E25";
                trashButton.C_btn_Foreground = "#FFEDDB";
                trashButton.Style = (Style)FindResource("cart_button");
                trashButton.IsClicked += TrashButton_IsClicked;

                TextBlock textBlock_product = new TextBlock();
                textBlock_product.Text = productDescription;
                textBlock_product.Style = (Style)FindResource("cart_itemName");

                CustomButton minusButton = new CustomButton();
                minusButton.C_btn_Content = "−";
                minusButton.C_btn_Background = "#9C7C60";
                minusButton.C_btn_Foreground = "#FFEDDB";
                minusButton.Style = (Style)FindResource("cart_button");
                minusButton.IsClicked += MinusButton_IsClicked; ;

                TextBlock textBlock_amount = new TextBlock();
                textBlock_amount.Text = amount.ToString();
                textBlock_amount.Style = (Style)FindResource("cart_itemName");

                CustomButton plusButton = new CustomButton();
                plusButton.C_btn_Content = "+";
                plusButton.C_btn_Background = "#9C7C60";
                plusButton.C_btn_Foreground = "#FFEDDB";
                plusButton.Style = (Style)FindResource("cart_button");
                plusButton.IsClicked += PlusButton_IsClicked;

                TextBlock textBlock_price = new TextBlock();
                textBlock_price.Tag = Globals.OrderBoard_Cart_InitialPrice(productID);
                textBlock_price.Text = $"₱ {Math.Round(((double)textBlock_price.Tag * amount), 2, MidpointRounding.AwayFromZero)}"; 
                textBlock_price.Style = (Style)FindResource("cart_itemName");

                stackPanel.Children.Add(trashButton);
                stackPanel.Children.Add(textBlock_product);
                stackPanel.Children.Add(minusButton);
                stackPanel.Children.Add(textBlock_amount);
                stackPanel.Children.Add(plusButton);
                stackPanel.Children.Add(textBlock_price);
                

                panel_cart.Children.Add(stackPanel);
                UpdateSubtotal();
            }
        }

        private void TrashButton_IsClicked(object sender, RoutedEventArgs e)
        {
            CustomButton trashButton = (CustomButton)sender;
            StackPanel stackPanel = (StackPanel)trashButton.Parent;

            if (stackPanel != null) 
            {
                (stackPanel.Parent as Panel)?.Children.Remove(stackPanel);
            }

            UpdateSubtotal();
        }

        private void MinusButton_IsClicked(object sender, RoutedEventArgs e)
        {
            CustomButton plusButton = (CustomButton)sender;
            StackPanel stackPanel = (StackPanel)plusButton.Parent;
            TextBlock tb_amount = (TextBlock)stackPanel.Children[3];
            TextBlock tb_price = (TextBlock)stackPanel.Children[5];

            tb_amount.Text = (int.Parse(tb_amount.Text) - 1).ToString();
            tb_price.Text = $"₱ {Math.Round(((double)tb_price.Tag * int.Parse(tb_amount.Text)), 2, MidpointRounding.AwayFromZero)}";

            if (int.Parse(tb_amount.Text) == 0)
            {
                (stackPanel.Parent as Panel)?.Children.Remove(stackPanel);
            }

            UpdateSubtotal();
        }

        private void PlusButton_IsClicked(object sender, RoutedEventArgs e)
        {
            CustomButton plusButton = (CustomButton)sender;
            StackPanel stackPanel = (StackPanel)plusButton.Parent;
            TextBlock textBlock = (TextBlock)stackPanel.Children[3];
            TextBlock tb_amount = (TextBlock)stackPanel.Children[3];
            TextBlock tb_price = (TextBlock)stackPanel.Children[5];

            tb_amount.Text = (int.Parse(tb_amount.Text) + 1).ToString();
            tb_price.Text = $"₱ {Math.Round(((double)tb_price.Tag * int.Parse(tb_amount.Text)), 2, MidpointRounding.AwayFromZero)}";

            UpdateSubtotal();
        }

        private void Btn_CheckOut_Clicked(object sender, RoutedEventArgs e)
        {
            int paymentID;
            int receiptID = -1;
            double discountValue = double.Parse(txt_discount.Text);
            double subtotal = int.Parse(tb_subtotal.Text.Substring(2));
            double total = double.Parse(tb_total.Text.Substring(2));
            string timestampNow = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            if (radio_Cash.IsChecked == true)
            {
                paymentID = 1;
            }
            else if (radio_GCash.IsChecked == true)
            {
                paymentID = 2;
            }
            else 
            { 
                paymentID = -1; 
            }

            if (panel_cart.Children.Count != 0)
            {
                if (paymentID != -1)
                {
                    MessageBoxResult result = MessageBox.Show($"Are you sure you want to checkout?", "Checkout", MessageBoxButton.YesNo, MessageBoxImage.Information);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            using (SQLiteConnection connection = new SQLiteConnection(Globals.connectionString))
                            {
                                connection.Open();
                                string query = "INSERT INTO tbl_SalesReceipt(discount, subtotal, total, paymentId, receiptDate, userId) VALUES(@discount, @subtotal, @total, @paymentId, @receiptDate, @userId)";

                                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                                {
                                    command.Parameters.AddWithValue("@discount", discountValue);
                                    command.Parameters.AddWithValue("@subtotal", subtotal);
                                    command.Parameters.AddWithValue("@total", total);
                                    command.Parameters.AddWithValue("@paymentId", paymentID);
                                    command.Parameters.AddWithValue("@receiptDate", timestampNow);
                                    command.Parameters.AddWithValue("@userId", userID);
                                    command.ExecuteNonQuery();
                                }

                                query = "SELECT id FROM tbl_SalesReceipt ORDER BY receiptDate DESC LIMIT 1";

                                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                                {
                                    using (SQLiteDataReader reader = command.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                            receiptID = reader.GetInt32(0);
                                        }
                                    }
                                }

                                foreach (var child in panel_cart.Children)
                                {
                                    int productID, productAmount;

                                    if (child is StackPanel stackPanel)
                                    {
                                        TextBlock tb_amount = (TextBlock)stackPanel.Children[3];

                                        productID = (int)stackPanel.Tag;
                                        productAmount = int.Parse(tb_amount.Text);

                                        query = "INSERT INTO tbl_Sales(receiptId, productId, amount) VALUES (@receiptId, @productId, @amount)";

                                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                                        {
                                            command.Parameters.AddWithValue("@receiptId", receiptID);
                                            command.Parameters.AddWithValue("@productId", productID);
                                            command.Parameters.AddWithValue("@amount", productAmount);
                                            command.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                            MessageBox.Show("Checkout successful", "Checkout", MessageBoxButton.OK, MessageBoxImage.Information);
                            panel_cart.Children.Clear();
                            tb_subtotal.Text = "₱ 0";
                            tb_total.Text = "₱ 0";
                            txt_discount.Text = "0.00";
                            radio_Cash.IsChecked = false;
                            radio_GCash.IsChecked = false;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Select payment method", "Checkout", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No items listed", "Checkout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txt_discount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt_discount.Text))
            {
                txt_discount.Text = "0.00";
            }
            else if (double.TryParse(txt_discount.Text, out double result))
            {
                if (result > 100)
                {
                    txt_discount.Text = "100";
                }
            }
            else
            {
                txt_discount.Text = "0.00";
            }
            UpdateSubtotal();
        }

        private void txt_discount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
