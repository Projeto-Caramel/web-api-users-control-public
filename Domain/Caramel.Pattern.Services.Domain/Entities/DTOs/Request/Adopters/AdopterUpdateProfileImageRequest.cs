using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.Entities.DTOs.Request.Adopters
{
    [ExcludeFromCodeCoverage]
    public class AdopterUpdateProfileImageRequest
    {        
        public string Base64Image { get; set; }
    }
}
