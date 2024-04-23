using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
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
using CofTeaPOSSystem.Windows;
using System.Xml.Linq;

namespace CofTeaPOSSystem.Resources.UserControls
{
    public partial class WindowAddProduct : UserControl
    {
        Window ParentWindow;
        int userID;

        public WindowAddProduct(Window ParentWindow, int userID)
        {
            InitializeComponent();
            this.ParentWindow = ParentWindow;
            this.userID = userID;
            radio_series.IsChecked = true;

            tb_user.Text = $"Welcome, {Globals.GetUserDetails(userID).Item1}";
        }

        private void Radio_Series_Checked(object sender, RoutedEventArgs e)
        {
            if (radio_series.IsChecked == true)
            {
                btn_create.Opacity = 1;
                btn_create.IsEnabled = true;
                btn_create.C_btn_Content = "Create New Series";

                using (var connection = new SQLiteConnection(Globals.connectionString))
                {
                    connection.Open();

                    string commandText = @"SELECT tbl_ProductSeries.id AS ""ID"", 
                                           tbl_ProductType.productType AS ""Product Type"", 
                                           seriesName AS ""Series Name"",
                                           seriesIndex AS ""Index"",
                                           COALESCE(COUNT(DISTINCT tbl_Product.productName), 0) AS ""Product Item Count""

                                           FROM tbl_ProductSeries 
                                           INNER JOIN tbl_ProductType ON typeId = tbl_ProductType.id
                                           LEFT JOIN tbl_Product ON tbl_ProductSeries.id = tbl_Product.seriesId
                                           GROUP BY seriesName
                                           ORDER BY ""Product Type""";

                    using (var command = new SQLiteCommand(commandText, connection))
                    {
                        var adapter = new SQLiteDataAdapter(command);
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        abcd.ItemsSource = dataTable.DefaultView;
                    }
                }
            }
            else if (radio_item.IsChecked == true)
            {
                btn_create.Opacity = 1;
                btn_create.IsEnabled = true;
                btn_create.C_btn_Content = "Create New Item";

                using (var connection = new SQLiteConnection(Globals.connectionString))
                {
                    connection.Open();

                    string commandText = @"SELECT DISTINCT seriesName AS ""Series Name"", 
                                           productName AS ""Product Item""
                                           FROM tbl_ProductSeries 
                                           INNER JOIN tbl_Product ON tbl_Product.seriesId = tbl_ProductSeries.id
                                           ORDER BY ""Series Name""";

                    using (var command = new SQLiteCommand(commandText, connection))
                    {
                        var adapter = new SQLiteDataAdapter(command);
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        abcd.ItemsSource = dataTable.DefaultView;
                    }
                }
            }
            else if (radio_cup.IsChecked == true)
            {
                btn_create.Opacity = 1;
                btn_create.IsEnabled = true;
                btn_create.C_btn_Content = "Create New Cup";

                using (var connection = new SQLiteConnection(Globals.connectionString))
                {
                    connection.Open();

                    string commandText = @"SELECT id AS ""ID"", 
                                           cupName AS ""Cup Size"" 
                                           FROM tbl_Cups
                                           ORDER BY ""ID""";

                    using (var command = new SQLiteCommand(commandText, connection))
                    {
                        var adapter = new SQLiteDataAdapter(command);
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        abcd.ItemsSource = dataTable.DefaultView;
                    }
                }
            }
            else if (radio_price.IsChecked == true)
            {
                btn_create.Opacity = 0.4;
                btn_create.IsEnabled = false;

                using (var connection = new SQLiteConnection(Globals.connectionString))
                {
                    connection.Open();

                    string commandText = @"SELECT tbl_Product.id AS ""ID"", 
                                           tbl_ProductSeries.seriesName AS ""Series"", 
                                           productName AS ""Product Name"", 
                                           tbl_Cups.cupName AS ""Cup Size"", 
                                           productPrice AS ""Price"" 

                                           FROM tbl_Product 
                                           LEFT JOIN tbl_Cups ON cupId = tbl_Cups.id
                                           INNER JOIN tbl_ProductSeries ON seriesId = tbl_ProductSeries.id
                                           ORDER BY ""Series""";

                    using (var command = new SQLiteCommand(commandText, connection))
                    {
                        var adapter = new SQLiteDataAdapter(command);
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        abcd.ItemsSource = dataTable.DefaultView;
                    }
                }
            }
        }

        private void btn_create_IsClicked(object sender, RoutedEventArgs e)
        {
            if (radio_series.IsChecked == true)
            {
                WindowNewSeries obj = new WindowNewSeries();
                ParentWindow.Opacity = 0.4;
                obj.ShowDialog();
                ParentWindow.Opacity = 1;

                Radio_Series_Checked(radio_series as object, new RoutedEventArgs(RadioButton.CheckedEvent));
            }
            else if (radio_item.IsChecked == true)
            {
                WindowNewItem obj = new WindowNewItem();
                ParentWindow.Opacity = 0.4;
                obj.ShowDialog();
                ParentWindow.Opacity = 1;

                Radio_Series_Checked(radio_item as object, new RoutedEventArgs(RadioButton.CheckedEvent));
            }
            else if (radio_cup.IsChecked == true)
            {
                WindowNewCup obj = new WindowNewCup();
                ParentWindow.Opacity = 0.4;
                obj.ShowDialog();
                ParentWindow.Opacity = 1;

                Radio_Series_Checked(radio_cup as object, new RoutedEventArgs(RadioButton.CheckedEvent));
            }
        }

        private void Datagrid_EditRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (radio_series.IsChecked == true)
            {
                int seriesID;
                string seriesName;
                string seriesIndex;

                if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
                {
                    if (dataGrid.SelectedItem is DataRowView selectedRow)
                    {
                        seriesID = Convert.ToInt32(selectedRow.Row.ItemArray[0]);
                        seriesName = selectedRow.Row.ItemArray[2].ToString();
                        seriesIndex = selectedRow.Row.ItemArray[3].ToString();

                        WindowEditSeries obj = new WindowEditSeries(seriesID, seriesName, seriesIndex);
                        ParentWindow.Opacity = 0.4;
                        obj.ShowDialog();
                        ParentWindow.Opacity = 1;

                        Radio_Series_Checked(radio_series as object, new RoutedEventArgs(RadioButton.CheckedEvent));
                    }
                }
            }
            else if (radio_item.IsChecked == true)
            {
                string seriesName;
                string productName;

                if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
                {
                    if (dataGrid.SelectedItem is DataRowView selectedRow)
                    {
                        seriesName = selectedRow.Row.ItemArray[0].ToString();
                        productName = selectedRow.Row.ItemArray[1].ToString();

                        WindowEditItem obj = new WindowEditItem(seriesName, productName);
                        ParentWindow.Opacity = 0.4;
                        obj.ShowDialog();
                        ParentWindow.Opacity = 1;

                        Radio_Series_Checked(radio_item as object, new RoutedEventArgs(RadioButton.CheckedEvent));
                    }
                }
            }
            else if (radio_cup.IsChecked == true)
            {
                int cupID;
                string cupName;

                if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
                {
                    if (dataGrid.SelectedItem is DataRowView selectedRow)
                    {
                        cupID = Convert.ToInt32(selectedRow.Row.ItemArray[0]);
                        cupName = selectedRow.Row.ItemArray[1].ToString();

                        WindowEditCup obj = new WindowEditCup(cupID, cupName);
                        ParentWindow.Opacity = 0.4;
                        obj.ShowDialog();
                        ParentWindow.Opacity = 1;

                        Radio_Series_Checked(radio_cup as object, new RoutedEventArgs(RadioButton.CheckedEvent));
                    }
                }

            }
            else if (radio_price.IsChecked == true)
            {
                int productID;
                double productPrice;
                string productCup;

                if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
                {
                    if (dataGrid.SelectedItem is DataRowView selectedRow)
                    {
                        productID = Convert.ToInt32(selectedRow.Row.ItemArray[0]);
                        productCup = selectedRow.Row.ItemArray[3].ToString();
                        productPrice = Convert.ToDouble(selectedRow.Row.ItemArray[4]);

                        WindowEditPrice obj = new WindowEditPrice(productID, productPrice, productCup);
                        ParentWindow.Opacity = 0.4;
                        obj.ShowDialog();
                        ParentWindow.Opacity = 1;

                        Radio_Series_Checked(radio_price as object, new RoutedEventArgs(RadioButton.CheckedEvent));
                    }
                }
            }
        }

        private void abcd_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (radio_cup.IsChecked == false)
            {
                if (e.PropertyName == "ID")
                {
                    e.Column.Visibility = Visibility.Hidden;
                }
            }
        }

    }
}
