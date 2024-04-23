using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
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

namespace CofTeaPOSSystem.Resources.UserControls
{
    public partial class WindowSalesReport : UserControl
    {
        Window ParentWindow;
        DateTime dp_start, dp_end;

        int userID;
        string beginDate, endDate;
        bool isInitialized = false;

        double totalIncome = 0;
        int totalOrders = 0;
        int totalReceipt = 0;

        public WindowSalesReport(Window ParentWindow, int userID)
        {
            InitializeComponent();
            this.ParentWindow = ParentWindow;
            this.userID = userID;

            isInitialized = true;
            dp_maxDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            tb_user.Text = $"Welcome, {Globals.GetUserDetails(userID).Item1}";
        }

        private string GetIncome()
        {
            bool rowFound = false;
            foreach (DataRowView row in abcd.ItemsSource)
            {
                rowFound = true;
                totalIncome += Convert.ToDouble(row["Total Price"]);
                tb_total.C_totalPrice = $"₱ {Math.Round(totalIncome, 2, MidpointRounding.AwayFromZero)}";
            }

            if (rowFound)
            {
                return $"₱ {Math.Round(totalIncome, 2, MidpointRounding.AwayFromZero).ToString()}";
            }
            else
            {
                return $"₱ 0.00";
            }
        }

        private string GetOrders()
        {
            bool rowFound = false;
            foreach (DataRowView row in abcd.ItemsSource)
            {
                rowFound = true;
                totalOrders += 1;
            }

            if (rowFound)
            {
                return $"{totalOrders}";
            }
            else
            {
                return $"0";
            }
        }

        private string GetReceipt()
        {
            bool rowFound = false;
            List<int> receiptIDs = new List<int>();

            foreach (DataRowView row in abcd.ItemsSource)
            {
                int receiptID = Convert.ToInt32(row["Receipt ID"]);

                if (!receiptIDs.Contains(receiptID))
                {
                    rowFound = true;
                    receiptIDs.Add(receiptID);
                    totalReceipt += 1;
                }
            }

            if (rowFound)
            {
                return $"{totalReceipt}";
            }
            else
            {
                return $"0";
            }
        }

        private void FillDataGrid()
        {
            dp_start = dp_beginningDate.SelectedDate.Value;
            dp_end = dp_maxDate.SelectedDate.Value;
            beginDate = dp_start.ToString("yyyy/MM/dd");
            endDate = dp_end.ToString("yyyy/MM/dd");

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
										WHERE tbl_SalesReceipt.receiptDate BETWEEN '{beginDate}%' AND '{endDate}%'

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
                                        WHERE tbl_SalesReceipt.receiptDate BETWEEN '{beginDate}%' AND '{endDate}%'
                                        ORDER BY ""Timestamp"" DESC";

                using (var command = new SQLiteCommand(commandText, connection))
                {
                    var adapter = new SQLiteDataAdapter(command);
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    abcd.ItemsSource = dataTable.DefaultView;

                    totalIncome = 0;
                    totalOrders = 0;
                    totalReceipt = 0;
                    tb_total.C_totalPrice = GetIncome();
                    tb_orders.C_totalPrice = GetOrders();
                    tb_receipt.C_totalPrice = GetReceipt();
                }
            }
        }

        private void dp_beginningDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isInitialized)
            {
                FillDataGrid();
            }
        }

    }
}
