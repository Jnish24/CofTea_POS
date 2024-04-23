using CofTeaPOSSystem.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CofTeaPOSSystem
{
    internal static class Globals
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        public static Tuple<string, string, string, string, string> GetUserDetails(int userID)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT firstName, lastName, userName, email, contact FROM tbl_User WHERE id = @id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", userID);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string firstname = reader.GetString(0);
                                string lastname = reader.GetString(1);
                                string username = reader.GetString(2);
                                string email = reader.GetString(3);
                                string contact = reader.GetString(4);

                                return Tuple.Create(firstname, lastname, username, email, contact);
                            }
                            else
                            {
                                return Tuple.Create("Unknown User", "Unknown User", "Unknown User", "Unknown User", "Unknown User");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return Tuple.Create("Unknown User", "Unknown User", "Unknown User", "Unknown User", "Unknown User");
            }
        }

        public static int GetLoginID(string username)
        {
            try // Should there be an error when accessing the database.
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT id FROM tbl_User WHERE userName = @userName";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userName", username);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetInt32(0);
                            }
                            else
                            {
                                return -1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

        public static int GetUserRole(int userID)
        {
            try // Should there be an error when accessing the database.
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT userTypeId FROM tbl_User WHERE id = @id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", userID);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetInt32(0);
                            }
                            else
                            {
                                return -1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

        public static double OrderBoard_Cart_InitialPrice(int productID)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT productPrice FROM tbl_Product WHERE id = @id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", productID);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                double price =  reader.GetInt32(0);
                                return price;
                            }
                            else
                            {
                                return -1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

        public static bool AddProduct_AddSeries_NameExists(string seriesName, string index)
        {
            seriesName = seriesName.ToUpper();
            index = index.ToUpper();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT seriesName, seriesIndex FROM tbl_ProductSeries WHERE seriesName = @seriesName OR seriesIndex = @seriesIndex";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@seriesName", seriesName);
                        command.Parameters.AddWithValue("@seriesIndex", index);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string data_series = reader.GetString(0).ToUpper();
                                string data_index = reader.GetString(1).ToUpper();

                                if (data_series == seriesName)
                                {
                                    MessageBox.Show("Series name already exists", "Submit", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return true;
                                }
                                else if (data_index == index)
                                {
                                    MessageBox.Show("Series index already exists", "Submit", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return true;
                                }

                                MessageBox.Show("An error occurred: Data Record", "Submit", MessageBoxButton.OK, MessageBoxImage.Error);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
        }
        
        public static void AddProduct_AddSeries(string seriesName, string index, int productType)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO tbl_ProductSeries(typeId, seriesName, seriesIndex) VALUES (@typeId, @seriesName, @seriesIndex)";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@typeId", productType);
                        command.Parameters.AddWithValue("@seriesName", seriesName);
                        command.Parameters.AddWithValue("@seriesIndex", index);
                        command.ExecuteNonQuery();

                        MessageBox.Show("Product Series Added Successfully", "Submit", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static List<string> AddProduct_AddItem_GetAllSeries()
        {
            List<string> seriesList = new List<string>();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT seriesName FROM tbl_ProductSeries";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                seriesList.Add(reader.GetString(0));
                            }
                            return seriesList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return seriesList;
            }
        }

        public static int AddProduct_AddItem_GetProductType(string seriesName)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT typeId FROM tbl_ProductSeries WHERE seriesName = @seriesName";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@seriesName", seriesName);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetInt32(0);
                            }
                            else
                            {
                                return -1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

        public static bool AddProduct_AddItem_NameExists(string seriesName, string productName)
        {
            int seriesID = 0;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT id FROM tbl_ProductSeries WHERE seriesName = @seriesName";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@seriesName", seriesName);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                seriesID = reader.GetInt32(0);
                            }
                        }
                    }

                    query = "SELECT DISTINCT productName FROM tbl_Product WHERE productName = @productName AND seriesId = @seriesId";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@productName", productName);
                        command.Parameters.AddWithValue("@seriesId", seriesID);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
        }

        public static List<string> AddProduct_AddItem_GetAllCupSize()
        {
            List<string> cupList = new List<string>();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT cupName FROM tbl_Cups";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cupList.Add(reader.GetString(0));
                            }
                            return cupList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return cupList;
            }
        }

        public static bool AddProduct_AddCup_NameExists(string cupName)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT cupName FROM tbl_Cups WHERE cupName = @cupName";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cupName", cupName);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
        }

        public static string AddProduct_UpdatePrice_GetProductFullName(int productID)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT seriesIndex || productName 
                                     FROM tbl_Product 
                                     INNER JOIN tbl_ProductSeries ON seriesId = tbl_ProductSeries.id 
                                     WHERE tbl_Product.id = @id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", productID);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetString(0);
                            }
                            else
                            {
                                return "Unknown Product";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "Unknown Product";
            }
        }

        public static void AddProduct_UpdatePrice(int productID, double newPrice)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE tbl_Product SET productPrice = @productPrice WHERE id = @id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@productPrice", newPrice);
                        command.Parameters.AddWithValue("@id", productID);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Product Price Updated Successfully", "Set Price", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void AddProduct_UpdateItem(int productID, string productName, string oldProductName)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE tbl_Product SET productName = @productName WHERE seriesId = @seriesId AND productName = @oldProductName";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@productName", productName);
                        command.Parameters.AddWithValue("@seriesId", productID);
                        command.Parameters.AddWithValue("@oldProductName", oldProductName);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Product Item Updated Successfully", "Update Item", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static int AddProduct_UpdateItem_GetSeriesID(string seriesName)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT id FROM tbl_ProductSeries WHERE seriesName = @seriesName";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@seriesName", seriesName);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetInt32(0);
                            }
                            else
                            {
                                return -1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

        public static bool AddProduct_UpdateItem_NameExists(int seriesID, string productName)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM tbl_Product WHERE seriesId = @seriesId AND productName = @productName";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@seriesId", seriesID);
                        command.Parameters.AddWithValue("@productName", productName);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
        }

        public static bool AddProduct_UpdateSeries_NameExists(int seriesID, string seriesName)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM tbl_ProductSeries WHERE id <> @id AND seriesName = @seriesName";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", seriesID);
                        command.Parameters.AddWithValue("@seriesName", seriesName);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
        }

        public static bool AddProduct_UpdateSeries_IndexExists(int seriesID, string seriesIndex)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM tbl_ProductSeries WHERE id <> @id AND seriesIndex = @seriesIndex";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", seriesID);
                        command.Parameters.AddWithValue("@seriesIndex", seriesIndex);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
        }

        public static void AddProduct_UpdateSeries(int seriesID, string seriesName, string seriesIndex)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE tbl_ProductSeries SET seriesName = @seriesName, seriesIndex = @seriesIndex WHERE id = @id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", seriesID);
                        command.Parameters.AddWithValue("@seriesName", seriesName);
                        command.Parameters.AddWithValue("@seriesIndex", seriesIndex);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Product Series Updated Successfully", "Submit", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void AddProduct_UpdateCup(int cupID, string cupName)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE tbl_Cups SET cupName = @cupName WHERE id = @id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cupName", cupName);
                        command.Parameters.AddWithValue("@id", cupID);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Cup Name Updated Successfully", "Submit", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void AddProduct_AddCup(string cupName)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO tbl_Cups(cupName) VALUES(@cupName)";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cupName", cupName);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Cup Size Added Successfully", "Submit", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void AddProduct_AddItemStandard(string seriesName, string productName, double productPrice)
        {
            int seriesID = 0;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT id FROM tbl_ProductSeries WHERE seriesName = @seriesName";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@seriesName", seriesName);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                seriesID = reader.GetInt32(0);
                            }
                        }
                    }

                    query = "INSERT INTO tbl_Product(seriesId, productName, productPrice) VALUES(@seriesId, @productName, @productPrice)";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@seriesId", seriesID);
                        command.Parameters.AddWithValue("@productName", productName);
                        command.Parameters.AddWithValue("@productPrice", productPrice);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Product Item Added Successfully", "Submit", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void AddProduct_AddItemBeverage(string seriesName, string cupName, string productName, double productPrice)
        {
            int seriesID = 0;
            int cupID = 0;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT id FROM tbl_ProductSeries WHERE seriesName = @seriesName";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@seriesName", seriesName);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                seriesID = reader.GetInt32(0);
                            }
                        }
                    }

                    query = "SELECT id FROM tbl_Cups WHERE cupName = @cupName";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cupName", cupName);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cupID = reader.GetInt32(0);
                            }
                        }
                    }

                    query = "INSERT INTO tbl_Product(seriesId, cupId, productName, productPrice) VALUES(@seriesId, @cupId, @productName, @productPrice)";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@seriesId", seriesID);
                        command.Parameters.AddWithValue("@cupId", cupID);
                        command.Parameters.AddWithValue("@productName", productName);
                        command.Parameters.AddWithValue("@productPrice", productPrice);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void AccountSettings_UpdateContact(int userID, string inputContact)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT contact FROM tbl_User WHERE id <> @id AND contact = @contact";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", userID);
                        command.Parameters.AddWithValue("@contact", inputContact);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                MessageBox.Show("Contact number already exists", "Submit", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                    }

                    query = "UPDATE tbl_User SET contact = @contact WHERE id = @id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@contact", inputContact);
                        command.Parameters.AddWithValue("@id", userID);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Your contact number has been updated", "Submit", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void AccountSettings_UpdateEmail(int userID, string inputEmail)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT email FROM tbl_User WHERE id <> @id AND email = @email";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", userID);
                        command.Parameters.AddWithValue("@email", inputEmail);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                MessageBox.Show("Email address already exists", "Submit", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                    }

                    query = "UPDATE tbl_User SET email = @email WHERE id = @id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@email", inputEmail);
                        command.Parameters.AddWithValue("@id", userID);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Your email address has been updated", "Submit", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void AccountSettings_UpdateUsername(int userID, string inputUsername)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT userName FROM tbl_User WHERE id <> @id AND userName = @userName";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", userID);
                        command.Parameters.AddWithValue("@userName", inputUsername);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                MessageBox.Show("Username already exists", "Submit", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                    }

                    query = "UPDATE tbl_User SET userName = @userName WHERE id = @id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userName", inputUsername);
                        command.Parameters.AddWithValue("@id", userID);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Your username has been updated", "Submit", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void AccountSettings_UpdateName(int userID, string fname, string lname)
        {
            try // Should there be an error when accessing the database.
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE tbl_User SET firstName = @firstName, " +
                                   "lastName = @lastName " +
                                   "WHERE id = @id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@firstName", fname);
                        command.Parameters.AddWithValue("@lastName", lname);
                        command.Parameters.AddWithValue("@id", userID);
                        command.ExecuteNonQuery();

                        MessageBox.Show("Your name has been updated", "Submit", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void AccountSettings_UpdatePassword(int userID, string password)
        {
            try // Should there be an error when accessing the database.
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE tbl_User SET password = @password WHERE id = @id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@id", userID);
                        command.ExecuteNonQuery();

                        MessageBox.Show("Your password has been updated", "Submit", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static string MainWindow_GetUserFirstName(int userID)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT firstName FROM tbl_User WHERE id = @id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", userID);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetString(0);
                            }
                            else
                            {
                                return "Unknown User";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "Unknown User";
            }
        }

        public static bool ManageAccounts_IsEditingSelf(int currentUserID, string selectedUsername)
        {
            int selectedUserID;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT id FROM tbl_User WHERE userName = @userName";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userName", selectedUsername);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                selectedUserID = reader.GetInt32(0);

                                if (selectedUserID == currentUserID)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
        }

        public static bool AddUser_IsExistingUsername(string inputUsername, string existingUsername)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT userName FROM tbl_User WHERE userName = @userName";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userName", inputUsername);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (existingUsername == reader.GetString(0))
                                {
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool AddUser_IsExistingContact(string inputContact, string existingUsername)
        {
            if (string.IsNullOrEmpty(existingUsername))
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT contact FROM tbl_User WHERE contact = @contact";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@contact", inputContact);

                            using (SQLiteDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }
            }
            else
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT contact FROM tbl_User WHERE contact = @contact AND userName <> @userName";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@contact", inputContact);
                            command.Parameters.AddWithValue("@userName", existingUsername);

                            using (SQLiteDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }
            }
        }

        public static bool AddUser_IsExistingEmail(string? inputEmail, string existingUsername)
        {
            if (string.IsNullOrEmpty(inputEmail))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(existingUsername))
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT email FROM tbl_User WHERE email = @email";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@email", inputEmail);

                            using (SQLiteDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }
            }
            else
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT email FROM tbl_User WHERE email = @email AND userName <> @userName";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@email", inputEmail);
                            command.Parameters.AddWithValue("@userName", existingUsername);

                            using (SQLiteDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }
            }
        }

        public static void AddUser_ModifyUser(bool isNewUser, string fname, string lname, string newUsername, string password, string? email, string contact, int? role, string oldUsername)
        {
            int userID = -1;

            if (isNewUser)
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string query = "INSERT INTO tbl_User (userTypeId, firstName, lastName, email, contact, userName, password) VALUES (@userTypeId, @firstName, @lastName, @email, @contact, @userName, @password)";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@userTypeId", role);
                            command.Parameters.AddWithValue("@firstName", fname);
                            command.Parameters.AddWithValue("@lastName", lname);
                            command.Parameters.AddWithValue("@email", email);
                            command.Parameters.AddWithValue("@contact", contact);
                            command.Parameters.AddWithValue("@userName", newUsername);
                            command.Parameters.AddWithValue("@password", password);
                            command.ExecuteNonQuery();

                            MessageBox.Show("User added successfully", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT id FROM tbl_User WHERE userName = @userName";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@userName", oldUsername);

                            using (SQLiteDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    userID = reader.GetInt32(0);
                                }
                            }
                        }

                        query = "UPDATE tbl_User SET userTypeId = @userTypeId, " +
                                "firstName = @firstName, " +
                                "lastName = @lastName, " +
                                "email = @email, " +
                                "contact = @contact, " +
                                "userName = @userName, " +
                                "password = @password " +
                                "WHERE id = @id";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            //MessageBox.Show(userID.ToString());
                            command.Parameters.AddWithValue("@userTypeId", role);
                            command.Parameters.AddWithValue("@firstName", fname);
                            command.Parameters.AddWithValue("@lastName", lname);
                            command.Parameters.AddWithValue("@email", email);
                            command.Parameters.AddWithValue("@contact", contact);
                            command.Parameters.AddWithValue("@userName", newUsername);
                            command.Parameters.AddWithValue("@password", password);
                            command.Parameters.AddWithValue("@id", userID);
                            command.ExecuteNonQuery();

                            MessageBox.Show("User data has been updated", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
