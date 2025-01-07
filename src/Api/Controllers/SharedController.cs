using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace csharp_scalar.Controllers
{
    [ApiController]
    [OutputCache]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SharedController : ControllerBase
    {
    }
}