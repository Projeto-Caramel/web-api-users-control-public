using Caramel.Pattern.Services.Domain.Entities.Models.Adopters;

namespace Caramel.Pattern.Services.Domain.Services.Adopters
{
    public interface IAdopterService
    {
        Task<Adopter> GetSingleOrDefaultByIdAsync(string id);
        Task<Adopter> GetSingleOrDefaultByEmailAsync(string email);
        Task<IEnumerable<Adopter>> GetAllAsync();
        Task<Adopter> RegisterAsync(Adopter entity);
        Task<Adopter> UpdateAsync(Adopter entity);
        Task<Adopter> UpdatePasswordAsync(Adopter entity);
        Task DeleteAsync(string id);
        Task<Adopter> ProfileImageUpdateAsync(string id, string base64Image);
    }
}
