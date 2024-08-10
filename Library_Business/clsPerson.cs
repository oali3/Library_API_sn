using Library_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Business
{
    public class clsPerson
    {
        public enum enMode { AddNew, Update };
        public enMode Mode;

        public int PersonID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Gendor { get; set; }
        public string? Address { get; set; }
        public string? PersonalPicture { get; set; }
        public PersonDTO DTO { get { return new PersonDTO(PersonID, Name, Email, Phone, DateOfBirth, Gendor, Address, PersonalPicture); } }

        public clsPerson(PersonDTO dto, enMode mode = enMode.AddNew)
        {
            PersonID = dto.PersonID;
            Name = dto.Name;
            Email = dto.Email;
            Phone = dto.Phone;
            DateOfBirth = dto.DateOfBirth;
            Gendor = dto.Gendor;
            Address = dto.Address;
            PersonalPicture = dto.PersonalPicture;

            Mode = mode;
        }


        public static List<PersonDTO> GetAllPeople()
        {
            return clsPeopleData.GetAllPeople();
        }

        public static clsPerson? Find(int PersonID)
        {
            PersonDTO? Dto = clsPeopleData.GetPersonById(PersonID);
            if (Dto != null)
                return new clsPerson(Dto, enMode.Update);
            else return null;
        }
        private bool _AddNewPerson()
        {
            PersonID = clsPeopleData.AddNewPerson(DTO);
            return PersonID != -1;
        }
        private bool _UpdatePerson()
        {
            return clsPeopleData.UpdatePerson(DTO);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    if (_UpdatePerson())
                        return true;
                    else
                        return false;
            }
            return false;
        }

        public static bool IsPersonExist(int PersonID)
        {
            return clsPeopleData.IsPersonExist(PersonID);
        }

        public static bool DeletePerson(int PersonID)
        {
            return clsPeopleData.DeletePerson(PersonID);
        }



    }
}
