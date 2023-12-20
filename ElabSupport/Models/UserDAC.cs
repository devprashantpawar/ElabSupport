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
                querry = "SELECT * FROM UserRole where Active = 1 AND UserRoleId = @userroleid";
            }
            else
            {
                querry = "SELECT * FROM UserRole where Active = 1";
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
        public bool DeleteUserRole(int UserRoleId)
        {
            bool deactivated = false;
            var query = "UPDATE UserRole SET Active = 0 WHERE UserRoleId = @userroleid";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userroleid", UserRoleId);

                    // Execute the query
                    int rowsAffected = command.ExecuteNonQuery();

                    // Check if any rows were affected
                    if (rowsAffected > 0)
                    {
                        // Deactivation successful
                        deactivated = true;
                    }
                }
            }

            return deactivated;
        }
        public DataTable GetUsers(string UserId =null)
        {
            var dataTable = new DataTable();
            var querry = "";
            if (!string.IsNullOrEmpty(UserId))
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
                    if (!string.IsNullOrEmpty(UserId))
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
        public int AddUserRole(UseRoleModel newUserRole)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string insertQuery = "";
                        // Assuming "UserRoles" is the name of your database table
                        if (newUserRole.UserRoleId == 0)
                        {
                            insertQuery = @"INSERT INTO UserRole (UserRole, UserRoleDescription, Rates, Active,Shift1,Shiftboth)
                                   VALUES (@UserRole, @UserRoleDescription, @Rates, 1,@Shift1,@Shiftboth);
                                   SELECT SCOPE_IDENTITY();"; // Assuming UserRoleId is an identity column
                        }
                        else
                        {
                            insertQuery = @"UPDATE UserRole 
                                    SET UserRole = @UserRole,
                                        UserRoleDescription = @UserRoleDescription,
                                        Rates = @Rates,
                                        Shift1 = @Shift1,
                                        Shiftboth = @Shiftboth
                                    WHERE UserRoleId = @userroleid;
                                    SELECT @@ROWCOUNT;";
                        }

                        using (SqlCommand command = new SqlCommand(insertQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@UserRole", newUserRole.UserRole);
                            command.Parameters.AddWithValue("@UserRoleDescription", newUserRole.UserRoleDescription);
                            command.Parameters.AddWithValue("@Rates", newUserRole.Rates ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@Shift1", newUserRole.Shift1 ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@Shiftboth", newUserRole.Shiftboth ?? (object)DBNull.Value);
                            if (newUserRole.UserRoleId > 0)
                            {
                                command.Parameters.AddWithValue("@userroleid", newUserRole.UserRoleId);
                            }

                            // Execute the query and get the number of rows affected
                            newUserRole.UserRoleId = Convert.ToInt32(command.ExecuteScalar());
                        }

                        // Commit the transaction
                        transaction.Commit();
                        return newUserRole.UserRoleId;
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
        public int AddUser(UserModel newUser)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int result = 0;
                        string insertQuery = "";
                        // Assuming "User" is the name of your database table
                        if (!string.IsNullOrEmpty(newUser.UserId))
                        {
                            insertQuery = @"UPDATE [Users] 
                                    SET Username = @Username,
                                        Password = @Password,
                                        DeviceId = @DeviceId,
                                        UserRoleId = @UserRoleId,
                                        RateType = @RateType,
                                        Rates = @Rates,
                                        Active = @Active,
                                        EmailId = @EmailId,
                                        MobileNumber = @MobileNumber,
                                        Address = @Address,
                                        FirstName = @FirstName
                                    WHERE UserId = @UserId;
                                    SELECT @@ROWCOUNT;";
                        }
                        else
                        {
                            insertQuery = @"INSERT INTO [Users] (UserId,Username, Password, DeviceId, UserRoleId, RateType, Rates, Active, EmailId, MobileNumber, Address,FirstName)
                                   VALUES (@UserId,@Username, @Password, @DeviceId, @UserRoleId, @RateType, @Rates, @Active, @EmailId, @MobileNumber, @Address,@FirstName);
                                   SELECT SCOPE_IDENTITY();"; // Assuming UserId is an identity column
                        }

                        using (SqlCommand command = new SqlCommand(insertQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@Username", newUser.Username);
                            command.Parameters.AddWithValue("@Password", newUser.Password);
                            command.Parameters.AddWithValue("@FirstName", newUser.FirstName);
                            command.Parameters.AddWithValue("@DeviceId", newUser.DeviceId);
                            command.Parameters.AddWithValue("@UserRoleId", newUser.UserRoleId);
                            command.Parameters.AddWithValue("@RateType", newUser.RateType);
                            command.Parameters.AddWithValue("@Rates", newUser.Rates ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@Active", 1);
                            command.Parameters.AddWithValue("@EmailId", newUser.EmailId);
                            command.Parameters.AddWithValue("@MobileNumber", newUser.MobileNumber);
                            command.Parameters.AddWithValue("@Address", newUser.Address);
                            if (newUser.UserId != null)
                            {
                                // If not null, add the UserId as a parameter
                                command.Parameters.AddWithValue("@UserId", newUser.UserId);
                            }
                            else
                            {
                                // If null, generate a random number and add it with a string
                                Random random = new Random();
                                int randomNumber = random.Next(1000); // You can adjust the range as needed
                                string randomUserId = "GeneratedUser_" + randomNumber;

                                command.Parameters.AddWithValue("@UserId", randomUserId);
                            }
                            command.ExecuteNonQuery();
                            // Execute the query and get the number of rows affected
                            result = 1;
                        }

                        // Commit the transaction
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an exception
                        transaction.Rollback();
                        throw new Exception($"Failed to add/update user. {ex.Message}");
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
                        double daycount = 1;
                        foreach (var data in excelData)
                        {
                            for (int i = 0; i < data.SupportPersons.Count; i++)
                            {
                                var OOSPerson = data.SupportPersons[i];
                                if (i != 14)
                                {
                                    // mark as half day except holiday
                                    if((i== 2 || i== 4) && data.holiday!=1)
                                    {
                                        daycount = 0.5;
                                    }
                                    using (SqlCommand command = new SqlCommand("OOSExcelUpload", connection, transaction))
                                    {
                                        command.CommandType = CommandType.StoredProcedure;

                                        // Assuming your stored procedure parameters are named @ScheduleDate and @OOSPerson
                                        command.Parameters.AddWithValue("@ScheduleDate", data.Date);
                                        command.Parameters.AddWithValue("@OOSPerson", OOSPerson != null ? OOSPerson : "");
                                        command.Parameters.AddWithValue("@holiday", data.holiday);
                                        command.Parameters.AddWithValue("@daycount", daycount);
                                        if (i == 13)
                                        {
                                            command.Parameters.AddWithValue("@Interface", 1);
                                        }
                                        else
                                        {
                                            command.Parameters.AddWithValue("@Interface", 0);
                                        }


                                        // ExecuteNonQuery returns the number of rows affected, which may not be needed
                                        // If you need to capture the InsertedId, you can use ExecuteScalar instead
                                        // Note: Make sure to cast the result appropriately (e.g., int)
                                        result += Convert.ToInt32(command.ExecuteScalar());
                                    }
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
        public List<SupportData> GetSupportData(DateTime? fromDate, DateTime? toDate,string UserId=null)
        {
            List<SupportData> result = new List<SupportData>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetSupportPersonsByDate", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserId);
                    command.Parameters.AddWithValue("@fromDate", fromDate);
                    command.Parameters.AddWithValue("@toDate", toDate);

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
        public DataTable GetUserSupportData()
        {
            DataTable result = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetUserSupportData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Create columns in the DataTable based on the SqlDataReader schema
                        result.Load(reader);

                        // If you need to handle DBNull values, you can use the following loop
                        foreach (DataColumn column in result.Columns)
                        {
                            if (column.DataType == typeof(string))
                            {
                                foreach (DataRow row in result.Rows)
                                {
                                    if (row.IsNull(column))
                                    {
                                        row[column] = string.Empty;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
        public List<AccountSupportData> GetAccountSupportData(DateTime? fromDate,DateTime? toDate,string UserId=null)
        {
            List<AccountSupportData> result = new List<AccountSupportData>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SelectAccountData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserId);
                    command.Parameters.AddWithValue("@fromDate", fromDate);
                    command.Parameters.AddWithValue("@toDate", toDate);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AccountSupportData AccountSupportData = new AccountSupportData
                            {
                                OOSPerson = reader["OOSPerson"] != DBNull.Value ? reader["OOSPerson"].ToString() : null,
                                UserRole = reader["UserRole"] != DBNull.Value ? reader["UserRole"].ToString() : null,
                                Days = reader["Days"] != DBNull.Value ? reader["Days"].ToString() : null,
                                TotalAmount = reader["TotalAmount"] != DBNull.Value ? reader["TotalAmount"].ToString() : null
                            };

                            result.Add(AccountSupportData);
                        }
                    }
                }
            }

            return result;
        }
        public List<UserAccountData> GetUserAccountData(DateTime? fromDate, DateTime? toDate, string UserId = null)
        {
            List<UserAccountData> result = new List<UserAccountData>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SelectUserAccountData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserId);
                    command.Parameters.AddWithValue("@fromDate", fromDate);
                    command.Parameters.AddWithValue("@toDate", toDate);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserAccountData UserAccountData = new UserAccountData
                            {
                                ScheduleDate = reader["ScheduleDate"] != DBNull.Value ? reader["ScheduleDate"].ToString() : null,
                                OOSPerson = reader["OOSPerson"] != DBNull.Value ? reader["OOSPerson"].ToString() : null,
                                UserRole = reader["UserRole"] != DBNull.Value ? reader["UserRole"].ToString() : null,
                                Day = reader["Days"] != DBNull.Value ? reader["Days"].ToString() : null,
                                Amount= reader["Amount"] != DBNull.Value ? reader["Amount"].ToString() : null,
                                MobileNumber = reader["MobileNumber"] != DBNull.Value ? reader["MobileNumber"].ToString() : null
                            };

                            result.Add(UserAccountData);
                        }
                    }
                }
            }

            return result;
        }

    }
}