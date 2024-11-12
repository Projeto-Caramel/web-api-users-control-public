using Caramel.Pattern.Services.Domain.Enums.Adopters;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Caramel.Pattern.Services.Domain.Entities.Models.Adopters
{
    public class Adopter : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateOnly Birthday { get; set; }
        public ResidencyType ResidencyType { get; set; }
        public Lifestyle Lifestyle { get; set; }
        public PetExperience PetExperience { get; set; }
        public HasChildren HasChildren { get; set; }
        public FinancialSituation FinancialSituation { get; set; }
        public FreeTime FreeTime { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
