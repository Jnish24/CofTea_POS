using CofTeaPOSSystem.Windows;
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

namespace CofTeaPOSSystem
{
    public partial class LoginWindow : Window
    {
        private string connectionString = Globals.connectionString;
        PasswordBox passwordbox_password;

        public LoginWindow()
        {
            InitializeComponent();
            txt_username.textbox.Focus();
        }

        // EVENTS
        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            string username, password;

            passwordbox_password = (PasswordBox)txt_password.FindName("passwordbox");

            username = txt_username.C_txt_Text?.Trim();
            password = passwordbox_password.Password.Trim();

            try // Should there be an error when accessing the database.
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT password FROM tbl_User WHERE userName = @userName";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userName", username);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string realPassword = reader.GetString(0);

                                if (password == realPassword)
                                {
                                    MainWindow obj = new MainWindow(Globals.GetLoginID(username));
                                    obj.Show();
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Incorrect Username or Password", "Incorrect Login Credentials", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("User does not exist", "User Status", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void Btn_ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            ForgotPasswordWindow obj = new ForgotPasswordWindow();
            this.Opacity = 0.5;
            obj.ShowDialog();
            this.Opacity = 1;
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        
        private void Btn_Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // TO MAKE WINDOW DRAGGABLE
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

    }
}
