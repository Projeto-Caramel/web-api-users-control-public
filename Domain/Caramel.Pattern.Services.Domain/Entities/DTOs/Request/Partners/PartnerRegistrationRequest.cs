using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Enums.Parterns;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.DTOs.Request.Partners
{
    [ExcludeFromCodeCoverage]
    public class PartnerRegistrationRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Cellphone { get; set; }
        public OrganizationType Type { get; set; }
        public Roles Role { get; set; }
        public int MaxCapacity { get; set; }
    }
}
