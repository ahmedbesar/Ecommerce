using Common.Authentication.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BaseApiController : ControllerBase {}
}
