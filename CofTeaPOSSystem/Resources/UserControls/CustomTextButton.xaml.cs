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
    /// Interaction logic for CustomTextButton.xaml
    /// </summary>
    public partial class CustomTextButton : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event RoutedEventHandler IsClicked;
        private string c_btn_Text;
        private int c_btn_FontSize;
        private string c_btn_Foreground;
        private string c_btn_FontFamily;




        public CustomTextButton()
        {
            InitializeComponent();
            DataContext = this;

            C_btn_Text = "Text";
            C_btn_FontSize = 16;
            C_btn_Foreground = "Black";
            C_btn_FontFamily = "Arial";
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        public string C_btn_Text
        {
            get { return c_btn_Text; }
            set
            {
                c_btn_Text = value;
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

        private void Hovered_TextBlock(object sender, MouseEventArgs e)
        {
            Button thisButton = sender as Button;
            TextBlock textBlock = thisButton.Template.FindName("tb", thisButton) as TextBlock;
            if (btn.IsMouseOver)
            {
                textBlock.TextDecorations = TextDecorations.Underline;
            }
            else
            {
                textBlock.TextDecorations = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsClicked?.Invoke(this, e);
        }
    }
}
