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
    public partial class CustomDashboardButtons : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event RoutedEventHandler IsClicked;

        private string c_fa_Icon;
        private string c_txt_Text;
        private string c_grid_Background;

        public CustomDashboardButtons()
        {
            InitializeComponent();
            DataContext = this;

            C_fa_Icon = "Eye";
            C_txt_Text = "Text";
            C_grid_Background = "Transparent";
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string C_grid_Background
        {
            get { return c_grid_Background; }
            set
            {
                c_grid_Background = value;
                OnPropertyChanged();
            }
        }

        public string C_fa_Icon
        {
            get { return c_fa_Icon; }
            set
            {
                c_fa_Icon = value;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsClicked?.Invoke(this, e);
        }
    }
}
