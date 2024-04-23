using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
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
    public partial class WindowEditSeries : Window
    {
        int seriesID;
        string seriesName;

        public WindowEditSeries(int seriesID, string seriesName, string seriesIndex)
        {
            InitializeComponent();
            this.seriesID = seriesID;
            this.seriesName = seriesName;

            tb_Header.Text = seriesName;

            txt_name.C_txt_Text = seriesName;
            txt_index.C_txt_Text = seriesIndex.Replace("_", "");
        }

        private void Btn_DeleteSeries_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($@"Are you sure you want to delete product series ""{seriesName}"" and its product items? This cannot be undone", "Delete Series", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(Globals.connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM tbl_ProductSeries WHERE id = @id";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", seriesID);
                            command.ExecuteNonQuery();

                            MessageBox.Show("Product Series Deleted Successfully", "Delete Series", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void Btn_Update_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_name.C_txt_Text) && !string.IsNullOrEmpty(txt_index.C_txt_Text))
            {
                string input_seriesName = txt_name.C_txt_Text.Trim();
                string input_seriesIndex = txt_index.C_txt_Text.Replace(" ", "") + "_";
                input_seriesIndex = input_seriesIndex.ToUpper();

                if (Globals.AddProduct_UpdateSeries_NameExists(seriesID, input_seriesName))
                {
                    MessageBox.Show("Series name already exists", "Update Series", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (Globals.AddProduct_UpdateSeries_IndexExists(seriesID,input_seriesIndex))
                {
                    MessageBox.Show("Series index already exists", "Update Series", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Globals.AddProduct_UpdateSeries(seriesID,input_seriesName, input_seriesIndex);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Please fill out all required fields", "Update Series", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txt_index_IsPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z0-9]");
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
