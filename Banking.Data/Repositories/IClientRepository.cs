using Banking.Data.Models;

namespace Banking.Data.Repositories;

public interface IClientRepository 
{
    Task<Client?> FindByEmailOrPersonalIdAsync(string email, string personalId, CancellationToken cancellationToken = default);
    Task<Client?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Client>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Client client, CancellationToken cancellationToken = default);
    Task UpdateAsync(Client client, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IQueryable<Client>> GetAllAsQueryable(CancellationToken cancellationToken = default);
}
