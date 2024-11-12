using Caramel.Pattern.Services.Domain.Repositories;
using Caramel.Pattern.Services.Domain.Repositories.UnitOfWork;

namespace Caramel.Pattern.Services.Infra.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MongoDbContext _context;
        public IPartnerRepository Partners { get; }
        public IAdopterRepository Adopters { get; }

        public UnitOfWork(
            MongoDbContext context,
            IPartnerRepository partners,
            IAdopterRepository adopters
            )
        {
            _context = context;
            Partners = partners;
            Adopters = adopters;
        }
    }
}
