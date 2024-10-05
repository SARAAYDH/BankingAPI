using Banking.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking.Data.BankingDbContext;

public class SearchParameterContext : DbContext
{
    public DbSet<SearchParameter> SearchParameters { get; set; }

    public SearchParameterContext(DbContextOptions<SearchParameterContext> options) : base(options)
    {
    }
}
