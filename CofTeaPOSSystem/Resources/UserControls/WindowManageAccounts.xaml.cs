using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
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
using System.Data;
using CofTeaPOSSystem.Windows;

namespace CofTeaPOSSystem.Resources.UserControls
{

    public partial class WindowManageAccounts : UserControl
    {
        string connectionString = Globals.connectionString;
        Window ParentWindow;

        int userID;

        public WindowManageAccounts(Window parentWindow, int userID)
        {
            InitializeComponent();
            this.userID = userID;

            ParentWindow = parentWindow;
            ShowUpdatedTable();
        }

        private void ShowUpdatedTable()
        {
            string userFirstName = Globals.GetUserDetails(userID).Item1;
            tb_user.Text = $"Welcome, {userFirstName}";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string commandText = "SELECT userType AS \"Role\", " +
                                     "firstName AS \"First Name\", " +
                                     "lastName AS \"Last Name\", " +
                                     "email AS Email, " +
                                     "contact AS \"Contact Number\", " +
                                     "userName AS Username, " +
                                     "password AS Password, " +
                                     "dateCreated AS \"Date Registered\" " +
                                     "FROM tbl_User " +
                                     "INNER JOIN tbl_UserType ON tbl_UserType.id = tbl_User.userTypeId";

                using (var command = new SQLiteCommand(commandText, connection))
                {
                    var adapter = new SQLiteDataAdapter(command);
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    abcd.ItemsSource = dataTable.DefaultView;
                }
            }
        }

        private void Btn_NewUser_Click(object sender, RoutedEventArgs e)
        {
            AddUser obj = new AddUser();

            ParentWindow.Opacity = 0.4;
            obj.ShowDialog();
            ShowUpdatedTable();
            ParentWindow.Opacity = 1;
        }

        private void Datagrid_EditRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string role, fname, lname, email, contact, username, password;

            if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)
            {
                if (dataGrid.SelectedItem is DataRowView selectedRow)
                {
                    role = selectedRow.Row.ItemArray[0].ToString();
                    fname = selectedRow.Row.ItemArray[1].ToString();
                    lname = selectedRow.Row.ItemArray[2].ToString();
                    email = selectedRow.Row.ItemArray[3].ToString();
                    contact = selectedRow.Row.ItemArray[4].ToString();
                    username = selectedRow.Row.ItemArray[5].ToString();
                    password = selectedRow.Row.ItemArray[6].ToString();

                    if (Globals.ManageAccounts_IsEditingSelf(userID, username))
                    {
                        MessageBox.Show("You cannot edit yourself. Use \"Account Settings\" instead", "Edit User", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        AddUser obj = new AddUser(isNewUser: false,
                                                  role: role,
                                                  fname: fname,
                                                  lname: lname,
                                                  username: username,
                                                  password: password,
                                                  email: email,
                                                  contact: contact);

                        ParentWindow.Opacity = 0.4;
                        obj.ShowDialog();
                        ShowUpdatedTable();
                        ParentWindow.Opacity = 1;
                    }
                }
            }
        }

    }
}
