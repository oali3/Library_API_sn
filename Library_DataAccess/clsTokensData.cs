using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_DataAccess
{
    public class TokenDTO
    {
        public TokenDTO(int userid, string? token)
        {
            UserId = userid;
            Token = token;
        }

        public int UserId { get; set; }
        public string? Token { get; set; }
    }
    public class clsTokensData
    {
        public static TokenDTO? GetTokenById(int UserId)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Tokens_GetTokenByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", UserId);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TokenDTO(
                        reader.GetInt32("UserId"),
                        reader.GetString("Token"));
                    }
                }
            }
            return null;
        }
        public static TokenDTO? GetTokenByToken(string Token)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Tokens_GetTokenByToken", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Token", Token);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TokenDTO(
                        reader.GetInt32("UserId"),
                        reader.GetString("Token"));
                    }
                }
            }
            return null;
        }
        public static bool AddNewToken(TokenDTO dto)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Tokens_AddNewToken", connection))
            {
                command.CommandType = CommandType.StoredProcedure; command.Parameters.AddWithValue("@UserId", dto.UserId);
                command.CommandType = CommandType.StoredProcedure; command.Parameters.AddWithValue("@Token", dto.Token);

                connection.Open();
                int Affected = command.ExecuteNonQuery();

                return Affected != 0;
            }
        }
        public static bool UpdateToken(TokenDTO dto)
        {
            int AffectedRows = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Tokens_UpdateToken", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", dto.UserId);
                command.Parameters.AddWithValue("@Token", dto.Token);

                connection.Open();
                AffectedRows = command.ExecuteNonQuery();

            }
            return AffectedRows != 0;
        }

    }


}
