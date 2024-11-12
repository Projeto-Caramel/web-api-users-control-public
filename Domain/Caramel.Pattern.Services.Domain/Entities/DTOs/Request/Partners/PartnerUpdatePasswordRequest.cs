using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Enums.Parterns;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.DTOs.Request.Partners
{
    [ExcludeFromCodeCoverage]
    public class PartnerUpdatePasswordRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Cellphone { get; set; }
        public string CNPJ { get; set; }
        public string AdoptionRate { get; set; }
        public string PIX { get; set; }
        public string Website { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public OrganizationType Type { get; set; }
        public Roles Role { get; set; }
        public string ProfileImageUrl { get; set; }
        public int MaxCapacity { get; set; }
    }
}
