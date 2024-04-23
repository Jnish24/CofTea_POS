using System;
using System.Collections.Generic;
using System.Data.SQLite;
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


    public partial class WindowEditItem : Window
    {
        string productName;
        int seriesID;

        public WindowEditItem(string seriesName, string productName)
        {
            InitializeComponent();
            this.productName = productName;

            tb_Header.Text = productName;
            txt_name.C_txt_Text = productName;

            seriesID = Globals.AddProduct_UpdateItem_GetSeriesID(seriesName);
        }

        private void Btn_Update_Click(object sender, RoutedEventArgs e)
        {
            string input_productName = txt_name.C_txt_Text?.Trim();

            if (!string.IsNullOrEmpty(txt_name.C_txt_Text))
            {
                if (Globals.AddProduct_UpdateItem_NameExists(seriesID, input_productName))
                {
                    MessageBox.Show("Product name already exists", "Update Series", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Globals.AddProduct_UpdateItem(seriesID, input_productName, productName);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Please fill out all required fields", "Update Series", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn_DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($@"Are you sure you want to delete product item ""{productName}""? This cannot be undone", "Delete Item", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(Globals.connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM tbl_Product WHERE seriesId = @seriesId AND productName = @productName";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@seriesId", seriesID);
                            command.Parameters.AddWithValue("@productName", productName);
                            command.ExecuteNonQuery();

                            MessageBox.Show("Product Item Deleted Successfully", "Delete Item", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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
