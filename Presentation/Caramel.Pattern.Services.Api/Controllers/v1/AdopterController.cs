using AutoMapper;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Request;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Request.Adopters;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Responses;
using Caramel.Pattern.Services.Domain.Entities.Models.Adopters;
using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Services.Adopters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Caramel.Pattern.Services.Api.Controllers.v1
{
    [ApiController]
    [Route("v1/[controller]")]
    public class AdopterController : BaseController
    {
        private readonly ILogger<AdopterController> _logger;
        private readonly IAdopterService _service;
        private readonly IMapper _mapper;

        public AdopterController(
            ILogger<AdopterController> logger,
            IAdopterService service,
            IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Recupera uma lista de todos os Usu�rios.
        /// </summary>
        /// <param name="page">P�gina de dados a serem trazidos. Default: Page = 1.</param>
        /// <param name="size">Tamanho da p�gina de dados a serem trazidos. Default: Size = 10.</param>
        /// <returns>Lista de Usu�rios, Status do Processo e Descri��o</returns>
        [Authorize]
        [HttpGet("/users-control/adopters")]
        [ProducesResponseType(typeof(CustomResponse<IEnumerable<Adopter>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAdoptersAsync(int page, int size)
        {
            Pagination pagination = new(page, size);

            var adopters = await _service.GetAllAsync();

            var paginatedAdopters = ReturnPaginated(adopters, pagination);

            var response = new CustomResponse<IEnumerable<Adopter>>(paginatedAdopters, StatusProcess.Success);

            return Ok(response);
        }

        /// <summary>
        /// Recupera um usu�rio espec�fico por ID.
        /// </summary>
        /// <param name="id">O ID do usu�rio a ser recuperado.</param>
        /// <returns>Usu�rio, Status do Processo e Descri��o</returns>
        [Authorize]
        [HttpGet("/users-control/adopter")]
        [ProducesResponseType(typeof(CustomResponse<Adopter>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAdopterAsync(string id)
        {
            var adopter = await _service.GetSingleOrDefaultByIdAsync(id);

            var response = new CustomResponse<Adopter>(adopter, StatusProcess.Success);

            return Ok(response);
        }

        /// <summary>
        /// Recupera um usu�rio espec�fico por email.
        /// </summary>
        /// <param name="email">O ID do usu�rio a ser recuperado.</param>
        /// <returns>Usu�rio, Status do Processo e Descri��o</returns>
        [Authorize]
        [HttpGet("/users-control/adopter/email")]
        [ProducesResponseType(typeof(CustomResponse<Adopter>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAdopterByEmailAsync(string email)
        {
            var adopter = await _service.GetSingleOrDefaultByEmailAsync(email);

            var response = new CustomResponse<Adopter>(adopter, StatusProcess.Success);

            return Ok(response);
        }

        /// <summary>
        /// Cria um novo usu�rio.
        /// </summary>
        /// <param name="request">Dados do novo Usu�rio.</param>
        /// <returns>Usu�rio Criado, Status do Processo e Descri��o</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("/users-control/adopters")]
        [ProducesResponseType(typeof(CustomResponse<Adopter>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterAdopterAsync([FromBody] AdopterRegistrationRequest request)
        {
            var adopter = _mapper.Map<Adopter>(request);

            var addedAdopter = await _service.RegisterAsync(adopter);

            var response = new CustomResponse<Adopter>(addedAdopter, StatusProcess.Success);

            var uri = Url.Action("GetAdopter", "AdopterController", new { id = addedAdopter.Id });

            return Created(uri, response);
        }

        /// <summary>
        /// Atualiza um usu�rio existente.
        /// </summary>
        /// <param name = "id" > O ID do usu�rio a ser Atualizado.</param>
        /// <param name = "request" > Dados Atualizados do Usu�rio.</param>
        /// <returns>Usu�rio Atualizado, Status do Processo e Descri��o</returns>
        [Authorize]
        [HttpPut("/users-control/adopters")]
        [ProducesResponseType(typeof(CustomResponse<Adopter>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAdopterAsync(string id, AdopterUpdateRequest request)
        {
            var adopter = _mapper.Map<Adopter>(request);
            adopter.Id = id;

            var updatedAdopter = await _service.UpdateAsync(adopter);

            var response = new CustomResponse<Adopter>(updatedAdopter, StatusProcess.Success);

            return Ok(response);
        }

        /// <summary>
        /// Atualiza a senha de um usu�rio existente.
        /// </summary>
        /// <param name = "id" > O ID do usu�rio a ser Atualizado.</param>
        /// <param name = "request" > Dados Atualizados do Usu�rio.</param>
        /// <returns>Usu�rio Atualizado, Status do Processo e Descri��o</returns>
        [Authorize]
        [HttpPut("/users-control/adopters/password")]
        [ProducesResponseType(typeof(CustomResponse<Adopter>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAdopterPasswordAsync(string id, AdopterUpdatePasswordRequest request)
        {
            var adopter = _mapper.Map<Adopter>(request);
            adopter.Id = id;

            var updatedAdopter = await _service.UpdatePasswordAsync(adopter);

            var response = new CustomResponse<Adopter>(updatedAdopter, StatusProcess.Success);

            return Ok(response);
        }

        /// <summary>
        /// Exclui um usu�rio por ID.
        /// </summary>
        /// <param name="adopterId">O ID do usu�rio a ser exclu�do.</param>
        /// <returns>Usu�rio Deletado, Status do Processo e Descri��o</returns>
        [Authorize]
        [HttpDelete("/users-control/adopters")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAdopterAsync(string adopterId)
        {
            await _service.DeleteAsync(adopterId);

            return NoContent();
        }

        /// <summary>
        /// Atualiza a senha de um usu�rio existente.
        /// </summary>
        /// <param name = "id" > O ID do usu�rio a ser Atualizado.</param>
        /// <param name = "request" > Dados Atualizados do Usu�rio.</param>
        /// <returns>Usu�rio Atualizado, Status do Processo e Descri��o</returns>
        [Authorize]
        [HttpPut("/users-control/adopters/profile-image")]
        [ProducesResponseType(typeof(CustomResponse<Adopter>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAdopterPrfileImageAsync(string id, AdopterUpdateProfileImageRequest request)
        {
            var updatedAdopter = await _service.ProfileImageUpdateAsync(id, request.Base64Image);

            var response = new CustomResponse<Adopter>(updatedAdopter, StatusProcess.Success);

            return Ok(response);
        }
    }
}
