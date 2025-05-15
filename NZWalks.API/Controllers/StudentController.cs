using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudent()
        {
            string[] student = new string[] { "Pranav","Jhone","Bob"};
            return Ok(student);
        }        
    }
}
