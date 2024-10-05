using Banking.Data.BankingDbContext;
using Banking.Data.Models;

namespace Banking.Data.Repositories;

public class AccountRepository : GenericRepository<Account>, IAccountRepository
{
    public AccountRepository(BankingContext context) : base(context)
    {
    }
}
