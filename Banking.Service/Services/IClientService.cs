using Banking.Data.Models;
using Banking.Service.Dtos;
using Banking.Service.Helpers;

namespace Banking.Service.Services;

public interface IClientService
{
    Task<GenericResult<ClientDto>> GetClientByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<GenericResult<IEnumerable<ClientDto>>> GetAllClientAsync(CancellationToken cancellationToken = default);
    Task<Result> AddClientAsync(CreateClientDto client, CancellationToken cancellationToken = default);
    Task<Result> UpdateClientAsync(int id, UpdateClientDto client, CancellationToken cancellationToken = default);
    Task<Result> DeleteClientAsync(int id, CancellationToken cancellationToken = default);
    Task<GenericResult<PagedResult<ClientDto>>> GetFilteredClientsAsync(ClientQueryDto queryDto, CancellationToken cancellationToken = default);
    Task<GenericResult<IQueryable<Client>>> ApplyFiltering(IQueryable<Client> query, string searchTerm, string filterBy);
    Task<GenericResult<IQueryable<Client>>> ApplySorting(IQueryable<Client> query, string sortBy, string sortDirection);
    Task<IQueryable<Client>> ApplyPaging(IQueryable<Client> query, int page, int pageSize);
}
