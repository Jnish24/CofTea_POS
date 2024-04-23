using CofTeaPOSSystem.Resources.UserControls;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Printing;
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
    public partial class WindowAddToCart : Window
    {
        public int ReturnAmount { get; private set; }
        public int ReturnProductID { get; private set; }

        int seriesID;
        int standardID;
        double standardPrice;
        int productTypeID;
        string productName;

        public WindowAddToCart(int seriesID, string productName)
        {
            InitializeComponent();
            this.seriesID = seriesID;
            this.productName = productName;
            txt_amount.textbox.TextAlignment = TextAlignment.Center;
            setHeaderText();
        }

        private void setHeaderText()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(Globals.connectionString))
                {
                    connection.Open();

                    string query = @"SELECT seriesIndex || productName FROM tbl_ProductSeries
                                     INNER JOIN tbl_Product ON tbl_Product.seriesId = tbl_ProductSeries.id
                                     WHERE seriesId = @seriesId";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@seriesId", seriesID);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tb_Header.Text = $"Add {reader.GetString(0)}";
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

        private void Btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            bool isRadioChecked = false;

            if (productTypeID == 1)
            {
                foreach (var child in panel_cups.Children)
                {
                    if (child is RadioButton radiobutton)
                    {
                        if (radiobutton.IsChecked == true)
                        {
                            isRadioChecked = true;
                            ReturnProductID = (int)radiobutton.Tag;
                        }
                    }
                }
                if (!isRadioChecked)
                {
                    MessageBox.Show("Please select a cup size", "Submit", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (string.IsNullOrEmpty(txt_amount.C_txt_Text))
                {
                    MessageBox.Show("Amount cannot be empty", "Submit", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (!int.TryParse(txt_amount.C_txt_Text, out int result) || int.Parse(txt_amount.C_txt_Text) == 0)
                {
                    MessageBox.Show("Invalid amount", "Submit", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ReturnAmount = int.Parse(txt_amount.C_txt_Text);
                    this.Close();
                }
            }
            else if (productTypeID == 2)
            {
                if (string.IsNullOrEmpty(txt_amount.C_txt_Text))
                {
                    MessageBox.Show("Amount cannot be empty", "Submit", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (!int.TryParse(txt_amount.C_txt_Text, out int result) || int.Parse(txt_amount.C_txt_Text) == 0)
                {
                    MessageBox.Show("Invalid amount", "Submit", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ReturnAmount = int.Parse(txt_amount.C_txt_Text);
                    ReturnProductID = standardID;
                    this.Close();
                }
            }
        }

        private void panel_cups_Loaded(object sender, RoutedEventArgs e)
        {
            panel_cups.Children.Clear();
            
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(Globals.connectionString))
                {
                    connection.Open();

                    string query = @"SELECT typeId FROM tbl_ProductSeries WHERE id = @id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", seriesID);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                productTypeID = reader.GetInt32(0);
                            }
                        }
                    }

                    if (productTypeID == 1)
                    {
                        query = @"SELECT tbl_Product.id, cupName, productPrice FROM tbl_Cups
                              INNER JOIN tbl_Product ON cupId = tbl_Cups.id
                              WHERE seriesId = @seriesId AND productName = @productName AND productPrice <> 0";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@seriesId", seriesID);
                            command.Parameters.AddWithValue("@productName", productName);

                            using (SQLiteDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    RadioButton cupSize = new RadioButton();
                                    cupSize.Tag = reader.GetInt32(0);
                                    cupSize.GroupName = "grp_cups";
                                    cupSize.Content = $"(₱{reader.GetDouble(2)}) {reader.GetString(1)}";
                                    cupSize.Style = (Style)FindResource("radio_payment");

                                    panel_cups.Children.Add(cupSize);
                                }
                            }
                        }
                    }
                    else if (productTypeID == 2)
                    {
                        query = @"SELECT id, productPrice FROM tbl_Product
                                  WHERE seriesId = @seriesId AND productName = @productName";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@seriesId", seriesID);
                            command.Parameters.AddWithValue("@productName", productName);

                            using (SQLiteDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    standardID = reader.GetInt32(0);
                                    standardPrice = reader.GetInt32(1);
                                    tb_price.Text = $"₱ { reader.GetDouble(1)}";
                                    tb_price.FontFamily = new FontFamily("Arial");
                                    //tb_description.Margin = new Thickness(0, 15, 0, 30);
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
        }

        private void txt_amount_IsPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
