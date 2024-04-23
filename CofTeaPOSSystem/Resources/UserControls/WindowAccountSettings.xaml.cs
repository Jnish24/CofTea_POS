using CofTeaPOSSystem.Windows;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CofTeaPOSSystem.Resources.UserControls
{
    public partial class WindowAccountSettings : UserControl
    {
        string fname, lname, username, email, contact;
        int userID, userRole;
        Window parentWindow;

        public WindowAccountSettings(Window parentWindow, int userID, int userRole)
        {
            InitializeComponent();

            this.userID = userID;
            this.userRole = userRole;
            this.parentWindow = parentWindow;
            GetUserInformation();

            if (userRole == 2)
            {
                personalInfo.Visibility = Visibility.Collapsed;
                contactInfo.Visibility = Visibility.Collapsed;
            }
        }

        private void GetUserInformation()
        {
            fname = Globals.GetUserDetails(userID).Item1;
            lname = Globals.GetUserDetails(userID).Item2;
            username = Globals.GetUserDetails(userID).Item3;
            email = Globals.GetUserDetails(userID).Item4;
            contact = Globals.GetUserDetails(userID).Item5;

            txt_fname.C_txt_Text = fname;
            txt_lname.C_txt_Text = lname;
            tb_username.Text = username;
            tb_email.Text = email;
            tb_contact.Text = contact;

            tb_user.Text = $"Welcome, {fname}";
        }

        private void Btn_ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = pb_newPass.passwordbox.Password;
            string confirmPassword = pb_confirmPass.passwordbox.Password;

            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill out all required fields", "Change Password", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (newPassword.Trim().Length < 8)
            {
                MessageBox.Show("Password must consist at least 8 characters", "Change Password", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (newPassword.Trim() != confirmPassword.Trim())
            {
                MessageBox.Show("Password do not match", "Change Password", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Globals.AccountSettings_UpdatePassword(userID, newPassword);
                pb_newPass.passwordbox.Password = string.Empty;
                pb_confirmPass.passwordbox.Password = string.Empty;
            }
        }

        private void Btn_UpdatePersonal_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_fname.C_txt_Text) && !string.IsNullOrEmpty(txt_lname.C_txt_Text))
            {
                string inputFname, inputLname;
                inputFname = txt_fname.C_txt_Text.Trim();
                inputLname = txt_lname .C_txt_Text.Trim();

                if (inputFname == fname && inputLname == lname) {}
                else
                {
                    Globals.AccountSettings_UpdateName(userID, inputFname, inputLname);
                    GetUserInformation();
                }
            }
            else
            {
                MessageBox.Show("Please fill out all required fields", "Update Personal Information", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn_ChangeUsername_Click(object sender, RoutedEventArgs e)
        {
            ChangeUserDetails obj = new ChangeUserDetails(userID, 1);
            parentWindow.Opacity = 0.4;
            obj.ShowDialog();
            GetUserInformation();
            parentWindow.Opacity = 1;
        }

        private void Btn_ChangeEmail_Click(object sender, RoutedEventArgs e)
        {
            ChangeUserDetails obj = new ChangeUserDetails(userID, 2);
            parentWindow.Opacity = 0.4;
            obj.ShowDialog();
            GetUserInformation();
            parentWindow.Opacity = 1;
        }

        private void Btn_ChangeContact_Click(object sender, RoutedEventArgs e)
        {
            ChangeUserDetails obj = new ChangeUserDetails(userID, 3);
            parentWindow.Opacity = 0.4;
            obj.ShowDialog();
            GetUserInformation();
            parentWindow.Opacity = 1;
        }

    }
}
