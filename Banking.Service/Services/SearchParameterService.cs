using AutoMapper;
using Banking.Data.BankingDbContext;
using Banking.Data.Models;
using Banking.Service.Dtos;
using Banking.Service.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Service.Services;

public class SearchParameterService : ISearchParameterService
{
    private readonly SearchParameterContext _context;
    private readonly IMapper _mapper;
    public SearchParameterService(SearchParameterContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task AddSearchParameter(ClientQueryDto parameter, CancellationToken cancellationToken)
    {
        var newParameter = _mapper.Map<SearchParameter>(parameter);
        await _context.SearchParameters.AddAsync(newParameter);
        await _context.SaveChangesAsync();

        // If more than 3, remove the oldest one
        var count = await _context.SearchParameters.CountAsync();
        if (count > 3)
        {
            var oldestParameter = await _context.SearchParameters.OrderBy(p => p.Id).FirstAsync(cancellationToken);
            _context.SearchParameters.Remove(oldestParameter);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<GenericResult<IEnumerable<ClientQueryDto>>> GetLastThreeParameters(CancellationToken cancellationToken)
    {
        var searchedParameters = await _context.SearchParameters.OrderByDescending(p => p.Id).Take(3).ToListAsync(cancellationToken);
        if(searchedParameters == null || searchedParameters.Count == 0) 
        {
            return GenericResult<IEnumerable<ClientQueryDto>>.Failure("No search parameters found.");
        }
        // Map the search parameters to ClientQueryDto
        var parametersDto = _mapper.Map<List<ClientQueryDto>>(searchedParameters);

        // Return the success result with the parameters
        return GenericResult<IEnumerable<ClientQueryDto>>.Success(parametersDto);
    }
}
