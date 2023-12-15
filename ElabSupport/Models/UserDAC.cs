using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ElabSupport.Models
{
    public class UserDAC
    {
       public string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public DataTable GetUserRoles(int UserRoleId)
        {
            
            var dataTable = new DataTable();
            var querry = "";
            if (UserRoleId > 0)
            {
                querry = "SELECT * FROM UserRole where UserRoleId = @userroleid";
            }
            else
            {
                querry = "SELECT * FROM UserRole";
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(querry, connection))
                {
                    if (UserRoleId > 0)
                    {
                        command.Parameters.AddWithValue("@userroleid", UserRoleId);
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }
        public DataTable GetUsers(Guid UserId)
        {
            var dataTable = new DataTable();
            var querry = "";
            if (UserId != null && UserId != Guid.Empty)
            {
                querry = "SELECT * FROM Users where Active =1 AND UserId = @UserId";
            }
            else
            {
                querry = "SELECT * FROM Users where Active =1";
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(querry, connection))
                {
                    if (UserId != null && UserId != Guid.Empty)
                    {
                        command.Parameters.AddWithValue("@UserId", UserId);
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }
        public UseRoleModel AddUserRole(UseRoleModel newUserRole)
        {
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Assuming "UserRoles" is the name of your database table
                        string insertQuery = @"INSERT INTO UserRole (UserRole, UserRoleDescription, Rates)
                                           VALUES (@UserRole, @UserRoleDescription, @Rates);
                                           SELECT SCOPE_IDENTITY();"; // Assuming UserRoleId is an identity column

                        using (SqlCommand command = new SqlCommand(insertQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@UserRole", newUserRole.UserRole);
                            command.Parameters.AddWithValue("@UserRoleDescription", newUserRole.UserRoleDescription);
                            command.Parameters.AddWithValue("@Rates", newUserRole.Rates ?? (object)DBNull.Value);

                            // Execute the insert query and get the newly created UserRoleId
                            newUserRole.UserRoleId = Convert.ToInt32(command.ExecuteScalar());
                        }

                        // Commit the transaction
                        transaction.Commit();
                        return newUserRole;
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an exception
                        transaction.Rollback();
                        throw new Exception($"Failed to add user role. {ex.Message}");
                    }
                }
            }
        }
        public UseRoleModel UpdateUserRole(UseRoleModel newUserRole)
        {
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Assuming "UserRoles" is the name of your database table
                        string updateQuery = @"
                    UPDATE UserRole
                    SET UserRole = @UserRole,
                        UserRoleDescription = @UserRoleDescription,
                        Rates = @Rates
                    WHERE UserRoleId = @UserRoleId";

                        using (SqlCommand command = new SqlCommand(updateQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@UserRoleId", newUserRole.UserRoleId);
                            command.Parameters.AddWithValue("@UserRole", newUserRole.UserRole);
                            command.Parameters.AddWithValue("@UserRoleDescription", newUserRole.UserRoleDescription);
                            command.Parameters.AddWithValue("@Rates", newUserRole.Rates ?? (object)DBNull.Value);

                            // Execute the update query
                            command.ExecuteNonQuery();
                        }

                        // Commit the transaction
                        transaction.Commit();
                        return newUserRole;
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an exception
                        transaction.Rollback();
                        throw new Exception($"Failed to update user role. {ex.Message}");
                    }
                }
            }
        }
        public DataTable GetLogin(string UserName, string Password)
        {
            var dataTable = new DataTable();
            var querry = "SELECT * FROM Users where Active =1 AND Username = @UserName AND Password=@Password ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(querry, connection))
                {

                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", Password);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }
        public int UploadExcelData(List<ExcelData> excelData)
        {
            int result = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var data in excelData)
                        {
                            foreach (var OOSPerson in data.SupportPersons)
                            {
                                
                                    using (SqlCommand command = new SqlCommand("OOSExcelUpload", connection, transaction))
                                    {
                                        command.CommandType = CommandType.StoredProcedure;

                                        // Assuming your stored procedure parameters are named @ScheduleDate and @OOSPerson
                                        command.Parameters.AddWithValue("@ScheduleDate", data.Date);
                                        command.Parameters.AddWithValue("@OOSPerson", OOSPerson!=null?OOSPerson:"");

                                        // ExecuteNonQuery returns the number of rows affected, which may not be needed
                                        // If you need to capture the InsertedId, you can use ExecuteScalar instead
                                        // Note: Make sure to cast the result appropriately (e.g., int)
                                        result += Convert.ToInt32(command.ExecuteScalar());
                                    }
                                
                            }
                        }

                        // If everything is successful, commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions and log the error
                        transaction.Rollback();
                        // You might also want to throw the exception or log it for further investigation
                        throw;
                    }
                }
            }

            return result;
        }
        public List<SupportData> GetSupportData()
        {
            List<SupportData> result = new List<SupportData>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetSupportPersonsByDate", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SupportData supportPersonsByDate = new SupportData
                            {
                                Date = (DateTime)reader["ScheduleDate"],
                                Support1 = reader["Support1"] != DBNull.Value ? reader["Support1"].ToString() : null,
                                Support2 = reader["Support2"] != DBNull.Value ? reader["Support2"].ToString() : null,
                                Support3 = reader["Support3"] != DBNull.Value ? reader["Support3"].ToString() : null,
                                ONCALLSUPPORTPERSON = reader["ONCALLSUPPORTPERSON"] != DBNull.Value ? reader["ONCALLSUPPORTPERSON"].ToString() : null,
                                Support4 = reader["Support4"] != DBNull.Value ? reader["Support4"].ToString() : null,
                                Support5 = reader["Support5"] != DBNull.Value ? reader["Support5"].ToString() : null,
                                Support6 = reader["Support6"] != DBNull.Value ? reader["Support6"].ToString() : null,
                                Support7 = reader["Support7"] != DBNull.Value ? reader["Support7"].ToString() : null,
                                Support8 = reader["Support8"] != DBNull.Value ? reader["Support8"].ToString() : null,
                                Support9 = reader["Support9"] != DBNull.Value ? reader["Support9"].ToString() : null,
                                Support10 = reader["Support10"] != DBNull.Value ? reader["Support10"].ToString() : null,
                                Support11 = reader["Support11"] != DBNull.Value ? reader["Support11"].ToString() : null,
                                Support12 = reader["Support12"] != DBNull.Value ? reader["Support12"].ToString() : null,
                                MACHINEINTERFACESUPPORT = reader["MACHINEINTERFACESUPPORT"] != DBNull.Value ? reader["MACHINEINTERFACESUPPORT"].ToString() : null,
                            };

                            result.Add(supportPersonsByDate);
                        }
                    }
                }
            }

            return result;
        }
        public SupportData
    }
}