using Banking.Data.BankingDbContext;
using Banking.Data.Models;

namespace Banking.Data.Repositories;

public class AddressRepository : GenericRepository<Address>, IAddressRepository
{
    public AddressRepository(BankingContext context) : base(context)
    {
    }
}
