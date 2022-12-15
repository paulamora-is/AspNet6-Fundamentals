using Microsoft.AspNetCore.Mvc;

namespace AspNet_Core6.Fundamentals.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult Get()
         => Ok();
    }
}
