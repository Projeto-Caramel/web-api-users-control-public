using Caramel.Pattern.Services.Domain.Entities.Models.Adopters;
using Caramel.Pattern.Services.Domain.Repositories;
using Caramel.Pattern.Services.Infra.Context;

namespace Caramel.Pattern.Services.Infra.Repositories
{
    public class AdopterRepository(MongoDbContext context) : BaseRepository<Adopter, string>(context, "adopters-data"), IAdopterRepository
    {
    }
}
