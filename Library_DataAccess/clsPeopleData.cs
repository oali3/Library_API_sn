using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_DataAccess
{

    public class PersonDTO
    {
        public PersonDTO(int personid, string? name, string? email, string? phone, DateTime dateofbirth, bool gendor, string? address, string? personalpicture)
        {
            PersonID = personid;
            Name = name;
            Email = email;
            Phone = phone;
            DateOfBirth = dateofbirth;
            Gendor = gendor;
            Address = address;
            PersonalPicture = personalpicture;
        }

        public int PersonID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Gendor { get; set; }
        public string? Address { get; set; }
        public string? PersonalPicture { get; set; }
    }
    public class clsPeopleData
    {
        public static List<PersonDTO> GetAllPeople()
        {
            List<PersonDTO> list = new List<PersonDTO>();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_People_GetAllPeople", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string? s2 = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email");

                        list.Add(new PersonDTO(
                        reader.GetInt32("PersonID"),
                        reader.GetString("Name"),
                        s2,
                        reader.GetString("Phone"),
                        reader.GetDateTime("DateOfBirth"),
                        reader.GetBoolean("Gendor"),
                        reader.GetString("Address"),
                        reader.GetString("PersonalPicture")));
                    }
                }
            }
            return list;
        }
        public static PersonDTO? GetPersonById(int PersonID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_People_GetPersonByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PersonID", PersonID);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string? s2 = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email");

                        return new PersonDTO(
                        reader.GetInt32("PersonID"),
                        reader.GetString("Name"),
                        s2,
                        reader.GetString("Phone"),
                        reader.GetDateTime("DateOfBirth"),
                        reader.GetBoolean("Gendor"),
                        reader.GetString("Address"),
                        reader.GetString("PersonalPicture"));
                    }
                }
            }
            return null;
        }
        public static int AddNewPerson(PersonDTO dto)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_People_AddNewPerson", connection))
            {
                command.CommandType = CommandType.StoredProcedure; command.Parameters.AddWithValue("@Name", dto.Name);
                command.Parameters.AddWithValue("@Email", (string.IsNullOrEmpty(dto.Email) ? DBNull.Value : dto.Email));
                command.Parameters.AddWithValue("@Phone", dto.Phone);
                command.Parameters.AddWithValue("@DateOfBirth", dto.DateOfBirth);
                command.Parameters.AddWithValue("@Gendor", dto.Gendor);
                command.Parameters.AddWithValue("@Address", dto.Address);
                command.Parameters.AddWithValue("@PersonalPicture", dto.PersonalPicture);

                SqlParameter OutParam = new SqlParameter("@PersonID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(OutParam);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)OutParam.Value;
            }
        }
        public static bool UpdatePerson(PersonDTO dto)
        {
            int AffectedRows = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_People_UpdatePerson", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PersonID", dto.PersonID);
                command.Parameters.AddWithValue("@Name", dto.Name);
                command.Parameters.AddWithValue("@Email", (string.IsNullOrEmpty(dto.Email) ? DBNull.Value : dto.Email));
                command.Parameters.AddWithValue("@Phone", dto.Phone);
                command.Parameters.AddWithValue("@DateOfBirth", dto.DateOfBirth);
                command.Parameters.AddWithValue("@Gendor", dto.Gendor);
                command.Parameters.AddWithValue("@Address", dto.Address);
                command.Parameters.AddWithValue("@PersonalPicture", dto.PersonalPicture);

                connection.Open();
                AffectedRows = command.ExecuteNonQuery();

            }
            return AffectedRows != 0;
        }
        public static bool IsPersonExist(int PersonID)
        {
            int IsFound = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_People_IsPersonExist", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PersonID", PersonID);

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
        public static bool DeletePerson(int? PersonID)
        {
            if (PersonID == null)
                return false;
            int AffectedRows = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SP_People_DeletePerson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    AffectedRows = command.ExecuteNonQuery();
                }
            }
            return AffectedRows != 0;
        }
    }




}
