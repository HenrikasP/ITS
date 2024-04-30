using Claims.Infrastructure.Persistence.Entities;

namespace Claims.Infrastructure.Persistence.Repositories;

public interface IClaimsRepository : IRepository<ClaimEntity>
{
}