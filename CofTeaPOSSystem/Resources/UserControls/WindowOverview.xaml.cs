using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    public partial class WindowOverview : UserControl
    {
        Window ParentWindow;
        int userID;

        double totalIncome = 0.00;
        int totalOrders = 0;
        int totalReceipt = 0;

        public WindowOverview(Window ParentWindow, int userID)
        {
            InitializeComponent();
            this.ParentWindow = ParentWindow;
            this.userID = userID;

            FillDataGrid();
            tb_total.C_totalPrice = GetIncome();
            tb_orders.C_totalPrice = GetOrders();
            tb_receipt.C_totalPrice = GetReceipt();

            tb_user.Text = $"Welcome, {Globals.GetUserDetails(userID).Item1}";
        }

        private void FillDataGrid()
        {
            string dateToday = $"{DateTime.Now.ToString("yyyy/MM/dd")}%";

            using (var connection = new SQLiteConnection(Globals.connectionString))
            {
                connection.Open();

                string commandText = $@"SELECT
                                        tbl_SalesReceipt.id AS ""Receipt ID"",
                                        tbl_ProductSeries.seriesName AS ""Series Name"", 
                                        tbl_Product.productName AS ""Product Name"", 
                                        tbl_Cups.cupName AS ""Cup Size"",
                                        tbl_Product.productPrice AS ""Price"",
                                        tbl_Sales.amount AS ""Amount"", 
                                        (tbl_Sales.amount * tbl_Product.productPrice) AS ""Subtotal"",
                                        tbl_SalesReceipt.discount AS ""Discount %"",
                                        ROUND(((tbl_Sales.amount * tbl_Product.productPrice) * ((100.00 - tbl_SalesReceipt.discount) / 100.00)), 2) AS ""Total Price"",
                                        tbl_PaymentMethod.methodName AS ""Payment Method"",
                                        tbl_User.firstName AS ""Issued By"",
                                        tbl_SalesReceipt.receiptDate AS ""Timestamp""
                                        FROM tbl_SalesReceipt
                                        INNER JOIN tbl_Sales ON tbl_Sales.receiptId = tbl_SalesReceipt.id
                                        INNER JOIN tbl_Product ON tbl_Sales.productId = tbl_Product.id
                                        INNER JOIN tbl_ProductSeries ON tbl_Product.seriesId = tbl_ProductSeries.id
                                        INNER JOIN tbl_PaymentMethod ON tbl_PaymentMethod.id = tbl_SalesReceipt.paymentId
                                        INNER JOIN tbl_User ON tbl_User.id = tbl_SalesReceipt.userId
                                        LEFT JOIN tbl_Cups ON tbl_Product.cupId = tbl_Cups.id
										WHERE tbl_SalesReceipt.receiptDate LIKE '{dateToday}'

										UNION
                                        SELECT
                                        tbl_SalesReceipt.id AS ""Receipt ID"",
                                        tbl_ArchivedProductSeries.seriesName AS ""Series Name"", 
                                        tbl_ArchivedProduct.productName AS ""Product Name"", 
                                        tbl_ArchivedCups.cupName AS ""Cup Size"",
                                        tbl_ArchivedProduct.productPrice AS ""Price"",
                                        tbl_Sales.amount AS ""Amount"", 
                                        (tbl_Sales.amount * tbl_ArchivedProduct.productPrice) AS ""Subtotal"",
                                        tbl_SalesReceipt.discount AS ""Discount %"",
                                        ROUND(((tbl_Sales.amount * tbl_ArchivedProduct.productPrice) * ((100.00 - tbl_SalesReceipt.discount) / 100.00)), 2) AS ""Total Price"",
                                        tbl_PaymentMethod.methodName AS ""Payment Method"",
                                        tbl_User.firstName AS ""Issued By"",
                                        tbl_SalesReceipt.receiptDate AS ""Timestamp""
                                        FROM tbl_SalesReceipt
                                        INNER JOIN tbl_Sales ON tbl_Sales.receiptId = tbl_SalesReceipt.id
                                        INNER JOIN tbl_ArchivedProduct ON tbl_Sales.productId_Archived = tbl_ArchivedProduct.id
                                        INNER JOIN tbl_ArchivedProductSeries ON tbl_ArchivedProduct.seriesId = tbl_ArchivedProductSeries.id
                                        INNER JOIN tbl_PaymentMethod ON tbl_PaymentMethod.id = tbl_SalesReceipt.paymentId
                                        INNER JOIN tbl_User ON tbl_User.id = tbl_SalesReceipt.userId
                                        LEFT JOIN tbl_ArchivedCups ON tbl_ArchivedProduct.cupId = tbl_ArchivedCups.id
                                        WHERE tbl_SalesReceipt.receiptDate LIKE '{dateToday}'
                                        ORDER BY ""Timestamp"" DESC";

                using (var command = new SQLiteCommand(commandText, connection))
                {
                    var adapter = new SQLiteDataAdapter(command);
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    abcd.ItemsSource = dataTable.DefaultView;
                }
            }
        }

        private string GetIncome()
        {
            foreach (DataRowView row in abcd.ItemsSource)
            {
                totalIncome += Convert.ToDouble(row["Total Price"]);
                tb_total.C_totalPrice = $"₱ {Math.Round(totalIncome, 2, MidpointRounding.AwayFromZero)}";
            }
            return $"₱ {Math.Round(totalIncome, 2, MidpointRounding.AwayFromZero).ToString()}";
        }

        private string GetOrders()
        {
            foreach (DataRowView row in abcd.ItemsSource)
            {
                totalOrders += 1;
            }
            return $"{totalOrders}";
        }

        private string GetReceipt()
        {
            List<int> receiptIDs = new List<int>();

            foreach (DataRowView row in abcd.ItemsSource)
            {
                int receiptID = Convert.ToInt32(row["Receipt ID"]);

                if (!receiptIDs.Contains(receiptID))
                {
                    receiptIDs.Add(receiptID);
                    totalReceipt += 1;
                }
            }
            return $"{totalReceipt}";
        }

    }
}
