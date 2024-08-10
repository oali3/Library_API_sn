using Library_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Business
{
    public class clsToken
    {
        public enum enMode { AddNew, Update };
        public enMode Mode;

        public int UserId { get; set; }
        public string? Token { get; set; }
        public TokenDTO DTO { get { return new TokenDTO(UserId, Token); } }

        public clsToken(TokenDTO dto, enMode mode = enMode.AddNew)
        {
            UserId = dto.UserId;
            Token = dto.Token;

            Mode = mode;
        }

        public static clsToken? Find(int UserId)
        {
            TokenDTO? Dto = clsTokensData.GetTokenById(UserId);
            if (Dto != null)
                return new clsToken(Dto, enMode.Update);
            else return null;
        }
        public static clsToken? FindByToken(string Token)
        {
            TokenDTO? Dto = clsTokensData.GetTokenByToken(Token);
            if (Dto != null)
                return new clsToken(Dto, enMode.Update);
            else return null;
        }
        private bool _AddNewToken()
        {
            return clsTokensData.AddNewToken(DTO);    
        }
        private bool _UpdateToken()
        {
            return clsTokensData.UpdateToken(DTO);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewToken())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    if (_UpdateToken())
                        return true;
                    else
                        return false;
            }
            return false;
        }

    }

}
