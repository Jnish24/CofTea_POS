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
    public partial class CustomButton : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event RoutedEventHandler IsClicked;

        private string c_btn_FontFamily;
        private int c_btn_FontSize;
        private string c_btn_Foreground;
        private string c_btn_Background;
        private string c_btn_Content;
        private string c_btn_HoverColor;
        private int c_btn_CornerRadius;

        public CustomButton()
        {
            InitializeComponent();
            DataContext = this;

            C_btn_FontFamily = "Arial";
            C_btn_FontSize = 16;
            C_btn_Foreground = "Black";
            C_btn_Background = "White";
            C_btn_Content = "Button";

            C_btn_HoverColor = "Gray";

            C_btn_CornerRadius = 25;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int C_btn_CornerRadius
        {
            get { return c_btn_CornerRadius; }
            set
            {
                c_btn_CornerRadius = value;
                OnPropertyChanged();
            }
        }

        public string C_btn_FontFamily
        {
            get { return c_btn_FontFamily; }
            set
            {
                c_btn_FontFamily = value;
                OnPropertyChanged();
            }
        }

        public int C_btn_FontSize
        {
            get { return c_btn_FontSize; }
            set
            {
                c_btn_FontSize = value;
                OnPropertyChanged();
            }
        }

        public string C_btn_Foreground
        {
            get { return c_btn_Foreground; }
            set
            {
                c_btn_Foreground = value;
                OnPropertyChanged();
            }
        }

        public string C_btn_Background
        {
            get { return c_btn_Background; }
            set
            {
                c_btn_Background = value;
                OnPropertyChanged();
            }
        }

        public string C_btn_Content
        {
            get { return c_btn_Content; }
            set
            {
                c_btn_Content = value;
                OnPropertyChanged();
            }
        }

        public string C_btn_HoverColor
        {
            get { return c_btn_HoverColor; }
            set
            {
                c_btn_HoverColor = value;
                OnPropertyChanged();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsClicked?.Invoke(this, e);
        }
    }
}
