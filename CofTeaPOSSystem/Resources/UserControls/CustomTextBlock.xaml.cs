using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
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
    public partial class CustomTextBlock : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string c_totalPrice;
        private string c_header_text;
        private string c_icon;

        public CustomTextBlock()
        {
            InitializeComponent();
            DataContext = this;

            C_totalPrice = "₱ 0.00";
            C_header_text = "Today";
            C_icon = "Money";
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string C_totalPrice
        {
            get { return c_totalPrice; }
            set
            {
                c_totalPrice = value;
                OnPropertyChanged();
            }
        }

        public string C_icon
        {
            get { return c_icon; }
            set
            {
                c_icon = value;
                OnPropertyChanged();
            }
        }

        public string C_header_text
        {
            get { return c_header_text; }
            set
            {
                c_header_text = value;
                OnPropertyChanged();
            }
        }

    }
}
