namespace Caramel.Pattern.Services.Domain.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        IPartnerRepository Partners { get; }
        IAdopterRepository Adopters { get; }
    }
}
