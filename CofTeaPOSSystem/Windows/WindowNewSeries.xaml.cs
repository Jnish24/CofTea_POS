using CofTeaPOSSystem.Resources.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for WindowNewSeries.xaml
    /// </summary>
    public partial class WindowNewSeries : Window
    {
        public WindowNewSeries()
        {
            InitializeComponent();
        }

        private void Btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            string seriesName, index;
            int productType;

            if (string.IsNullOrEmpty(txt_seriesName.C_txt_Text) || string.IsNullOrEmpty(txt_index.C_txt_Text) || (radio_beverage.IsChecked == false && radio_standard.IsChecked == false))
            {
                MessageBox.Show("Please fill out all fields", "Submit", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (radio_beverage.IsChecked == true)
                {
                    productType = 1;
                }
                else
                {
                    productType = 2;
                }

                seriesName = txt_seriesName.C_txt_Text.Trim();
                index = txt_index.C_txt_Text.Replace(" ", "") + "_";

                if (!Globals.AddProduct_AddSeries_NameExists(seriesName, index))
                {
                    Globals.AddProduct_AddSeries(seriesName, index.ToUpper(), productType);
                    this.Close();
                }
            }
        }

        private void txt_index_IsPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
