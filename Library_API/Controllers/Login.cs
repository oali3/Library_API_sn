using Library_Business;
using Library_DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Library_API.Controllers
{
    [Route("api/Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        [HttpPost(Name = "Login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<UserDTO>> Login(UserDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.UserName) || string.IsNullOrEmpty(dto.Password))
                return BadRequest("Missing UserName or Password");

            clsUser? user = clsUser.FindByUserName(dto.UserName);

            if (user == null)
                return NotFound("User Not Found");

            if (user?.Password != dto.Password)
                return BadRequest("Password is incorrect");

            clsToken Token = clsToken.Find(user.UserID);

            HttpContext.Response.Headers.Append("Authorization", $"Bearer {Token.Token}");

            return CreatedAtRoute("RegisterUser", new { token = Token.Token });
        }



    }


}
