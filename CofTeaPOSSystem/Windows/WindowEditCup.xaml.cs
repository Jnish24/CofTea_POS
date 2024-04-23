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
    public partial class WindowEditCup : Window
    {
        int cupID;
        string cupName;

        public WindowEditCup(int cupID, string cupName)
        {
            InitializeComponent();
            this.cupID = cupID;
            this.cupName = cupName;

            tb_Header.Text = cupName;
            txt_name.C_txt_Text = cupName;
        }

        private void Btn_Update_Click(object sender, RoutedEventArgs e)
        {
            string input_cupName = txt_name.C_txt_Text?.Trim();

            if (!string.IsNullOrEmpty(txt_name.C_txt_Text))
            {
                if (Globals.AddProduct_AddCup_NameExists(input_cupName))
                {
                    MessageBox.Show("Cup name already exists", "Update Cup Name", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Globals.AddProduct_UpdateCup(cupID, input_cupName);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Please fill out all required fields", "Update Series", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn_DeleteCup_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($@"Are you sure you want to delete cup size ""{cupName}""? This will also delete the pricing for items with this cup", "Delete Cup Size", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(Globals.connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM tbl_Cups WHERE id = @id";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", cupID);
                            command.ExecuteNonQuery();

                            MessageBox.Show("Cup Size Deleted Successfully", "Delete Cup Size", MessageBoxButton.OK, MessageBoxImage.Information);
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
