using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Lomba.API.Services;
using Lomba.API.Models;
using Lomba.API.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Lomba.API.Controllers
{
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        #region Get Role by Id
        /// <summary>
        /// Obtiene un role a partir del Id (GUID) del role.
        /// </summary>
        /// <remarks>
        /// Ejemplo: GET /name
        /// name: basic
        /// </remarks>
        /// <param name="name">string</param>
        /// <returns>Un único role</returns>
        /// <response code="200">Retorna el role</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Role no encontrado</response>  
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(Role), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(string name)
        {
            try
            {
                var role = await this._roleService.GetRoleByNameAsync(name);
                if (role == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "Role no encontrado" });
                }
                return Ok(role);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Error en la solicitud" });
            }
        }
        #endregion

        #region Get Roles
        /// <summary>
        /// Obtiene una lista de roles
        /// </summary>
        /// <returns>Una lista de roles</returns>
        /// <response code="200">Retorna roles</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Roles no encontrados</response>  
        [HttpGet()]
        [ProducesResponseType(typeof(List<Role>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var roles = await this._roleService.GetRolesAsync();
                if (roles == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "No hay roles" });
                }
                return Ok(roles);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Error en la solicitud" });
            }
        }
        #endregion

        #region Enable Role
        /// <summary>
        /// Habilita el role
        /// </summary>
        /// <remarks>
        /// Ejemplo: PUT /name
        /// name: basic
        /// </remarks>
        /// <param name="name">string</param>
        /// <returns>Role deshabilitado</returns>
        /// <response code="200">Retorna el role</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Role no encontrado</response>  
        [HttpPut("enable")]
        [ProducesResponseType(typeof(Role), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Default.Roles.Role_Name_SuperAdmin)]
        public async Task<IActionResult> EnableByIdAsync(string name)
        {
            try
            {
                var role = await this._roleService.SetEnableAsync(name);
                if (role == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "Role no encontrado" });
                }
                return Ok(role);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Error en la solicitud" });
            }
        }
        #endregion

        #region Disable Role
        /// <summary>
        /// Deshabilita el role
        /// </summary>
        /// <remarks>
        /// Ejemplo: PUT /name
        /// name: basic
        /// </remarks>
        /// <param name="name">string</param>
        /// <returns>Role habilitado</returns>
        /// <response code="200">Retorna el role</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Role no encontrado</response>  
        [HttpPut("disable")]
        [ProducesResponseType(typeof(Role), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Default.Roles.Role_Name_SuperAdmin)]
        public async Task<IActionResult> DisableByIdAsync(string name)
        {
            try
            {
                var role = await this._roleService.SetEnableAsync(name, true);
                if (role == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "Role no encontrado" });
                }
                return Ok(role);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Error en la solicitud" });
            }
        }
        #endregion
    }
}
