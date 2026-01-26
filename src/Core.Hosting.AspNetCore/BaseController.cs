using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Hosting.AspNetCore
{
    /// <summary>
    /// Base controller to include authorization and versioning
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Authorize]
    [ApiVersion(1)]
    public abstract class BaseController : ControllerBase { }
}
