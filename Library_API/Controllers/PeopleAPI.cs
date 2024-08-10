using Library_Business;
using Library_DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_API.Controllers
{
    [Route("api/Person")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        [HttpGet(Name = "GetAllPerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<PersonDTO>> GetAllPeople()
        {
            List<PersonDTO> list = clsPerson.GetAllPeople(); if (list.Count == 0)
                return NotFound("No Person Found");
            return Ok(list);
        }
        [HttpGet("{id}", Name = "GetPersonById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<PersonDTO>> GetPersonById(int id)
        {
            if (id < 1)
                return BadRequest("Not valid ID");

            clsPerson? person = clsPerson.Find(id);

            if (person == null)
                return NotFound($"Person with Id({id}) Not Found");

            return Ok(person.DTO);
        }



        [HttpPost(Name = "AddPerson")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<PersonDTO>> AddNewPerson(PersonDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Phone) || string.IsNullOrEmpty(dto.Address) || string.IsNullOrEmpty(dto.PersonalPicture))
                return BadRequest("Invalid Person data");

            clsPerson? person = new clsPerson(dto);

            person.Save();

            return CreatedAtRoute("AddPerson", person.DTO);
        }

        [HttpPut("{id}", Name = "UpdatePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<PersonDTO>> UpdatePerson(int id, PersonDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Phone) || string.IsNullOrEmpty(dto.Address) || string.IsNullOrEmpty(dto.PersonalPicture))
                return BadRequest("Invalid Person data");

            clsPerson? person = clsPerson.Find(id);


            if (person == null)
                return NotFound($"Person with Id({id}) Not Found");

            person.Name = dto.Name;
            person.Email = dto.Email;
            person.Phone = dto.Phone;
            person.DateOfBirth = dto.DateOfBirth;
            person.Gendor = dto.Gendor;
            person.Address = dto.Address;
            person.PersonalPicture = dto.PersonalPicture;

            person.Save();

            return Ok("Person Updated Successfully");
        }



        [HttpDelete("{id}", Name = "DeletePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<PersonDTO>> DeletePerson(int id)
        {
            if (id < 1)
                return BadRequest("Invalid Id");

            if (!clsPerson.IsPersonExist(id))
                return NotFound($"Person with Id({id}) not Found");

            clsPerson.DeletePerson(id);

            return Ok("Person Deleted Successfully");
        }

    }



}
