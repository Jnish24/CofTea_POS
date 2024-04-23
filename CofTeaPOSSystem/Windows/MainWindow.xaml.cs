using CofTeaPOSSystem.Resources.UserControls;
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
    public partial class MainWindow : Window
    {
        int userID, userRole;
        string userFirstName;
        CustomDashboardButtons activeButton = new CustomDashboardButtons();

        public MainWindow(int userID)
        {
            InitializeComponent();

            this.userID = userID;
            this.userRole = Globals.GetUserRole(this.userID);
            userFirstName = Globals.MainWindow_GetUserFirstName(this.userID);

            if (userRole == 1) { }
            else if (userRole == 2)
            {
                btn_SalesReport.Visibility = Visibility.Collapsed;
                btn_ManageMenu.Visibility = Visibility.Collapsed;
                btn_ManageAccounts.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("An error occurred: role ID error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Btn_Overview_IsClicked(object sender, RoutedEventArgs e)
        {
            if (!(childWindow.Content is WindowOverview))
            {
                activeButton.C_grid_Background = null;
                activeButton = btn_Overview;
                btn_Overview.C_grid_Background = "#D1A782";

                WindowOverview obj = new WindowOverview(this, userID);
                childWindow.Content = obj;
            }
        }

        private void Btn_OrderBoard_IsClicked(object sender, RoutedEventArgs e)
        {
            if (!(childWindow.Content is WindowOrderBoard))
            {
                activeButton.C_grid_Background = null;
                activeButton = btn_OrderBoard;
                btn_OrderBoard.C_grid_Background = "#D1A782";

                WindowOrderBoard obj = new WindowOrderBoard(this, userID);
                childWindow.Content = obj;
            }
        }

        private void Btn_ManageAccounts_Click(object sender, RoutedEventArgs e)
        {
            if (!(childWindow.Content is WindowManageAccounts))
            {
                activeButton.C_grid_Background = null;
                activeButton = btn_ManageAccounts;
                btn_ManageAccounts.C_grid_Background = "#D1A782";
                
                WindowManageAccounts obj = new WindowManageAccounts(this, userID);
                childWindow.Content = obj;
            }
        }

        private void Btn_SalesReport_IsClicked(object sender, RoutedEventArgs e)
        {
            if (!(childWindow.Content is WindowSalesReport))
            {
                activeButton.C_grid_Background = null;
                activeButton = btn_SalesReport;
                btn_SalesReport.C_grid_Background = "#D1A782";

                WindowSalesReport obj = new WindowSalesReport(this, userID);
                childWindow.Content = obj;
            }

        }

        private void Btn_AccountSettings_Click(object sender, RoutedEventArgs e)
        {
            if (!(childWindow.Content is WindowAccountSettings))
            {
                activeButton.C_grid_Background = null;
                activeButton = btn_AccountSettings;
                btn_AccountSettings.C_grid_Background = "#D1A782";

                WindowAccountSettings obj = new WindowAccountSettings(this, userID, userRole);
                childWindow.Content = obj;
            }
        }

        private void Btn_AddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!(childWindow.Content is WindowAddProduct))
            {
                activeButton.C_grid_Background = null;
                activeButton = btn_ManageMenu;
                btn_ManageMenu.C_grid_Background = "#D1A782";

                WindowAddProduct obj = new WindowAddProduct(this, userID);
                childWindow.Content = obj;
            }
        }

        private void Btn_Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to log out?", "Log Out", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                LoginWindow obj = new LoginWindow();
                obj.Show();
                this.Close();
            }
        }

        private void Btn_Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Btn_Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Normal;
            }
            else
            {
                
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Maximized;
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
