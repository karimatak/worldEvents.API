using Microsoft.AspNetCore.Mvc;

namespace WorldEvents.API.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        
        [HttpGet]
        public string sayHello(){
            return "Hello World\nGo to /api/events";
        }
        
    }
}