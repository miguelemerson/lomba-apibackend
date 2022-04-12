using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Lomba.API.Services;
using Lomba.API.Models;
using Lomba.API.ViewModels;

namespace Lomba.API.Controllers
{
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [ApiController]
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
    }
}
