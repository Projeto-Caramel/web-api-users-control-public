using Caramel.Pattern.Services.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.Auth
{
    [ExcludeFromCodeCoverage]
    public class Auth
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public Roles Role { get; set; }
        public TokenModel Token { get; set; }
    }
}
