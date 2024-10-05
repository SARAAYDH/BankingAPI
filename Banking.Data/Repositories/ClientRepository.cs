using Banking.Data.BankingDbContext;
using Banking.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking.Data.Repositories;

public class ClientRepository : IClientRepository
{
    protected readonly BankingContext _context;
    public ClientRepository(BankingContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Client client, CancellationToken cancellationToken = default)
    {
        if(client !=null) 
        { 
             _context.Clients.Add(client);
             await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
      var client = await _context.Clients.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
      if(client != null) 
        { 
            client.IsDeleted= true;
            _context.Clients.Update(client);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<Client?> FindByEmailOrPersonalIdAsync(string email, string personalId, CancellationToken cancellationToken = default)
    {
       return await _context.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Email == email || c.PersonalId == personalId, cancellationToken);
    }

    public async Task<IEnumerable<Client>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Clients.AsNoTracking().Where(c => !c.IsDeleted.HasValue || !c.IsDeleted.Value).ToListAsync(cancellationToken);
    }

    public async Task<Client?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Clients.AsNoTracking().Where(c => !c.IsDeleted.HasValue || !c.IsDeleted.Value).FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IQueryable<Client>> GetAllAsQueryable(CancellationToken cancellationToken)
    {
        return _context.Clients.AsQueryable();
    }

    public async Task UpdateAsync(Client client, CancellationToken cancellationToken = default)
    {
        if(client != null)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync(cancellationToken);
        }        
    }
}
