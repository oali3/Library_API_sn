using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_DataAccess
{
    public class UserDTO
    {
        public UserDTO(int userid, int personid, string? username, string? password, bool isadmin)
        {
            UserID = userid;
            PersonID = personid;
            UserName = username;
            Password = password;
            IsAdmin = isadmin;
        }

        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public bool IsAdmin { get; set; }
    }
    public class clsUsersData
    {
        public static List<UserDTO> GetAllUsers()
        {
            List<UserDTO> list = new List<UserDTO>();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Users_GetAllUsers", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new UserDTO(
                        reader.GetInt32("UserID"),
                        reader.GetInt32("PersonID"),
                        reader.GetString("UserName"),
                        reader.GetString("Password"),
                        reader.GetBoolean("IsAdmin")));
                    }
                }
            }
            return list;
        }
        public static UserDTO? GetUserById(int UserID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Users_GetUserByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", UserID);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserDTO(
                        reader.GetInt32("UserID"),
                        reader.GetInt32("PersonID"),
                        reader.GetString("UserName"),
                        reader.GetString("Password"),
                        reader.GetBoolean("IsAdmin"));
                    }
                }
            }
            return null;
        }
        public static UserDTO? GetUserByUserName(string UserName)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Users_GetUserByUserName", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserName", UserName);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserDTO(
                        reader.GetInt32("UserID"),
                        reader.GetInt32("PersonID"),
                        reader.GetString("UserName"),
                        reader.GetString("Password"),
                        reader.GetBoolean("IsAdmin"));
                    }
                }
            }
            return null;
        }
        public static int AddNewUser(UserDTO dto)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Users_AddNewUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure; command.Parameters.AddWithValue("@PersonID", dto.PersonID);
                command.Parameters.AddWithValue("@UserName", dto.UserName);
                command.Parameters.AddWithValue("@Password", dto.Password);
                command.Parameters.AddWithValue("@IsAdmin", dto.IsAdmin);

                SqlParameter OutParam = new SqlParameter("@UserID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(OutParam);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)OutParam.Value;
            }
        }
        public static bool UpdateUser(UserDTO dto)
        {
            int AffectedRows = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Users_UpdateUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", dto.UserID);
                command.Parameters.AddWithValue("@PersonID", dto.PersonID);
                command.Parameters.AddWithValue("@UserName", dto.UserName);
                command.Parameters.AddWithValue("@Password", dto.Password);
                command.Parameters.AddWithValue("@IsAdmin", dto.IsAdmin);

                connection.Open();
                AffectedRows = command.ExecuteNonQuery();

            }
            return AffectedRows != 0;
        }
        public static bool IsUserExist(int UserID)
        {
            int IsFound = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Users_IsUserExist", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", UserID);

                SqlParameter OutParam = new SqlParameter("@Out", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(OutParam);

                connection.Open();
                command.ExecuteNonQuery();
                IsFound = (int)OutParam.Value;
            }
            return IsFound != 0;
        }
        public static bool IsUserExistByUserName(string UserName)
        {
            int IsFound = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Users_IsUserExistByUserName", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserName", UserName);

                SqlParameter OutParam = new SqlParameter("@Out", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(OutParam);

                connection.Open();
                command.ExecuteNonQuery();
                IsFound = (int)OutParam.Value;
            }
            return IsFound != 0;
        }
        public static bool DeleteUser(int? UserID)
        {
            if (UserID == null)
                return false;
            int AffectedRows = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SP_Users_DeleteUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserID", UserID);

                    AffectedRows = command.ExecuteNonQuery();
                }
            }
            return AffectedRows != 0;
        }
    }


}
