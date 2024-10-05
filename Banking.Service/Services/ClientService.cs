using AutoMapper;
using Banking.Data.Models;
using Banking.Data.Repositories;
using Banking.Service.Dtos;
using Banking.Service.Helpers;
using Banking.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Banking.Service;

public class ClientService: IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly ISearchParameterService _searchParameterService;
    private readonly IMapper _mapper;
    public ClientService(IClientRepository clientRepository, IMapper mapper, ISearchParameterService searchParameterService)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
        _searchParameterService = searchParameterService;
    }

    public async Task<Result> AddClientAsync(CreateClientDto client, CancellationToken cancellationToken = default)
    {
        var isExistingClient = await ValidateExistingClientAsync(client, cancellationToken);
        if(!isExistingClient.IsSuccess) 
        { 
            return isExistingClient;
        }

        var isValidMobileNumber = ValidatePhoneNumber(client.MobileNumber);
        if (!isValidMobileNumber.IsSuccess)
        {
            return isValidMobileNumber;
        }
        var newClient = _mapper.Map<Client>(client);
        //////
        //// Ensure at least one account exists
        //if (newClient.Accounts == null || !newClient.Accounts.Any())
        //{
        //    // Create a default account if none exist
        //    var defaultAccount = new Account
        //    {
        //        Balance = 0, // Initial balance can be 0 or a default value
        //        Client = newClient
        //    };

        //    newClient.Accounts = new List<Account> { defaultAccount };  // Initialize accounts collection and add account
        //}
        HandleClientAccounts(client, newClient);
        await _clientRepository.AddAsync(newClient, cancellationToken);
        return  Result.Success();
    }

    public async Task<Result> ValidateExistingClientAsync(CreateClientDto client, CancellationToken cancellationToken = default)
    {
        var existingClient = await _clientRepository.FindByEmailOrPersonalIdAsync(client.Email, client.PersonalId, cancellationToken);

        if (existingClient != null)
        {
            if (existingClient.Email == client.Email)
            {
                return Result.Failure("A client with this email already exists.");
            }

            if (existingClient.PersonalId == client.PersonalId)
            {
                return Result.Failure("A client with this Personal ID already exists.");
            }
        }
        return Result.Success();
    }
    private Result ValidatePhoneNumber(string mobileNumber)
    {
        var phoneNumberValidator = new PhoneNumberValidator();
        if (!phoneNumberValidator.IsValidInternationalPhoneNumber(mobileNumber))
        {
            return Result.Failure("Invalid mobile number format. Ensure it includes the correct country code.");
        }
        return Result.Success();
    }
    private void HandleClientAccounts(CreateClientDto clientDto, Client newClient)
    {
        // Check if the client DTO has any account information
        if (clientDto.Accounts == null || !clientDto.Accounts.Any())
        {
            // Create a default account if no accounts are provided
            var defaultAccount = new Account
            {
                Balance = 0, // Initial balance can be set to 0 or any default value
                Client = newClient // Link the account to the new client
            };
            newClient.Accounts.Add(defaultAccount); // Add the default account to the new client
        }
        else
        {
            // Map each AccountDto to the Account entity if accounts exist in DTO
            foreach (var accountDto in clientDto.Accounts)
            {
                var account = new Account
                {
                    Balance = accountDto.Balance,
                    Client = newClient // Link the account to the new client
                };
                newClient.Accounts.Add(account); // Add the account to the new client
            }
        }
    }

    public async Task<Result> DeleteClientAsync(int id, CancellationToken cancellationToken = default)
    {
        var isExist = await _clientRepository.GetByIdAsync(id, cancellationToken);
        if (isExist == null)
        {
            return Result.Failure($"client does not exist with this Id {id}");
        }
        await _clientRepository.DeleteAsync(id, cancellationToken);
        return Result.Success();
    }

    public async Task<GenericResult<IEnumerable<ClientDto>>> GetAllClientAsync(CancellationToken cancellationToken = default)
    {
        var allClients = await _clientRepository.GetAllAsync(cancellationToken);
        if (allClients.IsNullOrEmpty() || !allClients.Any())
        {
            return GenericResult<IEnumerable<ClientDto>>.Failure("No clients found.");
        }
        var clientsDto = _mapper.Map<List<ClientDto>>(allClients);
        return GenericResult<IEnumerable<ClientDto>>.Success(clientsDto);
    }

    public async Task<GenericResult<ClientDto>> GetClientByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByIdAsync(id, cancellationToken);
        if(client == null)
        {
            return GenericResult<ClientDto>.Failure($"client does not exist with this Id {id}");
        }
        var clientDto = _mapper.Map<ClientDto>(client);
        return GenericResult<ClientDto>.Success(clientDto);

    }

    public async Task<Result> UpdateClientAsync(int id, UpdateClientDto updatedClient, CancellationToken cancellationToken = default)
    {
        var existingClient = await _clientRepository.GetByIdAsync(id, cancellationToken);
        if (existingClient == null)
        {
            return Result.Failure($"client does not exist with this Id {id}");
        }
        var phoneNumberValidator = new PhoneNumberValidator();
        if (!phoneNumberValidator.IsValidInternationalPhoneNumber(updatedClient.MobileNumber))
        {
            return Result.Failure("Invalid mobile number format. Ensure it includes the correct country code.");
        }
        existingClient.FirstName = updatedClient.FirstName ?? existingClient.FirstName;
        existingClient.LastName = updatedClient.LastName ?? existingClient.LastName;
        existingClient.MobileNumber = updatedClient.MobileNumber ?? existingClient.MobileNumber;
        existingClient.ProfilePhoto = updatedClient.ProfilePhoto ?? existingClient.ProfilePhoto;
        await _clientRepository.UpdateAsync(existingClient, cancellationToken);
        return Result.Success();
    }
    public async Task<GenericResult<PagedResult<ClientDto>>> GetFilteredClientsAsync(ClientQueryDto queryDto, CancellationToken cancellationToken = default)
    {
        var clientsQuery = await _clientRepository.GetAllAsQueryable();
        await _searchParameterService.AddSearchParameter(_mapper.Map<ClientQueryDto>(queryDto));
        // Apply filtering
        var filteredQueryResult = await ApplyFiltering(clientsQuery, queryDto.SearchTerm, queryDto.FilterBy);
        // Apply sorting
        var sortedQueryResult = await ApplySorting(filteredQueryResult.Data, queryDto.SortBy, queryDto.SortDirection);
        // Paging
        var totalClients = await sortedQueryResult.Data.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalClients / queryDto.PageSize);
        if (totalClients == 0)
        {
            return GenericResult<PagedResult<ClientDto>>.Failure("No clients found.");
        }
        if (queryDto.Page > totalPages)
        {
            return GenericResult<PagedResult<ClientDto>>.Failure("Requested page exceeds the available data.");
        }
        var clientsPerPage = await ApplyPaging(sortedQueryResult.Data, queryDto.Page, queryDto.PageSize);
        // Get the final list of clients
        var clientList = await clientsPerPage.ToListAsync(cancellationToken);
        // Map to ClientDto
        var clientDtos = _mapper.Map<List<ClientDto>>(clientList);

        // Return result with paging information
        var pagedResult = new PagedResult<ClientDto>
        {
            Items = clientDtos,
            TotalCount = totalClients,
            Page = queryDto.Page,
            PageSize = queryDto.PageSize
        };

        return GenericResult<PagedResult<ClientDto>>.Success(pagedResult);
    }

    public async Task<GenericResult<IQueryable<Client>>> ApplyFiltering(IQueryable<Client> query, string searchTerm, string filterBy)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return GenericResult<IQueryable<Client>>.Success(query); 
        }
        string lowerFilterBy = string.IsNullOrEmpty(filterBy) ? "all" : filterBy.ToLower();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            if (lowerFilterBy == "firstname")
            {
                query = query.Where(c => c.FirstName.ToLower().Contains(searchTerm.ToLower()));
            }
            if (lowerFilterBy == "lastname")
            {
                query = query.Where(c => c.LastName.ToLower().Contains(searchTerm.ToLower()));
            }
            if (lowerFilterBy == "email")
            {
                query = query.Where(c => c.Email.ToLower().Contains(searchTerm.ToLower()));
            }
            if (lowerFilterBy == "mobilenumber")
            {
                query = query.Where(c => c.MobileNumber.ToLower().Contains(searchTerm.ToLower()));
            }
            if (lowerFilterBy == "all")
            {
                query = query.Where(c =>
                    c.FirstName.ToLower().Contains(searchTerm.ToLower()) ||
                    c.LastName.ToLower().Contains(searchTerm.ToLower()) ||
                    c.Email.ToLower().Contains(searchTerm.ToLower()));
            }
        }          
            return GenericResult<IQueryable<Client>>.Success(query);
    }
    public async Task<GenericResult<IQueryable<Client>>> ApplySorting(IQueryable<Client> query, string sortBy, string sortDirection)
    {
        var ascending = string.IsNullOrEmpty(sortDirection) || sortDirection.ToLower() == "asc";
        string lowerSortBy = string.IsNullOrEmpty(sortBy) ? "firstname" : sortBy.ToLower();
        switch (lowerSortBy)
        {
            case "firstname":
                query = ascending ? query.OrderBy(c => c.FirstName) : query.OrderByDescending(c => c.FirstName);
                break;

            case "lastname":
                query = ascending ? query.OrderBy(c => c.LastName) : query.OrderByDescending(c => c.LastName);
                break;

            case "email":
                query = ascending ? query.OrderBy(c => c.Email) : query.OrderByDescending(c => c.Email);
                break;
        }

        return GenericResult<IQueryable<Client>>.Success(query);
    }
    public async Task<IQueryable<Client>> ApplyPaging(IQueryable<Client> query, int page, int pageSize)
    {
        var clients =  query.Skip((page - 1) * pageSize).Take(pageSize);
        return clients;
    }
}
