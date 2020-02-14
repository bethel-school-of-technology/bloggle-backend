using Microsoft.AspNetCore.Mvc;

namespace collaby_backend.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error() => Problem();
    }
}