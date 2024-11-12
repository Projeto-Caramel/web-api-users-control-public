using Caramel.Pattern.Services.Application.Services.Base;
using Caramel.Pattern.Services.Domain.Entities.Models.Adopters;
using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Repositories.UnitOfWork;
using Caramel.Pattern.Services.Domain.Services.Adopters;
using Caramel.Pattern.Services.Domain.Services.Bucket;
using Caramel.Pattern.Services.Domain.Validators;
using MongoDB.Bson.Serialization.Conventions;
using System.Net;

namespace Caramel.Pattern.Services.Application.Services.Adopters
{
    public class AdopterService : BaseService, IAdopterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBucketService _bucketService;

        public AdopterService(IUnitOfWork unitOfWork, IBucketService bucketService)
        {
            _unitOfWork = unitOfWork;
            _bucketService = bucketService;
        }

        public async Task<Adopter> GetSingleOrDefaultByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new BusinessException("O Id é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var adopter = await _unitOfWork.Adopters.GetSingleAsync(x => x.Id == id);

            return adopter;
        }

        public async Task<Adopter> GetSingleOrDefaultByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new BusinessException("O email é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var adopters = await _unitOfWork.Adopters.GetSingleAsync(x => x.Email == email);

            return adopters;
        }

        public async Task<IEnumerable<Adopter>> GetAllAsync()
        {
            var adopters = await _unitOfWork.Adopters.FetchAsync();

            return adopters;
        }

        public async Task<Adopter> RegisterAsync(Adopter entity)
        {
            BusinessException.ThrowIfNull(entity, "Usuário");

            ValidateEntity<AdopterValidator, Adopter>(entity);

            var adopter = await GetSingleOrDefaultByEmailAsync(entity.Email);

            if (adopter != null)
                throw new BusinessException("Usuário já existe.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableContent);

            await _unitOfWork.Adopters.AddAsync(entity);

            return entity;
        }

        public async Task<Adopter> UpdateAsync(Adopter entity)
        {
            BusinessException.ThrowIfNull(entity, "Usuário");

            if (string.IsNullOrEmpty(entity.Id))
                throw new BusinessException("O Id é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var adopter = await GetSingleOrDefaultByIdAsync(entity.Id);

            if (adopter == null)
                throw new BusinessException("Usuário não encontrado na nossa base de dados.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            entity.Id = adopter.Id;
            entity.Email = adopter.Email;
            entity.Password = adopter.Password;
            entity.ProfileImageUrl = adopter.ProfileImageUrl;

            _unitOfWork.Adopters.Update(entity);

            return entity;
        }

        public async Task<Adopter> UpdatePasswordAsync(Adopter entity)
        {
            BusinessException.ThrowIfNull(entity, "Usuário");

            if (string.IsNullOrEmpty(entity.Id))
                throw new BusinessException("O Id é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var adopter = await GetSingleOrDefaultByIdAsync(entity.Id) ??
                throw new BusinessException("Usuário não encontrado na nossa base de dados.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity); 

            adopter.Password = entity.Password;
            adopter.Email = entity.Email;

            _unitOfWork.Adopters.Update(adopter);

            return adopter;
        }

        public async Task DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new BusinessException("O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var entity = await GetSingleOrDefaultByIdAsync(id);

            if (entity == null)
                throw new BusinessException("Não foi possível encontrar nenhum Usuário com essas informações.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            _unitOfWork.Adopters.Delete(entity);
        }

        public async Task<Adopter> ProfileImageUpdateAsync(string id, string base64Image)
        {
            if (string.IsNullOrEmpty(id))
                throw new BusinessException("O campo ID é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            if (string.IsNullOrEmpty(base64Image))
                throw new BusinessException("A Imagem é obrigatória", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var entity = await GetSingleOrDefaultByIdAsync(id) ??
                throw new BusinessException("Usuário não encontrado", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var key = $"adopters/{id}/profileImage.jpg";
            var imageUrl = await _bucketService.UploadFileAsync(base64Image, key);

            entity.ProfileImageUrl = imageUrl;

            _unitOfWork.Adopters.Update(entity);

            return entity;
        }
    }
}
