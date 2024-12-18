using Microsoft.AspNetCore.Mvc;

namespace csharp_scalar.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SharedController : ControllerBase
    {

    }
}