using Caramel.Pattern.Services.Domain.Enums;

namespace Caramel.Pattern.Services.Domain.Entities.DTOs.Request.Partners
{
    public class PartnerUpdateRequest
    {
        public string Email { get; set; }
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
        public string Base64Image { get; set; }
        public int MaxCapacity { get; set; }
    }
}
