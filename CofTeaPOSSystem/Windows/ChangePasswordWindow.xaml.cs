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
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace CofTeaPOSSystem.Windows
{
    /// <summary>
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        string connectionString = Globals.connectionString;
        PasswordBox newPass, confirmPass;
        string userEmail;

        public ChangePasswordWindow(string email)
        {
            InitializeComponent();
            userEmail = email;
        }

        // METHODS USED BY EVENTS
        private void UpdateUserPassword(string newUserPassword)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE tbl_User SET password = @password WHERE email = @email";
                    
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@email", userEmail);
                        command.Parameters.AddWithValue("@password", newUserPassword);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Your password has been updated", "Password Status", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // EVENTS
        private void Btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            string newPassword, confirmPassword;

            newPass = (PasswordBox)txt_password.FindName("passwordbox");
            confirmPass = (PasswordBox)txt_confirmPassword.FindName("passwordbox");

            newPassword = newPass.Password.Trim();
            confirmPassword = confirmPass.Password.Trim();

            if (newPassword.Length >= 8 && confirmPassword.Length >= 8)
            {
                if (newPassword == confirmPassword)
                {
                    UpdateUserPassword(newPassword);
                }
                else
                {
                    MessageBox.Show("Password do not match", "Password Status", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Password must consist at least 8 characters", "Password Status", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // MAKE WINDOW MOVEABLE
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
