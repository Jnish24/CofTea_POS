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
    /// Interaction logic for CustomTextBox.xaml
    /// </summary>
    public partial class CustomTextBox : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event TextCompositionEventHandler IsPreviewTextInput;

        private string c_txt_FontFamily;
        private int c_txt_FontSize;
        private string c_txt_Foreground;

        private string c_tb_Text;
        private string c_tb_Foreground;
        private string c_txt_BorderBrush;
        private string c_txt_Text;
        private int c_txt_CornerRadius;
        private double c_tb_Opacity;


        public CustomTextBox()
        {
            InitializeComponent();
            DataContext = this;

            C_txt_FontFamily = "Arial";
            C_txt_FontSize = 16;
            C_txt_Foreground = "Black";

            C_tb_Text = "This is a text";
            C_tb_Foreground = "Black";

            C_txt_BorderBrush = "#FFEDDB";
            C_txt_CornerRadius = 25;

            C_tb_Opacity = 0.5;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public double C_tb_Opacity
        {
            get { return c_tb_Opacity; }
            set
            {
                c_tb_Opacity = value;
                OnPropertyChanged();
            }
        }

        public string C_txt_Text
        {
            get { return c_txt_Text; }
            set
            {
                c_txt_Text = value;
                OnPropertyChanged();
            }
        }

        public int C_txt_CornerRadius
        {
            get { return c_txt_CornerRadius; }
            set
            {
                c_txt_CornerRadius = value;
                OnPropertyChanged();
            }
        }

        public string C_txt_BorderBrush
        {
            get { return c_txt_BorderBrush; }
            set
            {
                c_txt_BorderBrush = value;
                OnPropertyChanged();
            }
        }

        public string C_txt_FontFamily
        {
            get { return c_txt_FontFamily; }
            set
            {
                c_txt_FontFamily = value;
                OnPropertyChanged();
            }
        }

        public int C_txt_FontSize
        {
            get { return c_txt_FontSize; }
            set
            {
                c_txt_FontSize = value;
                OnPropertyChanged();
            }
        }

        public string C_txt_Foreground
        {
            get { return c_txt_Foreground; }
            set
            {
                c_txt_Foreground = value;
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

        public string C_tb_Foreground
        {
            get { return c_tb_Foreground; }
            set
            {
                c_tb_Foreground = value;
                OnPropertyChanged();
            }
        }

        private void textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(textbox.Text))
            {
                textblock.Visibility = Visibility.Visible;
            }
            else
            {
                textblock.Visibility = Visibility.Hidden;
            }
        }

        private void textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IsPreviewTextInput?.Invoke(this, e);
        }
    }
}
