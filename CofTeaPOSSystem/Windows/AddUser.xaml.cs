using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public partial class AddUser : Window
    {
        string connectionString = Globals.connectionString;

        string firstName, lastName, username, contact;
        bool isNewUser;
        int userId, userIDD;
        PasswordBox Password;

        public AddUser(bool isNewUser = true,
                       string role = null, 
                       string fname = null, 
                       string lname = null, 
                       string username = null,
                       string password = null, 
                       string email = null, 
                       string contact = null)
        {
            InitializeComponent();
            Password = (PasswordBox)txt_password.FindName("passwordbox");
            this.isNewUser = isNewUser;

            if (!isNewUser)
            {
                firstName = fname;
                lastName = lname;
                this.username = username;
                this.contact = contact;

                tb_text.Text = $"You are editing the information of \"{firstName} {lastName}\"";
                txt_fname.C_txt_Text = firstName;
                txt_lname.C_txt_Text = lastName;
                txt_username.C_txt_Text = username;
                Password.Password = password;
                txt_email.C_txt_Text = email;
                txt_contact.C_txt_Text = contact;

                if (role == "Admin")
                {
                    rb_admin.IsChecked = true;
                }
                else if (role == "Staff")
                {
                    rb_staff.IsChecked = true;
                }
            }
            else
            {
                tb_text.Text = "Add User";
                btn_deleteUser.IsEnabled = false;
                btn_deleteUser.Visibility = Visibility.Hidden;
            }
        }

        private void btn_saveUser_IsClicked(object sender, RoutedEventArgs e)
        {
            string inputFirstName, inputLastName, inputUsername, inputPassword, inputEmail, inputContact;
            int? inputRole;

            inputFirstName = txt_fname.C_txt_Text?.Trim();
            inputLastName = txt_lname.C_txt_Text?.Trim();
            inputUsername = txt_username.C_txt_Text?.Trim();
            inputPassword = Password.Password?.Trim();
            inputEmail = txt_email.C_txt_Text?.Trim();
            inputContact = txt_contact.C_txt_Text?.Trim();

            if (rb_admin.IsChecked == true)
            {
                inputRole = 1;
            }
            else if (rb_staff.IsChecked == true)
            {
                inputRole = 2;
            }
            else
            {
                inputRole = null;
            }


            if (string.IsNullOrWhiteSpace(inputFirstName) ||
                string.IsNullOrWhiteSpace(inputLastName) ||
                string.IsNullOrWhiteSpace(inputUsername) ||
                string.IsNullOrWhiteSpace(inputPassword) ||
                string.IsNullOrWhiteSpace(inputContact) ||
                inputRole == null)
            {
                MessageBox.Show("Please fill out all required fields", "Save", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (Globals.AddUser_IsExistingUsername(inputUsername, username))
            {
                MessageBox.Show("Username already exists", "Save", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (inputPassword.Length < 8)
            {
                MessageBox.Show("Password must consist at least 8 characters", "Save", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (Globals.AddUser_IsExistingEmail(inputEmail, username))
            {
                MessageBox.Show("Email already exists", "Save", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (Globals.AddUser_IsExistingContact(inputContact, username))
                {
                    MessageBox.Show("Contact number already exists", "Save", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Globals.AddUser_ModifyUser(isNewUser, inputFirstName, inputLastName, inputUsername, inputPassword, inputEmail, inputContact, inputRole, username);
                    this.Close();
                }
            }
        }

        private void btn_deleteUser_IsClicked(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the record of \"{firstName} {lastName}\"? This cannot be undone", "Delete User", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM tbl_User WHERE userName = @userName";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@userName", username);
                            command.ExecuteNonQuery();
                            MessageBox.Show("User record successfully deleted", "Delete User", MessageBoxButton.OK, MessageBoxImage.Information);
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
