using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// <summary>
    /// Interaction logic for CustomPasswordBox.xaml
    /// </summary>
    public partial class CustomPasswordBox : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string c_tb_Text;
        private string c_tb_TextFamily;
        private int c_tb_FontSize;
        private string c_tb_Foreground;
        private string c_pb_BorderBrush;
        private string c_pb_Foreground;

        public CustomPasswordBox()
        {
            InitializeComponent();
            DataContext = this;

            C_tb_Text = "This is a text";
            C_tb_TextFamily = "Arial";
            C_tb_FontSize = 16;
            C_tb_Foreground = "Black";

            C_pb_BorderBrush = "#FFEDDB";
            C_pb_Foreground = "Black";
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string C_pb_Foreground
        {
            get { return c_pb_Foreground; }
            set
            {
                c_pb_Foreground = value;
                OnPropertyChanged();
            }
        }

        public string C_tb_Text
        {
            get { return c_tb_Text; }
            set
            {
                c_tb_Text = value;
                OnPropertyChanged();
            }
        }

        public string C_tb_TextFamily
        {
            get { return c_tb_TextFamily; }
            set
            {
                c_tb_TextFamily = value;
                OnPropertyChanged();
            }
        }

        public int C_tb_FontSize
        {
            get { return c_tb_FontSize; }
            set
            {
                c_tb_FontSize = value;
                OnPropertyChanged();
            }
        }

        public string C_tb_Foreground
        {
            get { return c_tb_Foreground; }
            set
            {
                c_tb_Foreground = value;
                OnPropertyChanged();
            }
        }

        public string C_pb_BorderBrush
        {
            get { return c_pb_BorderBrush; }
            set
            {
                c_pb_BorderBrush = value;
                OnPropertyChanged();
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(passwordbox.Password))
            {
                textblock.Visibility = Visibility.Visible;
            }
            else
            {
                textblock.Visibility = Visibility.Hidden;
            }
        }
    }
}
