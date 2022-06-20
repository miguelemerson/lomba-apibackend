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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #region Get User by Id
        /// <summary>
        /// Obtiene un usuario a partir del Id (GUID) del usuario.
        /// </summary>
        /// <remarks>
        /// Ejemplo: GET /Id
        /// Id: ECD0F9C4-32A2-48D5-832D-0230F4CB4A3F
        /// </remarks>
        /// <param name="Id">GUID</param>
        /// <returns>Un único usuario</returns>
        /// <response code="200">Retorna el usuario</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Usuario no encontrado</response>  
        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(string Id)
        {
            try
            {
                var user = await this._userService.GetUserByIdAsync(Guid.Parse(Id));
                if (user == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "Usuario no encontrado" });
                }
                return Ok(user);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Error en la solicitud" });
            }
        }
        #endregion

        #region Get Users
        /// <summary>
        /// Obtiene una lista de usuarios
        /// </summary>
        /// <returns>Una lista de usuarios</returns>
        /// <response code="200">Retorna el usuario</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Usuario no encontrado</response>  
        [HttpGet()]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Default.Roles.Role_AdminANDSuperAdmin)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var users = await this._userService.GetUsersAsync();
                if (users == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "No hay usuarios" });
                }
                return Ok(users);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Error en la solicitud" });
            }
        }
        #endregion

        #region Get Users with Orga count
        /// <summary>
        /// Obtiene una lista de usuarios
        /// </summary>
        /// <returns>Una lista de usuarios con cantidad de Orgas</returns>
        /// <response code="200">Retorna el usuario con cantidad de Orgas</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Usuario no encontrado</response>  
        [HttpGet("orgacount")]
        [ProducesResponseType(typeof(List<UserItemAll>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Default.Roles.Role_AdminANDSuperAdmin)]
        public async Task<IActionResult> GetWithOrgaCountAsync()
        {
            try
            {
                var users = await this._userService.GetUsersWithOrgaCountAsync();
                if (users == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "No hay usuarios" });
                }
                return Ok(users);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Error en la solicitud" });
            }
        }
        #endregion

        #region Authenticate Users
        /// <summary>
        /// Autenticación de usuario a través de su usuario y contraseña
        /// </summary>
        /// <returns>Una lista de usuarios</returns>
        /// <response code="200">Retorna el usuario</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Usuario no encontrado</response>  
        [HttpPost("authenticate")]
        [ProducesResponseType(typeof(UserLogged), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAuthenticateAsync(UserAuth userAuth)
        {
            try
            {
                var userLogged = await this._userService.AuthenticateAsync(userAuth);
                if (userLogged == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "Usuario no autenticado." });
                }
                return Ok(userLogged);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Usuario no autenticado." });
            }
        }
        #endregion

        #region Get Orgas by User
        /// <summary>
        /// Obtiene un listado de Orgas a partir del Id (GUID) del usuario.
        /// </summary>
        /// <remarks>
        /// Ejemplo: GET /Id/orgas
        /// Id: ECD0F9C4-32A2-48D5-832D-0230F4CB4A3F
        /// </remarks>
        /// <param name="Id">GUID</param>
        /// <returns>Un único usuario</returns>
        /// <response code="200">Retorna el listado de orgas</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Orgas no encontradas</response>  
        [HttpGet("{Id}/orgas")]
        [ProducesResponseType(typeof(List<OrgaUser>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrgasByIdAsync(string Id)
        {
            try
            {
                var orgasuser = await this._userService.GetOrgasByUserIdAsync(Guid.Parse(Id));
                if (orgasuser == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "Organizaciones no encontradas" });
                }
                return Ok(orgasuser);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Error en la solicitud" });
            }
        }
        #endregion

        #region Enable User
        /// <summary>
        /// Habilita al usuario
        /// </summary>
        /// <remarks>
        /// Ejemplo: PUT /Id
        /// Id: 01000003-0000-0000-0000-00000000000B
        /// </remarks>
        /// <param name="Id">string</param>
        /// <returns>Usuario habilitado</returns>
        /// <response code="200">Retorna al usuario habilitado</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Usuario no encontrado</response>  
        [HttpPut("enable")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Default.Roles.Role_Name_SuperAdmin)]
        public async Task<IActionResult> EnableByIdAsync(string Id)
        {
            try
            {
                var user = await this._userService.SetEnableAsync(Guid.Parse(Id));
                if (user == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "Usuario no encontrado" });
                }
                return Ok(user);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Error en la solicitud" });
            }
        }
        #endregion

        #region Disable User
        /// <summary>
        /// Deshabilita al usuario
        /// </summary>
        /// <remarks>
        /// Ejemplo: PUT /Id
        /// Id: 01000003-0000-0000-0000-00000000000B
        /// </remarks>
        /// <param name="Id">string</param>
        /// <returns>Usuario deshabilitado</returns>
        /// <response code="200">Retorna al usuario deshabilitado</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Usuario no encontrado</response>  
        [HttpPut("disable")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Default.Roles.Role_Name_SuperAdmin)]
        public async Task<IActionResult> DisableByIdAsync(string Id)
        {
            try
            {
                var user = await this._userService.SetEnableAsync(Guid.Parse(Id), true);
                if (user == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "Usuario no encontrado" });
                }
                return Ok(user);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Error en la solicitud" });
            }
        }
        #endregion
    }
}
