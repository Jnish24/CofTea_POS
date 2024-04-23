using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CofTeaPOSSystem.Windows
{
    public partial class ForgotPasswordWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        string connectionString = Globals.connectionString;

        private string header_Text;
        private string description_Text;
        private string textbox_Header;

        string randomNumber, email;
        bool otpSent = false;
        int count = 1;

        TextBox emailTextBox;
        TextBlock emailTextBlock;

        public ForgotPasswordWindow()
        {
            InitializeComponent();
            DataContext = this;

            Header_Text = "Forgot your password?";
            Description_Text = "Don't worry! Resetting your password is easy. Just type in the email you used to register your account";
            Textbox_Header = "Email";
        }

        // BINDER
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // METHODS USED IN EVENTS
        private void AccessDatabase()
        {
            emailTextBox = (TextBox)txt_email.FindName("textbox");

            email = emailTextBox.Text.Trim();

            try // Should there be an error when accessing the database.
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT email FROM tbl_User WHERE email = @email";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@email", email);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                SendOTP();
                            }
                            else
                            {
                                MessageBox.Show("No user exists with that email", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void SendOTP()
        {
            string from, messageBody;

            Random rand = new Random();
            randomNumber = rand.Next(100000, 999999).ToString(); // Makes a random 6 digit number that is converted to string

            from = "venicecalifornia123@gmail.com"; // Email that is used to send the message
            messageBody = $"Your reset code is: {randomNumber}";

            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(from, "gaqk norv xgqw vsjt"); // Replace "your_password" with the actual password for the sender's email

                try
                {
                    using (MailMessage message = new MailMessage(from, email, "Password Reset Code", messageBody))
                    {
                        smtpClient.Send(message);
                        MessageBox.Show("Reset code sent successfully!", "Status", MessageBoxButton.OK, MessageBoxImage.Information);
                        otpSent = true;
                        ChangePassword();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to send reset code: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ChangePassword()
        {
            emailTextBlock = (TextBlock)txt_email.FindName("textblock");
            emailTextBox.Clear();
            emailTextBlock.Text = "Enter 6-digit code";

            Header_Text = "Enter the OTP code";
            Description_Text = "Enter the one-time password (OTP) code you received so you can change your password";
            Textbox_Header = "OTP";
        }

        // EVENTS
        private void Btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            if (otpSent)
            {
                if (emailTextBox.Text.Trim() == randomNumber)
                {
                    ChangePasswordWindow obj = new ChangePasswordWindow(email);
                    this.Hide();
                    obj.ShowDialog();
                    this.Close();
                }
                else
                {
                    if (count < 5)
                    {
                        count += 1;
                        MessageBox.Show("Incorrect OTP code", "OTP", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Max trials reached, this window will close", "OTP", MessageBoxButton.OK, MessageBoxImage.Warning);
                        this.Close();
                    }
                }
            }
            else
            {
                AccessDatabase();
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


        // DATA BINDING
        public string Header_Text
        {
            get { return header_Text; }
            set
            {
                header_Text = value;
                OnPropertyChanged();
            }
        }

        public string Description_Text
        {
            get { return description_Text; }
            set
            {
                description_Text = value;
                OnPropertyChanged();
            }
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public string Textbox_Header
        {
            get { return textbox_Header; }
            set
            {
                textbox_Header = value;
                OnPropertyChanged();
            }
        }
    }
}
