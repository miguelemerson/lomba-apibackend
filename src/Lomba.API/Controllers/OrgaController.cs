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
    public class OrgaController : ControllerBase
    {
        private readonly IOrgaService _orgaService;

        public OrgaController(IOrgaService orgaService)
        {
            _orgaService = orgaService;
        }

        #region Get Orga by Id
        /// <summary>
        /// Obtiene un orga a partir del Id (GUID) del orga.
        /// </summary>
        /// <remarks>
        /// Ejemplo: GET /Id
        /// name: 
        /// </remarks>
        /// <param name="Id">string</param>
        /// <returns>Un único orga</returns>
        /// <response code="200">Retorna el orga</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Orga no encontrado</response>  
        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(Orga), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(string Id)
        {
            try
            {
                var orga = await this._orgaService.GetOrgaByIdAsync(Guid.Parse(Id));
                if (orga == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "Orga no encontrado" });
                }
                return Ok(orga);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Error en la solicitud" });
            }
        }
        #endregion

        #region Get Orgas
        /// <summary>
        /// Obtiene una lista de orgas
        /// </summary>
        /// <returns>Una lista de orgas</returns>
        /// <response code="200">Retorna orgas</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Orgas no encontrados</response>  
        [HttpGet()]
        [ProducesResponseType(typeof(List<Orga>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var orgas = await this._orgaService.GetOrgasAsync();
                if (orgas == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "No hay orgas" });
                }
                return Ok(orgas);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Error en la solicitud" });
            }
        }
        #endregion

        #region Enable Orga
        /// <summary>
        /// Habilita el orga
        /// </summary>
        /// <remarks>
        /// Ejemplo: PUT /Id
        /// name: 
        /// </remarks>
        /// <param name="Id">string</param>
        /// <returns>Orga deshabilitado</returns>
        /// <response code="200">Retorna el orga</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Orga no encontrado</response>  
        [HttpPut("enable")]
        [ProducesResponseType(typeof(Orga), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Default.Roles.Role_Name_SuperAdmin)]
        public async Task<IActionResult> EnableByIdAsync(string Id)
        {
            try
            {
                var orga = await this._orgaService.SetEnableAsync(Guid.Parse(Id));
                if (orga == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "Orga no encontrado" });
                }
                return Ok(orga);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Error en la solicitud" });
            }
        }
        #endregion

        #region Disable Orga
        /// <summary>
        /// Deshabilita el orga
        /// </summary>
        /// <remarks>
        /// Ejemplo: PUT /Id
        /// name: 
        /// </remarks>
        /// <param name="Id">string</param>
        /// <returns>Orga habilitado</returns>
        /// <response code="200">Retorna el orga</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Orga no encontrado</response>  
        [HttpPut("disable")]
        [ProducesResponseType(typeof(Orga), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Default.Roles.Role_Name_SuperAdmin)]
        public async Task<IActionResult> DisableByIdAsync(string Id)
        {
            try
            {
                var orga = await this._orgaService.SetEnableAsync(Guid.Parse(Id), true);
                if (orga == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "Orga no encontrado" });
                }
                return Ok(orga);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Error en la solicitud" });
            }
        }
        #endregion

        #region Get Users by Orga
        /// <summary>
        /// Obtiene un listado de Users a partir del Id (GUID) de la orga.
        /// </summary>
        /// <remarks>
        /// Ejemplo: GET /Id/users
        /// Id: 
        /// </remarks>
        /// <param name="Id">GUID</param>
        /// <returns>Un único orga</returns>
        /// <response code="200">Retorna el listado de users</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Users no encontradas</response>  
        [HttpGet("{Id}/users")]
        [ProducesResponseType(typeof(List<OrgaUser>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Default.Roles.Role_AdminANDSuperAdmin)]
        public async Task<IActionResult> GetUsersByIdAsync(string Id)
        {
            try
            {
                var orgasuser = await this._orgaService.GetUsersByOrgaIdAsync(Guid.Parse(Id));
                if (orgasuser == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "Usuarios no encontradas" });
                }
                return Ok(orgasuser);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Error en la solicitud" });
            }
        }
        #endregion

        #region Enable User Orga
        /// <summary>
        /// Habilita el orga
        /// </summary>
        /// <remarks>
        /// Ejemplo: PUT /Id
        /// name: 
        /// </remarks>
        /// <param name="Id">string</param>
        /// <returns>Orga deshabilitado</returns>
        /// <response code="200">Retorna el orga</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Orga no encontrado</response>  
        [HttpPut("{Id}/users/enable")]
        [ProducesResponseType(typeof(OrgaUser), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Default.Roles.Role_AdminANDSuperAdmin)]
        public async Task<IActionResult> EnableUserByIdAsync(string Id, string userId)
        {
            try
            {
                var orga = await this._orgaService.SetUserEnableAsync(Guid.Parse(Id), Guid.Parse(userId));
                if (orga == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "Orga no encontrado" });
                }
                return Ok(orga);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Error en la solicitud" });
            }
        }
        #endregion

        #region Disable User Orga
        /// <summary>
        /// Deshabilita el user orga
        /// </summary>
        /// <remarks>
        /// Ejemplo: PUT /Id
        /// name: 
        /// </remarks>
        /// <param name="Id">string</param>
        /// <returns>Orga habilitado</returns>
        /// <response code="200">Retorna el orga</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Orga no encontrado</response>  
        [HttpPut("{Id}/users/disable")]
        [ProducesResponseType(typeof(Orga), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Default.Roles.Role_AdminANDSuperAdmin)]
        public async Task<IActionResult> DisableUserByIdAsync(string Id, string userId)
        {
            try
            {
                var orga = await this._orgaService.SetUserEnableAsync(Guid.Parse(Id), Guid.Parse(userId), setToDisable: true);
                if (orga == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "Orga no encontrado" });
                }
                return Ok(orga);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Error en la solicitud" });
            }
        }
        #endregion

        #region Remove User Orga
        /// <summary>
        /// Deshabilita el user orga
        /// </summary>
        /// <remarks>
        /// Ejemplo: PUT /Id
        /// name: 
        /// </remarks>
        /// <param name="Id">string</param>
        /// <returns>Orga habilitado</returns>
        /// <response code="200">Retorna el orga</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Orga no encontrado</response>  
        [HttpDelete("{Id}/users")]
        [ProducesResponseType(typeof(List<OrgaUser>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Default.Roles.Role_AdminANDSuperAdmin)]
        public async Task<IActionResult> RemoveUserAsync(string Id, string userId)
        {
            try
            {
                var orgasuser = await this._orgaService.RemoveUserAsync(Guid.Parse(Id), Guid.Parse(userId));
                if (orgasuser == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "No se encontraron asocaciones" });
                }
                return Ok(orgasuser);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Error en la solicitud" });
            }
        }
        #endregion

        #region Associate User Orga
        /// <summary>
        /// Deshabilita el user orga
        /// </summary>
        /// <remarks>
        /// Ejemplo: POST /Id
        /// name: 
        /// </remarks>
        /// <param name="Id">string</param>
        /// <returns>Orga habilitado</returns>
        /// <response code="200">Retorna el orga</response>
        /// <response code="400">Error en el proceso</response>     
        /// <response code="404">Orga no encontrado</response>  
        [HttpPost("users")]
        [ProducesResponseType(typeof(OrgaUser), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [Authorize(Roles = Default.Roles.Role_AdminANDSuperAdmin)]
        public async Task<IActionResult> AssociateUserAsync([FromBody] OrgaUserInput orgaUserInput)
        {
            try
            {
                var orgasuser = await this._orgaService.AssociateOrgaUserAsync(orgaUserInput);
                if (orgasuser == null)
                {
                    return NotFound(new APIResponse() { Code = 9, Message = "No se encontraron asocaciones" });
                }
                return Ok(orgasuser);
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse() { Code = 9, Message = "Error en la solicitud" });
            }
        }
        #endregion
    }
}
