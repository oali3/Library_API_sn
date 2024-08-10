using Library_Business;
using Library_DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_API.Controllers
{
    [Route("api/Rigister")]
    [ApiController]
    public class RegisterController : ControllerBase
    {


        [HttpPost(Name = "RegisterUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<UserDTO>> AddNewUser(UserDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.UserName) || string.IsNullOrEmpty(dto.Password))
                return BadRequest("Missing UserName or Password");

            dto.PersonID = 8;

            if (clsUser.IsUserExistByUserName(dto.UserName))
                return BadRequest("User Name is already exist");

            clsUser? user = new clsUser(dto);

            user.Save();

            string? tk = "Qhy4jh4un6mp6ka3";

            TokenDTO Tdto = new TokenDTO(user.UserID, tk);

            clsToken Token = new clsToken(Tdto);

            Token.Save();

            HttpContext.Response.Headers.Append("Authorization", $"Bearer {Token.Token}");

            return CreatedAtRoute("RegisterUser", new { id = user.DTO.UserID, token = Token.Token });
        }



    }
}
