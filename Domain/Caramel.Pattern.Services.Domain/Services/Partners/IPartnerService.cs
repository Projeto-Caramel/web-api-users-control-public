using Caramel.Pattern.Services.Domain.Entities.Models.Partners;

namespace Caramel.Pattern.Services.Domain.Services.Partners
{
    public interface IPartnerService
    {
        Task<Partner> GetSingleOrDefaultByIdAsync(string id);
        Task<Partner> GetSingleOrDefaultByEmailAsync(string email);
        Task<IEnumerable<Partner>> GetAllAsync();
        Task<IEnumerable<Partner>> GetByFilterAsync(PartnerFilter filter);
        Task<Partner> RegisterAsync(Partner entity);
        Task<Partner> UpdateAsync(Partner entity, string base64Image);
        Partner UpdatePassword(Partner entity);
        Task DeleteAsync(string id);
    }
}
