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
using System.Windows.Shapes;

namespace CofTeaPOSSystem.Windows
{
    /// <summary>
    /// Interaction logic for ChangeUserDetails.xaml
    /// </summary>
    public partial class ChangeUserDetails : Window
    {
        int modeType, userID;
        public ChangeUserDetails(int userID, int modeType)
        {
            InitializeComponent();
            this.modeType = modeType;
            this.userID = userID;

            SetWindowDetails();
        }

        private void SetWindowDetails()
        {
            switch (modeType)
            {
                case 1: // Username
                    tb_Header.Text = "Username Change";
                    tb_Description.Text = "You are changing your username";
                    tb_TextboxHeader.Text = "Username";
                    txt_inputField.C_tb_Text = "Enter new username";
                    break;

                case 2: // Email
                    tb_Header.Text = "Email Address Change";
                    tb_Description.Text = "You are changing your email address";
                    tb_TextboxHeader.Text = "Email Address";
                    txt_inputField.C_tb_Text = "Enter new email address";
                    break;

                case 3: // Contact
                    tb_Header.Text = "Contact Number Change";
                    tb_Description.Text = "You are changing your contact number";
                    tb_TextboxHeader.Text = "Contact Number";
                    txt_inputField.C_tb_Text = "Enter new contact number";
                    break;
            }
        }

        private void Btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            string inputField;

            if (!string.IsNullOrEmpty(txt_inputField.C_txt_Text))
            {
                inputField = txt_inputField.C_txt_Text.Trim();

                switch (modeType)
                {
                    case 1: // Username
                        Globals.AccountSettings_UpdateUsername(userID, inputField);
                        this.Close();
                        break;

                    case 2: // Email
                        Globals.AccountSettings_UpdateEmail(userID, inputField);
                        this.Close();
                        break;

                    case 3: // Contact
                        Globals.AccountSettings_UpdateContact(userID, inputField);
                        this.Close();
                        break;
                }
            }
            else
            {
                MessageBox.Show("Please fill out all required fields", "Submit", MessageBoxButton.OK, MessageBoxImage.Error);
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
