using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Lomba.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PingController : ControllerBase
    {
        private readonly IEnumerable<EndpointDataSource> _endpointSources;
        public PingController(IEnumerable<EndpointDataSource> endpointSources)
        {
            _endpointSources = endpointSources;
        }

        [HttpGet()]
        [AllowAnonymous]
        [ProducesResponseType(typeof(DateTime), StatusCodes.Status200OK)]
        public ActionResult GetOk()
        {
            return Ok(DateTime.UtcNow);
        }

        [HttpGet("endpoints")]
        public ActionResult ListAllEndpoints()
        {
            var endpoints = _endpointSources
                .SelectMany(es => es.Endpoints)
                .OfType<RouteEndpoint>();

            var output = endpoints.Select(
                e =>
                {
                    var controller = e.Metadata
                        .OfType<ControllerActionDescriptor>()
                        .FirstOrDefault();

                    var action = controller != null
                        ? $"{controller.ControllerName}.{controller.ActionName}"
                        : null;

                    var controllerMethod = controller != null
                        ? $"{controller.ControllerTypeInfo.Name}:{controller.MethodInfo.Name}"
                        : null;

                    var parametersMethods = controller != null
                    ? controller.Parameters.Select(p => {
                        return new
                        {
                            Name = p.Name,
                            Type = p.ParameterType.FullName
                        };
                    }) : null;

                    return new
                    {
                        Method = e.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault()?.HttpMethods?[0],
                        Route = $"/{e.RoutePattern.RawText.TrimStart('/')}",
                        Action = action,
                        ControllerMethod = controllerMethod,
                        Parameters = parametersMethods
                    };
                }
            );

            return Ok(output);
        }
    }
}
