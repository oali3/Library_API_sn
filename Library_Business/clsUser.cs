using Library_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Business
{
    public class clsUser
    {
        public enum enMode { AddNew, Update };
        public enMode Mode;

        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public bool IsAdmin { get; set; }
        public UserDTO DTO { get { return new UserDTO(UserID, PersonID, UserName, Password, IsAdmin); } }

        public clsUser(UserDTO dto, enMode mode = enMode.AddNew)
        {
            UserID = dto.UserID;
            PersonID = dto.PersonID;
            UserName = dto.UserName;
            Password = dto.Password;
            IsAdmin = dto.IsAdmin;

            Mode = mode;
        }


        public static List<UserDTO> GetAllUsers()
        {
            return clsUsersData.GetAllUsers();
        }

        public static clsUser? Find(int UserID)
        {
            UserDTO? Dto = clsUsersData.GetUserById(UserID);
            if (Dto != null)
                return new clsUser(Dto, enMode.Update);
            else return null;
        }
        public static clsUser? FindByUserName(string UserName)
        {
            UserDTO? Dto = clsUsersData.GetUserByUserName(UserName);
            if (Dto != null)
                return new clsUser(Dto, enMode.Update);
            else return null;
        }
        private bool _AddNewUser()
        {
            UserID = clsUsersData.AddNewUser(DTO);
            return UserID != -1;
        }
        private bool _UpdateUser()
        {
            return clsUsersData.UpdateUser(DTO);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    if (_UpdateUser())
                        return true;
                    else
                        return false;
            }
            return false;
        }

        public static bool IsUserExist(int UserID)
        {
            return clsUsersData.IsUserExist(UserID);
        }
        public static bool IsUserExistByUserName(string UserName)
        {
            return clsUsersData.IsUserExistByUserName(UserName);
        }

        public static bool DeleteUser(int UserID)
        {
            return clsUsersData.DeleteUser(UserID);
        }

    }




}
