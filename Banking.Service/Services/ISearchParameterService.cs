using Banking.Service.Dtos;
using Banking.Service.Helpers;
namespace Banking.Service.Services;

public interface ISearchParameterService
{
    Task AddSearchParameter(ClientQueryDto parameter, CancellationToken cancellationToken = default);
    Task<GenericResult<IEnumerable<ClientQueryDto>>> GetLastThreeParameters(CancellationToken cancellationToken = default);
}
