using Production.Models;

namespace Production.DAL.Repositories;

public interface ICreditRepository
{
    public Task<List<Credit>> GetCredits();
    public Task CreateCredit(Credit credit);
}