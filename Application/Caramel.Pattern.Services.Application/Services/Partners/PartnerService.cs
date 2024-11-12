using Caramel.Pattern.Services.Application.Services.Base;
using Caramel.Pattern.Services.Domain.Entities.Models.Partners;
using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Repositories.UnitOfWork;
using Caramel.Pattern.Services.Domain.Services.Bucket;
using Caramel.Pattern.Services.Domain.Services.Partners;
using Caramel.Pattern.Services.Domain.Validators;
using System.Net;

namespace Caramel.Pattern.Services.Application.Services.Partners
{
    public class PartnerService : BaseService, IPartnerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBucketService _bucketService;

        public PartnerService(IUnitOfWork unitOfWork, IBucketService bucketService)
        {
            _unitOfWork = unitOfWork;
            _bucketService = bucketService;
        }

        public async Task<Partner> GetSingleOrDefaultByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new BusinessException("O Id é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var partner = await _unitOfWork.Partners.GetSingleAsync(x => x.Id == id);

            return partner;
        }

        public async Task<Partner> GetSingleOrDefaultByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new BusinessException("O email é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var partner = await _unitOfWork.Partners.GetSingleAsync(x => x.Email == email);

            return partner;
        }

        public async Task<IEnumerable<Partner>> GetAllAsync()
        {
            var partners = await _unitOfWork.Partners.FetchAsync();

            return partners;
        }

        public async Task<IEnumerable<Partner>> GetByFilterAsync(PartnerFilter filter)
        {
            BusinessException.ThrowIfNull(filter, "Partner Filter");

            var partners = await _unitOfWork.Partners.FetchAsync();
            var partnersFiltered = FilterPartners(partners, filter);

            return partnersFiltered;
        }

        public async Task<Partner> RegisterAsync(Partner entity)
        {
            BusinessException.ThrowIfNull(entity, "Parceiro");

            ValidateEntity<PartnerValidator, Partner>(entity);

            var partner = await GetSingleOrDefaultByEmailAsync(entity.Email);

            if (partner != null)
                throw new BusinessException("Parceiro já existe.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableContent);

            await _unitOfWork.Partners.AddAsync(entity);

            return entity;
        }

        public async Task<Partner> UpdateAsync(Partner entity, string base64Image)
        {
            BusinessException.ThrowIfNull(entity, "Parceiro");

            if (string.IsNullOrEmpty(entity.Id))
                throw new BusinessException("O Id é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);            

            var partner = await GetSingleOrDefaultByIdAsync(entity.Id) ?? 
                throw new BusinessException("Parceiro não encontrado na nossa base de dados.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var imageUrl = await _bucketService.UploadFileAsync(base64Image, $"partners/{entity.Id}/profileImage.jpg");
            
            entity.Id = partner.Id;
            entity.Role = partner.Role;
            entity.Type = partner.Type;
            entity.Password = partner.Password;
            entity.ProfileImageUrl = imageUrl;

            _unitOfWork.Partners.Update(entity);

            return entity;
        }

        public Partner UpdatePassword(Partner entity)
        {
            BusinessException.ThrowIfNull(entity, "Parceiro");

            if (string.IsNullOrEmpty(entity.Id))
                throw new BusinessException("O Id é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);            

            _unitOfWork.Partners.Update(entity);

            return entity;
        }

        public async Task DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new BusinessException("O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var entity = await GetSingleOrDefaultByIdAsync(id);

            if (entity == null)
                throw new BusinessException("Não foi possível encontrar nenhum Parceiro com essas informações.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            _unitOfWork.Partners.Delete(entity);
        }

        public async Task<string> GetImageBase64(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new BusinessException("O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var key = $"partners/{id}/profileImage.jpg";

            if (!await _bucketService.ImageExistsAsync(key))
                return string.Empty;

            return await _bucketService.GetImageAsBase64Async(key);
        }

        private IEnumerable<Partner> FilterPartners(IEnumerable<Partner> partners, PartnerFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Name))
                partners = partners.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));
            if (filter.Type != 0)
                partners = partners.Where(x => x.Type == filter.Type);

            return partners;
        }
    }
}
