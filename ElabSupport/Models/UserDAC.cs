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

    }
}