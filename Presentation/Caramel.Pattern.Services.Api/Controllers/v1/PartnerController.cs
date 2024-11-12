using AutoMapper;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Request;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Request.Partners;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Responses;
using Caramel.Pattern.Services.Domain.Entities.Models.Partners;
using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Services.Partners;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Caramel.Pattern.Services.Api.Controllers.v1
{
    [ApiController]
    [Route("v1/[controller]")]
    public class PartnerController : BaseController
    {
        private readonly ILogger<PartnerController> _logger;
        private readonly IPartnerService _service;
        private readonly IMapper _mapper;

        public PartnerController(
            ILogger<PartnerController> logger,
            IPartnerService service,
            IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Recupera uma lista de todos os Parceiros.
        /// </summary>
        /// <param name="page">Página de dados a serem trazidos. Default: Page = 1.</param>
        /// <param name="size">Tamanho da página de dados a serem trazidos. Default: Size = 10.</param>
        /// <returns>Lista de Parceiros, Status do Processo e Descrição</returns>
        [Authorize]
        [HttpGet("/users-control/partners")]
        [ProducesResponseType(typeof(CustomResponse<IEnumerable<Partner>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPartnersAsync(int page, int size)
        {
            Pagination pagination = new(page, size);

            var partners = await _service.GetAllAsync();

            var paginatedPartners = ReturnPaginated(partners, pagination);

            var response = new CustomResponse<IEnumerable<Partner>>(paginatedPartners, StatusProcess.Success);

            return Ok(response);
        }

        /// <summary>
        /// Recupera um parceiro específico por ID.
        /// </summary>
        /// <param name="id">O ID do parceiro a ser recuperado.</param>
        /// <returns>Parceiro, Status do Processo e Descrição</returns>
        [Authorize]
        [HttpGet("/users-control/partner")]
        [ProducesResponseType(typeof(CustomResponse<Partner>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPartnerAsync(string id)
        {
            var partner = await _service.GetSingleOrDefaultByIdAsync(id);

            var response = new CustomResponse<Partner>(partner, StatusProcess.Success);

            return Ok(response);
        }

        /// <summary>
        /// Recupera uma lista de partners filtrada por critérios específicos.
        /// </summary>
        /// <param name="request">Página e Total de dados a serem trazidos e Filtro a ser realizado. Default: Page = 1 e Size = 10</param>
        /// <returns>Lista de Partners Filtrados, Status do Processo e Descrição.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("/users-control/partner/filtered")]
        [ProducesResponseType(typeof(CustomResponse<Partner>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPartnerFiltered(GetPartnerFilteredRequest request)
        {
            var partner = await _service.GetByFilterAsync(request.PartnerFilter);

            var paginetedPartner = ReturnPaginated<Partner>(partner, request.Pagination);

            var response = new CustomResponse<IEnumerable<Partner>>(paginetedPartner, StatusProcess.Success);

            return Ok(response);
        }

        /// <summary>
        /// Recupera um parceiro específico por email.
        /// </summary>
        /// <param name="email">O ID do parceiro a ser recuperado.</param>
        /// <returns>Parceiro, Status do Processo e Descrição</returns>
        [Authorize]
        [HttpGet("/users-control/partner/email")]
        [ProducesResponseType(typeof(CustomResponse<Partner>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPartnerByEmailAsync(string email)
        {
            var partner = await _service.GetSingleOrDefaultByEmailAsync(email);

            var response = new CustomResponse<Partner>(partner, StatusProcess.Success);

            return Ok(response);
        }

        /// <summary>
        /// Cria um novo parceiro.
        /// </summary>
        /// <param name="request">Dados do novo Parceiro.</param>
        /// <returns>Parceiro Criado, Status do Processo e Descrição</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("/users-control/partners")]
        [ProducesResponseType(typeof(CustomResponse<Partner>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterPartnerAsync([FromBody] PartnerRegistrationRequest request)
        {
            var partner = _mapper.Map<Partner>(request);

            var addedPartner = await _service.RegisterAsync(partner);

            var response = new CustomResponse<Partner>(addedPartner, StatusProcess.Success);

            var uri = Url.Action("GetPartner", "PartnerController", new { id = addedPartner.Id });

            return Created(uri, response);
        }

        /// <summary>
        /// Atualiza um parceiro existente.
        /// </summary>
        /// <param name="id">O ID do parceiro a ser Atualizado.</param>
        /// <param name="request">Dados Atualizados do Parceiro.</param>
        /// <returns>Parceiro Atualizado, Status do Processo e Descrição</returns>
        [Authorize]
        [HttpPut("/users-control/partners")]
        [ProducesResponseType(typeof(CustomResponse<Partner>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePartnerAsync(string id, PartnerUpdateRequest request)
        {
            var partner = _mapper.Map<Partner>(request);
            partner.Id = id;

            var updatedPartner = await _service.UpdateAsync(partner, request.Base64Image);

            var response = new CustomResponse<Partner>(updatedPartner, StatusProcess.Success);

            return Ok(response);
        }

        /// <summary>
        /// Atualiza a senha de um parceiro existente.
        /// </summary>
        /// <param name="id">O ID do parceiro a ser Atualizado.</param>
        /// <param name="request">Dados Atualizados do Parceiro.</param>
        /// <returns>Parceiro Atualizado, Status do Processo e Descrição</returns>
        [Authorize]
        [HttpPut("/users-control/partners/password")]
        [ProducesResponseType(typeof(CustomResponse<Partner>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePartnerPassword(string id, PartnerUpdatePasswordRequest request)
        {
            var partner = _mapper.Map<Partner>(request);
            partner.Id = id;

            var updatedPartner = _service.UpdatePassword(partner);

            var response = new CustomResponse<Partner>(updatedPartner, StatusProcess.Success);

            return Ok(response);
        }

        /// <summary>
        /// Exclui um parceiro por ID.
        /// </summary>
        /// <param name="partnerId">O ID do parceiro a ser excluído.</param>
        /// <returns>Parceiro Deletado, Status do Processo e Descrição</returns>
        [Authorize]
        [HttpDelete("/users-control/partners")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePartnerAsync(string partnerId)
        {
            await _service.DeleteAsync(partnerId);

            return NoContent();
        }
    }
}
