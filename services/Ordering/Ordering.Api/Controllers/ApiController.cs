using Microsoft.AspNetCore.Mvc;

namespace Ordering.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase
    {
    }
}
